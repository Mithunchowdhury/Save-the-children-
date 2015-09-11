using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS.Reports
{
    public partial class GRN : System.Web.UI.Page
    {
        AppManager am = new AppManager();       
        protected void Page_Load(object sender, EventArgs e)
        {
            GeneratePDF();            
        }

        public void GeneratePDF()
        {
            string type = Request.QueryString["type"];
            string fileName = Request.QueryString["grnNo"].ToString();

            DataTable[] dts = new DataTable[1];
            dts[0] = getAllGRNs();
            string[] tableNames = new string[1];
            tableNames[0] = "dtGRN";
            if (type == "GRN")
            {
                am.Report.PrintReport(Page, "crGRN.rpt", fileName, dts, tableNames);
            }
            else if (type == "SRN")
            {
                am.Report.PrintReport(Page, "crSRN.rpt", fileName, dts, tableNames);
            }
        }       

        private DataTable getAllGRNs()
        {
            DataTable dt = null;
            string grnId = Request.QueryString["grnId"].ToString();

            string query = "EXEC rptGRN " + grnId + "";

            try
            {
                dt = am.DataAccess.RecordSet(query, new string[] { });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }            
            return dt;
        }


    }
}