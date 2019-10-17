using System.Web;
using System.Web.Optimization;

namespace PosSystem
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryPlugIn").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.BlockUI.js",
                        "~/Scripts/jquery.DataTables.min.js",
                        "~/Scripts/jquery.DataTables.foundation.min.js",
                        "~/Scripts/jquery.DataTables.FindCellRowIndex.js",
                        "~/Scripts/jquery.DataTables.rowsGroup.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.DatePicker-zh-TW.js",
                        "~/Scripts/jquery.Select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/externalPlugIn").Include(
                        "~/Scripts/external.moment.js",
                        "~/Scripts/external.highchart.js"));

            bundles.Add(new ScriptBundle("~/bundles/calcite").Include(
                        "~/Scripts/calcite-web.min.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/siteJs").Include(
                        "~/Scripts/siteConfig.js",
                        "~/Scripts/siteUtil.js"));

            #region 各頁面 js 檔案
            // 點餐結帳
            bundles.Add(new ScriptBundle("~/bundles/siteOrderIndexJs").Include(
                        "~/Scripts/siteOrderIndex.js"));

            // 出餐明細
            bundles.Add(new ScriptBundle("~/bundles/siteOrderDetailJs").Include(
                        "~/Scripts/siteOrderDetail.js"));

            // 租金管理
            bundles.Add(new ScriptBundle("~/bundles/siteRentJs").Include(
                        "~/Scripts/siteRent.js"));

            // 發行概況
            bundles.Add(new ScriptBundle("~/bundles/siteIssueJs").Include(
                        "~/Scripts/siteIssue.js"));

            // 操作紀錄
            bundles.Add(new ScriptBundle("~/bundles/siteOperationJs").Include(
                        "~/Scripts/siteOperation.js"));

            // 會員管理
            bundles.Add(new ScriptBundle("~/bundles/siteMemberJs").Include(
                        "~/Scripts/siteMember.js"));

            // 帳號設定
            bundles.Add(new ScriptBundle("~/bundles/siteSettingJs").Include(
                        "~/Scripts/siteSetting.js"));

            // 餐點管理
            bundles.Add(new ScriptBundle("~/bundles/siteMealJs").Include(
                        "~/Scripts/siteMeal.js"));

            // 員工管理
            bundles.Add(new ScriptBundle("~/bundles/siteUserJs").Include(
                        "~/Scripts/siteUser.js"));

            // 帳務資料
            bundles.Add(new ScriptBundle("~/bundles/siteAccountingJs").Include(
                        "~/Scripts/siteAccounting.js"));
            #endregion

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/jquery-ui.css",
                        "~/Content/jquery.Select2.css",
                        "~/Content/calcite-web.min.css",
                        "~/Content/jquery.datatables.min.css",
                        "~/Content/jquery.dataTables.foundation.min.css",
                        "~/Content/style.min.css"));
        }
    }
}
