using PosSystem.CustomHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PosSystem.Models
{
    // 登入
    public class LoginMD
    {
        [Required(ErrorMessage = "請輸入手機號碼")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "手機號碼錯誤")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "請輸入有效手機號碼 (09xxxxxxxx)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效密碼 (密碼長度 6 - 20字元)")]
        public string UserPwd { get; set; }
    }

    // 員工管理 & 帳號設定
    public class UserMD
    {
        public UserMD()
        {
            AuthOptions = new List<SelectListItem> { new SelectListItem { Text = "使用者", Value = "使用者" }, new SelectListItem { Text = "管理者", Value = "管理者" } };
        }

        public long RecordID { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "請輸入有效姓名 (字數長度 2 - 30 字元)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入身份證字號")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "請輸入有效身份證字號 (字數長度 4 或 10 字元)")]
        //[RegularExpression(@"^[a-zA-Z][0-9]+$", ErrorMessage = "身份證字號格式錯誤")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "請輸入西元生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "請輸入手機號碼")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "手機號碼錯誤")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "請輸入有效手機號碼 (09xxxxxxxx)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請輸入住址")]
        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string Address { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "電子郵件無效")]
        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string Email { get; set; }

        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string LineID { get; set; }

        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string WeChatID { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效密碼 (密碼長度 6 - 20字元)")]
        public string UserPwd { get; set; }

        [Required(ErrorMessage = "請選擇操作權限")]
        public string AuthLevel { get; set; }

        public DateTime ConstructTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        [Required(ErrorMessage = "請輸入管理者密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效管理者密碼")]
        public string AdministratorPwd { get; set; }

        public List<SelectListItem> AuthOptions { get; set; }
    }

    // 餐點管理
    public class MealMD
    {
        public MealMD()
        {
            TypeOptions = new List<SelectListItem> {
                new SelectListItem { Text = "餐點", Value = "餐點" },
                new SelectListItem { Text = "飲料", Value = "飲料" },
                new SelectListItem { Text = "其他", Value = "其他" }
            };
        }

        public long RecordID { get; set; }

        [Required(ErrorMessage = "請選擇餐點類別")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "請選擇餐點類別")]
        public string Type { get; set; }

        [Required(ErrorMessage = "請輸入餐點名稱")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "請輸入餐點名稱")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入積分")]
        [Range(10, int.MaxValue, ErrorMessage = "請輸入有效積分")]
        public int Integration { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        [Required(ErrorMessage = "請輸入管理者密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效管理者密碼")]
        public string AdministratorPwd { get; set; }

        public List<SelectListItem> TypeOptions { get; set; }
    }

    // 會員管理
    public class MemberMD
    {
        public long RecordID { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "請輸入有效姓名 (字數長度 2 - 30 字元)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入身份證字號")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "請輸入有效身份證字號 (字數長度 4 或 10 字元)")]
        //[RegularExpression(@"^[a-zA-Z][0-9]+$", ErrorMessage = "身份證字號格式錯誤")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "請輸入西元生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "請輸入手機號碼")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "手機號碼錯誤")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "請輸入有效手機號碼 (09xxxxxxxx)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請輸入住址")]
        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string Address { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "電子郵件無效")]
        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string Email { get; set; }

        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string LineID { get; set; }

        [MaxLength(30, ErrorMessage = "字數長度過長 (字數長度 30 字元)")]
        public string WeChatID { get; set; }

        [Required(ErrorMessage = "請輸入免驗證額度")]
        public long FreeChangingAmount { get; set; }

        [Required(ErrorMessage = "請輸入會員密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效會員密碼 (密碼長度 6 - 20字元)")]
        public string MemberPwd { get; set; }

        public DateTime ConstructTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OperatorID { get; set; }

        public long Integration { get; set; }

        public DateTime? LastConsumptionDate { get; set; }

        [Required(ErrorMessage = "請輸入管理者密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效管理者密碼")]
        public string AdministratorPwd { get; set; }
    }

    // 積分操作
    public class OperationMD
    {
        public OperationMD()
        {
            RepositoryMember repo = new RepositoryMember();
            List<MemberViewModel> members = repo.Query();

            MemberOptions = new List<MemberSelectListItem>();
            TargetMemberOptions = new List<MemberSelectListItem>();

            foreach (var member in members)
            {
                MemberSelectListItem option = new MemberSelectListItem
                {
                    Text = member.Name,
                    Value = member.RecordID.ToString(),
                    Phone = member.Phone,
                    Integration = member.Integration.ToString(),
                    FreeChangeAmount = member.FreeChangingAmount.ToString()
                };

                MemberOptions.Add(option);
                TargetMemberOptions.Add(option);
            }

            Balance = MemberOptions[0].Integration;
        }

        [Required(ErrorMessage = "請選擇會員")]
        public long MemberID { get; set; }

        [Required(ErrorMessage = "請輸入積分")]
        [Range(10, long.MaxValue, ErrorMessage = "請輸入有效積分")]
        public long Integration { get; set; }

        [Required(ErrorMessage = "請選擇轉入會員")]
        public long TargetMemberID { get; set; }

        [Required(ErrorMessage = "請輸入會員密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效會員密碼 (密碼長度 6 - 20字元)")]
        public string MemberPwd { get; set; }

        [Required(ErrorMessage = "請輸入管理者密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效管理者密碼")]
        public string AdministratorPwd { get; set; }

        public string Balance { get; set; }

        public List<MemberSelectListItem> MemberOptions { get; set; }

        public List<MemberSelectListItem> TargetMemberOptions { get; set; }
    }

    // 租金結帳
    public class RentMD
    {
        public long RecordID { get; set; }

        [Required(ErrorMessage = "請輸入積分")]
        [Range(10, long.MaxValue, ErrorMessage = "請輸入有效積分")]
        public long Integration { get; set; }

        public string Status { get; set; }

        public DateTime UpdateTime { get; set; }

        public string OperatorName { get; set; }

        [Required(ErrorMessage = "請輸入管理者密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效管理者密碼")]
        public string AdministratorPwd { get; set; }
    }

    // 點餐
    public class OrderMD
    {
        public OrderMD()
        {
            Order = new List<OrderInfo>();
            MemberOptions = new List<MemberSelectListItem>();

            RepositoryMember repo = new RepositoryMember();

            foreach (var member in repo.Query())
            {
                MemberSelectListItem option = new MemberSelectListItem
                {
                    Text = member.Name,
                    Value = member.RecordID.ToString(),
                    Phone = member.Phone,
                    Integration = member.Integration.ToString(),
                    FreeChangeAmount = member.FreeChangingAmount.ToString()
                };

                MemberOptions.Add(option);
            }
        }

        public string OrderNo { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<OrderInfo> Order { get; set; }

        [Required(ErrorMessage = "請選擇會員")]
        public long MemberID { get; set; }

        [Required(ErrorMessage = "請輸入會員密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效會員密碼 (密碼長度 6 - 20字元)")]
        public string MemberPwd { get; set; }

        [Required(ErrorMessage = "請輸入管理者密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "請輸入有效管理者密碼")]
        public string AdministratorPwd { get; set; }

        public List<MemberSelectListItem> MemberOptions { get; set; }
    }
}