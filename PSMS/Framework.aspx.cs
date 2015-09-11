using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class Framework : System.Web.UI.Page
    {
        string pagename = "Framework Information";
        string fileUploadPrefix = "FWA_";
        string FWADownloadURL = "http://softdev/scms/Attachment/FrameworkAttachment/";
        string uploadFolder = "/Attachment/FrameworkAttachment";
        string fileUploadLocation = "";

        AppManager am = new AppManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            fileUploadLocation = Server.MapPath(uploadFolder);
            //fileUploadLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\psms\\bin\\FrameworkAttachment";

            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                tbxFWNo.Text = GenerateRefNo();
                LoadAllFrameworks();
                LoadComboBoxes();
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
        private void LoadAllFrameworks()
        {
            string sqlStr = "";
            sqlStr += " select fw.FrameWorkID, fw.FrameWorkNo, ic.ItemCategoryName as CategoryName, ";
            sqlStr += " vi.VendorName as VendorName, ui.FullName as PreparedPerson, fw.StartDate, fw.EndDate, ";
            sqlStr += " CAST(CASE WHEN fw.Active = 1 THEN 'Active' ELSE 'Expired' END AS varchar(10)) as Action ";
            sqlStr += " from FrameWork fw left join ItemCategory ic ";
            sqlStr += " on fw.CategoryID = ic.ItemCategoryID ";
            sqlStr += " left join VendorInfo vi ";
            sqlStr += " on fw.VendorID = vi.VendorID ";
            sqlStr += " left join UserInfo ui ";
            sqlStr += " on fw.PreparedPerson = ui.StaffCode ";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                grdFWs.DataSource = dt;
                grdFWs.DataBind();
            }
        }

        protected void grdFWs_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":
                    gdItem = (GridDataItem)e.Item;
                    Edit(gdItem);
                    e.Canceled = true;
                    break;
                case "Download":
                    gdItem = (GridDataItem)e.Item;
                    DownloadAllFilesAsZip(gdItem);
                    e.Canceled = true;
                    break;
                case "Action":
                    gdItem = (GridDataItem)e.Item;
                    GridColumn gc = grdFWs.Columns[8];
                    string ID = gdItem.GetDataKeyValue("FrameWorkID").ToString();
                    string actionText = (gdItem[gc].Controls[0] as IButtonControl).Text;
                    if (actionText == "Active")
                    {
                        string strFields = "Active";
                        string strValues = "0";
                        am.DataAccess.BatchQuery.Update("FrameWork", strFields, new string[] { strValues }, "FrameWorkID=@FrameWorkID", new string[] { ID });
                        if (am.DataAccess.BatchQuery.Execute())
                        {
                            am.Utility.ShowHTMLAlert(this.Page, "000", "Framework expired.");
                            LoadAllFrameworks();
                        }
                    }
                    else if (actionText == "Expired")
                    {
                        string strFields = "Active";
                        string strValues = "1";
                        am.DataAccess.BatchQuery.Update("FrameWork", strFields, new string[] { strValues }, "FrameWorkID=@FrameWorkID", new string[] { ID });
                        if (am.DataAccess.BatchQuery.Execute())
                        {
                            am.Utility.ShowHTMLAlert(this.Page, "000", "Framework activated.");
                            LoadAllFrameworks();
                        }
                    }
                    e.Canceled = true;
                    break;
                case "Page":
                case "Filter":
                    LoadAllFrameworks();
                    break;
            }
        }

        private void Edit(GridDataItem gdItem)
        {
            int ID;
            try
            {
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FrameWorkID"]);
                string idStr = ID.ToString();
                ShowRecord(idStr);
            }
            catch (Exception ex)
            {

            }
        }

        private void ShowRecord(string Id)
        {
            string str = "SELECT * FROM FrameWork WHERE FrameWorkID=@FrameWorkID";

            try
            {
                DataTable dt = am.DataAccess.RecordSet(str, new string[] { Id });

                hdfId.Value = Id;

                tbxFWNo.Text = dt.Rows[0]["FrameWorkNo"].ToString();
                cbxCategoryType.SelectedValue = "0";
                cbxCategoryType.SelectedValue = dt.Rows[0]["CategoryID"].ToString();
                cbxCategoryType_SelectedIndexChanged(null, null);
                cbxVendor.SelectedValue = dt.Rows[0]["VendorID"].ToString();
                dtpFWStartDate.SelectedDate = DateTime.Parse(dt.Rows[0]["StartDate"].ToString());
                dtpFWSEndDate.SelectedDate = DateTime.Parse(dt.Rows[0]["EndDate"].ToString());
                dtpFWSSignDate.SelectedDate = DateTime.Parse(dt.Rows[0]["SignDate"].ToString());
                cbxProcessPerson.SelectedValue = dt.Rows[0]["PreparedPerson"].ToString();
                bool multiLocation = bool.Parse(dt.Rows[0]["Multilocation"].ToString());
                if (multiLocation)
                    chkMultipleLocation.Checked = true;
                else
                    chkMultipleLocation.Checked = false;
                bool active = bool.Parse(dt.Rows[0]["Active"].ToString());
                if (active)
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;
            }
            catch (Exception ex)
            {

            }

            LoadFrameworkItems(hdfId.Value);
            LoadFrameworkAttachments(hdfId.Value);
        }

        private void LoadFrameworkItems(string FrameowkrID)
        {
            string str = "SELECT fwi.*, ii.ItemName as ItemName, UnitIn as UnitName FROM FrameWorkItem fwi";
            str += " LEFT JOIN ItemInfo ii ON fwi.ItemID=ii.ItemID";
            str += " WHERE FrameWorkID=@FrameWorkID";
            if (FrameowkrID.Trim().Length <= 0)
            {
                FrameowkrID = "0";
            }
            DataTable dt = am.DataAccess.RecordSet(str, new string[] { FrameowkrID });
            rgdFWItems.DataSource = dt;
            rgdFWItems.DataBind();
        }
        private void LoadFrameworkAttachments(string FrameowkrID)
        {
            string str = "SELECT *,SUBSTRING(FrameWorkAttachLocation,33,LEN(FrameWorkAttachLocation)) as FileName FROM FrameworkAttach WHERE FrameWorkID=@FrameWorkID";
            if (FrameowkrID.Trim().Length <= 0)
            {
                FrameowkrID = "0";
            }
            DataTable dt = am.DataAccess.RecordSet(str, new string[] { FrameowkrID });
            rgdAttachment.DataSource = dt;
            rgdAttachment.DataBind();
        }
        #region Combo Loading
        private void LoadComboBoxes()
        {
            LoadPreparedPersons();
            LoadUnits();
            LoadCategory();
        }

        private void LoadPreparedPersons()
        {
            try
            {
                string sqlStr = "SELECT StaffCode, FullName from UserInfo WHERE (StaffCode <> '0' and StaffCode <> '')";
                am.Utility.LoadComboBox(cbxProcessPerson, sqlStr, "FullName", "StaffCode");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
        }
        private void LoadUnits()
        {
            try
            {
                string sqlStr = "SELECT Distinct UnitName from Unit";
                am.Utility.LoadComboBox(cbxUnit, sqlStr, "UnitName", "UnitName");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
        }
        private void LoadCategory()
        {
            try
            {
                string sqlStr = "SELECT ItemCategoryID, ItemCategoryName from ItemCategory";
                am.Utility.LoadComboBox(cbxCategoryType, sqlStr, "ItemCategoryName", "ItemCategoryID");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
            cbxVendor.Items.Clear();
            cbxSubCategories.Items.Clear();
        }
        private void CategoryChangeHandler(string categoryid)
        {
            //Load Vendor and Sub Category
            try
            {
                string sqlStr = "SELECT VendorID, VendorName from VendorInfo WHERE " +
                                "VendorID in (SELECT distinct VendorID FROM VendorCategory WHERE ItemCategoryID=@ItemCategoryID)";
                am.Utility.LoadComboBox(cbxVendor, sqlStr, "VendorName", "VendorID", new string[] { categoryid });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
            try
            {
                string sqlStr = "SELECT SubCategoryID, SubCategoryName from ItemSubCategory WHERE CategoryID=@CategoryID";
                am.Utility.LoadComboBox(cbxSubCategories, sqlStr, "SubCategoryName", "SubCategoryID", new string[] { categoryid });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
            cbxItems.Items.Clear();
        }
        private void SubCategoryChangeHandler(string subcategoryid)
        {
            //Load Item
            try
            {
                string sqlStr = "SELECT ItemID, ItemName from ItemInfo WHERE SubCategoryID=@SubCategoryID";
                am.Utility.LoadComboBox(cbxItems, sqlStr, "ItemName", "ItemID", new string[] { subcategoryid });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, ex.HResult.ToString(), ex.Message);
            }
        }

        protected void cbxCategoryType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cbxVendor.Items.Clear();
            cbxSubCategories.Items.Clear();
            CategoryChangeHandler(cbxCategoryType.SelectedValue);
        }

        protected void cbxSubCategories_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cbxItems.Items.Clear();
            SubCategoryChangeHandler(cbxSubCategories.SelectedValue);
        }

        #endregion

        protected void rgdFWItems_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":

                    string querySubCategoryByItem = "SELECT isc.SubCategoryID FROM ItemSubCategory isc";
                    querySubCategoryByItem += " LEFT JOIN ItemInfo ii ON isc.SubCategoryID=ii.SubCategoryID";
                    querySubCategoryByItem += " WHERE ii.ItemID=@ItemID";

                    gdItem = (GridDataItem)e.Item;
                    gdItem["FWItemEdit"].Text = "1";
                    string itemSelected = gdItem["ItemID"].Text;
                    DataTable dt1 = am.DataAccess.RecordSet(querySubCategoryByItem, new string[] { itemSelected });
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        cbxSubCategories.SelectedValue = dt1.Rows[0]["SubCategoryID"].ToString();
                    }
                    else
                    {
                        cbxSubCategories.SelectedValue = "0";
                    }
                    cbxSubCategories_SelectedIndexChanged(null, null);
                    cbxItems.SelectedValue = itemSelected;
                    tbxSpecification.Text = gdItem["Specification"].Text;
                    cbxUnit.SelectedValue = gdItem["UnitName"].Text;
                    tbxQuantity.Text = gdItem["Qty"].Text;
                    tbxPrice.Text = gdItem["Price"].Text;

                    e.Canceled = true;
                    break;
                case "Delete":
                    gdItem = (GridDataItem)e.Item;

                    int rowID = gdItem.DataSetIndex;

                    DataTable dt = null;
                    dt = am.Utility.GetDataTableFromItems(rgdFWItems);
                    if (dt == null)
                        dt = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows.RemoveAt(rowID);
                    }

                    rgdFWItems.DataSource = dt;
                    rgdFWItems.DataBind();

                    e.Canceled = true;
                    break;
            }
        }

        protected void btnItemSave_Click(object sender, EventArgs e)
        {
            if (!ItemIsValid())
            {
                return;
            }
            DataTable dt = null;
            dt = am.Utility.GetDataTableFromItems(rgdFWItems);
            if (dt == null)
                dt = new DataTable();

            //DataTable dtnew = new DataTable();
            ////set columns
            //dtnew.Columns.Add(new DataColumn("FWDID", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("FrameWorkID", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("ItemID", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("ItemName", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("Specification", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("UnitIn", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("UnitName", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("PackSize", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("Qty", typeof(string)));
            //dtnew.Columns.Add(new DataColumn("Price", typeof(string)));

            //set value 
            //DataRow dr = dtnew.NewRow();

            bool updated = false;
            DataRow dr = null;
            foreach (DataRow dri in dt.Rows)
            {
                //10th column to check if this was opened for edit
                if (dri[10].ToString() == "1")
                {
                    dr = dri;
                    updated = true;
                    break;
                }
            }
            if (dr == null)
            {
                dr = dt.NewRow();
            }
            dr[0] = "0";
            dr[1] = "0";
            dr[2] = cbxItems.SelectedValue;
            dr[3] = cbxItems.Text;
            dr[4] = tbxSpecification.Text;
            string unitName = "";
            if (!string.IsNullOrEmpty(cbxUnit.Text))
            {
                unitName = cbxUnit.Text;
            }
            dr[5] = unitName;
            dr[6] = unitName;
            dr[7] = "0";
            dr[8] = tbxQuantity.Text;
            dr[9] = tbxPrice.Text;
            //dtnew.Rows.Add(dr);
            if (!updated)
            {
                dr[10] = "0";
                dt.Rows.Add(dr);
            }

            //dt.Merge(dtnew);
            rgdFWItems.DataSource = dt;
            rgdFWItems.DataBind();

            btnItemReset_Click(null, null);
        }

        protected void btnItemReset_Click(object sender, EventArgs e)
        {
            cbxSubCategories.SelectedValue = "0";
            cbxSubCategories_SelectedIndexChanged(null, null);
            cbxItems.SelectedValue = "0";
            tbxSpecification.Text = "";
            cbxUnit.SelectedValue = "0";
            tbxQuantity.Text = "";
            tbxPrice.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valid())
            {
                return;
            }
            string strFields = "FrameWorkNo,FrameWorkType,VendorID,CategoryID,StartDate,EndDate,SignDate,PreparedPerson,Multilocation,Active";
            string[] strValues = new string[] { tbxFWNo.Text.Trim() ,"1", cbxVendor.SelectedValue , cbxCategoryType.SelectedValue ,
                dtpFWStartDate.SelectedDate.Value.ToString() , dtpFWSEndDate.SelectedDate.Value.ToString() , dtpFWSSignDate.SelectedDate.Value.ToString() ,
                cbxProcessPerson.SelectedValue , (chkMultipleLocation.Checked == true ? 1 : 0).ToString() , (chkActive.Checked == true ? 1 : 0).ToString() };

            if (hdfId.Value != "0")
            {
                //update
                am.DataAccess.BatchQuery.Update("FrameWork", strFields, strValues, "FrameWorkID=@FrameWorkID", new string[] { hdfId.Value });
            }
            else
            {
                //insert
                am.DataAccess.BatchQuery.Insert("FrameWork", strFields, strValues, "FrameWorkID");
            }
            bool saved = false;
            if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
            {
                string frameworkId = am.DataAccess.ActiveIdentity;
                saved = true;
                if (rgdFWItems.Items.Count > 0)
                {
                    SaveFrameworkItem(frameworkId);
                    SaveFrameworkAttachmentItem(frameworkId);
                    if (!am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                    {
                        saved = false;
                    }
                }
                if (saved)
                {
                    am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
                    btnItemReset_Click(null, null);
                    btnReset_Click(null, null);
                    LoadAllFrameworks();
                }
            }
        }

        private void SaveFrameworkItem(string frameworkId)
        {
            string strFields = "";
            string[] strValues = new string[7];

            strFields = "FrameWorkID,ItemID,Specification,UnitIn,PackSize,Qty,Price";

            //Delete all for this framework 
            am.DataAccess.BatchQuery.Delete("FrameWorkItem", "FrameWorkID=@FrameWorkID", new string[] { frameworkId });

            foreach (GridDataItem dataItem in rgdFWItems.MasterTableView.Items)
            {
                string unitName = "";
                if (!dataItem["UnitName"].Text.Contains("&nbsp"))
                {
                    unitName = dataItem["UnitName"].Text;
                }
                strValues = new string[] {  frameworkId , dataItem["ItemID"].Text , dataItem["Specification"].Text ,
                        unitName , dataItem["PackSize"].Text ,
                        dataItem["Qty"].Text , dataItem["Price"].Text };

                am.DataAccess.BatchQuery.Insert("FrameWorkItem", strFields, strValues);
            }
        }

        private void SaveFrameworkAttachmentItem(string frameworkId)
        {
            string strFields = "";
            string[] strValues = new string[3];

            strFields = "FrameWorkID,FrameWorkAttachLocation,Note";

            //Delete all for this framework 
            am.DataAccess.BatchQuery.Delete("FrameworkAttach", "FrameWorkID=@FrameWorkID", new string[] { frameworkId });

            foreach (GridDataItem dataItem in rgdAttachment.MasterTableView.Items)
            {
                string fileName = dataItem["FileName"].Text;
                string filePath = uploadFolder + "/" + fileName;
                string note = (dataItem.FindControl("txtAttachmentNote") as RadTextBox).Text;

                strValues = new string[] { frameworkId, filePath, note };
                am.DataAccess.BatchQuery.Insert("FrameworkAttach", strFields, strValues);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            hdfId.Value = "0";
            tbxFWNo.Text = GenerateRefNo();
            cbxCategoryType.SelectedValue = "0";
            cbxCategoryType_SelectedIndexChanged(null, null);
            cbxProcessPerson.SelectedValue = "0";
            cbxVendor.SelectedValue = "0";
            dtpFWStartDate.SelectedDate = null;
            dtpFWSEndDate.SelectedDate = null;
            dtpFWSSignDate.SelectedDate = null;
            chkActive.Checked = true;
            chkMultipleLocation.Checked = true;

            btnItemReset_Click(null, null);

            rgdFWItems.DataSource = null;
            rgdFWItems.Rebind();
            rgdAttachment.DataSource = null;
            rgdAttachment.Rebind();
        }
        private string GenerateRefNo()
        {
            string refNo = "";
            try
            {
                string year = "";
                if (dtpFWStartDate.DateInput.Text == "")
                {
                    year = DateTime.Now.Year.ToString().Substring(DateTime.Now.Year.ToString().Length - 2);
                }
                else
                {
                    year = dtpFWStartDate.SelectedDate.Value.Year.ToString().Substring(dtpFWStartDate.SelectedDate.Value.Year.ToString().Length - 2);
                }

                string sqlStr = "select ISNULL(Max(SUBSTRING([FrameWorkNo],LEN([FrameWorkNo])-CHARINDEX('/',reverse([FrameWorkNo]))+2,LEN([FrameWorkNo])))+1,1) as LastNo from [dbo].[FrameWork] ";
                                //+ "where SUBSTRING([FrameWorkNo],CHARINDEX('-',[FrameWorkNo])+1,2)=@Year";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { });
                if (dt != null && dt.Rows.Count > 0)
                {
                    string lastNo = dt.Rows[0]["LastNo"].ToString();
                    refNo = "FWA/SCI/BDCO/FY-" + year + "/" + lastNo.PadLeft(5, '0');
                    //FWA/SCI/BDCO/FY-15/00200
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
                refNo = "";
            }
            return refNo;
        }

        protected void btnAttachFile_Click(object sender, EventArgs e)
        {
            if (UploadAttachment.HasFiles)
            {
                DataTable dt = null;
                dt = am.Utility.GetDataTableFromItems(rgdAttachment);
                if (dt == null)
                    dt = new DataTable();

                //DataTable dtnew = new DataTable();
                //set columns
                //dtnew.Columns.Add(new DataColumn("FrameWorkID", typeof(string)));
                //dtnew.Columns.Add(new DataColumn("FrameWorkAttachLocation", typeof(string)));
                //dtnew.Columns.Add(new DataColumn("Note", typeof(string)));

                //Insert uploaded files in grid
                //HttpPostedFile
                foreach (HttpPostedFile file in UploadAttachment.PostedFiles)
                {
                    string fileNm = file.FileName;
                    if(file.FileName.Contains("\\"))
                    {
                        fileNm = file.FileName.Split('\\').Last();
                    }
                    string fileName = fileUploadPrefix + fileNm; //file.FileName;
                    string localFilePath = Path.Combine(fileUploadLocation, fileName);

                    //set value 
                    DataRow dr = dt.NewRow();
                    dr[0] = "0";
                    //The location is not saved in db, untill save button press.
                    dr[1] = uploadFolder + "/" + fileName;
                    dr[2] = fileName;
                    dr[3] = "";

                    dt.Rows.Add(dr);

                    file.SaveAs(localFilePath);
                }

                //                
                rgdAttachment.DataSource = dt;
                rgdAttachment.DataBind();
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Nothing to upload.");
            }
        }

        protected void rgdAttachment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Delete":
                    gdItem = (GridDataItem)e.Item;
                    int rowID = gdItem.DataSetIndex;

                    DataTable dt = null;
                    dt = am.Utility.GetDataTableFromItems(rgdAttachment);
                    if (dt == null)
                        dt = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows.RemoveAt(rowID);
                    }

                    rgdAttachment.DataSource = dt;
                    rgdAttachment.DataBind();
                    e.Canceled = true;
                    break;
                case "Download":
                    gdItem = (GridDataItem)e.Item;
                    try
                    {                        
                        GridColumn gc = rgdAttachment.Columns[1];
                        string savedFileName = gdItem[gc].Text;
                        if(savedFileName.Contains("/"))
                        {
                            savedFileName = savedFileName.Split('/').Last();
                        }
                        //string localFilePath = Path.Combine(fileUploadLocation, savedFileName);
                        //if (File.Exists(localFilePath))
                        //{
                            am.Utility.FileDownload(Page, FWADownloadURL, savedFileName);
                        //}
                        //else
                        //{
                        //    am.Utility.ShowHTMLMessage(Page, "000", "File does not exists.");
                        //}
                    }
                    catch(Exception ex)
                    {

                    }
                    e.Canceled = true;
                    break;
            }
        }

        private bool Valid()
        {
            if (dtpFWStartDate.DateInput.Text == "")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Start Date.");
                dtpFWStartDate.Focus();
                return false;
            }
            if (dtpFWSEndDate.DateInput.Text == "")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select End Date.");
                dtpFWSEndDate.Focus();
                return false;
            }
            if (dtpFWSSignDate.DateInput.Text == "")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Sign Date.");
                dtpFWSSignDate.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cbxProcessPerson.SelectedValue) || cbxProcessPerson.SelectedValue == "0")
            {
                am.Utility.ShowHTMLAlert(this.Page, "00", "Select Process Person.");
                cbxProcessPerson.Focus();
                return false;
            }

            return true;
        }

        private bool ItemIsValid()
        {
            if (string.IsNullOrEmpty(cbxItems.SelectedValue) || cbxItems.SelectedValue == "0")
            {
                am.Utility.ShowHTMLAlert(this.Page, "00", "Select Framework Item.");
                cbxItems.Focus();
                return false;
            }
            if (tbxSpecification.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Framework Item Specification.");
                tbxSpecification.Focus();
                return false;
            }
            if (tbxQuantity.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Framework Item Quantity.");
                tbxQuantity.Focus();
                return false;
            }
            if (tbxPrice.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Framework Item Price.");
                tbxPrice.Focus();
                return false;
            }

            return true;
        }


        private void DownloadAllFilesAsZip(GridDataItem gdItem)
        {            
            int ID;
            try
            {
                ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FrameWorkID"]);
                string idStr = ID.ToString();                

                string str = "SELECT * FROM FrameworkAttach WHERE FrameWorkID=@FrameWorkID";
                if (idStr.Trim().Length <= 0)
                {
                    idStr = "0";
                }
                DataTable dt = am.DataAccess.RecordSet(str, new string[] { idStr });
                if (dt != null && dt.Rows.Count > 0)
                {                    
                    foreach (DataRow dr in dt.Rows)
                    {
                        string savedFileName = dr["FrameWorkAttachLocation"].ToString();
                        if (savedFileName.Contains("/"))
                        {
                            savedFileName = savedFileName.Split('/').Last();
                            am.Utility.FileDownload(Page, FWADownloadURL, savedFileName);
                        }                        
                    }
                                      
                }
            }
            catch (Exception ex)
            {

            }
        }

        //private void DownloadAllFilesAsZip(GridDataItem gdItem)
        //{
        //    string downlaodfilename = "";
        //    int ID;
        //    try
        //    {
        //        ID = Convert.ToInt32(gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FrameWorkID"]);
        //        string idStr = ID.ToString();
        //        downlaodfilename = fileUploadPrefix + idStr + ".zip";
        //        string zipUploadLocation = Path.Combine(fileUploadLocation, downlaodfilename);

        //        string str = "SELECT * FROM FrameworkAttach WHERE FrameWorkID=@FrameWorkID";
        //        if (idStr.Trim().Length <= 0)
        //        {
        //            idStr = "0";
        //        }
        //        DataTable dt = am.DataAccess.RecordSet(str, new string[] { idStr });
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            string[] files = new string[dt.Rows.Count];
        //            int index = 0;
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string savedFileName = dr["FrameWorkAttachLocation"].ToString();
        //                files[index] = Server.MapPath(savedFileName);
        //                index++;
        //            }

        //            //Response.Clear();
        //            //Response.ContentType = "application/zip";
        //            //Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", downlaodfilename));

        //            //files[0] = @"F:\scsl\projects\PSMS\SourceCode\PSMS\PSMS\bin\FrameworkAttachment\FWA_Doc1.txt";
        //            //bool recurseDirectories = true;
        //            using (ZipFile zip = new ZipFile())
        //            {
        //                foreach (string file in files)
        //                {
        //                    //zip.AddFile(file, ""); 
        //                    zip.AddItem(file, "");
        //                }
        //                //zip.AddSelectedFiles("*", SourceFolderPath, string.Empty, recurseDirectories);

        //                if (File.Exists(zipUploadLocation))
        //                {
        //                    File.Delete(zipUploadLocation);
        //                }
        //                zip.Save(zipUploadLocation);
        //            }
        //            //string localFilePath = Path.Combine(fileUploadLocation, downlaodfilename);
        //            //if (File.Exists(localFilePath))
        //            //{
        //                am.Utility.FileDownload(Page, FWADownloadURL, downlaodfilename);
        //            //}
        //            //else
        //            //{
        //            //    am.Utility.ShowHTMLMessage(Page, "000", "Zip File does not exists.");
        //            //}                     
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }
}