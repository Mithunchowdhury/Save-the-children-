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
    public partial class Invitation : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        string InvitationDownloadURL = "http://softdev/scms/Attachment/InvitationAttachment/";
        string InvitationAttachmentfolderPath = "~/Attachment/InvitationAttachment";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            rdbListAR.Enabled = false;

            if (!IsPostBack)
            {
                btnInvSend.Enabled = false;
                btnInvSend.BackColor = Color.Gray;
                divIFT.Visible = false;
                LoadCheckedBy();
                LoadCategory();
                LoadTermsConditions();
                if (Request.QueryString["invId"] != null && Request.QueryString["type"] != null)
                {
                    string queryStringVal = Request.QueryString["invId"].ToString();
                    string type = Request.QueryString["type"].ToString();
                    if (type == "rfq")
                    {
                        hdfInvId.Value = Request.QueryString["invId"].ToString();
                        LoadInvitationInfo();
                        LoadCategoryId();
                        LoadPRInfo(Convert.ToInt32(hdfInvId.Value));
                        LoadVendorInfo();
                        LoadInvitationItem();
                        LoadInvitationVendor();
                        LoadInvitationTerms();
                        LoadInvitationAttachment();
                    }
                    else if (type == "pr")
                    {
                        SelectCategory(queryStringVal);
                    }
                }
                else
                {
                    dtpDate.SelectedDate = DateTime.Now;
                }
            }
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void LoadCategory()
        {

            string sqlStr = "SELECT DISTINCT ItemCategory.ItemCategoryID, ItemCategory.ItemCategoryName FROM   PRItem INNER JOIN "
                            + "ItemInfo ON PRItem.ItemID = ItemInfo.ItemID INNER JOIN ItemSubCategory ON ItemInfo.SubCategoryID = "
                            + "ItemSubCategory.SubCategoryID INNER JOIN ItemCategory ON ItemSubCategory.CategoryID = ItemCategory.ItemCategoryID "
                            + "WHERE (PRItem.PRItemID NOT IN (SELECT PRItemID FROM InvitationItem)) AND (PRItem.PRItemID NOT IN "
                            + "(SELECT PRDetailID FROM SelectionItem))";
            am.Utility.LoadComboBox(cbxCategory, sqlStr, "ItemCategoryName", "ItemCategoryID");

        }
        private void SelectCategory(string prID)
        {
            string sqlStr = "SELECT DISTINCT TOP 1 ItemSubCategory.CategoryID FROM PRItem " +
             "INNER JOIN ItemSubCategory ON ItemSubCategory.SubCategoryID=PRItem.SubCategoryID " +
             "WHERE PRID=@PRID";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { prID });
            if (dt != null && dt.Rows.Count > 0)
            {
                string categoryID = dt.Rows[0]["CategoryID"].ToString();
                cbxCategory.SelectedValue = categoryID;
                cbxCategory_SelectedIndexChanged(null, null);
            }
        }

        private void LoadVendorInfo()
        {
            string sqlStr = "select ROW_NUMBER() over (ORDER BY [dbo].[VendorInfo].[VendorID]) AS SlNo,[dbo].[VendorInfo].[VendorID],[dbo].[VendorInfo].[VendorCode],[dbo].[VendorInfo].VendorName "
                            + ",[dbo].[VendorInfo].VendorType,case [dbo].[VendorInfo].Address when '' then [dbo].[VendorInfo].Address else [dbo].[VendorInfo].Address end as Address "
                            + ",case [dbo].[VendorCategory].[ContactPerson] when '' then [dbo].[VendorInfo].ContactPerson else [dbo].[VendorCategory].[ContactPerson] end as ContactPerson "
                            + ",case [dbo].[VendorCategory].Phone when '' then [dbo].[VendorInfo].ContactPhone else [dbo].[VendorCategory].Phone end as Phone "
                            + ",case [dbo].[VendorCategory].Email when '' then [dbo].[VendorInfo].Email else [dbo].[VendorCategory].Email end as Email,0 as IsEmailSend "
                            + "from [dbo].[VendorInfo],[dbo].[VendorCategory] "
                            + "where [dbo].[VendorInfo].VendorID=[dbo].[VendorCategory].VendorID "
                            + "and [dbo].[VendorCategory].ItemCategoryID=@categoryId ORDER BY [dbo].[VendorInfo].VendorName";
            am.Utility.LoadGrid(grdVendorInfo, sqlStr, new string[] { cbxCategory.SelectedValue });
        }

        private string GenerateRefNo()
        {
            string refNo = "";

            string type = cbxType.SelectedValue;
            string year = "";
            if (dtpDate.DateInput.Text == "")
            {
                year = DateTime.Now.Year.ToString().Substring(DateTime.Now.Year.ToString().Length - 2);
            }
            else
            {
                year = dtpDate.DateInput.Text.Substring(2, 2);
                //year = dtpDate.Calendar.SelectedDate.Year.ToString().Substring(dtpDate.Calendar.SelectedDate.Year.ToString().Length - 2);

            }


            string sqlStr = "select ISNULL(Max(SUBSTRING([InvitationNo],LEN([InvitationNo])-CHARINDEX('/',reverse([InvitationNo]))+2,LEN([InvitationNo])))+1,1) as lastNo from [dbo].[Invitation] "
                                + "where SUBSTRING([InvitationNo],CHARINDEX('-',[InvitationNo])+1,2)=@year and SUBSTRING([InvitationNo],1,3)=@type";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { year, type });
            if (dt != null && dt.Rows.Count > 0)
            {
                string lastNo = dt.Rows[0]["lastNo"].ToString();
                refNo = type + "/SCI/BDCO/FY-" + year + "/" + lastNo.PadLeft(5, '0');
                //RFQ/SCI/BDCO/FY-15/00161
            }

            return refNo;
        }

        private void LoadPRInfo(int invId)
        {
            string sqlStr = "EXEC GetCategoryNUserWisePendingPR " + Convert.ToInt32(cbxCategory.SelectedValue) + "," + int.Parse(Session["StaffCode"].ToString()) + "," + invId + "";
            am.Utility.LoadGrid(grdPRInfo, sqlStr, new string[] { });
        }

        private void LoadCheckedBy()
        {
            am.Utility.LoadComboBox(cbxCheckedBy, "SELECT UserInfo.StaffCode, viewStaffInfo.StaffName FROM UserInfo" +
                    " INNER JOIN viewStaffInfo ON viewStaffInfo.StaffCode=UserInfo.StaffCode /*WHERE GroupID=12*/", "StaffName", "StaffCode");
        }

        protected void cbxType_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cbxType.SelectedValue != "")
            {
                txtInvitationNo.Text = GenerateRefNo();
            }
            if (cbxType.SelectedValue == "IFT")
            {
                divIFT.Visible = true;
            }
            else
            {
                divIFT.Visible = false;
            }
        }

        protected void dtpDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtpDate.Calendar.SelectedDate != null)
            {
                txtInvitationNo.Text = GenerateRefNo();
            }
        }

        protected void cbxCategory_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {

            if (cbxCategory.SelectedValue != "")
            {
                LoadPRInfo(0);
                LoadVendorInfo();
            }

        }


        protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdPRInfo.MasterTableView.Items)
            {
                (dataItem.FindControl("chkSelect") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdPRInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdPRInfo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked = checkHeader;
            }
        }

        protected void chkHeaderSelect_CheckedChanged1(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
            {
                (dataItem.FindControl("chkSelect") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkSelect_CheckedChanged1(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdVendorInfo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked = checkHeader;
            }
        }

        private void LoadTermsConditions()
        {
            string sqlStr = "select [TCNote],[TCTitle] from [dbo].[TermsCondition] where [Type]='RFQ' order by [TCTitle]";
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


        protected void btnPreview_Click(object sender, EventArgs e)
        {
            int invId = 0;
            if (hdfInvId.Value != "")
            {
                invId = Convert.ToInt32(hdfInvId.Value);
            }
            if (invId > 0)
            {
                Response.Redirect("Reports/Invitation.aspx?invId=" + invId + "&&type=" + txtInvitationNo.Text.Split('/')[0]);
            }
        }




        protected void btnInvSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (MailValid())
                {
                    string invId = hdfInvId.Value;
                    if (invId != "")
                    {
                        GeneratePDF(invId);

                        string[] files = new string[20];
                        DataTable dt = am.DataAccess.RecordSet("select [FilePath] from [dbo].[InvitationAttachment] where [InvitationID]=@invId",
                            new string[] { invId });
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            files[i] = Server.MapPath(dt.Rows[i]["FilePath"].ToString());
                        }

                        string[] toEmails = new string[100];
                        int j = 0;
                        foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
                        {
                            if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                            {
                                string[] emList = (dataItem.FindControl("txtEmail") as RadTextBox).Text.Split(';');
                                foreach (string em in emList)
                                {
                                    toEmails[j] = em;
                                    j++;
                                }
                                
                            }

                        }

                        string[] ccEmails = txtCCEmailID.Text.Split(';');
                        if (am.SendMail.SendOutlookMail(Page, toEmails, ccEmails, txtSubject.Text.Trim(), txtEmailBody.Text.Trim(), files))
                        {
                            SendToVendor();

                            //if (new PSMSUtility().updatePRHistory(am, grdPRInfo, "chkSelect", 3, "", Session["StaffCode"].ToString(), "",
                            //    "", "", "", "",
                            //    "PRRefNo", "PRNo", "PRItemID", "ItemID"))
                            //{
                               am.Utility.ShowHTMLAlert(Page, "000", "Mail Sent Successfully.");
                               btnInvSend.Enabled = false;
                               btnInvSend.BackColor = Color.Gray;
                            //}
                            //else
                            //{
                            //    am.Utility.ShowHTMLMessage(Page, "000", "Mail Sent, PR could not be updated.");
                            //}
                        }
                        else
                        {
                            am.Utility.ShowHTMLMessage(Page, "000", "Failed to send Mail.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
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

       
        private void LoadInvitationInfo()
        {

            string invId = hdfInvId.Value;
            string sqlStr = "SELECT [InvitationID],[InvitationNo],[InvitationType],[InvitationDate],[Sub],[Body],[StartDate],[EndDate],[Multilocation] "
                             + ",[DeliveryPlace],[InvitationMedia],[DeliveryDate],[Note],[WithinTime],[CheckedBy],Convert(varchar(10),[CheckedDate],103) as CheckedDate,[Status],[AmmendNo] "
                             + "FROM [dbo].[Invitation] WHERE [InvitationID]=@invId";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            if (dt != null && dt.Rows.Count > 0)
            {
                cbxType.SelectedValue = dt.Rows[0]["InvitationType"].ToString();
                txtInvitationNo.Text = dt.Rows[0]["InvitationNo"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["InvitationDate"])) dtpDate.SelectedDate = DateTime.Parse(dt.Rows[0]["InvitationDate"].ToString());
                txtSubject.Text = dt.Rows[0]["Sub"].ToString();
                txtBody.Text = dt.Rows[0]["Body"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["EndDate"])) dtpLastBidDate.SelectedDate = DateTime.Parse(dt.Rows[0]["EndDate"].ToString());
                if (am.Utility.IsValidDate(dt.Rows[0]["WithinTime"])) tpWithin.SelectedDate = DateTime.Parse(dt.Rows[0]["WithinTime"].ToString());
                if (am.Utility.IsValidDate(dt.Rows[0]["DeliveryDate"])) dtpDelivaryDate.SelectedDate = DateTime.Parse(dt.Rows[0]["DeliveryDate"].ToString());
                txtDeliveryPlace.Text = dt.Rows[0]["DeliveryPlace"].ToString();
                txtAttachmentNote.Text = dt.Rows[0]["Note"].ToString();
                chkMultilocation.Checked = Convert.ToBoolean(dt.Rows[0]["Multilocation"]);
                cbxCheckedBy.SelectedValue = dt.Rows[0]["CheckedBy"].ToString();
                if (Convert.ToInt32(dt.Rows[0]["CheckedBy"]) == Convert.ToInt32(Session["StaffCode"]) && Convert.ToInt32(dt.Rows[0]["Status"]) == 0)
                {
                    rdbListAR.Enabled = true;
                }
                if (Convert.ToInt32(dt.Rows[0]["Status"]) >= 2)
                {
                    btnInvSend.Enabled = true;
                    btnInvSend.BackColor = Color.FromName("#c7081b");
                }
                if (am.Utility.IsValidDate(dt.Rows[0]["CheckedDate"])) txtCheckedDate.Text = dt.Rows[0]["CheckedDate"].ToString();               
                if (dt.Rows[0]["Status"].ToString() == "2")
                {
                    rdbListAR.SelectedValue = "1";
                }
                else if (dt.Rows[0]["Status"].ToString() == "1")
                {
                    rdbListAR.SelectedValue = "0";
                }
                hdfAmendNo.Value = dt.Rows[0]["AmmendNo"].ToString();
                txtPublishPlace.Text = dt.Rows[0]["InvitationMedia"].ToString();
            }

        }

        private void LoadCategoryId()
        {

            string invId = hdfInvId.Value;
            string sqlStr = "select [CategoryID] from [dbo].[ItemSubCategory] where [SubCategoryID]= "
                             + "(select [SubCategoryID] from [dbo].[ItemInfo] where [ItemID]= "
                             + "(select top(1) [ItemID] from [dbo].[InvitationItem] where [InvitationID]=@invId))";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            if (dt != null && dt.Rows.Count > 0)
            {
                string categoryId = dt.Rows[0]["CategoryID"].ToString();
                cbxCategory.SelectedValue = categoryId;
            }

        }
        private void LoadInvitationItem()
        {

            string invId = hdfInvId.Value;
            string sqlStr = "select [PRItemID],[Specification],[Qty] from [dbo].[InvitationItem] where [InvitationID]=@invId";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (GridDataItem dataItem in grdPRInfo.MasterTableView.Items)
                    {
                        if (row["PRItemID"].ToString() == dataItem["PRItemID"].Text)
                        {
                            (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                            dataItem.Selected = true;
                            (dataItem.FindControl("txtSpecification") as RadTextBox).Text = row["Specification"].ToString();
                            (dataItem.FindControl("txtQty") as RadTextBox).Text = row["Qty"].ToString();
                        }

                    }

                }
            }

        }

        private void LoadInvitationVendor()
        {
            string invId = hdfInvId.Value;
            string sqlStr = "select [InvitationID],[VendorID],[IsEmailSend] from [dbo].[InvitationVendor] where [InvitationID]=@invId";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
                    {
                        if (row["VendorID"].ToString() == dataItem["VendorID"].Text)
                        {
                            (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                            dataItem.Selected = true;
                            dataItem["IsEmailSend"].Text = row["IsEmailSend"].ToString();
                        }

                    }

                }
            }
        }

        private void LoadInvitationTerms()
        {
            string invId = hdfInvId.Value;
            string sqlStr = "select distinct [dbo].[InvitationTerm].[Ordering], [dbo].[InvitationTerm].[TCNote],isnull([TCTitle],[dbo].[InvitationTerm].[TCNote]) as TCTitle "
                             + "from [dbo].[InvitationTerm] left join [dbo].[TermsCondition] on [dbo].[InvitationTerm].TCNote=[dbo].[TermsCondition].TCNote "
                             + "where [InvitationID]=@invId order by [Ordering]";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            lbxSTC.DataSource = null;
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

                lbxSTC.DataSource = dt;
                lbxSTC.DataTextField = "TCTitle";
                lbxSTC.DataValueField = "TCNote";
                lbxSTC.DataBind();

            }

        }

        private void LoadInvitationAttachment()
        {
            string invId = hdfInvId.Value;
            string sqlStr = "select [InvitationID],SUBSTRING([FilePath],35,LEN([FilePath])) as [FilePath],[Note] from [dbo].[InvitationAttachment] where [InvitationID]=@invId";
            am.Utility.LoadGrid(grdAttachment, sqlStr, new string[] { invId });

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            if (dt != null && dt.Rows.Count > 0)
            {
                txtAttachmentNote.Text = dt.Rows[0]["Note"].ToString();

            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                SaveInvitation();
            }
        }

        private bool Valid()
        {
            bool ret = false;

            if (cbxType.SelectedValue != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Type is required.");
                cbxType.Focus();
                return false;
            }
            if (txtInvitationNo.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Invitation No is required.");
                txtInvitationNo.Focus();
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
            if (cbxCategory.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Category is required.");
                cbxCategory.Focus();
                return false;
            }
            foreach (GridDataItem dataItem in grdPRInfo.MasterTableView.Items)
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
                grdPRInfo.Focus();
                return false;
            }
            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
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
                am.Utility.ShowHTMLMessage(Page, "000", "Vendor Selection is required.");
                grdVendorInfo.Focus();
                return false;
            }

            if (txtSubject.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Title is required.");
                txtSubject.Focus();
                return false;
            }
            if (txtBody.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Invitation Letter Body is required.");
                txtBody.Focus();
                return false;
            }
            if (dtpLastBidDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Last Bidding Date is required.");
                dtpLastBidDate.Focus();
                return false;
            }
            if (tpWithin.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Within Time is required.");
                tpWithin.Focus();
                return false;
            }
            if (dtpDelivaryDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Delivery Date is required.");
                dtpDelivaryDate.Focus();
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


            return ret;
        }

        private void SaveInvitation()
        {
            string strFields = "";
            string[] strValues = new string[] { };
            try
            {
                if (hdfInvId.Value == "")
                {
                    string invNo = GenerateRefNo();
                    strFields = "[InvitationNo],[InvitationType],[InvitationDate],[Sub],[Body],[EndDate] "
                              + ",[Multilocation],[DeliveryPlace],[InvitationMedia],[DeliveryDate],[Note],[WithinTime],[CheckedBy]";
                    strValues =
                        new string[] { invNo , cbxType.SelectedValue , dtpDate.SelectedDate.Value.ToString() , txtSubject.Text.Trim() , txtBody.Text.Trim() , dtpLastBidDate.SelectedDate.Value.ToString() ,
                            chkMultilocation.Checked.ToString() , txtDeliveryPlace.Text.Trim() , txtPublishPlace.Text.Trim() , dtpDelivaryDate.SelectedDate.Value.ToString() ,
                            txtAttachmentNote.Text.Trim() , tpWithin.SelectedDate.Value.ToShortTimeString() , cbxCheckedBy.SelectedValue};

                    am.DataAccess.BatchQuery.Insert("[dbo].[Invitation]", strFields, strValues, "[InvitationID]");
                    if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                    {
                        string invId = am.DataAccess.ActiveIdentity;
                        hdfInvId.Value = invId;
                        SaveInvitationItem(invId);
                        SaveInvitationVendor(invId);
                        SaveInvitationTerm(invId);
                        SaveInvitationAttachment(invId);
                        //GeneratePDF(invId);
                        if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                        {
                            am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                        }
                    }
                }
                else
                {

                    if (chkAmend.Checked)
                    {
                        strFields = "[InvitationNo],[InvitationType],[InvitationDate],[Sub],[Body],[EndDate] "
                              + ",[Multilocation],[DeliveryPlace],[InvitationMedia],[DeliveryDate],[Note],[WithinTime],[CheckedBy],[AmmendNo]";
                        strValues =
                            new string[] { txtInvitationNo.Text.Trim() , cbxType.SelectedValue , dtpDate.SelectedDate.Value.ToString() , txtSubject.Text.Trim() , txtBody.Text.Trim() , dtpLastBidDate.SelectedDate.Value.ToString() ,
                                chkMultilocation.Checked.ToString() , txtDeliveryPlace.Text.Trim() , txtPublishPlace.Text.Trim() , dtpDelivaryDate.SelectedDate.Value.ToString() ,
                                txtAttachmentNote.Text.Trim() , tpWithin.SelectedDate.Value.ToShortTimeString() , 
                                cbxCheckedBy.SelectedValue , (Convert.ToInt32(hdfAmendNo.Value) + 1).ToString()};

                        am.DataAccess.BatchQuery.Insert("[dbo].[Invitation]", strFields, strValues, "[InvitationID]");
                        if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                        {
                            string invId = am.DataAccess.ActiveIdentity;
                            hdfInvId.Value = invId;
                            SaveInvitationItem(invId);
                            SaveInvitationVendor(invId);
                            SaveInvitationTerm(invId);
                            SaveInvitationAttachment(invId);
                            if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                            {
                                //GeneratePDF(invId);
                                GetAmendNo();
                                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                            }
                        }
                    }
                    else
                    {
                        strFields = "[InvitationNo],[InvitationType],[InvitationDate],[Sub],[Body],[EndDate] "
                              + ",[Multilocation],[DeliveryPlace],[InvitationMedia],[DeliveryDate],[Note],[WithinTime],[CheckedBy]";
                        strValues =
                            new string[] { txtInvitationNo.Text.Trim() , cbxType.SelectedValue , dtpDate.SelectedDate.Value.ToString() , txtSubject.Text.Trim() , txtBody.Text.Trim() , dtpLastBidDate.SelectedDate.Value.ToString() ,
                                 chkMultilocation.Checked.ToString() , txtDeliveryPlace.Text.Trim() , txtPublishPlace.Text.Trim() , dtpDelivaryDate.SelectedDate.Value.ToString() ,
                                 txtAttachmentNote.Text.Trim() , tpWithin.SelectedDate.Value.ToShortTimeString() , cbxCheckedBy.SelectedValue };

                        am.DataAccess.BatchQuery.Update("[dbo].[Invitation]", strFields, strValues, "[InvitationID]=@InvitationID", new string[] { hdfInvId.Value });
                        DeleteInvitationItem(hdfInvId.Value);
                        SaveInvitationItem(hdfInvId.Value);

                        DeleteInvitationVendor(hdfInvId.Value);
                        SaveInvitationVendor(hdfInvId.Value);

                        DeleteInvitationTerm(hdfInvId.Value);
                        SaveInvitationTerm(hdfInvId.Value);

                        //DeleteInvitationAttachment(hdfInvId.Value);
                        SaveInvitationAttachment(hdfInvId.Value);

                        if (am.DataAccess.BatchQuery.Execute())
                        {
                            //GeneratePDF(hdfInvId.Value);
                            GetAmendNo();
                            am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }


        }
        private void DeleteInvitationItem(string invitationId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[InvitationItem]", "[InvitationID]=@InvitationID", new string[] { invitationId });
        }
        private void DeleteInvitationVendor(string invitationId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[InvitationVendor]", "[InvitationID]=@InvitationID", new string[] { invitationId });
        }
        private void DeleteInvitationTerm(string invitationId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[InvitationTerm]", "[InvitationID]=@InvitationID", new string[] { invitationId });
        }
        private void DeleteInvitationAttachment(string invitationId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[InvitationAttachment]", "[InvitationID]=@InvitationID", new string[] { invitationId });

        }
        private void GetAmendNo()
        {

            string invId = hdfInvId.Value;
            string sqlStr = "SELECT [InvitationID],[InvitationNo],[InvitationType],[InvitationDate],[Sub],[Body],[StartDate],[EndDate],[Multilocation] "
                             + ",[DeliveryPlace],[InvitationMedia],[DeliveryDate],[Note],[WithinTime],[CheckedBy],Convert(varchar(10),[CheckedDate],103) as CheckedDate,[Status],[AmmendNo] "
                             + "FROM [dbo].[Invitation] WHERE [InvitationID]=@invId";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { invId });
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfAmendNo.Value = dt.Rows[0]["AmmendNo"].ToString();
            }

        }
        private void SaveInvitationItem(string invitationId)
        {
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[InvitationID],[PRItemID],[ItemID],[Specification],[PackSize],[UnitID],[Qty],[Status]";

            foreach (GridDataItem dataItem in grdPRInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    strValues =
                        new string[] { invitationId , dataItem["PRItemID"].Text , dataItem["ItemID"].Text , (dataItem.FindControl("txtSpecification") as RadTextBox).Text , "0" , dataItem["UnitID"].Text ,
                         (dataItem.FindControl("txtQty") as RadTextBox).Text , "3" };

                    am.DataAccess.BatchQuery.Insert("[dbo].[InvitationItem]", strFields, strValues);
                }

            }

        }
        private void SaveInvitationVendor(string invitationId)
        {
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[InvitationID],[VendorID],[IsEmailSend]";

            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    strValues = new string[] { invitationId, dataItem["VendorID"].Text, dataItem["IsEmailSend"].Text };
                    am.DataAccess.BatchQuery.Insert("[dbo].[InvitationVendor]", strFields, strValues);
                }

            }

        }
        private void SaveInvitationTerm(string invitationId)
        {
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[InvitationID],[TCNote],[Ordering]";

            int r = 1;
            foreach (RadListBoxItem i in lbxSTC.Items)
            {
                strValues = new string[] { invitationId, i.Value.ToString(), r.ToString() };
                am.DataAccess.BatchQuery.Insert("[dbo].[InvitationTerm]", strFields, strValues);
                r++;
            }

        }
        private void SaveInvitationAttachment(string invitationId)
        {
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[InvitationID],[FilePath],[Note]";

            if (asyncUploadInvitationFile.UploadedFiles.Count > 0)
            {
                if (txtInvitationNo.Text != "")
                {
                    string fileName = txtInvitationNo.Text.Split('/')[0] + invitationId + "_";
                    string folderPath = InvitationAttachmentfolderPath;
                    String targetFolder = Server.MapPath(folderPath);
                    foreach (UploadedFile file in asyncUploadInvitationFile.UploadedFiles)
                    {
                        string filePath = folderPath + "/" + fileName + file.FileName;
                        strValues = new string[] { invitationId, filePath, txtAttachmentNote.Text.Trim() };
                        am.DataAccess.BatchQuery.Insert("[dbo].[InvitationAttachment]", strFields, strValues);
                        file.SaveAs(Path.Combine(targetFolder, fileName + file.FileName));

                    }
                }
            }

        }
        public void GeneratePDF(string invitationId)
        {
            ReportDocument rptDoc = new ReportDocument();
            dsPSMS ds = new dsPSMS();
            DataTable dt = new DataTable();

            dt.TableName = "Invitation";
            dt = getAllInvitations();

            ds.Tables["dtInvitation"].Merge(dt);

            //subreport data table 
            dt = new DataTable();
            dt.TableName = "Terms";
            dt = getAllTerms();
            ds.Tables["dtTerms"].Merge(dt);

            rptDoc.Load(Server.MapPath("~/Reports/crInvitation.rpt"));
            rptDoc.SetDataSource(ds);

            string fileName = cbxType.SelectedValue + invitationId + "_.pdf";

            String targetFolder = Server.MapPath(InvitationAttachmentfolderPath);
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
            string[] strValues = new string[] { };
            strFields = "[InvitationID],[FilePath],[Note]";
            string folderPath = InvitationAttachmentfolderPath;
            string filePath = folderPath + "/" + fileName;
            string sqlStr = "select [FilePath] from [dbo].[InvitationAttachment] where [FilePath]=@FilePath";
            DataTable hasData = am.DataAccess.RecordSet(sqlStr, new string[] { filePath });
            if (hasData.Rows.Count == 0)
            {
                strValues = new string[] { invitationId, filePath, txtAttachmentNote.Text.Trim() };
                am.DataAccess.BatchQuery.Insert("[dbo].[InvitationAttachment]", strFields, strValues);
                am.DataAccess.BatchQuery.Execute();
            }
        }

        public DataTable getAllInvitations()
        {
            string invitationId = hdfInvId.Value;
            string query = "EXEC rptINV '" + invitationId + "'";
            DataTable dt = am.DataAccess.RecordSet(query, new string[] { });
            return dt;
        }

        public DataTable getAllTerms()
        {
            string invitationId = hdfInvId.Value;
            string query = "select [InvitationNo] as TermParentNo,Ordering,[TCNote] as Note "
                            + "from [dbo].[Invitation] inner join [dbo].[InvitationTerm] on [dbo].[Invitation].InvitationID=[dbo].[InvitationTerm].InvitationID "
                            + "where [dbo].[Invitation].InvitationID=@InvitationID order by Ordering";

            DataTable dt = am.DataAccess.RecordSet(query, new string[] { invitationId });
            return dt;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }


        private void Reset()
        {
            hdfId.Value = "";
            hdfInvId.Value = "";
            hdfAmendNo.Value = "";
            cbxType.SelectedValue = "";
            txtInvitationNo.Text = "";
            dtpDate.SelectedDate = null;
            cbxCategory.SelectedValue = "0";
            grdPRInfo.DataSource = null;
            grdPRInfo.Rebind();
            grdVendorInfo.DataSource = null;
            grdVendorInfo.Rebind();
            txtCCEmailID.Text = "";
            txtEmailBody.Text = "";
            txtSubject.Text = "";
            txtBody.Text = "";
            dtpLastBidDate.SelectedDate = null;
            tpWithin.SelectedDate = null;
            dtpDelivaryDate.SelectedDate = null;
            txtDeliveryPlace.Text = "";
            chkMultilocation.Checked = false;
            txtAttachmentNote.Text = "";
            grdAttachment.DataSource = null;
            grdAttachment.Rebind();
            cbxCheckedBy.SelectedValue = "0";
            txtCheckedDate.Text = "";
            rdbListAR.SelectedValue = null;
            txtApprovalNote.Text = "";
            foreach (RadListBoxItem i in lbxTC.Items)
            {
                i.Checked = false;
            }
            lbxSTC.Items.Clear();
            txtUpdateTC.Text = "";
            txtPublishPlace.Text = "";

        }



        protected void btnApprovalSend_Click(object sender, EventArgs e)
        {
            string sqlStr = "select [Email] from [dbo].[viewStaffInfo] where [StaffCode]=@scode";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { cbxCheckedBy.SelectedValue });
            if (dt != null && dt.Rows.Count > 0)
            {
                string toEmail = dt.Rows[0]["Email"].ToString();
                if (am.SendMail.SendOutlookMail(Page, toEmail, "New Invitation for checking", txtApprovalNote.Text.Trim(), ""))
                {
                    am.Utility.ShowHTMLAlert(Page, "000", "Mail Sent Successfully");
                }

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
                am.Utility.FileDownload(Page, InvitationDownloadURL, filepath);
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
                string folderPath = InvitationAttachmentfolderPath;
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();

                am.DataAccess.BatchQuery.Delete("[dbo].[InvitationAttachment]", "[FilePath]=@FilePath", new string[] { folderPath + "/" + filepath });
                bool ret = am.DataAccess.BatchQuery.Execute();

                if (ret)
                {
                    String targetFolder = Server.MapPath(folderPath);
                    string path = Path.Combine(targetFolder, filepath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    LoadInvitationAttachment();
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
            string invId = hdfInvId.Value;
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[CheckedDate],[Status]";
            strValues = new string[] { DateTime.Now.ToString(), "2" };
            am.DataAccess.BatchQuery.Update("[dbo].[Invitation]", strFields, strValues, "[InvitationID]=@InvitationID", new string[] { invId });
            if (am.DataAccess.BatchQuery.Execute())
            {
                am.Utility.ShowHTMLAlert(Page, "000", "Verified Successfully.");
            }
        }
        private void Reject()
        {
            string invId = hdfInvId.Value;
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[CheckedDate],[Status]";
            strValues = new string[] { DateTime.Now.ToString(), "1" };
            am.DataAccess.BatchQuery.Update("[dbo].[Invitation]", strFields, strValues, "[InvitationID]=@InvitationID", new string[] { invId });
            if (am.DataAccess.BatchQuery.Execute())
            {
                am.Utility.ShowHTMLAlert(Page, "000", "Rejected Successfully.");
            }
        }
        private void SendToVendor()
        {
            string invId = hdfInvId.Value;
            string strFields = "";
            string[] strValues = new string[] { };

            strFields = "[Status]";
            strValues = new string[] { "3" };
            am.DataAccess.BatchQuery.Update("[dbo].[Invitation]", strFields, strValues, "[InvitationID]=@InvitationID", new string[] { invId });
            am.DataAccess.BatchQuery.Execute();
        }






        private void ShowMessage(string sMsg)
        {
            am.Utility.ShowHTMLMessage(Page, "000", sMsg);
        }
    }

}
