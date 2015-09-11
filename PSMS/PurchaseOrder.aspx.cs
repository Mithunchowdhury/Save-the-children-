using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PSMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class PurchaseOrder : System.Web.UI.Page
    {
        string PODownloadURL = "http://softdev/scms/Attachment/POAttachment/";
        string POAttachmentfolderPath = "~/Attachment/POAttachment";

        HRISGateway hg = new HRISGateway();
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
                txtPONo.Text = GenerateRefNo();
                LoadRefNo();
                LoadPreparedByInfo();
                LoadCheckedBy();
                LoadCurrency();
                LoadTermsConditions();
                LoadApprovedBy();
                if (!string.IsNullOrEmpty(Request.QueryString["poId"]) && Request.QueryString["type"] != null)
                {
                    string queryStringVal = Request.QueryString["poId"].ToString();
                    string type = Request.QueryString["type"].ToString();
                    if (type == "po")
                    {
                        hdfId.Value = queryStringVal;
                        LoadPOInfo();
                        LoadPOItemInfo();
                        LoadPOTermInfo();
                        LoadPOAttachmentInfo();

                    }
                    else if (type == "sel")
                    {
                        //queryStringVal = selection ID
                        cbxRefNo.SelectedValue = queryStringVal;
                        cbxRefNo_SelectedIndexChanged(this, null);
                    }
                }
               
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void LoadPOInfo()
        {
            try
            {
                int poId = Convert.ToInt32(hdfId.Value);
                string sqlStr = "SELECT [POID],[PONO],[PODate],[SelectionID],[VendorID],[Address],[Email],[Attn],[CellNo],[Sub],[Body],[Currency],[Rate],[Note],[PreparedByName] "
                                 + ",[PreparedByDesignation],[PreparedDate],[CheckedByName],[CheckedByDesignation],convert(varchar(10),[CheckedDate],103) as [CheckedDate],[ApprovedByName] "
                                 + ",[ApprovedByDesignation],convert(varchar(10),[ApprovedDate],103) as [ApprovedDate],[CCList],[EmailBody],[GRNStatus],[CheckedStatus],[ApprovedStatus] "
                                 + ",[DeliveryAddress],convert(varchar(10),[POSendMailDate],103) as [POSendMailDate],[AmmendNo],[VATAmount],[TAXAmount],[DiscountAmount],[ServiceCharge] "
                                 +",(SELECT TOP(1) [FullName] FROM [dbo].[UserInfo] WHERE [StaffCode]=[PreparedByName]) AS PreparedBy FROM [dbo].[PO] WHERE POID=@POID";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId.ToString() });
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtPONo.Text = dt.Rows[0]["PONO"].ToString();
                    if (am.Utility.IsValidDate(dt.Rows[0]["PODate"])) dtpDate.SelectedDate = DateTime.Parse(dt.Rows[0]["PODate"].ToString());
                    if (dt.Rows[0]["SelectionID"] != DBNull.Value)
                    {
                        cbxRefNo.SelectedValue = dt.Rows[0]["SelectionID"].ToString();
                    }
                    hdfVendorId.Value = dt.Rows[0]["VendorID"].ToString();
                    LoadVendorName();
                    txtVendorAddress.Text = dt.Rows[0]["Address"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                    txtAttention.Text = dt.Rows[0]["Attn"].ToString();
                    txtCellNo.Text = dt.Rows[0]["CellNo"].ToString();
                    txtSubject.Text = dt.Rows[0]["Sub"].ToString();
                    txtPOBody.Text = dt.Rows[0]["Body"].ToString();
                    cbxCurrency.SelectedValue = dt.Rows[0]["Currency"].ToString();
                    txtCoversionRate.Text = dt.Rows[0]["Rate"].ToString();
                    txtVAT.Text = dt.Rows[0]["VATAmount"].ToString();
                    txtTAX.Text = dt.Rows[0]["TAXAmount"].ToString();
                    txtDiscount.Text = dt.Rows[0]["DiscountAmount"].ToString();
                    txtServiceCharge.Text = dt.Rows[0]["ServiceCharge"].ToString();
                    txtPONote.Text = dt.Rows[0]["Note"].ToString();
                    lblPreparedById.Text = dt.Rows[0]["PreparedByName"].ToString();
                    lblPreparedBy.Text = dt.Rows[0]["PreparedBy"].ToString();
                    lblPreparedByDesignation.Text = dt.Rows[0]["PreparedByDesignation"].ToString();
                    cbxCheckedBy.SelectedValue = dt.Rows[0]["CheckedByName"].ToString();
                    lblCheckedByDesignation.Text = dt.Rows[0]["CheckedByDesignation"].ToString();
                    if (dt.Rows[0]["ApprovedByName"].ToString() != "")
                    {
                        cbxApprovedBy.SelectedValue = dt.Rows[0]["ApprovedByName"].ToString();
                        lblApprovedByDesignation.Text = dt.Rows[0]["ApprovedByDesignation"].ToString();
                        if (am.Utility.IsValidDate(dt.Rows[0]["ApprovedDate"])) txtApproveDate.Text = dt.Rows[0]["ApprovedDate"].ToString();
                    }
                    txtCCEmail.Text = dt.Rows[0]["CCList"].ToString();
                    txtDeliveryAddress.Text = dt.Rows[0]["DeliveryAddress"].ToString();
                    hdfAmendNo.Value = dt.Rows[0]["AmmendNo"].ToString();
                    if (am.Utility.IsValidDate(dt.Rows[0]["POSendMailDate"])) txtMailSendDate.Text = dt.Rows[0]["POSendMailDate"].ToString();
                    if (Convert.ToInt32(dt.Rows[0]["CheckedByName"]) == Convert.ToInt32(Session["StaffCode"]) && Convert.ToInt32(dt.Rows[0]["CheckedStatus"]) == 0)
                    {
                        rdbListAR.Enabled = true;
                    }
                    else
                    {
                        rdbListAR.Enabled = false;
                    }
                    if (Convert.ToInt32(dt.Rows[0]["ApprovedStatus"]) >= 2)
                    {
                        btnMailSend.Enabled = true;
                        btnMailSend.BackColor = Color.FromName("#c7081b");
                    }
                    else
                    {
                        btnMailSend.Enabled = false;
                        btnMailSend.BackColor = Color.Gray;
                    }
                   
                    if (am.Utility.IsValidDate(dt.Rows[0]["CheckedDate"])) txtCheckedDate.Text = dt.Rows[0]["CheckedDate"].ToString();
                   
                    if (dt.Rows[0]["CheckedStatus"].ToString() == "2")
                    {
                        rdbListAR.SelectedValue = "1";
                    }
                    else if (dt.Rows[0]["CheckedStatus"].ToString() == "1")
                    {
                        rdbListAR.SelectedValue = "0";
                    }


                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadVendorName()
        {
            try
            {
                int vendorId = Convert.ToInt32(hdfVendorId.Value);
                string sqlStr = "select [VendorName],[Email] from [dbo].[VendorInfo] where VendorID=@VendorID";

                DataTable dtVendor = am.DataAccess.RecordSet(sqlStr, new string[] { vendorId.ToString()});
                if (dtVendor != null && dtVendor.Rows.Count > 0)
                {
                    txtVendor.Text = dtVendor.Rows[0]["VendorName"].ToString();
                    txtEmail.Text = dtVendor.Rows[0]["Email"].ToString();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void LoadPOItemInfo()
        {
            try
            {

                int poId = Convert.ToInt32(hdfId.Value);
                string sqlStr = "select [RefNo],[PRItemID],[ItemDesc],[Price],[Note],[Qty]*[Price] as TotalPrice from [dbo].[POItem] where POID=@POID";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId.ToString()});

                string refNo = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    refNo = dt.Rows[0]["RefNo"].ToString();
                }
                string qry = "select MAX([SelectionID]) AS [SelectionID] from [dbo].[VendorSelection] where RefNo=@RefNo";
                DataTable dtSId = am.DataAccess.RecordSet(qry, new string[] { refNo });
                string selectionId = "";
                if (dtSId != null && dtSId.Rows.Count > 0)
                {
                    selectionId = dtSId.Rows[0]["SelectionID"].ToString();
                    cbxRefNo.SelectedValue = selectionId;
                }
              
                LoadItemInfo();

                foreach (DataRow row in dt.Rows)
                {
                    foreach (GridDataItem dataItem in grdPurchaseItem.MasterTableView.Items)
                    {
                        if (row["PRItemID"].ToString() == dataItem["PRItemID"].Text)
                        {
                            (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                            dataItem.Selected = true;
                            (dataItem.FindControl("txtSpecification") as RadTextBox).Text = row["ItemDesc"].ToString();
                            (dataItem.FindControl("txtUnitPrice") as RadTextBox).Text = row["Price"].ToString();
                            dataItem["TotalPrice"].Text = row["TotalPrice"].ToString();
                            (dataItem.FindControl("txtNote") as RadTextBox).Text = row["Note"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadPOTermInfo()
        {
            try
            {
                int poId = Convert.ToInt32(hdfId.Value);
                string sqlStr = "select distinct [dbo].[POTerms].[Ordering], [dbo].[POTerms].[TCNote],isnull([TCTitle],[dbo].[POTerms].[TCNote]) as TCTitle "
                                 + "from [dbo].[POTerms] left join [dbo].[TermsCondition] on [dbo].[POTerms].TCNote=[dbo].[TermsCondition].TCNote "
                                 + "where POID=@POID order by [Ordering]";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId.ToString()});
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (RadListBoxItem i in lbxTC.Items)
                        {
                            if (row["TCNote"].ToString() == i.Value)
                            {
                                i.Checked = true;
                            }
                        }
                    }
                }

                lbxSTC.DataSource = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    lbxSTC.DataSource = dt;
                    lbxSTC.DataTextField = "TCTitle";
                    lbxSTC.DataValueField = "TCNote";
                    lbxSTC.DataBind();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadPOAttachmentInfo()
        {
            try
            {
                int poId = Convert.ToInt32(hdfId.Value);
                string sqlStr = "select [POID],SUBSTRING([FilePath],27,LEN([FilePath])) as [FilePath],[Note] from [dbo].[POAttach] where POID=@POID";

                am.Utility.LoadGrid(grdAttachment, sqlStr, new string[] { poId.ToString()});

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId.ToString() });
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtAttachmentNote.Text = dt.Rows[0]["Note"].ToString();

                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadCheckedBy()
        {
            try
            {
                am.Utility.LoadComboBox(cbxCheckedBy, "SELECT UserInfo.StaffCode, viewStaffInfo.StaffName FROM UserInfo" +
                    " INNER JOIN viewStaffInfo ON viewStaffInfo.StaffCode=UserInfo.StaffCode", "StaffName", "StaffCode");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadApprovedBy()
        {
            try
            {
                string sqlStr = "select distinct [StaffCode],[Name]+' ('+convert(varchar(80),[StaffCode])+')' as [StaffName] from [dbo].[viewSOD] order by [StaffCode]";
                am.Utility.LoadComboBox(cbxApprovedBy, sqlStr, "StaffName", "StaffCode");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadTermsConditions()
        {
            string sqlStr = "select [TCNote],[TCTitle] from [dbo].[TermsCondition] where [Type]='PO' order by [TCTitle]";
            try
            {
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { });
                lbxTC.DataSource = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    lbxTC.DataSource = dt;
                    lbxTC.DataTextField = "TCTitle";
                    lbxTC.DataValueField = "TCNote";
                    lbxTC.DataBind();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private string GenerateRefNo()
        {
            string refNo = "";
            try
            {
                string year = "";
                if (dtpDate.DateInput.Text == "")
                {
                    year = DateTime.Now.Year.ToString().Substring(DateTime.Now.Year.ToString().Length - 2);
                }
                else
                {
                    year = dtpDate.Calendar.SelectedDate.Year.ToString().Substring(dtpDate.Calendar.SelectedDate.Year.ToString().Length - 2);
                }

                string sqlStr = "select ISNULL(Max(SUBSTRING([PONO],LEN([PONO])-CHARINDEX('/',reverse([PONO]))+2,LEN([PONO])))+1,1) as LastNo from [dbo].[PO] "
                                + "where SUBSTRING([PONO],CHARINDEX('-',[PONO])+1,2)=@Year";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { year});
                lbxTC.DataSource = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string lastNo = dt.Rows[0]["LastNo"].ToString();
                    refNo = "PO/SCI/BDCO/FY-" + year + "/" + lastNo.PadLeft(5, '0');
                    //PO/SCI/BDCO/FY-15/00200
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
                refNo = "";
            }
            return refNo;

        }

        protected void dtpDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtpDate.Calendar.SelectedDate != null)
            {
                txtPONo.Text = GenerateRefNo();
            }
        }

        private void LoadRefNo()
        {
            try
            {
                string sqlStr = "";
                if (!string.IsNullOrEmpty(Request.QueryString["poId"]) && Request.QueryString["type"] != null)
                {
                    //sqlStr = "SELECT DISTINCT [SelectionID],[RefNo] FROM [dbo].[VendorSelection] WHERE [TO_PO]=1 "
                    //                   + " AND [SelectionID] IN (select MAX([SelectionID]) FROM [dbo].[VendorSelection] GROUP BY [RefNo])";
                    sqlStr = "SELECT DISTINCT [SelectionID],[RefNo]+' ('+CAST([SelectionID] AS VARCHAR)+')' AS [RefNo] FROM [dbo].[VendorSelection] WHERE [TO_PO]=1";
                                      
                }
                else
                {
                    //sqlStr = "SELECT DISTINCT [SelectionID],[RefNo] FROM [dbo].[VendorSelection] WHERE [TO_PO]=1 "
                    //                   + " AND [SelectionID] IN (select MAX([SelectionID]) FROM [dbo].[VendorSelection] GROUP BY [RefNo]) "
                    //                   + " AND [SelectionID] NOT IN (SELECT ISNULL(SelectionID,0) FROM PO) ORDER BY [RefNo]";
                    sqlStr = "SELECT DISTINCT [SelectionID],[RefNo]+' ('+CAST([SelectionID] AS VARCHAR)+')' AS [RefNo] FROM [dbo].[VendorSelection] WHERE [TO_PO]=1 "                                     
                                      + " AND [SelectionID] NOT IN (SELECT ISNULL(SelectionID,0) FROM PO) ORDER BY [RefNo]";
                }
                am.Utility.LoadComboBox(cbxRefNo, sqlStr, "RefNo", "SelectionID");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void cbxRefNo_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadVendorInfo();
            LoadItemInfo();
        }
        private void LoadVendorInfo()
        {
            try
            {
                int selectionId = Convert.ToInt32(cbxRefNo.SelectedValue);
                string sqlStr = "SELECT TOP(1) [dbo].[VendorSelection].VendorID,[VendorName],[Address] "
                                +",[dbo].[VendorCategory].[ContactPerson],[dbo].[VendorCategory].Phone,[dbo].[VendorCategory].Email "
                                +"FROM [dbo].[VendorSelection],[dbo].[VendorInfo],[dbo].[VendorCategory] "
                                +"WHERE [dbo].[VendorInfo].[VendorID]=[dbo].[VendorSelection].VendorID "
                                + "AND [dbo].[VendorInfo].[VendorID]=[dbo].[VendorCategory].VendorID AND SelectionID=@SelectionID";               
              

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { selectionId.ToString() });
                if (dt != null && dt.Rows.Count > 0)
                {
                    hdfVendorId.Value = dt.Rows[0]["VendorID"].ToString();
                    txtVendor.Text = dt.Rows[0]["VendorName"].ToString();
                    txtVendorAddress.Text = dt.Rows[0]["Address"].ToString();
                    txtAttention.Text = dt.Rows[0]["ContactPerson"].ToString();
                    txtCellNo.Text = dt.Rows[0]["Phone"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void LoadItemInfo()
        {
            try
            {
                int selectionId = Convert.ToInt32(cbxRefNo.SelectedValue);
                string sqlStr = "SELECT ROW_NUMBER() over (ORDER BY [PRDetailID]) AS SlNo,[PRDetailID] as [PRItemID],[RefNo],[dbo].[SelectionItem].[ItemID],[ItemName],[ItemDesc] as [Specification],[UnitName] "
                                 + ",[dbo].[SelectionItem].[UnitID],[Qty],[UnitPrice],[Qty]*[UnitPrice] as TotalPrice,'' as Note "
                                 + "FROM [dbo].[VendorSelection],[dbo].[SelectionItem],[dbo].[ItemInfo],[dbo].[Unit] WHERE [dbo].[ItemInfo].ItemID=[dbo].[SelectionItem].ItemID "
                                 + "AND  [dbo].[Unit].UnitID=[dbo].[SelectionItem].UnitID AND [dbo].[VendorSelection].SelectionID=[dbo].[SelectionItem].SelectionID "
                                 + "AND [dbo].[VendorSelection].SelectionID=@SelectionID";
                am.Utility.LoadGrid(grdPurchaseItem, sqlStr, new string[] { selectionId.ToString() });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadCurrency()
        {
            try
            {
                string sqlStr = "select [CurrencyID],[CurrencyName] from [dbo].[Currency]";
                am.Utility.LoadComboBox(cbxCurrency, sqlStr, "CurrencyName", "CurrencyID");
                //By default select BDT
                cbxCurrency.SelectedValue = "1";
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdPurchaseItem.MasterTableView.Items)
            {
                (dataItem.FindControl("chkSelect") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdPurchaseItem.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdPurchaseItem.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked = checkHeader;
            }
        }

        protected void lbxTC_CheckAllCheck(object sender, RadListBoxCheckAllCheckEventArgs e)
        {
            bool check = false;
            foreach (RadListBoxItem item in lbxTC.Items)
            {
                if (item.Checked)
                {
                    check = true;
                    break;
                }
            }
            btnTransferForm.Enabled = check;
            foreach (RadListBoxItem item in lbxTC.Items)
            {
                if (item.Checked)
                {
                    item.BackColor = Color.LightGray;
                }
                else
                {
                    item.BackColor = Color.White;
                }
            }
        }

        protected void lbxTC_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            bool check = false;
            foreach (RadListBoxItem item in lbxTC.Items)
            {
                if (item.Checked)
                {
                    check = true;
                    break;
                }
            }
            btnTransferForm.Enabled = check;
            foreach (RadListBoxItem item in lbxTC.Items)
            {
                if (item.Checked)
                {
                    item.BackColor = Color.LightGray;
                }
                else
                {
                    item.BackColor = Color.White;
                }
            }
        }

        protected void btnTransferForm_Click(object sender, EventArgs e)
        {
            bool dup = false;
            foreach (RadListBoxItem item in lbxTC.CheckedItems)
            {
                foreach (RadListBoxItem i in lbxSTC.Items)
                {
                    if (i.Text == item.Text)
                    {
                        dup = true;
                        break;
                    }
                    else
                    {
                        dup = false;
                    }
                }
                if (!dup)
                {
                    RadListBoxItem item2 = new RadListBoxItem(item.Text, item.Value);
                    lbxSTC.Items.Add(item2);
                }
            }
        }

        protected void lbxSTC_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            //reset earlier checked item
            foreach (RadListBoxItem item in lbxSTC.CheckedItems)
            {
                if (item != e.Item)
                {
                    item.Checked = false;
                }
            }
            if (((RadListBoxItem)e.Item).Checked)
            {
                txtUpdateTC.Text = ((RadListBoxItem)e.Item).Value;
            }
            else
            {
                txtUpdateTC.Text = "";
            }
        }

        protected void btnUpdateTC_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxSTC.CheckedItems.Count > 0)
                {
                    lbxSTC.CheckedItems[0].Value = txtUpdateTC.Text;

                    lbxSTC.CheckedItems[0].Checked = false;
                    txtUpdateTC.Text = "";
                }
                else
                {
                    //new item
                    bool dup = false;
                    foreach (RadListBoxItem i in lbxSTC.Items)
                    {
                        if (i.Text == txtUpdateTC.Text.Trim())
                        {
                            dup = true;
                            break;
                        }
                        else
                        {
                            dup = false;
                        }
                    }
                    if (!dup)
                    {
                        RadListBoxItem item = new RadListBoxItem(txtUpdateTC.Text.Trim(), txtUpdateTC.Text.Trim());
                        lbxSTC.Items.Add(item);
                        txtUpdateTC.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

       
        private void SendMail()
        {
            try
            {
                int poId = Convert.ToInt32(hdfId.Value);
                if (poId > 0)
                {

                    GeneratePDF(poId.ToString());
                  
                    string strFields = "";
                    string[] strValues = new string[2];

                    strFields = "[POSendMailDate],[ApprovedStatus]";
                    strValues = new string[] { DateTime.Now.ToString(), "3" };
                    am.DataAccess.BatchQuery.Update("[dbo].[PO]", strFields, strValues, "[POID]=@POID", new string[] { poId.ToString() });
                    am.DataAccess.BatchQuery.Execute();                  


                    string[] files = new string[20];
                    DataTable dt = am.DataAccess.RecordSet("select [FilePath] from [dbo].[POAttach] where [POID]=@POID", new string[] { poId.ToString() });
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        files[i] = Server.MapPath(dt.Rows[i]["FilePath"].ToString());
                    }                   

                    string[] toEmails = txtEmail.Text.Split(';');
                    string[] ccEmails = txtCCEmail.Text.Split(';');
                    am.SendMail.SendOutlookMail(Page, toEmails, ccEmails, txtSubject.Text.Trim(), txtEmailBody.Text.Trim(), files);
                    am.Utility.ShowHTMLAlert(Page, "000", "Mail sent successfully");
                }

            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }
        private bool MailValid()
        {
            bool ret = false;

            if (txtEmailBody.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Email Body is required.");
                txtEmailBody.Focus();
                return false;
            }

            return ret;
        }
        public void GeneratePDF(string poId)
        {
            ReportDocument rptDoc = new ReportDocument();
            dsPSMS ds = new dsPSMS();
            DataTable dt = new DataTable();

            dt.TableName = "PO";
            dt = getAllPOs();

            ds.Tables["dtPO"].Merge(dt);

            //subreport data table 
            dt = new DataTable();
            dt.TableName = "Terms";
            dt = getAllTerms();
            ds.Tables["dtTerms"].Merge(dt);

            rptDoc.Load(Server.MapPath("~/Reports/crPO.rpt"));
            rptDoc.SetDataSource(ds);

            string fileName = "PO" + poId + "_.pdf";

            String targetFolder = Server.MapPath(POAttachmentfolderPath);
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = targetFolder + "\\" + fileName;
            CrExportOptions = rptDoc.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            rptDoc.Export();

            string strFields = "";
            string[] strValues = new string[3];
            strFields = "[POID],[FilePath],[Note]";
            
            string filePath = POAttachmentfolderPath + "/" + fileName;
            string sqlStr = "select [FilePath] from [dbo].[POAttach] where [FilePath]=@FilePath";
            DataTable hasData = am.DataAccess.RecordSet(sqlStr, new string[] { filePath });
            if (hasData.Rows.Count == 0)
            {
                strValues = new string[] { poId.ToString() , filePath , txtAttachmentNote.Text.Trim() };
                am.DataAccess.BatchQuery.Insert("[dbo].[POAttach]", strFields, strValues);
                am.DataAccess.BatchQuery.Execute();
            }

        }
        public DataTable getAllPOs()
        {
            string poId = hdfId.Value;
            string query = "EXEC rptPO '" + poId + "'";

            DataTable dt = am.DataAccess.RecordSet(query, new string[] { });
            return dt;
        }

        public DataTable getAllTerms()
        {
            string poId = hdfId.Value;
            string query = "select [dbo].[PO].PONO as TermParentNo,[Ordering], [dbo].[POTerms].TCNote as [Note] " +
                "from [dbo].[PO],[dbo].[POTerms] where [dbo].[PO].[POID]=[dbo].[POTerms].POID " +
                "and [dbo].[PO].[POID]=@POID order by [Ordering]";

            DataTable dt = am.DataAccess.RecordSet(query, new string[] { poId });
            return dt;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    if (SavePO())
                        am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
                    else
                        am.Utility.ShowHTMLMessage(Page, "000", "Could not Save.");
                }                
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private bool Valid()
        {
            bool ret = false;

            if (txtPONo.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "PO No is required.");
                txtPONo.Focus();
                return false;
            }
            if (dtpDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Date is required.");
                dtpDate.Focus();
                return false;
            }
            if (cbxRefNo.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Ref No is required.");
                cbxRefNo.Focus();
                return false;
            }

            if (hdfVendorId.Value != "")
            {
                ret = true;
            }
            else
            {
                ret = false;
            }
            if (txtVendor.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Vendor is required.");
                txtVendor.Focus();
                return false;
            }
            if (txtVendorAddress.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Vendor Address is required.");
                txtVendorAddress.Focus();
                return false;
            }
            if (txtDeliveryAddress.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Delivery Address is required.");
                txtDeliveryAddress.Focus();
                return false;
            }
            if (txtAttention.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Attention is required.");
                txtAttention.Focus();
                return false;
            }
            if (txtCellNo.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Cell No is required.");
                txtCellNo.Focus();
                return false;
            }
            if (cbxCurrency.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Currency is required.");
                cbxCurrency.Focus();
                return false;
            }
            if (txtSubject.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Subject is required.");
                txtSubject.Focus();
                return false;
            }
            if (txtPOBody.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "PO Body is required.");
                txtPOBody.Focus();
                return false;
            }
           
            foreach (GridDataItem dataItem in grdPurchaseItem.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    ret = true;
                    break;
                }
                else
                {
                    ret = false;
                }

            }
            if (!ret)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Item Selection is required.");
                grdPurchaseItem.Focus();
                return false;
            }
            if (lbxSTC.Items.Count > 0)
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Terms and Condition is required.");
                lbxTC.Focus();
                return false;
            }

            if (cbxCheckedBy.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Checked By is required.");
                cbxCheckedBy.Focus();
                return false;
            }
            if (cbxApprovedBy.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Approved By is required.");
                cbxApprovedBy.Focus();
                return false;
            }


            return ret;
        }
        private bool SavePO()
        {
            bool result = false;
            try
            {
                string rateVal = "0";
                if (txtCoversionRate.Text != "")
                {
                    rateVal = txtCoversionRate.Text;
                }

                if (hdfId.Value != "")
                {
                    if (chkAmend.Checked)
                    {
                        am.DataAccess.BatchQuery.Insert("PO",
                       "[PONO],[PODate],[SelectionID],[VendorID],[Address],[Email],[Attn],[CellNo],[Sub],[Body],[Currency],[Rate],[Note],[PreparedByName]," +
                       "[PreparedByDesignation],[PreparedDate],[CheckedByName],[CheckedByDesignation],[CCList],[EmailBody],[DeliveryAddress],[AmmendNo],[ApprovedByName],[ApprovedByDesignation],"+
                       "[VATAmount],[TAXAmount],[DiscountAmount],[ServiceCharge]",
                        new string[]{
                            txtPONo.Text.Trim() , dtpDate.SelectedDate.Value.ToString(),cbxRefNo.SelectedValue, hdfVendorId.Value , txtVendorAddress.Text.Trim() , txtEmail.Text.Trim() ,
                       txtAttention.Text.Trim() , txtCellNo.Text.Trim() , txtSubject.Text.Trim() , txtPOBody.Text.Trim() , cbxCurrency.SelectedValue ,
                       rateVal , txtPONote.Text.Trim() , lblPreparedById.Text.Trim() , lblPreparedByDesignation.Text.Trim() , DateTime.Now.ToString() ,
                       cbxCheckedBy.SelectedValue , lblCheckedByDesignation.Text.Trim() , txtCCEmail.Text.Trim() , txtEmailBody.Text.Trim() ,
                       txtDeliveryAddress.Text.Trim() , (Convert.ToInt32(hdfAmendNo.Value) + 1).ToString() , cbxApprovedBy.SelectedValue , lblApprovedByDesignation.Text.Trim(),
                       txtVAT.Text==""?"0":txtVAT.Text, txtTAX.Text==""?"0":txtTAX.Text, txtDiscount.Text==""?"0":txtDiscount.Text, txtServiceCharge.Text==""?"0":txtServiceCharge.Text
                        }, "POID");

                        if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                        {
                            string retId = am.DataAccess.ActiveIdentity;
                            hdfId.Value = retId;
                            SavePOItem(retId);
                            SavePOTerm(retId);
                            SavePOAttachment(retId);
                            if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                            {
                                GeneratePDF(retId);
                                GetAmendNo();
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        am.DataAccess.BatchQuery.Update("PO",
                         "[PONO],[PODate],[SelectionID],[VendorID],[Address],[Email],[Attn],[CellNo],[Sub],[Body],[Currency],[Rate],[Note],[PreparedByName]," +
                         "[PreparedByDesignation],[PreparedDate],[CheckedByName],[CheckedByDesignation],[CCList],[EmailBody],[DeliveryAddress],[ApprovedByName],[ApprovedByDesignation]," +
                         "[VATAmount],[TAXAmount],[DiscountAmount],[ServiceCharge]",
                         new string[]{
                         txtPONo.Text.Trim() , dtpDate.SelectedDate.Value.ToString() , cbxRefNo.SelectedValue , hdfVendorId.Value , txtVendorAddress.Text.Trim() , txtEmail.Text.Trim() ,
                         txtAttention.Text.Trim() , txtCellNo.Text.Trim() , txtSubject.Text.Trim() , txtPOBody.Text.Trim() , cbxCurrency.SelectedValue ,
                         rateVal , txtPONote.Text.Trim() , lblPreparedById.Text.Trim() , lblPreparedByDesignation.Text.Trim() , DateTime.Now.ToString() ,
                         cbxCheckedBy.SelectedValue , lblCheckedByDesignation.Text.Trim() , txtCCEmail.Text.Trim() , txtEmailBody.Text.Trim() ,
                         txtDeliveryAddress.Text.Trim() , cbxApprovedBy.SelectedValue , lblApprovedByDesignation.Text.Trim(),
                         txtVAT.Text==""?"0":txtVAT.Text, txtTAX.Text==""?"0":txtTAX.Text, txtDiscount.Text==""?"0":txtDiscount.Text, txtServiceCharge.Text==""?"0":txtServiceCharge.Text
                         }, "POID=@POID", new string[]{hdfId.Value});


                        DeletePOItem(hdfId.Value);
                        SavePOItem(hdfId.Value);

                        DeletePOTerm(hdfId.Value);
                        SavePOTerm(hdfId.Value);

                        SavePOAttachment(hdfId.Value);

                        if (am.DataAccess.BatchQuery.Execute())
                        {
                            GeneratePDF(hdfId.Value);
                            GetAmendNo();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    am.DataAccess.BatchQuery.Insert("PO",
                         "[PONO],[PODate],[SelectionID],[VendorID],[Address],[Email],[Attn],[CellNo],[Sub],[Body],[Currency],[Rate],[Note],[PreparedByName]," +
                         "[PreparedByDesignation],[PreparedDate],[CheckedByName],[CheckedByDesignation],[CCList],[EmailBody],[DeliveryAddress],[ApprovedByName],[ApprovedByDesignation]," +
                         "[VATAmount],[TAXAmount],[DiscountAmount],[ServiceCharge]",
                         new string[]{
                         GenerateRefNo() , dtpDate.SelectedDate.Value.ToString() , cbxRefNo.SelectedValue , hdfVendorId.Value , txtVendorAddress.Text.Trim() , txtEmail.Text.Trim() ,
                         txtAttention.Text.Trim() , txtCellNo.Text.Trim() , txtSubject.Text.Trim() , txtPOBody.Text.Trim() , cbxCurrency.SelectedValue ,
                         rateVal , txtPONote.Text.Trim() , lblPreparedById.Text.Trim() , lblPreparedByDesignation.Text.Trim() , DateTime.Now.ToString() ,
                         cbxCheckedBy.SelectedValue , lblCheckedByDesignation.Text.Trim() , txtCCEmail.Text.Trim() , txtEmailBody.Text.Trim() ,
                         txtDeliveryAddress.Text.Trim() , cbxApprovedBy.SelectedValue , lblApprovedByDesignation.Text.Trim(),
                         txtVAT.Text==""?"0":txtVAT.Text, txtTAX.Text==""?"0":txtTAX.Text, txtDiscount.Text==""?"0":txtDiscount.Text, txtServiceCharge.Text==""?"0":txtServiceCharge.Text
                         }, "POID");

                    if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                    {
                        string retId = am.DataAccess.ActiveIdentity;
                        hdfId.Value = retId;
                        SavePOItem(retId);
                        SavePOTerm(retId);
                        SavePOAttachment(retId);
                        if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                        {
                            GeneratePDF(retId);
                            GetAmendNo();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
                result = false;
            }
            return result;
        }
        private void DeletePOItem(string poId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[POItem]", "[POID]=@POID", new string[] { poId });
        }
        private void DeletePOTerm(string poId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[POTerms]", "[POID]=@POID", new string[] { poId });
        }
        private void DeletePOAttachment(string poId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[POAttach]", "[POID]=@POID", new string[] { poId });

        }
        private void GetAmendNo()
        {
            string poId = hdfId.Value;
            string sqlStr = "SELECT [POID],[AmmendNo] FROM [dbo].[PO] WHERE POID=@POID";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId });
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfAmendNo.Value = dt.Rows[0]["AmmendNo"].ToString();
            }

        }
        private void SavePOItem(string poId)
        {
            foreach (GridDataItem dataItem in grdPurchaseItem.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    am.DataAccess.BatchQuery.Insert("POItem", "[POID],[RefNo],[PRItemID],[ItemID],[ItemDesc],[PackSize],[UnitID],[Qty],[Price],[Status],[Note]",
                        new string[] {
                            poId , dataItem["RefNo"].Text , dataItem["PRItemID"].Text , dataItem["ItemID"].Text ,
                        (dataItem.FindControl("txtSpecification") as RadTextBox).Text , "0" , dataItem["UnitID"].Text ,
                        dataItem["Qty"].Text, (dataItem.FindControl("txtUnitPrice") as RadTextBox).Text , "3" , (dataItem.FindControl("txtNote") as RadTextBox).Text
                        });

                }
            }

        }

        private void SavePOTerm(string poId)
        {
            int count = 1;
            foreach (RadListBoxItem i in lbxSTC.Items)
            {
                am.DataAccess.BatchQuery.Insert("POTerms", "[POID],[TCNote],[Ordering]", 
                    new string[] {
                        poId , i.Value, count.ToString()
                    });
                count++;
            }
        }
        private void SavePOAttachment(string poId)
        {

            if (asyncUploadInvitationFile.UploadedFiles.Count > 0)
            {
                if (txtPONo.Text != "")
                {
                    string fileName = txtPONo.Text.Split('/')[0] + poId + "_";
                    string folderPath = "~/Attachment/POAttachment";
                    String targetFolder = Server.MapPath(folderPath);

                    foreach (UploadedFile file in asyncUploadInvitationFile.UploadedFiles)
                    {
                        string filePath = folderPath + "/" + fileName + file.FileName;

                        am.DataAccess.BatchQuery.Insert("POAttach", "[POID],[FilePath],[Note]", 
                            new string[] {
                            poId , filePath, txtAttachmentNote.Text.Trim()
                            });

                        //Upload file in server location
                        file.SaveAs(Path.Combine(targetFolder, fileName + file.FileName));
                    }
                }
            }

        }
        private void LoadPreparedByInfo()
        {
            try
            {
                if (Session["StaffCode"] != null)
                {
                    int preparedbyId = Convert.ToInt32(Session["StaffCode"].ToString());
                    string sqlStr = "select [StaffCode],[StaffName],[Designation],[Dept] from [dbo].[viewStaffInfo] where StaffCode=@StaffCode";

                    DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { preparedbyId.ToString() });
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lblPreparedById.Text = dt.Rows[0]["StaffCode"].ToString();
                        lblPreparedBy.Text = dt.Rows[0]["StaffName"].ToString();
                        lblPreparedByDesignation.Text = dt.Rows[0]["Designation"].ToString() + "-" + dt.Rows[0]["Dept"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            int poId = Convert.ToInt32(hdfId.Value);
            if (poId > 0)
            {
                Response.Redirect("Reports/PO.aspx?poId=" + poId);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            hdfId.Value = "";
            hdfVendorId.Value = "";
            hdfAmendNo.Value = "";
            dtpDate.SelectedDate = null;
            txtPONo.Text = GenerateRefNo();
            cbxRefNo.SelectedValue = "0";
            txtVendor.Text = "";
            txtVendorAddress.Text = "";
            txtDeliveryAddress.Text = "";
            txtAttention.Text = "";
            txtCellNo.Text = "";
            txtEmail.Text = "";
            //By default select BDT
            cbxCurrency.SelectedValue = "1";
            txtCoversionRate.Text = "";
            txtSubject.Text = "";
            txtPOBody.Text = "";
            txtEmailBody.Text = "";
            txtCCEmail.Text = "";
            grdAttachment.DataSource = null;
            grdAttachment.Rebind();
            txtAttachmentNote.Text = "";
            grdPurchaseItem.DataSource = null;
            grdPurchaseItem.Rebind();
            foreach (RadListBoxItem i in lbxTC.Items)
            {
                i.Checked = false;
            }
            lbxSTC.Items.Clear();
            //txtAddNew.Text = "";
            txtUpdateTC.Text = "";
            LoadPreparedByInfo();
            cbxCheckedBy.SelectedValue = "0";
            cbxApprovedBy.SelectedValue = "0";
            txtMailSendDate.Text = "";
            txtPONote.Text = "";

        }

        protected void cbxCheckedBy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadCheckedByInfo();
        }
        private void LoadCheckedByInfo()
        {
            try
            {
                int checkedbyId = Convert.ToInt32(cbxCheckedBy.SelectedValue);
                string sqlStr = "select [StaffCode],[StaffName],[Designation],[Dept] from [dbo].[viewStaffInfo] where StaffCode=@StaffCode";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { checkedbyId.ToString() });
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblCheckedByDesignation.Text = dt.Rows[0]["Designation"].ToString() + "-" + dt.Rows[0]["Dept"].ToString();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        protected void cbxApprovedBy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadApprovedByInfo();
        }

        private void LoadApprovedByInfo()
        {
            try
            {
                int approvedbyId = Convert.ToInt32(cbxApprovedBy.SelectedValue);
                string sqlStr = "select [StaffCode],[Name],[Designation] from [dbo].[viewSOD] where StaffCode=@StaffCode";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { approvedbyId.ToString() });
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblApprovedByDesignation.Text = dt.Rows[0]["Designation"].ToString();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void grdAttachment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Open":

                    gdItem = (GridDataItem)e.Item;
                    OpenAttachment(gdItem);
                    e.Canceled = true;
                    break;
                case "Delete":

                    gdItem = (GridDataItem)e.Item;
                    DeleteAttachment(gdItem);
                    e.Canceled = true;
                    break;

            }
        }
        private void OpenAttachment(GridDataItem gdItem)
        {

            try
            {
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();
                am.Utility.FileDownload(Page, PODownloadURL, filepath);
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void DeleteAttachment(GridDataItem gdItem)
        {
            try
            {                
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();

                am.DataAccess.BatchQuery.Delete("POAttach", "FilePath=@FilePath", new string[] { POAttachmentfolderPath + "/" + filepath });
                if (am.DataAccess.BatchQuery.Execute())
                {
                    String targetFolder = Server.MapPath(POAttachmentfolderPath);
                    string path = Path.Combine(targetFolder, filepath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    LoadPOAttachmentInfo();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void rdbListAR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbListAR.SelectedValue == "1")
            {
                Approve();
            }
            if (rdbListAR.SelectedValue == "0")
            {
                Reject();
            }
        }

        private void Approve()
        {
            string poId = hdfId.Value;
            string strFields = "";
            string[] strValues = new string[2];

            strFields = "[CheckedDate],[CheckedStatus]";
            strValues = new string[] { DateTime.Now.ToString() , "2" };
            am.DataAccess.BatchQuery.Update("[dbo].[PO]", strFields, strValues, "[POID]=@POID", new string[] { poId });
            if (am.DataAccess.BatchQuery.Execute())
            {
                string mBody = "Dear Mr./Ms. " + hg.getName(cbxApprovedBy.SelectedValue) + ",<br /> You have a Purchase Order ('" + txtPONo.Text + "') Verification Request.";
                am.SendMail.SendOutlookMail(Page, hg.getOfficeEmail(cbxApprovedBy.SelectedValue), "Purchase Order Verification Request", mBody, "");
                am.Utility.ShowHTMLAlert(Page, "000", "Verified Successfully.");
            }
        }
        private void Reject()
        {
            string poId = hdfId.Value;
            string strFields = "";
            string[] strValues = new string[2];

            strFields = "[CheckedDate],[CheckedStatus]";
            strValues = new string[] { DateTime.Now.ToString() , "1" };
            am.DataAccess.BatchQuery.Update("[dbo].[PO]", strFields, strValues, "[POID]=@POID", new string[] { poId });
            if (am.DataAccess.BatchQuery.Execute())
            {
                am.Utility.ShowHTMLAlert(Page, "000", "Rejected Successfully.");
            }
        }

        protected void btnMailSend_Click(object sender, EventArgs e)
        {
            if (MailValid())
            {
                SendMail();
            }   
        }
    }
}