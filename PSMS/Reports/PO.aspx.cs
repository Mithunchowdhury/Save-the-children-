using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class PO : System.Web.UI.Page
    {
        AppManager am = new AppManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            GeneratePDF();
        }

        public void GeneratePDF()
        {
            string fileName = "PO" + Request.QueryString["poId"].ToString() + "_";
           
            DataTable[] dts = new DataTable[2];
            dts[0] = getAllPOs();
            dts[1] = getAllTerms();
            string[] tableNames = new string[2];
            tableNames[0] = "dtPO";
            tableNames[1] = "dtTerms";
            am.Report.PrintReport(Page, "crPO.rpt", fileName, dts, tableNames);           
        }

        public DataTable getAllPOs()
        {
            DataTable dt = null;
            string poId = Request.QueryString["poId"].ToString();
            string query = "EXEC rptPO '" + poId + "'";

            dt = am.DataAccess.RecordSet(query, new string[] { });          
            return dt;
        }

        

        public DataTable getAllTerms()
        {
            DataTable dt = null;
            string poId = Request.QueryString["poId"].ToString();
            string query = "select [dbo].[PO].PONO as TermParentNo,[Ordering], [dbo].[POTerms].TCNote as [Note] " +
                "from [dbo].[PO],[dbo].[POTerms] where [dbo].[PO].[POID]=[dbo].[POTerms].POID " +
                "and [dbo].[PO].[POID]=@POID order by [Ordering]";                

            dt = am.DataAccess.RecordSet(query, new string[] { poId }); 
            return dt;
        }

    }
}