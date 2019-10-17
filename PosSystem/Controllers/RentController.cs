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
    public class RentController : Controller
    {
        // 租金管理首頁
        public ActionResult Index()
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            List<AccountingViewModel> data = repo.Query().Where(x => x.OperatorID == auth.ID && x.Type == Accounting_Type.Rent).ToList();

            return View(data);
        }

        // 結帳
        public ActionResult Check()
        {
            RentMD data = new RentMD();

            return PartialView("_Check", data);
        }

        // 取消
        public ActionResult Cancel(long RecordID)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            RentMD data = repo.GetRentMD(RecordID);

            return PartialView("_Cancel", data);
        }

        // 結帳方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Check(RentMD Data)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<AccountingViewModel> result = repo.CheckRent(Data, auth);

            return Json(result);
        }

        // 取消方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(RentMD Data)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<AccountingViewModel> result = repo.CancelRent(Data, auth);

            return Json(result);
        }
    }
}