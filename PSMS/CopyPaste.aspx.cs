using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class CopyPaste : System.Web.UI.Page
    {
        string pagename = "User Information";
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
                LoadComboItems();
            }

        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
        private void LoadComboItems()
        {
            am.Utility.LoadComboBox(cbxGroups, "SELECT GroupID, GroupName from GroupInfo where Active = 1", "GroupName", "GroupID");

            am.Utility.LoadComboBox(cbxLocations, "SELECT LocationID, LocationName from Location where Active = 1", "LocationName", "LocationID");
        }
        protected void grd_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = am.DataAccess.RecordSet("SELECT *,iif(UserInfo.Active = 1, \'Yes\', \'No\') as ActiveStr from UserInfo", new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                grd.DataSource = dt;
            }
            else
            {
                grd.DataSource = null;
            }
        }
        protected void grd_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":

                    gdItem = (GridDataItem)e.Item;
                    Edit(gdItem);
                    e.Canceled = true;
                    break;
                case "Delete":

                    gdItem = (GridDataItem)e.Item;
                    Delete(gdItem);
                    e.Canceled = true;
                    break;
                case RadGrid.RebindGridCommandName:
                    grd.Rebind();
                    grd.CurrentPageIndex = 0;
                    break;
            }
        }







        private void Edit(GridDataItem gdItem)
        {
            int ID;
            try
            {
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["UserID"]);
                string idStr = ID.ToString();
                ShowRecord(idStr);
            }
            catch (Exception ex)
            {

            }
        }
        private void ShowRecord(string Id)
        {
            string str = "SELECT * FROM UserInfo WHERE UserID=@UserID";
            try
            {
                DataTable dt = null;
                DataRow dr = null;
                dt = am.DataAccess.RecordSet(str, new string[] { Id });
                if (dt != null && dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
                if (dr != null)
                {
                    hdfId.Value = Id;

                    tbxUserName.Text = dr["UserName"].ToString();
                    tbxFullName.Text = dr["FullName"].ToString();
                    tbxStaffCode.Text = dr["StaffCode"].ToString();
                    cbxGroups.SelectedValue = dr["GroupID"].ToString();
                    cbxLocations.SelectedValue = dr["LocationID"].ToString();
                    chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                }
            }
            catch (Exception ex)
            {

            }

        }

     

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord(true);
        }
        private void ResetRecord(bool clearError)
        {
            tbxUserName.Text = "";
            tbxFullName.Text = "";
            tbxStaffCode.Text = "";
            cbxGroups.SelectedValue = "0";
            cbxLocations.SelectedValue = "0";
            chkActive.Checked = true;

            hdfId.Value = "0";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
        private void SaveRecord()
        {
            if (!Valid())
            {
                return;
            }

            int id = 0;
            if (hdfId.Value != "0")
            {
                id = int.Parse(hdfId.Value);
            }

            string userName = tbxUserName.Text.Trim();
            string password = "";//tbxPassword.Text.Trim();
            string fullName = tbxFullName.Text.Trim();
            string staffCode = tbxStaffCode.Text.Trim();
            int groupId = 0;
            try
            {
                groupId = int.Parse(cbxGroups.SelectedValue);
            }
            catch (Exception ex)
            {

            }
            int locationId = 0;
            try
            {
                locationId = int.Parse(cbxLocations.SelectedValue);
            }
            catch (Exception ex)
            {

            }
            bool active = chkActive.Checked;

            if (id > 0)
            {
                am.DataAccess.BatchQuery.Update("UserInfo", "UserName,Password,FullName,StaffCode,GroupID,LocationID,Active",
                    new string[] {userName , password , fullName , staffCode , groupId.ToString() , locationId.ToString() ,
                (active == true ? 1 : 0).ToString() }, "UserID=@UserID", new string[] { id.ToString() });
            }
            else
            {
                am.DataAccess.BatchQuery.Insert("UserInfo", "UserName,Password,FullName,StaffCode,GroupID,LocationID,Active",
                    new string[] { userName , password , fullName , staffCode , groupId.ToString() , locationId.ToString() ,
                (active == true ? 1 : 0).ToString()});
            }

            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                grd.Rebind();
                ResetRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }
        private void Delete(GridDataItem gdItem)
        {
            int ID;
            try
            {
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["UserID"]);
                DeleteRecord(ID);
            }
            catch (Exception ex)
            {

            }
        }
        private void DeleteRecord(int id)
        {
            if (id <= 0)
            {
                return;
            }
            am.DataAccess.BatchQuery.Delete("UserInfo", "UserID=@UserID", new string[] { id.ToString() });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                grd.Rebind();
                ResetRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }

        private bool Valid()
        {
            if (tbxUserName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(this.Page, "000", "Enter Login Name.");
                tbxUserName.Focus();
                return false;
            }
            if (tbxStaffCode.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(this.Page, "000", "Enter Staff Code.");
                tbxStaffCode.Focus();
                return false;
            }
            if (cbxGroups.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Group.");
                cbxGroups.Focus();
                return false;
            }
            if (cbxLocations.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Location.");
                cbxLocations.Focus();
                return false;
            }

            return true;
        }




    }
}