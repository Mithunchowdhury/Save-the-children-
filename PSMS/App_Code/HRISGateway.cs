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
    public class HRISGateway
    {
        //type Name
        //{
        //} 
        private String sqlStr;
        private String cnStr = "Data Source=10.12.0.2;Initial Catalog=HRIS;User Id=sa;Password=bdco;Connection Timeout=0;";
        private SqlConnection sqlCon;
        //private SqlCommand sqlCmd;
        //private SqlTransaction sTrn;
        private SqlDataAdapter sqlDA;
        private DataTable dTable;

        public enum empStatus
        {
            Active = 1,
            Inactive = 2,
            All = 3,
        };

        public void getComboList_ID_Name(empStatus eStatus, RadComboBox cbx)
        {
            sqlStr = "EXEC proc_Get_EmpInfoIdName '" + (int)eStatus + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            cbx.DataSource = dTable;
            cbx.DataTextField = "FullName";
            cbx.DataValueField = "EmpId";
            cbx.DataBind();

            sqlCon.Close();
            sqlCon.Dispose();
        }
        public void getComboList_ID_Name(empStatus eStatus, DropDownList cbx)
        {
            sqlStr = "EXEC proc_Get_EmpInfoIdName '" + (int)eStatus + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            cbx.DataSource = dTable;
            cbx.DataTextField = "FullName";
            cbx.DataValueField = "EmpId";
            cbx.DataBind();

            sqlCon.Close();
            sqlCon.Dispose();
        }
        public DataTable getData(empStatus eStatus)
        {
            sqlStr = "EXEC proc_Get_EmpInfoAll '" + (int) eStatus + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            sqlCon.Close();
            sqlCon.Dispose();

            return dTable;
        }
        public DataTable getData_Emp_ID_Name(empStatus eStatus)
        {
            sqlStr = "EXEC proc_Get_EmpInfoAll '" + (int)eStatus + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            sqlCon.Close();
            sqlCon.Dispose();

            return dTable;
        }
        public DataTable getDataSupervisorList(string empID)
        {
            sqlStr = "EXEC OnDesk.dbo.GetSupervisorHRIS2 '" + empID + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            sqlCon.Close();
            sqlCon.Dispose();

            return dTable;
        }
        public DataTable getData(string empID)
        {
            sqlStr = "EXEC proc_Get_EmpInfo '" + empID + "'";
            sqlCon = new SqlConnection(cnStr);
            sqlCon.Open();

            sqlDA = new SqlDataAdapter(sqlStr, sqlCon);
            dTable = new DataTable();
            sqlDA.Fill(dTable);

            sqlCon.Close();
            sqlCon.Dispose();

            return dTable;
        }
        public string getName(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["FullName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getNameFrist(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["FirstName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getNameMiddle(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["MiddleName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getNameLast(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["LastName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getGender(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["Gender"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getReligion(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["ReligionName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getBloodGroup(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["BloodGroupName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public DateTime getDOB(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            DateTime getName = Convert.ToDateTime(dTable.Rows[0]["DOB"]);
            dTable.Dispose();
            return getName;
        }
        public string getFatherName(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["FatherName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getMotherName(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["MotherName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPresentAddress(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PreAddress"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPresentPhone(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PrePhone"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPresentFax(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PreFax"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPermanentAddress(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PerAddress"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPermanentPhone(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PerPhone"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPermanentFax(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PerFax"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getOfficeEmail(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["OfficeEmail"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getDrivingLicense(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["DrivingLicense"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getLicenseRenewDate(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["LicenseRenewDate"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getTINNo(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["TINNo"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPassportNo(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PassportNo"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPassportExpiryDate(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PassExpDate"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getGrade(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["GradeName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getJobTitle(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["JobTitleName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getDesignation(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["DesigName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getSector(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["SectorName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getDepartment(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["DeptName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getUnit(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["UnitName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPositionFunction(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PosFuncName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPostingDivision(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PostingDivName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPostingDistrict(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PostingDistName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getPostingPlace(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["PostingPlaceName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getSalaryLocation(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["SalLocName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getSalarySubLocation(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["SalSubLocName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public DateTime getDOJ(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            DateTime getName = Convert.ToDateTime(dTable.Rows[0]["JoiningDate"]);
            dTable.Dispose();
            return getName;
        }
        public Boolean getStatus(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["EmpStatus"].ToString();
            dTable.Dispose();
            Boolean empStatus = false;
            if (getName == "A") { empStatus = true; } else { empStatus = false; }
            return empStatus;
        }
        public string getBankName(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["BankName"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getRoutingNo(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["RoutingNo"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getAccountNo(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["BankAccNo"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getSupervisorID(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["SupervisorId"].ToString();
            dTable.Dispose();
            return getName;
        }
        public string getSupervisorName(string empID)
        {
            dTable = new DataTable();
            dTable = getData(empID);
            string getName = dTable.Rows[0]["SupervisorName"].ToString();
            dTable.Dispose();
            return getName;
        }
    }
}