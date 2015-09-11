using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS
{
    public partial class AuditLog : System.Web.UI.Page
    {
        AppManager am = new AppManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                LoadComboBox();
                am.Utility.LoadGrid(grdAuditLogs, "SELECT * FROM AuditLog",new string[]{});
                Reset();
            }
        }

        private void LoadComboBox()
        {
            am.Utility.LoadComboBox(cbxUser, "select UserName from UserInfo", "UserName", "UserName");
            am.Utility.LoadComboBox(cbxScreen, "select Name from AppResource where Active = 1", "Name", "Name");
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbxUser.SelectedValue.Length > 0 && cbxScreen.SelectedValue.Length > 0 && dprFrom.SelectedDate != null && dprTo.SelectedDate != null)
            {
                string query = "SELECT * FROM AuditLog where @FromDate <= CreatedDate and CreatedDate<=@ToDate and [USER]=@USER and SCREEN=@SCREEN";
                string[] whereValue = new string[]{ dprFrom.SelectedDate.Value.ToString() , dprTo.SelectedDate.Value.ToString() , 
                    cbxUser.SelectedValue , cbxScreen.SelectedValue };
                am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
            }
            else if (cbxScreen.SelectedValue.Length > 0 && cbxUser.SelectedValue.Length > 0)
            {
                string query = "SELECT * FROM AuditLog where [USER]=@USER and SCREEN=@SCREEN";
                string[] whereValue = new string[]{ cbxUser.SelectedValue , cbxScreen.SelectedValue };
                am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
            }
            else if(cbxScreen.SelectedValue.Length > 0 && dprFrom.SelectedDate != null && dprTo.SelectedDate != null)
            {
                string query = "SELECT * FROM AuditLog where @FromDate <= CreatedDate and CreatedDate<=@ToDate and SCREEN=@SCREEN";
                string[] whereValue = new string[]{ dprFrom.SelectedDate.Value.ToString() , dprTo.SelectedDate.Value.ToString() ,
                    cbxScreen.SelectedValue };
                am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
            }
            else if (cbxUser.SelectedValue.Length > 0 && dprFrom.SelectedDate != null && dprTo.SelectedDate != null)
            {
                string query = "SELECT * FROM AuditLog where @FromDate <= CreatedDate and CreatedDate<=@ToDate and [USER]=@USER";
                string[] whereValue = new string[]{ dprFrom.SelectedDate.Value.ToString() , dprTo.SelectedDate.Value.ToString() , 
                    cbxUser.SelectedValue };
                am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
            }
            else if (cbxUser.SelectedValue.Length > 0)
            {
                string query = "SELECT * FROM AuditLog where [USER]=@USER";
                string[] whereValue = new string[]{cbxUser.SelectedValue};
                am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
            }
            else if (cbxScreen.SelectedValue.Length > 0)
            {
                string query = "SELECT * FROM AuditLog where SCREEN=@SCREEN";
                string[] whereValue = new string[]{ cbxScreen.SelectedValue};
                am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
            }
            else if (dprFrom.SelectedDate != null || dprTo.SelectedDate != null)
            {
                if (dprFrom.SelectedDate != null && dprTo.SelectedDate != null)
                {
                    string query = "SELECT * FROM AuditLog where @FromDate <= CreatedDate and CreatedDate<=@ToDate";
                    string[] whereValue = new string[]{ dprFrom.SelectedDate.Value.ToString() , dprTo.SelectedDate.Value.ToString()};
                    am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
                }
                else if (dprFrom.SelectedDate != null)
                {
                    string query = "SELECT * FROM AuditLog where @FromDate <= CreatedDate";
                    string[] whereValue = new string[]{ dprFrom.SelectedDate.Value.ToString()};
                    am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
                }
                else if (dprTo.SelectedDate != null)
                {
                    string query = "SELECT * FROM AuditLog where CreatedDate<=@ToDate";
                    string[] whereValue = new string[]{ dprTo.SelectedDate.Value.ToString()};
                    am.Utility.LoadGrid(grdAuditLogs, query, whereValue );
                }
            }
            else
            {
                string query = "SELECT * FROM AuditLog";
                am.Utility.LoadGrid(grdAuditLogs, query, new string[] { });
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {            
            am.Utility.LoadGrid(grdAuditLogs, "SELECT * FROM AuditLog",new string[]{});
            Reset();
        }
        private void Reset()
        {
            cbxScreen.SelectedValue = "";
            cbxUser.SelectedValue = "";
            dprFrom.SelectedDate = null;
            dprTo.SelectedDate = null;
        }

        protected void grdAuditLogs_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            btnSearch_Click(null, null);
        }

        protected void grdAuditLogs_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
        {
            btnSearch_Click(null, null);
        }

        protected void grdAuditLogs_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Filter")
            {
                btnSearch_Click(null, null);
            }
        }
       

        

        
       
    }
}