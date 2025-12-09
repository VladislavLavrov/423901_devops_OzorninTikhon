using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RaspredeleniyeDutyaApp.Models;
using System.Text.Json;
using RaspredeleniyeDutyaApp.Helpers;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Web;
using System.Net;
using RaspredeleniyeDutyaApp.Models.Client;

namespace RaspredeleniyeDutyaApp.Controllers
{
    public class HomeController(IWebHostEnvironment env) : Controller
    {
        public static bool FilterVariant(VariantDataModel variant, VariantSearchDataModel variantInfo)
        {
            if (variantInfo.VariantId is not null && variant.Id != variantInfo.VariantId)
                return false;
            if (variantInfo.VariantName is not null && !variant.Name.Contains(variantInfo.VariantName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (variantInfo.UserId is not null && variant.Owner.Id != variantInfo.UserId)
                return false;
            if (variantInfo.UserLastName is not null && !variant.Owner.LastName.Contains(variantInfo.UserLastName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (variantInfo.UserFirstName is not null && !variant.Owner.FirstName.Contains(variantInfo.UserFirstName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (variantInfo.UserMiddleName is not null && !variant.Owner.MiddleName.Contains(variantInfo.UserMiddleName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (variantInfo.UserEmail is not null && !variant.Owner.Email.Contains(variantInfo.UserEmail, StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VariantSearchDataModel variantInfo)
        {
            List<VariantDataModel>? variants =
                (await RequestHelper.RequestGet<Dictionary<string, List<VariantDataModel>>>("/server/getVariants", Request.Cookies))?["variants"];
            variants ??= [];
            variants = variants.Where(v => FilterVariant(v, variantInfo)).ToList();
            return View(variants);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(VariantSearchDataModel variantInfo)
        {
            return RedirectToAction("Index", variantInfo);
        }

        private async Task<string?> GetVariantName(int id)
        {
            var variant = await RequestHelper.RequestGet<Dictionary<string, JsonElement>>("/server/getVariant?id=" + id, Request.Cookies);
            if (variant != null && variant.TryGetValue("name", out JsonElement value))
            {
                return value.ToString();
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Parameters(int id, int nRabFurm, int results)
        {
            ParametersViewModel model = new();
            model.VariantId = id;
            model.VariantName = await GetVariantName(id);
            if (id > 0 && model.VariantName == null)
            {
                return ForbiddenAccessError();
            }
            model.NRabFurm = nRabFurm;
            model.ShowResults = results == 1;
            if (TempData.ContainsKey("ErrorMessage"))
            {
                model.ErrorMessage = TempData["ErrorMessage"] as string;
                TempData.Remove("ErrorMessage");
            }
            return View(model);
        }

        public async Task<IActionResult> Results([FromForm] Dictionary<string, string> dict, int id)
        {
            DataViewModel model = new();
            JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };
            foreach (string key in dict.Keys)
            {
                if (key.StartsWith("variant"))
                {
                    continue;
                }

                var currentDict = JsonSerializer.Deserialize<Dictionary<string, DataViewModel.Parameter>>(dict[key], jsonOptions);
                if (currentDict is null)
                    continue;
                model.InitialData[key] = currentDict;
                foreach (var value in currentDict.Values)
                {
                    if (value?.Value is JsonElement json)
                    {
                        if (json.ValueKind == JsonValueKind.Number)
                        {
                            value.Value = json.GetDouble();
                        }
                        else if (json.ValueKind == JsonValueKind.Array)
                        {
                            value.Value = json.EnumerateArray().Select(
                                x => x.ValueKind == JsonValueKind.Number ? (object)x.GetDouble() : x.ValueKind == JsonValueKind.True
                            ).ToArray();
                        }
                    }
                }
            }

            DataViewModel.Results? results = await RequestHelper.RequestGet<DataViewModel.Results>(
                    "/server/calculateVariant?id=" + id,
                    Request.Cookies);

            if (results == null && RequestHelper.LastStatusCode == HttpStatusCode.BadRequest)
            {
                TempData["ErrorMessage"] = "Неверные входные данные. Перепроверьте правильность введённых данных.";
                return Redirect($"/home/parameters?id={id}");
            }

            if (results == null || !model.InitialData.TryGetValue("initialData", out var initialData) || initialData is null)
                return UnknownError();

            model.ResultsData = results;

            ViewData["VariantId"] = id;
            ViewData["VariantName"] = JsonSerializer.Deserialize<string>(dict["variantName"]);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string? variantName = (await RequestHelper.RequestGet<Dictionary<string, string?>>("/server/delete?id=" + id, Request.Cookies))?["variantName"];
            if (variantName != null)
                return RedirectToAction("Index");
            else
                return ForbiddenAccessError();
        }

        [HttpGet]
        public IActionResult Login(string message)
        {
            return View((object)message);
        }

        private async Task UserLogin(int id, string fullName)
        {
            var claims = new List<Claim> {
                        new("UserId", id.ToString()),
                        new(ClaimTypes.Name, fullName),
                    };
            ClaimsIdentity claimsIdentity = new(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
        }

        [HttpPost]
        public async Task<IActionResult> Login(IDictionary<string, string> data)
        {
            if (!data.Values.Any(string.IsNullOrEmpty))
            {
                var user = await RequestHelper.RequestPost<Dictionary<string, string>>("/server/login", data, Request.Cookies);
                if (user != null && user.TryGetValue("userId", out string? value))
                {
                    await UserLogin(int.Parse(value), user["name"]);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View((object)"loginFail");
        }

        [HttpGet]
        public IActionResult Register(string message)
        {
            return View((object)message);
        }

        public async Task<IActionResult> Logout()
        {
            await RequestHelper.RequestGetNoReturn("/server/logout", Request.Cookies);
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(Dictionary<string, string?> data)
        {
            string? message = null;
            if (data.Any(x => x.Key != "middleName" && string.IsNullOrEmpty(x.Value)))
                message = "Все поля должны быть заполнены.";
            data["middleName"] ??= "";
            message ??= await RequestHelper.RequestPost<string>("/server/register", data, Request.Cookies);
            if (message == "")
            {
                return Redirect("/home/login?message=registerSuccessful");
            }
            return Redirect("/home/register?message=" + HttpUtility.UrlEncode(message ?? "Произошла непредвиденная ошибка, повторите попытку позже."));
        }

        [HttpGet]
        public async Task<IActionResult> UserSettings(int userId, bool success, string message)
        {
            UserAccountDataModel? user = await RequestHelper.RequestGet<UserAccountDataModel>("/server/getUserInformation?id=" + userId, Request.Cookies);
            if (user is null)
                return ForbiddenAccessError();
            return View(new UserSettingsDataModel(user, success, message));
        }

        public record UserSettingsInfo(
            int Id,
            string? LastName,
            string? FirstName,
            string? MiddleName,
            string? Email)
        {
            public string GetFullName()
            {
                if (MiddleName?.Length > 0)
                    return $"{LastName} {FirstName} {MiddleName}";
                else
                    return $"{LastName} {FirstName}";
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserSettings([FromForm] UserSettingsInfo userInfo)
        {
            if (userInfo.LastName is null
                || userInfo.FirstName is null
                || userInfo.MiddleName is null
                || userInfo.Email is null)
                return Redirect($"/home/userSettings?userId={userInfo.Id}&message=" + HttpUtility.UrlEncode("Все поля должны быть заполнены."));

            HttpStatusCode status = await RequestHelper.RequestPostStatus("/server/saveUserInformation", userInfo, Request.Cookies);
            if (status != HttpStatusCode.OK)
                return ForbiddenAccessError();
            int currentUserId = int.Parse(User?.FindFirst("UserId")?.Value ?? "0");
            if (currentUserId == userInfo.Id)
            {
                await HttpContext.SignOutAsync();
                await UserLogin(currentUserId, userInfo.GetFullName());
            }
            return Redirect($"/home/userSettings?userId={userInfo.Id}&success=true");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int userId, string message)
        {
            UserAccountDataModel? user = await RequestHelper.RequestGet<UserAccountDataModel>("/server/getUserInformation?id=" + userId, Request.Cookies);
            if (user is null)
                return ForbiddenAccessError();
            return View(new ChangePasswordDataModel(user, message));
        }

        public record UserPasswordInfo(
            int Id,
            string? Password,
            string? ConfirmPassword);

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] UserPasswordInfo passInfo)
        {
            if (passInfo.Password is null || passInfo.ConfirmPassword is null)
                return Redirect($"/home/changePassword?userId={passInfo.Id}&message=" + HttpUtility.UrlEncode("Все поля должны быть заполнены."));
            string? message = await RequestHelper.RequestPost<string>("/server/saveUserPassword", passInfo, Request.Cookies);
            if (RequestHelper.LastStatusCode == HttpStatusCode.NotFound || RequestHelper.LastStatusCode == HttpStatusCode.Forbidden)
                return ForbiddenAccessError();
            if (RequestHelper.LastStatusCode == HttpStatusCode.OK)
                return Redirect("/home/userSettings?userId=" + passInfo.Id);
            if (message is null)
                return UnknownError();
            return Redirect($"/home/changePassword?userId={passInfo.Id}&message=" + HttpUtility.UrlEncode(message));
        }

        private IActionResult AppError(string message)
        {
            ViewData["Message"] = message;
            return View("AppError");
        }

        public IActionResult ForbiddenAccessError() => AppError(
            "У Вас нет доступа к данному действию или то, что Вы запрашиваете, не существует. " +
            "Если Вы считаете, что это ошибка, обратитесь к администратору сайта."
            );
        public IActionResult UnknownError()
        {
            if (env.IsDevelopment())
                throw new Exception("Был вызван метод HomeController.UnknownError()");
            else
                return AppError("Кажется, что-то пошло не так. Пожалуйста, обратитесь к администратору сайта.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
