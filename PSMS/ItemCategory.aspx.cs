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

    public partial class ItemCategory : System.Web.UI.Page
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
            string sqlStr = "SELECT ItemCategoryID, ItemCategoryName from ItemCategory";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { });

            if (dt != null && dt.Rows.Count > 0)
            {
                rlbItems.DataSource = dt;
                rlbItems.DataTextField = "ItemCategoryName";
                rlbItems.DataValueField = "ItemCategoryID";
                rlbItems.DataBind();
            }
            ResetRecord(clearError);
        }

        private void ShowRecord()
        {
            ResetRecord(true);
            string str = "SELECT * FROM ItemCategory WHERE ItemCategoryID=@pItemCategoryID";
            DataTable dt = am.DataAccess.RecordSet(str, new string[] { rlbItems.SelectedValue });
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfId.Value = rlbItems.SelectedValue;
                tbxName.Text = dt.Rows[0]["ItemCategoryName"].ToString();
                cbxCatType.SelectedValue = dt.Rows[0]["ItemCategoryType"].ToString();
                tbxDescription.Text = dt.Rows[0]["Description"].ToString();
                chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Active"]);
            }
        }


        private void SaveRecord()
        {
            if (!Valid())
            {
                return;
            }


            string itemName = tbxName.Text.Trim();
            string itemCategoryType = cbxCatType.SelectedValue;
            string description = tbxDescription.Text.Trim();
            string active = chkActive.Checked == true ? "1" : "0";

            string sqlStr = "";

            if (hdfId.Value != "0")
            {
                am.DataAccess.BatchQuery.Update("ItemCategory", "ItemCategoryName,ItemCategoryType,Description,Active",
                    new string[]{
                        itemName ,
                        itemCategoryType ,
                        description ,
                        active}, "WHERE ItemCategoryID=@ItemCategoryID", new string[] { hdfId.Value });

            }
            else
            {
                am.DataAccess.BatchQuery.Insert("ItemCategory", "ItemCategoryName,ItemCategoryType,Description,Active",
                new string[]{
                        itemName ,
                        itemCategoryType ,
                        description ,
                        active});
            }


            if (am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
                LoadRecord(false);
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
                bool deleted = false;

                try
                {


                    try
                    {
                        string sqlStr = "";


                        am.DataAccess.BatchQuery.Delete("ItemCategory", "WHERE ItemCategoryID=@ItemCategoryID", new string[] { hdfId.Value });
                        deleted = am.DataAccess.BatchQuery.Execute();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {

                }

                if (!deleted)
                {

                }
                else
                {
                    hdfId.Value = "0";
                    am.Utility.ShowHTMLAlert(Page, "000", "Deleted Successfully.");

                    LoadRecord(false);
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
            cbxCatType.SelectedValue = "0";
            tbxDescription.Text = "";
            chkActive.Checked = true;
        }

        private bool Valid()
        {
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Category Name.");
                tbxName.Focus();
                return false;
            }

            if (cbxCatType.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Category Type.");
                cbxCatType.Focus();
                return false;
            }

            return true;
        }



    }
}