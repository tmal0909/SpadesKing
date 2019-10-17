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
    public class AccountingController : Controller
    {
        // 帳務資料首頁
        public ActionResult Index()
        {
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            RepositoryAccounting repoAccounting = new RepositoryAccounting();
            AnalysisViewModel data = new AnalysisViewModel();

            data.GeneralAccounting = repoAccounting.Query();
            data.StoredAccounting = repoIssueRecord.Query();
            data.DateList.AddRange(data.GeneralAccounting.Select(x => x.UpdateTime.ToString("yyyy/MM/dd")).ToList());
            data.DateList.AddRange(data.StoredAccounting.Select(x => x.UpdateTime.ToString("yyyy/MM/dd")).ToList());
            data.DateList = data.DateList.Distinct().OrderBy(x => x).ToList();

            return PartialView("_Index", data);
            /*
            RepositoryAccounting repo = new RepositoryAccounting();
            List<AccountingViewModel> data = repo.Query();

            return PartialView("_Index", data);
            */
        }

        // 帳務明細
        public ActionResult AccountingDetail(string Date)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            List<AccountingViewModel> data = repo.Query().Where(x => x.UpdateTime.ToString("yyyy/MM/dd").Equals(Date)).ToList();

            ViewBag.Date = Date;

            return PartialView("_AccountingDetail", data);
        }
        
        // 點餐明細
        public ActionResult OrderDetail(string Date)
        {
            RepositoryOrder repo = new RepositoryOrder();
            List<OrderViewModel> data = repo.Query().Where(x => x.UpdateTime.ToString("yyyy/MM/dd").Equals(Date)).ToList();

            ViewBag.Date = Date;

            return PartialView("_OrderDetail", data);
        }

        // 統計圖表
        public ActionResult StatisticChart(DateTime DateStart, DateTime DateEnd)
        {
            RepositoryAccounting repo = new RepositoryAccounting();
            StatisticChartViewModel data = repo.GetStatisticChartViewModel(DateStart, DateEnd);

            return PartialView("_StatisticChart", data);
        }
    }
}