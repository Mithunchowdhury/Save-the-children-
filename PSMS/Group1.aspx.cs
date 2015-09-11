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
    public partial class Group1 : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        string pagename = "Group Information";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {

            }
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }


        protected void grd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            DataTable dt = am.DataAccess.RecordSet("SELECT *, iif(GroupInfo.Active = 1, \'Yes\', \'No\') as ActiveStr from GroupInfo", new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                grd.DataSource = dt;
            }
            else
            {
                grd.DataSource = null;
            }
        }

        protected void grd_ItemCommand(object sender, GridCommandEventArgs e)
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
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["GroupID"]);
                string idStr = ID.ToString();
                ShowRecord(idStr);
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void Delete(GridDataItem gdItem)
        {
            int ID;
            try
            {
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["GroupID"]);
                DeleteRecord(ID);
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void ShowRecord(string Id)
        {
            string str = "SELECT * FROM GroupInfo WHERE GroupID=@GroupID";
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
                    //Store Id
                    hdfId.Value = Id;

                    tbxName.Text = dr["GroupName"].ToString();
                    tbxDescription.Text = dr["Description"].ToString();
                    chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }



       
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord(true);

        }
        private void ResetRecord(bool clearError)
        {
            tbxName.Text = "";
            tbxDescription.Text = "";
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

            string groupName = tbxName.Text.Trim();
            string groupDesacription = tbxDescription.Text.Trim();
            bool active = chkActive.Checked;

            if (id > 0)
            {
                am.DataAccess.BatchQuery.Update("GroupInfo", "GroupName,Description,Active",
                    new string[] { groupName, groupDesacription, (active == true ? 1 : 0).ToString() }, "GroupID=@GroupID", new string[] { id.ToString() });
            }
            else
            {
                am.DataAccess.BatchQuery.Insert("GroupInfo", "GroupName,Description,Active",
                    new string[] { groupName, groupDesacription, (active == true ? 1 : 0).ToString() });
            }

            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                grd.Rebind();
                ResetRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }

        }

        private void DeleteRecord(int id)
        {

            if (id <= 0)
            {
                return;
            }

            am.DataAccess.BatchQuery.Delete("GroupInfo", "GroupID=@GroupID", new string[] { id.ToString() });
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
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(this.Page, "000", "Enter Group Name.");
                tbxName.Focus();
                return false;
            }

            return true;
        }



      
    }
}