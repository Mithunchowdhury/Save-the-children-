using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS.Reports
{
    public partial class ItemInfo : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        DataTable dtSource = null;
        protected void Page_Load(object sender, EventArgs e)
        {            
            ReportDocument rptDoc = new ReportDocument();
            dsPSMS ds = new dsPSMS();
            DataTable dt = new DataTable();

            
            dt.TableName = "Items";
            dt = getAllItems();

            ds.Tables["dtItemInfo"].Merge(dt);
                        
            rptDoc.Load(Server.MapPath("../Reports/crItemInfo.rpt"));
                        
            rptDoc.SetDataSource(ds);
            CrystalReportViewer1.ReportSource = rptDoc;

        }
        public DataTable getAllItems()
        {            
            string treeQuery = "";
            treeQuery += " select ItemCategory.ItemCategoryName, ItemSubCategory.SubCategoryName, ItemInfo.ItemName, ";
            treeQuery += " ItemInfo.ItemAlias, ItemInfo.ItemType, ItemInfo.Description ";
            treeQuery += " from ItemInfo left join ItemSubCategory ";
            treeQuery += " on ItemInfo.SubCategoryID = ItemSubCategory.SubCategoryID ";
            treeQuery += " left join ItemCategory ";
            treeQuery += " on ItemSubCategory.CategoryID = ItemCategory.ItemCategoryID ";
            treeQuery += " where ItemInfo.SubCategoryID in ";
            treeQuery += " ( ";
            treeQuery += " select distinct SubCategoryID from ItemSubCategory ";
            treeQuery += " ) ";

            dtSource = am.DataAccess.RecordSet(treeQuery, new string[] { });           
            return dtSource;
        }
    }
}