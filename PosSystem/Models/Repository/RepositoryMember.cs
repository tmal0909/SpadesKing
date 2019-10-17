using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Dapper;
using PosSystem.CustomAuthentication;

namespace PosSystem.Models
{
    public class RepositoryMember
    {
        private string sql = string.Empty;

        // 查詢
        public List<MemberViewModel> Query()
        {
            List<MemberViewModel> result = new List<MemberViewModel>();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Member] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MemberViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得資料 - By Primary Key
        public MemberViewModel GetMemberViewModel(Member Data)
        {
            MemberViewModel result = new MemberViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Member] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID where A.Phone = @Phone ";
            pars.Add("@Phone", Data.Phone);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MemberViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得資料 - By RecordID
        public MemberViewModel GetMemberViewModel(long RecordID)
        {
            MemberViewModel result = new MemberViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Member] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID where A.RecordID = @RecordID ";
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MemberViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        // 取得編輯資料
        public MemberMD GetMemberMD(long RecordID)
        {
            MemberMD result = new MemberMD();
            DynamicParameters pars = new DynamicParameters();

            sql = "select * from [SpadesKing].[dbo].[Member] where RecordID = @RecordID ";
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MemberMD>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
        
        // 取得儲值餘額
        public long GetBalanceSum()
        {
            List<MemberViewModel> data = Query();

            return data.Sum(x => x.Integration);
        }

        // 建立
        public OperationResult<MemberViewModel> Create(MemberMD Data, AuthModel Operator)
        {
            OperationResult<MemberViewModel> result = new OperationResult<MemberViewModel>();
            DynamicParameters pars = new DynamicParameters();
            DateTime processTime = DateTime.Now;

            result.Type = "Create";

            sql = "insert into [SpadesKing].[dbo].[Member] ";
            sql += "(Name, IdentityNumber, BirthDay, Phone, Address, Email, LineID, WeChatID, FreeChangingAmount, MemberPwd, ConstructTime, UpdateTime, OperatorID, Integration, LastConsumptionDate) values ";
            sql += "(@Name, @IdentityNumber, @BirthDay, @Phone, @Address, @Email, @LineID, @WeChatID, @FreeChangingAmount, @MemberPwd, @ConstructTime, @UpdateTime, @OperatorID, @Integration, @LastConsumptionDate) ";

            pars.Add("@Name", Data.Name);
            pars.Add("@IdentityNumber", Data.IdentityNumber.ToUpper());
            pars.Add("@BirthDay", Data.BirthDay);
            pars.Add("@Phone", Data.Phone);
            pars.Add("@Address", Data.Address);
            pars.Add("@Email", Data.Email);
            pars.Add("@LineID", Data.LineID);
            pars.Add("@WeChatID", Data.WeChatID);
            pars.Add("@FreeChangingAmount", Data.FreeChangingAmount);
            pars.Add("@MemberPwd", Data.MemberPwd);
            pars.Add("@ConstructTime", processTime);
            pars.Add("@UpdateTime", processTime);
            pars.Add("@OperatorID", Operator.ID);
            pars.Add("@Integration", 0);
            pars.Add("@LastConsumptionDate", null);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法新增資料列 (會員)，請聯絡系統管理員");
                    result.Message = "新增成功";
                    result.Data.Add(GetMemberViewModel(new Member { Phone = Data.Phone }));
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "新增失敗 : " + ex.Message;
                result.Data = null;
            }

            return result;
        }

        // 更新
        public OperationResult<MemberViewModel> Update(MemberMD Data, AuthModel Operator)
        {
            OperationResult<MemberViewModel> result = new OperationResult<MemberViewModel>();
            DynamicParameters pars = new DynamicParameters();
            DateTime processTime = DateTime.Now;

            result.Type = "Update";

            sql = "update [SpadesKing].[dbo].[Member] set ";
            sql += "Name = @Name, IdentityNumber = @IdentityNumber, BirthDay = @BirthDay, Address = @Address, Email = @Email, ";
            sql += "LineID = @LineID, WeChatID = @WeChatID, FreeChangingAmount = @FreeChangingAmount, MemberPwd = @MemberPwd, ";
            sql += "UpdateTime = @UpdateTime, OperatorID = @OperatorID ";
            sql += "where RecordID = @RecordID ";

            pars.Add("@Name", Data.Name);
            pars.Add("@IdentityNumber", Data.IdentityNumber.ToUpper());
            pars.Add("@BirthDay", Data.BirthDay);
            pars.Add("@Address", Data.Address);
            pars.Add("@Email", Data.Email);
            pars.Add("@LineID", Data.LineID);
            pars.Add("@WeChatID", Data.WeChatID);
            pars.Add("@FreeChangingAmount", Data.FreeChangingAmount);
            pars.Add("@MemberPwd", Data.MemberPwd);
            pars.Add("@UpdateTime", processTime);
            pars.Add("@OperatorID", Operator.ID);
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法更新資料列 (會員)，請聯絡系統管理員");
                    result.Message = "更新成功";
                    result.Data.Add(GetMemberViewModel(Data.RecordID));
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "更新失敗 : " + ex.Message;
                result.Data = null;
            }

            return result;
        }

        // 刪除
        public OperationResult<MemberViewModel> Delete(MemberMD Data)
        {
            OperationResult<MemberViewModel> result = new OperationResult<MemberViewModel>();
            DynamicParameters pars = new DynamicParameters();
            MemberViewModel member = GetMemberViewModel(Data.RecordID);
            string administratorPwd = ConfigurationManager.AppSettings["AdministratorPwd"].Trim();

            // 驗證管理者密碼
            if (!Data.AdministratorPwd.Equals(administratorPwd))
            {
                result.Status = false;
                result.Message = "刪除失敗 : 管理者密碼錯誤";
                result.Data = null;

                return result;
            }

            // 驗證會員
            if (member == null)
            {
                result.Status = false;
                result.Message = "刪除失敗 : 系統錯誤，無法取得會員資料";
                result.Data = null;

                return result;
            }

            // 驗證積分
            if (member.Integration > 0)
            {
                result.Status = false;
                result.Message = "刪除失敗 : 會員仍有積分未使用，無法刪除該會員";
                result.Data = null;

                return result;
            }

            result.Type = "Delete";

            sql = "delete from [SpadesKing].[dbo].[Member] where RecordID = @RecordID ";
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("資料列無法刪除 (會員)，請聯絡系統管理員");
                    result.Message = "刪除成功";
                    result.Data.Add(member);
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "刪除失敗 : " + ex.Message;
                result.Data = null;
            }

            return result;
        }

        // 會員積分操作
        public OperationResult UpdateBalance(clsDBDapper DB, BalanceOperationInfo Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();
            MemberViewModel member = GetMemberViewModel(Data.MemberID);
            long balance = 0;

            // 驗證會員是否存在
            if (member == null)
            {
                result.Status = false;
                result.Message = "無法取得會員資料";
                return result;
            }

            // 驗證密碼
            switch (Data.ValidationType)
            {
                case BalanceOperationInfo_ValidationType.None:
                    break;

                case BalanceOperationInfo_ValidationType.Force:
                    if (!Data.MemberPwd.Equals(member.MemberPwd))
                    {
                        result.Status = false;
                        result.Message = "會員密碼錯誤";
                        return result;
                    }
                    break;

                case BalanceOperationInfo_ValidationType.Auto:
                    if (Data.Integration > member.FreeChangingAmount)
                    {
                        if (!Data.MemberPwd.Equals(member.MemberPwd))
                        {
                            result.Status = false;
                            result.Message = "會員密碼錯誤";
                            return result;
                        }
                    }
                    break;

                default:
                    result.Status = false;
                    result.Message = "驗證類型錯誤";
                    return result;
            }

            // 取得餘額
            switch (Data.OperationType)
            {
                case BalanceOperationInfo_OperationType.Import:
                    balance = member.Integration + Data.Integration;
                    break;

                case BalanceOperationInfo_OperationType.Export:
                    balance = member.Integration - Data.Integration;
                    break;

                default:
                    result.Status = false;
                    result.Message = "操作類型錯誤";
                    return result;
            }

            // 驗證餘額
            if (balance < 0)
            {
                result.Status = false;
                result.Message = "會員餘額不足";
                return result;
            }

            sql = "update [SpadesKing].[dbo].[Member] ";
            sql += "set UpdateTime = @UpdateTime, OperatorID = @OperatorID, Integration = @Integration, LastConsumptionDate = @LastConsumptionDate ";
            sql += "where RecordID = @RecordID ";

            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);
            pars.Add("@Integration", balance);
            pars.Add("@LastConsumptionDate", Data.UpdateConsumptionDate ? Data.UpdateTime : member.LastConsumptionDate);
            pars.Add("@RecordID", Data.MemberID);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法更新資料列 (會員)，請聯絡系統管理員");
                result.Message = "更新成功";
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}