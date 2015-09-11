using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{

    //Add DataKeyNames="ColumnName" in RadGrid.
    //Remove Auto Generated Column Name&Value from add/update


    public partial class Test : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), "/Donor.aspx");
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                am.Utility.LoadComboBox(cboUserID, "SELECT UserID, UserName FROM UserInfo", "UserName", "UserID");
                am.Utility.LoadComboBox(cboGroupID, "SELECT GroupID, GroupName FROM GroupInfo", "GroupName", "GroupID");
                am.Utility.LoadComboBox(cboLocationID, "SELECT LocationID, LocationName FROM Location", "LocationName", "LocationID");
                am.Utility.LoadGrid(grdUserInfo, "SELECT * FROM UserInfo", new string[] { });
            }
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
        protected void grd_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":
                    gdItem = (GridDataItem)e.Item;
                    string editId = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["UserID"].ToString();
                    ShowRecord(editId);
                    e.Canceled = true;
                    break;
                case "Delete":
                    gdItem = (GridDataItem)e.Item;
                    string delId = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["UserID"].ToString();
                    DeleteRecord(delId);
                    e.Canceled = true;
                    break;
            }
        }
        private void AddRecord()
        {
            am.DataAccess.BatchQuery.Insert("UserInfo", "UserName,FullName,StaffCode,GroupID,LocationID,Active", new string[] { tbxUserName.Text, tbxFullName.Text, tbxStaffCode.Text, cboGroupID.SelectedValue, cboLocationID.SelectedValue, cbxActive.Checked == true ? "1" : "0" });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
                am.Utility.LoadGrid(grdUserInfo, "SELECT * FROM UserInfo", new string[] { });
            }
        }
        private void UpdateRecord()
        {
            am.DataAccess.BatchQuery.Update("UserInfo", "UserName,FullName,StaffCode,GroupID,LocationID,Active", new string[] { tbxUserName.Text, tbxFullName.Text, tbxStaffCode.Text, cboGroupID.SelectedValue, cboLocationID.SelectedValue, cbxActive.Checked == true ? "1" : "0" }, "WHERE UserID=@UserID", new string[] { hdfId.Value });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
                am.Utility.LoadGrid(grdUserInfo, "SELECT * FROM UserInfo", new string[] { });
            }
        }
        private void DeleteRecord(string Id)
        {
            am.DataAccess.BatchQuery.Delete("UserInfo", "WHERE UserID=@UserID", new string[] { Id });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                am.Utility.ShowHTMLAlert(Page, "000", "Deleted Successfully.");
                am.Utility.LoadGrid(grdUserInfo, "SELECT * FROM UserInfo", new string[] { });
            }
        }
        private void ResetRecord()
        {
            cboUserID.SelectedValue = "0";
            tbxUserName.Text = "";
            tbxPassword.Text = "";
            tbxFullName.Text = "";
            tbxStaffCode.Text = "";
            cboGroupID.SelectedValue = "0";
            cboLocationID.SelectedValue = "0";
            cbxActive.Checked = true;
            hdfId.Value = "0";
        }
        private void ShowRecord(string UserID)
        {
            DataTable dt = am.DataAccess.RecordSet("SELECT * FROM UserInfo WHERE UserID=@UserID", new string[] { UserID });
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfId.Value = dt.Rows[0][0].ToString();
                cboUserID.SelectedValue = dt.Rows[0]["UserID"].ToString();
                tbxUserName.Text = dt.Rows[0]["UserName"].ToString();
                tbxPassword.Text = dt.Rows[0]["Password"].ToString();
                tbxFullName.Text = dt.Rows[0]["FullName"].ToString();
                tbxStaffCode.Text = dt.Rows[0]["StaffCode"].ToString();
                cboGroupID.SelectedValue = dt.Rows[0]["GroupID"].ToString();
                cboLocationID.SelectedValue = dt.Rows[0]["LocationID"].ToString();
                cbxActive.Checked = Convert.ToBoolean(dt.Rows[0]["Active"]);
            }
        }
        private bool Valid()
        {
            return true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hdfId.Value != "0")
            {
                UpdateRecord();
            }
            else
            {
                AddRecord();
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRecord(hdfId.Value);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord();
        }

    }


}