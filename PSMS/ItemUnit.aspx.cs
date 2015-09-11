using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace PSMS
{
    public partial class ItemUnit : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                LoadRecord(true);
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        protected void rlbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRecord();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord(true);
        }

        private void LoadRecord(bool clearError)
        {
            string sqlStr = "SELECT UnitID, UnitName FROM Unit ORDER BY UnitName ASC";

            dt = am.DataAccess.RecordSet(sqlStr, new string[] { });
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    rlbItems.DataSource = dt;
                    rlbItems.DataTextField = "UnitName";
                    rlbItems.DataValueField = "UnitID";
                    rlbItems.DataBind();
                }
                ResetRecord(clearError);
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }

        }

        private void ShowRecord()
        {
            ResetRecord(true);
            string str = "SELECT * FROM Unit WHERE UnitID=@UnitID";

            dt = new DataTable();
            try
            {

                dt = am.DataAccess.RecordSet(str, new string[] { rlbItems.SelectedValue });
                if (dt != null && dt.Rows.Count > 0)
                {
                    hdfId.Value = rlbItems.SelectedValue;
                    tbxName.Text = dt.Rows[0]["UnitName"].ToString();
                    chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Active"]);
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }
        }


        private void SaveRecord()
        {
            if (!Valid())
            {
                return;
            }

            try
            {
                string unitName = tbxName.Text.Trim();
                string active = chkActive.Checked == true ? "1" : "0";

                string sqlStr = "";
                if (hdfId.Value != "0")
                {
                    am.DataAccess.BatchQuery.Update("Unit", "UnitName,Active",
                        new string[] { unitName, active }, "UnitID=@UnitID", new string[] { hdfId.Value.ToString() });
                }
                else
                {
                    am.DataAccess.BatchQuery.Insert("Unit", "UnitName,Active", new string[] { unitName, active });
                }


                if (am.DataAccess.BatchQuery.Execute())
                {
                    hdfId.Value = "0";
                    am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
                    LoadRecord(false);
                }
                else
                {
                    am.Utility.ShowHTMLAlert(Page, "000", "Could not Save.");
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }

        }

        private void DeleteRecord()
        {
            try
            {
                if (hdfId.Value == "0")
                {
                    return;
                }

                int id = int.Parse(hdfId.Value);

                string sqlStr = "DELETE FROM Unit WHERE UnitID=@pUnitID";

                am.DataAccess.BatchQuery.Delete("Unit", "UnitID=@UnitID", new string[] { id.ToString() });
                if (am.DataAccess.BatchQuery.Execute())
                {
                    LoadRecord(false);
                    am.Utility.ShowHTMLAlert(Page, "000", "Deleted Successfully.");
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }

        }

        private void ResetRecord(bool clearError)
        {

            hdfId.Value = "0";
            tbxName.Text = "";
            chkActive.Checked = true;
        }

        private bool Valid()
        {
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Unit Name");
                tbxName.Focus();
                return false;
            }

            return true;
        }



    }
}