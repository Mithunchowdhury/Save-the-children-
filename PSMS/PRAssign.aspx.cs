using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class PRAssign : System.Web.UI.Page
    {
        DataTable dt = null;
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
                LoadPR();
                LoadAssignTo();
                if (Request.QueryString["prno"] != null && Request.QueryString["type"] != null)
                {
                    string queryStringVal = Request.QueryString["prno"].ToString();
                    string type = Request.QueryString["type"].ToString();
                    if (type == "apr")
                    {
                        PRSelect.Visible = true;
                        string prno = queryStringVal;
                        cbxPR.SelectedValue = prno;
                        LoadPRData(prno);                    
                        LoadChargingData();
                        LoadItemInfo();
                        LoadAssignPRItemInfo();
                    }
                    else
                    {
                        PRSelect.Visible = false;
                        string prno = queryStringVal;
                        LoadPRData(prno);
                        txtPRRefNo.Text = GenerateRefNo();
                        LoadChargingData();
                        LoadItemInfo();
                    }

                    HideAttachButton();
                }
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }


        private void LoadPRData(string prno)
        {
            try
            {
                string sqlStr = "select distinct [dbo].[viewPRTracker].[PRNo],[dbo].[PR].[PRRefNo],convert(varchar(10),[dbo].[viewPRTracker].[PRDate],103) as PRDate,'Bangladesh' as Country,[dbo].[viewPRTracker].[PriorityID],[PriorityName],[dbo].[viewPRTracker].[DonorName], "
                                + "[DonorRequirement],convert(varchar(10),[dbo].[viewPRTracker].[AwardEndDate],103) as AwardEndDate,[dbo].[viewPRTracker].[DeliveryMethod],[DeliveryMethodName],convert(varchar(10),[RequiredDate],103) as RequiredDate, "
                                + "[dbo].[viewPRTracker].[ReceiverName],[dbo].[viewPRTracker].[ReceiverAddress],[dbo].[viewPRTracker].[ReceiverEmail],[dbo].[viewPRTracker].[ReceiverPhone],[dbo].[viewPRTracker].[PRLocation],[dbo].[viewPRTracker].[RequestorID],[dbo].[viewPRTracker].[StaffName],[RequestorDesignation],[RequestorDept], "
                                + "[BHName],[BHDesignation],[BHDept],convert(varchar(10),[BHApprovedDate],103) as BHApprovedDate,[dbo].[viewPRTracker].[IsPlanned],[dbo].[viewPRTracker].[PlannedQuarter],[dbo].[PR].[AssignUserID],[dbo].[viewStaffInfo].[StaffName] as AssignUserName,[dbo].[viewStaffInfo].[Designation] as AssignUserPosition, "
                                + "[dbo].[viewStaffInfo].[Dept] as AssignUserDept,CONVERT(VARCHAR(10),[dbo].[PR].[AssignDate],103)+' '+CONVERT(VARCHAR(8),[dbo].[PR].[AssignDate],108) AS AssignDate,[dbo].[PR].[Remarks],[dbo].[PR].[Note] "
                                + "from [dbo].[viewPRTracker] left join [dbo].[PR] on [dbo].[viewPRTracker].[PRNo]=[dbo].[PR].PRNo "
                                + "left join [dbo].[viewStaffInfo] on [dbo].[PR].[AssignUserID]=[dbo].[viewStaffInfo].StaffCode "
                                + "where [dbo].[viewPRTracker].PRNo=@PRNo";

                dt = am.DataAccess.RecordSet(sqlStr, new string[] {prno});

                txtPRRefNo.Text = dt.Rows[0]["PRRefNo"].ToString();
                txtPRNo.Text = dt.Rows[0]["PRNo"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["PRDate"])) txtPRDate.Text = dt.Rows[0]["PRDate"].ToString();
                txtCountry.Text = dt.Rows[0]["Country"].ToString();
                txtPriority.Text = dt.Rows[0]["PriorityName"].ToString();
                txtDonorName.Text = dt.Rows[0]["DonorName"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["AwardEndDate"])) txtAwardEndDate.Text = dt.Rows[0]["AwardEndDate"].ToString();
                txtDeliveryMethod.Text = dt.Rows[0]["DeliveryMethodName"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["RequiredDate"])) txtGoodsRequiredDate.Text = dt.Rows[0]["RequiredDate"].ToString();
                txtReceiverName.Text = dt.Rows[0]["ReceiverName"].ToString();
                txtReceiverEmail.Text = dt.Rows[0]["ReceiverEmail"].ToString();
                txtReceiverPhoneNo.Text = dt.Rows[0]["ReceiverPhone"].ToString();
                txtReceiverLocation.Text = dt.Rows[0]["PRLocation"].ToString();
                txtRequestorName.Text = dt.Rows[0]["StaffName"].ToString();
                txtProgramManagerName.Text = dt.Rows[0]["BHName"].ToString();
                txtLogisticsName.Text = dt.Rows[0]["AssignUserName"].ToString();
                txtRequestorPosition.Text = dt.Rows[0]["RequestorDesignation"].ToString();
                txtProgramManagerPosition.Text = dt.Rows[0]["BHDesignation"].ToString();
                txtLogisticsPosition.Text = dt.Rows[0]["AssignUserPosition"].ToString();
                txtRequestorDeptSector.Text = dt.Rows[0]["RequestorDept"].ToString();
                txtProgramManagerDeptSector.Text = dt.Rows[0]["BHDept"].ToString();
                txtLogisticsDeptSector.Text = dt.Rows[0]["AssignUserDept"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["PRDate"])) txtRequestorDate.Text = dt.Rows[0]["PRDate"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["BHApprovedDate"])) txtProgramManagerDate.Text = dt.Rows[0]["BHApprovedDate"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["AssignDate"])) txtLogisticsDate.Text = dt.Rows[0]["AssignDate"].ToString();
                cbxAssignTo.SelectedValue = dt.Rows[0]["AssignUserID"].ToString();
                if (am.Utility.IsValidDate(dt.Rows[0]["AssignDate"])) txtAssignDate.Text = dt.Rows[0]["AssignDate"].ToString();
                txtAssignNote.Text = dt.Rows[0]["Remarks"].ToString();
                txtPRRequestorNote.Text = dt.Rows[0]["Note"].ToString();

                hdfPriorityId.Value = dt.Rows[0]["PriorityID"].ToString();
                hdfDonorSpecification.Value = dt.Rows[0]["DonorRequirement"].ToString();
                hdfDeliveryMethod.Value = dt.Rows[0]["DeliveryMethod"].ToString();
                hdfIsPlanned.Value = dt.Rows[0]["IsPlanned"].ToString();
                hdfPlannedQuarter.Value = dt.Rows[0]["PlannedQuarter"].ToString();
                hdfReceiverAddress.Value = dt.Rows[0]["ReceiverAddress"].ToString();
                hdfRemarks.Value = dt.Rows[0]["Remarks"].ToString();
                hdfRequistorId.Value = dt.Rows[0]["RequestorID"].ToString();

                sqlStr = "SELECT TOP 1 StatusNote FROM [PSMS].[dbo].[ViewPRHistory] WHERE StatusID <= 2 AND PRNo=@PRNo ORDER BY StatusDate DESC";
                DataTable dtH = am.DataAccess.RecordSet(sqlStr, new string[] { prno });
                if (dtH != null && dtH.Rows.Count > 0)
                {
                    string stausNote = dtH.Rows[0]["StatusNote"].ToString();
                    txtPRRequestorNote.Text = stausNote;
                }        
               
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void LoadChargingData()
        {
            try
            {
                string sqlStr = "select [Account] as [Account Code],[CostCenter] as [Cost Center],[Project] as [Project Code],[SOF] as [SOF Code], "
                                + "[DEA] as [DEA Code],[Amount] from [dbo].[viewPRChargeDetails] where [PRNo]=@PRNO";
                am.Utility.LoadGrid(grdCharging, sqlStr, new string[] {txtPRNo.Text});
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
                string sqlStr = "select ROW_NUMBER() over (ORDER BY [PRItemID]) AS SlNo,[ItemName],[Specification],'' as PackSize,[Qty], "
                                + "[UnitName],[Amount],[Qty]*[Amount] AS TotalAmount,[CurrencyName],[PRItemID],[ItemID],[SubCategoryID],[UnitID],[CurrencyID] "
                                + "from [dbo].[viewPRTracker] where [dbo].[viewPRTracker].PRNo=@PRNo";
                am.Utility.LoadGrid(grdItemInfo, sqlStr, new string[] {txtPRNo.Text});
                
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }
        private void LoadAssignPRItemInfo()
        {
            try
            {
                string sqlStr = "SELECT [PRItemID],[PRID],[PRNo],[ItemID],[ItemName],[Specification],[UnitID],[Qty],[Amount],[Qty]*[Amount] AS TotalAmount,[CurrencyID],[Status],[Note],[IsActive] "
                                  +"FROM [dbo].[PRItem] WHERE [PRNo]=@PRNo";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { txtPRNo.Text });
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                        {
                            if (row["PRItemID"].ToString() == dataItem["PRItemID"].Text)
                            {
                                if (row["Status"].ToString()=="3")
                                {
                                    (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                                    dataItem.Selected = true;
                                }
                                else if (row["Status"].ToString() == "2")
                                {
                                    (dataItem.FindControl("chkCancel") as CheckBox).Checked = true;
                                    dataItem.Selected = true;
                                }
                                else if (row["Status"].ToString() == "1")
                                {
                                    (dataItem.FindControl("chkReject") as CheckBox).Checked = true;
                                    dataItem.Selected = true;
                                }                               
                              
                            }
                        }
                    }                   
                }               

            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        private void LoadAssignTo()
        {
            try
            {
                string sqlStr = "SELECT UserInfo.StaffCode, viewStaffInfo.StaffName FROM UserInfo" +
                        " INNER JOIN viewStaffInfo ON viewStaffInfo.StaffCode=UserInfo.StaffCode";
                am.Utility.LoadComboBox(cbxAssignTo, sqlStr, "StaffName", "StaffCode");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }



        private string GenerateRefNo()
        {
            try
            {
                string year = txtPRDate.Text.Substring(txtPRDate.Text.Length - 2);
                string sqlStr = "select ISNULL(Max(SUBSTRING([PRRefNo],LEN([PRRefNo])-CHARINDEX('/',reverse([PRRefNo]))+2,LEN([PRRefNo])))+1,1) as LastNo from [dbo].[PR] "
                                + "where SUBSTRING([PRRefNo],CHARINDEX('-',[PRRefNo])+1,2)=@year";
                string lastNo = am.DataAccess.RecordSet(sqlStr, new string[] {year}).Rows[0]["LastNo"].ToString();
                string refNo = "PR/SCI/BDCO/FY-" + year + "/" + lastNo.PadLeft(5, '0');
                //PR/SCI/BDCO/FY-15/00200
                return refNo;
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
                return "";
            }

        }

        protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                (dataItem.FindControl("chkSelect") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
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

        protected void chkHeaderCancel_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                (dataItem.FindControl("chkCancel") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkCancel_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkCancel") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdItemInfo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderCancel") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderCancel") as CheckBox).Checked = checkHeader;
            }
        }

        protected void chkHeaderReject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                (dataItem.FindControl("chkReject") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkReject_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkReject") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdItemInfo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderReject") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderReject") as CheckBox).Checked = checkHeader;
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            GeneratePDF();
        }

        private void GeneratePDF()
        {
            if (txtPRRefNo.Text.Trim() == "")
            {
                am.Report.PrintReport(Page, "PRNew.rpt", txtPRNo.Text, null, null, new string[] { "@PR" }, new string[] { txtPRNo.Text });                
            }
            else
            {
                am.Report.PrintReport(Page, "PRNew.rpt", txtPRRefNo.Text, null, null, new string[] { "@PR" }, new string[] { txtPRNo.Text });                
            }           
          
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                SavePR();

            }            

        }
        private bool Valid()
        {
            bool ret = false;

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

            if (cbxAssignTo.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Assign To is required.");
                cbxAssignTo.Focus();
                return false;
            }           

            return ret;
        }

        private void SavePR()
        {
            string sqlStr = "";
            try
            {
                sqlStr = "select [PRID] from [dbo].[PR] where [PRNo]=@PRNo";
                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] {txtPRNo.Text});
                if (dt != null && dt.Rows.Count > 0)
                {
                    hdfId.Value = dt.Rows[0]["PRID"].ToString();
                }

                if (hdfId.Value != "")
                {
                    am.DataAccess.BatchQuery.Update("PR", "[PRRefNo],[AssignUserID],[AssignDate],[Remarks]",
                        new string[] {txtPRRefNo.Text.Trim() , cbxAssignTo.SelectedValue , DateTime.Now.ToString() ,
                        txtAssignNote.Text.Trim() }, "[PRNo]=@PRNo", new string[] {txtPRNo.Text});

                    UpdatePRItem();
                    if (am.DataAccess.BatchQuery.Execute())
                    {
                        updatePRHistory(2, txtPRRefNo.Text);
                    }
                }
                else
                {
                    DateTime? awdDate = null;
                    if (txtAwardEndDate.Text != "")
                    {
                        awdDate = DateTime.ParseExact(txtAwardEndDate.Text, "dd/MM/yyyy", null);
                    }
                    string PRRefNo = GenerateRefNo();
                    am.DataAccess.BatchQuery.Insert("PR",
                                "[PRNo],[PRRefNo],[PRDate],[RequestorID],[PRLocation],[PriorityID],[DonorName],[DonorSpecification] "
                                + ",[AwardEndDate],[DeliveryMethod],[IsPlanned],[PlannedQuarter],[ReceiverName],[ReceiverAddress] "
                                + ",[ReceiverPhone],[ReceiverEmail],[AssignUserID],[AssignDate],[Remarks]",
                                        new string[] {txtPRNo.Text ,
                                        PRRefNo ,
                                        DateTime.ParseExact(txtPRDate.Text, "dd/MM/yyyy", null).ToString() ,
                                        hdfRequistorId.Value ,
                                        txtReceiverLocation.Text.Trim() ,
                                        hdfPriorityId.Value ,
                                        txtDonorName.Text.Trim() ,
                                        hdfDonorSpecification.Value ,
                                        awdDate.ToString() ,
                                        hdfDeliveryMethod.Value ,
                                        Convert.ToBoolean(hdfIsPlanned.Value) == true ? "1" : "0" ,
                                        hdfPlannedQuarter.Value ,
                                        txtReceiverName.Text.Trim() ,
                                        hdfReceiverAddress.Value ,
                                        txtReceiverPhoneNo.Text.Trim() ,
                                        txtReceiverEmail.Text.Trim() ,
                                        cbxAssignTo.SelectedValue , DateTime.Now.ToString() ,
                                        txtAssignNote.Text.Trim()}, "PRID");
                    if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                    {
                        int retId = Convert.ToInt32(am.DataAccess.ActiveIdentity);
                        if (retId > 0)
                        {
                            SavePRItem(retId);
                            if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                            {
                                updatePRHistory(2, PRRefNo);
                            }
                        }
                    }
                }
                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }

        }

        private void SavePRItem(int prId)
        {
            //am.DataAccess.BatchQuery.Delete("PRItem", "[PRNo]=@PRNo", new string[] {txtPRNo.Text.Trim()});

            int status = 0;

            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    status = 3;
                }
                else if ((dataItem.FindControl("chkCancel") as CheckBox).Checked)
                {
                    status = 2;
                }
                else if ((dataItem.FindControl("chkReject") as CheckBox).Checked)
                {
                    status = 1;
                }

                am.DataAccess.BatchQuery.Insert("PRItem", "[PRItemID],[PRID],[PRNo],[ItemID],[LineNo],[SubCategoryID],[ItemName],[Specification],[UnitID] "
                            + ",[Qty],[Amount],[PackSize],[ProcessID],[ProcessDate],[CurrencyID],[Status]",
                            new string[] {dataItem["PRItemID"].Text ,
                            prId.ToString() ,
                            txtPRNo.Text ,
                            dataItem["ItemID"].Text ,
                            "1" ,
                            dataItem["SubCategoryID"].Text ,
                            dataItem["ItemName"].Text ,
                            dataItem["Specification"].Text ,
                            dataItem["UnitID"].Text ,
                            decimal.Parse(dataItem["Qty"].Text).ToString() ,
                            decimal.Parse(dataItem["Amount"].Text).ToString() ,
                            "0" ,
                            "0" , DateTime.Now.ToString() ,
                            dataItem["CurrencyID"].Text ,
                            status.ToString()
                            });
            }
        }
        private void UpdatePRItem()
        {
            int status = 0;

            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    status = 3;
                }
                else if ((dataItem.FindControl("chkCancel") as CheckBox).Checked)
                {
                    status = 2;
                }
                else if ((dataItem.FindControl("chkReject") as CheckBox).Checked)
                {
                    status = 1;
                }     

                am.DataAccess.BatchQuery.Update("PRItem", "[Status]", new string[] { status.ToString() }, "[PRItemID]=@PRItemID", new string[] { dataItem["PRItemID"].Text });
                            
            }
        }


        protected void grdItemInfo_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    string sqlStr = "select [ProcessID],[ProcessName],[Active] from [dbo].[Process] order by [ProcessName]";
                    RadComboBox combo = (RadComboBox)e.Item.FindControl("cbxProcessType");
                    am.Utility.LoadComboBox(combo, sqlStr, "ProcessName", "ProcessID");
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }

        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            string sqlStr = "";
            try
            {
                if (MailValid())
                {

                    sqlStr = "select [PRID] from [dbo].[PR] where [PRNo]=@PRNo";
                    DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { txtPRNo.Text });
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        am.DataAccess.BatchQuery.Update("PR", "[Note]", new string[] { txtPRRequestorNote.Text.Trim() }, "[PRNo]=@PRNo", new string[] { txtPRNo.Text });
                        am.DataAccess.BatchQuery.Execute();
                    }
                   
                    //cancel
                    foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                    {
                        if ((dataItem.FindControl("chkCancel") as CheckBox).Checked)
                        {
                            sqlStr = "UPDATE PRItemDetails SET IsCanceled=1 WHERE PRItemID='" + dataItem["PRItemID"].Text + "'";                          
                            am.DataAccess.BatchQuery.Update("PRItemDetails", "IsCanceled", new string[] { "1" }, "PRItemID=@PRItemID", new string[] { dataItem["PRItemID"].Text });
                        }
                    }
                    //reject
                    foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                    {
                        if ((dataItem.FindControl("chkReject") as CheckBox).Checked)
                        {                           
                            am.DataAccess.BatchQuery.Update("PRItemDetails", "IsRejected", new string[] { "1" }, "PRItemID=@PRItemID", new string[] { dataItem["PRItemID"].Text });
                        }
                    }
                 
                    am.DataAccess.BatchQuery.Execute(true, ConnectionType.OpenGOClose, dbINFO.PRConnectionString);
                    if (am.SendMail.SendOutlookMail(Page, txtReceiverEmail.Text, "PR Note", "PR No # " + txtPRNo.Text + "<br/> " + txtPRRequestorNote.Text.Trim(), ""))
                    {
                        updatePRHistory(1, "");
                        am.Utility.ShowHTMLAlert(Page, "000", "Mail Sent Successfully");
                    }



                }

            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }
        }
        private bool MailValid()
        {
            bool ret = false;

            if (txtPRRequestorNote.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Requestor note is required.");
                txtPRRequestorNote.Focus();
                return false;
            }

            return ret;
        }
      
        private bool updatePRHistory(int PRStatus, string prRefNo)
        {           
            string sql = "";
            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                bool IsChecked = (dataItem.FindControl("chkSelect") as CheckBox).Checked;
                if (PRStatus == 1) IsChecked = true;

                if (IsChecked)
                {                    
                    am.DataAccess.BatchQuery.Insert("PRHistory", "PRDetailID,PRNo,ItemID,StatusID,StatusNote,UserID",
                        new string[] { dataItem["PRItemID"].Text, txtPRNo.Text, dataItem["ItemID"].Text, PRStatus.ToString(), txtPRRequestorNote.Text, Session["StaffCode"].ToString() });
                   
                }
            }



            if (PRStatus > 1)
            {
                am.DataAccess.BatchQuery.Update("PR", "PRRefNo,AssignUserID", new string[] { prRefNo, cbxAssignTo.SelectedValue }, "PRNo=@PRNo", new string[] { txtPRNo.Text });
               
            }
            return am.DataAccess.BatchQuery.Execute(false, ConnectionType.OpenGOClose, dbINFO.PRConnectionString);
       
        }
        protected void btnA1_Click(object sender, EventArgs e)
        {
            string sqlStr = "SELECT DISTINCT AttachDocPath FROM viewPRTracker WHERE PRNo=@prno";
            am.Utility.FileDownload(Page, sqlStr, new string[]{txtPRNo.Text});

        }

        protected void btnA2_Click(object sender, EventArgs e)
        {
            string sqlStr = "SELECT DISTINCT AttachDocPath FROM viewPRTracker WHERE PRNo=@prno";
            am.Utility.FileDownload(Page, sqlStr, new string[]{txtPRNo.Text});
        }

        protected void btnA3_Click(object sender, EventArgs e)
        {
            string sqlStr = "SELECT DISTINCT AttachDocPath FROM viewPRTracker WHERE PRNo=@prno";
            am.Utility.FileDownload(Page, sqlStr, new string[]{txtPRNo.Text});

        }
        private void HideAttachButton()
        {
            btnA1.Visible = false;
            btnA2.Visible = false;
            btnA3.Visible = false;
            DataTable docA, docB, docC;
            string sqlStr;
            sqlStr = "SELECT DISTINCT AttachDocPath FROM viewPRTracker WHERE PRNo=@prno";
            docA = am.DataAccess.RecordSet(sqlStr, new string[]{txtPRNo.Text});
            sqlStr = "SELECT DISTINCT AttachDocBPath FROM viewPRTracker WHERE PRNo=@prno";
            docB = am.DataAccess.RecordSet(sqlStr, new string[]{txtPRNo.Text});
            sqlStr = "SELECT DISTINCT AttachDocCPath FROM viewPRTracker WHERE PRNo=@prno";
            docC = am.DataAccess.RecordSet(sqlStr, new string[]{txtPRNo.Text});

            if (docA.Rows[0]["AttachDocPath"].ToString() != "")
            {
                btnA1.Visible = true;
            }
            if (docB.Rows[0]["AttachDocBPath"].ToString() != "")
            {
                btnA2.Visible = true;
            }
            if (docC.Rows[0]["AttachDocCPath"].ToString() != "")
            {
                btnA3.Visible = true;
            }
        }

        protected void cbxPR_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string prno = cbxPR.SelectedValue;
            if (!string.IsNullOrEmpty(prno) && prno.Trim() != "0")
            {
                LoadPRData(prno);            
                LoadChargingData();
                LoadItemInfo();
                HideAttachButton();
            }
        }

        private void LoadPR()
        {
            string sqlStr = "select distinct [dbo].[PR].[PRID],[PRRefNo],[PR].PRNo from [dbo].[PR],[dbo].[PRItem] "
                            + "where [dbo].[PR].PRID=[dbo].[PRItem].PRID and ([IsActive]=0 or [IsActive] is null)";
            am.Utility.LoadComboBox(cbxPR, sqlStr, "PRRefNo", "PRNo");
        }


    }
}