using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

namespace PSMS.Reports
{
    public partial class Invitation : System.Web.UI.Page
    {
        AppManager am = new AppManager();       
        protected void Page_Load(object sender, EventArgs e)
        {
            GeneratePDF();
        }

        public void GeneratePDF()
        {
            string fileName = Request.QueryString["type"].ToString() + Request.QueryString["invId"].ToString() + "_";          

            DataTable[] dts= new DataTable[2];
            string[] tableNames = new string[2];
            tableNames[0] = "dtInvitation";
            dts[0] = getAllInvitations();
            tableNames[1] = "dtTerms";
            dts[1] = getAllTerms();
            

            am.Report.PrintReport(Page, "crInvitation.rpt", fileName, dts, tableNames);
        }

        public DataTable getAllInvitations()
        {
            DataTable dt = null;
            string invId = Request.QueryString["invId"].ToString();
            string query = "EXEC rptINV '" + invId + "'";
            dt = am.DataAccess.RecordSet(query, new string[] { });          
            return dt;
        }
        

        public DataTable getAllTerms()
        {
            DataTable dt = null;
            string invId = Request.QueryString["invId"].ToString();
            string query = "select [InvitationNo] as TermParentNo,Ordering,[TCNote] as Note "
                            +"from [dbo].[Invitation] inner join [dbo].[InvitationTerm] on [dbo].[Invitation].InvitationID=[dbo].[InvitationTerm].InvitationID "
                            + "where [dbo].[Invitation].InvitationID=@InvitationID order by Ordering";

            dt = am.DataAccess.RecordSet(query, new string[] { invId });            
            return dt;
        }

    }
}