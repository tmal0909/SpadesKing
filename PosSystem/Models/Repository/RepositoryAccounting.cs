using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Dapper;
using PosSystem.CustomAuthentication;

namespace PosSystem.Models
{
    public class RepositoryAccounting
    {
        private string sql = string.Empty;

        // 查詢
        public List<AccountingViewModel> Query()
        {
            List<AccountingViewModel> result = new List<AccountingViewModel>();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Name MemberName from [SpadesKing].[dbo].[Accounting] A left join [SpadesKing].[dbo].[Member] B on A.MemberID = B.RecordID ) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<AccountingViewModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Clear();
            }

            return result;
        }

        // 取得統計資料
        public StatisticChartViewModel GetStatisticChartViewModel(DateTime DateStart, DateTime DateEnd)
        {
            StatisticChartViewModel result = new StatisticChartViewModel();
            List<AccountingViewModel> data = Query().Where(x => x.Status == Accounting_Status.Normal && x.UpdateTime > DateStart && x.UpdateTime < DateEnd.AddDays(1)).ToList();

            result.DateStart = DateStart.ToString("yyyy/MM/dd");
            result.DateEnd = DateEnd.ToString("yyyy/MM/dd");
            result.SumRent = data.Where(x => x.Type == Accounting_Type.Rent).Sum(x => x.Integration);
            result.SumOrder = data.Where(x => x.Type == Accounting_Type.Order && !x.PaymentType.Equals(Accounting_PaymentType.Treat)).Sum(x => x.Integration);
            result.SumTreat = data.Where(x => x.Type == Accounting_Type.Order && x.PaymentType.Equals(Accounting_PaymentType.Treat)).Sum(x => x.Integration);
            result.SumIncome = result.SumRent + result.SumOrder;

            return result;
        }

        // 取得帳務資料 - By All Columns
        public AccountingViewModel GetAccountingViewModel(Accounting Data)
        {
            AccountingViewModel result = new AccountingViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Name MemberName from [SpadesKing].[dbo].[Accounting] A left join [SpadesKing].[dbo].[Member] B on A.MemberID = B.RecordID ) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";
            sql += "where AB.Type = @Type and AB.PaymentType = @PaymentType and AB.Integration = @Integration and AB.Status = @Status and AB.UpdateTime = @UpdateTime and AB.OperatorID = @OperatorID and ";
            sql += Data.MemberID == null ? "AB.MemberID is null and " : "AB.MemberID = @MemberID and ";
            sql += Data.OrderNo == null ? "AB.OrderNo is null " : "AB.OrderNo = @OrderNo ";


            pars.Add("@Type", Data.Type);
            pars.Add("@PaymentType", Data.PaymentType);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@OrderNo", Data.OrderNo);
            pars.Add("@MemberID", Data.MemberID);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);
            pars.Add("@Status", Data.Status);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<AccountingViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得帳務資料 - By RecordID
        public AccountingViewModel GetAccountingViewModel(long RecordID)
        {
            AccountingViewModel result = new AccountingViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Name MemberName from [SpadesKing].[dbo].[Accounting] A left join [SpadesKing].[dbo].[Member] B on A.MemberID = B.RecordID ) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";
            sql += "where AB.RecordID = @RecordID ";

            pars.Add("@RecordID", RecordID);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<AccountingViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得帳務資料 - By OrderNo
        public AccountingViewModel GetAccountingViewModel(string OrderNo)
        {
            AccountingViewModel result = new AccountingViewModel();
            DynamicParameters pars = new DynamicParameters();

            sql = "select AB.*, C.Name OperatorName from ";
            sql += "(select A.*, B.Name MemberName from [SpadesKing].[dbo].[Accounting] A left join [SpadesKing].[dbo].[Member] B on A.MemberID = B.RecordID ) AB ";
            sql += "left join [SpadesKing].[dbo].[User] C on AB.OperatorID = C.RecordID ";
            sql += "where AB.OrderNo = @OrderNo ";

            pars.Add("@OrderNo", OrderNo);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<AccountingViewModel>(sql, pars).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得帳務點餐資料 - By OrderNo
        public List<AccountingOrderViewModel> GetAccountingOrderViewModel(string OrderNo)
        {
            List<AccountingOrderViewModel> result = new List<AccountingOrderViewModel>();
            DynamicParameters pars = new DynamicParameters();

            string sqlMealInfo = "(select A.RecordID OrderRecordID, A.OrderNo OrderOrderNo, A.MealID OrderMealID, B.Type OrderMealType, B.Name OrderMealName, B.Integration OrderUnitIntegration, A.Status OrderStatus, A.Quantity OrderQuantity, A.UpdateTime OrderUpdateTime, A.OperatorID OrderOperatorID from [SpadesKing].[dbo].[Order] A left join [SpadesKing].[dbo].[Meal] B on A.MealID = B.RecordID) ";
            string sqlOrderInfo = string.Format("(select MealInfo.*, B.Name OrderOperatorName from {0} MealInfo left join  [SpadesKing].[dbo].[User] B on MealInfo.OrderOperatorID = B.RecordID)  ", sqlMealInfo);
            string sqlAccountingInfo = "(select A.*, B.Name OperatorName from  [SpadesKing].[dbo].[Accounting] A left join [SpadesKing].[dbo].[User] B on A.OperatorID = B.RecordID) ";

            sql = string.Format("select * from {0} AccountingInfo left join {1} OrderInfo on AccountingInfo.OrderNo = OrderInfo.OrderOrderNo where AccountingInfo.OrderNo = @OrderNo  ", sqlAccountingInfo, sqlOrderInfo);
            pars.Add("@OrderNo", OrderNo);

            try
            {
                using (clsDBDapper db = new clsDBDapper())
                {
                    result = db.ToClass<AccountingOrderViewModel>(sql, pars).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        // 取得編輯資料 - 租金結帳
        public RentMD GetRentMD(long RecordID)
        {
            AccountingViewModel accounting = GetAccountingViewModel(RecordID);

            return new RentMD
            {
                RecordID = accounting.RecordID,
                Integration = accounting.Integration,
                Status = accounting.Status,
                UpdateTime = accounting.UpdateTime,
                OperatorName = accounting.OperatorName
            };
        }

        // 取得編輯資料 - 餐點結帳
        public OrderMD GetOrderMD(OrderMD Data)
        {
            OrderMD result = new OrderMD();
            RepositoryMeal repoMeal = new RepositoryMeal();
            List<MealViewModel> mealInfo = repoMeal.Query(Data.Order.Select(x => x.MealID).Distinct().ToList());

            // 設定付款方式
            switch (Data.PaymentType)
            {
                case "Cash":
                    result.PaymentType = Accounting_PaymentType.Cash;
                    break;

                case "Integration":
                    result.PaymentType = Accounting_PaymentType.Integration;
                    break;

                case "Member":
                    result.PaymentType = Accounting_PaymentType.Member;
                    break;

                case "Treat":
                    result.PaymentType = Accounting_PaymentType.Treat;
                    break;

                default:
                    return null;
            }

            // 設定餐點明細
            foreach (var meal in mealInfo)
            {
                OrderInfo info = new OrderInfo
                {
                    MealID = meal.RecordID,
                    MealName = meal.Name,
                    MealType = meal.Type,
                    Quantity = Data.Order.Where(x => x.MealID == meal.RecordID).Sum(x => x.Quantity),
                    Integration = meal.Integration,
                    Total = meal.Integration * Data.Order.Where(x => x.MealID == meal.RecordID).Sum(x => x.Quantity),
                    Status = Order_Status.Preparing
                };

                result.Order.Add(info);
            }

            return result;
        }

        // 取得編輯資料 - 餐點取消
        public OrderMD GetOrderMD(string OrderNo)
        {
            OrderMD result = new OrderMD();
            List<AccountingOrderViewModel> data = GetAccountingOrderViewModel(OrderNo);

            if (data == null) return null;

            result.OrderNo = data.First().OrderNo;
            result.PaymentType = data.First().PaymentType;
            result.Status = data.First().Status;
            result.UpdateTime = data.First().UpdateTime;
            result.Order = data.Select(x => new OrderInfo
            {
                MealID = Convert.ToInt64(x.OrderMealID),
                MealName = x.OrderMealName,
                MealType = x.OrderMealType,
                Quantity = Convert.ToInt32(x.OrderQuantity),
                Integration = Convert.ToInt64(x.OrderUnitIntegration),
                Total = Convert.ToInt64(x.OrderQuantity) * Convert.ToInt64(x.OrderUnitIntegration),
                Status = x.OrderStatus
            }).ToList();
            result.MemberID = data.First().PaymentType == Accounting_PaymentType.Member ? Convert.ToInt64(data.First().MemberID) : -1;

            return result;
        }

        // 租金結帳
        public OperationResult<AccountingViewModel> CheckRent(RentMD Data, AuthModel Operator)
        {
            OperationResult<AccountingViewModel> result = new OperationResult<AccountingViewModel>();
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            OperationResult resultIssueRecord, resultAccounting;
            DateTime processTime = DateTime.Now;

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Accounting_CheckRent_{0}", Operator.ID));

                    // 建立發行紀錄
                    IssueRecord dataIssueRecord = new IssueRecord
                    {
                        OperationType = IssueRecord_OperationType.Import,
                        Integration = Data.Integration,
                        Category = IssueRecord_Category.CheckRent,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultIssueRecord = repoIssueRecord.Create(db, dataIssueRecord);
                    if (!resultIssueRecord.Status) throw new Exception(resultIssueRecord.Message);

                    // 建立帳務紀錄
                    Accounting dataAccounting = new Accounting
                    {
                        Type = Accounting_Type.Rent,
                        PaymentType = Accounting_PaymentType.Integration,
                        Integration = Data.Integration,
                        Status = Accounting_Status.Normal,
                        OrderNo = null,
                        MemberID = null,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultAccounting = Create(db, dataAccounting);
                    if (!resultAccounting.Status) throw new Exception(resultAccounting.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "CheckRent";
                    result.Status = true;
                    result.Message = "結帳成功";
                    result.Data.Add(GetAccountingViewModel(dataAccounting));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "結帳失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }

        // 租金取消
        public OperationResult<AccountingViewModel> CancelRent(RentMD Data, AuthModel Operator)
        {
            OperationResult<AccountingViewModel> result = new OperationResult<AccountingViewModel>();
            RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
            OperationResult resultIssueRecord, resultAccounting;
            DateTime processTime = DateTime.Now;
            AccountingViewModel accounting = GetAccountingViewModel(Data.RecordID);

            // 驗證帳務紀錄
            if (accounting == null)
            {
                result.Status = false;
                result.Message = "取消失敗 : 系統錯誤，無法取得帳務紀錄";
                result.Data = null;

                return result;
            }

            // 驗證刪除狀態
            if (accounting.Status.Equals(Accounting_Status.Cancel))
            {
                result.Status = false;
                result.Message = "取消失敗 : 帳務紀錄已取消";
                result.Data = null;

                return result;
            }

            // 驗證管理者密碼
            if (!Data.AdministratorPwd.Equals(ConfigurationManager.AppSettings["AdministratorPwd"].Trim()))
            {
                result.Status = false;
                result.Message = "取消失敗 : 管理者密碼錯誤";
                result.Data = null;

                return result;
            }

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Accounting_CancelRent_{0}", Operator.ID));

                    // 建立發行紀錄
                    IssueRecord dataIssueRecord = new IssueRecord
                    {
                        OperationType = IssueRecord_OperationType.Export,
                        Integration = accounting.Integration,
                        Category = IssueRecord_Category.CancelRent,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultIssueRecord = repoIssueRecord.Create(db, dataIssueRecord);
                    if (!resultIssueRecord.Status) throw new Exception(resultIssueRecord.Message);

                    // 取消帳務紀錄
                    Accounting dataAccounting = new Accounting
                    {
                        RecordID = accounting.RecordID,
                        Type = accounting.Type,
                        PaymentType = accounting.PaymentType,
                        Integration = accounting.Integration,
                        Status = Accounting_Status.Cancel,
                        OrderNo = accounting.OrderNo,
                        MemberID = accounting.MemberID,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    resultAccounting = Cancel(db, dataAccounting);
                    if (!resultAccounting.Status) throw new Exception(resultAccounting.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "CancelRent";
                    result.Status = true;
                    result.Message = "取消成功";
                    result.Data.Add(GetAccountingViewModel(dataAccounting));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "取消失敗 : " + ex.Message;
                    result.Data = null;
                }
            }

            return result;
        }

        // 餐點結帳
        public OperationResult<AccountingViewModel> CheckOrder(OrderMD Data, AuthModel Operator)
        {
            OperationResult<AccountingViewModel> result = new OperationResult<AccountingViewModel>();
            DateTime processTime = DateTime.Now;
            string orderNo = string.Format("{0}{1}", processTime.ToString("yyyyMMddHHmmss"), new Random().Next(100, 999));
            long? integrationSum = Data.Order.Sum(x => x.Total);
            long? memberID = null;

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Accounting_CheckOrder_{0}", Operator.ID));

                    // 更新會員 & 操作紀錄
                    if (Data.PaymentType == Accounting_PaymentType.Member)
                    {
                        // 更新會員積分
                        RepositoryMember repoMember = new RepositoryMember();
                        OperationResult resultMember = repoMember.UpdateBalance(db, new BalanceOperationInfo
                        {
                            MemberID = Convert.ToInt64(Data.MemberID),
                            MemberPwd = Data.MemberPwd,
                            Integration = Convert.ToInt64(integrationSum),
                            UpdateTime = processTime,
                            OperatorID = Operator.ID,
                            OperationType = BalanceOperationInfo_OperationType.Export,
                            ValidationType = BalanceOperationInfo_ValidationType.Force,
                            UpdateConsumptionDate = true
                        });
                        if (!resultMember.Status) throw new Exception(resultMember.Message);

                        // 更新積分操作紀錄
                        RepositoryOperationRecord repoOperationRecord = new RepositoryOperationRecord();
                        OperationResult resultOperationRecord = repoOperationRecord.Create(db, new OperationRecord
                        {
                            MemberID = Convert.ToInt64(Data.MemberID),
                            OperationType = OperationRecord_OperationType.CheckOrder,
                            Integration = Convert.ToInt64(integrationSum),
                            Note = null,
                            UpdateTime = processTime,
                            OperatorID = Operator.ID
                        });
                        if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);

                        memberID = Data.MemberID;
                    }

                    // 建立發行紀錄
                    if (Data.PaymentType == Accounting_PaymentType.Member || Data.PaymentType == Accounting_PaymentType.Integration)
                    {
                        RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
                        OperationResult resultIssueRecord = repoIssueRecord.Create(db, new IssueRecord
                        {
                            OperationType = IssueRecord_OperationType.Import,
                            Integration = Convert.ToInt64(integrationSum),
                            Category = IssueRecord_Category.CheckOrder,
                            UpdateTime = processTime,
                            OperatorID = Operator.ID
                        });
                        if (!resultIssueRecord.Status) throw new Exception(resultIssueRecord.Message);
                    }

                    // 建立點餐資訊
                    RepositoryOrder repoOrder = new RepositoryOrder();
                    List<Order> dataOrder = Data.Order.Select(x => new Order
                    {
                        OrderNo = orderNo,
                        MealID = x.MealID,
                        Status = Order_Status.Preparing,
                        Quantity = x.Quantity,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    }).ToList();
                    OperationResult resultOrder = repoOrder.Create(db, dataOrder);
                    if (!resultOrder.Status) throw new Exception(resultOrder.Message);

                    // 建立帳務資訊
                    Accounting dataAccounting = new Accounting
                    {
                        Type = Accounting_Type.Order,
                        PaymentType = Data.PaymentType,
                        Integration = Convert.ToInt64(integrationSum),
                        Status = Accounting_Status.Normal,
                        OrderNo = orderNo,
                        MemberID = memberID,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    };
                    OperationResult resultAccounting = Create(db, dataAccounting);
                    if (!resultAccounting.Status) throw new Exception(resultAccounting.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "CheckOrder";
                    result.Status = true;
                    result.Message = "結帳成功";
                    result.Data.Add(GetAccountingViewModel(dataAccounting));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "結帳失敗 : " + ex.Message;
                }
            }

            return result;
        }

        // 餐點取消
        public OperationResult<OrderViewModel> CancelOrder(OrderMD Data, AuthModel Operator)
        {
            OperationResult<OrderViewModel> result = new OperationResult<OrderViewModel>();
            AccountingViewModel accounting = GetAccountingViewModel(Data.OrderNo);
            DateTime processTime = DateTime.Now;

            // 驗證帳務紀錄
            if (accounting == null)
            {
                result.Status = false;
                result.Message = "取消失敗 : 系統錯誤，無法取得帳務紀錄";
                result.Data = null;

                return result;
            }

            // 驗證刪除狀態
            if (accounting.Status.Equals(Accounting_Status.Cancel))
            {
                result.Status = false;
                result.Message = "取消失敗 : 帳務紀錄已取消";
                result.Data = null;

                return result;
            }

            // 驗證管理者密碼
            if (!Data.AdministratorPwd.Equals(ConfigurationManager.AppSettings["AdministratorPwd"].Trim()))
            {
                result.Status = false;
                result.Message = "取消失敗 : 管理者密碼錯誤";
                result.Data = null;

                return result;
            }

            using (clsDBDapper db = new clsDBDapper())
            {
                try
                {
                    // 開啟交易
                    db.TransactionStart(string.Format("Accounting_CancelOrder_{0}", Operator.ID));

                    // 會員結帳 - 更新會員積分資訊 & 建立操作紀錄
                    if (accounting.PaymentType == Accounting_PaymentType.Member)
                    {
                        // 更新會員積分資訊
                        RepositoryMember repoMember = new RepositoryMember();
                        OperationResult resultMember = repoMember.UpdateBalance(db, new BalanceOperationInfo
                        {
                            MemberID = Convert.ToInt64(accounting.MemberID),
                            MemberPwd = null,
                            Integration = accounting.Integration,
                            UpdateTime = processTime,
                            OperatorID = Operator.ID,
                            OperationType = BalanceOperationInfo_OperationType.Import,
                            ValidationType = BalanceOperationInfo_ValidationType.None,
                            UpdateConsumptionDate = true
                        });
                        if (!resultMember.Status) throw new Exception(resultMember.Message);

                        // 建立積分操作紀錄
                        RepositoryOperationRecord repoOperationRecord = new RepositoryOperationRecord();
                        OperationResult resultOperationRecord = repoOperationRecord.Create(db, new OperationRecord
                        {
                            MemberID = Convert.ToInt64(accounting.MemberID),
                            OperationType = OperationRecord_OperationType.CancelOrder,
                            Integration = accounting.Integration,
                            Note = null,
                            UpdateTime = processTime,
                            OperatorID = Operator.ID
                        });
                        if (!resultOperationRecord.Status) throw new Exception(resultOperationRecord.Message);
                    }

                    // 積分結帳
                    if (accounting.PaymentType == Accounting_PaymentType.Member || accounting.PaymentType == Accounting_PaymentType.Integration)
                    {
                        // 建立積分發行紀錄
                        RepositoryIssueRecord repoIssueRecord = new RepositoryIssueRecord();
                        OperationResult resultIssueRecord = repoIssueRecord.Create(db, new IssueRecord
                        {
                            OperationType = IssueRecord_OperationType.Export,
                            Integration = accounting.Integration,
                            Category = IssueRecord_Category.CancelOrder,
                            UpdateTime = processTime,
                            OperatorID = Operator.ID
                        });
                        if (!resultIssueRecord.Status) throw new Exception(resultIssueRecord.Message);
                    }

                    // 更新點餐紀錄
                    RepositoryOrder repoOrder = new RepositoryOrder();
                    OperationResult resultOrder = repoOrder.Cancel(db, new Order
                    {
                        OrderNo = accounting.OrderNo,
                        Status = Order_Status.Canceled,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID
                    });
                    if (!resultOrder.Status) throw new Exception(resultOrder.Message);

                    // 更新帳務紀錄
                    OperationResult resultAccounting = Cancel(db, new Accounting
                    {
                        Status = Accounting_Status.Cancel,
                        UpdateTime = processTime,
                        OperatorID = Operator.ID,
                        RecordID = accounting.RecordID
                    });
                    if (!resultAccounting.Status) throw new Exception(resultAccounting.Message);

                    // 確認及關閉交易
                    db.TransactionCommit();
                    db.TransactionDispose();

                    // 製作回傳結果
                    result.Type = "CancelOrder";
                    result.Status = true;
                    result.Message = "取消成功";
                    result.Data.AddRange(repoOrder.GetOrderViewModel(accounting.OrderNo));
                }
                catch (Exception ex)
                {
                    // 回復資料
                    db.TransactionRollBack();

                    result.Status = false;
                    result.Message = "取消失敗 : " + ex.Message;
                }
            }

            return result;
        }

        // 建立
        public OperationResult Create(clsDBDapper DB, Accounting Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();

            sql = "insert into [SpadesKing].[dbo].[Accounting] (Type, PaymentType, Integration, Status, OrderNo, MemberID, UpdateTime, OperatorID) ";
            sql += "values(@Type, @PaymentType, @Integration, @Status, @OrderNo, @MemberID, @UpdateTime, @OperatorID) ";

            pars.Add("@Type", Data.Type);
            pars.Add("@PaymentType", Data.PaymentType);
            pars.Add("@Integration", Data.Integration);
            pars.Add("@Status", Data.Status);
            pars.Add("@OrderNo", Data.OrderNo);
            pars.Add("@MemberID", Data.MemberID);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法新增資料列 (帳務紀錄)，請聯絡系統管理員");
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
        public OperationResult Cancel(clsDBDapper DB, Accounting Data)
        {
            OperationResult result = new OperationResult();
            DynamicParameters pars = new DynamicParameters();

            sql = "update [SpadesKing].[dbo].[Accounting] set Status = @Status, UpdateTime = @UpdateTime, OperatorID = @OperatorID where RecordID = @RecordID ";

            pars.Add("@Status", Data.Status);
            pars.Add("@UpdateTime", Data.UpdateTime);
            pars.Add("@OperatorID", Data.OperatorID);
            pars.Add("@RecordID", Data.RecordID);

            try
            {
                result.Status = DB.ToExecuteWithTran(sql, pars);
                if (!result.Status) throw new Exception("無法更新資料列 (帳務紀錄)，請聯絡系統管理員");
                result.Message = "更新成功";
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