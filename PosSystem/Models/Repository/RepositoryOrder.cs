using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using PosSystem.CustomAuthentication;

namespace PosSystem.Models
{
    public class RepositoryOrder
    {
        private string sql = string.Empty;

        // 查詢
        public List<OrderViewModel> Query()
        {
            List<OrderViewModel> result = new List<OrderViewModel>();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Type MealType, B.Name MealName, B.Integration UnitIntegration from [SpadesKing].[dbo].[Order] A left join [SpadesKing].[dbo].[Meal] B on A.MealID = B.RecordID) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<OrderViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得資料 - By RecordID
        public OrderViewModel GetOrderViewModel(long RecordID)
        {
            OrderViewModel result = new OrderViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Type MealType, B.Name MealName, B.Integration UnitIntegration from [SpadesKing].[dbo].[Order] A left join [SpadesKing].[dbo].[Meal] B on A.MealID = B.RecordID) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";
            sql += "where AB.RecordID = @RecordID ";

            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<OrderViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得資料 - By OrderNo
        public List<OrderViewModel> GetOrderViewModel(string OrderNo)
        {
            List<OrderViewModel> result = new List<OrderViewModel>();
            DynamicParameters pars = new DynamicParameters();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Type MealType, B.Name MealName, B.Integration UnitIntegration from [SpadesKing].[dbo].[Order] A left join [SpadesKing].[dbo].[Meal] B on A.MealID = B.RecordID) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";
            sql += "where AB.OrderNo = @OrderNo ";

            pars.Add("@OrderNo", OrderNo);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<OrderViewModel>(sql, pars).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 新增
        public OperationResult Create(clsDBDapper DB, List<Order> Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();

            // 驗證餐點資訊
            if (Data == null || Data.Count <= 0)
            {
                result.Status = false;
                result.Message = "無點餐資訊";

                return result;
            }

            sql = "insert into [SpadesKing].[dbo].[Order] (OrderNo, MealID, Status, Quantity, UpdateTime, OperatorID) values ";

            for (int i = 0, len = Data.Count; i < len; i++)
            {
                string parMealID = string.Format("@parMealID_{0}", i);
                string parQuantity = string.Format("@parQuantity_{0}", i);

                sql += i == 0 ? string.Format("(@OrderNo, {0}, @Status, {1}, @UpdateTime, @OperatorID) ", parMealID, parQuantity) : string.Format(",(@OrderNo, {0}, @Status, {1}, @UpdateTime, @OperatorID) ", parMealID, parQuantity);

                pars.Add(parMealID, Data[i].MealID);
                pars.Add(parQuantity, Data[i].Quantity);
            }

            pars.Add("@OrderNo", Data.First().OrderNo);
            pars.Add("@Status", Data.First().Status);
            pars.Add("@UpdateTime", Data.First().UpdateTime);
            pars.Add("@OperatorID", Data.First().OperatorID);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法新增資料列 (點餐紀錄)，請聯絡系統管理員");
                result.Message = "新增成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        // 取消
        public OperationResult Cancel(clsDBDapper DB, Order Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();

            sql = "update [SpadesKing].[dbo].[Order] set Status = @Status, UpdateTime = @UpdateTime, OperatorID = @OperatorID where OrderNo = @OrderNo ";
            pars.Add("@Status", Data.Status);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);
            pars.Add("@OrderNo", Data.OrderNo);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法更新資料列 (點餐紀錄)，請聯絡系統管理員");
                result.Message = "更新成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        // 完成
        public OperationResult<OrderViewModel> Finish(long RecordID, AuthModel Operator)
        {
            OperationResult<OrderViewModel> result = new OperationResult<OrderViewModel>();
            DynamicParameters pars = new DynamicParameters();
            OrderViewModel order = GetOrderViewModel(RecordID);

            // 驗證餐點狀態
            if (order.Status != Order_Status.Preparing)
            {
                result.Status = false;
                result.Message = string.Format("操作失敗 : {0}", order.Status == Order_Status.Finished ? "餐點已完成" : "餐點已取消");
                result.Data = null;

                return result;
            }

            sql = "update [SpadesKing].[dbo].[Order] set Status = @Status, UpdateTime = @UpdateTime, OperatorID = @OperatorID where RecordID = @RecordID ";
            pars.Add("@Status", Order_Status.Finished);
            pars.Add("@UpdateTime", DateTime.Now);
            pars.Add("@OperatorID", Operator.ID);
            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result.Status = db.ToExecute(sql, pars);
                    if (!result.Status) throw new Exception("無法更新資料列 (點餐紀錄)，請聯絡系統管理員");
                    result.Message = "更新成功";
                    result.Data.Add(GetOrderViewModel(RecordID));
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
    }
}