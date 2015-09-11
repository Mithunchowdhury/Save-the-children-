using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PSMS
{
    public partial class Permission : System.Web.UI.Page
    {
        AppManager am = new AppManager();
        string pagename = "Permission Information";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                am.DataAccess.SetUISecurity(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                am.DataAccess.OnShowError += DataAccess_OnShowError;
            }
            if (!IsPostBack)
            {
                LoadComboItems();
                LoadRecord(true, true, false, false, false);
                //rdbApplySP.Visible = false;
                //rdbRemoveSP.Visible = false;
                //btnSave.Attributes.Add("ajaxloader", "true");
            }
        }

        void DataAccess_OnShowError(string ErrorCode, string ErrorMessage)
        {
            am.Utility.ShowHTMLMessage(this.Page, ErrorCode, ErrorMessage);
        }

        private void LoadComboItems()
        {
            string sqlStr = "SELECT GroupID, GroupName from GroupInfo where Active = 1 and GroupID in (select distinct GroupID from UserInfo where GroupID is not null)";
            am.Utility.LoadComboBox(cbxGroups, sqlStr, "GroupName", "GroupID", false);
            if (cbxGroups.Items.Count > 0)
            {
                cbxGroups.SelectedIndex = 0;
            }

        }

        private void LoadRecord(bool clearError, bool group, bool skipGroupReset, bool skipUserBind, bool skipResetUser)
        {
            try
            {
                string sqlQuery = "";
                //users - conditionally
                if (group)
                {
                    if (!skipUserBind)
                    {
                        sqlQuery = " select CAST(0 AS BIT) as IsChecked, UserID, UserName from UserInfo ";
                        sqlQuery += " where UserID not in ( ";
                        sqlQuery += " select distinct UserId from AppPermission where GroupId = -1 ";
                        sqlQuery += " ) ";
                        sqlQuery += "and GroupID = @GroupID";

                        DataTable dt = am.DataAccess.RecordSet(sqlQuery, new string[] { cbxGroups.SelectedValue });
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rdgUsers.DataSource = dt;
                        }
                        else
                        {
                            rdgUsers.DataSource = new string[] {};
                        }
                        rdgUsers.Rebind();
                    }
                }
                else
                {
                    if (!skipUserBind)
                    {
                        sqlQuery = "select CAST(0 AS BIT) as IsChecked, UserID, UserName from UserInfo";

                        DataTable dt = am.DataAccess.RecordSet(sqlQuery, new string[] { });
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rdgUsers.DataSource = dt;
                        }
                        else
                        {
                            rdgUsers.DataSource = new string[] {};
                        }
                        rdgUsers.Rebind();
                    }
                }
                //Set first user
                if (!group && rdgUsers.MasterTableView.Items.Count > 0)
                {
                    //0 - from radio -1 from checkbox
                    if (hdfUserId.Value == "0")
                    {
                        hdfUserId.Value = "-1";
                        foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
                        {
                            try
                            {
                                hdfUserId.Value = dataItem["UserID"].Text;
                                (dataItem.FindControl("chkSelectedUser") as CheckBox).Checked = true;
                                break;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        //load selected row's record
                        foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
                        {
                            try
                            {
                                if ((dataItem.FindControl("chkSelectedUser") as CheckBox).Checked)
                                {
                                    hdfUserId.Value = dataItem["UserID"].Text;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                else
                {
                    //For group - select all                    
                    GridHeaderItem headerItem = rdgUsers.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                    (headerItem.FindControl("chkSelectedUserHeader") as CheckBox).Enabled = false;

                    foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
                    {
                        (dataItem.FindControl("chkSelectedUser") as CheckBox).Checked = true;
                        (dataItem.FindControl("chkSelectedUser") as CheckBox).Enabled = false;
                    }
                }

                //Resources                
                sqlQuery = "";
                string[] sqlWhereValue = new string[] { };

                SqlParameter[] parametersR = new SqlParameter[2];
                sqlQuery += " select Cast(1 as bit) as IsChecked, ar.Id, ar.Name, ar." + ConfigurationManager.AppSettings["ResourcePath"] + ", ";
                sqlQuery += " ap.Id as PermissionId, ap.UserId, ap.GroupId, ap.ExecuteInsert, ";
                sqlQuery += " ap.ExecuteRead, ap.ExecuteUpdate, ap.ExecuteDelete from AppResource as ar left join AppPermission as ap ";
                sqlQuery += " on ar.Id = ap.AppResourceId ";
                sqlQuery += " where ar.Active = 1 and ";
                sqlQuery += " ap.UserId = @UserId and ap.GroupId = @GroupId ";
                sqlQuery += " union  ";
                sqlQuery += " select Cast(0 as bit) as IsChecked, ar.Id, ar.Name, ar.RelativePath, ";
                sqlQuery += " Cast(0 as bit) as PermissionId, Cast(0 as bit) as UserId, Cast(0 as bit) as GroupId, Cast(0 as bit) as ExecuteInsert, ";
                sqlQuery += " Cast(0 as bit) as ExecuteRead, Cast(0 as bit) as ExecuteUpdate, Cast(0 as bit) as ExecuteDelete  ";
                sqlQuery += " from AppResource as ar ";
                sqlQuery += " where ar.Active = 1 and ";
                sqlQuery += " ar.Id not in (select distinct AppResourceId from AppPermission as ap where ap.UserId = @UserId1 and ap.GroupId = @GroupId2) ";
                sqlQuery += " order by ar.Id ";
                if (group)
                {
                    sqlWhereValue = new string[] { 
                    "-1" , cbxGroups.SelectedValue , "-1" , cbxGroups.SelectedValue};
                }
                else
                {
                    sqlWhereValue = new string[] { hdfUserId.Value, "-1", hdfUserId.Value, "-1" };
                }

                DataTable dtR = am.DataAccess.RecordSet(sqlQuery, sqlWhereValue);
                if (dtR != null && dtR.Rows.Count > 0)
                {
                    grdResources.DataSource = dtR;
                }
                else
                {
                    grdResources.DataSource = null;
                }
                grdResources.Rebind();
                ResetRecord(clearError, skipGroupReset, skipResetUser);
            }
            catch (Exception ex)
            {
                am.Utility.ShowHTMLMessage(this.Page, "000", ex.Message);
            }

        }

        private void ResetRecord(bool clearError, bool skipGroupReset, bool skipResetUser)
        {
            if (!skipResetUser)
            {
                hdfUserId.Value = "-1";
            }

            if (!skipGroupReset)
            {
                rdbGroup.Checked = true;
                rdbUser.Checked = false;
                //rdbApplySP.Visible = false;
                //rdbRemoveSP.Visible = false;
                lblGroup.Visible = true;
                cbxGroups.Visible = true;
            }
        }



        public void SpecialPermissionControl()
        {
            if (rdbGroup.Checked)
            {
                //rdbApplySP.Visible = false;
                //rdbRemoveSP.Visible = false;
                btnResetSPPermission.Visible = false;
                lblGroup.Visible = true;
                cbxGroups.Visible = true;
                LoadRecord(true, true, true, false, false);
            }
            else
            {
                //rdbApplySP.Visible = true;
                //rdbRemoveSP.Visible = true;
                //rdbApplySP.Checked = true;
                //rdbRemoveSP.Checked = false;
                btnResetSPPermission.Visible = true;
                lblGroup.Visible = false;
                cbxGroups.Visible = false;
                hdfUserId.Value = "0";
                LoadRecord(true, false, true, false, true);
            }
        }

        protected void cbxGroups_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadRecord(true, rdbGroup.Checked, rdbGroup.Checked, false, false);
        }

        protected void chkSelectedUserHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
            {
                (dataItem.FindControl("chkSelectedUser") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkSelectedUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGroup.Checked)
            {
                ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
                bool checkHeader = false;
                foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
                {
                    if ((dataItem.FindControl("chkSelectedUser") as CheckBox).Checked)
                    {
                        checkHeader = true;
                        break;
                    }
                }
                GridHeaderItem headerItem = rdgUsers.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                if ((headerItem.FindControl("chkSelectedUserHeader") as CheckBox).Checked)
                {
                    (headerItem.FindControl("chkSelectedUserHeader") as CheckBox).Checked = checkHeader;
                }
            }
            else
            {
                CheckBox chk = (sender as CheckBox);
                bool individualUserSelected = chk.Checked;
                if (individualUserSelected)
                {
                    foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
                    {
                        CheckBox item = (dataItem.FindControl("chkSelectedUser") as CheckBox);
                        if (item != chk)
                            item.Checked = false;
                        else
                        {
                            //leave it
                        }
                    }

                    //restore
                    hdfUserId.Value = "-1";
                    LoadRecord(true, false, true, true, true);
                }
            }

        }

        protected void chkSelectedResourceHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                (dataItem.FindControl("chkSelectedResource") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void chkSelectedResource_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelectedResource") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdResources.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkSelectedResourceHeader") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkSelectedResourceHeader") as CheckBox).Checked = checkHeader;
            }
        }

        protected void chkExecuteInsertHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelectedResource") as CheckBox).Checked)
                {
                    (dataItem.FindControl("chkExecuteInsert") as CheckBox).Checked = headerCheckBox.Checked;
                    dataItem.Selected = headerCheckBox.Checked;
                }
            }
        }

        protected void chkExecuteInsert_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkExecuteInsert") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdResources.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkExecuteInsertHeader") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkExecuteInsertHeader") as CheckBox).Checked = checkHeader;
            }
        }

        protected void chkExecuteReadHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelectedResource") as CheckBox).Checked)
                {
                    (dataItem.FindControl("chkExecuteRead") as CheckBox).Checked = headerCheckBox.Checked;
                    dataItem.Selected = headerCheckBox.Checked;
                }
            }
        }

        protected void chkExecuteRead_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkExecuteRead") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdResources.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkExecuteReadHeader") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkExecuteReadHeader") as CheckBox).Checked = checkHeader;
            }
        }

        protected void chkExecuteUpdateHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelectedResource") as CheckBox).Checked)
                {
                    (dataItem.FindControl("chkExecuteUpdate") as CheckBox).Checked = headerCheckBox.Checked;
                    dataItem.Selected = headerCheckBox.Checked;
                }
            }
        }

        protected void chkExecuteUpdate_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkExecuteUpdate") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdResources.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkExecuteUpdateHeader") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkExecuteUpdateHeader") as CheckBox).Checked = checkHeader;
            }
        }

        protected void chkExecuteDeleteHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkSelectedResource") as CheckBox).Checked)
                {
                    (dataItem.FindControl("chkExecuteDelete") as CheckBox).Checked = headerCheckBox.Checked;
                    dataItem.Selected = headerCheckBox.Checked;
                }
            }
        }

        protected void chkExecuteDelete_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = false;
            foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
            {
                if ((dataItem.FindControl("chkExecuteDelete") as CheckBox).Checked)
                {
                    checkHeader = true;
                    break;
                }
            }
            GridHeaderItem headerItem = grdResources.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            if ((headerItem.FindControl("chkExecuteDeleteHeader") as CheckBox).Checked)
            {
                (headerItem.FindControl("chkExecuteDeleteHeader") as CheckBox).Checked = checkHeader;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            LoadRecord(true, true, false, false, false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SavePermission())
            {
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_SAVE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
                //LoadRecord(false, true, false, false, false);
            }
        }

        private bool SavePermission(bool removeSpecialPermission = false)
        {
            bool needtosave = false;
            try
            {
                string gid = cbxGroups.SelectedValue;
                string uid = hdfUserId.Value;
                //if (rdbGroup.Checked || (rdbUser.Checked && rdbApplySP.Checked))
                if (rdbGroup.Checked || (rdbUser.Checked && !removeSpecialPermission))
                {
                    //save group wise
                    foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
                    {
                        CheckBox resourceSelected = (dataItem.FindControl("chkSelectedResource") as CheckBox);
                        bool isResourceSelected = resourceSelected.Checked;
                        string resourceId = dataItem["Id"].Text;
                        if (isResourceSelected)
                        {

                            //add permission for this resource (group)
                            CheckBox insertChk = (dataItem.FindControl("chkExecuteInsert") as CheckBox);
                            bool canInsert = insertChk.Checked;
                            CheckBox readChk = (dataItem.FindControl("chkExecuteRead") as CheckBox);
                            bool canRead = readChk.Checked;
                            CheckBox updateChk = (dataItem.FindControl("chkExecuteUpdate") as CheckBox);
                            bool canUpdate = updateChk.Checked;
                            CheckBox deleteChk = (dataItem.FindControl("chkExecuteDelete") as CheckBox);
                            bool canDelete = deleteChk.Checked;

                            string sqlStr = "";
                            string deleteWhereClause = "";
                            string[] sqlValues = new string[] { };
                            string[] deleteWhereValues = new string[] { };

                            if (rdbGroup.Checked)
                            {
                                deleteWhereClause = "GroupId=@GroupId and AppResourceId=@AppResourceId and UserId=-1";
                                deleteWhereValues = new string[] { gid, resourceId };
                            }
                            else if (rdbUser.Checked && !removeSpecialPermission)
                            {
                                deleteWhereClause = "UserId=@UserId and AppResourceId=@AppResourceId and GroupId=-1";
                                deleteWhereValues = new string[] { uid, resourceId };
                            }

                            SqlParameter[] parameters = new SqlParameter[7];
                            if (rdbGroup.Checked)
                            {
                                sqlValues = new string[]{"-1" , gid , resourceId , (canInsert == true ? 1 : 0).ToString() , (canRead == true ? 1 : 0).ToString() ,
                                (canUpdate == true ? 1 : 0).ToString() , (canDelete == true ? 1 : 0).ToString()};
                            }
                            else if (rdbUser.Checked && !removeSpecialPermission)
                            {
                                sqlValues = new string[]{uid , "-1" , resourceId , (canInsert == true ? 1 : 0).ToString() , (canRead == true ? 1 : 0).ToString() ,
                                (canUpdate == true ? 1 : 0).ToString() , (canDelete == true ? 1 : 0).ToString()};
                            }

                            am.DataAccess.BatchQuery.Delete("AppPermission", deleteWhereClause, deleteWhereValues);
                            am.DataAccess.BatchQuery.Insert("AppPermission", "UserId,GroupId,AppResourceId,ExecuteInsert,ExecuteRead,ExecuteUpdate,ExecuteDelete", sqlValues, "UserId");

                            needtosave = true;
                            
                            //saved = am.DataAccess.BatchQuery.Execute(true);
                            //if (!saved)
                            //    return false;
                        }
                        else
                        {
                            //delete any existing permission for this resource (group)
                            if (rdbGroup.Checked)
                            {
                                string deleteWhereClause = "GroupId=@GroupId and AppResourceId=@AppResourceId and UserId=-1";
                                string[] deleteWhereValues = new string[] { gid, resourceId };
                                am.DataAccess.BatchQuery.Delete("AppPermission", deleteWhereClause, deleteWhereValues);
                                //bool deleted = am.DataAccess.BatchQuery.Execute();
                                //if (!deleted)
                                //    return false;
                            }
                            else if (rdbUser.Checked)
                            {
                                string deleteWhereClause = "GroupId=-1 and AppResourceId=@AppResourceId and UserId=@UserId";
                                string[] deleteWhereValues = new string[] { resourceId, uid };
                                am.DataAccess.BatchQuery.Delete("AppPermission", deleteWhereClause, deleteWhereValues);
                                //bool deleted = am.DataAccess.BatchQuery.Execute();
                                //if (!deleted)
                                //    return false;
                            }
                        }
                    }
                    if(needtosave)
                        return am.DataAccess.BatchQuery.Execute(true);
                    else
                        return am.DataAccess.BatchQuery.Execute();
                }
                else if (rdbUser.Checked && removeSpecialPermission)
                {
                    //remove user wise
                    foreach (GridDataItem dataItem in grdResources.MasterTableView.Items)
                    {
                        string resourceId = dataItem["Id"].Text;

                        string deleteWhereClause = "UserId=@UserId and AppResourceId=@AppResourceId and GroupId=-1";
                        string[] deleteWhereValues = new string[] { uid, resourceId };
                        am.DataAccess.BatchQuery.Delete("AppPermission", deleteWhereClause, deleteWhereValues);
                        bool deleted = am.DataAccess.BatchQuery.Execute();
                        if (!deleted)
                            return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        protected void rdbGroup_CheckedChanged(object sender, EventArgs e)
        {
            SpecialPermissionControl();
        }

        protected void rdbUser_CheckedChanged(object sender, EventArgs e)
        {
            SpecialPermissionControl();
        }

        protected void btnResetSPPermission_Click(object sender, EventArgs e)
        {
            if (SavePermission(true))
            {
                am.Utility.ShowHTMLAlert(this.Page, "000", AppUtility.SUCCESSFUL_DELETE_MSG.Replace(AppUtility.MESSAGING_REPLACE_TAG, pagename));
                LoadRecord(false, true, false, false, false);
            }
        }

        //protected void rdgUsers_ItemCommand(object sender, GridCommandEventArgs e)
        //{
        //    bool group = rdbGroup.Checked;
        //    if (e.CommandName == "Filter")
        //    {
        //        if (group)
        //        {
        //            LoadRecord(true, rdbGroup.Checked, rdbGroup.Checked, true, true);
        //            foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
        //            {                        
        //                    (dataItem.FindControl("chkSelectedUser") as CheckBox).Enabled = false;                       
        //            }
        //        }
        //        else
        //        {
        //            LoadRecord(true, rdbGroup.Checked, rdbGroup.Checked, false, true);
        //            foreach (GridDataItem dataItem in rdgUsers.MasterTableView.Items)
        //            {
        //                (dataItem.FindControl("chkSelectedUser") as CheckBox).Enabled = true;
        //            }
        //        }
        //    }

        //}

    }


    public class AppUtility
    {
        #region Messaging
        public static string MESSAGING_REPLACE_TAG = "PageName";
        public static string SUCCESSFUL_SAVE_MSG = "PageName Saved.";
        public static string UNSUCCESSFUL_SAVE_MSG = "Could not save PageName.";
        public static string SUCCESSFUL_DELETE_MSG = "PageName Deleted.";
        public static string UNSUCCESSFUL_DELETE_MSG = "Could not delete PageName.";
        #endregion
    }
}