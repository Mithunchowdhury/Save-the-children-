using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class TaskManager : System.Web.UI.Page
    {
        DataTable dtSearchType = new DataTable("SEARCHTYPE");
        AppManager am = new AppManager();
        HRISGateway hg = new HRISGateway();
        string userName;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    userName = Session["UserName"].ToString();
                    am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                    am.DataAccess.OnShowError += DataAccess_OnShowError;
                }

                dtSearchType.Columns.Add("RefNO", Type.GetType("System.String"));
                dtSearchType.Columns.Add("ReqVen", Type.GetType("System.String"));
                dtSearchType.Rows.Add("PR NO", "REQUESTOR"); //PR
                dtSearchType.Rows.Add("REF NO", "REQUESTOR"); //INVITAITON
                dtSearchType.Rows.Add("PR/REF NO", "VENDOR"); //SELECTION
                dtSearchType.Rows.Add("PO NO", "VENDOR");
                dtSearchType.Rows.Add("GRN NO", "VENDOR");
                dtSearchType.Rows.Add("BILL NO", "VENDOR");

                if (!IsPostBack)
                {

                    dtForm.SelectedDate = System.DateTime.Now.AddDays(-30);
                    dtTo.SelectedDate = System.DateTime.Now;
                    am.Utility.LoadComboBox(cboREFID, "SELECT DISTINCT PRRefNo FROM PR WHERE PRRefNo IS NOT NULL", "PRRefNo", "PRRefNo");

                    DataTable dtGroup = am.DataAccess.RecordSet("SELECT GroupID FROM UserInfo WHERE UserName='" + userName + "'", new string[] { });
                    if (dtGroup != null && dtGroup.Rows.Count > 0)
                    {
                        string groupID = dtGroup.Rows[0][0].ToString();

                        if (groupID == "5")
                        {
                            am.Utility.LoadComboBox(cboOfficer, "SELECT UserInfo.StaffCode, viewStaffInfo.StaffName FROM UserInfo" +
                            " INNER JOIN viewStaffInfo ON viewStaffInfo.StaffCode=UserInfo.StaffCode ORDER BY viewStaffInfo.StaffName", "StaffName", "StaffCode");
                        }
                        else
                        {
                            am.Utility.LoadComboBox(cboOfficer, "SELECT UserInfo.StaffCode, viewStaffInfo.StaffName FROM UserInfo" +
                            " INNER JOIN viewStaffInfo ON viewStaffInfo.StaffCode=UserInfo.StaffCode WHERE UserName='" + userName + "'" +
                            " ORDER BY viewStaffInfo.StaffName", "StaffName", "StaffCode");
                            cboOfficer.Items[0].Remove();
                            cboOfficer.SelectedIndex = 0;
                        }
                        string option = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE).ToString();
                        SwitchOption(option);
                        //rblType_SelectedIndexChanged(null, null);
                    }
                    LoadRblTypeItemCount();
                }
            }
            catch (Exception exp)
            {
                am.Utility.ShowAlert(Page, exp.Message);
            }
        }
        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void LoadRblTypeItemCount()
        {
            try
            {
                //get count from DB
                DataTable dt = null;
                DataRow dr = null;
                string sql = "SELECT (SELECT COUNT(*) FROM   PR) AS PR," +
                             " (SELECT COUNT(*) FROM   Invitation) AS INVITATION," +
                             " (SELECT COUNT(*) FROM   VendorSelection) AS SELECTION," +
                             " (SELECT COUNT(*) FROM   PO) AS PO," +
                             " (SELECT COUNT(*) FROM   GRN) AS GRN," +
                             " (SELECT COUNT(*) FROM   PaymentInfo) AS PAYMENT";
                dt = am.DataAccess.RecordSet(sql, new string[] { });
                if (dt != null && dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];

                    //set count 
                    rblType.Items[0].Text = "PR (" + dr["PR"].ToString() + ")";
                    rblType.Items[1].Text = "INVITATION (" + dr["INVITATION"].ToString() + ")";
                    rblType.Items[2].Text = "SELECTION (" + dr["SELECTION"].ToString() + ")";
                    rblType.Items[3].Text = "PO (" + dr["PO"].ToString() + ")";
                    rblType.Items[4].Text = "GRN/SCN (" + dr["GRN"].ToString() + ")";
                    rblType.Items[5].Text = "PAYMENT (" + dr["PAYMENT"].ToString() + ")";
                }
            }
            catch (Exception exp)
            {
                am.Utility.ShowAlert(Page, exp.Message);
            }
        }

        private void SwitchOption(string option)
        {
            try
            {
                //string option = Request.QueryString["type"].ToString();
                switch (option)
                {
                    case "PR":
                        rblType.SelectedValue = "PR";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                    case "RFQ":
                        rblType.SelectedValue = "RFQ";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                    case "SELECTION":
                        rblType.SelectedValue = "SELECTION";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                    case "PO":
                        rblType.SelectedValue = "PO";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                    case "GRN":
                        rblType.SelectedValue = "GRN";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                    case "PAYMENT":
                        rblType.SelectedValue = "PAYMENT";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                    default:
                        rblType.SelectedValue = "PR";
                        rblType_SelectedIndexChanged(this, null);
                        break;
                }
            }
            catch (Exception exp)
            {
                am.Utility.ShowAlert(Page, exp.Message);
            }
        }
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (rblType.SelectedValue)
                {
                    case "PR":
                        lblRefNO.Text = dtSearchType.Rows[0][0].ToString();
                        lblReqVen.Text = dtSearchType.Rows[0][1].ToString();
                        cboStatus.Items.Clear();
                        cboStatus.Items.Add(new RadComboBoxItem("ASSIGNED", "ASSIGNED"));
                        cboStatus.Items.Add(new RadComboBoxItem("UNASSIGNED", "UNASSIGNED"));
                        cboStatus.Items.Add(new RadComboBoxItem("IN PROGRESS", "IN PROGRESS"));
                        cboStatus.SelectedValue = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_PR).ToString();
                        am.Utility.LoadComboBox(cboRefNO, "SELECT DISTINCT CAST(PRNo AS VARCHAR) PRNo FROM viewPRTracker", "PRNo", "PRNo");
                        hg.getComboList_ID_Name(HRISGateway.empStatus.Active, cboReqVen);
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE, "PR");
                        break;
                    case "RFQ":
                        lblRefNO.Text = dtSearchType.Rows[1][0].ToString();
                        lblReqVen.Text = dtSearchType.Rows[1][1].ToString();
                        cboStatus.Items.Clear();
                        cboStatus.Items.Add(new RadComboBoxItem("PENDING PR", "PENDING PR"));
                        cboStatus.Items.Add(new RadComboBoxItem("VERIFIED", "VERIFIED"));
                        cboStatus.Items.Add(new RadComboBoxItem("NOT VERIFIED", "NOT VERIFIED"));
                        cboStatus.SelectedValue = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_Invitation).ToString();
                        //cboStatus.Items.Add(new RadComboBoxItem("Checked", "0"));
                        //cboStatus.Items.Add(new RadComboBoxItem("Unchecked", "1"));
                        am.Utility.LoadComboBox(cboRefNO, "SELECT InvitationNo FROM Invitation WHERE InvitationNo IS NOT NULL", "InvitationNo", "InvitationNo");
                        hg.getComboList_ID_Name(HRISGateway.empStatus.Active, cboReqVen);
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE, "RFQ");
                        break;
                    case "SELECTION":
                        lblRefNO.Text = dtSearchType.Rows[2][0].ToString();
                        lblReqVen.Text = dtSearchType.Rows[2][1].ToString();
                        cboStatus.Items.Clear();
                        cboStatus.Items.Add(new RadComboBoxItem("PENDING", "PENDING"));
                        cboStatus.Items.Add(new RadComboBoxItem("COMPLETE", "COMPLETE"));
                        cboStatus.SelectedValue = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_Selection).ToString();
                        //cboStatus.Items.Add(new RadComboBoxItem("Checked", "0"));
                        //cboStatus.Items.Add(new RadComboBoxItem("Unchecked", "1"));
                        am.Utility.LoadComboBox(cboRefNO, "SELECT RefNo FROM VendorSelection WHERE RefNo IS NOT NULL", "RefNo", "RefNo");
                        am.Utility.LoadComboBox(cboReqVen, "SELECT VendorID, VendorName FROM VendorInfo ORDER BY VendorName", "VendorName", "VendorID");
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE, "SELECTION");
                        break;
                    case "PO":
                        lblRefNO.Text = dtSearchType.Rows[3][0].ToString();
                        lblReqVen.Text = dtSearchType.Rows[3][1].ToString();
                        cboStatus.Items.Clear();
                        cboStatus.Items.Add(new RadComboBoxItem("PENDING SELECTION", "PENDING SELECTION"));
                        cboStatus.Items.Add(new RadComboBoxItem("VERIFIED", "VERIFIED"));
                        cboStatus.Items.Add(new RadComboBoxItem("NOT VERIFIED", "NOT VERIFIED"));
                        cboStatus.SelectedValue = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_PO).ToString();
                        am.Utility.LoadComboBox(cboRefNO, "SELECT PONO FROM PO WHERE PONO IS NOT NULL", "PONO", "PONO");
                        am.Utility.LoadComboBox(cboReqVen, "SELECT VendorID, VendorName FROM VendorInfo ORDER BY VendorName", "VendorName", "VendorID");
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE, "PO");
                        break;
                    case "GRN":
                        lblRefNO.Text = dtSearchType.Rows[4][0].ToString();
                        lblReqVen.Text = dtSearchType.Rows[4][1].ToString();
                        cboStatus.Items.Clear();
                        cboStatus.Items.Add(new RadComboBoxItem("PENDING PO", "PENDING PO"));
                        cboStatus.Items.Add(new RadComboBoxItem("FULL GRN", "FULL GRN"));
                        cboStatus.Items.Add(new RadComboBoxItem("PARTIAL GRN", "PARTIAL GRN"));
                        cboStatus.SelectedValue = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_GrnSrn).ToString();
                        am.Utility.LoadComboBox(cboRefNO, "SELECT GRNNo FROM GRN WHERE GRNNo IS NOT NULL", "GRNNo", "GRNNo");
                        am.Utility.LoadComboBox(cboReqVen, "SELECT VendorID, VendorName FROM VendorInfo ORDER BY VendorName", "VendorName", "VendorID");
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE, "GRN");
                        break;
                    case "PAYMENT":
                        lblRefNO.Text = dtSearchType.Rows[5][0].ToString();
                        lblReqVen.Text = dtSearchType.Rows[5][1].ToString();
                        cboStatus.Items.Clear();
                        cboStatus.Items.Add(new RadComboBoxItem("PENDING PO", "PENDING PO"));
                        cboStatus.Items.Add(new RadComboBoxItem("PARTIAL PAYMENT", "PARTIAL PAYMENT"));
                        cboStatus.Items.Add(new RadComboBoxItem("FULL PAYMENT", "FULL PAYMENT"));
                        cboStatus.SelectedValue = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_Payment).ToString();
                        am.Utility.LoadComboBox(cboRefNO, "SELECT BillEntryNo FROM PaymentInfo", "BillEntryNo", "BillEntryNo");
                        am.Utility.LoadComboBox(cboReqVen, "SELECT VendorID, VendorName FROM VendorInfo ORDER BY VendorName", "VendorName", "VendorID");
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_CHOICE, "PAYMENT");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }
            cmdSearch_Click(this, e);
        }

        protected void rgTaskManager_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            //if (e.CommandName == "RowClick" && e.Item is GridDataItem)
            //{
            //    //ManageHandler((GridDataItem)e.Item);
            //}
            //    //Response.Redirect("xx.aspx?id=" + e.Item.Cells[2].Text);
            //if (e.CommandName == "Manage" && e.Item is GridDataItem)
            //{
            //    int f = 0;
            //}
            //if (e.CommandName == "View" && e.Item is GridDataItem)
            //{
            //    int g = 0;
            //}

        }
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string sSQL = "";
                string sWhereSQL = "";
                string sWhereSQLVal = "";
                string sStatus = cboStatus.SelectedValue;
                string sRefNO = cboRefNO.SelectedValue == "" ? "0" : cboRefNO.SelectedValue;
                string sPRRefID = cboREFID.SelectedValue;
                string sReqVen = cboReqVen.SelectedValue;
                string sFrom = dtForm.SelectedDate.ToString();
                string sTo = dtTo.SelectedDate.ToString();
                string sOfficer = cboOfficer.SelectedValue == "" ? "0" : cboOfficer.SelectedValue;
                switch (rblType.SelectedValue)
                {
                    case "PR":
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_PR, cboStatus.SelectedValue);

                        sSQL = "EXEC TaskManager_PR '" + cboStatus.SelectedValue + "','" + cboRefNO.SelectedValue + "','" +
                            dtForm.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" + dtTo.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" +
                            cboREFID.SelectedValue.ToString() + "','" + cboReqVen.SelectedValue.ToString() + "','" + sOfficer + "'";
                        break;
                    case "RFQ":
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_Invitation, cboStatus.SelectedValue);
                        sSQL = "EXEC TaskManager_INV '" + cboStatus.SelectedValue + "','" + cboRefNO.SelectedValue + "','" +
                        dtForm.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" + dtTo.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" +
                        cboREFID.SelectedValue.ToString() + "','" + cboReqVen.SelectedValue.ToString() + "','" + sOfficer + "'";
                        break;
                    case "SELECTION":
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_Selection, cboStatus.SelectedValue);
                        sSQL = "EXEC TaskManager_SLN '" + cboStatus.SelectedValue + "','" + cboRefNO.SelectedValue + "','" +
                        dtForm.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" + dtTo.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" +
                        cboREFID.SelectedValue.ToString() + "','" + cboReqVen.SelectedValue.ToString() + "','" + sOfficer + "'";
                        break;
                    case "PO":
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_PO, cboStatus.SelectedValue);
                        sSQL = "EXEC TaskManager_PO '" + cboStatus.SelectedValue + "','" + cboRefNO.SelectedValue + "','" +
                        dtForm.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" + dtTo.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" +
                        cboREFID.SelectedValue.ToString() + "','" + cboReqVen.SelectedValue.ToString() + "','" + sOfficer + "'";
                        break;
                    case "GRN":
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_GrnSrn, cboStatus.SelectedValue);
                        sSQL = "EXEC TaskManager_GRN '" + cboStatus.SelectedValue + "','" + sRefNO + "','" +
                        dtForm.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" + dtTo.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" +
                        cboREFID.SelectedValue.ToString() + "','" + cboReqVen.SelectedValue.ToString() + "','" + sOfficer + "'";
                        break;
                    case "PAYMENT":
                        am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_Payment, cboStatus.SelectedValue);
                        sSQL = "EXEC TaskManager_PMT '" + cboStatus.SelectedValue + "','" + sRefNO + "','" +
                        dtForm.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" + dtTo.SelectedDate.Value.ToString("dd-MMM-yyyy") + "','" +
                        cboREFID.SelectedValue.ToString() + "','" + cboReqVen.SelectedValue.ToString() + "','" + sOfficer + "'";
                        break;
                    default:
                        break;
                }


                rgTaskManager.DataSource = am.DataAccess.RecordSet(sSQL, new string[] { });
                rgTaskManager.DataBind();
            }
            catch (Exception exp)
            {
                am.Utility.ShowAlert(Page, exp.Message);
            }
        }
        protected void cmdExport_Click(object sender, EventArgs e)
        {

        }
        protected void cmdManage_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridDataItem item = (GridDataItem)btn.NamingContainer;
            ManageHandler(item);
        }

        private void ManageHandler(GridDataItem item)
        {
            string value = "";
            try
            {
                TableCell cell = (TableCell)item["REFNO"];
                if (cell.Text != "&nbsp;")
                    value = cell.Text;

                switch (rblType.SelectedValue)
                {
                    case "PR":
                        if (cboStatus.Text == "ASSIGNED")
                        {
                            Response.Redirect("PRAssign.aspx?prno=" + value + "&type=apr");
                        }
                        else if (cboStatus.Text == "UNASSIGNED")
                        {
                            Response.Redirect("PRAssign.aspx?prno=" + value + "&type=upr");
                        }
                        else
                        {
                            Response.Redirect("PRAssign.aspx?prno=" + value + "&type=apr");
                        }
                        break;
                    case "RFQ":
                        if (cboStatus.Text == "PENDING PR")
                        {
                            Response.Redirect("Invitation.aspx?invId=" + value + "&type=pr");
                        }
                        else
                        {
                            Response.Redirect("Invitation.aspx?invId=" + value + "&type=rfq");
                        }
                        break;
                    case "SELECTION":
                        if (cboStatus.Text == "PENDING")
                        {
                            string type = "PR";
                            TableCell cell1 = (TableCell)item["COL1"];
                            if (cell1.Text.Contains("RFQ"))
                                type = "RFQ";
                            else if (cell1.Text.Contains("RFP"))
                                type = "RFP";
                            else if (cell1.Text.Contains("IFT"))
                                type = "IFT";

                            Response.Redirect("VendorSelection.aspx?selectionId=" + value + "&type=" + type + "");
                        }
                        else
                        {
                            Response.Redirect("VendorSelection.aspx?selectionId=" + value + "&type=selected");
                        }
                        break;
                    case "PO":
                        if (cboStatus.Text == "PENDING SELECTION")
                        {
                            Response.Redirect("PurchaseOrder.aspx?poId=" + value + "&type=sel");
                        }
                        else
                        {
                            Response.Redirect("PurchaseOrder.aspx?poId=" + value + "&type=po");
                        }
                        break;
                    case "GRN":
                        if (cboStatus.Text == "PENDING PO")
                        {
                            Response.Redirect("GRNSCN.aspx?grnscnId=" + value + "&type=po");
                        }
                        else
                        {
                            Response.Redirect("GRNSCN.aspx?grnscnId=" + value + "&type=grn");
                        }
                        break;
                    case "PAYMENT":
                        if (cboStatus.Text == "PENDING PO")
                        {
                            Response.Redirect("Payment.aspx?paymentId=" + value + "&type=po");
                        }
                        else
                        {
                            Response.Redirect("Payment.aspx?paymentId=" + value + "&type=pmt");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, "0000", ex.Message);
            }
        }
        protected void cmdView_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridDataItem item = (GridDataItem)btn.NamingContainer;
            TableCell cell = (TableCell)item["REFNO"];
            string value = cell.Text;
            switch (rblType.SelectedValue)
            {
                case "PR":
                    //Response.Redirect("Reports/PR.aspx?prNo=" + value);
                   am.Report.PrintReport(Page, "PRNew.rpt", "PR" + value + "_", null, null, new string[] { "@PR" }, new string[] { value });
                    
                    break;
                case "RFQ":
                    if (cboStatus.Text != "PENDING PR")
                    {
                        Response.Redirect("Reports/Invitation.aspx?invId=" + value + "&&type=" + "RFQ");
                    }
                    else
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "Nothing to view for Pending PR.");
                    }
                    break;
                case "SELECTION":
                    //NO Preview
                    am.Utility.ShowHTMLMessage(Page, "000", "Nothing to view for Vendor Selection.");
                    break;
                case "PO":
                    if (cboStatus.Text != "PENDING SELECTION")
                    {
                        Response.Redirect("Reports/PO.aspx?poId=" + value);
                    }
                    else
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "Nothing to view for Pending Selection.");
                    }
                    break;
                case "GRN":
                    if (cboStatus.Text != "PENDING PO")
                    {
                        Response.Redirect("Reports/GRN.aspx?grnId=" + value);
                    }
                    else
                    {
                        am.Utility.ShowHTMLMessage(Page, "000", "Nothing to view for Pending Purchase Order.");
                    }
                    break;
                case "PAYMENT":
                    //NO Preview
                    am.Utility.ShowHTMLMessage(Page, "000", "Nothing to view for Payment.");
                    break;
                default:
                    break;
            }
        }
        
        protected void cmdEditExten_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridDataItem item = (GridDataItem)btn.NamingContainer;
            TableCell cell = (TableCell)item["REFNO"];
            string value = cell.Text;
        }
        protected void Delete_Click(object sender, EventArgs e)
        {

        }

        protected void rgTaskManager_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            cmdSearch_Click(null, null);
        }

        protected void rgTaskManager_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            cmdSearch_Click(null, null);
        }

        

    }
}