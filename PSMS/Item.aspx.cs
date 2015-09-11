using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class Item : System.Web.UI.Page
    {
        //sqlLink sLink = new sqlLink();
        //SqlCommand sqlCmd;
        //DataTable dt = null;
        AppManager am = new AppManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }

            if (!Page.IsPostBack)
            {
                //lblMsg.Text = "";
                LoadComboItems();
                BindToDataSet(RadTreeView1, true);
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void LoadComboItems()
        {
            //lblMsg.Text = "";
            //load combo box items
            //try
            //{
            //string sqlStr = "SELECT SubCategoryID, SubCategoryName from ItemSubCategory";
            //sLink.cbxLoad_Table(sqlStr, cbxSubCat, "SubCategoryID", "SubCategoryName");
            am.Utility.LoadComboBox(cbxSubCat, "SELECT SubCategoryID, SubCategoryName FROM ItemSubCategory", "SubCategoryName", "SubCategoryID");
            //}
            //catch (Exception ex)
            //{
            //    //lblMsg.Text = "Error: " + ex.Message;
            //}
        }

        private void BindToDataSet(RadTreeView treeView, bool clearError)
        {
            if (clearError)
            {
                //lblMsg.Text = "";
            }
            try
            {
                string treeQuery = " select Concat(\'C-\',ic.ItemCategoryID) as Id, NULL as ParentId, ic.ItemCategoryName as Name ";
                treeQuery += " from ItemCategory as ic ";
                treeQuery += " union ";
                treeQuery += " select Concat(\'S-\',isc.SubCategoryID) as Id, Concat(\'C-\',isc.CategoryID) as ParentId, isc.SubCategoryName as Name ";
                treeQuery += " from ItemSubCategory as isc ";
                treeQuery += " where isc.CategoryID is not null and isc.CategoryID in (select ItemCategoryID from ItemCategory) ";
                treeQuery += " union ";
                treeQuery += " select Concat(\'I-\',ii.ItemID) as Id, Concat(\'S-\',ii.SubCategoryID) as ParentId, ii.ItemName as Name ";
                treeQuery += " from ItemInfo as ii ";
                treeQuery += " where ii.SubCategoryID in ";
                treeQuery += " ( ";
                treeQuery += " select sc.SubCategoryID from ItemSubCategory sc left join ItemCategory c ";
                treeQuery += " on sc.CategoryID = c.ItemCategoryID ";
                treeQuery += " ) ";

                //SqlDataAdapter adapter = new SqlDataAdapter(treeQuery,
                //        "Data Source=10.12.1.2;Initial Catalog=PSMS;User Id=psms;Password=psms;Connection Timeout=0;");
                //DataTable links = new DataTable();
                //adapter.Fill(links);

                //sqlCmd = new SqlCommand(treeQuery);
                //DataTable itemTree = sLink.GetData(sqlCmd);
                DataTable dt = am.DataAccess.RecordSet(treeQuery, new string[] { });
                if (dt != null && dt.Rows.Count > 0)
                {
                    treeView.DataTextField = "Name";
                    treeView.DataValueField = "Id";
                    treeView.DataFieldID = "Id";
                    treeView.DataFieldParentID = "ParentId";

                    treeView.DataSource = dt;
                    treeView.DataBind();
                }
                ResetRecord(clearError, false, false);
            }
            catch (Exception ex)
            {
                //lblMsg.Text = "Error: " + ex.Message;
            }
            finally
            {
                //if (sqlCmd != null)
                //    sqlCmd.Dispose();
            }
        }

        protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            RadTreeNodeChangeHandler(e.Node);
        }

        private void RadTreeNodeChangeHandler(RadTreeNode tn)
        {
            //lblMsg.Text = "";
            try
            {
                string id = tn.Value;

                if (id.StartsWith("I"))
                {
                    //Starts with I means this node represents Item

                    //category Id
                    string catid = tn.ParentNode.ParentNode.Value;
                    hdfCatId.Value = catid.Split('-')[1];
                    //sub category Id
                    string subcatid = tn.ParentNode.Value;
                    subcatid = subcatid.Split('-')[1];
                    hdfSubCatId.Value = subcatid;
                    cbxSubCat.SelectedValue = subcatid;

                    //Item Id
                    string itemId = id.Split('-')[1];
                    hdfId.Value = itemId;
                    ShowRecord(itemId);
                }
                else if (id.StartsWith("S"))
                {
                    //Starts with S means this node represents Sub Category
                    //category Id
                    string catid = tn.ParentNode.Value;
                    hdfCatId.Value = catid.Split('-')[1];
                    //sub category Id            
                    string subcatid = id.Split('-')[1];
                    hdfSubCatId.Value = subcatid;
                    cbxSubCat.SelectedValue = subcatid;
                    ResetRecord(true, true, true);
                }
                else if (id.StartsWith("C"))
                {
                    //Starts with C means this node represents Category

                    //category Id                                      
                    hdfCatId.Value = id.Split('-')[1];
                    ResetRecord(true, true, false);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ShowRecord(string itemId)
        {
            ResetRecord(true, true, true);
            //string str = "SELECT * FROM ItemInfo WHERE ItemID=@pItemID";
            //sqlCmd = new SqlCommand(str);
            try
            {
                //lblMsg.Text = "";
                //sqlCmd.Parameters.AddWithValue("@pItemID", itemId);
                //dt = new DataTable();
                //dt = sLink.GetData(sqlCmd);
                DataTable dt = am.DataAccess.RecordSet("SELECT * FROM ItemInfo WHERE ItemID=@pItemID", new string[] { itemId });
                if (dt != null && dt.Rows.Count > 0)
                {
                    hdfId.Value = itemId;
                    tbxName.Text = dt.Rows[0]["ItemName"].ToString();
                    tbxAlias.Text = dt.Rows[0]["ItemAlias"].ToString();
                    cbxItemType.SelectedValue = dt.Rows[0]["ItemType"].ToString();
                    tbxDescription.Text = dt.Rows[0]["Description"].ToString();
                    chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Active"]);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //if (sqlCmd != null)
                //    sqlCmd.Dispose();
            }
        }

        private void ResetRecord(bool clearError, bool skipCatReset, bool skipSubCatReset)
        {
            if (clearError)
            {
                //lblMsg.Text = "";
            }

            tbxName.Text = "";
            tbxAlias.Text = "";
            cbxItemType.SelectedValue = "";
            tbxDescription.Text = "";
            chkActive.Checked = true;

            hdfId.Value = "0";
            if (!skipCatReset)
            {
                hdfCatId.Value = "0";
            }
            if (!skipSubCatReset)
            {
                hdfSubCatId.Value = "0";
                cbxSubCat.SelectedValue = "0";
            }
            if (!skipCatReset && !skipSubCatReset)
            {
                if (RadTreeView1.SelectedNode != null)
                    RadTreeView1.SelectedNode.Selected = false;
            }            
        }

        private void SaveRecord()
        {
            if (!Valid())
            {
                return;
            }
            //if(IsDuplicate(tbxName.Text))
            //{
            //    //lblMsg.Text = "Already exists.";
            //    return;
            //}
            //lblMsg.Text = "";
            if (hdfCatId.Value == "0" || hdfSubCatId.Value == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select a Subcategory.");
                return;
            }

            //try
            //{
            string itemName = tbxName.Text.Trim();
            string itemAlias = tbxAlias.Text.Trim();
            string itemType = cbxItemType.SelectedValue;
            string description = tbxDescription.Text.Trim();
            string active = chkActive.Checked == true ? "1" : "0";

            string sqlStr = "";
            
            if (hdfId.Value != "0")
            {               
                am.DataAccess.BatchQuery.Update("ItemInfo", "SubCategoryID,ItemName,ItemAlias,ItemType,Description,Active",
                new string[] { hdfSubCatId.Value , itemName , itemAlias , itemType , description , active },
                "WHERE ItemID=@ItemID", new string[] { hdfId.Value });
            }
            else
            {                               
                am.DataAccess.BatchQuery.Insert("ItemInfo", "SubCategoryID,ItemName,ItemAlias,ItemType,Description,Active",
                new string[] { hdfSubCatId.Value , itemName , itemAlias , itemType , description , active }, "ItemID");                
            }           

            if(am.DataAccess.BatchQuery.Execute())
            {
                hdfId.Value = "0";
                am.Utility.ShowHTMLAlert(Page, "000", "Saved Successfully.");
                BindToDataSet(RadTreeView1, false);
                SearchAndSelectNode(itemName, false);
            }            
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetRecord(true, false, false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void DeleteRecord()
        {
            try
            {
                if (hdfId.Value == "0")
                {
                    return;
                }
                int id = int.Parse(hdfId.Value);

                bool deleted = false;               
                try
                {
                    try
                    {
                        string sqlStr = "";                       

                        am.DataAccess.BatchQuery.Delete("ItemInfo", "WHERE ItemID=@ItemID", new string[] { hdfId.Value });
                        deleted = am.DataAccess.BatchQuery.Execute();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    
                }

                if (!deleted)
                {
                    
                }
                else
                {                    
                    am.Utility.ShowHTMLAlert(Page, "000", "Deleted Successfully.");
                    BindToDataSet(RadTreeView1, false);
                    SearchAndSelectNode(cbxSubCat.Text, false);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRecord();
        }


        private bool Valid()
        {
            if (tbxName.Text.Trim().Length <= 0)
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Enter Item Name.");
                tbxName.Focus();
                return false;
            }

            if (hdfSubCatId.Value == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select a Subcategory.");
                RadTreeView1.Focus();
                return false;
            }

            if (cbxItemType.SelectedValue == "0")
            {
                am.Utility.ShowHTMLMessage(Page, "000", "Select Item Type.");
                cbxItemType.Focus();
                return false;
            }
            return true;
        }

        private void SearchAndSelectNode(string fieldText, bool fireClick)
        {
            RadTreeNode node = null;
            node = RadTreeView1.FindNodeByText(fieldText, true);
            if (node != null)
            {
                node.Selected = true;
                if (node.ParentNode != null)
                {
                    node.ParentNode.Expanded = true;
                    if (node.ParentNode.ParentNode != null)
                    {
                        node.ParentNode.ParentNode.Expanded = true;
                    }
                }
                if (fireClick)
                {
                    RadTreeNodeChangeHandler(node);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            foreach (RadTreeNode item in RadTreeView1.Nodes)
            {
                if (item.Selected)
                    item.Selected = false;
                if (item.Expanded)
                    item.Expanded = false;
            }
            ResetRecord(true, false, false);
            SearchAndSelectNode(txtFilter.Text.Trim(), true);
        }

    }
}