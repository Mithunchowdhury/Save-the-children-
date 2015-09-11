using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class GRNSCN : System.Web.UI.Page
    {
        string GRNDownloadURL = "http://softdev/scms/Attachment/GRNAttachment/";
        string GRNAttachmentfolderPath = "~/Attachment/GRNAttachment";
        AppManager am = new AppManager();
        HRISGateway hg = new HRISGateway();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            //btnGRNComplete.Enabled = false;
            //btnGRNComplete.BackColor = Color.Gray;
            if (!IsPostBack)
            {
                LoadPONo();
                LoadVendorEvaluation();
                LoadPreparedByInfo();                
                LoadProgramRequestor();
                if (Request.QueryString["grnscnId"] != null && Request.QueryString["type"] != null)
                {
                    string queryStringVal = Request.QueryString["grnscnId"].ToString();
                    string type = Request.QueryString["type"].ToString();
                    if (type == "grn")
                    {
                        hdfId.Value = queryStringVal;
                        LoadGRNInfo();
                        LoadGRNItemInfo();
                        LoadGRNAttachmentInfo();
                    }
                    else if (type == "po")
                    {                        
                        //queryStringVal = purchase order ID
                        cbxPONo.SelectedValue = queryStringVal;
                        //cbxPONo_SelectedIndexChanged(null, null);
                        CBXPoNoChangeHandler(true);
                    }
                }
            }   
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void LoadProgramRequestor()
        {
            hg.getComboList_ID_Name(HRISGateway.empStatus.Active, cbxCheckedBy);
        }

        private void LoadVendorEvaluation()
        {
            string sqlStr = "select [EvaluationID],[EvaluationName] from [dbo].[VendorEvaluation] where [Active]=1";
            am.Utility.LoadComboBox(cbxDeliveryTime, sqlStr, "EvaluationName", "EvaluationID",new string[]{});           
            am.Utility.LoadComboBox(cbxGoodsQuality, sqlStr, "EvaluationName", "EvaluationID",new string[]{});
            am.Utility.LoadComboBox(cbxPresentable, sqlStr, "EvaluationName", "EvaluationID",new string[]{}); 
        }

        private void LoadPONo()
        {
            string sqlStr = "select distinct [dbo].[PO].[POID],[PONO] from [dbo].[PO],[dbo].[POItem] where [dbo].[PO].[POID]=[dbo].[POItem].POID "
                               +"and [dbo].[PO].[POID] in (select max([POID]) from [dbo].[PO] group by [PONO]) order by [PONO]";
            am.Utility.LoadComboBox(cbxPONo, sqlStr, "PONO", "POID",new string[]{});           
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

            string sqlStr = "select ISNULL(Max(SUBSTRING([GRNNo],LEN([GRNNo])-CHARINDEX('/',reverse([GRNNo]))+2,LEN([GRNNo])))+1,1) as lastNo from [dbo].[GRN] "
            + "where SUBSTRING([GRNNo],CHARINDEX('-',[GRNNo])+1,2)=@year and SUBSTRING([GRNNo],1,3)=@type";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { year, type });
            if (dt != null && dt.Rows.Count > 0)
            {
                string lastNo = dt.Rows[0]["lastNo"].ToString();
                refNo = type + "/SCI/BDCO/FY-" + year + "/" + lastNo.PadLeft(5, '0');
                //GRN/SCI/BDCO/FY-15/00029
            }
            return refNo;
        }

        protected void cbxType_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cbxType.SelectedValue != "")
            {
                txtGRNSCNNo.Text = GenerateRefNo();
                ChangeVendorEvaluationTitle();
            } 
        }
        private void ChangeVendorEvaluationTitle()
        {
            string type = cbxType.SelectedValue;
            if(type=="SCN")
            {
                tdDT.InnerText = "Ability to Meet Deadlines";
                tdGQ.InnerText = "Quality of Work/Service";
                tdP.InnerText = "Deliverables as per Requirement";
            }
            else
            {
                tdDT.InnerText = "Delivery Time";
                tdGQ.InnerText = "Goods Quality";
                tdP.InnerText = "Presentable";
            }
 
        }

        protected void dtpDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtpDate.Calendar.SelectedDate != null)
            {
                txtGRNSCNNo.Text = GenerateRefNo();
            }
        }

        protected void cbxPONo_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CBXPoNoChangeHandler(true);
        }

        private void CBXPoNoChangeHandler(bool newGRN)
        {
            if (cbxPONo.SelectedValue != "")
            {
                string poId = cbxPONo.SelectedValue;
                POInfo(poId);
                if (newGRN)
                {
                    POItemInfo(poId,newGRN);                    
                }
                else
                {                    
                    string grnId = "";                   
                    POItemInfo(grnId, newGRN);                    
                }
                LoadGRNItemInfo();
            } 
        }

        private void POInfo(string poId)
        {
            string sqlStr = "select distinct [dbo].[PO].[POID],[dbo].[PO].[VendorID],[VendorName] "
                               +" ,STUFF((SELECT ', ' + [PRRefNo] [text()] "
                                        +" FROM  (select distinct [dbo].[PR].PRID,[dbo].[PR].[PRRefNo] "
				                                +",STUFF((SELECT ', ' + [StaffName] [text()] "
							                               +" FROM  [dbo].[viewStaffInfo] "
							                               +" WHERE [StaffCode] = [dbo].[PR].[RequestorID] "
							                               +" FOR XML PATH(''), TYPE) "
						                               +" .value('.','varchar(MAX)'),1,2,' ') [StaffName] "
				                               +" ,STUFF((SELECT ', ' + [SOF] [text()] "
							                               +" FROM  [dbo].[viewPRChargeDetails] "
							                               +" WHERE [PRNo] = [dbo].[PR].PRNo "
							                               +" FOR XML PATH(''), TYPE) "
						                               +" .value('.','varchar(MAX)'),1,2,' ') [SOF] "
				                               +" from [dbo].[PR] "
				                               +" where [PRID] in "
				                                +"(select distinct [PRID] from [dbo].[POItem],[dbo].[PRItem] "
				                               +" where [dbo].[PRItem].PRItemID=[dbo].[POItem].PRItemID "
				                               +" and [POID]=@poId1)) as p "
                                         +"WHERE p.PRID = [dbo].[PRItem].PRID "
                                        +" FOR XML PATH(''), TYPE) "
                                        +".value('.','varchar(MAX)'),1,2,' ') [PRRefNo] "
                                +",STUFF((SELECT ', ' + [StaffName] [text()] "
                                        +" FROM  (select distinct [dbo].[PR].PRID,[dbo].[PR].[PRRefNo] "
				                               +" ,STUFF((SELECT ', ' + [StaffName] [text()] "
							                               +" FROM  [dbo].[viewStaffInfo] "
							                               +" WHERE [StaffCode] = [dbo].[PR].[RequestorID] "
							                               +" FOR XML PATH(''), TYPE) "
						                                +".value('.','varchar(MAX)'),1,2,' ') [StaffName] "
				                                +",STUFF((SELECT ', ' + [SOF] [text()] "
							                               +" FROM  [dbo].[viewPRChargeDetails] "
							                               +" WHERE [PRNo] = [dbo].[PR].PRNo "
							                               +" FOR XML PATH(''), TYPE) "
						                                +".value('.','varchar(MAX)'),1,2,' ') [SOF] "
				                               +" from [dbo].[PR] "
				                               +" where [PRID] in "
				                               +" (select distinct [PRID] from [dbo].[POItem],[dbo].[PRItem] "
				                               +" where [dbo].[PRItem].PRItemID=[dbo].[POItem].PRItemID "
				                              +"  and [POID]=@poId2)) as p "
                                        +" WHERE p.PRID = [dbo].[PRItem].PRID "
                                        +" FOR XML PATH(''), TYPE) "
                                       +" .value('.','varchar(MAX)'),1,2,' ') [StaffName] "
                                +",STUFF((SELECT ', ' + [SOF] [text()] "
                                        +" FROM  (select distinct [dbo].[PR].PRID,[dbo].[PR].[PRRefNo] "
				                                +",STUFF((SELECT ', ' + [StaffName] [text()] "
							                               +" FROM  [dbo].[viewStaffInfo] "
							                               +" WHERE [StaffCode] = [dbo].[PR].[RequestorID] "
							                               +" FOR XML PATH(''), TYPE) "
						                               +" .value('.','varchar(MAX)'),1,2,' ') [StaffName] "
				                               +" ,STUFF((SELECT ', ' + [SOF] [text()] "
							                               +" FROM  [dbo].[viewPRChargeDetails] "
							                               +" WHERE [PRNo] = [dbo].[PR].PRNo "
							                               +" FOR XML PATH(''), TYPE) "
						                                +".value('.','varchar(MAX)'),1,2,' ') [SOF] "
				                               +" from [dbo].[PR] "
				                                +"where [PRID] in "
				                                +"(select distinct [PRID] from [dbo].[POItem],[dbo].[PRItem] "
				                               +" where [dbo].[PRItem].PRItemID=[dbo].[POItem].PRItemID "
				                               +" and [POID]=@poId3)) as p "
                                        +" WHERE p.PRID = [dbo].[PRItem].PRID "
                                        +" FOR XML PATH(''), TYPE) "
                                        +".value('.','varchar(MAX)'),1,2,' ') [SOF] "

                               +" from [dbo].[PO],[dbo].[POItem],[dbo].[PRItem],[dbo].[VendorInfo] "
                               +" where [dbo].[PO].[POID]=[dbo].[POItem].POID "
                               +" and [dbo].[POItem].PRItemID=[dbo].[PRItem].PRItemID "
                               +" and [dbo].[VendorInfo].[VendorID]=[dbo].[PO].[VendorID] "
                               +" and [dbo].[PO].[POID]=@poId";
            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { poId, poId, poId, poId });
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfVendorId.Value = dt.Rows[0]["VendorID"].ToString();
                txtVendor.Text = dt.Rows[0]["VendorName"].ToString();
                txtPRNo.Text = dt.Rows[0]["PRRefNo"].ToString();
                txtRequestor.Text = dt.Rows[0]["StaffName"].ToString();
                txtBudgetSource.Text = dt.Rows[0]["SOF"].ToString();               
            }
        }
        private void POItemInfo(string Id, bool newGRN)
        {
            //string sqlStr = "select ROW_NUMBER() over (ORDER BY [PRItemID]) AS SlNo,[PRItemID],[dbo].[POItem].[ItemID],[ItemName] "
            //                    +",[ItemDesc] as Specification,[dbo].[POItem].[UnitID],[UnitName],[dbo].[POItem].Qty as POQty "
            //                    +",0 as [ReceiveQty],'' as Remarks from [dbo].[PO],[dbo].[POItem],[dbo].[ItemInfo],[dbo].[Unit] "
            //                    +"where [dbo].[ItemInfo].ItemID=[dbo].[POItem].ItemID and [dbo].[POItem].UnitID=[dbo].[Unit].UnitID "
            //                    + "and [dbo].[PO].POID=[dbo].[POItem].POID and [dbo].[PO].POID=@poId";
            string sqlStr = "";
            if (newGRN)
            {
                sqlStr = "EXEC loadGRNItem " + Id;
                
            }
            else
            {
                sqlStr = "EXEC loadGRNItemEdit " + Id;
            }
            am.Utility.LoadGrid(grdItemInfo, sqlStr, new string[] { });
        }

        protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if (dataItem.Enabled == true)
                { 
                    (dataItem.FindControl("chkSelect") as CheckBox).Checked = headerCheckBox.Checked;
                    dataItem.Selected = headerCheckBox.Checked;
                }
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdItemInfo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderSelect") as CheckBox).Checked = checkHeader;
            }
        }

        private void LoadPreparedByInfo()
        {

            if (Session["StaffCode"] != null)
            {
                string preparedbyId = Session["StaffCode"].ToString();

                string sqlStr = "select [StaffCode],[StaffName],[Designation],[Dept],[ProgramName],convert(varchar(10),getdate(),103) as PreparedDate from [dbo].[viewStaffInfo] where [StaffCode]=@preparedbyId";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{preparedbyId});
                if (dt != null && dt.Rows.Count > 0)
                {                  
                    lblPreparedBy.Text=dt.Rows[0]["StaffCode"].ToString();
                    txtPreparedBy.Text = dt.Rows[0]["StaffName"].ToString();
                    txtPreparedByDesignation.Text = dt.Rows[0]["Designation"].ToString() + "-" + dt.Rows[0]["ProgramName"].ToString();
                    //txtPreparedDate.Text = dt.Rows[0]["PreparedDate"].ToString();
                }

                
            }

        }
       

        protected void cbxCheckedBy_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadCheckedByInfo();
        }
        private void LoadCheckedByInfo()
        {

            string checkedbyId = cbxCheckedBy.SelectedValue;
            string sqlStr = "select [StaffCode],[StaffName],[Designation],[Dept],[ProgramName] from [dbo].[viewStaffInfo] where [StaffCode]=@checkedbyId";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{checkedbyId});
            if (dt != null && dt.Rows.Count > 0)
            {
                txtCheckedByDesignation.Text = dt.Rows[0]["Designation"].ToString() + "-" + dt.Rows[0]["ProgramName"].ToString();
              
            }

            


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    SaveGRN();
                }
            }
            catch(Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }


        }
        private void SaveGRN()
        {

            string strFields = "";
            string[] strValues = { };
            strFields = "[GRNNo],[GRNDate],[VendorID],[RefNo],[ReceiveLocation],[POID],[Remarks],[ReceiveByName],[ReceiveByDesignation] "
                                + ",[ReceivedDate],[CheckedByName],[CheckedByDesignation],[DeliveryTime],[GoodsQuality],[Presentable] "
                                + ",[Multilocation],[IsClose]";
            string isClose = "0";
            string grnStatus = "0";
            if(chkGRNComplete.Checked)
            {
                isClose = "1";
                grnStatus = "2";
            }
            
            if (hdfId.Value == "")
            {
                strValues = new string[]{GenerateRefNo() , dtpDate.SelectedDate.Value.ToString() , hdfVendorId.Value , txtRef.Text.Trim() , txtReceiveLocation.Text.Trim() ,
                       cbxPONo.SelectedValue , txtRemarks.Text.Trim() , lblPreparedBy.Text.Trim() , txtPreparedByDesignation.Text.Trim() ,
                       DateTime.Now.ToString() , cbxCheckedBy.SelectedValue , txtCheckedByDesignation.Text.Trim() , cbxDeliveryTime.SelectedValue ,
                       cbxGoodsQuality.SelectedValue , cbxPresentable.SelectedValue , chkMultilocation.Checked.ToString() , isClose};

                am.DataAccess.BatchQuery.Insert("[dbo].[GRN]", strFields, strValues, "[GRNID]");
                if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                {
                    string grnId = am.DataAccess.ActiveIdentity;
                    SaveGRNItem(grnId);
                    SaveGRNAttachment(grnId);
                    if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                    {
                        am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                    }
                }
            }
            else
            {
                string grnId = hdfId.Value;

                if (isClose == "1")
                {
                    strFields = "[GRNNo],[GRNDate],[VendorID],[RefNo],[ReceiveLocation],[POID],[Remarks],[ReceiveByName],[ReceiveByDesignation] "
                    + ",[CheckedByName],[CheckedByDesignation],[DeliveryTime],[GoodsQuality],[Presentable] "
                    + ",[Multilocation],[IsClose],[Status]";
                    strValues = new string[]{txtGRNSCNNo.Text.Trim() , dtpDate.SelectedDate.Value.ToString() , hdfVendorId.Value , txtRef.Text.Trim() , txtReceiveLocation.Text.Trim(),
                    cbxPONo.SelectedValue , txtRemarks.Text.Trim() , lblPreparedBy.Text.Trim() , txtPreparedByDesignation.Text.Trim(),
                    cbxCheckedBy.SelectedValue , txtCheckedByDesignation.Text.Trim() , cbxDeliveryTime.SelectedValue,
                    cbxGoodsQuality.SelectedValue , cbxPresentable.SelectedValue , chkMultilocation.Checked.ToString() , isClose, grnStatus};
                }
                else
                {
                    strFields = "[GRNNo],[GRNDate],[VendorID],[RefNo],[ReceiveLocation],[POID],[Remarks],[ReceiveByName],[ReceiveByDesignation] "
                    + ",[CheckedByName],[CheckedByDesignation],[DeliveryTime],[GoodsQuality],[Presentable] "
                    + ",[Multilocation],[IsClose]";
                    strValues = new string[]{txtGRNSCNNo.Text.Trim() , dtpDate.SelectedDate.Value.ToString() , hdfVendorId.Value , txtRef.Text.Trim() , txtReceiveLocation.Text.Trim(),
                    cbxPONo.SelectedValue , txtRemarks.Text.Trim() , lblPreparedBy.Text.Trim() , txtPreparedByDesignation.Text.Trim(),
                    cbxCheckedBy.SelectedValue , txtCheckedByDesignation.Text.Trim() , cbxDeliveryTime.SelectedValue,
                    cbxGoodsQuality.SelectedValue , cbxPresentable.SelectedValue , chkMultilocation.Checked.ToString() , isClose};
                }

                am.DataAccess.BatchQuery.Update("[dbo].[GRN]", strFields, strValues, "[GRNID]=@grnId", new string[]{grnId});              

                DeleteGRNItem(grnId);
                SaveGRNItem(grnId);
                DeleteGRNAttachment(grnId);
                SaveGRNAttachment(grnId);
                if (am.DataAccess.BatchQuery.Execute())
                {
                    am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                }

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
            if (txtGRNSCNNo.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "GRN/SCN No is required.");
                txtGRNSCNNo.Focus();
                return false;
            }
            if (dtpDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Receive Date is required.");
                dtpDate.Focus();
                return false;
            }
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

            if (hdfVendorId.Value != "")
            {
                ret = true;
            }
            else
            {
                ret = false;
            }
            if (txtReceiveLocation.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Receive Location is required.");
                txtReceiveLocation.Focus();
                return false;
            }
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
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
                grdItemInfo.Focus();
                return false;
            }

            string type = "po";
            if (Request.QueryString["type"] != null)
            {
                type = Request.QueryString["type"].ToString();
            }
            if (type == "po")
            {
                foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                {
                    if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        if (Convert.ToDouble(dataItem["ReceivedQty"].Text) + Convert.ToDouble((dataItem.FindControl("txtReceiveQty") as RadTextBox).Text) > Convert.ToDouble(dataItem["POQty"].Text))
                        {
                            am.Utility.ShowHTMLMessage(Page, "000", "Receive quantity should not more than PO quantity.");
                            grdItemInfo.Focus();
                            return false;
                        }
                        else
                        {
                            ret = true;
                        }
                    }

                }
            }

            if (cbxDeliveryTime.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Delivery Time is required.");
                cbxDeliveryTime.Focus();
                return false;
            }
            if (cbxGoodsQuality.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Goods Quality is required.");
                cbxGoodsQuality.Focus();
                return false;
            }
            if (cbxPresentable.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Presentable is required.");
                cbxPresentable.Focus();
                return false;
            }


            if (cbxCheckedBy.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Program/Requestor is required.");
                cbxCheckedBy.Focus();
                return false;
            }


            return ret;
        }

        private void SaveGRNItem(string grnId)
        {
            string strFields = "";
            string[] strValues = { };

            strFields = "[GRNID],[ItemID],[Specification],[UnitID],[POQty],[ReceiveQty],[Remarks],[PRDetailID]";

            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    strValues = new string[]{grnId , dataItem["ItemID"].Text , dataItem["Specification"].Text, dataItem["UnitID"].Text ,
                        dataItem["POQty"].Text , (dataItem.FindControl("txtReceiveQty") as RadTextBox).Text ,
                        (dataItem.FindControl("txtRemarks") as RadTextBox).Text , dataItem["PRItemID"].Text};

                    am.DataAccess.BatchQuery.Insert("[dbo].[GRNItem]", strFields, strValues);                    
                }

            }

        }

        private void DeleteGRNItem(string grnId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[GRNItem]", "[GRNID]=@grnId", new string[]{grnId});                   
           
        }

        private void SaveGRNAttachment(string grnId)
        {
            string strFields = "";
            string[] strValues = {};

            strFields = "[GRNID],[GRNAttachLocation],[Note]";

            if (asyncUploadInvitationFile.UploadedFiles.Count > 0)
            {
                if (txtGRNSCNNo.Text != "")
                {
                    string fileName = txtGRNSCNNo.Text.Split('/')[0] + grnId + "_";
                    string folderPath = GRNAttachmentfolderPath;                   
                    String targetFolder = Server.MapPath(folderPath);
                    foreach (UploadedFile file in asyncUploadInvitationFile.UploadedFiles)
                    {
                        string filePath = folderPath + "/" + fileName + file.FileName;

                        strValues = new string[] { grnId, filePath, txtAttachmentNote.Text.Trim() };

                        am.DataAccess.BatchQuery.Insert("[dbo].[GRNAttachment]", strFields, strValues);                       
                       
                        file.SaveAs(Path.Combine(targetFolder, fileName + file.FileName));

                    }
                }
            }

        }
        private void DeleteGRNAttachment(string grnId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[GRNAttachment]", "[GRNID]=@grnId", new string[]{grnId});          

        }

        private void LoadGRNInfo()
        {
            string grnId = hdfId.Value;

            string sqlStr = "select [GRNID],SUBSTRING([GRNNo],1,3) as Type,[GRNNo],[GRNDate],[dbo].[GRN].[VendorID],[VendorName],[RefNo],[ReceiveLocation],[POID],[Remarks],[ReceiveByName] "
                                    + ",[ReceiveByDesignation],convert(varchar(10),[ReceivedDate],103) as [ReceivedDate],[CheckedByName],[CheckedByDesignation],convert(varchar(10),[CheckedDate],103) as [CheckedDate] "
                                    + ",[DeliveryTime],[GoodsQuality],[Presentable],[Multilocation],[IsClose],[Status] from [dbo].[GRN],[dbo].[VendorInfo] "
                                    + "where [dbo].[VendorInfo].VendorID=[dbo].[GRN].VendorID and [dbo].[GRN].[GRNID]=@grnId";

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{grnId});
            if (dt != null && dt.Rows.Count > 0)
            {
                cbxType.SelectedValue = dt.Rows[0]["Type"].ToString();
                txtGRNSCNNo.Text = dt.Rows[0]["GRNNo"].ToString();
                dtpDate.SelectedDate = DateTime.Parse(dt.Rows[0]["GRNDate"].ToString());
                txtRef.Text = dt.Rows[0]["RefNo"].ToString();
                cbxPONo.SelectedValue = dt.Rows[0]["POID"].ToString();
                hdfVendorId.Value = dt.Rows[0]["VendorID"].ToString();
                txtVendor.Text = dt.Rows[0]["VendorName"].ToString();
                POInfo(dt.Rows[0]["POID"].ToString());
                POItemInfo(grnId,false);
                txtReceiveLocation.Text = dt.Rows[0]["ReceiveLocation"].ToString();
                if (dt.Rows[0]["Multilocation"].ToString() != "")
                {
                    chkMultilocation.Checked = Convert.ToBoolean(dt.Rows[0]["Multilocation"].ToString());
                }
                else
                {
                    chkMultilocation.Checked = false;
                }
                cbxDeliveryTime.SelectedValue = dt.Rows[0]["DeliveryTime"].ToString();
                cbxGoodsQuality.SelectedValue = dt.Rows[0]["GoodsQuality"].ToString();
                cbxPresentable.SelectedValue = dt.Rows[0]["Presentable"].ToString();
                lblPreparedBy.Text = dt.Rows[0]["ReceiveByName"].ToString();
                txtPreparedByDesignation.Text = dt.Rows[0]["ReceiveByDesignation"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["ReceivedDate"])) txtPreparedDate.Text = dt.Rows[0]["ReceivedDate"].ToString();                
                cbxCheckedBy.SelectedValue = dt.Rows[0]["CheckedByName"].ToString().PadLeft(5,'0');
                txtCheckedByDesignation.Text = dt.Rows[0]["CheckedByDesignation"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["CheckedDate"])) txtCheckedDate.Text = dt.Rows[0]["CheckedDate"].ToString();                
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                if (dt.Rows[0]["IsClose"].ToString() != "")
                {
                    chkGRNComplete.Checked = Convert.ToBoolean(dt.Rows[0]["IsClose"].ToString());
                }
                else
                {
                    chkGRNComplete.Checked = false;
                }
                if (dt.Rows[0]["Status"].ToString() == "2")
                {
                    //btnGRNComplete.Enabled = true;
                    //btnGRNComplete.BackColor = Color.FromName("#c7081b");
                }  
            }

        }

     
        private void LoadGRNItemInfo()
        {
            //string grnId = hdfId.Value;

            //string sqlStr = "select ROW_NUMBER() over (ORDER BY [PRDetailID]) AS SlNo,[PRDetailID] as [PRItemID],[dbo].[GRNItem].[ItemID],[ItemName] "
            //                    + ",[Specification],[dbo].[GRNItem].[UnitID],[UnitName],[POQty] "
            //                    + ",[ReceiveQty],[dbo].[GRNItem].[Remarks] from [dbo].[GRN],[dbo].[GRNItem],[dbo].[ItemInfo],[dbo].[Unit] "
            //                    + "where [dbo].[ItemInfo].ItemID=[dbo].[GRNItem].ItemID and [dbo].[GRNItem].UnitID=[dbo].[Unit].UnitID "
            //                    + "and [dbo].[GRN].GRNID=[dbo].[GRNItem].GRNID and [dbo].[GRN].GRNID=@grnId";
            //string sqlStr = "EXEC loadGRNItem " + cbxPONo.SelectedValue;

            //DataTable dt = am.DataAccess.RecordSet(sqlStr, "");          
            //if (dt != null && dt.Rows.Count > 0)
            //{

                //foreach (DataRow row in dt.Rows)
                //{
            string type = "po";
            if (Request.QueryString["type"] != null)
            {
                type = Request.QueryString["type"].ToString();
            }
            if (type == "po")
            {
                foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                {
                    if (Convert.ToDouble(dataItem["POQty"].Text) == Convert.ToDouble(dataItem["ReceivedQty"].Text))
                    {
                        (dataItem.FindControl("chkSelect") as CheckBox).Checked = false;
                        dataItem.Selected = false;
                        dataItem.Enabled = false;
                        //(dataItem.FindControl("txtReceiveQty") as RadTextBox).Text = row["ReceiveQty"].ToString();
                        //(dataItem.FindControl("txtRemarks") as RadTextBox).Text = row["Remarks"].ToString();
                    }
                    else
                    {
                        (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                        dataItem.Selected = true;
                        dataItem.Enabled = true;
                    }

                }
            }
            else
            {
                foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                {
                    if (Convert.ToDouble(dataItem["POQty"].Text) == Convert.ToDouble(dataItem["ReceivedQty"].Text))
                    {
                        (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                        dataItem.Selected = true;
                        //dataItem.Enabled = true;
                      
                    }
                    

                }
            }

                //}
            //}

            
        }

        private void LoadGRNAttachmentInfo()
        {
            string grnId = hdfId.Value;

            string sqlStr = "select [GRNID],SUBSTRING([GRNAttachLocation],27,LEN([GRNAttachLocation])) as [FilePath],[Note] from [dbo].[GRNAttachment] where [GRNID]=@grnId";
            am.Utility.LoadGrid(grdAttachment, sqlStr, new string[]{grnId});

            DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{grnId});
            if (dt != null && dt.Rows.Count > 0)
            {
                txtAttachmentNote.Text = dt.Rows[0]["Note"].ToString();

            }
          
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string grnId = "0";            
            string type = "";
            string grnNo="";
            try
            {
                grnId = hdfId.Value;
                type = cbxType.SelectedValue;
                grnNo=txtGRNSCNNo.Text;
            }
            catch(Exception ex)
            {

            }
            if (grnId != "")
            {
                Response.Redirect("Reports/GRN.aspx?grnId=" + grnId + "&&type=" + type + "&&grnNo=" + grnNo);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            Request.QueryString["grnscnId"] = null;
            hdfId.Value = "";
            hdfVendorId.Value = "";
            cbxType.SelectedValue = "";
            txtGRNSCNNo.Text = "";
            dtpDate.SelectedDate = null;
            txtRef.Text = "";
            cbxPONo.SelectedValue = "0";
            txtVendor.Text = "";
            txtPRNo.Text = "";
            txtRequestor.Text = "";
            txtBudgetSource.Text = "";
            txtReceiveLocation.Text = "";
            chkMultilocation.Checked=false;
            txtAttachmentNote.Text = "";
            grdAttachment.DataSource = null;
            grdAttachment.Rebind();
            grdItemInfo.DataSource = null;
            grdItemInfo.Rebind();
            cbxDeliveryTime.SelectedValue = "0";
            cbxGoodsQuality.SelectedValue = "0";
            cbxPresentable.SelectedValue = "0";           
            LoadPreparedByInfo();
            txtPreparedDate.Text = "";
            cbxCheckedBy.SelectedValue = "0";
            txtCheckedByDesignation.Text = "";
            txtCheckedDate.Text = "";           
            txtRemarks.Text = "";
            chkGRNComplete.Checked = false;

        }
        protected void btnGRNComplete_Click(object sender, EventArgs e)
        {

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
                //string folderPath = "~/bin/GRNAttachment";
                //String targetFolder = Server.MapPath(folderPath);
                //string path = Path.Combine(targetFolder, filepath);                             
                //System.Diagnostics.Process.Start(path);
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();
                am.Utility.FileDownload(Page, GRNDownloadURL, filepath);
            }
            catch (Exception ex)
            {
               
            }
        }
        private void DeleteAttachment(GridDataItem gdItem)
        {           
            try
            {
                string folderPath = GRNAttachmentfolderPath;
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();

                am.DataAccess.BatchQuery.Delete("[dbo].[GRNAttachment]", "[GRNAttachLocation]=@GRNAttachLocation", new string[]{folderPath + "/" + filepath});
                bool ret = am.DataAccess.BatchQuery.Execute();                
               
                if (ret)
                {
                    String targetFolder = Server.MapPath(folderPath);
                    string path = Path.Combine(targetFolder, filepath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    LoadGRNAttachmentInfo();
                }
            }
            catch (Exception ex)
            {
               
            }
        }

















    }
}