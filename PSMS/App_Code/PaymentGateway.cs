using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS
{
    public class PaymentGateway
    {
        private String sqlStr;
        private String cnStr = "Data Source=10.12.0.2;Initial Catalog=PowerPayment;User Id=sa;Password=bdco;Connection Timeout=0;";
        private SqlConnection sqlCon;
        private SqlCommand sqlCmd;
        private SqlTransaction sTrn;
        private SqlDataAdapter sqlDA;
        private DataTable dTable;

        private static Boolean hasDetails = false;
        private static Boolean deletedTemp = false;

        public enum VoucherType
        {
            VendorPaymentLocal = 1,
            VendorPaymentInternational = 2,
            ConsultantPaymentLocal = 3,
            ConsultantPaymentInternational = 4,
            AdvancePaymentUptoTK10000 = 5,
            AdvancePaymentAboveTK10000 = 6,
            AdvancePaymentForeignCurrency = 7,
            AdvanceAdjustmentLocal = 8,
            AdvanceAdjustmentInternational = 9,
            AccrualPaymentForm = 10,
            ExpenseClaimUptoTK10000 = 11,
            ExpenseClaimAboveTK10000 = 12,
            PartnerPayment = 13,
        };

        public enum CurrencyCode 
        { 
            BDT = 1,
            USD = 2,
            GBP = 3, 
            EUR = 4, 
            INR = 5, 
            CAD = 6,
            THB = 7,
            AUD = 8,
            ZAR = 9,
        };

        public enum LocationCode
        {
            Dhaka =	1,
            Barisal	= 2,
            Meherpur = 3,
            Hobigonj = 4,
            CoxsBazar =	5,
            Khulna = 6,
            Sylhet = 7,
            Noakhali = 8,
            Jhalokathi = 9,
            Barguna = 10,
            Bhola =	11,
        };

        public void getPaymentLocation(RadComboBox cbx)
        {
            sqlStr = "SELECT * FROM Location WHERE Active=1 ORDER BY LocationID ASC";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            cbx.DataSource = dTable;
            cbx.DataTextField = "LocationName";
            cbx.DataValueField = "LocationID";
            cbx.DataBind();

            sqlCon.Close();
            sqlCon.Dispose();
        }

        public void getPaymentType(RadComboBox cbx)
        {
            sqlStr = "SELECT * FROM VoucherType WHERE Active=1 ORDER BY VoucherTypeID ASC";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            cbx.DataSource = dTable;
            cbx.DataTextField = "VoucherTypeName";
            cbx.DataValueField = "VoucherTypeID";
            cbx.DataBind();

            sqlCon.Close();
            sqlCon.Dispose();
        }

        public void getPaymentCurrency(RadComboBox cbx)
        {
            sqlStr = "SELECT * FROM Currency WHERE Active=1 ORDER BY CurrencyID ASC";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            cbx.DataSource = dTable;
            cbx.DataTextField = "CurrencyName";
            cbx.DataValueField = "CurrencyID";
            cbx.DataBind();

            sqlCon.Close();
            sqlCon.Dispose();
        }

        public string getPaymentNo(string PONo)
        {
            sqlStr = "SELECT MAX(RequestNo) RequestNo FROM Request WHERE PONumber='" + PONo + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlCmd = new SqlCommand(sqlStr, sqlCon);
            string scalarData = Convert.ToString(sqlCmd.ExecuteScalar());
            
            sqlCmd.Dispose();
            sqlCon.Close();
            sqlCon.Dispose();

            return scalarData;
        }

        public void postRequest(int RequestorID, int SupervisorID, int VoucherType, string Descriptions,
            int Currency, double InvoiceAmount, double AdvanceUsed, double CashReturn, DateTime RequiredDate, 
            DateTime ExpectedReturnDate, string VendorPartnerID, string VendorPartnerName, string InvoiceNo,
            DateTime InvoiceDate, string PONumber, string OnBehalfID, string OnBehalfName, int Location, 
            int BudgetHolderID)
        {
            if (hasDetails == false) return;

            sqlStr = "INSERT INTO Request (RequestorID, SupervisorID, VoucherTypeID," +
            " Descriptions, CurrencyID, InvoiceAmount, AdvanceUsed, CashReturn, RequiredDate," +
            " ExpectedReturnDate, VendorPartnerID, VendorPartnerName, InvoiceNo, InvoiceDate," +
            " PONumber, OnBehalfID, OnBehalfName, LocationID, BudgetHolderID)" +
            " VALUES (@RequestorID, @SupervisorID, @VoucherTypeID, @Descriptions," +
            " @CurrencyID, @InvoiceAmount, @AdvanceUsed, @CashReturn, @RequiredDate," +
            " @ExpectedReturnDate, @VendorPartnerID, @VendorPartnerName, @InvoiceNo, @InvoiceDate," +
            " @PONumber, @OnBehalfID, @OnBehalfName, @LocationID, @BudgetHolderID)";

            sqlCmd = new SqlCommand(sqlStr);

            sqlCmd.Parameters.Add("@RequestorID", SqlDbType.Int);
            sqlCmd.Parameters["@RequestorID"].Value = RequestorID;
            sqlCmd.Parameters.Add("@SupervisorID", SqlDbType.Int);
            sqlCmd.Parameters["@SupervisorID"].Value = SupervisorID;
            sqlCmd.Parameters.Add("@VoucherTypeID", SqlDbType.Int);
            sqlCmd.Parameters["@VoucherTypeID"].Value = VoucherType;
            sqlCmd.Parameters.Add("@Descriptions", SqlDbType.VarChar);
            sqlCmd.Parameters["@Descriptions"].Value = Descriptions;
            sqlCmd.Parameters.Add("@CurrencyID", SqlDbType.Int);
            sqlCmd.Parameters["@CurrencyID"].Value = Currency;
            sqlCmd.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal);
            sqlCmd.Parameters["@InvoiceAmount"].Value = InvoiceAmount;
            sqlCmd.Parameters.Add("@AdvanceUsed", SqlDbType.Decimal);
            sqlCmd.Parameters["@AdvanceUsed"].Value = AdvanceUsed;
            sqlCmd.Parameters.Add("@CashReturn", SqlDbType.Decimal);
            sqlCmd.Parameters["@CashReturn"].Value = CashReturn;
            sqlCmd.Parameters.Add("@RequiredDate", SqlDbType.DateTime);
            sqlCmd.Parameters["@RequiredDate"].Value = RequiredDate;
            sqlCmd.Parameters.Add("@ExpectedReturnDate", SqlDbType.DateTime);
            sqlCmd.Parameters["@ExpectedReturnDate"].Value = ExpectedReturnDate;
            sqlCmd.Parameters.Add("@VendorPartnerID", SqlDbType.VarChar);
            sqlCmd.Parameters["@VendorPartnerID"].Value = VendorPartnerID;
            sqlCmd.Parameters.Add("@VendorPartnerName", SqlDbType.VarChar);
            sqlCmd.Parameters["@VendorPartnerName"].Value = VendorPartnerName;
            sqlCmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar);
            sqlCmd.Parameters["@InvoiceNo"].Value = InvoiceNo;
            sqlCmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime);
            sqlCmd.Parameters["@InvoiceDate"].Value = InvoiceDate;
            sqlCmd.Parameters.Add("@PONumber", SqlDbType.VarChar);
            sqlCmd.Parameters["@PONumber"].Value = PONumber;
            sqlCmd.Parameters.Add("@OnBehalfID", SqlDbType.VarChar);
            sqlCmd.Parameters["@OnBehalfID"].Value = OnBehalfID;
            sqlCmd.Parameters.Add("@OnBehalfName", SqlDbType.VarChar);
            sqlCmd.Parameters["@OnBehalfName"].Value = OnBehalfName;
            sqlCmd.Parameters.Add("@LocationID", SqlDbType.Int);
            sqlCmd.Parameters["@LocationID"].Value = Location;
            sqlCmd.Parameters.Add("@BudgetHolderID", SqlDbType.Int);
            sqlCmd.Parameters["@BudgetHolderID"].Value = BudgetHolderID;

            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();
            //SqlTransaction sTrn;

            sTrn = sqlCon.BeginTransaction();

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sTrn;
            sqlCmd.ExecuteNonQuery();

            string RID = excScalar("SELECT IDENT_CURRENT('[PowerPayment].[dbo].[Request]')");

            sqlStr = "INSERT INTO [PowerPayment].[dbo].RequestDetails " +
            "SELECT " + RID + ", LineOrder, Narrative, Account, CostCenter, Project, SOF, " +
            "DEA, Analysis, Amount FROM PowerPayment.dbo.RequestDetailsTemp WHERE EmpID = '" + RequestorID + "'";
            sqlCmd = new SqlCommand(sqlStr);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sTrn;
            sqlCmd.ExecuteNonQuery();

            sTrn.Commit();
            sTrn.Dispose();

            sqlCon.Close();
            sqlCon.Dispose();

            //string sSQL = "DELETE FROM [PowerPayment].[dbo].[RequestDetailsTemp] WHERE EmpID='" + Convert.ToInt32(hfEID.Value) + "'";
            //dbo.ExectuteQuery(sSQL);

            // string sqlStr = "INSERT INTO PowerPayment.dbo.RequestDetailsTemp" +
            //"(EmpID, LineOrder, Narrative, Account, CostCenter, Project, SOF, DEA, Analysis, Amount) VALUES" +
            //"('" + Convert.ToInt32(hfEID.Value) + "','" + lOrder + "','" + tbxNarrative.Text.Replace("'", "") + "'," +
            //"'" + tbxAccount.Text.Replace("'", "") + "','" + cbxCostCenter.SelectedValue.Replace("'", "") + "','" + cbxProject.SelectedValue.Replace("'", "") + "'," +
            //"'" + cbxSOF.SelectedValue.Replace("'", "") + "','" + cbxDEA.SelectedValue.Replace("'", "") + "','" + tbxAnalysis.Text.Replace("'", "") + "','" + tbxAmount.Text + "')";
            //            dbo.ExectuteQuery(sqlStr);
        }

        public void postRequestDetails(int RequestorID, int LineOrder, string Narrative, string Account, 
            string CostCenter, string Project, string SOF, string DEA, string Analysis, double Amount)
        {
            if (deletedTemp == false) return;

            hasDetails = false;

            sqlStr = "INSERT INTO RequestDetailsTemp (EmpID, LineOrder, Narrative, " +
            " Account, CostCenter, Project, SOF, DEA, Analysis, Amount)" +
            " VALUES (@EmpID, @LineOrder, @Narrative, @Account," +
            " @CostCenter, @Project, @SOF, @DEA, @Analysis, @Amount)";

            sqlCmd = new SqlCommand(sqlStr);

            sqlCmd.Parameters.Add("@EmpID", SqlDbType.Int);
            sqlCmd.Parameters["@EmpID"].Value = RequestorID;
            sqlCmd.Parameters.Add("@LineOrder", SqlDbType.Int);
            sqlCmd.Parameters["@LineOrder"].Value = LineOrder;
            sqlCmd.Parameters.Add("@Narrative", SqlDbType.VarChar);
            sqlCmd.Parameters["@Narrative"].Value = Narrative;
            sqlCmd.Parameters.Add("@Account", SqlDbType.VarChar);
            sqlCmd.Parameters["@Account"].Value = Account;
            sqlCmd.Parameters.Add("@CostCenter", SqlDbType.VarChar);
            sqlCmd.Parameters["@CostCenter"].Value = CostCenter;
            sqlCmd.Parameters.Add("@Project", SqlDbType.VarChar);
            sqlCmd.Parameters["@Project"].Value = Project;
            sqlCmd.Parameters.Add("@SOF", SqlDbType.VarChar);
            sqlCmd.Parameters["@SOF"].Value = SOF;
            sqlCmd.Parameters.Add("@DEA", SqlDbType.VarChar);
            sqlCmd.Parameters["@DEA"].Value = DEA;
            sqlCmd.Parameters.Add("@Analysis", SqlDbType.VarChar);
            sqlCmd.Parameters["@Analysis"].Value = Analysis;
            sqlCmd.Parameters.Add("@Amount", SqlDbType.Decimal);
            sqlCmd.Parameters["@Amount"].Value = Amount;

            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();
            //SqlTransaction sTrn;

            sTrn = sqlCon.BeginTransaction();

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sTrn;
            sqlCmd.ExecuteNonQuery();

            sTrn.Commit();
            sTrn.Dispose();
            
            hasDetails = true;

            sqlCon.Close();
            sqlCon.Dispose();
        }

        public void postDeleteTemp(int RequestorID)
        {
            deletedTemp = false;

            sqlStr = "DELETE FROM RequestDetailsTemp WHERE EmpID=@EmpID";
            SqlCommand sqlCmd = new SqlCommand(sqlStr);

            sqlCmd.Parameters.Add("@EmpID", SqlDbType.Int);
            sqlCmd.Parameters["@EmpID"].Value = RequestorID;

            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();
            SqlTransaction sTrn;

            sTrn = sqlCon.BeginTransaction();

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sTrn;
            sqlCmd.ExecuteNonQuery();

            sTrn.Commit();
            sTrn.Dispose();

            deletedTemp = true;

            sqlCon.Close();
            sqlCon.Dispose();
        }
        //private void exeQuery(SqlCommand sqlCmd)
        //{
        //    sqlCon = new SqlConnection(cnStr);
        //    sqlCmd.CommandType = CommandType.Text;
        //    sqlCmd.Connection = sqlCon;
        //    try
        //    {
        //        sqlCon.Open();
        //        sqlCmd.ExecuteNonQuery();
        //        //return true;
        //    }
        //    catch //(Exception exp)
        //    {
        //        //Console.WriteLine(exp.Message);
        //        //return false;
        //    }
        //    finally
        //    {
        //        if (sqlCon.State == ConnectionState.Open)
        //        {
        //            sqlCon.Close();
        //        }
        //        sqlCon.Dispose();
        //    }
        //}
        private string excScalar(String sStr)
        {
            try
            {
                SqlConnection sqlCon1 = new SqlConnection(cnStr);
                sqlCon1.Open();

                SqlCommand sqlCmd1 = new SqlCommand(sStr, sqlCon1);
                string scalarData = Convert.ToString(sqlCmd1.ExecuteScalar());
                sqlCmd1.Dispose();
                sqlCon1.Close();
                sqlCon1.Dispose();
                return scalarData;
            }
            catch
            {
                return "";
            }
        }
    }
}