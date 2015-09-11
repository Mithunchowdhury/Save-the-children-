using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class FWT : System.Web.UI.Page
    {
        AppManager am = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am = new AppManager();
                am.DataAccess.OnShowError += DataAccess_OnShowError;
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), "/Donor.aspx");

                //am.DataAccess.OnShowError += DataAccess_OnShowError;
                //am.DataAccess.SetUISecurity(Session["UserName"].ToString(), "/Donor.aspx");
            }
            
        }
        public AppManager Manager
        {
            get
            {
                if (this.ViewState["AppamObj"] == null)
                {
                    AppManager obj = new AppManager();
                    ViewState["AppamObj"] = obj;
                    return obj;
                }
                else return (AppManager)this.ViewState["AppamObj"];
            }
            set { this.ViewState["AppamObj"] = value; }
        }


        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //Insert 
            //Success  
            //am.DataAccess.BatchQuery.Insert("test1", "C1", "C2", "test");
            am.DataAccess.BatchQuery.Insert("test", "C2,C3,C4,C5,C6", new string[] { "test,1,4", "1", "2", "07-Jun-2015", "234.44" });
            //am.DataAccess.BatchQuery.Insert("test", "C2,C3,C4,C5,C6", "test,1,3,08-Jun-2015, 334.44");
            //am.DataAccess.BatchQuery.Insert("test", "C2,C3,C4,C5,C6", "test,1,4,09-Jun-2015, 455.44");
            am.DataAccess.BatchQuery.Execute();
            string currentId = am.DataAccess.ActiveIdentity;
            //am.DataAccess.BatchQuery.Insert("test", "C2,C3,C4,C5,C6", "test,1," + currentId + ",06-Jun-2015 13:15:35.000, 123.50");
            //am.DataAccess.BatchQuery.Execute();
            //Failed
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //Update 
            //Success  
            am.DataAccess.BatchQuery.Update("test1", "C2", new string[] { "test1" }, "C1=@C1", new string[] { "6" });
            //am.DataAccess.BatchQuery.Update("test", "C2,C3,C4", "test update,1,2", "C1=@C1", "14");
            //am.DataAccess.BatchQuery.Update("test", "C2,C3,C4", "test update,1,3", "C1=@C1", "15");
            //am.DataAccess.BatchQuery.Update("test", "C2,C3,C4", "test update,1,4", "C1=@C1", "16");
            am.DataAccess.BatchQuery.Execute();
            string currentId = am.DataAccess.ActiveIdentity;
            //Failed

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            //Delete 
            //Success  
            am.DataAccess.BatchQuery.Delete("test1", "C1=@C1", new string[] { "1" });
            //am.DataAccess.BatchQuery.Delete("test", "C3=@C3 and C1=@C1", "1,15");          
            am.DataAccess.BatchQuery.Execute();
            string currentId = am.DataAccess.ActiveIdentity;
            //Failed
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            ////Read 
            ////Record Set Test - 2015-Aug-2  
            DataTable dt = am.DataAccess.RecordSet(
                "select * from PO where PO.POID in (select a.POID from PO a where a.CheckedStatus=@CheckedStatus) AND PO.VendorID=@VendorID AND PO.Address=@Address",
                new string[3] { "2", "1362", "Dhaka, Bangladesh" });
            if (dt != null && dt.Rows.Count > 0)
            {
                string s = dt.Rows[0]["C2"].ToString();
            }


            //Scalar Test - 2015-Aug-5
            String result = am.DataAccess.GetScalarValue(
                "select * from PO where PO.POID in (select a.POID from PO a where a.CheckedStatus=@CheckedStatus) AND PO.VendorID=@VendorID AND PO.Address=@Address",
                new string[3] { "2", "1362", "Dhaka, Bangladesh" });
            if (string.IsNullOrEmpty(result))
            {

            }
        }

        protected void Button22_Click(object sender, EventArgs e)
        {
            am.Utility.ShowHTMLMessage(this.Page, "WertgwER", "test");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            am.Utility.ShowHTMLAlert(this.Page, "WertgwER", "test");
            Thread.Sleep(3000);
        }

        protected void grd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (am != null)
            {
                DataTable dt = am.DataAccess.RecordSet("select * from Donor", new string[] { });
                if (dt != null && dt.Rows.Count > 0)
                {
                    grd.DataSource = dt;
                }
                else
                {
                    grd.DataSource = null;
                }
            }
        }
        protected void grd_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gdItem = null;
            switch (e.CommandName)
            {
                case "Edit":
                    if (am != null && !am.DataAccess.UIPermission.IsExcutionAllowed(2))
                    {
                        am.Utility.ShowHTMLMessage(this.Page, ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.PermissionError), "You do not have edit permission");
                        e.Canceled = true;
                        return;
                    }
                    gdItem = (GridDataItem)e.Item;
                   // Edit(gdItem);
                    e.Canceled = true;
                    break;
                case "Delete":

                    gdItem = (GridDataItem)e.Item;
                    am.DataAccess.BatchQuery.Delete("Donor", "DonorID=@DonorID", new string[] { "4" });
                    am.DataAccess.BatchQuery.Execute();
                    e.Canceled = true;
                    break;
            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            string value = am.Utility.GetCookeRecord(Page, PSMSCookie.Preferred_Status_PR).ToString();
            //if (string.IsNullOrEmpty(value))
            //{
            am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_PR, "ASSIGNED");
            //}
            //else
            //{
            //    am.Utility.SetCookeRecord(Page, PSMSCookie.Preferred_Status_PR, value + "1");
            //}
        }

        protected void Button23_Click(object sender, EventArgs e)
        {
            am.Utility.FileDownload(Page, "http://softdev/scms/bin/POAttachment/", "PO649_.pdf");
        }

    }
}