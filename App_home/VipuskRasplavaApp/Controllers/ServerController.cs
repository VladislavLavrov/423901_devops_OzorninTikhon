using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaspredeleniyeDutyaApp.Data;
using RaspredeleniyeDutyaApp.Helpers;
using RaspredeleniyeDutyaApp.Models.Server;
using System.Security.Claims;

namespace RaspredeleniyeDutyaApp.Controllers
{
    /// <summary>
    /// Контроллер для вычислений и работы с базой данных
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ServerController : Controller
    {
        private readonly ILogger<ServerController> _logger;
        private readonly CalculationHelper _calculationHelper;
        private readonly DatabaseContext _context;

        public ServerController(ILogger<ServerController> logger, DatabaseContext context)
        {
            _logger = logger;
            _calculationHelper = new(this);
            _context = context;
        }

        private void LogUserAction(string message)
        {
            _logger.Log(LogLevel.Information, "Пользователь под номером {UserId} ({FullName}): {Message}", GetUserId(), GetUserFullName(), message);
        }

        /// <summary>
        /// Получение списка входных данных со значениями по-умолчанию.
        /// </summary>
        /// <remarks>
        /// Получение списка входных данных со значениями по-умолчанию.
        /// Также можно получить данные входные параметры у какого-либо конкретного варианта,
        /// указав параметр id.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <param name="id">Номер варианта</param>
        /// <returns></returns>
        [HttpGet("Parameters")]
        public IActionResult Parameters(int id)
        {
            LogUserAction("Получение входных данных варианта под номером " + id);
            Variant? variant = GetVariantIfAvailable(id);
            if (variant == null && id > 0)
            {
                return Forbid();
            }
            return _calculationHelper.Parameters(variant?.Data);
        }

        /// <summary>
        /// Расчёт варианта
        /// </summary>
        /// <remarks>
        /// Рассчитывает распределение дутья по фурмам по данным из варианта под номером id.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// Возвращает код 400 "Bad Request", если входные данные неверны.
        /// </remarks>
        /// <param name="id">Номер варианта</param>
        /// <returns></returns>
        [HttpGet("CalculateVariant")]
        public IActionResult CalculateVariant(int id)
        {
            Variant? variant = GetVariantIfAvailable(id);
            if (variant == null)
            {
                LogUserAction($"Расчёт варианта {id}, ошибка: вариант не существует или недоступен для пользователя");
                return Forbid();
            }

            var results = _calculationHelper.CalculateVariant(variant.Data);

            // Результаты, как и входные данные, неверные
            if (results == null)
            {
                LogUserAction($"Расчёт варианта {id}, ошибка: неверные входные данные");
                return BadRequest();
            }

            LogUserAction($"Расчёт варианта {id}, успешно");

            return Json(results);
        }

        /// <summary>
        /// Генерация файла Microsoft Excel для варианта
        /// </summary>
        /// <remarks>
        /// Генерирует файл Microsoft Excel для варианта под номером variantId.
        /// Возвращает строку Base64 с байтами готового файла.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <param name="variantId">Номер варианта, для которого нужно сгенерировать отчёт Excel</param>
        /// <returns></returns>
        [HttpGet("GenerateExcelReport")]
        public IActionResult GenerateExcelReport(int variantId)
        {
            Variant? variant = GetVariantIfAvailable(variantId);
            if (variant == null)
            {
                LogUserAction($"Создание Excel файла для варианта {variantId}, ошибка: вариант не существует или недоступен для пользователя");
                return Forbid();
            }

            ExcelReportHelper.Results results = new()
            {
                Variant = variant,
                PredvaritResults = _calculationHelper.CalculatePredvarit(variant.Data),
                DutyeResults = _calculationHelper.CalculateDutye(variant.Data),
            };

            LogUserAction($"Создание Excel файла для варианта {variantId}, успешно");
            return Json(new ExcelReportHelper(results).GetBase64());
        }

        /// <summary>
        /// Доступные пользователю варианты
        /// </summary>
        /// <remarks>
        /// Возвращает список вариантов из базы данных, к которым у пользователя есть доступ.
        /// Возвращает JSON со списком вариантов внутри поля "variants".
        /// </remarks>
        /// <returns></returns>
        [HttpGet("GetVariants")]
        public IActionResult GetVariants()
        {
            List<Variant> variantList = GetAvailableVariants(GetUserId());
            variantList.ForEach(v => v.Owner.Variants = []);

            LogUserAction($"Получение доступных вариантов (количество доступных вариантов: {variantList.Count})");

            return Json(new Dictionary<string, List<Variant>>() {
                { "variants", variantList }
            });
        }

        /// <summary>
        /// Получение данных о варианте
        /// </summary>
        /// <remarks>
        /// Возвращает данные о варианте под номером id из базы данных, а также информацию о владельце данного варианта.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <param name="id">Номер варианта</param>
        /// <returns></returns>
        [HttpGet("GetVariant")]
        public IActionResult GetVariant(int id)
        {
            Variant? variant = GetVariantIfAvailable(id);
            if (variant != null)
            {
                LogUserAction($"Получение информации о варианте {id}: успешно");
                variant.Owner.Variants = [];
                return Json(variant);
            }
            else
            {
                LogUserAction($"Получение информации о варианте {id}: ошибка: вариант не существует или недоступен для пользователя");
                return Forbid();
            }
        }

        /// <summary>
        /// Сохранение варианта
        /// </summary>
        /// <remarks>
        /// Сохраняет новый вариант в базу данных (или обновляет уже существующий).
        /// Возвращает JSON c номером варианта при успешной работе.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <param name="variant">Данные о варианта</param>
        /// <param name="id">Номер варианта, или 0, если нужно создать новый вариант в базе данных</param>
        /// <returns></returns>
        [HttpPost("SaveVariant")]
        public IActionResult SaveVariant([FromBody] Variant variant, int id)
        {
            try
            {
                if (id <= 0)
                {
                    int userId = GetUserId();
                    // Неавторизованные пользователи не могут создавать варианты
                    if (userId == 0)
                        return Forbid();

                    if (!_context.UserAccounts.First(u => u.Id == userId).IsAdmin)
                    {
                        variant.OwnerId = userId;
                    }
                    _context.Variants.Add(variant);
                }
                else
                {
                    Variant? variantFromDb = GetVariantIfAvailable(id);
                    if (variantFromDb == null)
                    {
                        return NotFound();
                    }
                    variantFromDb.Name = variant.Name;
                    variantFromDb.Data = variant.Data;
                }
                _context.SaveChanges();
                if (id <= 0)
                {
                    LogUserAction("Создание нового варианта в базе данных: ID варианта = " + variant.Id);
                }
                else
                {
                    LogUserAction("Изменение данных существующего в базе данных варианта: ID варианта = " + id);
                }
                return Json(id <= 0 ? variant.Id : id);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Удаление варианта
        /// </summary>
        /// <remarks>
        /// Удаляет информацию о варианте под номером id из базы данных.
        /// Возвращает JSON с полем "variantName", в котором хранится название удалённого варианта при успешном удалении.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <param name="id">Номер варианта</param>
        /// <returns></returns>
        [HttpGet("Delete")]
        public IActionResult Delete(int id)
        {
            Variant? variant = GetVariantIfAvailable(id);
            if (variant == null)
            {
                LogUserAction($"Удаление варианта {id}, ошибка: вариант не существует или недоступен для пользователя");
                return Forbid();
            }

            _context.Variants.Remove(variant);
            _context.SaveChanges();
            LogUserAction($"Удаление варианта {id}, успешно");
            return Json(new Dictionary<string, string?>() { { "variantName", variant.Name } });
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="data">Электронная почта и пароль</param>
        /// <remarks>
        /// JSON с полями "userId" (имя пользователя) и "name" (полное имя пользователя).
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDataModel data)
        {
            var user = _context.UserAccounts.ToList().FirstOrDefault(x => x.Email == data.Email && PasswordHasher.Verify(data.Password, x.PasswordHash));
            if (user != null)
            {
                var claims = new List<Claim> {
                    new("UserId", user.Id.ToString()),
                    new(ClaimTypes.Name, user.GetFullName()),
                };
                ClaimsIdentity claimsIdentity = new(claims, "Cookies");
                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
                LogUserAction("Успешный вход в аккаунт");
                return Json(new Dictionary<string, string>()
                {
                    {"userId", user.Id.ToString()},
                    {"name", user.GetFullName()},
                });
            }
            _logger.LogInformation("Неудачная попытка входа в аккаунт с электронной почтой \"{Email}\"", data.Email);
            return Forbid();
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns></returns>
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            LogUserAction("Успешный выход из аккаунта");
            await HttpContext.SignOutAsync();
            return Ok();
        }

        private const string REGISTER_FORBIDDEN_CHARACTERS = "!?@#$%^&*()_+=|/\\\"\'.,`~";

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        /// <param name="data">Данные аккаунта</param>
        /// <remarks>
        /// JSON с текстом ошибки при неудачной попытке создания аккаунта.
        /// Возвращает код 200 "OK" при успешном создании аккаунта.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDataModel data)
        {
            string? message = null;

            if (_context.UserAccounts.Any(x => x.Email == data.Email))
                message = "Пользователь с данной электронной почтой уже существует.";

            if (message is null && !IsEmailValid(data.Email))
                message = "Недопустимый адрес электронной почты.";

            if (message is null)
                message = CheckPassword(data.Password, data.ConfirmPassword);

            if (message is null && data.LastName.Any(REGISTER_FORBIDDEN_CHARACTERS.Contains))
                message = "Недопустимые знаки в фамилии. Список недопустимых знаков: " + REGISTER_FORBIDDEN_CHARACTERS;

            if (message is null && data.FirstName.Any(REGISTER_FORBIDDEN_CHARACTERS.Contains))
                message = "Недопустимые знаки в имени. Список недопустимых знаков: " + REGISTER_FORBIDDEN_CHARACTERS;

            if (message is null && !string.IsNullOrEmpty(data.MiddleName) && data.MiddleName.Any(REGISTER_FORBIDDEN_CHARACTERS.Contains))
                message = "Недопустимые знаки в отчестве. Список недопустимых знаков: " + REGISTER_FORBIDDEN_CHARACTERS;

            // Если есть ошибка, показываем её пользователю и записываем в логи
            if (message is not null)
            {
                _logger.LogInformation("Неудачная попытка создания аккаунта с электронной почтой \"{Email}\", ошибка: {Message}", data.Email, message);
                return Json(message);
            }

            UserAccount account = new()
            {
                Email = data.Email,
                LastName = data.LastName,
                FirstName = data.FirstName,
                MiddleName = data.MiddleName,
                IsAdmin = false,
                PasswordHash = PasswordHasher.Hash(data.Password)
            };
            _context.UserAccounts.Add(account);
            _context.SaveChanges();
            _logger.LogInformation("Создание нового аккаунта с электронной почтой \"{Email}\", успешно", data.Email);
            return Ok();
        }

        /// <summary>
        /// Получение информации об аккаунте пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <remarks>
        /// JSON с информацией о пользователе.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("GetUserInformation")]
        public IActionResult GetUserInformation(int id)
        {
            if (GetUserId() != id && !IsCurrentUserAdmin())
                return Forbid();

            UserAccount? user = _context.UserAccounts.Include(u => u.Variants).FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            // Нужно, чтобы избежать циклов сериализации JSON
            user.Variants.ForEach(v => v.Owner = null);

            return Json(user);
        }

        /// <summary>
        /// Сохранение информации об аккаунте пользователя
        /// </summary>
        /// <param name="userInfo">Информация об аккаунте пользователя вместе с его идентификатором</param>
        /// <remarks>
        /// Возвращает код 200 "OK" при успешном сохранении данных.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// Возвращает код 404 "Not Found", если запрашиваемого пользователя не существует.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("SaveUserInformation")]
        public IActionResult SaveUserInformation(UserSettingsDataModel userInfo)
        {
            if (GetUserId() != userInfo.Id && !IsCurrentUserAdmin())
                return Forbid();

            UserAccount? user = _context.UserAccounts.FirstOrDefault(u => u.Id == userInfo.Id);
            if (user == null)
                return NotFound();

            user.Email = userInfo.Email;
            user.LastName = userInfo.LastName;
            user.FirstName = userInfo.FirstName;
            user.MiddleName = userInfo.MiddleName ?? "";
            _context.SaveChanges();

            return Ok();
        }
        public record UserPasswordInfo(
            int Id,
            string Password,
            string ConfirmPassword);

        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        /// <param name="passInfo">Новый пароль пользователя с подтвердением и идентификатором</param>
        /// <remarks>
        /// Возвращает код 200 "OK" при успешном сохранении данных.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// Возвращает код 404 "Not Found", если запрашиваемого пользователя не существует.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("SaveUserPassword")]
        public IActionResult SaveUserPassword(UserPasswordInfo passInfo)
        {
            if (GetUserId() != passInfo.Id && !IsCurrentUserAdmin())
                return Forbid();

            UserAccount? user = _context.UserAccounts.FirstOrDefault(u => u.Id == passInfo.Id);
            if (user == null)
                return NotFound();

            string? message = CheckPassword(passInfo.Password, passInfo.ConfirmPassword);
            if (message is not null)
                return Json(message);

            user.PasswordHash = PasswordHasher.Hash(passInfo.Password);
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Получение статуса администратора данного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("IsUserAdmin")]
        public JsonResult IsUserAdmin()
            => Json(IsCurrentUserAdmin());

        /// <summary>
        /// Удаление аккаунта пользователя (для администраторов)
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <remarks>
        /// Возвращает код 200 "OK" при успешном удалении данных.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// Возвращает код 404 "Not Found", если запрашиваемого пользователя не существует.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            if (!IsCurrentUserAdmin())
                return Forbid();

            UserAccount? user = _context.UserAccounts.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            user.Variants.ForEach(v => _context.Variants.Remove(v));
            _context.UserAccounts.Remove(user);
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Назначение пользователя администратором сайта (для администраторов)
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="admin">"true", если пользователь должен стать администратором, в противном случае "false"</param>
        /// <remarks>
        /// Возвращает код 200 "OK" при успешном сохранении данных.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// Возвращает код 404 "Not Found", если запрашиваемого пользователя не существует.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("SetUserAdmin")]
        public IActionResult SetUserAdmin(int userId, bool admin)
        {
            if (!IsCurrentUserAdmin())
                return Forbid();

            UserAccount? user = _context.UserAccounts.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            user.IsAdmin = admin;
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Получение списка пользователей (для администраторов)
        /// </summary>
        /// <remarks>
        /// JSON со списком пользователей и их информацией.
        /// Возвращает код 403 "Forbidden" при отсутствии доступа.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("GetUserList")]
        public IActionResult GetUserList()
        {
            if (!IsCurrentUserAdmin())
                return Forbid();

            List<UserAccount> users = _context.UserAccounts.Include(u => u.Variants).ToList();
            // Нужно, чтобы избежать циклов сериализации JSON
            users.ForEach(u => u.Variants.ForEach(v => v.Owner = null));
            return Json(users);
        }

        public static string? CheckPassword(string password, string confirmPassword)
        {
            if (password is null && confirmPassword is null)
                return "Поля с паролями должны быть заполнены.";

            if (password != confirmPassword)
                return "Пароли не совпадают.";

            if (password.Any(c => c == ' '))
                return "Пароль не должен содержать пробелы.";

            if (!password.Any(c => char.IsDigit(c) || !char.IsLetter(c)))
                return "Пароль должен содержать цифры или другие символы, кроме букв.";

            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower))
                return "Пароль должен содержать прописные и строчные буквы.";

            if (password.Length < 8)
                return "Длина пароля должна быть 8 символов или больше.";

            return null;
        }

        public static bool IsEmailValid(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith('.'))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private int GetUserId() => int.Parse(User?.FindFirst("UserId")?.Value ?? "0");
        private string GetUserFullName() => User?.FindFirst(ClaimTypes.Name)?.Value ?? " (Гость) ";
        private bool IsCurrentUserAdmin() => _context.UserAccounts.FirstOrDefault(a => a.Id == GetUserId())?.IsAdmin ?? false;

        private List<Variant> GetAvailableVariants(int userId)
        {
            List<Variant> variantList = [];
            if (userId <= 0)
                return [];

            UserAccount? user = _context.UserAccounts.Where(a => a.Id == userId).Include(u => u.Variants).FirstOrDefault();
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    variantList = _context.Variants.Include(v => v.Owner).OrderBy(v => v.Id).ToList();
                }
                else
                {
                    variantList = user.Variants.OrderBy(v => v.Id).ToList();
                }
            }
            return variantList;
        }

        private Variant? GetVariantIfAvailable(int id)
            => GetAvailableVariants(GetUserId()).FirstOrDefault(v => v.Id == id);
    }
}
