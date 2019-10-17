using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// 需安裝套件:Dapper,若不想安裝可用 clsDB
/// </summary>
public class clsDBDapper : IDisposable
{

    private SqlConnection objConn;
    private SqlTransaction objTran;

    public enum ConnStrNameEnum { DBConnection, DBConnTest }


    /// <summary>
    /// 建構元
    /// </summary>
    public clsDBDapper()
    {
        objConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStrNameEnum.DBConnection.ToString()].ToString());

    }


    /// <summary>
    /// 建構元
    /// </summary>
    public clsDBDapper(ConnStrNameEnum ConnStrName)
    {
        objConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStrName.ToString()].ToString());
    }


    /// <summary>
    /// 解構元
    /// </summary>
    public void Dispose()
    {
        this.ToConnClose();
    }


    /// <summary>
    /// 開啟連線
    /// </summary>
    public bool ToConnOpen()
    {
        bool result = true;

        try
        {
            if (objConn.State != ConnectionState.Open)
            {
                objConn.Open();
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }


    /// <summary>
    /// 關閉連線
    /// </summary>
    public bool ToConnClose()
    {
        bool result = true;

        try
        {
            if (objTran != null)
            {
                objTran.Dispose();
                objTran = null;
            }

            if (objConn.State != ConnectionState.Closed)
            {
                objConn.Close();
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }


    /// <summary>
    /// 回傳指定類別 -- 需傳入SQL參數
    /// </summary>
    public IList<T> ToClass<T>(string sql, DynamicParameters Params) where T : new()
    {
        if (string.IsNullOrEmpty(sql) == true)
        {
            return null;
        }
        if (Params != null)
        {
            var _result = this.objConn.Query<T>(sql, Params).ToList();
            return _result;
        }
        else
        {
            var _result = this.objConn.Query<T>(sql).ToList();
            return _result;
        }

    }



    public IList<T> ToClassByObj<T>(string sql, object Params) where T : new()
    {
        if (string.IsNullOrEmpty(sql) == true)
        {
            return null;
        }
        if (Params != null)
        {
            var _result = this.objConn.Query<T>(sql, Params).ToList();
            return _result;
        }
        else
        {
            var _result = this.objConn.Query<T>(sql).ToList();
            return _result;
        }

    }


    /// <summary>
    /// 回傳指定類別
    /// </summary>
    public IList<T> ToClass<T>(string sql) where T : new()
    {
        return this.ToClass<T>(sql, null);
    }


    /// <summary>
    /// 執行指定的SQL語法
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="Params"></param>
    /// <returns></returns>
    public bool ToExecute(string sql, DynamicParameters Params)
    {
        bool _flag = false;
        int _result = 0;
        if (string.IsNullOrEmpty(sql) == true)
        {

        }
        if (Params != null)
        {
            _result = this.objConn.Execute(sql, Params);
        }
        else
        {
            _result = this.objConn.Execute(sql, Params);
        }
        _flag = (_result == 0) ? false : true;
        return _flag;
    }


    public bool ToExecuteAllowNone(string sql, DynamicParameters Params, bool AllowNone)
    {
        bool _flag = false;

        int _result = 0;

        if (string.IsNullOrEmpty(sql) == true)
        {

        }

        if (Params != null)
        {
            _result = this.objConn.Execute(sql, Params);
        }
        else
        {
            _result = this.objConn.Execute(sql, Params);
        }

        if (AllowNone)
        {
            return _result >= 0 ? true : false;
        }

        return _result == 0 ? false : true;
    }


    public bool ToExecute(string sql, object Params)
    {
        bool _flag = false;
        int _result = 0;
        if (string.IsNullOrEmpty(sql) == true)
        {

        }
        if (Params != null)
        {
            _result = this.objConn.Execute(sql, Params);
        }
        else
        {
            _result = this.objConn.Execute(sql, Params);
        }
        _flag = (_result == 0) ? false : true;
        return _flag;
    }


    public dynamic ToObj(string sql, DynamicParameters Params)
    {
        var _result = this.objConn.Query(sql, Params);
        return _result;
    }


    /// <summary>
    /// 建立交易
    /// </summary>
    public void TransactionStart(string TransactionName)
    {
        if (objConn.State != ConnectionState.Open)
        {
            objConn.Open();
        }

        if (objTran != null)
        {
            objTran.Dispose();
            objTran = null;
        }

        objTran = objConn.BeginTransaction(TransactionName);
    }


    /// <summary>
    /// 確認交易
    /// </summary>
    public void TransactionCommit()
    {
        if (objTran != null && objTran.Connection != null)
        {
            objTran.Commit();
            objTran.Dispose();
            objTran = null;
        }
    }


    /// <summary>
    /// 返回交易
    /// </summary>
    public void TransactionRollBack()
    {
        if (objTran != null && objTran.Connection != null)
        {
            objTran.Rollback();
            objTran.Dispose();
            objTran = null;
        }
    }


    /// <summary>
    /// 釋放交易
    /// </summary>
    public void TransactionDispose()
    {
        if (objTran != null)
        {
            objTran.Dispose();
            objTran = null;
        }
    }


    /// <summary>
    /// 執行指定的 transaction sql
    /// </summary>
    public bool ToExecuteWithTran(string sql, DynamicParameters Params = null, bool allowNone = false)
    {
        int _result = 0;

        if (string.IsNullOrEmpty(sql) == true)
        {

        }

        if (objTran == null || objTran.Connection == null) return false;

        if (Params != null)
        {
            _result = this.objConn.Execute(sql, Params, objTran);
        }
        else
        {
            _result = this.objConn.Execute(sql, null, objTran);
        }

        if (allowNone)
        {
            return _result >= 0 ? true : false;
        }

        return _result == 0 ? false : true;
    }
}