using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using PosSystem.CustomAuthentication;
using System.Configuration;

namespace PosSystem.Models
{
    public class RepositoryUser
    {
        private string sql = string.Empty;

        // 查詢
        public List<UserViewModel> Query()
        {
            List<UserViewModel> result = new List<UserViewModel>();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[User] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<UserViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得資料 - By Primary Key
        public UserViewModel GetUserViewModel(User Data)
        {
            UserViewModel result = new UserViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[User] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID where A.Phone = @Phone ";
            pars.Add("@Phone", Data.Phone);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<UserViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得資料 - By RecordID
        public UserViewModel GetUserViewModel(long RecordID)
        {
            UserViewModel result = new UserViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[User] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID where A.RecordID = @RecordID ";
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<UserViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得編輯資料
        public UserMD GetUserMD(long RecordID)
        {
            UserMD result = new UserMD();
            DynamicParameters pars = new DynamicParameters();

            sql = "select * from [SpadesKing].[dbo].[User] where RecordID = @RecordID ";
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<UserMD>(sql, pars).FirstOrDefault();
                    if (result != null) result.AuthOptions.Where(x => x.Value == result.AuthLevel).First().Selected = true;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 建立
        public OperationResult<UserViewModel> Create(UserMD Data, AuthModel Operator)
        {
            OperationResult<UserViewModel> result = new OperationResult<UserViewModel>();
            DynamicParameters pars = new DynamicParameters();
            DateTime processTime = DateTime.Now;

            result.Type = "Create";

            sql = "insert into [SpadesKing].[dbo].[User] ";
            sql += "(Name, IdentityNumber, BirthDay, Phone, Address, Email, LineID, WeChatID, UserPwd, AuthLevel, ConstructTime, UpdateTime, OperatorID) values ";
            sql += "(@Name, @IdentityNumber, @BirthDay, @Phone, @Address, @Email, @LineID, @WeChatID, @UserPwd, @AuthLevel, @ConstructTime, @UpdateTime, @OperatorID) ";

            pars.Add("@Name", Data.Name);
            pars.Add("@IdentityNumber", Data.IdentityNumber.ToUpper());
            pars.Add("@BirthDay", Data.BirthDay);
            pars.Add("@Phone", Data.Phone);
            pars.Add("@Address", Data.Address);
            pars.Add("@Email", Data.Email);
            pars.Add("@LineID", Data.LineID);
            pars.Add("@WeChatID", Data.WeChatID);
            pars.Add("@UserPwd", Data.UserPwd);
            pars.Add("@AuthLevel", Data.AuthLevel);
            pars.Add("@ConstructTime", processTime);
            pars.Add("@UpdateTime", processTime);
            pars.Add("@OperatorID", Operator.ID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法新增資料列 (員工)，請聯絡系統管理員");
                    result.Message = "新增成功";
                    result.Data.Add(GetUserViewModel(new User { Phone = Data.Phone }));
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
        public OperationResult<UserViewModel> Update(UserMD Data, AuthModel Operator, bool UpdateAuthLevel)
        {
            OperationResult<UserViewModel> result = new OperationResult<UserViewModel>();
            DynamicParameters pars = new DynamicParameters();
            DateTime processTime = DateTime.Now;

            result.Type = "Update";

            sql = "update [SpadesKing].[dbo].[User] set ";
            sql += "Name = @Name, IdentityNumber = @IdentityNumber, BirthDay = @BirthDay, Address = @Address, Email = @Email, ";
            sql += "LineID = @LineID, WeChatID = @WeChatID, UserPwd = @UserPwd, UpdateTime = @UpdateTime, OperatorID = @OperatorID ";
            sql += UpdateAuthLevel ? ",AuthLevel = @AuthLevel " : "";
            sql += "where RecordID = @RecordID ";

            pars.Add("@Name", Data.Name);
            pars.Add("@IdentityNumber", Data.IdentityNumber.ToUpper());
            pars.Add("@BirthDay", Data.BirthDay);
            pars.Add("@Address", Data.Address);
            pars.Add("@Email", Data.Email);
            pars.Add("@LineID", Data.LineID);
            pars.Add("@WeChatID", Data.WeChatID);
            pars.Add("@UserPwd", Data.UserPwd);
            pars.Add("@AuthLevel", Data.AuthLevel);
            pars.Add("@UpdateTime", processTime);
            pars.Add("@OperatorID", Operator.ID);
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法更新資料列 (員工)，請聯絡系統管理員");
                    result.Message = "更新成功";
                    result.Data.Add(GetUserViewModel(Data.RecordID));
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
        public OperationResult<UserViewModel> Delete(UserMD Data)
        {
            OperationResult<UserViewModel> result = new OperationResult<UserViewModel>();
            DynamicParameters pars = new DynamicParameters();
            UserViewModel user = GetUserViewModel(Data.RecordID);
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
            if (user == null)
            {
                result.Status = false;
                result.Message = "刪除失敗 : 系統錯誤，無法取得員工資料";
                result.Data = null;

                return result;
            }

            result.Type = "Delete";

            sql = "delete from [SpadesKing].[dbo].[User] where RecordID = @RecordID ";
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("資料列無法刪除 (員工)，請聯絡系統管理員");
                    result.Message = "刪除成功";
                    result.Data.Add(user);
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
    }
}