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
    public partial class Default : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            
            //RadTreeNode rtnPR = new RadTreeNode("PR - [Unassigned (12)]");
            //rtnPR.CssClass = "nodeStyle1";
            //rtnPR.Value = "PR";
            //rtvPendingTask.Nodes.Add(rtnPR);
            //RadTreeNode rtnInvitation = new RadTreeNode("Invitation - [Check (05)]");
            //rtnInvitation.CssClass = "nodeStyle1";
            //rtnInvitation.Value = "Inv";
            //rtvPendingTask.Nodes.Add(rtnInvitation);
            //RadTreeNode rtnSelection = new RadTreeNode("Selection - [Check (08)]");
            //rtnSelection.CssClass = "nodeStyle2";
            //rtnSelection.Value = "Sel";
            //rtvPendingTask.Nodes.Add(rtnSelection);
            //RadTreeNode rtnPO = new RadTreeNode("PO - [Check (11), Approve (11)]");
            //rtnPO.CssClass = "nodeStyle2";
            //rtnPO.Value = "PO";
            //rtvPendingTask.Nodes.Add(rtnPO);
            //RadTreeNode rtnGRN = new RadTreeNode("GRN/SRN - [Check (11)]");
            //rtnGRN.CssClass = "nodeStyle3";
            //rtnGRN.Value = "GS";
            //rtvPendingTask.Nodes.Add(rtnGRN);
            //RadTreeNode rtnBill = new RadTreeNode("Bill Payment - [Check (10)]");
            //rtnBill.CssClass = "nodeStyle3";
            //rtnBill.Value = "Bill";
            //rtvPendingTask.Nodes.Add(rtnBill);

            //DataTable dt = new DataTable("TMP");
            //dt.Columns.Add("Month", Type.GetType("System.String"));
            //dt.Columns.Add("PR", Type.GetType("System.Int64"));
            //dt.Columns.Add("FRAMEWORK", Type.GetType("System.Int64"));
            //dt.Columns.Add("DIRECT", Type.GetType("System.Int64"));

            //dt.Rows.Add("JAN", 30, 133, 100);
            //dt.Rows.Add("FEB", 20, 233, 200);
            //dt.Rows.Add("MAR", 50, 322, 16);
            //dt.Rows.Add("APR", 60, 42, 300);
            //dt.Rows.Add("MAY", 130, 52, 400);
            //dt.Rows.Add("JUN", 230, 62, 15);

            //ChartHelper ch = new ChartHelper("ProcessType", 330, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom);
            //ch.CreateChart(HtmlChartHolder, ChartHelper.ChartType.AreaChart, dt, new string[] { "Red", "Blue", "Green" });

            //DailyActivities();
            //ProcessTypePieChart();

            //DataTable dt11 = new DataTable("TMP");
            //dt11.Columns.Add("Type", Type.GetType("System.String"));
            //dt11.Columns.Add("Count", Type.GetType("System.Int64"));

            //dt11.Rows.Add("PR", 1330);
            //dt11.Rows.Add("Freamework", 1120);
            //dt11.Rows.Add("Direct", 1250);

            ////ChartHelper ch11 = new ChartHelper("ProcessType11", 300, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Right);
            ////ch11.CreateChart(HtmlChartHolderPieTopLeft, ChartHelper.ChartType.PieChart, dt11, null);


            //DataTable dt2 = new DataTable("TMP");
            //dt2.Columns.Add("Month", Type.GetType("System.String"));
            //dt2.Columns.Add("PR", Type.GetType("System.Int64"));
            //dt2.Columns.Add("FRAMEWORK", Type.GetType("System.Int64"));
            //dt2.Columns.Add("DIRECT", Type.GetType("System.Int64"));

            //dt2.Rows.Add("JAN", 30, 133, 100);
            //dt2.Rows.Add("FEB", 20, 233, 200);
            //dt2.Rows.Add("MAR", 50, 322, 16);

            //ChartHelper ch2 = new ChartHelper("ProcessType2", 310, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom);
            //ch2.CreateChart(HtmlChartHolderBar, ChartHelper.ChartType.BarChartVartical, dt2, null);

           

            //ChartHelper ch211 = new ChartHelper("ProcessType211", 310, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom);
            //ch211.CreateChart(HtmlChartHolderBarHorBotomRight, ChartHelper.ChartType.BarChartHorizontal, dt2, null);

            //ChartHelper ch3 = new ChartHelper("ProcessType3", 310, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom);
            //ch3.CreateChart(HtmlChartHolderLine, ChartHelper.ChartType.LineChart, dt2, null);

            //rgPRPOQTR.DataSource = dt;
            //rgPRPOQTR.DataBind();

            //RadGrid1.DataSource = dt;
            //RadGrid1.DataBind();
            //RadGrid2.DataSource = dt;
            //RadGrid2.DataBind();
            //RadGrid3.DataSource = dt;
            //RadGrid3.DataBind();
            //RadGrid4.DataSource = dt;
            //RadGrid4.DataBind();

        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void DailyActivities()
        {
            lblDailyActivities.Text = "DAILY ACTIVITIES (" + DateTime.Now.ToString("dd-MMM-yyyy") + ")";
            DataTable dt = new DataTable();
            string qry = "EXEC GetDailyActivities";
            dt = am.DataAccess.RecordSet(qry, new string[] { });           
            ChartHelper ch = new ChartHelper("ProcessType21", 310, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom);
            ch.CreateChart(HtmlChartHolderBarHorTopLeft, ChartHelper.ChartType.BarChartHorizontalDA, dt, new string[] { "Violet", "Indigo", "Blue", "Green", "Yellow", "Orange", "Red" });           
        }
        private void ProcessTypePieChart()
        {
            lblProcessTypePerYear.Text = "SELECTION PROCESS (" + DateTime.Now.Year + ")";
            DataTable dt = new DataTable();
            string qry = "EXEC GetProcessTypePieChart";
            dt = am.DataAccess.RecordSet(qry, new string[] { });
            ChartHelper ch = new ChartHelper("ProcessType1", 300, 200, Telerik.Web.UI.HtmlChart.ChartLegendPosition.Right);
            ch.CreateChart(HtmlChartHolderPie, ChartHelper.ChartType.PieChart, dt, null);
        }

        protected void rtvPendingTask_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                switch (e.Node.Value)
                {
                    case "PR":
                        Response.Redirect(string.Format("TaskManager.aspx?type={0}", Server.UrlEncode("PR")));
                        break;
                    case "Inv":
                        Response.Redirect(string.Format("TaskManager.aspx?type={0}", Server.UrlEncode("Inv")));
                        break;
                    case "Sel":
                        Response.Redirect(string.Format("TaskManager.aspx?type={0}", Server.UrlEncode("Sel")));
                        break;
                    case "PO":
                        Response.Redirect(string.Format("TaskManager.aspx?type={0}", Server.UrlEncode("PO")));
                        break;
                    case "GS":
                        Response.Redirect(string.Format("TaskManager.aspx?type={0}", Server.UrlEncode("GS")));
                        break;
                    case "Bill":
                        Response.Redirect(string.Format("TaskManager.aspx?type={0}", Server.UrlEncode("Bill")));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}