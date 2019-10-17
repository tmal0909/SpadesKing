using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosSystem.Models
{
    // 員工
    public class UserViewModel
    {
        public long RecordID { get; set; }

        public string Name { get; set; }

        public string IdentityNumber { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string LineID { get; set; }

        public string WeChatID { get; set; }

        public string UserPwd { get; set; }

        public string AuthLevel { get; set; }

        public DateTime ConstructTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
    }

    // 餐點
    public class MealViewModel
    {
        public long RecordID { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public int Integration { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
    }

    // 會員
    public class MemberViewModel
    {
        public long RecordID { get; set; }

        public string Name { get; set; }

        public string IdentityNumber { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string LineID { get; set; }

        public string WeChatID { get; set; }

        public long FreeChangingAmount { get; set; }

        public string MemberPwd { get; set; }

        public DateTime ConstructTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }

        public long Integration { get; set; }

        public DateTime? LastConsumptionDate { get; set; }
    }

    // 會員消費紀錄
    public class MemberOperationRecordViewModel
    {
        public MemberViewModel Member { get; set; }

        public List<OperationRecordViewModel> Record { get; set; }
    }

    // 發行紀錄
    public class IssueRecordViewModel
    {
        public long RecordID { get; set; }

        public string OperationType { get; set; }

        public long Integration { get; set; }

        public string Category { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
    }

    // 操作紀錄
    public class OperationRecordViewModel
    {
        public long RecordID { get; set; }

        public long MemberID { get; set; }

        public string MemberName { get; set; }

        public string OperationType { get; set; }

        public long Integration { get; set; }

        public string Note { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
    }

    // 積分發行概況
    public class IssueSummaryViewModel
    {
        public long Distribution { get; set; }

        public long Balance { get; set; }
    }

    // 帳務
    public class AccountingViewModel
    {
        public long RecordID { get; set; }

        public string Type { get; set; }

        public string PaymentType { get; set; }

        public long Integration { get; set; }

        public string Status { get; set; }

        public string OrderNo { get; set; }

        public long? MemberID { get; set; }

        public string MemberName { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
    }

    // 帳務點餐
    public class AccountingOrderViewModel
    {
        #region 帳務
        public long RecordID { get; set; }

        public string Type { get; set; }

        public string PaymentType { get; set; }

        public long Integration { get; set; }

        public string Status { get; set; }

        public string OrderNo { get; set; }

        public long? MemberID { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
        #endregion

        #region 點餐資訊
        public long? OrderRecordID { get; set; }

        public string OrderOrderNo { get; set; }

        public long? OrderMealID { get; set; }

        public string OrderMealType { get; set; }

        public string OrderMealName { get; set; }

        public long? OrderUnitIntegration { get; set; }

        public string OrderStatus { get; set; }

        public int? OrderQuantity { get; set; }

        public DateTime? OrderUpdateTime { get; set; }

        public long? OrderOperatorID { get; set; }

        public string OrderOperatorName { get; set; }
        #endregion
    }

    // 營業分析
    public class AnalysisViewModel
    {
        public AnalysisViewModel()
        {
            GeneralAccounting = new List<AccountingViewModel>();
            StoredAccounting = new List<IssueRecordViewModel>();
            DateList = new List<string>();
        }

        public List<AccountingViewModel> GeneralAccounting { get; set; }

        public List<IssueRecordViewModel> StoredAccounting { get; set; }

        public List<string> DateList { get; set; }
    }

    // 統計圖表
    public class StatisticChartViewModel
    {
        public string DateStart { get; set; }

        public string DateEnd { get; set; }

        public long SumRent { get; set; }

        public long SumOrder { get; set; }

        public long SumTreat { get; set; }

        public long SumIncome { get; set; }
    }

    // 出餐列表
    public class OrderViewModel
    {
        public long RecordID { get; set; }

        public string OrderNo { get; set; }

        public long MealID { get; set; }

        public string MealType { get; set; }

        public string MealName { get; set; }

        public long UnitIntegration { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public string OperatorName { get; set; }
    }

    // 操作結果
    public class OperationResult<T>
    {
        public OperationResult()
        {
            Status = false;
            Message = string.Empty;
            Type = string.Empty;
            Data = new List<T>();
        }

        public bool Status { get; set; }

        public string Message { get; set; }

        public string Type { get; set; }

        public List<T> Data { get; set; }
    }
}