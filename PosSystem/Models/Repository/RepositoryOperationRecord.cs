using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Dapper;
using PosSystem.CustomAuthentication;

namespace PosSystem.Models
{
    public class RepositoryOperationRecord
    {
        private string sql = string.Empty;

        // 查詢
        public List<OperationRecordViewModel> Query()
        {
            List<OperationRecordViewModel> result = new List<OperationRecordViewModel>();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Name MemberName from [SpadesKing].[dbo].[OperationRecord] A left join [SpadesKing].[dbo].[Member] B on A.MemberID = B.RecordID ) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<OperationRecordViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得資料 - By All Columns
        public OperationRecordViewModel GetOperationRecordViewModel(OperationRecord Data)
        {
            OperationRecordViewModel result = new OperationRecordViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Name MemberName from [SpadesKing].[dbo].[OperationRecord] A left join [SpadesKing].[dbo].[Member] B on A.MemberID = B.RecordID ) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";
            sql += "where AB.MemberID = @MemberID and AB.OperationType = @OperationType and AB.Integration = @Integration and AB.UpdateTime = @UpdateTime and AB.OperatorID = @OperatorID and ";
            sql += string.IsNullOrEmpty(Data.Note) ? "AB.Note is null " : " AB.Note = @Note";

            pars.Add("@MemberID", Data.MemberID);
            pars.Add("@OperationType", Data.OperationType);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);
            pars.Add("@Note", Data.Note);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<OperationRecordViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 購買
        public OperationResult<OperationRecordViewModel> Purchase(OperationMD Data, AuthModel Operator)
        {
            OperationResult<OperationRecordViewModel> result = new OperationResult<OperationRecordViewModel>();
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            RepositoryMember repoMember = new RepositoryMember();
            DateTime processTime = DateTime.Now;
            OperationResult resultIssueRecord, resultMember, resultOperationRecord;

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Operation_Purchase_{0}_{1}", Data.MemberID, Operator.ID));

                    // 建立發行紀錄
                    IssueRecord dataIssueRecord = new IssueRecord
                    {
                        OperationType = IssueRecord_OperationType.Export,
                        Integration = Data.Integration,
                        Category = IssueRecord_Category.Purchase,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultIssueRecord = repoIssueRecord.Create(db, dataIssueRecord);
                    if (!resultIssueRecord.Status) throw new Exception(resultIssueRecord.Message);

                    // 更新會員餘額
                    BalanceOperationInfo dataBalaceOperationInfo = new BalanceOperationInfo
                    {
                        MemberID = Data.MemberID,
                        MemberPwd = string.Empty,
                        Integration = Data.Integration,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        OperationType = BalanceOperationInfo_OperationType.Import,
                        ValidationType = BalanceOperationInfo_ValidationType.None,
                        UpdateConsumptionDate = true
                    };
                    resultMember = repoMember.UpdateBalance(db, dataBalaceOperationInfo);
                    if (!resultMember.Status) throw new Exception(resultMember.Message);

                    // 更新操作紀錄
                    OperationRecord dataOperationRecord = new OperationRecord
                    {
                        MemberID = Data.MemberID,
                        OperationType = OperationRecord_OperationType.Purchase,
                        Integration = Data.Integration,
                        Note = null,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultOperationRecord = Create(db, dataOperationRecord);
                    if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "Purchase";
                    result.Status = true;
                    result.Message = "購買成功";
                    result.Data.Add(GetOperationRecordViewModel(dataOperationRecord));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "購買失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }

        // 提領
        public OperationResult<OperationRecordViewModel> Withdraw(OperationMD Data, AuthModel Operator)
        {
            OperationResult<OperationRecordViewModel> result = new OperationResult<OperationRecordViewModel>();
            RepositoryMember repoMember = new RepositoryMember();
            DateTime processTime = DateTime.Now;
            OperationResult resultMember, resultOperationRecord;

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Operation_Withdraw_{0}_{1}", Data.MemberID, Operator.ID));

                    // 更新會員餘額
                    BalanceOperationInfo dataBalaceOperationInfo = new BalanceOperationInfo
                    {
                        MemberID = Data.MemberID,
                        MemberPwd = Data.MemberPwd,
                        Integration = Data.Integration,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        OperationType = BalanceOperationInfo_OperationType.Export,
                        ValidationType = BalanceOperationInfo_ValidationType.Auto,
                        UpdateConsumptionDate = true
                    };
                    resultMember = repoMember.UpdateBalance(db, dataBalaceOperationInfo);
                    if (!resultMember.Status) throw new Exception(resultMember.Message);

                    // 更新操作紀錄
                    OperationRecord dataOperationRecord = new OperationRecord
                    {
                        MemberID = Data.MemberID,
                        OperationType = OperationRecord_OperationType.Withdraw,
                        Integration = Data.Integration,
                        Note = null,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultOperationRecord = Create(db, dataOperationRecord);
                    if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "Withdraw";
                    result.Status = true;
                    result.Message = "提領成功";
                    result.Data.Add(GetOperationRecordViewModel(dataOperationRecord));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "提領失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }

        // 回存
        public OperationResult<OperationRecordViewModel> Deposit(OperationMD Data, AuthModel Operator)
        {
            OperationResult<OperationRecordViewModel> result = new OperationResult<OperationRecordViewModel>();
            RepositoryMember repoMember = new RepositoryMember();
            DateTime processTime = DateTime.Now;
            OperationResult resultMember, resultOperationRecord;

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Operation_Deposit_{0}_{1}", Data.MemberID, Operator.ID));

                    // 更新會員餘額
                    BalanceOperationInfo dataBalaceOperationInfo = new BalanceOperationInfo
                    {
                        MemberID = Data.MemberID,
                        MemberPwd = string.Empty,
                        Integration = Data.Integration,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        OperationType = BalanceOperationInfo_OperationType.Import,
                        ValidationType = BalanceOperationInfo_ValidationType.None,
                        UpdateConsumptionDate = true
                    };
                    resultMember = repoMember.UpdateBalance(db, dataBalaceOperationInfo);
                    if (!resultMember.Status) throw new Exception(resultMember.Message);

                    // 更新操作紀錄
                    OperationRecord dataOperationRecord = new OperationRecord
                    {
                        MemberID = Data.MemberID,
                        OperationType = OperationRecord_OperationType.Deposit,
                        Integration = Data.Integration,
                        Note = null,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultOperationRecord = Create(db, dataOperationRecord);
                    if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "Deposit";
                    result.Status = true;
                    result.Message = "回存成功";
                    result.Data.Add(GetOperationRecordViewModel(dataOperationRecord));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "回存失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }

        // 轉移
        public OperationResult<OperationRecordViewModel> Transfer(OperationMD Data, AuthModel Operator)
        {
            OperationResult<OperationRecordViewModel> result = new OperationResult<OperationRecordViewModel>();
            RepositoryMember repoMember = new RepositoryMember();
            DateTime processTime = DateTime.Now;
            OperationResult resultMemberExport, resultMemberImport, resultOperationRecord;
            MemberViewModel memberImport = repoMember.GetMemberViewModel(Data.TargetMemberID);

            // 驗證管理者密碼
            if (!Data.AdministratorPwd.Equals(ConfigurationManager.AppSettings["AdministratorPwd"].Trim()))
            {
                result.Status = false;
                result.Message = "轉移失敗 : 管理者密碼錯誤";
                result.Data = null;

                return result;
            }

            // 驗證轉入與轉出會員
            if (Data.MemberID == Data.TargetMemberID)
            {
                result.Status = false;
                result.Message = "轉移失敗 : 轉入會員與轉出會員相同";
                result.Data = null;

                return result;
            }

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Operation_Transfer_{0}_{1}", Data.MemberID, Operator.ID));

                    // 更新會員餘額 (轉出)
                    BalanceOperationInfo dataBalaceOperationInfoExport = new BalanceOperationInfo
                    {
                        MemberID = Data.MemberID,
                        MemberPwd = Data.MemberPwd,
                        Integration = Data.Integration,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        OperationType = BalanceOperationInfo_OperationType.Export,
                        ValidationType = BalanceOperationInfo_ValidationType.Force,
                        UpdateConsumptionDate = true
                    };
                    resultMemberExport = repoMember.UpdateBalance(db, dataBalaceOperationInfoExport);
                    if (!resultMemberExport.Status) throw new Exception(resultMemberExport.Message);

                    // 更新會員餘額 (轉入)
                    BalanceOperationInfo dataBalaceOperationInfoImport = new BalanceOperationInfo
                    {
                        MemberID = Data.TargetMemberID,
                        MemberPwd = string.Empty,
                        Integration = Data.Integration,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        OperationType = BalanceOperationInfo_OperationType.Import,
                        ValidationType = BalanceOperationInfo_ValidationType.None,
                        UpdateConsumptionDate = false
                    };
                    resultMemberImport = repoMember.UpdateBalance(db, dataBalaceOperationInfoImport);
                    if (!resultMemberImport.Status) throw new Exception(resultMemberImport.Message);

                    // 更新操作紀錄
                    OperationRecord dataOperationRecord = new OperationRecord
                    {
                        MemberID = Data.MemberID,
                        OperationType = OperationRecord_OperationType.Transfer,
                        Integration = Data.Integration,
                        Note = memberImport.Name,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultOperationRecord = Create(db, dataOperationRecord);
                    if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "Transfer";
                    result.Status = true;
                    result.Message = "轉移成功";
                    result.Data.Add(GetOperationRecordViewModel(dataOperationRecord));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "轉移失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }

        // 修正
        public OperationResult<OperationRecordViewModel> Modify(OperationMD Data, AuthModel Operator)
        {
            OperationResult<OperationRecordViewModel> result = new OperationResult<OperationRecordViewModel>();
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            RepositoryMember repoMember = new RepositoryMember();
            DateTime processTime = DateTime.Now;
            OperationResult resultIssueRecord, resultMember, resultOperationRecord;

            // 驗證管理者密碼
            if (!Data.AdministratorPwd.Equals(ConfigurationManager.AppSettings["AdministratorPwd"].Trim()))
            {
                result.Status = false;
                result.Message = "修正失敗 : 管理者密碼錯誤";
                result.Data = null;

                return result;
            }

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Operation_Modify_{0}_{1}", Data.MemberID, Operator.ID));

                    // 建立發行紀錄
                    IssueRecord dataIssueRecord = new IssueRecord
                    {
                        OperationType = IssueRecord_OperationType.Import,
                        Integration = Data.Integration,
                        Category = IssueRecord_Category.Modify,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultIssueRecord = repoIssueRecord.Create(db, dataIssueRecord);
                    if (!resultIssueRecord.Status) throw new Exception(resultIssueRecord.Message);

                    // 更新會員餘額
                    BalanceOperationInfo dataBalaceOperationInfo = new BalanceOperationInfo
                    {
                        MemberID = Data.MemberID,
                        MemberPwd = string.Empty,
                        Integration = Data.Integration,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        OperationType = BalanceOperationInfo_OperationType.Export,
                        ValidationType = BalanceOperationInfo_ValidationType.None,
                        UpdateConsumptionDate = true
                    };
                    resultMember = repoMember.UpdateBalance(db, dataBalaceOperationInfo);
                    if (!resultMember.Status) throw new Exception(resultMember.Message);

                    // 更新操作紀錄
                    OperationRecord dataOperationRecord = new OperationRecord
                    {
                        MemberID = Data.MemberID,
                        OperationType = OperationRecord_OperationType.Modify,
                        Integration = Data.Integration,
                        Note = null,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultOperationRecord = Create(db, dataOperationRecord);
                    if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "Modify";
                    result.Status = true;
                    result.Message = "修改成功";
                    result.Data.Add(GetOperationRecordViewModel(dataOperationRecord));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "修改失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }
        
        // 建立
        public OperationResult Create(clsDBDapper DB, OperationRecord Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();

            sql = "insert into [SpadesKing].[dbo].[OperationRecord] ";
            sql += "(MemberID, OperationType, Integration, Note, UpdateTime, OperatorID) ";
            sql += "values(@MemberID, @OperationType, @Integration, @Note, @UpdateTime, @OperatorID) ";

            pars.Add("@MemberID", Data.MemberID);
            pars.Add("@OperationType", Data.OperationType);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@Note", Data.Note);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法新增資料列 (操作紀錄)，請聯絡系統管理員");
                result.Message = "新增成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}