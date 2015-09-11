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
    public partial class Location : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        string pagename = "Location Information"; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (hdfId.Value != "0")
            {
                id = int.Parse(hdfId.Value);
            }
            SaveRecord(id);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord(true);
        }
        protected void grd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            DataTable dt = am.DataAccess.RecordSet("select * from Location", new string[] { });
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
                    EditRecord(gdItem);
                    e.Canceled = true;
                    break;
                case "Delete":
                    gdItem = (GridDataItem)e.Item;
                    DeleteRecord(gdItem);
                    e.Canceled = true;
                    break;
                case RadGrid.RebindGridCommandName:
                    grd.Rebind();
                    grd.CurrentPageIndex = 0;
                    break;
            }
        }

        private void SaveRecord(int id)
        {
            if (!Valid())
            {
                return;
            }
            if (id > 0)
            {
                am.DataAccess.BatchQuery.Update("Location", "LocationName,Address,Active",
                    new string[] {  tbxName.Text.Trim() , txtAddress.Text.Trim() ,
                    (chkActive.Checked == true ? 1 : 0).ToString() }, 
                    "LocationID=@LocationID", new string[] { id.ToString() });
            }
            else
            {
                am.DataAccess.BatchQuery.Insert("Location", "LocationName,Address,Active",
                    new string[] {  tbxName.Text.Trim() , txtAddress.Text.Trim() ,
                    (chkActive.Checked == true ? 1 : 0).ToString() });
            }

            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                grd.Rebind();
                ResetRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }

        private void EditRecord(GridDataItem gdItem)
        {
            int ID;
            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["LocationID"]);
            string idStr = ID.ToString();

            DataTable dt = null;
            DataRow dr = null;

            dt = am.DataAccess.RecordSet("select * from Location WHERE LocationID=@LocationID", new string[] { idStr });
            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }

            if (dr != null)
            {
                //Store Id
                hdfId.Value = idStr;

                tbxName.Text = dr["LocationName"].ToString();
                txtAddress.Text = dr["Address"].ToString();        
                chkActive.Checked = Convert.ToBoolean(dr["Active"]);
            }
        }
        private void DeleteRecord(GridDataItem gdItem)
        {
            int ID;

            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["LocationID"]);
            am.DataAccess.BatchQuery.Delete("Location", "LocationID=@LocationID", new string[] { ID.ToString() });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                grd.Rebind();
                ResetRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }

        private void ResetRecord(bool clearError)
        {
            tbxName.Text = "";
            txtAddress.Text = "";
            chkActive.Checked = true;
            hdfId.Value = "0";
        }

        private bool Valid()
        {
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Location Name.");
                tbxName.Focus();
                return false;
            }
            return true;
        }

    }
}