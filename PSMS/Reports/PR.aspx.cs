using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS.Reports
{
    public partial class PR : System.Web.UI.Page
    {
        AppManager am = new AppManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["prNo"] != null)
            {
                string prNo = Request.QueryString["prNo"].ToString();
                am.Report.PrintReport(Page, "PRNew.rpt", prNo, null, null, new string[] { "@PR" }, new string[] { prNo });
                
            }            
        }
        
    }
}