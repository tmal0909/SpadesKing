using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosSystem.Models
{
    // 發行紀錄 - 操作選項
    public static class IssueRecord_OperationType
    {
        public const string Import = "轉入";

        public const string Export = "轉出";
    }

    // 發行紀錄 - 分類選項
    public static class IssueRecord_Category
    {
        public const string Purchase = "積分購買";

        public const string Modify = "積分修正";

        public const string CheckRent = "租金結帳";

        public const string CancelRent = "租金取消";

        public const string CheckOrder = "餐點結帳";

        public const string CancelOrder = "餐點取消";
    }

    // 操作紀錄 - 操作選項
    public static class OperationRecord_OperationType
    {
        public const string Purchase = "購買";

        public const string Withdraw = "提領";

        public const string Deposit = "回存";

        public const string Transfer = "轉移";

        public const string Modify = "修正";

        public const string CheckOrder = "餐點結帳";

        public const string CancelOrder = "餐點取消";
    }

    // 積分餘額操作資訊 -  操作選項
    public static class BalanceOperationInfo_OperationType
    {
        public const string Import = "轉入";

        public const string Export = "轉出";
    }

    // 積分餘額操作資訊 - 驗證選項
    public static class BalanceOperationInfo_ValidationType
    {
        public const string Auto = "自動";

        public const string None = "不驗證";

        public const string Force = "強制";
    }

    // 帳務紀錄 - 帳務類別
    public static class Accounting_Type
    {
        public const string Rent = "租金";

        public const string Order = "餐點";
    }

    // 帳務紀錄 - 付款方式
    public static class Accounting_PaymentType
    {
        public const string Cash = "現金";

        public const string Integration = "積分";

        public const string Member = "會員";

        public const string Treat = "招待";
    }

    // 帳務紀錄 - 狀態
    public static class Accounting_Status
    {
        public const string Normal = "";

        public const string Cancel = "取消";
    }

    // 出餐列表 - 狀態
    public static class Order_Status
    {
        public const string Preparing = "準備中";

        public const string Finished = "完成";

        public const string Canceled = "取消";
    } 

    // 積分餘額操作資訊
    public class BalanceOperationInfo
    {
        public long MemberID { get; set; }

        public string MemberPwd { get; set; }

        public long Integration { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperationType { get; set; }

        public string ValidationType { get; set; }

        public bool UpdateConsumptionDate { get; set; }
    }
    
    // 點餐資訊
    public class OrderInfo
    {
        public long MealID { get; set; }

        public string MealName { get; set; }

        public string MealType { get; set; }

        public int Quantity { get; set; }

        public long Integration { get; set; }

        public long Total { get; set; }

        public string Status { get; set; }
    }
    
    // 操作結果
    public class OperationResult
    {
        public OperationResult()
        {
            Status = false;
            Message = string.Empty;
        }

        public bool Status { get; set; }

        public string Message { get; set; }
    }
}