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
    public class MealController : Controller
    {
        // 餐點管理首頁
        public ActionResult Index()
        {
            RepositoryMeal repo = new RepositoryMeal();
            List<MealViewModel> data = repo.Query();

            return View(data);
        }

        // 詳細資訊
        public ActionResult Detail(long RecordID)
        {
            RepositoryMeal repo = new RepositoryMeal();
            MealViewModel data = repo.GetMealViewModel(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Detail", data);
        }

        // 新增餐點
        public ActionResult Create()
        {
            MealMD data = new MealMD();

            return PartialView("_Create", data);
        }

        // 編輯餐點
        public ActionResult Update(long RecordID)
        {
            RepositoryMeal repo = new RepositoryMeal();
            MealMD data = repo.GetMealMD(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Update", data);
        }

        // 刪除餐點
        public ActionResult Delete(long RecordID)
        {
            RepositoryMeal repo = new RepositoryMeal();
            MealMD data = repo.GetMealMD(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Delete", data);
        }

        // 新增方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MealMD Data)
        {
            RepositoryMeal repo = new RepositoryMeal();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<MealViewModel> result = repo.Create(Data, auth);

            return Json(result);
        }

        // 編輯方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(MealMD Data)
        {
            RepositoryMeal repo = new RepositoryMeal();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<MealViewModel> result = repo.Update(Data, auth);

            return Json(result);
        }

        // 刪除方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MealMD Data)
        {
            RepositoryMeal repo = new RepositoryMeal();
            OperationResult<MealViewModel> result = repo.Delete(Data);

            return Json(result);
        }
    }
}