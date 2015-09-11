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
    public partial class VendorByCategory : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        DataTable dtSource = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            ReportDocument rptDoc = new ReportDocument();
            dsPSMS ds = new dsPSMS(); 
            DataTable dt = new DataTable();

            dt.TableName = "Vendors";
            dt = getAllItems();

            ds.Tables["dtVendorInfo"].Merge(dt);

            rptDoc.Load(Server.MapPath("../Reports/crVendorByCategory.rpt"));

            rptDoc.SetDataSource(ds);
            CrystalReportViewer1.ReportSource = rptDoc;

        }

        public DataTable getAllItems()
        {
            
            string query = "";
            query += " select ic.ItemCategoryName, vi.VendorCode, vi.VendorName, vi.VendorType, ";
            query += " vi.Address, vi.PostCode, vi.City, vi.Country, vi.Email, vi.Fax, vi.WebSite ";
            query += " from VendorInfo as vi left join VendorCategory as vc ";
            query += " on vi.VendorID = vc.VendorID ";
            query += " left join ItemCategory ic ";
            query += " on vc.ItemCategoryID = ic.ItemCategoryID ";
            query += " where vi.VendorID in( ";
            query += " select distinct VendorID from VendorCategory ";
            query += " ) ";

            dtSource = am.DataAccess.RecordSet(query, new string[] { }); 
            return dtSource;
        }
    }
}