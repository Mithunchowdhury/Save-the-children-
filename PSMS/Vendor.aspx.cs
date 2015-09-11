using System;
using System.Collections;
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
    public partial class Vendor : System.Web.UI.Page
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
                LoadVendorCategories("-1");
            }
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
        protected void grdVendors_NeedDataSource(object sender, EventArgs e)
        {
            string sqlStr = ""; // "SELECT * from VendorInfo ORDER BY VendorName";
            sqlStr += "SELECT *,ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(STUFF( ";
            sqlStr += " (select ic.ItemCategoryName as Category ";
            sqlStr += " from VendorCategory as vn inner join ";
            sqlStr += " ItemCategory as ic ";
            sqlStr += " on ic.ItemCategoryID = vn.ItemCategoryID ";
            sqlStr += " where ic.ItemCategoryID in( ";
            sqlStr += " select distinct ven.ItemCategoryID ";
            sqlStr += " from VendorCategory as ven ";
            sqlStr += " where ven.VendorID = vi.VendorID) ";
            sqlStr += " and vn.VendorID = vi.VendorID FOR XML PATH ('')) ";
            sqlStr += " , 1, 0, ''), '</Category><Category>',','),'<Category>',''), '</Category>',''),'&amp;','&'),'')  AS Category ";
            sqlStr += " from VendorInfo as vi ";
            sqlStr += " ORDER BY VendorName ";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                grdVendors.DataSource = dt;

            }
        }

        protected void grdVendors_ItemCommand(object sender, GridCommandEventArgs e)
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
                    grdVendors.Rebind();
                    grdVendors.CurrentPageIndex = 0;
                    break;
            }
        }

        private void Edit(GridDataItem gdItem)
        {
            int ID;
            try
            {
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["VendorID"]);
                string idStr = ID.ToString();
                ShowRecord(idStr);
                tabMain.SelectedIndex = 0;
                mpgPages.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }
        }

        private void ShowRecord(string Id)
        {
            string str = "SELECT * FROM VendorInfo WHERE VendorID=@VendorID";

            try
            {
                DataTable dt = am.DataAccess.RecordSet(str, new string[] { Id });

                hdfId.Value = Id;

                tbxCode.Text = dt.Rows[0]["VendorCode"].ToString();
                tbxName.Text = dt.Rows[0]["VendorName"].ToString();
                tbxAddress.Text = dt.Rows[0]["Address"].ToString();
                tbxPostalCode.Text = dt.Rows[0]["PostCode"].ToString();
                tbxCity.Text = dt.Rows[0]["City"].ToString();
                tbxCountry.Text = dt.Rows[0]["Country"].ToString();
                tbxFax.Text = dt.Rows[0]["Fax"].ToString();
                tbxWebsite.Text = dt.Rows[0]["WebSite"].ToString();
                tbxEmail.Text = dt.Rows[0]["Email"].ToString();
                string listed = dt.Rows[0]["VendorType"].ToString();
                cbxVendorType.SelectedValue = listed;
                chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Active"]);
            }
            catch (Exception ex)
            {

            }

            LoadVendorCategories(hdfId.Value);
        }

        private void Delete(GridDataItem gdItem)
        {
            int ID;
            ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["VendorID"]);
            DeleteRecord(ID);
        }

        private void DeleteRecord(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return;
                }
                bool deleted = false;

                try
                {
                    try
                    {
                        string sqlStr = "";

                        am.DataAccess.BatchQuery.Delete("VendorCategory", "WHERE VendorID=@VendorID", new string[] { id.ToString() });
                        if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                        {
                            am.DataAccess.BatchQuery.Delete("VendorInfo", "WHERE VendorID=@VendorID", new string[] { id.ToString() });
                            deleted = am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close);
                        }                        
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
                    am.Utility.ShowHTMLAlert(Page, "000", "Deleted Successfully.");
                    grdVendors.Rebind();
                    ResetRecord(false);
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void SaveRecord()
        {
            if (!Valid())
            {
                return;
            }
            //Check for Duplicate records
            bool vendorSaved = false;
            bool categorySaved = true;

            string vendorId = "0";
            try
            {
                string vendorCode = tbxCode.Text.Trim();
                string vendorName = tbxName.Text.Trim();
                string vendorAddress = tbxAddress.Text.Trim();
                string vendorPostCode = tbxPostalCode.Text.Trim();
                string vendorCity = tbxCity.Text.Trim();
                string vendorCountry = tbxCountry.Text.Trim();
                string vendorFax = tbxFax.Text.Trim();
                string vendorWebsite = tbxWebsite.Text.Trim();
                string vendorEmail = tbxEmail.Text.Trim();
                string listedStr = cbxVendorType.SelectedValue;
                string active = (chkActive.Checked == true ? 1 : 0).ToString();

                if (hdfId.Value != "0")
                {
                    am.DataAccess.BatchQuery.Update("VendorInfo", "VendorCode,VendorName,VendorType,Address,PostCode,Email,City,Country,Fax,WebSite,Active",
                    new string[] { vendorCode , vendorName , listedStr , vendorAddress , vendorPostCode ,
                    vendorEmail , vendorCity , vendorCountry , vendorFax , vendorWebsite , active },
                    "WHERE VendorID=@VendorID", new string[] { hdfId.Value });

                    vendorId = hdfId.Value;
                }
                else
                {
                    am.DataAccess.BatchQuery.Insert("VendorInfo", "VendorCode,VendorName,VendorType,Address,PostCode,Email,City,Country,Fax,WebSite,Active",
                    new string[] { vendorCode , vendorName , listedStr , vendorAddress , vendorPostCode ,
                    vendorEmail , vendorCity , vendorCountry , vendorFax , vendorWebsite , active }, "VendorID");
                }
                if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                {
                    vendorSaved = true;
                    vendorId = am.DataAccess.ActiveIdentity;
                    
                    am.DataAccess.BatchQuery.Delete("VendorCategory", "WHERE VendorID=@VendorID", new string[] { vendorId });
                    foreach (GridDataItem row in grdVendorCategory.MasterTableView.Items)
                    {
                        bool rowChecked = (row.FindControl("chkSelected") as CheckBox).Checked;
                        if (rowChecked)
                        {
                            string rowCategoryId = row["CategoryID"].Text;
                            string rowCategory = row["Category"].Text;
                            string rowPerson = (row.FindControl("tbxContactPerson") as RadTextBox).Text;
                            string rowDesignation = (row.FindControl("tbxContactDesignation") as RadTextBox).Text;
                            string rowPhone = (row.FindControl("tbxContactPhone") as RadTextBox).Text;
                            string rowEmail = (row.FindControl("tbxContactEmail") as RadTextBox).Text;

                            am.DataAccess.BatchQuery.Insert("VendorCategory", "VendorID,ItemCategoryID,ContactPerson,Designation,Phone,Email",
                            new string[] { vendorId , rowCategoryId , rowPerson , rowDesignation , rowPhone , rowEmail }); 
                        }
                    }
                    if (!am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                    {
                        categorySaved = false;
                    }                    
                }

                if (vendorSaved && categorySaved)
                {
                    hdfId.Value = "0";
                    am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");

                    //Reload
                    grdVendors.Rebind();
                    ResetRecord(false);
                }
                else
                {
                    if (!vendorSaved)
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "Failed to Save Vendor Information.");
                    }
                    else if (!categorySaved)
                    {
                        hdfId.Value = vendorId;
                        //Reload
                        grdVendors.Rebind();
                        am.Utility.ShowHTMLMessage(Page, "000", "Failed to Save Vendor Category.");
                    }
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord(true);
        }

        private void ResetRecord(bool clearError)
        {
            if (clearError)
            {

            }

            tbxCode.Text = "";
            tbxName.Text = "";
            tbxAddress.Text = "";
            tbxPostalCode.Text = "";
            tbxCity.Text = "";
            tbxCountry.Text = "";
            tbxFax.Text = "";
            tbxWebsite.Text = "";
            tbxEmail.Text = "";
            cbxVendorType.SelectedValue = "";

            hdfId.Value = "0";
            chkActive.Checked = true;
            LoadVendorCategories("-1");
        }

        private void LoadVendorCategories(string vendorId)
        {
            string str = "";
            //
            //only which vendor provides
            str += " select CAST(1 AS BIT) as IsChecked, ic.ItemCategoryID as CategoryID, ic.ItemCategoryName as Category, ";
            str += " vn.ContactPerson as ContactPerson, vn.Designation as ContactDesignation, vn.Phone as ContactPhone, vn.Email as ContactEmail ";
            str += " from ItemCategory as ic inner join VendorCategory as vn ";
            str += " on ic.ItemCategoryID = vn.ItemCategoryID ";
            str += " where ic.ItemCategoryID in( ";
            str += " select distinct ven.ItemCategoryID ";
            str += " from VendorCategory as ven ";
            str += " where ven.VendorID = @VendorID) ";
            str += " and vn.VendorID = @VendorID1 ";
            str += " union all ";
            //only which vendor not provides
            str += " select CAST(0 AS BIT) as IsChecked, ic.ItemCategoryID as CategoryID, ic.ItemCategoryName as Category, ";
            str += " '' as ContactPerson, '' as ContactDesignation, '' as ContactPhone, '' as ContactEmail ";
            str += " from ItemCategory as ic ";
            str += " where ic.ItemCategoryID not in( ";
            str += " select distinct ven.ItemCategoryID ";
            str += " from VendorCategory as ven ";
            str += " where ven.VendorID = @VendorID2) ";

            if (vendorId.Trim().Length <= 0)
            {
                vendorId = "0";
            }

            DataTable dt = am.DataAccess.RecordSet(str, new string[] { vendorId , vendorId , vendorId });
            if (dt != null && dt.Rows.Count > 0)
            {

                grdVendorCategory.DataSource = dt;
                grdVendorCategory.DataBind();
            }
        }

        private void GetCategorySelection()
        {
            foreach (GridDataItem row in grdVendorCategory.MasterTableView.Items)
            {
                bool rowChecked = (row.FindControl("chkSelected") as CheckBox).Checked;
                string rowCategoryId = row["CategoryID"].Text;
                string rowCategory = row["Category"].Text;
                string rowPerson = (row.FindControl("tbxContactPerson") as RadTextBox).Text;
                string rowDesignation = (row.FindControl("tbxContactDesignation") as RadTextBox).Text;
                string rowPhone = (row.FindControl("tbxContactPhone") as RadTextBox).Text;
                string rowEmail = (row.FindControl("tbxContactEmail") as RadTextBox).Text;
            }
        }

        private bool Valid()
        {
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Vendor Name.");
                tbxName.Focus();
                return false;
            }
            if (tbxCode.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Vendor Code.");
                tbxCode.Focus();
                return false;
            }

            if (cbxVendorType.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Vendor Type.");
                cbxVendorType.Focus();
                return false;
            }

            return true;
        }

        protected void grdVendorCategory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    foreach (GridColumn column in item.OwnerTableView.RenderColumns)
            //    {

            //        if (column.UniqueName == "ContactPhone" ||
            //            column.UniqueName == "ContactDesignation")
            //        {
            //            TextBox tbEditBox = new TextBox();
            //            tbEditBox.Text = item[column].Text;

            //            //tbEditBox.CssClass = "gridEditItem";
            //            //tbEditBox.Width = Unit.Pixel(50);
            //            tbEditBox.TextChanged += new EventHandler(tbEditBox_TextChanged);
            //            tbEditBox.AutoPostBack = true;
            //            item[column].Controls.Clear();
            //            item[column].Controls.Add(tbEditBox);
            //        }

            //    }
            //}  
        }

        protected void tbEditBox_TextChanged(object sender, EventArgs e)
        {
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
            //TextBox txt = (TextBox)currentRow.FindControl("tbxContactPhone");
            //TextBox txt1 = (TextBox)currentRow.FindControl("tbxContactDesignation");
            ////TextBox txt3 = 
            ////Int32 count = Convert.ToInt32(txt.Text);
            ////txt.Text = Convert.ToString(count + 10);

        }
    }

}