using PosSystem.CustomAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PosSystem.Controllers
{
    [CustomAuthorize]
    public class AnalysisController : Controller
    {
        // 營業分析首頁
        public ActionResult Index()
        {
            return View();
        }
    }
}