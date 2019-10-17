using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Dapper;
using PosSystem.CustomAuthentication;

namespace PosSystem.Models
{
    public class RepositoryMeal
    {
        private string sql = string.Empty;

        // 查詢
        public List<MealViewModel> Query()
        {
            List<MealViewModel> result = new List<MealViewModel>();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Meal] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MealViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 查詢
        public List<MealViewModel> Query(List<long> MealList)
        {
            List<MealViewModel> result = new List<MealViewModel>();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Meal] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID ";
            sql += string.Format("where A.RecordID in ({0})", string.Join(",", MealList));

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MealViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得資料 - By Primary Key
        public MealViewModel GetMealViewModel(Meal Data)
        {
            MealViewModel result = new MealViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Meal] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID where A.Name = @Name ";
            pars.Add("@Name", Data.Name);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MealViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得資料 - By RecordID
        public MealViewModel GetMealViewModel(long RecordID)
        {
            MealViewModel result = new MealViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[Meal] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID where A.RecordID = @RecordID ";
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MealViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得編輯資料
        public MealMD GetMealMD(long RecordID)
        {
            MealMD result = new MealMD();
            DynamicParameters pars = new DynamicParameters();

            sql = "select * from [SpadesKing].[dbo].[Meal] where RecordID = @RecordID ";
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<MealMD>(sql, pars).FirstOrDefault();
                    if (result != null) result.TypeOptions.Where(x => x.Value == result.Type).First().Selected = true;
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 建立
        public OperationResult<MealViewModel> Create(MealMD Data, AuthModel Operator)
        {
            OperationResult<MealViewModel> result = new OperationResult<MealViewModel>();
            DynamicParameters pars = new DynamicParameters();
            DateTime processTime = DateTime.Now;

            result.Type = "Create";

            sql = "insert into [SpadesKing].[dbo].[Meal] ";
            sql += "(Type, Name, Integration, UpdateTime, OperatorID) ";
            sql += "values(@Type, @Name, @Integration, @UpdateTime, @OperatorID) ";

            pars.Add("@Type", Data.Type);
            pars.Add("@Name", Data.Name);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@UpdateTime", processTime);
            pars.Add("@OperatorID", Operator.ID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法新增資料列 (餐點)，請聯絡系統管理員");
                    result.Message = "新增成功";
                    result.Data.Add(GetMealViewModel(new Meal { Name = Data.Name }));
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
        public OperationResult<MealViewModel> Update(MealMD Data, AuthModel Operator)
        {
            OperationResult<MealViewModel> result = new OperationResult<MealViewModel>();
            DynamicParameters pars = new DynamicParameters();
            DateTime processTime = DateTime.Now;

            result.Type = "Update";

            sql = "update [SpadesKing].[dbo].[Meal] set ";
            sql += "Type = @Type, Integration = @Integration, UpdateTime = @UpdateTime, OperatorID = @OperatorID ";
            sql += "where RecordID = @RecordID ";

            pars.Add("@Type", Data.Type);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@UpdateTime", processTime);
            pars.Add("@OperatorID", Operator.ID);
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法更新資料列 (餐點)，請聯絡系統管理員");
                    result.Message = "更新成功";
                    result.Data.Add(GetMealViewModel(Data.RecordID));
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
        public OperationResult<MealViewModel> Delete(MealMD Data)
        {
            OperationResult<MealViewModel> result = new OperationResult<MealViewModel>();
            DynamicParameters pars = new DynamicParameters();
            MealViewModel meal = GetMealViewModel(Data.RecordID);
            string administratorPwd = ConfigurationManager.AppSettings["AdministratorPwd"].Trim();

            // 驗證管理者密碼
            if (!Data.AdministratorPwd.Equals(administratorPwd))
            {
                result.Status = false;
                result.Message = "刪除失敗 : 管理者密碼錯誤";
                result.Data = null;

                return result;
            }

            // 驗證餐點資料
            if (meal == null)
            {
                result.Status = false;
                result.Message = "刪除失敗 : 系統錯誤，無法取得餐點資料";
                result.Data = null;

                return result;
            }
            
            result.Type = "Delete";

            sql = "delete from [SpadesKing].[dbo].[Meal] where RecordID = @RecordID ";
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("資料列無法刪除 (餐點)，請聯絡系統管理員");
                    result.Message = "刪除成功";
                    result.Data.Add(meal);
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