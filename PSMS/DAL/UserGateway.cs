//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;

//namespace PSMS.DAL
//{
//    public class UserGateway
//    {
//        string connectionString = ConfigurationManager.ConnectionStrings.ConnectionStrings["PRConnection"].ConnectionString;
//        public int GetUser(string name, string pass)
//        {
//            //bool IsUserExist = false;
//            int id = 0;
//            string query = "SELECT * FROM Usertable WHERE UserName='" + name + "' and Password='" + pass + "'";
//            SqlConnection connection = new SqlConnection(connectionString);

//            SqlCommand command = new SqlCommand(query, connection);
//            connection.Open();
//            SqlDataReader reader = command.ExecuteReader();
//            while (reader.Read())
//            {

//                id = int.Parse(reader["Id"].ToString());
//            }
//            reader.Close();
//            connection.Close();
//            return id;
//        }
//    }
//}