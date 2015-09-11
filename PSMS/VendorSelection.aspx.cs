using PSMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class VendorSelection : System.Web.UI.Page
    {
        string VSDownloadURL = "http://softdev/scms/Attachment/SelectionAttachment/";
        string VSAttachmentfolderPath = "~/Attachment/SelectionAttachment";
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
                LoadIvitation();
                LoadVendors();
                LoadProcess();
                LoadFrameworkNo();
                if ((Session["pritem"] as DataTable) != null && (Session["pritem"] as DataTable).Rows.Count > 0)
                {
                    Session["pritem"] = null;
                }
                if (Request.QueryString["selectionId"] != null && Request.QueryString["type"] != null)
                {                    
                    string queryStringVal = Request.QueryString["selectionId"].ToString();
                    string type = Request.QueryString["type"].ToString();
                    if (type == "selected")
                    {         
                        hdfId.Value = queryStringVal;
                        LoadSelectionInfo();
                        LoadSelectionItem();
                        LoadSelectionVendorStatus();
                        LoadSelectionGroup();
                        LoadSelectionAttachment();
                    }
                    else if (type == "RFQ" || type == "RFP" || type == "IFT")
                    {    
                        cbxInvitation.SelectedValue = queryStringVal;
                        cbxInvitation_SelectedIndexChanged(this, null);
                    }
                    else if (type == "PR")
                    {                      
                        cbxPR.SelectedValue = queryStringVal;
                        cbxPR_SelectedIndexChanged(this, null);
                    }
                }
                

            }   
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
        private void LoadSelectionAttachment()
        {
            try
            {
                string selectionId = hdfId.Value;
                string sqlStr = "select [SelectionID],SUBSTRING([FilePath],34,LEN([FilePath])) as [FilePath],[AttachNote] from [dbo].[VendorSelectionAttach] where [SelectionID]=@selectionId";
                am.Utility.LoadGrid(grdAttachment, sqlStr, new string[]{selectionId});

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{selectionId});
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtAttachmentNote.Text = dt.Rows[0]["AttachNote"].ToString();

                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
           
        }

        private void LoadSelectionGroup()
        {
            try
            {
                string selectionId = hdfId.Value;
                string sqlStr = "select [SelectionID],[StaffID],[StaffName] from [dbo].[VendorSelectionGroup],[dbo].[viewStaffInfo] "
                                + "where [dbo].[viewStaffInfo].[StaffCode]=[dbo].[VendorSelectionGroup].StaffID "
                                + "and [dbo].[VendorSelectionGroup].SelectionID=@selectionId";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{selectionId});
                if (dt != null && dt.Rows.Count > 0)
                {
                    //int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        tbxCommitteeMember.Entries.Add(new AutoCompleteBoxEntry(row["StaffName"].ToString(), row["StaffID"].ToString()));
                        //tbxCommitteeMember.Entries[i].Value = row["StaffID"].ToString();
                        //i++;
                    }
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }

        private void LoadSelectionVendorStatus()
        {
            try
            {
                string selectionId = hdfId.Value;
                string sqlStr = "select ROW_NUMBER() over (ORDER BY [dbo].[VendorSelectionStatus].VendorID) AS SlNo,[dbo].[VendorSelectionStatus].[VendorID],[VendorCode],[VendorName],[IsParticipated],[BidPosition],[Note] "
                                + "from [dbo].[VendorSelectionStatus],[dbo].[VendorInfo]  "
                                + "where [dbo].[VendorInfo].[VendorID]=[dbo].[VendorSelectionStatus].VendorID and [dbo].[VendorSelectionStatus].SelectionID=@selectionId";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{selectionId});
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
                        {
                            if (row["VendorID"].ToString() == dataItem["VendorID"].Text)
                            {
                                (dataItem.FindControl("chkParticipate") as CheckBox).Checked = Convert.ToBoolean(row["IsParticipated"].ToString());
                                (dataItem.FindControl("txtBidPosition") as RadTextBox).Text = row["BidPosition"].ToString();
                                (dataItem.FindControl("txtSelectionNote") as RadTextBox).Text = row["Note"].ToString();

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

        private void LoadSelectionItem()
        {
            try
            {
                string selectionId = hdfId.Value;
                string sqlStr = "SELECT ROW_NUMBER() over (ORDER BY [PRDetailID]) AS SlNo,[PRDetailID] as [PRItemID],[dbo].[SelectionItem].[ItemID],[ItemName],[ItemDesc] as [Specification],[UnitName] "
                                + ",[dbo].[SelectionItem].[UnitID],[Qty],[UnitPrice],[Qty]*[UnitPrice] as TotalPrice "
                                + "FROM [dbo].[SelectionItem],[dbo].[ItemInfo],[dbo].[Unit] WHERE [dbo].[ItemInfo].ItemID=[dbo].[SelectionItem].ItemID "
                                + "AND  [dbo].[Unit].UnitID=[dbo].[SelectionItem].UnitID "
                                + "AND [SelectionID]=@selectionId";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{selectionId});
                foreach (DataRow row in dt.Rows)
                {
                    foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                    {
                        if (row["PRItemID"].ToString() == dataItem["PRItemID"].Text)
                        {
                            (dataItem.FindControl("chkSelect") as CheckBox).Checked = true;
                            dataItem.Selected = true;
                            (dataItem.FindControl("cbxUnit") as RadComboBox).SelectedValue = row["UnitID"].ToString();
                            (dataItem.FindControl("txtQty") as RadTextBox).Text = row["Qty"].ToString();
                            (dataItem.FindControl("txtUnitPrice") as RadTextBox).Text = row["UnitPrice"].ToString();
                            dataItem["TotalPrice"].Text = row["TotalPrice"].ToString();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }

        private void LoadSelectionInfo()
        {
            try
            {
                string selectionId = hdfId.Value;
                string sqlStr = "SELECT [SelectionID],[Type],[ProcessID],[SelectionDate],[VendorID],[RefNo],[Note],[TO_PO],[TO_PODate],[FWANo] "
                                  + "FROM [dbo].[VendorSelection] WHERE [SelectionID]=@selectionId";

                DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[]{selectionId});
                if (dt.Rows[0]["RefNo"].ToString().Substring(0, 2) == "PR")
                {       
                    txtPR.Text = dt.Rows[0]["RefNo"].ToString();
                    LoadMultiplePRItem();
                    string[] prRef = txtPR.Text.Split(';');
                    if(prRef.Length==1)
                    {
                        string qry = "select [PRID] from [dbo].[PR] where [PRRefNo]=@RefNo";
                        DataTable dtPR = am.DataAccess.RecordSet(qry, new string[] { txtPR.Text.Trim() });
                        cbxPR.SelectedValue = dtPR.Rows[0]["PRID"].ToString();
                    }
                    cbxInvitation.Enabled = false;
                }
                else
                {
                    //cbxInvitation.SelectedItem.Text = dt.Rows[0]["RefNo"].ToString();
                    string qry = "select max([InvitationID]) as [InvitationID] from [dbo].[Invitation] where [InvitationNo]=@RefNo";
                    DataTable dtInv = am.DataAccess.RecordSet(qry, new string[]{dt.Rows[0]["RefNo"].ToString()});
                    cbxInvitation.SelectedValue = dtInv.Rows[0]["InvitationID"].ToString();
                    LoadInvitationItemInfo();
                    LoadVendornfo();
                    cbxPR.Enabled = false;
                }

                if (am.Utility.IsValidDate(dt.Rows[0]["SelectionDate"])) dtpSelectionDate.SelectedDate = DateTime.Parse(dt.Rows[0]["SelectionDate"].ToString());
                cbxVendor.SelectedValue = dt.Rows[0]["VendorID"].ToString();
                cbxSelectionProcess.SelectedValue = dt.Rows[0]["ProcessID"].ToString();
                cbxFrameWorkNo.SelectedValue = dt.Rows[0]["FWANo"].ToString();
                txtSpecialNote.Text = dt.Rows[0]["Note"].ToString();
                chkPO.Checked = Convert.ToBoolean(dt.Rows[0]["TO_PO"]);               
                if (am.Utility.IsValidDate(dt.Rows[0]["TO_PODate"])) dtpPODate.SelectedDate = DateTime.Parse(dt.Rows[0]["TO_PODate"].ToString());
                
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }
        private void LoadMultiplePRItem()
        {
            DataTable dt=null;
            string[] prRef = txtPR.Text.Split(';');
            string qry = "select [PRID] from [dbo].[PR] where [PRRefNo]=@RefNo";
            string sqlStr = "select ROW_NUMBER() over (ORDER BY [PRItemID]) AS SlNo,[PRItemID],[ItemID],[ItemName],[Specification],[UnitName] "
                           + ",[dbo].[PRItem].UnitID,[dbo].[PRItem].Qty,0 as UnitPrice,0 as TotalPrice from [dbo].[PR],[dbo].[PRItem],[dbo].[Unit] "
                           + "where [dbo].[PR].PRID=[dbo].[PRItem].PRID and [dbo].[Unit].UnitID=[dbo].[PRItem].UnitID "
                           + "and [dbo].[PR].PRID=@prId";
            foreach (string pr in prRef)
            {
                DataTable dtPR = am.DataAccess.RecordSet(qry, new string[]{pr});
                if (dtPR != null && dtPR.Rows.Count > 0)
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.Merge(am.DataAccess.RecordSet(sqlStr, new string[] { dtPR.Rows[0]["PRID"].ToString() }));
                    }
                    else
                    {
                        dt = am.DataAccess.RecordSet(sqlStr, new string[] { dtPR.Rows[0]["PRID"].ToString()});
                    }
                }
            }

            grdItemInfo.DataSource = dt;
            grdItemInfo.DataBind();
                 
        }

        private void LoadProcess()
        {
            string sqlStr = "select [ProcessID],[ProcessName] from [dbo].[Process] order by [ProcessName]";
            am.Utility.LoadComboBox(cbxSelectionProcess, sqlStr, "ProcessName", "ProcessID",new string[]{});
        }
        private void LoadFrameworkNo()
        {
            string sqlStr = "select [FrameWorkNo] from [dbo].[FrameWork]";
            am.Utility.LoadComboBox(cbxFrameWorkNo, sqlStr, "FrameWorkNo", "FrameWorkNo",true);
        }

        private void LoadVendors()
        {
            string sqlStr = "select [VendorID],[VendorName] from [dbo].[VendorInfo] order by [VendorName]";
            am.Utility.LoadComboBox(cbxVendor, sqlStr, "VendorName", "VendorID",new string[]{});
        }

        private void LoadVendors(string invId)
        {
            string sqlStr = "select vi.[VendorID],vi.[VendorName] from [dbo].[VendorInfo] vi"+
                            " left join [dbo].[InvitationVendor] iv on vi.VendorID = iv.VendorID" +
                            " where iv.InvitationID=@InvitationID" +
                            " order by [VendorName]";
            am.Utility.LoadComboBox(cbxVendor, sqlStr, "VendorName", "VendorID", new string[]{invId});
        }

        private void LoadIvitation()
        {
            string sqlStr = "select distinct [dbo].[Invitation].[InvitationID],[dbo].[Invitation].[InvitationNo] "
                            + "from [dbo].[Invitation],[dbo].[InvitationItem] "
                            + "where [dbo].[Invitation].InvitationID=[dbo].[InvitationItem].InvitationID "
                            + "and [dbo].[Invitation].InvitationID in (select max([InvitationID]) from [dbo].[Invitation] group by [InvitationNo]) "
                            + "and ([IsActive]=0 or [IsActive] is null)";
            am.Utility.LoadComboBox(cbxInvitation, sqlStr, "InvitationNo", "InvitationID",new string[]{});
        }

        private void LoadPR()
        {
            string sqlStr = "select distinct [dbo].[PR].[PRID],[PRRefNo] from [dbo].[PR],[dbo].[PRItem] "
                            + "where [dbo].[PR].PRID=[dbo].[PRItem].PRID and ([IsActive]=0 or [IsActive] is null)";
            am.Utility.LoadComboBox(cbxPR, sqlStr, "PRRefNo", "PRID",new string[]{});
        }

        protected void grdItemInfo_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            if (e.Item is GridDataItem)
            {
                string sqlStr = "select [UnitID],[UnitName] from [dbo].[Unit] order by [UnitName]";
                RadComboBox combo = (RadComboBox)e.Item.FindControl("cbxUnit");
                am.Utility.LoadComboBox(combo, sqlStr, "UnitName", "UnitID",new string[]{});
                combo.SelectedValue = DataBinder.Eval(e.Item.DataItem, "UnitID").ToString();

            }           

            //combo.SelectedValue = ((DataRowView)e.Item.DataItem)["UnitID"].ToString();
        }


        [WebMethod]
        public static AutoCompleteBoxData GetStaffInfo(object context)
        {            
          
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();

            string sqlStr = "select [StaffCode],[StaffName],[Designation],[Dept] from [dbo].[viewStaffInfo] where StaffName like '%" + searchString + "%'";
            DataTable data = new AppManager().DataAccess.RecordSet(sqlStr, new string[]{});

            //DataTable data = sLink.GetDataTable("select [StaffCode],[StaffName],[Designation],[Dept] from [dbo].[viewStaffInfo] where StaffName like '%" + searchString + "%'");
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            foreach (DataRow row in data.Rows)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                childNode.Text = row["StaffName"].ToString();
                childNode.Value = row["StaffCode"].ToString();
                childNode.Attributes.Add("Designation", row["Designation"].ToString());
                childNode.Attributes.Add("Dept", row["Dept"].ToString());
                result.Add(childNode);
            }
           

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();

            return res;
        }

        protected void cbxPR_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cbxPR.SelectedValue != "")
            {
                bool IsExist = false;
                string[] PRRef = txtPR.Text.Split(';');
                foreach (string s in PRRef)
                {
                    if (s == cbxPR.Text.Trim())
                    {
                        IsExist = true;
                        break;
                    }
                }
                if (!IsExist)
                {
                    LoadPRItemInfo();
                    AddPR();
                }
            }
        }

        private void LoadPRItemInfo()
        {
            int count = 0;
            if ((Session["pritem"] as DataTable) != null && (Session["pritem"] as DataTable).Rows.Count > 0)
            {
                count=(Session["pritem"] as DataTable).Rows.Count;
            }

            string sqlStr = "select ROW_NUMBER() over (ORDER BY [PRItemID])+" + count.ToString() + " AS SlNo,[PRItemID],[ItemID],[ItemName],[Specification],[UnitName] "
                            + ",[dbo].[PRItem].UnitID,[dbo].[PRItem].Qty,[dbo].[PRItem].[Amount] as UnitPrice,0 as TotalPrice from [dbo].[PR],[dbo].[PRItem],[dbo].[Unit] "
                            + "where [dbo].[PR].PRID=[dbo].[PRItem].PRID and [dbo].[Unit].UnitID=[dbo].[PRItem].UnitID "
                            + "and [dbo].[PR].PRID=@prId";          
          
            
               DataTable dt = am.DataAccess.RecordSet(sqlStr, new string[] { cbxPR.SelectedValue });

               if ((Session["pritem"] as DataTable) != null && (Session["pritem"] as DataTable).Rows.Count > 0)
               {
                   (Session["pritem"] as DataTable).Merge(dt);
               }
               else
               {
                   Session.Add("pritem", dt);
               }
         
            

            grdItemInfo.DataSource = Session["pritem"] as DataTable;
            grdItemInfo.DataBind();


           
        }

        private void AddPR()
        {
            string pr = "";
            if (cbxPR.Text != "")
            {
                if (txtPR.Text == "")
                {
                    pr = cbxPR.Text.Trim();

                }
                else
                {
                    pr = ";" + cbxPR.Text.Trim();
                }
                txtPR.Text = txtPR.Text + pr;
            }
        }

        protected void cbxInvitation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cbxInvitation.SelectedValue != "")
            {
                LoadInvitationItemInfo();
                LoadVendornfo();
                LoadVendors(cbxInvitation.SelectedValue);
            }

        }

        private void LoadInvitationItemInfo()
        {
            if ((Session["pritem"] as DataTable) != null && (Session["pritem"] as DataTable).Rows.Count > 0)
            {
                Session["pritem"] = null;
            }
            string sqlStr = "select ROW_NUMBER() over (ORDER BY [PRItemID]) AS SlNo,[PRItemID],[dbo].[InvitationItem].[ItemID] "
                              + ",[ItemName],[Specification],[UnitName] ,[dbo].[InvitationItem].UnitID,[dbo].[InvitationItem].Qty,0 as UnitPrice "
                              +"from [dbo].[Invitation],[dbo].[InvitationItem],[dbo].[Unit],[dbo].[ItemInfo] "
                              +"where [dbo].[Invitation].InvitationID=[dbo].[InvitationItem].InvitationID and [dbo].[Unit].UnitID=[dbo].[InvitationItem].UnitID "
                              +"and [dbo].[ItemInfo].ItemID=[dbo].[InvitationItem].ItemID "
                              + "and [dbo].[Invitation].InvitationID=@invId";
            am.Utility.LoadGrid(grdItemInfo, sqlStr, new string[]{cbxInvitation.SelectedValue});            
        }

        private void LoadVendornfo()
        {
            string sqlStr = "select ROW_NUMBER() over (ORDER BY [dbo].[InvitationVendor].VendorID) AS SlNo,[dbo].[InvitationVendor].[VendorID],[VendorCode],[VendorName],0 as [IsParticipated],0 as [BidPosition],'' as [Note] "
                            +"from [dbo].[InvitationVendor],[dbo].[VendorInfo] "
                            + "where [dbo].[VendorInfo].[VendorID]=[dbo].[InvitationVendor].VendorID and [dbo].[InvitationVendor].InvitationID=@invId";
            am.Utility.LoadGrid(grdVendorInfo, sqlStr, new string[]{cbxInvitation.SelectedValue});           
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

        protected void chkHeaderParticipate_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
            {
                (dataItem.FindControl("chkParticipate") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkParticipate_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkParticipate") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdVendorInfo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkHeaderParticipate") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkHeaderParticipate") as CheckBox).Checked = checkHeader;
            }
        }

        private bool Valid()
        {
            bool ret = false;

            if (dtpSelectionDate.DateInput.Text != "")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Selection Date is required.");
                dtpSelectionDate.Focus();
                ret = false;
            }
            if (cbxVendor.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Vendor is required.");
                cbxVendor.Focus();
                ret = false;
            }
            if (cbxSelectionProcess.SelectedValue != "0")
            {
                ret = true;
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Selection Process is required.");
                cbxSelectionProcess.Focus();
                ret = false;
            }
            if (grdItemInfo.MasterTableView.Items.Count > 0)
            {
                foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
                {
                    if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        ret = true;
                        break;
                    }
                    else
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "At least one Item Selection is required.");
                        dataItem.Focus();
                        ret = false;
                    }
                }
            }
            else
            {
                am.Utility.ShowHTMLMessage(Page, "000", "At least one Item Selection is required.");
                grdItemInfo.Focus();
                ret = false;
            }
            //if (tbxCommitteeMember.Text != "")
            //{
            //    ret = true;
            //}
            //else
            //{
            //    ret = false;
            //}        
           
            return ret;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {

                    SaveSelection();

                }
                else
                {
                    am.Utility.ShowHTMLAlert(Page, "000", "Please provide all the required information");
                }
            }
            catch(Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }

        }
        private void SaveSelection()
        {
            string strFields = "";
            string[] strValues = {};

            string refno = "";
            if (cbxInvitation.SelectedValue != "0")
            {
                refno = cbxInvitation.Text.Trim();
            }
            else
            {
                refno = txtPR.Text.Trim();
                //refno = cbxPR.Text.Trim();
            }

            string podate = DBNull.Value.ToString();
            if (dtpPODate.DateInput.Text != "")
            {
                podate = dtpPODate.SelectedDate.Value.ToString();
            }
            string frameworkNo = " ";
            if (cbxFrameWorkNo.SelectedValue != "")
            {
                frameworkNo = cbxFrameWorkNo.SelectedValue;
            }

            strFields = "[ProcessID],[SelectionDate],[VendorID],[RefNo],[Note],[TO_PO],[TO_PODate],[FWANo]";
            strValues = new string[]{cbxSelectionProcess.SelectedValue , dtpSelectionDate.SelectedDate.Value.ToString() , cbxVendor.SelectedValue , refno , txtSpecialNote.Text.Trim() ,
                     chkPO.Checked.ToString() , podate , frameworkNo};

            if (hdfId.Value == "")
            {              

                am.DataAccess.BatchQuery.Insert("[dbo].[VendorSelection]", strFields, strValues, "[SelectionID]");
                if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Open))
                {
                    string selectionId = am.DataAccess.ActiveIdentity;
                    hdfId.Value = selectionId;
                    SaveSelectionItem(selectionId);
                    SaveSelectionVendorStatus(selectionId);
                    SaveSelectionGroup(selectionId);
                    SaveSelectionAttachment(selectionId);
                    if (am.DataAccess.BatchQuery.Execute(true, ConnectionType.Close))
                    {
                        am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                    }
                }
            }
            else
            {
                string selectionId = hdfId.Value;

                am.DataAccess.BatchQuery.Update("[dbo].[VendorSelection]", strFields, strValues, "[SelectionID]=@SelectionID", new string[]{selectionId});

                DeleteSelectionItem(selectionId);
                SaveSelectionItem(selectionId);

                DeleteSelectionVendorStatus(selectionId);
                SaveSelectionVendorStatus(selectionId);

                DeleteSelectionGroup(selectionId);
                SaveSelectionGroup(selectionId);

                //DeleteSelectionAttachment(selectionId);
                SaveSelectionAttachment(selectionId);

                if (am.DataAccess.BatchQuery.Execute())
                {
                    am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully");
                }
                
            }

        }
        private void SaveSelectionItem(string selectionId)
        {
            string strFields = "";
            string[] strValues = {};

            strFields = "[SelectionID],[ItemID],[ItemDesc],[UnitID],[PackSize],[Qty],[UnitPrice],[PRDetailID]";

            foreach (GridDataItem dataItem in grdItemInfo.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelect") as CheckBox).Checked)
                {
                    strValues = new string[]{selectionId , dataItem["ItemID"].Text , dataItem["Specification"].Text, (dataItem.FindControl("cbxUnit") as RadComboBox).SelectedValue , "0" , (dataItem.FindControl("txtQty") as RadTextBox).Text ,
                         (dataItem.FindControl("txtUnitPrice") as RadTextBox).Text, dataItem["PRItemID"].Text};

                    am.DataAccess.BatchQuery.Insert("[dbo].[SelectionItem]", strFields, strValues);
                }

            }

        }
        private void SaveSelectionVendorStatus(string selectionId)
        {
            string strFields = "";
             string[] strValues = {};

            strFields = "[SelectionID],[VendorID],[IsParticipated],[BidPosition],[Note]";

            foreach (GridDataItem dataItem in grdVendorInfo.MasterTableView.Items)
            {
                strValues = new string[]{selectionId , dataItem["VendorID"].Text , (dataItem.FindControl("chkParticipate") as CheckBox).Checked.ToString() ,
                        (dataItem.FindControl("txtBidPosition") as RadTextBox).Text, (dataItem.FindControl("txtSelectionNote") as RadTextBox).Text};

                am.DataAccess.BatchQuery.Insert("[dbo].[VendorSelectionStatus]", strFields, strValues);
            }

        }
        private void SaveSelectionGroup(string selectionId)
        {
            string strFields = "";
            string[] strValues = { };

            strFields = "[SelectionID],[StaffID]";

            foreach (AutoCompleteBoxEntry entry in tbxCommitteeMember.Entries)
            {
                strValues = new string[]{selectionId , entry.Value};
                am.DataAccess.BatchQuery.Insert("[dbo].[VendorSelectionGroup]", strFields, strValues);

            }

        }

        private void SaveSelectionAttachment(string selectionId)
        {
            string strFields = "";
            string[] strValues = {};

            strFields = "[SelectionID],[FilePath],[AttachNote]";

            if (asyncUploadSelectionFile.UploadedFiles.Count > 0)
            {
                string fileName = "Selection" + selectionId + "_";
                string folderPath = VSAttachmentfolderPath;
                String targetFolder = Server.MapPath(folderPath);
                foreach (UploadedFile file in asyncUploadSelectionFile.UploadedFiles)
                {
                    string filePath = folderPath + "/" + fileName + file.FileName;
                    strValues = new string[] { selectionId, filePath, txtAttachmentNote.Text.Trim() };
                    am.DataAccess.BatchQuery.Insert("[dbo].[VendorSelectionAttach]", strFields, strValues);
                    file.SaveAs(Path.Combine(targetFolder, fileName + file.FileName));
                }

            }

        }

        private void DeleteSelectionItem(string selectionId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[SelectionItem]", "[SelectionID]=@SelectionID", new string[]{selectionId});  
        }
        private void DeleteSelectionVendorStatus(string selectionId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[VendorSelectionStatus]", "[SelectionID]=@SelectionID", new string[]{selectionId}); 
        }
        private void DeleteSelectionGroup(string selectionId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[VendorSelectionGroup]", "[SelectionID]=@SelectionID", new string[]{selectionId});          
        }
        private void DeleteSelectionAttachment(string selectionId)
        {
            am.DataAccess.BatchQuery.Delete("[dbo].[VendorSelectionAttach]", "[SelectionID]=@SelectionID", new string[]{selectionId});          

        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            if ((Session["pritem"] as DataTable) != null && (Session["pritem"] as DataTable).Rows.Count > 0)
            {
                Session["pritem"] = null;
            }
            hdfId.Value = "";
            cbxPR.SelectedValue = "0";
            cbxInvitation.SelectedValue = "0";
            dtpSelectionDate.SelectedDate = null;
            cbxVendor.SelectedValue = "0";
            cbxSelectionProcess.SelectedValue = "0";
            cbxFrameWorkNo.SelectedValue = "0";
            grdItemInfo.DataSource = null;
            grdItemInfo.Rebind();
            grdVendorInfo.DataSource = null;
            grdVendorInfo.Rebind();
            txtSpecialNote.Text = "";
            tbxCommitteeMember.DataSource = null;
            chkPO.Checked = false;
            dtpPODate.SelectedDate = null;
            txtAttachmentNote.Text = "";
            grdAttachment.DataSource = null;
            grdAttachment.Rebind();

        }

        protected void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double qty = 0, price = 0, total = 0;

                RadTextBox txt = (RadTextBox)sender;
                GridDataItem gvr = (GridDataItem)txt.NamingContainer;               

                if (((RadTextBox)gvr.FindControl("txtQty")).Text != "")
                {
                    qty = double.Parse(((RadTextBox)gvr.FindControl("txtQty")).Text);
                }
                if (((RadTextBox)gvr.FindControl("txtUnitPrice")).Text != "")
                {
                    price = double.Parse(((RadTextBox)gvr.FindControl("txtUnitPrice")).Text);
                }
                total = qty * price;
                gvr.Cells[9].Text = total.ToString();
                //((RadTextBox)gvr.FindControl("txtUnitPrice")).Focus();
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "000", ex.Message);
            }
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double qty = 0, price = 0, total = 0;

                RadTextBox txt = (RadTextBox)sender;
                GridDataItem gvr = (GridDataItem)txt.NamingContainer;           

                if (((RadTextBox)gvr.FindControl("txtQty")).Text != "")
                {
                    qty = double.Parse(((RadTextBox)gvr.FindControl("txtQty")).Text);
                }
                if (((RadTextBox)gvr.FindControl("txtUnitPrice")).Text != "")
                {
                    price = double.Parse(((RadTextBox)gvr.FindControl("txtUnitPrice")).Text);
                }
                total = qty * price;
                gvr.Cells[9].Text = total.ToString();
                //((RadTextBox)gvr.FindControl("txtQty")).Focus();
            }
            catch(Exception ex)
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
                //string folderPath = "~/bin/SelectionAttachment";               
                //String targetFolder = Server.MapPath(folderPath);
                //string path = Path.Combine(targetFolder, filepath);
                //System.Diagnostics.Process.Start(path);
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();
                am.Utility.FileDownload(Page, VSDownloadURL, filepath);
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
                string folderPath = VSAttachmentfolderPath;
                string filepath = gdItem.OwnerTableView.DataKeyValues[gdItem.ItemIndex]["FilePath"].ToString();
                am.DataAccess.BatchQuery.Delete("[dbo].[VendorSelectionAttach]", "[FilePath]=@FilePath", new string[]{folderPath + "/" + filepath});
                bool ret = am.DataAccess.BatchQuery.Execute();

                if (ret)
                {
                    String targetFolder = Server.MapPath(folderPath);
                    string path = Path.Combine(targetFolder, filepath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    LoadSelectionAttachment();
                }
            }
            catch (Exception ex)
            {

            }
        }










    }
}