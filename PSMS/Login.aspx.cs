using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;

namespace PSMS
{
    public partial class Login : System.Web.UI.Page
    {
        AppManager am = null;
        //string UserId;
        //int password;
        protected void Page_Load(object sender, EventArgs e)
        {           
            am = new AppManager();
            am.DataAccess.OnShowError += DataAccess_OnShowError;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Session["UserID"] = "1";
            //Session["UserName"] = "ABC";
            //Response.Redirect("Default.aspx");
            try
            {
              
                if (am != null)
                {
                    DataTable dt = am.DataAccess.RecordSet("select UserID,UserName,StaffCode,GroupID,LocationID from UserInfo where UserName=@UserName And Password=@password and Active=1", new string[] { tbxLgnUserName.Text.Trim(), tbxLgnPassword.Text.Trim() });
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //if (am.AppLogin.IsValidLogin_SCIAD(this.Page, tbxLgnUserName.Text.Trim(), tbxLgnPassword.Text.Trim()))
                        //{
                        Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                        Session["UserName"] = dt.Rows[0]["UserName"].ToString();
                        Session["StaffCode"] = dt.Rows[0]["StaffCode"].ToString();
                        Session["GroupID"] = dt.Rows[0]["GroupID"].ToString();
                        Session["LocationID"] = dt.Rows[0]["LocationID"].ToString();
                        //Session["UserID"] = "1";
                        //Session["UserName"] = "ABC";
                        //Session["StaffCode"] = "001";
                        //Session["GroupID"] = "1";
                        //Session["LocationID"] = "1";
                        Response.Redirect("Default.aspx");
                        //}
                    }
                    else
                        am.Utility.ShowHTMLMessage(this.Page, ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.LoginError),
                            "Login Failure. You are not authorized user!!");

            }
                //if (am != null)
                //{

                //    if (tbxLgnUserName.Text = "admin" && tbxLgnPassword.Text = "123")
                //    {

                //        //Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                //        //Session["Password"] = d;

                //        Response.Redirect("Default.aspx");
                //    }

                //    else
                //    {
                //        am.Utility.ShowHTMLMessage(this.Page, ErrorNumber.SetType(ErrorNumber.PermissionDenied, errorType.LoginError),
                //            "Login Failure. You are not authorized user!!");
                //    }

                //}
            }

            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(Page, ex.HResult.ToString(), ex.Message);
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            if (am != null)
                am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }
    }
}