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
    public partial class ApprovalSetup : System.Web.UI.Page
    {
        string pagename = "";
        AppManager am = new AppManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = "";
            try
            {
                type = Page.Request.Params["type"].ToString();
            }
            catch (Exception ex)
            {
            }
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                LoadComboItems();
            }

            //If grants menu seleected
            if (type == "Grants")
            {
                pagename = "Grants Approval";
                appLabel.Text = "GRANTS APPROVAL";
                rdbGrantsApp.Checked = true;
                pnlTechApproval.Visible = false;
                pnlGrantsApproval.Visible = true;
                rdbGrantsApp.Visible = false;
                rdbTechApp.Visible = false;
            }
            //else default check tech
            else
            {
                pagename = "Technical Approval";
                appLabel.Text = "TECHNICAL APPROVAL";
                rdbTechApp.Checked = true;
                pnlTechApproval.Visible = true;
                pnlGrantsApproval.Visible = false;
                rdbGrantsApp.Visible = false;
                rdbTechApp.Visible = false;
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        #region Tech Approval
        protected void btnTechSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (hdfTechId.Value != "0")
            {
                id = int.Parse(hdfTechId.Value);
            }
            SaveTechRecord(id);
        }
        protected void btnTechReset_Click(object sender, EventArgs e)
        {
            //lblTechMsg.Text = "";
            ResetTechRecord(true);
        }
        protected void grdTech_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string sql = "select ta.*, vs.StaffName + ', ' + vs.Designation as StaffName from TechApproval ta ";
            sql += "left join viewStaffInfo vs ";
            sql += "on ta.TechApprovalID = vs.StaffCode";
            DataTable dt = am.DataAccess.RecordSet(sql, new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                grdTech.DataSource = dt;
            }
            else
            {
                grdTech.DataSource = null;
            }
        }
        protected void grdTech_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":
                    gdItem = (GridDataItem)e.Item;
                    EditTechRecord(gdItem);
                    e.Canceled = true;
                    break;
                case "Delete":
                    gdItem = (GridDataItem)e.Item;
                    DeleteTechRecord(gdItem);
                    e.Canceled = true;
                    break;
                case RadGrid.RebindGridCommandName:
                    grdTech.Rebind();
                    grdTech.CurrentPageIndex = 0;
                    break;
            }
        }
        private void SaveTechRecord(int id)
        {
            if (!ValidTechRecord())
            {
                return;
            }

            if (id > 0)
            {
                am.DataAccess.BatchQuery.Update("TechApproval", "TechReviewBy,Active",
                    new string[] { cbxTechReviewedBy.SelectedValue, (chkTechActive.Checked == true ? 1 : 0).ToString() },
                    "TechApprovalID=@TechApprovalID", new string[] { id.ToString() });
            }
            else
            {
                am.DataAccess.BatchQuery.Insert("TechApproval", "TechApprovalID,TechReviewBy,Active",
                    new string[] { cbxTechApproval.SelectedValue, cbxTechReviewedBy.SelectedValue, (chkTechActive.Checked == true ? 1 : 0).ToString() });
            }

            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfTechId.Value = "0";
                grdTech.Rebind();
                ResetTechRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
            else
            {
                am.Utility.ShowHTMLMessage(this.Page, "", AppUtility.UNSUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }
        private void EditTechRecord(GridDataItem gdItem)
        {
            int ID;
            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["TechApprovalID"]);
            string idStr = ID.ToString();

            DataTable dt = null;
            DataRow dr = null;

            dt = am.DataAccess.RecordSet("select * from TechApproval WHERE TechApprovalID=@TechApprovalID", new string[] { idStr });
            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }

            if (dr != null)
            {
                //Store Id
                hdfTechId.Value = idStr;

                cbxTechApproval.SelectedValue = dr["TechApprovalID"].ToString();
                cbxTechReviewedBy.SelectedValue = dr["TechReviewBy"].ToString();
                chkTechActive.Checked = Convert.ToBoolean(dr["Active"]);

                //disable 
                cbxTechApproval.Enabled = false;
                //cbxTechReviewedBy.Enabled = false;
            }
        }
        private void DeleteTechRecord(GridDataItem gdItem)
        {
            int ID;

            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["TechApprovalID"]);
            string reviewedByID = gdItem["TechReviewBy"].Text;
            am.DataAccess.BatchQuery.Delete("TechApproval", "TechApprovalID=@TechApprovalID and TechReviewBy=@TechReviewBy", 
                new string[] { ID.ToString() , reviewedByID });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfTechId.Value = "0";
                grdTech.Rebind();
                ResetTechRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
            else
            {
                am.Utility.ShowHTMLMessage(this.Page, "", AppUtility.UNSUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }
        private void ResetTechRecord(bool clearError)
        {
            if (clearError)
            {
                //lblTechMsg.Text = "";
            }

            cbxTechApproval.SelectedValue = "0";
            cbxTechReviewedBy.SelectedValue = "0";
            chkTechActive.Checked = true;

            hdfTechId.Value = "0";
            cbxTechApproval.Enabled = true;
            //cbxTechReviewedBy.Enabled = true;
        }
        #endregion

        #region Grants Approval
        protected void btnGrantsSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (hdfGrantsId.Value != "0")
            {
                id = int.Parse(hdfGrantsId.Value);
            }
            SaveGrantsRecord(id);
        }
        protected void btnGrantsReset_Click(object sender, EventArgs e)
        {
            //lblGrantsMsg.Text = "";
            ResetGrantsRecord(true);
        }
        protected void grdGrants_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string sql = "select ga.*, vs.StaffName + ', ' + vs.Designation as StaffName from GrantsApproval ga ";
            sql += "left join viewStaffInfo vs ";
            sql += "on ga.GrantsApprovalID = vs.StaffCode";
            DataTable dt = am.DataAccess.RecordSet(sql, new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                grdGrants.DataSource = dt;
            }
            else
            {
                grdGrants.DataSource = null;
            }
        }

        protected void grdGrants_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":
                    gdItem = (GridDataItem)e.Item;
                    EditGrantsRecord(gdItem);
                    e.Canceled = true;
                    break;
                case "Delete":
                    gdItem = (GridDataItem)e.Item;
                    DeleteGrantsRecord(gdItem);
                    e.Canceled = true;
                    break;
                case RadGrid.RebindGridCommandName:
                    grdGrants.Rebind();
                    grdGrants.CurrentPageIndex = 0;
                    break;
            }
        }

        private void SaveGrantsRecord(int id)
        {
            if (!ValidGrantsRecord())
            {
                return;
            }
            if (id > 0)
            {
                am.DataAccess.BatchQuery.Update("GrantsApproval", "Active", 
                    new string[] { (chkGrantsActive.Checked == true ? 1 : 0).ToString() }, "GrantsApprovalID=@GrantsApprovalID", new string[] { id.ToString() });
            }
            else
            {
                am.DataAccess.BatchQuery.Insert("GrantsApproval", "GrantsApprovalID,Active", 
                    new string[] { cbxGrantsApproval.SelectedValue , (chkGrantsActive.Checked == true ? 1 : 0).ToString() });
            }

            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfGrantsId.Value = "0";
                grdGrants.Rebind();
                ResetGrantsRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
            else
            {
                am.Utility.ShowHTMLMessage(this.Page, "", AppUtility.UNSUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }

        private void EditGrantsRecord(GridDataItem gdItem)
        {
            int ID;
            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["GrantsApprovalID"]);
            string idStr = ID.ToString();

            DataTable dt = null;
            DataRow dr = null;

            dt = am.DataAccess.RecordSet("select * from GrantsApproval WHERE GrantsApprovalID=@GrantsApprovalID", new string[] { idStr });
            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }

            if (dr != null)
            {
                //Store Id
                hdfGrantsId.Value = idStr;

                cbxGrantsApproval.SelectedValue = dr["GrantsApprovalID"].ToString();
                chkGrantsActive.Checked = Convert.ToBoolean(dr["Active"]);

                //disable 
                cbxGrantsApproval.Enabled = false;
            }
        }
        private void DeleteGrantsRecord(GridDataItem gdItem)
        {
            int ID;

            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["GrantsApprovalID"]);
            am.DataAccess.BatchQuery.Delete("GrantsApproval", "GrantsApprovalID=@GrantsApprovalID", new string[] { ID.ToString() });
            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfGrantsId.Value = "0";
                grdGrants.Rebind();
                ResetGrantsRecord(false);
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
            else
            {
                am.Utility.ShowHTMLMessage(this.Page, "", AppUtility.UNSUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
            }
        }

        private void ResetGrantsRecord(bool clearError)
        {
            if (clearError)
            {
                //lblGrantsMsg.Text = "";
            }

            cbxGrantsApproval.SelectedValue = "0";
            chkGrantsActive.Checked = true;

            hdfGrantsId.Value = "0";
            cbxGrantsApproval.Enabled = true;
        }
        #endregion

        private void LoadComboItems()
        {
            DataTable dt = am.DataAccess.RecordSet("SELECT StaffCode, StaffName + ', ' + Designation as StaffName FROM viewStaffInfo", new string[] { });
            if (dt != null)
            {
                DataRow dRow = dt.NewRow();
                dRow["StaffCode"] = "0";
                dRow["StaffName"] = "";
                dt.Rows.Add(dRow);

                cbxTechApproval.DataSource = dt;
                cbxTechApproval.DataTextField = "StaffName";
                cbxTechApproval.DataValueField = "StaffCode";
                cbxTechApproval.DataBind();
                cbxTechApproval.SelectedValue = "0";

                cbxGrantsApproval.DataSource = dt;
                cbxGrantsApproval.DataTextField = "StaffName";
                cbxGrantsApproval.DataValueField = "StaffCode";
                cbxGrantsApproval.DataBind();
                cbxGrantsApproval.SelectedValue = "0";
            }
        }

        protected void rdbGrantsApp_Click(object sender, EventArgs e)
        {
            if (rdbGrantsApp.Checked)
            {
                pnlTechApproval.Visible = false;
                pnlGrantsApproval.Visible = true;
                grdGrants.Rebind();
            }
            else
            {
                pnlTechApproval.Visible = true;
                pnlGrantsApproval.Visible = false;
                grdTech.Rebind();
            }
        }

        protected void rdbTechApp_Click(object sender, EventArgs e)
        {
            if (rdbTechApp.Checked)
            {
                pnlTechApproval.Visible = true;
                pnlGrantsApproval.Visible = false;
                grdTech.Rebind();
            }
            else
            {
                pnlTechApproval.Visible = false;
                pnlGrantsApproval.Visible = true;
                grdGrants.Rebind();
            }
        }

        private bool ValidTechRecord()
        {

            if (cbxTechApproval.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Technical Approval.");
                cbxTechApproval.Focus();
                return false;
            }
            if (cbxTechReviewedBy.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Reviewed By.");
                cbxTechReviewedBy.Focus();
                return false;
            }

            return true;
        }

        private bool ValidGrantsRecord()
        {
            if (cbxGrantsApproval.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Grants Approval.");
                cbxGrantsApproval.Focus();
                return false;
            }

            return true;
        }

    }
}