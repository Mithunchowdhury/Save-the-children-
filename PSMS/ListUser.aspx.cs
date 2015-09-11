using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS
{
    public partial class ListUser : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDataToGridView();
        }



        public void GetDataToGridView()
        {
            List<Employee> users = GetAllData();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Userid", typeof(Int32));
            dataTable.Columns.Add("User Name", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Full Name", typeof(string));
            dataTable.Columns.Add("Staff Code", typeof(string));
            dataTable.Columns.Add("Group ID", typeof(string));
            dataTable.Columns.Add("Locationid", typeof(string));
            //dataTable.Columns.Add("Active", typeof(string));
            dataTable.Columns.Add ("Active", typeof(bool));
               
            
            foreach (Employee user in users)
            {
                dataTable.Rows.Add(user.Userid, user.Username, user.Password, user.Fullname, user.Stafcode, user.GroupId,
                    user.Locationid, user.Activeid == true ? 1 : 0);
            }
            RadGrid1.DataSource = dataTable;
            RadGrid1.DataBind();
        }
        public List<Employee> GetAllData()
        {
            string query = String.Format("SELECT UserInfo.UserID, UserInfo.UserName,UserInfo.Password,UserInfo.FullName, UserInfo.StaffCode, UserInfo.GroupID, Location.LocationName,UserInfo.Active FROM UserInfo INNER JOIN Location ON  UserInfo.LocationID=Location.LocationID ");
            List<Employee> users = new List<Employee>();


            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        Employee user = new Employee();
                        user.Userid = Convert.ToInt32(rdr[0]);
                        user.Username = rdr[1].ToString();
                        user.Password = rdr[2].ToString();
                        user.Fullname = rdr[3].ToString();
                        user.Stafcode = rdr[4].ToString();
                        user.GroupId = rdr[5].ToString();
                        user.Locationid = rdr[6].ToString();
                        user.Activeid =Convert.ToBoolean( rdr[7]);
                        users.Add(user);

                    }
                    connection.Close();
                }
            }
            return users;
        }
        //void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        //{
        //    am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        //}
        //protected void grd_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        //{
        //    DataTable dataTable = am.DataAccess.RecordSet("SELECT *,iif(UserInfo.Active = 1, \'Yes\', \'No\') as ActiveStr from UserInfo", new string[] { });
        //    if (dataTable != null && dataTable.Rows.Count > 0)
        //    {
        //        RadGrid1.DataSource = dataTable;
        //    }
        //    else
        //    {
        //        RadGrid1.DataSource = null;
        //    }
        //}
        ////public bool ActiveId
        ////{
        ////    if(Active)
        //}
        //private void LoadComboItems()
        //{
        //    am.Utility.LoadComboBox(cbxGroups, "SELECT GroupID, GroupName from GroupInfo where Active = 1", "GroupName", "GroupID");

        //    am.Utility.LoadComboBox(cbxLocations, "SELECT LocationID, LocationName from Location where Active = 1", "LocationName", "LocationID");
        //}

        //protected void grd_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        //{
        //    GridDataItem gdItem = null;
        //    switch (e.CommandName)
        //    {
        //        case "Edit":

        //            gdItem = (GridDataItem)e.Item;
        //            Edit(gdItem);
        //            e.Canceled = true;
        //            break;
        //        case "Delete":

        //            gdItem = (GridDataItem)e.Item;
        //            Delete(gdItem);
        //            e.Canceled = true;
        //            break;
        //        case RadGrid.RebindGridCommandName:
        //            RadGrid1.Rebind();
        //            RadGrid1.CurrentPageIndex = 0;
        //            break;
        //    }
        //}
    }
}