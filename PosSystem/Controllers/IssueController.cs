using PosSystem.CustomAuthentication;
using PosSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PosSystem.Controllers
{
    [CustomAuthorize]
    public class IssueController : Controller
    {
        // 發行概況
        public ActionResult Index()
        {
            IssueSummaryViewModel data = new IssueSummaryViewModel();
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            RepositoryMember repoMember = new RepositoryMember();

            data.Distribution = repoIssueRecord.GetDistribution();
            data.Balance = repoMember.GetBalanceSum();

            return PartialView("_Index", data);
        }

        // 發行紀錄
        public ActionResult Record()
        {
            RepositoryIssueRecord repo = new RepositoryIssueRecord();
            List<IssueRecordViewModel> data = repo.Query();

            return PartialView("_Record", data);
        }

        [HttpPost]
        public ActionResult GetSummary()
        {
            IssueSummaryViewModel data = new IssueSummaryViewModel();
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            RepositoryMember repoMember = new RepositoryMember();

            data.Distribution = repoIssueRecord.GetDistribution();
            data.Balance = repoMember.GetBalanceSum();

            return Json(data);
        }
    }
}