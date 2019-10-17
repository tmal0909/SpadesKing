using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosSystem.Models
{
    // 員工
    public class User
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
    }

    // 餐點
    public class Meal
    {
        public long RecordID { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public int Integration { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }
    }

    // 會員
    public class Member
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

        public long Integration { get; set; }

        public DateTime? LastConsumptionDate { get; set; }
    }

    // 發行紀錄
    public class IssueRecord
    {
        public long RecordID { get; set; }

        public string OperationType { get; set; }

        public long Integration { get; set; }

        public string Category { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }
    }
    
    // 操作紀錄
    public class OperationRecord
    {
        public long RecordID { get; set; }

        public long MemberID { get; set; }

        public string OperationType { get; set; }

        public long Integration { get; set; }

        public string Note { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }
    }

    // 帳務
    public class Accounting
    {
        public long RecordID { get; set; }

        public string Type { get; set; }

        public string PaymentType { get; set; }

        public long Integration { get; set; }

        public string Status { get; set; }

        public string OrderNo { get; set; }

        public long? MemberID { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }
    }

    // 出餐列表
    public class Order
    {
        public long RecordID { get; set; }

        public string OrderNo { get; set; }
        
        public long MealID { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }
    }
}