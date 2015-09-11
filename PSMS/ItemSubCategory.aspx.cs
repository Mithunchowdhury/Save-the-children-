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
    public partial class ItemSubCategory : System.Web.UI.Page
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
                LoadComboItems();
                //LoadRecordByCategoryId(true);
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
            //Inside load reset is done
            LoadRecordByCategoryId(true);
        }

        private void LoadComboItems()
        {
            try
            {
                string sqlStr = "SELECT ItemCategoryID, ItemCategoryName from ItemCategory";
                am.Utility.LoadComboBox(cbxCat, sqlStr, "ItemCategoryName", "ItemCategoryID");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
        }

        private void LoadRecordByCategoryId(bool clearError)
        {
            if (clearError)
            {

            }

            //Load list box
            try
            {               
                int catId = int.Parse(cbxCat.SelectedValue);
                if (catId > 0)
                {
                    string sqlStr = "SELECT SubCategoryID, SubCategoryName from ItemSubCategory WHERE CategoryID=@pCategoryID";
                    //dt = am.DataAccess.RecordSet(sqlStr, catId.ToString());
                    //string sqlStr = "SELECT SubCategoryID, SubCategoryName from ItemSubCategory";
                    dt = am.DataAccess.RecordSet(sqlStr, new string[] { catId.ToString() });
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        rlbItems.DataSource = dt;
                        rlbItems.DataTextField = "SubCategoryName";
                        rlbItems.DataValueField = "SubCategoryID";
                        rlbItems.DataBind();
                    }
                    else
                    {
                        rlbItems.Items.Clear();
                    }
                    ResetRecord(clearError, false);
                }
                else
                {
                    rlbItems.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }

        }

        private void ShowRecord()
        {
            ResetRecord(true, true);
            string str = "SELECT * FROM ItemSubCategory WHERE SubCategoryID=@pSubCategoryID";

            try
            {
                dt = new DataTable();
                dt = am.DataAccess.RecordSet(str, new string[] { rlbItems.SelectedValue });
                if (dt != null && dt.Rows.Count > 0)
                {
                    hdfId.Value = rlbItems.SelectedValue;
                    cbxCat.SelectedValue = dt.Rows[0]["CategoryID"].ToString();
                    tbxName.Text = dt.Rows[0]["SubCategoryName"].ToString();
                    tbxDescription.Text = dt.Rows[0]["Description"].ToString();
                    chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Active"]);

                    cbxCat.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
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
                string itemName = tbxName.Text.Trim();
                int itemCategory = int.Parse(cbxCat.SelectedValue);
                string description = tbxDescription.Text.Trim();
                string active = chkActive.Checked == true ? "1" : "0";

                if (hdfId.Value != "0")
                {
                    am.DataAccess.BatchQuery.Update("ItemSubCategory", "CategoryID,SubCategoryName,Description,Active",
                        new string[]{
                            itemCategory.ToString() ,
                            itemName ,
                            description ,
                            active
                        }, "SubCategoryID=@SubCategoryID", new string[] { hdfId.Value });
                }
                else
                {
                    am.DataAccess.BatchQuery.Insert("ItemSubCategory", "CategoryID,SubCategoryName,Description,Active",
                            new string[]
                            {
                                itemCategory.ToString() ,
                                itemName ,
                                description ,
                                active
                            });
                }

                if (am.DataAccess.BatchQuery.Execute())
                {
                    hdfId.Value = "0";
                    am.Utility.ShowHTMLAlert(this.Page, "000", "Saved Successfully.");
                    LoadRecordByCategoryId(false);
                }
                else
                {
                    am.Utility.ShowHTMLMessage(this.Page, "000", "Could not Save.");//lblMsg.Text = "Could not Save.";
                }

            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }

        }

        private void DeleteRecord()
        {
            try
            {
                if (hdfId.Value == "0") return;

                int id = int.Parse(hdfId.Value);

                am.DataAccess.BatchQuery.Delete("ItemSubCategory", "SubCategoryID=@SubCategoryID", new string[] { id.ToString() });
                if (am.DataAccess.BatchQuery.Execute())
                {
                    hdfId.Value = "0";
                    am.Utility.ShowHTMLAlert(this.Page, "00", "Deleted Successfully.");

                    LoadRecordByCategoryId(false);
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
        }

        private void ResetRecord(bool clearError, bool clearDependency)
        {
            hdfId.Value = "0";
            tbxName.Text = "";
            tbxDescription.Text = "";
            chkActive.Checked = true;
            if (clearDependency)
            {
                cbxCat.SelectedValue = "0";
            }
            cbxCat.Enabled = true;
        }

        protected void cbxCat_SelectedIndexChanged1(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {            
            LoadRecordByCategoryId(true);
        }

        private bool Valid()
        {
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLAlert(this.Page, "00", "Enter Subcategory Name.");
                tbxName.Focus();
                return false;
            }

            if (cbxCat.SelectedValue == "0")
            {
                am.Utility.ShowHTMLAlert(this.Page, "00", "Select a Category.");
                cbxCat.Focus();
                return false;
            }

            return true;
        }


    }
}