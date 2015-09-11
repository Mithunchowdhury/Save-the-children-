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
    public partial class PCMember : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        HRISGateway hg = new HRISGateway();
        string pagename = "PC Member Information";
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
            DataTable dt = am.DataAccess.RecordSet("SELECT pm.*, vs.StaffName FROM PCMember pm left join viewStaffInfo vs on pm.StaffID = vs.StaffCode", new string[] { });
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
            if (id > 0)
            {
                am.DataAccess.BatchQuery.Update("PCMember", "Active", 
                    new string[] { (chkActive.Checked == true ? 1 : 0).ToString() }, "StaffID=@StaffID", new string[] { id.ToString() });
            }
            else
            {
                am.DataAccess.BatchQuery.Insert("PCMember", "StaffID,Active", 
                    new string[] { cbxStaff.SelectedValue , (chkActive.Checked == true ? 1 : 0).ToString() });
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
            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["StaffID"]);
            string idStr = ID.ToString();

            DataTable dt = null;
            DataRow dr = null;

            dt = am.DataAccess.RecordSet("SELECT * FROM PCMember WHERE StaffID=@StaffID", new string[] { idStr });
            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }

            if (dr != null)
            {
                //Store Id
                hdfId.Value = idStr;

                cbxStaff.SelectedValue = dr["StaffID"].ToString().PadLeft(5, '0');
                chkActive.Checked = Convert.ToBoolean(dr["Active"]);

                //disable 
                cbxStaff.Enabled = false;
            }
        }
        private void DeleteRecord(GridDataItem gdItem)
        {
            int ID;

            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["StaffID"]);
            am.DataAccess.BatchQuery.Delete("PCMember", "StaffID=@StaffID", new string[] { ID.ToString() });
            if(am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                grd.Rebind();
                ResetRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }

        private void ResetRecord(bool clearError)
        {
            cbxStaff.SelectedValue = "0";
            chkActive.Checked = true;

            hdfId.Value = "0";
            cbxStaff.Enabled = true;
        }

        private void LoadComboItems()
        {              
            hg.getComboList_ID_Name(HRISGateway.empStatus.All, cbxStaff);
        }
    }
}