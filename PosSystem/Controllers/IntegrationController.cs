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
    public class IntegrationController : Controller
    {
        // 積分管理首頁
        public ActionResult Index()
        {
            return View();
        }
    }
}