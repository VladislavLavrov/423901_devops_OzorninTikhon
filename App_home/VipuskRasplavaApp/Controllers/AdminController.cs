using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaspredeleniyeDutyaApp.Helpers;
using RaspredeleniyeDutyaApp.Models.Client;
using System.Net;

namespace RaspredeleniyeDutyaApp.Controllers
{
    public class AdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Панель администратора недоступна для гостей и пользователей,
            // которые не являются администраторами.
            if (!RequestHelper.RequestIsUserAdmin(Request.Cookies))
            {
                context.Result = RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public static bool FilterUser(UserAccountDataModel user, UserInformationDataModel userInfo)
        {
            if (userInfo.UserId is not null && user.Id != userInfo.UserId)
                return false;
            if (userInfo.UserLastName is not null && !user.LastName.Contains(userInfo.UserLastName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (userInfo.UserFirstName is not null && !user.FirstName.Contains(userInfo.UserFirstName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (userInfo.UserMiddleName is not null && !user.MiddleName.Contains(userInfo.UserMiddleName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (userInfo.UserEmail is not null && !user.Email.Contains(userInfo.UserEmail, StringComparison.OrdinalIgnoreCase))
                return false;
            if (userInfo.UserIsAdmin is not null && (bool)userInfo.UserIsAdmin && !user.IsAdmin)
                return false;
            return true;
        }

        [HttpGet]
        public async Task<IActionResult> UserList([FromQuery] UserInformationDataModel userInfo)
        {
            List<UserAccountDataModel>? users =
                await RequestHelper.RequestGet<List<UserAccountDataModel>>("/server/getUserList", Request.Cookies);
            users ??= [];
            users = users.Where(u => FilterUser(u, userInfo)).ToList();
            return View(users);
        }

        [HttpPost]
        [ActionName("UserList")]
        public IActionResult UserListPost(UserInformationDataModel userInfo)
        {
            return RedirectToAction("UserList", userInfo);
        }

        public async Task<IActionResult> UserDeleteConfirmation(int userId)
        {
            UserAccountDataModel? user =
                await RequestHelper.RequestGet<UserAccountDataModel>("/server/getUserInformation?id=" + userId, Request.Cookies);
            if (user is null)
                return RedirectToAction("ForbiddenAccessError", "Home");
            return View(user);
        }

        public async Task<IActionResult> UserDelete(int userId)
        {
            HttpStatusCode status = await RequestHelper.RequestGetStatus("/server/deleteUser?userId=" + userId, Request.Cookies);
            return status switch
            {
                HttpStatusCode.OK => RedirectToAction("UserList"),
                HttpStatusCode.Forbidden or HttpStatusCode.NotFound => RedirectToAction("ForbiddenAccessError", "Home"),
                _ => RedirectToAction("UnknownError", "Home"),
            };
        }

        public async Task<IActionResult> UserAdminConfirmation(int userId)
        {
            UserAccountDataModel? user =
                await RequestHelper.RequestGet<UserAccountDataModel>("/server/getUserInformation?id=" + userId, Request.Cookies);
            if (user is null)
                return RedirectToAction("ForbiddenAccessError", "Home");
            return View(user);
        }

        public async Task<IActionResult> SetUserAdmin(int userId, bool admin)
        {
            HttpStatusCode status = await RequestHelper.RequestGetStatus($"/server/setUserAdmin?userId={userId}&admin={admin}", Request.Cookies);
            return status switch
            {
                HttpStatusCode.OK => RedirectToAction("UserList"),
                HttpStatusCode.Forbidden or HttpStatusCode.NotFound => RedirectToAction("ForbiddenAccessError", "Home"),
                _ => RedirectToAction("UnknownError", "Home"),
            };
        }
    }
}
