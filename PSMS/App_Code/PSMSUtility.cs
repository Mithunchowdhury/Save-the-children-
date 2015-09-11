using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS.App_Code
{
    public class PSMSUtility
    {
        //PRDetailID,PRNo,ItemID,StatusID,StatusNote,UserID
        public bool updatePRHistory(AppManager am,RadGrid grd, string rowCheckedCol, int PRStatus, string StatusNote, string StaffCode, string AssignedTo,
            string PrRefNo, string PRNo, string PRItemId, string ItemId,
            string PrRefNoCol, string PRNoCol, string PRItemIdCol, string ItemIdCol)
        {
            //string cnStr = "Data Source=10.12.0.2;Initial Catalog=PRTracker;User Id=sa;Password=bdco;Connection Timeout=0;";
            //SqlConnection sqlCon = new SqlConnection(cnStr);
            //sqlCon.Open();
            //SqlCommand cmd;
            bool result = false;
            string sql = "";
            try
            {
                foreach (GridDataItem dataItem in grd.MasterTableView.Items)
                {
                    bool IsChecked = (dataItem.FindControl(rowCheckedCol.Trim()) as CheckBox).Checked;
                    if (PRStatus == 1) IsChecked = true;

                    if (IsChecked)
                    {
                        if (PrRefNoCol.Trim().Length > 0)
                            PrRefNo = dataItem[PrRefNoCol.Trim()].Text;
                        if (PRNoCol.Trim().Length > 0)
                            PRNo = dataItem[PRNoCol.Trim()].Text;
                        if (PRItemIdCol.Trim().Length > 0)
                            PRItemId = dataItem[PRItemIdCol.Trim()].Text;
                        if (ItemIdCol.Trim().Length > 0)
                            ItemId = dataItem[ItemIdCol.Trim()].Text;

                        //sql = "INSERT INTO PRHistory(PRDetailID,PRNo,ItemID,StatusID,StatusNote,UserID)" +
                        //        " VALUES('" + PRItemId + "','" + PRNo + "','" + ItemId + "','" + PRStatus + "','" + StatusNote + "','" + StaffCode + "')";

                        am.DataAccess.BatchQuery.Insert("PRHistory", "PRDetailID,PRNo,ItemID,StatusID,StatusNote,UserID",
                            new string[] { PRItemId, PRNo, ItemId, PRStatus.ToString(), StatusNote, StaffCode });
                        
                        //cmd = new SqlCommand(sql, sqlCon);
                        //cmd.ExecuteNonQuery();
                        //cmd.Dispose();
                    }
                }



                if (PRStatus == 2)
                {
                    foreach (GridDataItem dataItem in grd.MasterTableView.Items)
                    {
                        bool IsChecked = (dataItem.FindControl(rowCheckedCol.Trim()) as CheckBox).Checked;
                        if (IsChecked)
                        {
                            if (PrRefNoCol.Trim().Length > 0)
                                PrRefNo = dataItem[PrRefNoCol.Trim()].Text;
                            if (PRNoCol.Trim().Length > 0)
                                PRNo = dataItem[PRNoCol.Trim()].Text;

                            //sql = "UPDATE PR SET PRRefNo='" + PrRefNo + "', AssignUserID='" + AssignedTo + "' WHERE PRNo=" + PRNo;
                            am.DataAccess.BatchQuery.Update("PR", "PRRefNo,AssignUserID",
                                new string[] { PrRefNo, AssignedTo }, "PRNo=@PRNo", new string[] { PRNo });
                                                      
                            //cmd = new SqlCommand(sql, sqlCon);
                            //cmd.ExecuteNonQuery();
                            //cmd.Dispose();
                        }
                    }
                }
                //sqlCon.Close();
                //sqlCon.Dispose();
            }
            catch(Exception ex)
            {
                
            }
            result = am.DataAccess.BatchQuery.Execute(false, ConnectionType.OpenGOClose, dbINFO.PRConnectionString);
            return result;
        }


        
    }
}