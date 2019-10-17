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
    public class MemberController : Controller
    {
        // 會員管理首頁
        public ActionResult Index()
        {
            RepositoryMember repo = new RepositoryMember();
            List<MemberViewModel> data = repo.Query();

            return View(data);
        }

        // 詳細資訊
        public ActionResult Detail(long RecordID)
        {
            RepositoryMember repo = new RepositoryMember();
            MemberViewModel data = repo.GetMemberViewModel(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Detail", data);
        }

        // 消費紀錄
        public ActionResult OperationRecord(long RecordID)
        {
            MemberOperationRecordViewModel data = new MemberOperationRecordViewModel();
            RepositoryMember repoMember = new RepositoryMember();
            RepositoryOperationRecord repoRecord = new RepositoryOperationRecord();

            data.Member = repoMember.GetMemberViewModel(RecordID);
            data.Record = repoRecord.Query().Where(x => x.MemberID == RecordID).ToList();

            return PartialView("_OperationRecord", data);
        }

        // 新增會員
        public ActionResult Create()
        {
            MemberMD data = new MemberMD();

            return PartialView("_Create", data);
        }

        // 編輯會員
        public ActionResult Update(long RecordID)
        {
            RepositoryMember repo = new RepositoryMember();
            MemberMD data = repo.GetMemberMD(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Update", data);
        }

        // 刪除會員
        public ActionResult Delete(long RecordID)
        {
            RepositoryMember repo = new RepositoryMember();
            MemberMD data = repo.GetMemberMD(RecordID);

            return data == null ? PartialView("_Error") : PartialView("_Delete", data);
        }

        // 新增方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberMD Data)
        {
            RepositoryMember repo = new RepositoryMember();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<MemberViewModel> result = repo.Create(Data, auth);

            return Json(result);
        }

        // 編輯方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(MemberMD Data)
        {
            RepositoryMember repo = new RepositoryMember();
            AuthModel auth = RepositoryAuthModel.GetAuthModel(HttpContext.User.Identity as FormsIdentity);
            OperationResult<MemberViewModel> result = repo.Update(Data, auth);

            return Json(result);
        }

        // 刪除方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MemberMD Data)
        {
            RepositoryMember repo = new RepositoryMember();
            OperationResult<MemberViewModel> result = repo.Delete(Data);

            return Json(result);
        }
    }
}