using PosSystem.CustomAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PosSystem.Models;
using System.Web.Security;

namespace PosSystem.Controllers
{
    [CustomAuthorize]
    public class SettingController : Controller
    {
        // 帳號設定首頁
        public ActionResult Index()
        {
            RepositoryUser repo = new RepositoryUser();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            UserViewModel data = repo.GetUserViewModel(auth.ID);
            
            return data == null ? View("Error") : View(data);
        }

        // 編輯帳號
        public ActionResult Update(long RecordID)
        {
            RepositoryUser repo = new RepositoryUser();
            UserMD data = repo.GetUserMD(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Update", data);
        }

        // 編輯方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserMD Data)
        {
            RepositoryUser repo = new RepositoryUser();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<UserViewModel> result = repo.Update(Data, auth, false);

            return Json(result);
        }
    }
}