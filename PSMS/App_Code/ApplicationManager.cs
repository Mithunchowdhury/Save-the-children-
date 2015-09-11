
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Text;
using System.ComponentModel;
using System.DirectoryServices;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PSMS;
using System.IO;


/// <summary>
/// AppManager (Application Framework API) 
/// API Name    : AppManager
/// Create Date : 17-JUN-2015
/// Author      : Rokybul Imrose, Manager - ICT Application Management
///               Shyamal Kumar Mondal, Sr. Officer - Application Development
///               Mohammed Nasir Ahmed, Analyst Programmer, Star Computer Systems Ltd.
///               Mohammod Hasan Imam, Sr. Software Engineer, Star Computer Systems Ltd.
/// Description : Helper Framework for Data Access, MAIL, ETC
/// </summary>

public enum errorType
{
    ConnectionError = 0,
    DataOperationError = 1,
    SendMailError = 2,
    ReportError = 3,
    LoginError = 4,
    PermissionError = 5
};

public enum dbPermission
{
    Insert = 0,
    Delete = 1,
    Update = 2
};

public static class ErrorNumber
{
    public static string PermissionDenied = "1000050";
    public static string DataOperationError = "1000051";
    public static string SetType(string ErrorCode, errorType eType)
    {
        string sRet = "";
        if (eType == errorType.ConnectionError) sRet = ErrorCode + " (Connection Error)";
        if (eType == errorType.DataOperationError) sRet = ErrorCode + " (Data Operation Error)";
        if (eType == errorType.SendMailError) sRet = ErrorCode + " (Send Mail Error)";
        if (eType == errorType.ReportError) sRet = ErrorCode + " (Report Error)";
        if (eType == errorType.LoginError) sRet = ErrorCode + " (Login Error)";
        if (eType == errorType.PermissionError) sRet = ErrorCode + " (Permission Error)";
        return sRet;
    }
}

public static class dbINFO
{
    public static string _SRV
    {
        get
        {
            if (ConfigurationManager.AppSettings["SERVERNAME"] != null)
                return ConfigurationManager.AppSettings["SERVERNAME"].ToString();
            return "10.12.1.2";
        }
    } //204.93.174.60ROCKSOFT\\SQLEXPRESS
    public static string _CAT
    {
        get
        {
            if (ConfigurationManager.AppSettings["DBNAME"] != null)
                return ConfigurationManager.AppSettings["DBNAME"].ToString();
            return "psms";
        }
    } //kazol201_rms
    public static string _UID
    {
        get
        {
            if (ConfigurationManager.AppSettings["UID"] != null)
                return ConfigurationManager.AppSettings["UID"].ToString();
            return "psms";
        }
    } //kazol201_sa
    public static string _PWD
    {
        get
        {
            if (ConfigurationManager.AppSettings["PWD"] != null)
                return ConfigurationManager.AppSettings["PWD"].ToString();
            return "psms";
        }
    } //rmsa@1

    public static string PRConnectionString
    {
        get
        {
            if (ConfigurationManager.ConnectionStrings["PRConnection"] != null)
                return ConfigurationManager.ConnectionStrings["PRConnection"].ToString();
            return "Data Source = 192.168.3.30,1434; Initial Catalog = PRTracker; Persist Security Info = True; UID = mep; Pwd =*mep; Connection Timeout = 30;";
        }
    }
}

public class AppManager
{
    public AccessDB DataAccess;
    public AppLoginHelper AppLogin;
    public ReportHelper Report;
    public MailHelper SendMail;
    public UtilityHelper Utility;

    public AppManager()
    {
        DataAccess = new AccessDB(dbINFO._SRV, dbINFO._CAT, dbINFO._UID, dbINFO._PWD);
        AppLogin = new AppLoginHelper();
        Utility = new UtilityHelper();
        SendMail = new MailHelper();
        Report = new ReportHelper(Utility, dbINFO._SRV, dbINFO._CAT, dbINFO._UID, dbINFO._PWD);
    }
}

#region Database Access Helper Class

public class AccessDB
{

    SqlConnection _Cn;
    private string strConStr = "";
    string _ActiveIdent = "";
    string _LoginName = "";
    public string ScreenName = "";
    public string LoginName = "";
    //public Hashtable htSqlQ =new Hashtable();    
    public Dictionary<int, string[][]> htSqlQueries = new Dictionary<int, string[][]>();
    public delegate void ErrorEventHandler(string ErrorCode, string ErrorMessage);
    public event ErrorEventHandler OnShowError;

    public AccessDB(string MSSQLServerName, string Catalog, string UserID, string Password)
    {
        strConStr = "Data Source = " + MSSQLServerName + "; Initial Catalog = " + Catalog +
                    "; Persist Security Info = True; UID = " + UserID + "; Pwd = " + Password + "; Connection Timeout = 30;";
    }

    void _Cn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
    { ///Double Check
        //ShowError(ErrorNumber.SetType("00000000", errorType.DataOperationError), e.Message);
    }

    public void ShowError(string sErrorCode, string sErrorMsg)
    {
        if (OnShowError != null) OnShowError(sErrorCode, sErrorMsg);
    }

    public SqlConnection Open(string customConnectionString = "")
    {
        string customSQLConStr = "";
        if (customConnectionString.Trim().Length > 0)
        {
            customSQLConStr = customConnectionString.Trim();
        }
        if (string.IsNullOrEmpty(customSQLConStr))
            _Cn = new SqlConnection(strConStr);
        else
            _Cn = new SqlConnection(customSQLConStr);
        try { _Cn.Open(); }
        catch (SqlException ex)
        {
            ShowError(ErrorNumber.SetType(ex.Number.ToString(), errorType.ConnectionError), ex.Message);
            return null;
        }
        _Cn.InfoMessage += new SqlInfoMessageEventHandler(_Cn_InfoMessage);
        return _Cn;
    }

    public bool Close()
    {
        try
        {
            if (_Cn != null)
            {
                _Cn.Close();
                _Cn.Dispose();
            }
        }
        catch (SqlException ex)
        {
            ShowError(ErrorNumber.SetType(ex.Number.ToString(), errorType.ConnectionError), ex.Message);
            return false;
        }
        return true;
    }

    private QueryExecutor batchQuery = null;
    public QueryExecutor BatchQuery
    {
        get
        {
            if (batchQuery == null)
                batchQuery = new QueryExecutor(this);
            return batchQuery;
        }
    }
    public string ActiveIdentity { get { return _ActiveIdent; } set { _ActiveIdent = value; } }
    public dbAccessPermission UIPermission = new dbAccessPermission();
    public void SetUISecurity(string loginName, string screenPath)
    {
        LoginName = loginName;

        SqlConnection tmpCon = new SqlConnection(strConStr);
        tmpCon.Open();
        if (!string.IsNullOrEmpty(loginName.Trim()))
        {
            string sUID = new SqlCommand("select UserID from UserInfo where UserName = '" + loginName + "'", tmpCon).ExecuteScalar().ToString();
            string sGID = new SqlCommand("select GroupID from UserInfo where UserName = '" + loginName + "'", tmpCon).ExecuteScalar().ToString(); ;

            bool[] access = AccessPermission(screenPath, sUID, sGID, tmpCon);

            UIPermission.IsInsertAllowed = access[0];
            //UIPermission = access[1];
            UIPermission.IsUpdateAllowed = access[2];
            UIPermission.IsDeleteAllowed = access[3];
        }
        if (!string.IsNullOrEmpty(screenPath.Trim()))
        {
            ScreenName = GetScreenName(screenPath, tmpCon);
        }
        tmpCon.Close();

    }
    public bool[] AccessPermission(string screenPath, string userId, string groupId, SqlConnection sqlCon)
    {
        bool[] granted = new bool[4];
        SqlCommand sqlCmd = null;
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dAdapter = new SqlDataAdapter();
            string sqlQuery = "";

            sqlQuery += " select ";
            sqlQuery += " iif(sum(iif(ExecuteInsert = 1, 1, 0)) > 0, Cast(1 as bit), Cast(0 as bit)) as ExecuteInsert, ";
            sqlQuery += " iif(sum(iif(ExecuteRead = 1, 1, 0)) > 0, Cast(1 as bit), Cast(0 as bit)) as ExecuteRead, ";
            sqlQuery += " iif(sum(iif(ExecuteUpdate = 1, 1, 0)) > 0, Cast(1 as bit), Cast(0 as bit)) as ExecuteUpdate, ";
            sqlQuery += " iif(sum(iif(ExecuteDelete = 1, 1, 0)) > 0, Cast(1 as bit), Cast(0 as bit)) as ExecuteDelete ";
            sqlQuery += " from AppPermission ";
            sqlQuery += " where ";
            sqlQuery += " AppResourceId = (select top 1 Id from AppResource where " + ConfigurationManager.AppSettings["ResourcePath"] + " = '" + screenPath + "') ";
            sqlQuery += " and (UserId = '" + userId + "' or GroupId = '" + groupId + "') ";

            sqlCmd = new SqlCommand(sqlQuery, sqlCon);
            dAdapter.SelectCommand = sqlCmd;
            dAdapter.Fill(dt);
            granted[0] = bool.Parse(dt.Rows[0]["ExecuteInsert"].ToString());
            granted[1] = bool.Parse(dt.Rows[0]["ExecuteRead"].ToString());
            granted[2] = bool.Parse(dt.Rows[0]["ExecuteUpdate"].ToString());
            granted[3] = bool.Parse(dt.Rows[0]["ExecuteDelete"].ToString());
        }
        catch (Exception ex)
        {
            ShowError(ErrorNumber.SetType(ex.HResult.ToString(), errorType.ConnectionError), ex.Message);
        }
        finally
        {
            if (sqlCmd != null)
            {
                sqlCmd.Dispose();
            }
        }
        return granted;
    }
    public string GetScreenName(string screenPath, SqlConnection sqlCon)
    {
        string screenname = "";
        SqlCommand sqlCmd = null;
        try
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dAdapter = new SqlDataAdapter();
            string sqlQuery = "";

            sqlQuery += " select Name from AppResource where " + ConfigurationManager.AppSettings["ResourcePath"] + " = '" + screenPath + "' ";

            sqlCmd = new SqlCommand(sqlQuery, sqlCon);
            dAdapter.SelectCommand = sqlCmd;
            dAdapter.Fill(dt);
            screenname = dt.Rows[0]["Name"].ToString();
        }
        catch (Exception ex)
        {
            ShowError(ErrorNumber.SetType(ex.HResult.ToString(), errorType.ConnectionError), ex.Message);
        }
        finally
        {
            if (sqlCmd != null)
            {
                sqlCmd.Dispose();
            }
        }
        return screenname;
    }

    public string[] ProcessWhere(string sWhereCluse)
    {
        string sTmpTok = "";
        sWhereCluse = sWhereCluse.Replace("@", " @");
        string[] arWhereFields = sWhereCluse.Split(' ');
        for (int i = 0; i < arWhereFields.Length; i++)
        {
            if (arWhereFields[i].Contains('@')) sTmpTok = sTmpTok + arWhereFields[i] + ",";
        }
        sTmpTok = sTmpTok.Substring(0, sTmpTok.Length - 1);
        string[] arTmpTok = sTmpTok.Split(',');
        //Added by roney for special character check

        string[] specialChars = new[] { "\\", "/", ":", "*", "<", ">", "|", "#", "{", "}", "%", "~", "&", "(", ")" };
        int j = 0;
        foreach (string s in arTmpTok)
        {
            foreach (string c in specialChars)
            {
                if (s.Contains(c))
                {
                    arTmpTok[j] = s.Replace(c, "");
                }
            }
            j++;
        }

        /////////////////

        return arTmpTok;
    }

    public DataTable RecordSet(string sSQL, string[] WhereCluseValue)
    {

        //if (!UIPermission.IsExcutionAllowed((int)dbPermission.Read))
        //{
        //    ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
        //                    "You do not have read permission on this screen");
        //    return null;
        //}
        DataTable dt = new DataTable();
        SqlConnection ActiveConnection = Open();
        if (ActiveConnection != null)
        {
            SqlCommand sCmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                sCmd.Connection = ActiveConnection;
                sCmd.CommandText = sSQL;
                sCmd.CommandType = CommandType.Text;

                string[] arWhereFields = null;
                string[] arWhereValues = null;
                if (WhereCluseValue != null && WhereCluseValue.Length > 0)
                {
                    arWhereFields = ProcessWhere(sSQL);
                    arWhereValues = WhereCluseValue;
                }
                if (arWhereFields != null)
                {
                    if (arWhereFields.Length != arWhereValues.Length)
                    {
                        ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
                            "Number of Parameter and Value is not equal.");
                        return null;
                    }
                    for (int i = 0; i < arWhereFields.Length; i++) { sCmd.Parameters.AddWithValue(arWhereFields[i], arWhereValues[i]); }
                }

                da.SelectCommand = sCmd;
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                ShowError(ErrorNumber.SetType(ex.Number.ToString(), errorType.DataOperationError), ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(ErrorNumber.SetType(ex.HResult.ToString(), errorType.DataOperationError), ex.Message);
            }
            finally
            {
                Close();
            }
        }

        return dt;
    }

    //Need to replace ProceesWhere from any  1 place - now it is in AccessDB and QueryExecutor

    public string GetScalarValue(string sSQL, string[] WhereCluseValue)
    {
        string result = "";
        SqlConnection ActiveConnection = Open();
        if (ActiveConnection != null)
        {
            SqlCommand sCmd = new SqlCommand();
            try
            {
                sCmd.Connection = ActiveConnection;
                sCmd.CommandText = sSQL;
                sCmd.CommandType = CommandType.Text;

                string[] arWhereFields = null;
                string[] arWhereValues = null;
                if (!string.IsNullOrEmpty(sSQL))
                {
                    arWhereFields = ProcessWhere(sSQL);
                    arWhereValues = WhereCluseValue;
                }
                if (arWhereFields != null)
                {
                    if (arWhereFields.Length != arWhereValues.Length)
                    {
                        ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
                            "Number of Parameter and Value is not equal.");
                        return null;
                    }
                    for (int i = 0; i < arWhereFields.Length; i++) { sCmd.Parameters.AddWithValue(arWhereFields[i], arWhereValues[i]); }
                }

                result = sCmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                ShowError(ErrorNumber.SetType(ex.Number.ToString(), errorType.DataOperationError), ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(ErrorNumber.SetType(ex.HResult.ToString(), errorType.DataOperationError), ex.Message);
            }
            finally
            {
                Close();
            }
        }

        return result;
    }


}

public class dbAccessPermission
{
    bool _IsInsertAllowed = false;
    bool _IsDeleteAllowed = false;
    bool _IsUpdateAllowed = false;
    public bool IsInsertAllowed { set { _IsInsertAllowed = value; } }
    public bool IsDeleteAllowed { set { _IsDeleteAllowed = value; } }
    public bool IsUpdateAllowed { set { _IsUpdateAllowed = value; } }
    public bool IsExcutionAllowed(int dp)
    {
        bool bRet = false;
        if (dp == (int)dbPermission.Insert) return _IsInsertAllowed;
        if (dp == (int)dbPermission.Delete) return _IsDeleteAllowed;
        if (dp == (int)dbPermission.Update) return _IsUpdateAllowed;
        return bRet;
    }
}

public class QueryExecutor
{
    AccessDB __AccessDB;
    SqlConnection ActiveConnection;
    SqlTransaction ActiveTransaction;

    public QueryExecutor(AccessDB ADB)
    {
        __AccessDB = ADB;
    }

    public void AddToQ(int QueryType, string Table, string Fields, string[] Values, string WhereCluse, string[] WhereValue, string AuditField)
    {
        int max = 1;
        if (Values != null && Values.Length > max)
            max = Values.Length;
        if (WhereValue != null && WhereValue.Length > max)
            max = WhereValue.Length;
        string[][] arFields = new string[7][];
        arFields[0] = new string[max];
        arFields[1] = new string[max];
        arFields[2] = new string[max];
        arFields[3] = new string[max];
        arFields[4] = new string[max];
        arFields[5] = new string[max];
        arFields[6] = new string[max];

        arFields[0][0] = QueryType.ToString();
        arFields[1][0] = Table;
        arFields[2][0] = Fields;
        arFields[3] = Values;
        arFields[4][0] = WhereCluse;
        arFields[5] = WhereValue;
        arFields[6][0] = AuditField;
        __AccessDB.htSqlQueries.Add(__AccessDB.htSqlQueries.Count, arFields);
    }


    private bool ProcessQuery(bool IsOutput = true, bool externalDB = false)
    {
        SqlCommand sCmd = new SqlCommand();
        bool bSuccess = false;
        bool logged = false;
        try
        {
            foreach (var entry in __AccessDB.htSqlQueries)
            {
                sCmd = new SqlCommand();
                string[][] arItem = (string[][])entry.Value;
                string sQueryType = arItem[0][0];
                string sTable = arItem[1][0];
                string sFields = arItem[2][0];
                string[] sValues = arItem[3];
                string sWhereCluse = arItem[4][0];
                string[] sWhereVal = arItem[5];
                string outputField = arItem[6][0];

                string[] arFields = new string[] { };
                if (!string.IsNullOrEmpty(sFields))
                    arFields = sFields.Split(',');
                //For new Framework - split in values removed and array passed.
                string[] arValues = sValues;

                string[] arWhereFields = null;
                string[] arWhereValues = null;

                //set operation type
                string operationType = "Browse";
                switch (sQueryType)
                {
                    case "0": operationType = "Insert";
                        break;
                    case "1": operationType = "Delete";
                        break;
                    case "2": operationType = "Update";
                        break;
                    default: operationType = "Browse";
                        break;
                }

                //For update-delete where caluse is mandatory
                if (sQueryType == "1" || sQueryType == "2")
                {
                    if (!string.IsNullOrEmpty(sWhereCluse))
                    {
                        arWhereFields = __AccessDB.ProcessWhere(sWhereCluse);
                        //For new Framework - split in values removed and array passed.
                        arWhereValues = sWhereVal;
                    }
                    else
                    {
                        ActiveTransaction.Rollback();
                        __AccessDB.ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
                            "Where caluse is mandatory for update/delete operation.");
                        return false;
                    }
                }

                //If Output Field is not set - get field from query                
                if (string.IsNullOrEmpty(outputField))
                {
                    if (sQueryType == "0")
                    {
                        //Get the first field from insert query
                        outputField = arFields[0];
                    }
                    else if (sQueryType == "1" || sQueryType == "2")
                    {
                        //Get the first field from where clause
                        outputField = arWhereFields[0].Replace("@", "");
                    }
                }

                string sSQL = "";
                if (__AccessDB.UIPermission.IsExcutionAllowed((int)dbPermission.Insert) && sQueryType == "0")
                {
                    if (IsOutput)
                    {
                        sSQL = "INSERT INTO " + sTable + " (" + sFields + ") OUTPUT INSERTED." + outputField + " VALUES(@" +
                            sFields.Replace(",", ",@").Replace("[", "").Replace("]", "") + ")";
                    }
                    else
                    {
                        sSQL = "INSERT INTO " + sTable + " (" + sFields + ") VALUES(@" + sFields.Replace(",", ",@").Replace("[", "").Replace("]", "") + ")";
                    }

                }

                if (__AccessDB.UIPermission.IsExcutionAllowed((int)dbPermission.Delete) && sQueryType == "1")
                {
                    if (sWhereCluse != "")
                    {
                        if (IsOutput)
                        {
                            sSQL = "DELETE FROM " + sTable + " OUTPUT DELETED." + outputField + " WHERE " + sWhereCluse.Replace("WHERE", "");
                        }
                        else
                        {
                            sSQL = "DELETE FROM " + sTable + " WHERE " + sWhereCluse.Replace("WHERE", "");
                        }
                    }

                }

                if (__AccessDB.UIPermission.IsExcutionAllowed((int)dbPermission.Update) && sQueryType == "2")
                {
                    string sUpdateSQL = "";
                    for (int i = 0; i < arFields.Length; i++)
                    {
                        sUpdateSQL += arFields[i] + " = @" + arFields[i].Replace("[", "").Replace("]", "") + ",";
                    }

                    sUpdateSQL = sUpdateSQL.Substring(0, sUpdateSQL.Length - 1);
                    if (sWhereCluse != "")
                    {
                        if (IsOutput)
                        {
                            sSQL = "UPDATE " + sTable + " SET " + sUpdateSQL + " OUTPUT INSERTED." + outputField + " WHERE " + sWhereCluse.Replace("WHERE", "");
                        }
                        else
                        {
                            sSQL = "UPDATE " + sTable + " SET " + sUpdateSQL + " WHERE " + sWhereCluse.Replace("WHERE", "");
                        }
                    }
                }

                if (sSQL != "")
                {
                    if (arFields.Length == arValues.Length)
                    {
                        sCmd.Connection = ActiveConnection;
                        sCmd.Transaction = ActiveTransaction;
                        sCmd.CommandText = sSQL;
                        sCmd.CommandType = CommandType.Text;
                        for (int i = 0; i < arFields.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(arFields[i])) sCmd.Parameters.AddWithValue("@" + arFields[i].Replace("[", "").Replace("]", ""), arValues[i]);
                        }
                        if (arWhereFields != null)
                        {
                            if (arWhereFields.Length != arWhereValues.Length)
                            {
                                ActiveTransaction.Rollback();
                                __AccessDB.ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
                                    "Number of Parameter and Value is not equal.");
                                return false;
                            }
                            for (int i = 0; i < arWhereFields.Length; i++) { sCmd.Parameters.AddWithValue(arWhereFields[i], arWhereValues[i]); }
                        }

                        string sRefVal = Convert.ToString(sCmd.ExecuteScalar());
                        if (!string.IsNullOrEmpty(sRefVal)) __AccessDB.ActiveIdentity = sRefVal;

                        //Insert into audit log
                        if (!externalDB && !logged)
                        {
                            string auditLogQuery = "INSERT INTO AuditLog ([TABLE],[SCREEN],[USER],[OPRETATION],[REFFIELD],[REFVALUE],[CreatedDate]) VALUES('" +
                                sTable + "','" + __AccessDB.ScreenName + "','" + __AccessDB.LoginName + "','" + operationType + "','" + outputField +
                                "','" + sRefVal + "','" + DateTime.Now.ToString() + "')";
                            sCmd.CommandText = auditLogQuery;
                            object result = sCmd.ExecuteScalar();
                            logged = true;
                        }
                        bSuccess = true;
                    }
                    else
                    {
                        ActiveTransaction.Rollback();
                        __AccessDB.ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
                            "Number of Parameter and Value is not equal.");
                        return false;
                    }
                }
                else
                {
                    ActiveTransaction.Rollback();
                    __AccessDB.ShowError(ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError),
                        "You do not have enough permission on this screen");
                    return false;
                }
            }
            if (!bSuccess)
            {
                ActiveTransaction.Rollback();
                __AccessDB.ShowError(ErrorNumber.SetType(ErrorNumber.DataOperationError, errorType.DataOperationError),
                    "Data Operation Failed!");
            }
            return bSuccess;
        }
        catch (SqlException ex)
        {
            ActiveTransaction.Rollback();
            __AccessDB.ShowError(ErrorNumber.SetType(ex.Number.ToString(), errorType.DataOperationError), ex.Message);
        }
        catch (Exception ex)
        {
            ActiveTransaction.Rollback();
            __AccessDB.ShowError(ErrorNumber.SetType(ex.HResult.ToString(), errorType.DataOperationError), ex.Message);
        }
        finally
        {
            if (sCmd != null)
                sCmd.Dispose();
        }
        return false;
    }

    public bool Execute(bool IsOutput = true, ConnectionType openConnection = ConnectionType.OpenGOClose,
        string customConnectionString = "")
    {
        bool result = false;
        if (openConnection == ConnectionType.OpenGOClose || openConnection == ConnectionType.Open)
        {
            ActiveConnection = __AccessDB.Open(customConnectionString);
            ActiveTransaction = ActiveConnection.BeginTransaction();
        }
        if (ActiveConnection != null)
        {
            if (string.IsNullOrEmpty(customConnectionString))
                result = ProcessQuery(IsOutput);
            else
                result = ProcessQuery(IsOutput, true);

            __AccessDB.htSqlQueries.Clear();
            if (openConnection == ConnectionType.OpenGOClose || openConnection == ConnectionType.Close || result == false)
            {
                if (result)
                {
                    ActiveTransaction.Commit();
                }
                __AccessDB.Close();
            }
        }
        return result;
    }

    public void Insert(string Table, string Fields, string[] Values, string AuditColumn = "") { AddToQ((int)dbPermission.Insert, Table, Fields, Values, null, null, AuditColumn); }
    public void Delete(string Table, string WhereCluse, string[] WhereValue, string AuditColumn = "") { AddToQ((int)dbPermission.Delete, Table, null, new string[] { }, WhereCluse, WhereValue, AuditColumn); }
    public void Update(string Table, string Fields, string[] Values, string WhereCluse, string[] WhereValue, string AuditColumn = "")
    {
        AddToQ((int)dbPermission.Update, Table, Fields, Values, WhereCluse, WhereValue, AuditColumn);
    }

}
#endregion

#region Report Helper Class

public class ReportHelper
{
    private string strConStr = "";
    public UtilityHelper Utility;

    //NEED to REMOVE Cons PRAM
    public ReportHelper(UtilityHelper uh, string MSSQLServerName, string Catalog, string UserID, string Password)
    {
        Utility = uh;
        strConStr = "Data Source = " + MSSQLServerName + "; Initial Catalog = " + Catalog +
                    "; Persist Security Info = True; UID = " + UserID + "; Pwd = " + Password + "; Connection Timeout = 30;";
    }


    public void PrintReport(Page _Page, string _ReportName, string _FileName, DataTable[] dts, string[] _DSTableName,
        string[] paramNames = null, string[] paramValues = null)
    {
        paramNames = paramNames == null ? new string[] { } : paramNames;
        paramValues = paramValues == null ? new string[] { } : paramValues;
        dts = dts == null ? new DataTable[] { } : dts;
        _DSTableName = _DSTableName == null ? new string[] { } : _DSTableName;

        ReportDocument rptDoc = new ReportDocument();
        try
        {
            rptDoc.Load(_Page.Server.MapPath("~/Reports/" + _ReportName));

            dsPSMS ds = new dsPSMS();
            for (int i = 0; i < dts.Length; i++)
            {
                ds.Tables[_DSTableName[i]].Merge(dts[i]);
            }
            if (dts.Length > 0)
                rptDoc.SetDataSource(ds);

            if (paramNames.Length == paramValues.Length)
            {
                for (int i = 0; i < paramNames.Length; i++)
                    rptDoc.SetParameterValue(paramNames[i], paramValues[i]);
            }

            rptDoc.SetDatabaseLogon(dbINFO._UID, dbINFO._PWD, dbINFO._SRV, dbINFO._CAT);
            _Page.Response.Buffer = false;
            _Page.Response.ClearContent();
            _Page.Response.ClearHeaders();
            //Export the Report to Response stream in PDF format and file name Customers
            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, _Page.Response, true, _FileName);

            _Page.Response.ClearContent();
            _Page.Response.ClearHeaders();
            _Page.Response.Clear();
            _Page.Response.Close();
        }
        catch (Exception ex)
        {
            //DO NOTHING            
        }
        finally
        {
            rptDoc.Close();
            rptDoc.Dispose();
        }
    }

    //ALL THIS FUNCTION NOT IN USE
    public ReportDocument cReport(String headerSql, Boolean isPortrait, Boolean headerReq, String reportSql, String reportName)
    {
        ReportDocument rptDoc = new ReportDocument();
        try
        {
            String hName;
            rptDoc.FileName = reportName;

            SqlConnection sqlCon = new SqlConnection(strConStr);
            sqlCon.Open();

            DataSet dSet = new DataSet();
            SqlDataAdapter dAdapter = new SqlDataAdapter();

            dAdapter.TableMappings.Add("Table", "Header");
            SqlCommand sqlCmd = new SqlCommand(headerSql, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            dAdapter.SelectCommand = sqlCmd;
            dAdapter.Fill(dSet);
            dAdapter.Dispose();

            dAdapter.TableMappings.Add("Table", "Report");
            sqlCmd = new SqlCommand(reportSql, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            dAdapter.SelectCommand = sqlCmd;
            dAdapter.Fill(dSet);

            sqlCmd.Cancel();
            sqlCmd.Dispose();

            if (isPortrait == true) hName = "crHeader_Portrait.rpt"; else hName = "crHeader_Landscape.rpt";
            if (headerReq == true) rptDoc.OpenSubreport(hName).SetDataSource(dSet.Tables["Header"]);

            rptDoc.SetDataSource(dSet.Tables["Report"]);

            dSet.Clear();
            dSet.Dispose();
            dAdapter.Dispose();
            sqlCon.Close();
            sqlCon.Dispose();

            return rptDoc;
        }
        //catch (SqlException sqlExp)
        //{
        //    //MessageBox.Show(sqlExp.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return;
        //}
        catch
        {
            //MessageBox.Show(ex.Message, "General Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return rptDoc;
        }
    }
    public void cReport(String reportSql, ReportDocument reportName)
    {
        try
        {
            //cnStr = Properties.Settings.Default.sCn;
            SqlConnection sqlCon = new SqlConnection(strConStr);
            sqlCon.Open();

            DataSet dSet = new DataSet();
            SqlDataAdapter dAdapter = new SqlDataAdapter();

            dAdapter.TableMappings.Add("Table", "Report");
            SqlCommand sqlCmd = new SqlCommand(reportSql, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            dAdapter.SelectCommand = sqlCmd;
            dAdapter.Fill(dSet);

            sqlCmd.Cancel();
            sqlCmd.Dispose();

            reportName.SetDataSource(dSet.Tables["Report"]);
            //Session["reportDocument"] = reportName;
            //Properties.Settings.Default.rptSource = reportName;
            //Properties.Settings.Default.Save();

            dSet.Clear();
            dSet.Dispose();
            dAdapter.Dispose();
            sqlCon.Close();
            sqlCon.Dispose();
            //Response.Redirect("ReportViewer.aspx");
        }
        //catch (SqlException sqlExp)
        //{
        //    //MessageBox.Show(sqlExp.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return;
        //}
        catch
        {
            //MessageBox.Show(ex.Message, "General Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
    //dbINFO._SRV, dbINFO._CAT, dbINFO._UID, dbINFO._PWD
    public void PrintReport(Page _Page, string _ReportName, string _FileName, DataTable dt)
    {
        ReportDocument rptDoc = new ReportDocument();
        try
        {
            rptDoc.Load(_Page.Server.MapPath("~/Reports/" + _ReportName));
            rptDoc.SetDataSource(dt);

            rptDoc.SetDatabaseLogon(dbINFO._UID, dbINFO._PWD, dbINFO._SRV, dbINFO._CAT);
            _Page.Response.Buffer = false;
            _Page.Response.ClearContent();
            _Page.Response.ClearHeaders();
            //Export the Report to Response stream in PDF format and file name Customers
            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, _Page.Response, true, _FileName);
        }
        catch (Exception ex)
        {
            Utility.ShowHTMLMessage(_Page, "100", ex.Message);
        }
        finally
        {
            rptDoc.Close();
            rptDoc.Dispose();
        }
    }
    //ALL THIS FUNCTION NOT IN USE
}
#endregion

#region Login Helper Class

public class AppLoginHelper
{
    UtilityHelper uh = new UtilityHelper();
    static protected bool CheckCert(Object sender, X509Certificate certificate,
       X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }
    public ExchangeService GetServiceEx(string sEmailID, string sEmailPass)
    {
        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
        service.Credentials = new NetworkCredential(sEmailID, sEmailPass);
        ServicePointManager.ServerCertificateValidationCallback = CheckCert;
        service.Url = new Uri("https://dbxprd0310.outlook.com/EWS/Exchange.asmx");
        return service;
    }
    public bool IsValidOutlookLogin(Page _Page, string sEmailID, string sEmailPass)
    {
        bool bRet = false;
        try
        {
            ExchangeService service = GetServiceEx(sEmailID, sEmailPass);
            List<AttendeeInfo> attendees = new List<AttendeeInfo>();
            attendees.Add(new AttendeeInfo(sEmailID));

            GetUserAvailabilityResults results = service.GetUserAvailability(attendees,
                    new TimeWindow(DateTime.Now, DateTime.Now.AddHours(24)), AvailabilityData.FreeBusy);

            AttendeeAvailability myAvailablity = results.AttendeesAvailability.FirstOrDefault();
            if (myAvailablity != null) { Console.WriteLine(String.Format("FREE", myAvailablity.CalendarEvents.Count)); }
            return true;
        }
        catch (Exception ex)
        {
            var w32ex = ex as Win32Exception;
            //uh.ShowHTMLMessage(_Page, ErrorNumber.SetType("00", errorType.LoginError), ex.Message);
            bRet = false;
        }
        return bRet;
    }

    public bool IsValidLogin_SCIMail(Page _Page, string sEmailID, string sEmailPass)
    {
        bool bRet = false;
        if (IsValidOutlookLogin(_Page, sEmailID, sEmailPass))
            bRet = true;
        else
        {
            uh.ShowHTMLMessage(_Page, ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.LoginError), "Login Failure. You are not authorized user!!");
            bRet = false;
        }
        return bRet;
    }
    public class LdapAuthentication
    {
        private String _path;
        private String _filterAttribute;
        public LdapAuthentication(String path) { _path = path; }
        public bool IsAuthenticated(String domain, String username, String pwd)
        {
            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            try
            {
                //Bind to the native AdsObject to force authentication.			
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result) { return false; }

                _path = result.Path; //Update the new path to the user in the directory.
                _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception("Error authenticating user. " + ex.Message);
            }
            return true;
        }

        public String GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();
            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex) { return null; }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex) { throw new Exception("Error obtaining group names. " + ex.Message); }
            return groupNames.ToString();
        }
    }
    public bool IsValidLogin_SCIAD(Page _Page, string ADUserName, string ADPassword)
    {
        bool bRet = false;
        string adPath = "LDAP://dhaka.org";
        LdapAuthentication adAuth = new LdapAuthentication(adPath);
        if (adAuth.IsAuthenticated("dhaka.org", ADUserName, ADPassword))
            bRet = true;
        else
        {
            uh.ShowHTMLMessage(_Page, ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.LoginError), "Login Failure. You are not authorized user!!");
            bRet = false;
        }
        return bRet;
    }
    public bool IsValidLogin_SCIDB(Page _Page, SqlConnection sCn, string Table, string UserName, string Password)
    {
        bool bRet = true;
        try
        {
            string sSQL = "SELECT COUNT(*) FROM " + Table + " WHERE UserName = @UserName AND Password = @Password";
            SqlCommand sCmd = new SqlCommand();
            sCmd.Connection = sCn;
            sCmd.CommandText = sSQL;
            sCmd.CommandType = CommandType.Text;
            sCmd.Parameters.AddWithValue("@UserName", UserName);
            sCmd.Parameters.AddWithValue("@Password", Password);
            string sUser = sCmd.ExecuteScalar().ToString();
            sCmd.Dispose(); sCn.Close();
            if (sUser == "0")
            {
                uh.ShowHTMLMessage(_Page, ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.LoginError), "Login Failure. You are not authorized user!!");
                bRet = false;
            }
        }
        catch (Exception ex)
        {
            var w32ex = ex as Win32Exception;
            uh.ShowHTMLMessage(_Page, ErrorNumber.SetType(w32ex.ErrorCode.ToString(), errorType.LoginError), w32ex.Message);
            bRet = false;
        }
        return bRet;
    }
}
#endregion

#region SendMail Helper Class

public class MailHelper
{
    UtilityHelper uh = new UtilityHelper();
    public MailHelper() { }
    static protected bool CheckCert(Object sender, X509Certificate certificate,
     X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }
    public ExchangeService GetServiceEx(string sEmailID, string sEmailPass)
    {
        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
        service.Credentials = new NetworkCredential(sEmailID, sEmailPass);
        ServicePointManager.ServerCertificateValidationCallback = CheckCert;
        service.Url = new Uri("https://dbxprd0310.outlook.com/EWS/Exchange.asmx");
        return service;
    }
    public bool SendOutlookMail(Page _Page, string sMailTo, string sSubject, string sBody, string sAttachmentPath)
    {
        try
        {
            string sqlStr = "SELECT TOP(1) [EmailID],[Password] FROM [dbo].[SysSet]";
            DataTable dt = new AppManager().DataAccess.RecordSet(sqlStr, new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                string sEmailID = dt.Rows[0]["EmailID"].ToString();
                string sEmailPass = dt.Rows[0]["Password"].ToString();
                //sMailTo = "shyamal.mondal@savethechildren.org";
                EmailMessage message = new EmailMessage(GetServiceEx(sEmailID, sEmailPass));
                message.Subject = sSubject;
                message.Body = sBody;
                if (sAttachmentPath != "") message.Attachments.AddFileAttachment(sAttachmentPath);
                message.ToRecipients.Add(sMailTo);
                message.SendAndSaveCopy();
                return true;
            }
            else
            {
                uh.ShowHTMLMessage(_Page, "100", "From email Id not found");
                return false;
            }
        }
        catch (Exception ex)
        {
            uh.ShowHTMLMessage(_Page, "100", ex.Message);
            return false;
        }
    }

    public bool SendOutlookMail(Page _Page, string[] sMailTo, string[] ccEmail, string sSubject, string sBody, string[] sAttachmentPath)
    {
        try
        {
            string sqlStr = "SELECT TOP(1) [EmailID],[Password] FROM [dbo].[SysSet]";
            DataTable dt = new AppManager().DataAccess.RecordSet(sqlStr, new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                string sEmailID = dt.Rows[0]["EmailID"].ToString();
                string sEmailPass = dt.Rows[0]["Password"].ToString();
                EmailMessage message = new EmailMessage(GetServiceEx(sEmailID, sEmailPass));
                message.Subject = sSubject;
                message.Body = sBody;

                if (sAttachmentPath.Length > 0)
                {
                    foreach (string file in sAttachmentPath)
                    {
                        if (!string.IsNullOrEmpty(file))
                        {
                            message.Attachments.AddFileAttachment(file);
                        }
                    }
                }

                if (sMailTo.Length > 0)
                {
                    foreach (string tm in sMailTo)
                    {
                        if (!string.IsNullOrEmpty(tm))
                        {
                            message.ToRecipients.Add(tm);
                        }
                    }

                }

                if (ccEmail.Length > 0)
                {
                    foreach (string ccm in ccEmail)
                    {
                        if (!string.IsNullOrEmpty(ccm))
                        {
                            message.CcRecipients.Add(ccm);
                        }
                    }

                }
                message.SendAndSaveCopy();
                return true;
            }
            else
            {
                uh.ShowHTMLMessage(_Page, "100", "From email Id not found");
                return false;
            }
        }
        catch (Exception ex)
        {
            uh.ShowHTMLMessage(_Page, "100", ex.Message);
            return false;
        }
    }


}
#endregion

#region Utility Helper Class

public class UtilityHelper
{
    public UtilityHelper() { }
    public void ShowHTMLMessage(Page _Page, string ErroeCode, string ErrorMessage)
    {
        ShowAlert(_Page, ErrorMessage, ErroeCode, MessageType.Error);
    }

    public void ShowHTMLAlert(Page _Page, string ErroeCode, string ErrorMessage)
    {
        ShowAlert(_Page, ErrorMessage, ErroeCode, MessageType.Success);
    }

    public void ShowAlert(Page _Page, string ErrorMessage, string ErroeCode = "0000", MessageType type = MessageType.Error)
    {
        string eMsg = ErrorMessage.Replace("\r\n", " ").Replace("'", "\"");
        eMsg = "<script type='text/javascript'>showMessage('" + ErroeCode + "','" + eMsg + "'," + (int)type + ");</script>";
        ScriptManager.RegisterClientScriptBlock(_Page, _Page.GetType(), "script", eMsg, false);
    }

    public void LoadComboBox(RadComboBox RadDropDown, string SQL, string DisplayField, string ValueField, bool AppendRow = true)
    {
        AppManager am = new AppManager();
        DataTable dt = am.DataAccess.RecordSet(SQL, new string[] { });
        if (AppendRow)
        {
            DataRow dRow = dt.NewRow();
            dRow[ValueField] = "0";
            dRow[DisplayField] = "";
            dt.Rows.InsertAt(dRow, 0);
        }
        RadDropDown.DataSource = dt;
        RadDropDown.DataTextField = DisplayField;
        RadDropDown.DataValueField = ValueField;
        RadDropDown.DataBind();
        if (AppendRow)
        {
            RadDropDown.SelectedValue = "0";
        }
    }
    public void LoadComboBox(RadComboBox RadDropDown, string SQL, string DisplayField, string ValueField, string[] WhereCluseValue)
    {
        AppManager am = new AppManager();
        DataTable dt = am.DataAccess.RecordSet(SQL, WhereCluseValue);
        DataRow dRow = dt.NewRow();
        dRow[ValueField] = "0";
        dRow[DisplayField] = "";
        dt.Rows.Add(dRow);
        RadDropDown.DataSource = dt;
        RadDropDown.DataTextField = DisplayField;
        RadDropDown.DataValueField = ValueField;
        RadDropDown.DataBind();
        RadDropDown.SelectedValue = "0";
    }


    public void LoadGrid(RadGrid grd, string SQL, string[] WhereCluseValue, bool binddata = true)
    {
        AppManager am = new AppManager();
        grd.DataSource = am.DataAccess.RecordSet(SQL, WhereCluseValue);
        if (binddata)
        {
            grd.DataBind();
        }
    }
    public object GetCookeRecord(Page _Page, PSMSCookie _AppCookie)
    {
        string CookeName = _AppCookie.ToString();
        HttpCookie _Cookie = new HttpCookie(CookeName);
        _Cookie = _Page.Request.Cookies[CookeName];
        if (_Cookie != null) return _Cookie.Value; else return "";
    }
    public void SetCookeRecord(Page _Page, PSMSCookie _AppCookie, string CookeValue)
    {
        string CookeName = _AppCookie.ToString();
        HttpCookie _Cookie = new HttpCookie(CookeName);
        _Cookie.Value = CookeValue;                     // Set the cookie value.
        _Cookie.Expires = DateTime.Now.AddDays(30);     // Set the cookie expiration date.
        _Page.Response.Cookies.Add(_Cookie);                  // Add the cookie.
        _Page.Request.Cookies[CookeName].Value = CookeValue;  //HttpCookie cookieCode = new HttpCookie(CookeName, CookeValue);//Response.SetCookie(cookieCode);
    }

    public DataTable GetDataTableFromItems(RadGrid grid)
    {
        DataTable dt = new DataTable();

        foreach (GridColumn col in grid.Columns)
        {
            DataColumn colString = new DataColumn(col.UniqueName);
            dt.Columns.Add(colString);
        }
        foreach (GridDataItem row in grid.Items) // loops through each rows in RadGrid
        {

            DataRow dr = dt.NewRow();
            foreach (GridColumn col in grid.Columns) //loops through each column in RadGrid
            {
                if (col.ColumnType == "GridTemplateColumn")
                {
                    //GridTemplateColumn
                    if (row[col.UniqueName].Controls[1].GetType().Name == "RadTextBox")
                    {
                        dr[col.UniqueName] = (row[col.UniqueName].Controls[1] as RadTextBox).Text;
                    }
                }
                else if (col.ColumnType == "GridBoundColumn")
                {
                    //GridBoundColumn
                    dr[col.UniqueName] = row[col.UniqueName].Text;
                }
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }

    public void FileDownload(Page _page, string sSQL, string[] WhereCluseValue)
    {        
        try
        {
            DataTable dt = new AppManager().DataAccess.RecordSet(sSQL, WhereCluseValue);
            string fileName = dt.Rows[0][0].ToString().ToLower();
            FileDownload(_page, "http://bdcomis/ondesk/prdocs/", fileName);
        }
        catch (Exception ex)
        {
            ShowHTMLMessage(_page, ex.HResult.ToString(), ex.Message);
        }
        finally
        {
            
        }
    }
    public void FileDownload(Page _page, string urlPath, string filename)
    {
        Stream stream = null;
        //This controls how many bytes to read at a time and send to the client
        int bytesToRead = 10000;
        // Buffer to read bytes in chunk size specified above
        byte[] buffer = new Byte[bytesToRead];
        try
        {
            string fileName = filename;

            //Create a WebRequest to get the file
            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(urlPath + fileName);
            //Create a response for this request
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
            if (fileReq.ContentLength > 0)
                fileResp.ContentLength = fileReq.ContentLength;
            //Get the Stream returned from the response
            stream = fileResp.GetResponseStream();
            // prepare the response to the client. resp is the client Response
            var resp = HttpContext.Current.Response;
            //Indicate the type of data being sent
            resp.ContentType = "application/octet-stream";
            resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());
            int length;
            do
            {
                // Verify that the client is connected.
                if (resp.IsClientConnected)
                {
                    // Read data into the buffer.
                    length = stream.Read(buffer, 0, bytesToRead);
                    // and write it out to the response's output stream
                    resp.OutputStream.Write(buffer, 0, length);
                    // Flush the data
                    resp.Flush();
                    //Clear the buffer
                    buffer = new Byte[bytesToRead];
                }
                else
                {
                    // cancel the download if client has disconnected
                    length = -1;
                }
            } while (length > 0); //Repeat until no data is read

        }
        catch (Exception ex)
        {
            ShowHTMLMessage(_page, ex.HResult.ToString(), ex.Message);
        }
        finally
        {
            if (stream != null)
            {
                //Close the input stream
                stream.Close();
            }
        }
    }

    public bool IsValidDate(object sDate)
    {
        try
        {            
            if (sDate == null) return false;
            //System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //DateTime date = DateTime.Parse(sDate.ToString(), ci,
            //                               System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            //if (date.Year == 1900) 
            //    return false;
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }



}
#endregion