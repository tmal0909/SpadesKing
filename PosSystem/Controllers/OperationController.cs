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
    public class OperationController : Controller
    {
        // 操作紀錄首頁
        public ActionResult Index()
        {
            RepositoryOperationRecord repo = new RepositoryOperationRecord();
            List<OperationRecordViewModel> data = repo.Query();

            return PartialView("_Index", data);
        }

        // 購買積分
        public ActionResult Purchase()
        {
            OperationMD data = new OperationMD();

            return PartialView("_Purchase", data);
        }
        
        // 提領積分
        public ActionResult Withdraw()
        {
            OperationMD data = new OperationMD();

            return PartialView("_Withdraw", data);
        }

        // 回存積分
        public ActionResult Deposit()
        {
            OperationMD data = new OperationMD();

            return PartialView("_Deposit", data);
        }

        // 轉移積分
        public ActionResult Transfer()
        {
            OperationMD data = new OperationMD();

            return PartialView("_Transfer", data);
        }

        // 修改
        public ActionResult Modify()
        {
            OperationMD data = new OperationMD();

            return PartialView("_Modify", data);
        }

        // 購買方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(OperationMD Data)
        {
            RepositoryOperationRecord repo = new RepositoryOperationRecord();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OperationRecordViewModel> result = repo.Purchase(Data, auth);

            return Json(result);
        }

        // 提領方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(OperationMD Data)
        {
            RepositoryOperationRecord repo = new RepositoryOperationRecord();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OperationRecordViewModel> result = repo.Withdraw(Data, auth);

            return Json(result);
        }

        // 回存方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit(OperationMD Data)
        {
            RepositoryOperationRecord repo = new RepositoryOperationRecord();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OperationRecordViewModel> result = repo.Deposit(Data, auth);

            return Json(result);
        }

        // 轉移方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(OperationMD Data)
        {
            RepositoryOperationRecord repo = new RepositoryOperationRecord();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OperationRecordViewModel> result = repo.Transfer(Data, auth);

            return Json(result);
        }

        // 修正方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(OperationMD Data)
        {
            RepositoryOperationRecord repo = new RepositoryOperationRecord();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OperationRecordViewModel> result = repo.Modify(Data, auth);

            return Json(result);
        }
    }
}