using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class Payment : System.Web.UI.Page
    {
        string PMTDownloadURL = "http://softdev/scms/Attachment/PaymentAttachment/";
        string PMTAttachmentfolderPath = "~/Attachment/PaymentAttachment";

        AppManager am = new AppManager();
        PaymentGateway pg = new PaymentGateway();
        HRISGateway hg = new HRISGateway();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                dtpDate.SelectedDate = DateTime.Now;
                txtBillNo.Text = GenerateRefNo();
                LoadPONo();
                LoadPreparedByInfo();
                LoadCheckedBy();

                pg.getPaymentLocation(cbxLocation);
                pg.getPaymentCurrency(cbxCurrency);
                pg.getPaymentType(cbxPaymentType);

                LoadVendorEvaluation();
                if (!string.IsNullOrEmpty(Request.QueryString["paymentId"]) && Request.QueryString["type"] != null)
                {
                    string queryStringVal = Request.QueryString["paymentId"].ToString();
                    string type = Request.QueryString["type"].ToString();
                    if (type == "po")
                    {
                        cbxPONo.SelectedValue = queryStringVal;
                        cbxPONo_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        hdfId.Value = Request.QueryString["paymentId"].ToString();
                        LoadPaymentInfo();
                        LoadPaymentChargingInfo();
                        LoadPaymentAttachmentInfo();
                    }
                }
            }
        }

        private void LoadPaymentAttachmentInfo()
        {
            string pmtId = hdfId.Value;
            string sqlStr = "select [PId],SUBSTRING([FilePath],32,LEN([FilePath])) as [FilePath],[Note] from [dbo].[PaymentAttach] where [PId]=@PId";
            am.Utility.LoadGrid(grdAttachment, sqlStr, new string[] { pmtId });

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { pmtId });
            if (dt != null && dt.Rows.Count > 0)
            {
                txtAttachmentNote.Text = dt.Rows[0]["Note"].ToString();

            }
        }

        private void LoadPaymentChargingInfo()
        {
            string sqlStr = "select ROW_NUMBER() over (ORDER BY [dbo].[PaymentCharge].[PCId]) AS SlNo,[PCId] "
                            + ",[PId],[PORef],[Narrative],[Account],[CostCenter],[Project],[SOF],[DEA],[Analysis],[Amount],[PartialAmount] "
                            + "FROM [dbo].[PaymentCharge] WHERE [PId]=@PId";
            am.Utility.LoadGrid(grdPaymentCharges, sqlStr, new string[] { hdfId.Value });
        }

        private void LoadPaymentInfo()
        {
            try
            {
                string pmtId = hdfId.Value;
                string sqlStr = "SELECT [PId],[TrackingNo],[BillEntryNo],[EntryDate],[VendorID],[InvoiceNo],[InvoiceDate],[InvoiceTotal],[PONO],[PRNO] "
                                + ",[GRNNO],[CurrencyID],[Description],[Remark],[AuthorizedStaffCode],[AuthorrizedStaffDesignation],[PerparedStaffCode] "
                                + ",[PreparedStaffDesignation],[PaymentMethod],[PaymentRef],[IsClose],[CommunicationFollowUp],[CommitmentFollowUp],[InvoiceChecked] "
                                + ",[CorrectCoding],[GRN],[ProcurementFollowed],[AuthorizedPerDEA],[PaymentAmount],[ReceivedPayment],[ReceivedDate] "
                                + "FROM [dbo].[PaymentInfo] WHERE [PId]=@PId";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { pmtId });
                if (dt != null && dt.Rows.Count > 0)
                {
                    string qry = "select [POID] from [dbo].[PO] where [PONO]=@PONO";
                    DataTable dtPayment = am.DataAccess.RecordSet(qry, new string[] { dt.Rows[0]["PONO"].ToString() });
                    cbxPONo.SelectedValue = dtPayment.Rows[0]["POID"].ToString();
                    txtPRNo.Text = dt.Rows[0]["PRNO"].ToString();
                    txtGRNSCNNo.Text = dt.Rows[0]["GRNNO"].ToString();
                    hdfVendorId.Value = dt.Rows[0]["VendorID"].ToString();
                    LoadVendorName();
                    txtTrackingNo.Text = dt.Rows[0]["TrackingNo"].ToString();
                    txtBillNo.Text = dt.Rows[0]["BillEntryNo"].ToString();
                    if (am.Utility.IsValidDate(dt.Rows[0]["EntryDate"])) dtpDate.SelectedDate = DateTime.Parse(dt.Rows[0]["EntryDate"].ToString());
                    cbxCurrency.SelectedValue = dt.Rows[0]["CurrencyID"].ToString();
                    txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
                    if (am.Utility.IsValidDate(dt.Rows[0]["InvoiceDate"])) dtpInvoiceDate.SelectedDate = DateTime.Parse(dt.Rows[0]["InvoiceDate"].ToString());
                    txtInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();
                    txtDescription.Text = dt.Rows[0]["Description"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remark"].ToString();
                    cbxAuthorizedBy.SelectedValue = dt.Rows[0]["AuthorizedStaffCode"].ToString();
                    txtCheckedByDesignation.Text = dt.Rows[0]["AuthorrizedStaffDesignation"].ToString();
                    hdfPreparedById.Value = dt.Rows[0]["PerparedStaffCode"].ToString();
                    txtPreparedBy.Text = hg.getName(hdfPreparedById.Value);
                    txtPreparedByDesignation.Text = dt.Rows[0]["PreparedStaffDesignation"].ToString();
                    cbxPaymentType.SelectedValue = dt.Rows[0]["PaymentMethod"].ToString();
                  
                    if (am.Utility.IsValidDate(dt.Rows[0]["ReceivedDate"])) dtpInvoiceReceivedDate.SelectedDate = DateTime.Parse(dt.Rows[0]["ReceivedDate"].ToString());
                   
                    if (Convert.ToInt32(dt.Rows[0]["IsClose"]) == 1)
                    {
                        chkFileClose.Checked = true;
                    }
                    else
                    {
                        chkFileClose.Checked = false;
                    }
                    cbxFollowUp.SelectedValue = dt.Rows[0]["CommunicationFollowUp"].ToString();
                    cbxCommitment.SelectedValue = dt.Rows[0]["CommitmentFollowUp"].ToString();
                    chkInvoiceChecked.Checked = Convert.ToBoolean(dt.Rows[0]["InvoiceChecked"].ToString());
                    chkCorrectCoding.Checked = Convert.ToBoolean(dt.Rows[0]["CorrectCoding"].ToString());
                    chkGoodsServiceReceived.Checked = Convert.ToBoolean(dt.Rows[0]["GRN"].ToString());
                    chkProcurementFollowed.Checked = Convert.ToBoolean(dt.Rows[0]["ProcurementFollowed"].ToString());
                    txtPaymentAmount.Text = dt.Rows[0]["PaymentAmount"].ToString();
                    sqlStr = "select sum([Qty]*[Price]) as POAmount from [dbo].[POItem] where [POID]=@POID";
                    DataTable dtPOAmount = am.DataAccess.RecordSet(sqlStr, new string[] { dtPayment.Rows[0]["POID"].ToString() });
                    txtPOAmount.Text = dtPOAmount.Rows[0]["POAmount"].ToString();
                    sqlStr = "select isnull(sum([PaymentAmount]),0) as [PaidAmount]  from [dbo].[PaymentInfo] where [PONO]=@PONO";
                    DataTable dtPaidAmount = am.DataAccess.RecordSet(sqlStr, new string[] { dt.Rows[0]["PONO"].ToString() });
                    txtPaidAmount.Text = (Convert.ToDouble(dtPaidAmount.Rows[0]["PaidAmount"]) - Convert.ToDouble(txtPaymentAmount.Text)).ToString();
                    rdbListPaymentMethod.SelectedValue = dt.Rows[0]["PaymentMethod"].ToString();


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
                string sqlStr = "select [VendorCode],[VendorName],[Email] from [dbo].[VendorInfo] where VendorID=@VendorID";

                DataTable dtVendor = am.DataAccess.RecordSet(sqlStr, new string[] { vendorId.ToString() });
                if (dtVendor != null && dtVendor.Rows.Count > 0)
                {
                    hdfVendorCode.Value = dtVendor.Rows[0]["VendorCode"].ToString();
                    txtVendor.Text = dtVendor.Rows[0]["VendorName"].ToString();

                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        protected void cbxPONo_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cbxPONo.SelectedValue != "")
            {
                string poId = cbxPONo.SelectedValue;
                LoadPOInfo(poId);
                LoadPRCharging(poId);
            }
        }

        private void LoadPOInfo(string poId)
        {
            try
            {
                string sqlStr = "select [dbo].[PO].[VendorID],[dbo].[VendorInfo].[VendorCode],[dbo].[VendorInfo].[VendorName],[dbo].[PO].[Address],[dbo].[PO].[Currency] "
                                + ",(select STUFF((SELECT ', ' + [PRRefNo] [text()] from [dbo].[PR] where [dbo].[PR].[PRID] in "
                                + "(select distinct [dbo].[PRItem].[PRID] from [dbo].[POItem],[dbo].[PRItem] "
                                + "where [dbo].[PRItem].PRItemID=[dbo].[POItem].PRItemID and [POID]=@poId1) "
                                + "  FOR XML PATH(''), TYPE).value('.','varchar(MAX)'),1,2,' ')) as [PRRefNo] "
                                + ",(select STUFF((SELECT ', ' + [GRNNo] [text()] from [dbo].[GRN] where [POID]=@poId2 "
                                + " FOR XML PATH(''), TYPE).value('.','varchar(MAX)'),1,2,' ')) as [GRNNo] "
                                + ",(select sum([Qty]*[Price]) from [dbo].[POItem] where [POID]=@poId3) as POAmount "
                                + ",(select isnull(sum([PaymentAmount]),0) as [PaymentAmount]  from [dbo].[PaymentInfo] where [PONO]= "
                                + "(select [PONO] from [dbo].[PO] where [POID]=@poId4)) as PaidAmount "
                                + "from [dbo].[PO],[dbo].[VendorInfo] "
                                + "where [dbo].[VendorInfo].VendorID=[dbo].[PO].VendorID and [POID]=@poId";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId, poId, poId, poId, poId });
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtPRNo.Text = dt.Rows[0]["PRRefNo"].ToString();
                    txtGRNSCNNo.Text = dt.Rows[0]["GRNNo"].ToString();
                    hdfVendorId.Value = dt.Rows[0]["VendorID"].ToString();
                    hdfVendorCode.Value = dt.Rows[0]["VendorCode"].ToString();
                    txtVendor.Text = dt.Rows[0]["VendorName"].ToString();
                    cbxCurrency.SelectedValue = dt.Rows[0]["Currency"].ToString();
                    txtPOAmount.Text = dt.Rows[0]["POAmount"].ToString();
                    txtPaidAmount.Text = dt.Rows[0]["PaidAmount"].ToString();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadPRCharging(string poId)
        {
            string sqlStr = "";

            sqlStr = "select sum([Qty]*[Price]) as POAmount from [dbo].[POItem] where [POID]=@poId";
            DataTable dtPOAmount = am.DataAccess.RecordSet(sqlStr, new string[] { poId });

            string sPrNo = "";
            string[] sPRnoArr = txtPRNo.Text.Replace(';', ',').Split(',');
            for (int i = 0; i < sPRnoArr.Length; i++)
            {
                sPrNo += "'" + sPRnoArr[i].Trim() + "',";
            }
            sPrNo = sPrNo.Substring(0, sPrNo.Length - 1);

            sqlStr = "select sum([Amount]) as PRAmount from [dbo].[viewPRChargeDetails] "
                     + "where [PRNo] in (select [PRNo] from [dbo].[PR] where [PRRefNo] in (" + sPrNo + "))";
            DataTable dtPRAmount = am.DataAccess.RecordSet(sqlStr, new string[] { });

            sqlStr = "select ROW_NUMBER() over (ORDER BY [PRNo]) AS SlNo,'' as Narrative,[Account],[CostCenter],[Project],[SOF],[DEA] "
                           + " ,'' as Analysis,[Amount] as PartialAmount "
                           + " from [dbo].[viewPRChargeDetails] "
                           + " where [PRNo] in (select [PRNo] from [dbo].[PR] where [PRRefNo] in (" + sPrNo + "))";

            DataTable dtCharging = am.DataAccess.RecordSet(sqlStr, new string[] { });

            if (dtCharging != null && dtCharging.Rows.Count > 0)
            {
                if (dtPOAmount != null && dtPOAmount.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCharging.Rows)
                    {
                        string PartialAmount = "";
                        PartialAmount = (Math.Round((double.Parse(row["PartialAmount"].ToString()) * double.Parse(dtPOAmount.Rows[0]["POAmount"].ToString())) / double.Parse(dtPRAmount.Rows[0]["PRAmount"].ToString()), MidpointRounding.AwayFromZero)).ToString();
                        row["PartialAmount"] = PartialAmount;
                    }

                }
            }

            grdPaymentCharges.DataSource = dtCharging;
            grdPaymentCharges.DataBind();
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
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
                    year = dtpDate.DateInput.Text.Substring(2, 2);
                }

                string sqlStr = "select ISNULL(Max(SUBSTRING([BillEntryNo],LEN([BillEntryNo])-CHARINDEX('/',reverse([BillEntryNo]))+2,LEN([BillEntryNo])))+1,1) as LastNo from [dbo].[PaymentInfo] "
                                + "where SUBSTRING([BillEntryNo],CHARINDEX('-',[BillEntryNo])+1,2)=@Year";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { year });

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lastNo = dt.Rows[0]["LastNo"].ToString();
                    refNo = "Bill/SCI/BDCO/FY-" + year + "/" + lastNo.PadLeft(5, '0');
                    //Bill/SCI/BDCO/FY-15/00001
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
                txtBillNo.Text = GenerateRefNo();
            }
        }

        private void LoadPONo()
        {
            try
            {
                string sqlStr = "select distinct [dbo].[PO].[POID],[PONO] from [dbo].[PO],[dbo].[POItem] where [dbo].[PO].[POID]=[dbo].[POItem].POID "
                                + "and [dbo].[PO].[POID] in (select max([POID]) from [dbo].[PO] group by [PONO]) and [IsActive]=1 order by [PONO]";
                am.Utility.LoadComboBox(cbxPONo, sqlStr, "PONO", "POID", new string[] { });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }


        private void LoadVendorEvaluation()
        {
            try
            {
                string sqlStr = "select [EvaluationID],[EvaluationName] from [dbo].[VendorEvaluation] where [Active]=1";
                am.Utility.LoadComboBox(cbxFollowUp, sqlStr, "EvaluationName", "EvaluationID", new string[] { });
                am.Utility.LoadComboBox(cbxCommitment, sqlStr, "EvaluationName", "EvaluationID", new string[] { });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
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
                        hdfPreparedById.Value = dt.Rows[0]["StaffCode"].ToString();
                        txtPreparedBy.Text = dt.Rows[0]["StaffName"].ToString();
                        txtPreparedByDesignation.Text = dt.Rows[0]["Designation"].ToString() + "-" + dt.Rows[0]["Dept"].ToString();
                    }
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
                am.Utility.LoadComboBox(cbxAuthorizedBy, "SELECT UserInfo.StaffCode, viewStaffInfo.StaffName FROM UserInfo" +
                    " INNER JOIN viewStaffInfo ON viewStaffInfo.StaffCode=UserInfo.StaffCode WHERE GroupID=12", "StaffName", "StaffCode", new string[] { });
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void cbxAuthorizedBy_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadCheckedByInfo();
        }

        private void LoadCheckedByInfo()
        {
            try
            {
                string sqlStr = "select [StaffCode],[StaffName],[Designation],[Dept] from [dbo].[viewStaffInfo] where StaffCode=@StaffCode";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { cbxAuthorizedBy.SelectedValue });
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtCheckedByDesignation.Text = dt.Rows[0]["Designation"].ToString() + "-" + dt.Rows[0]["Dept"].ToString();
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    if (txtPaidAmount.Text == "") txtPaidAmount.Text = "0";
                    if (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble(txtPaymentAmount.Text) > Convert.ToDouble(txtPOAmount.Text)+(Convert.ToDouble(txtPOAmount.Text) * 20) / 100)
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "Payment amount can't be more than 20% of PO amount");
                        return;
                    }

                    if (hdfId.Value == "")
                    {
                        SaveRecord();
                    }
                    else
                    {
                        EditRecord(hdfId.Value);
                    }
                }
                else
                {
                    am.Utility.ShowHTMLMessage(Page, "000", "Please provide all the required information");
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }


        private void SaveRecord()
        {
            string strFields =
                "[BillEntryNo],[EntryDate],[VendorID],[InvoiceNo],[InvoiceDate],[InvoiceTotal],[PONO],[PRNO],[GRNNO],[CurrencyID] " +
                ",[Description],[Remark],[AuthorizedStaffCode],[AuthorrizedStaffDesignation],[PerparedStaffCode],[PreparedStaffDesignation] " +
                ",[PaymentMethod],[IsClose],[CommunicationFollowUp],[CommitmentFollowUp],[InvoiceChecked],[CorrectCoding],[GRN] " +
                ",[ProcurementFollowed],[PaymentAmount],[ReceivedDate]";

            string isClose = "0";
            if (chkFileClose.Checked)
            {
                isClose = "1";
            }

            string[] strValues =
                new string[]{txtBillNo.Text.Trim() , dtpDate.SelectedDate.Value.ToString() , hdfVendorId.Value , txtInvoiceNo.Text.Trim() , dtpInvoiceDate.SelectedDate.Value.ToString() , txtInvoiceTotal.Text.Trim() ,
                cbxPONo.Text , txtPRNo.Text.Trim(), txtGRNSCNNo.Text.Trim() , cbxCurrency.SelectedValue , txtDescription.Text.Trim() ,
                txtRemarks.Text.Trim() , cbxAuthorizedBy.SelectedValue , txtCheckedByDesignation.Text.Trim() , hdfPreparedById.Value ,
                txtPreparedByDesignation.Text.Trim() , rdbListPaymentMethod.SelectedValue , isClose ,
                cbxFollowUp.SelectedValue , cbxCommitment.SelectedValue , chkInvoiceChecked.Checked.ToString() , chkCorrectCoding.Checked.ToString(),
                chkGoodsServiceReceived.Checked.ToString() , chkProcurementFollowed.Checked.ToString() , txtPaymentAmount.Text.Trim(), dtpInvoiceReceivedDate.SelectedDate.Value.ToString()};
            am.DataAccess.BatchQuery.Insert("PaymentInfo", strFields, strValues, "PId");
            if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
            {
                string sPID = am.DataAccess.ActiveIdentity;
                hdfId.Value = sPID;

                strFields = "[PId],[PORef],[Narrative],[Account],[CostCenter],[Project],[SOF],[DEA],[Analysis],[Amount],[PartialAmount]";
                foreach (GridDataItem dataItem in grdPaymentCharges.MasterTableView.Items)
                {
                    strValues = new string[]{ sPID , cbxPONo.Text ,
                       (dataItem.FindControl("txtNarrative") as RadTextBox).Text ,
                       (dataItem.FindControl("txtAccount") as RadTextBox).Text ,
                       (dataItem.FindControl("txtCostCenter") as RadTextBox).Text ,
                       (dataItem.FindControl("txtProject") as RadTextBox).Text ,
                       (dataItem.FindControl("txtSOF") as RadTextBox).Text ,
                       (dataItem.FindControl("txtDEA") as RadTextBox).Text ,
                       (dataItem.FindControl("txtAnalysis") as RadTextBox).Text ,
                       txtPOAmount.Text ,
                       (dataItem.FindControl("txtAmount") as RadTextBox).Text};
                    am.DataAccess.BatchQuery.Insert("PaymentCharge", strFields, strValues);
                }

                string fileName = "Payment" + sPID + "_";
                string folderPath = PMTAttachmentfolderPath;
                String targetFolder = Server.MapPath(folderPath);
                strFields = "[PId],[FilePath],[Note]";

                for (int i = 0; i < asyncUploadPaymentFile.UploadedFiles.Count; i++)
                {
                    string filePath = folderPath + "/" + fileName + asyncUploadPaymentFile.UploadedFiles[i].FileName;
                    strValues = new string[] { sPID, filePath, txtAttachmentNote.Text.Trim() };
                    am.DataAccess.BatchQuery.Insert("[dbo].[PaymentAttach]", strFields, strValues);
                    asyncUploadPaymentFile.UploadedFiles[i].SaveAs(Path.Combine(targetFolder, fileName + asyncUploadPaymentFile.UploadedFiles[i].FileName));
                }
                if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                {
                    am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                }
            }

        }

        private void EditRecord(string sID)
        {
            string strFields =
                "[BillEntryNo],[EntryDate],[VendorID],[InvoiceNo],[InvoiceDate],[InvoiceTotal],[PONO],[PRNO],[GRNNO],[CurrencyID] " +
                ",[Description],[Remark],[AuthorizedStaffCode],[AuthorrizedStaffDesignation],[PerparedStaffCode],[PreparedStaffDesignation] " +
                ",[PaymentMethod],[IsClose],[CommunicationFollowUp],[CommitmentFollowUp],[InvoiceChecked],[CorrectCoding],[GRN] " +
                ",[ProcurementFollowed],[PaymentAmount],[ReceivedDate]";

            string isClose = "0";
            if (chkFileClose.Checked)
            {
                isClose = "1";
            }

            string[] strValues =
                new string[]{txtBillNo.Text.Trim() , dtpDate.SelectedDate.Value.ToString() , hdfVendorId.Value , txtInvoiceNo.Text.Trim() , dtpInvoiceDate.SelectedDate.Value.ToString() , txtInvoiceTotal.Text.Trim() ,
                cbxPONo.Text , txtPRNo.Text.Trim() , txtGRNSCNNo.Text.Trim() , cbxCurrency.SelectedValue , txtDescription.Text.Trim() ,
                txtRemarks.Text.Trim() , cbxAuthorizedBy.SelectedValue , txtCheckedByDesignation.Text.Trim() , hdfPreparedById.Value ,
                txtPreparedByDesignation.Text.Trim() , rdbListPaymentMethod.SelectedValue , isClose ,
                cbxFollowUp.SelectedValue , cbxCommitment.SelectedValue , chkInvoiceChecked.Checked.ToString() , chkCorrectCoding.Checked.ToString() ,
                chkGoodsServiceReceived.Checked.ToString() , chkProcurementFollowed.Checked.ToString() , txtPaymentAmount.Text.Trim(), dtpInvoiceReceivedDate.SelectedDate.Value.ToString()};

            am.DataAccess.BatchQuery.Update("PaymentInfo", strFields, strValues, "[PId]=@PId", new string[] { sID });

            am.DataAccess.BatchQuery.Delete("[dbo].[PaymentCharge]", "[PId]=@PId", new string[] { sID });
            am.DataAccess.BatchQuery.Delete("[dbo].[PaymentAttach]", "[PId]=@PId", new string[] { sID });

            strFields = "[PId],[PORef],[Narrative],[Account],[CostCenter],[Project],[SOF],[DEA],[Analysis],[Amount],[PartialAmount]";
            foreach (GridDataItem dataItem in grdPaymentCharges.MasterTableView.Items)
            {
                strValues = new string[]{sID , cbxPONo.Text ,
                   (dataItem.FindControl("txtNarrative") as RadTextBox).Text ,
                   (dataItem.FindControl("txtAccount") as RadTextBox).Text ,
                   (dataItem.FindControl("txtCostCenter") as RadTextBox).Text ,
                   (dataItem.FindControl("txtProject") as RadTextBox).Text ,
                   (dataItem.FindControl("txtSOF") as RadTextBox).Text ,
                   (dataItem.FindControl("txtDEA") as RadTextBox).Text ,
                   (dataItem.FindControl("txtAnalysis") as RadTextBox).Text ,
                   txtPOAmount.Text ,
                   (dataItem.FindControl("txtAmount") as RadTextBox).Text};
                am.DataAccess.BatchQuery.Insert("PaymentCharge", strFields, strValues);
            }

            string fileName = "Payment" + sID + "_";
            string folderPath = PMTAttachmentfolderPath;
            String targetFolder = Server.MapPath(folderPath);
            strFields = "[PId],[FilePath],[Note]";

            for (int i = 0; i < asyncUploadPaymentFile.UploadedFiles.Count; i++)
            {
                string filePath = folderPath + "/" + fileName + asyncUploadPaymentFile.UploadedFiles[i].FileName;
                strValues = new string[] { sID + "," + filePath + "," + txtAttachmentNote.Text.Trim() };
                am.DataAccess.BatchQuery.Insert("[dbo].[PaymentAttach]", strFields, strValues);
                asyncUploadPaymentFile.UploadedFiles[i].SaveAs(Path.Combine(targetFolder, fileName + asyncUploadPaymentFile.UploadedFiles[i].FileName));
            }
            if (am.DataAccess.BatchQuery.Execute())
            {
                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
            }
        }

        protected void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdfId.Value == "") return;
                if (txtTrackingNo.Text != "") return;
                if (Session["StaffCode"] != null)
                {
                    int rID = Convert.ToInt32(Session["StaffCode"]);
                    pg.postDeleteTemp(rID);
                    int i = 1;
                    foreach (GridDataItem dataItem in grdPaymentCharges.MasterTableView.Items)
                    {
                        pg.postRequestDetails(rID, i,
                           (dataItem.FindControl("txtNarrative") as RadTextBox).Text,
                           (dataItem.FindControl("txtAccount") as RadTextBox).Text,
                           (dataItem.FindControl("txtCostCenter") as RadTextBox).Text,
                           (dataItem.FindControl("txtProject") as RadTextBox).Text,
                           (dataItem.FindControl("txtSOF") as RadTextBox).Text,
                           (dataItem.FindControl("txtDEA") as RadTextBox).Text,
                           (dataItem.FindControl("txtAnalysis") as RadTextBox).Text,
                           Convert.ToDouble((dataItem.FindControl("txtAmount") as RadTextBox).Text));
                        i++;
                    }


                    pg.postRequest(rID,
                        Convert.ToInt32(hg.getSupervisorID(rID.ToString())),
                        Convert.ToInt32(cbxPaymentType.SelectedValue),
                        txtDescription.Text.Trim(),
                        Convert.ToInt32(cbxCurrency.SelectedValue),
                        Convert.ToDouble(txtPaymentAmount.Text),
                        0,
                        0,
                        DateTime.Now,
                        DateTime.Now,
                        hdfVendorCode.Value,
                        txtVendor.Text,
                        txtInvoiceNo.Text,
                        dtpInvoiceDate.SelectedDate.Value,
                        cbxPONo.Text,
                        rID.ToString(),
                        hg.getName(rID.ToString()),
                        Convert.ToInt32(cbxLocation.SelectedValue),
                        Convert.ToInt32(cbxAuthorizedBy.SelectedValue));

                    string PaymentNo = pg.getPaymentNo(cbxPONo.Text);
                    am.DataAccess.BatchQuery.Update("[dbo].[PaymentInfo]", "[TrackingNo]", new string[] { PaymentNo }, "[PId]=@PId", new string[] {hdfId.Value });
                    am.DataAccess.BatchQuery.Execute();
                    txtTrackingNo.Text = PaymentNo;
                    if (txtTrackingNo.Text != "")
                    {
                        am.Utility.ShowHTMLAlert(Page, "000", "Payment forwarded successfully.");
                    }
                    else
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "Payment was not forwarded successfully.");
                    }
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
                //string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();
                //string folderPath = "~/bin/PaymentAttachment";
                //String targetFolder = Server.MapPath(folderPath);
                //string path = Path.Combine(targetFolder, filepath);
                //System.Diagnostics.Process.Start(path);
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();
                am.Utility.FileDownload(Page, PMTDownloadURL, filepath);
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
                string folderPath = PMTAttachmentfolderPath;
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();

                am.DataAccess.BatchQuery.Delete("[dbo].[PaymentAttach]", "[FilePath]=@FilePath", new string[] { folderPath + "/" + filepath });
                bool ret = am.DataAccess.BatchQuery.Execute();

                if (ret)
                {
                    String targetFolder = Server.MapPath(folderPath);
                    string path = Path.Combine(targetFolder, filepath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    LoadPaymentAttachmentInfo();
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

            if (cbxPONo.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "PO No is required.");
                cbxPONo.Focus();
                return false;
            }
            if (txtPRNo.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "PR No is required.");
                txtPRNo.Focus();
                return false;
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
            if (hdfVendorCode.Value != "")
            {
                ret = true;
            }
            else
            {
                ret = false;
            }
            if (cbxLocation.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Location is required.");
                cbxLocation.Focus();
                return false;
            }
            if (txtBillNo.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Bill No is required.");
                txtBillNo.Focus();
                return false;
            }
            if (dtpDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Entry Date is required.");
                dtpDate.Focus();
                return false;
            }
            if (txtInvoiceTotal.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Invoice Total is required.");
                txtInvoiceTotal.Focus();
                return false;
            }
            if (dtpInvoiceDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Invoice Date is required.");
                dtpInvoiceDate.Focus();
                return false;
            }
            if (txtPOAmount.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "PO Amount is required.");
                txtPOAmount.Focus();
                return false;
            }
            if (txtPaymentAmount.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Payment Amount is required.");
                txtPaymentAmount.Focus();
                return false;
            }
            if (cbxPaymentType.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Payment Type is required.");
                cbxPaymentType.Focus();
                return false;
            }
            if (dtpInvoiceReceivedDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Invoice Receive Date is required.");
                dtpInvoiceReceivedDate.Focus();
                return false;
            }
            if (grdPaymentCharges.MasterTableView.Items.Count > 0)
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Charging is required.");
                grdPaymentCharges.Focus();
                return false;
            }
            if (cbxAuthorizedBy.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Authorized By is required.");
                cbxAuthorizedBy.Focus();
                return false;
            }

            return ret;
        }

        private void Reset()
        {
            hdfId.Value = "";
            hdfVendorId.Value = "";
            hdfVendorCode.Value = "";
            hdfPRAmount.Value = "";
            hdfPreparedById.Value = "";
            cbxPONo.SelectedValue = "0";
            txtPRNo.Text = "";
            txtRemarks.Text = "";
            txtAttachmentNote.Text = "";
            dtpDate.SelectedDate = null;
            txtBillNo.Text = "";
            txtCheckedByDesignation.Text = "";
            txtDescription.Text = "";
            txtGRNSCNNo.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoiceTotal.Text = "";
            txtPaidAmount.Text = "";
            txtPaymentAmount.Text = "";
            dtpInvoiceReceivedDate.SelectedDate = null;
            txtPOAmount.Text = "";
            txtPreparedBy.Text = "";
            txtPreparedByDesignation.Text = "";
            txtTrackingNo.Text = "";
            txtVendor.Text = "";
            cbxAuthorizedBy.SelectedValue = "0";
            cbxCommitment.SelectedValue = "0";
            cbxFollowUp.SelectedValue = "0";
            grdPaymentCharges.DataSource = null;
            grdPaymentCharges.Rebind();
            grdAttachment.DataSource = null;
            grdAttachment.Rebind();
            chkCorrectCoding.Checked = false;
            chkFileClose.Checked = false;
            chkGoodsServiceReceived.Checked = false;
            chkInvoiceChecked.Checked = false;
            chkProcurementFollowed.Checked = false;
            rdbListPaymentMethod.SelectedValue = null;

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void btnGRNPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string grnId = "";
                string[] grn = txtGRNSCNNo.Text.Split(',');
                foreach (string g in grn)
                {
                    DataTable dt = am.DataAccess.RecordSet("select [GRNID] from [dbo].[GRN] where [GRNNo]=@GRNNo", new string[] { g.Trim() });
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        grnId = dt.Rows[0]["GRNID"].ToString();
                        if (grnId != "")
                        {
                            //GeneratePDF(grnId);
                            Response.Redirect("Reports/GRN.aspx?grnId=" + grnId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }

        protected void txtPaymentAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtPaymentAmount.Text != "" && Convert.ToDouble(txtPaymentAmount.Text) > 0)
            {
                string sqlStr = "";

                string sPrNo = "";
                string[] sPRnoArr = txtPRNo.Text.Replace(';', ',').Split(',');
                for (int i = 0; i < sPRnoArr.Length; i++)
                {
                    sPrNo += "'" + sPRnoArr[i].Trim() + "',";
                }
                sPrNo = sPrNo.Substring(0, sPrNo.Length - 1);

                sqlStr = "select sum([Amount]) as PRAmount from [dbo].[viewPRChargeDetails] "
                         + "where [PRNo] in (select [PRNo] from [dbo].[PR] where [PRRefNo] in (" + sPrNo + "))";
                DataTable dtPRAmount = am.DataAccess.RecordSet(sqlStr, new string[] { });

                sqlStr = "select ROW_NUMBER() over (ORDER BY [PRNo]) AS SlNo,'' as Narrative,[Account],[CostCenter],[Project],[SOF],[DEA] "
                               + " ,'' as Analysis,[Amount] as PartialAmount "
                               + " from [dbo].[viewPRChargeDetails] "
                               + " where [PRNo] in (select [PRNo] from [dbo].[PR] where [PRRefNo] in (" + sPrNo + "))";

                DataTable dtCharging = am.DataAccess.RecordSet(sqlStr, new string[] { });

                if (dtCharging != null && dtCharging.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCharging.Rows)
                    {
                        string PartialAmount = "";
                        PartialAmount = (Math.Round((double.Parse(row["PartialAmount"].ToString()) * Convert.ToDouble(txtPaymentAmount.Text)) / double.Parse(dtPRAmount.Rows[0]["PRAmount"].ToString()), MidpointRounding.AwayFromZero)).ToString();
                        row["PartialAmount"] = PartialAmount;
                    }
                }
                grdPaymentCharges.DataSource = dtCharging;
                grdPaymentCharges.DataBind();
            }
        }

        //public void GeneratePDF(string grnId)
        //{

        //    string fileName = grnId + "_";

        //    DataTable[] dts = new DataTable[1];
        //    dts[0] = getAllGRNs(grnId);
        //    string[] tableNames = new string[1];
        //    tableNames[0] = "dtGRN";

        //    am.Report.PrintReport(Page, "crGRN.rpt", fileName, dts, tableNames);
        //}

        //private DataTable getAllGRNs(string grnId)
        //{
        //    DataTable dt = null;           

        //    string query = "EXEC rptGRN " + grnId + "";

        //    try
        //    {
        //        dt = am.DataAccess.RecordSet(query, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
        //    }
        //    return dt;
        //}

    }
}