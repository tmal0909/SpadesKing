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
    public class OrderController : Controller
    {
        // 點餐首頁
        public ActionResult Index()
        {
            RepositoryMeal repo = new RepositoryMeal();
            List<MealViewModel> data = repo.Query();

            return View(data);
        }

        // 出餐明細
        public ActionResult Detail()
        {
            RepositoryOrder repo = new RepositoryOrder();
            List<OrderViewModel> data = repo.Query();

            return View(data);
        }

        // 點餐結帳
        [HttpPost]
        public ActionResult Check(OrderMD Data)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            OrderMD data = repo.GetOrderMD(Data);
            ModelState.Clear();
            
            return data == null ? PartialView("_Error") : PartialView("_Check", data);
        }

        // 刪除點餐紀錄
        public ActionResult Cancel(string OrderNo)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            OrderMD data = repo.GetOrderMD(OrderNo);

            return data == null ? PartialView("_Error") : PartialView("_Cancel", data);
        }

        // 出餐列表更新
        [HttpPost]
        public ActionResult RefreshOrder()
        {
            RepositoryOrder repo = new RepositoryOrder();
            List<OrderViewModel> data = repo.Query();

            return Json(data);
        }

        // 結帳方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOrder(OrderMD Data)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<AccountingViewModel> result = repo.CheckOrder(Data, auth);

            return Json(result);
        }

        // 取消方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelOrder(OrderMD Data)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OrderViewModel> result = repo.CancelOrder(Data, auth);

            return Json(result);
        }

        // 完成方法
        [HttpPost]
        public ActionResult Finish(long RecordID)
        {
            RepositoryOrder repo = new RepositoryOrder();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<OrderViewModel> result = repo.Finish(RecordID, auth);

            return Json(result);
        }
    }
}