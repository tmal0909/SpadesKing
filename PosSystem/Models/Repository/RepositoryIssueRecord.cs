using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace PosSystem.Models
{
    public class RepositoryIssueRecord
    {
        private string sql = string.Empty;

        // 查詢
        public List<IssueRecordViewModel> Query()
        {
            List<IssueRecordViewModel> result = new List<IssueRecordViewModel>();

            sql = "select A.*, B.Name OperatorName from [SpadesKing].[dbo].[IssueRecord] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<IssueRecordViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得總發行量
        public long GetDistribution()
        {
            List<IssueRecordViewModel> data = Query();
            long exportSum = data.Where(x => x.OperationType == IssueRecord_OperationType.Export).Sum(x => x.Integration);
            long importSum = data.Where(x => x.OperationType == IssueRecord_OperationType.Import).Sum(x => x.Integration);

            return exportSum - importSum;
        }

        // 建立
        public OperationResult Create(clsDBDapper DB, IssueRecord Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();

            sql = "insert into [SpadesKing].[dbo].[IssueRecord] (OperationType, Integration, Category, UpdateTime, OperatorID) ";
            sql += "values(@OperationType, @Integration, @Category, @UpdateTime, @OperatorID) ";

            pars.Add("@OperationType", Data.OperationType);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@Category", Data.Category);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法新增資料列 (發行紀錄)，請聯絡系統管理員");
                result.Message = "新增成功";
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