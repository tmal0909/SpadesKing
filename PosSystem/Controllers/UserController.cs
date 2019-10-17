using PosSystem.CustomAuthentication;
using PosSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PosSystem.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        // 員工管理首頁
        public ActionResult Index()
        {
            RepositoryUser repo = new RepositoryUser();
            List<UserViewModel> data = repo.Query();

            return View(data);
        }

        // 詳細資訊
        public ActionResult Detail(long RecordID)
        {
            RepositoryUser repo = new RepositoryUser();
            UserViewModel data = repo.GetUserViewModel(RecordID);
            
            return data == null ? PartialView("_Error") : PartialView("_Detail", data);
        }

        // 新增員工
        public ActionResult Create()
        {
            UserMD data = new UserMD();

            return PartialView("_Create", data);
        }

        // 編輯員工
        public ActionResult Update(long RecordID)
        {
            RepositoryUser repo = new RepositoryUser();
            UserMD data = repo.GetUserMD(RecordID);
            
            return data == null ? PartialView("_Error") : PartialView("_Update", data);
        }

        // 刪除
        public ActionResult Delete(long RecordID)
        {
            RepositoryUser repo = new RepositoryUser();
            UserMD data = repo.GetUserMD(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Delete", data);
        }

        // 新增方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMD Data)
        {
            RepositoryUser repo = new RepositoryUser();
            AuthModel user = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<UserViewModel> result = repo.Create(Data, user);

            return Json(result);
        }

        // 編輯方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserMD Data)
        {
            RepositoryUser repo = new RepositoryUser();
            AuthModel user = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<UserViewModel> result = repo.Update(Data, user, true);

            return Json(result);
        }

        // 刪除方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserMD Data)
        {
            RepositoryUser repo = new RepositoryUser();
            OperationResult<UserViewModel> result = repo.Delete(Data);

            return Json(result);
        }
    }
}