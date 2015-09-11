//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using PSMS.BLL;

//namespace PSMS
//{
    
//    public partial class index : System.Web.UI.Page
//    {
//        UserManager amanager = new UserManager();
        
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }

//        protected void saveButton_Click(object sender, EventArgs e)
//        {
//            string name = nameTextBox.Text;
//            string password = passwordTextBox.Text;
//            int id = amanager.GetUser(name, password);
//            Session["id"] = id;
//                if(id >0){
//                    Response.Redirect("Home.aspx");
//                }
//                else
//                {
//                    msgLabel.Text = "User name and password  is invalid";
//                }


//        }
//    }
//}