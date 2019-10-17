using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PosSystem.Models;
using System.Web.Security;
using PosSystem.CustomAuthentication;
using Newtonsoft.Json;

namespace PosSystem.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginMD data = new LoginMD();

            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginMD Data)
        {
            RepositoryUser repo = new RepositoryUser();
            UserViewModel user = repo.GetUserViewModel(new User { Phone = Data.Phone });

            // 驗證帳號
            if (user == null)
            {
                TempData["ErrMsg"] = "登入失敗 : 帳號錯誤";
                return RedirectToAction("Login", "Home");
            }

            // 驗證密碼
            if (!Data.UserPwd.Equals(user.UserPwd))
            {
                TempData["ErrMsg"] = "登入失敗 : 密碼錯誤";
                return RedirectToAction("Login", "Home");
            }

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                user.Name,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true,
                JsonConvert.SerializeObject(new AuthModel { ID = user.RecordID, Name = user.Name, Phone = user.Phone, AuthLevel = user.AuthLevel }),
                FormsAuthentication.FormsCookiePath
            );

            string encTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Login", "Home");
        }

        public ActionResult Index()
        {
            ViewBag.AuthLevel = RepositoryAuthModel.GetAuthLevel(HttpContext.User.Identity as FormsIdentity);

            return View();
        }
    }
}