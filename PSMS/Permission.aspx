<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Permission.aspx.cs" Inherits="PSMS.Permission" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Permission</title> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: 900px; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> PERMISSION INFO</div>
                <asp:HiddenField ID="hdfUserId" runat="server" Value="-1" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 10%"></td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: left;">
                                <asp:RadioButton ID="rdbGroup" AutoPostBack="true" runat="server" GroupName="PermissionType"
                                    ToggleType="Radio" ButtonType="ToggleButton" Text="Group" Checked="true" OnCheckedChanged="rdbGroup_CheckedChanged"></asp:RadioButton>
                                <asp:RadioButton ID="rdbUser" AutoPostBack="true" runat="server" GroupName="PermissionType"
                                    ToggleType="Radio" ButtonType="ToggleButton" Text="User" OnCheckedChanged="rdbUser_CheckedChanged"></asp:RadioButton>
                            </td>
                            <td colspan="2"></td>
                            <td>
                                <asp:Label ID="lblGroup" runat="server" Text="Groups"></asp:Label>
                            </td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="cbxGroups" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbxGroups_SelectedIndexChanged" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="10" style="text-align: left;">
                                <asp:RadioButton ID="rdbApplySP" AutoPostBack="true" runat="server" GroupName="SpecialPermission"
                                    ToggleType="Radio" ButtonType="ToggleButton" Text="Apply Special Permission" Checked="true" Visible="false"></asp:RadioButton>
                                <asp:RadioButton ID="rdbRemoveSP" AutoPostBack="true" runat="server" GroupName="SpecialPermission"
                                    ToggleType="Radio" ButtonType="ToggleButton" Text="Remove Special Permission" Checked="false" Visible="false"></asp:RadioButton>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="3">Users</td>
                            <td colspan="7">Resources</td>
                        </tr>
                        <tr>
                            <td colspan="3" style="vertical-align:top;">
                                <telerik:RadGrid ID="rdgUsers" runat="server" AutoGenerateColumns="False"
                                    ClientSettings-Scrolling-AllowScroll="true" Height="300px"
                                    MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Skin="Metro" 
                                    AllowFilteringByColumn="true">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                            ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                                    </ClientSettings>
                                    <MasterTableView BorderStyle="None">
                                        <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                        <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="50px" DataField="IsChecked"
                                                HeaderText="" UniqueName="IsChecked" AllowFiltering="false">
                                                <ItemStyle Width="12%" />
                                                <HeaderStyle Width="12%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectedUserHeader" runat="server" Text=""
                                                        AutoPostBack="True" OnCheckedChanged="chkSelectedUserHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectedUser" AutoPostBack="True" Checked='<%#Bind("IsChecked") %>' runat="server" OnCheckedChanged="chkSelectedUser_CheckedChanged" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="UserID" HeaderText="UserID" UniqueName="UserID" Display="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" HeaderText="User" UniqueName="UserName" AllowFiltering="false">                                                
                                                <ItemStyle Width="85%" />
                                                <HeaderStyle Width="85%" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                            <td colspan="7" style="vertical-align:top;">
                                <telerik:RadGrid ID="grdResources" runat="server" AutoGenerateColumns="False"
                                    ClientSettings-Scrolling-AllowScroll="true" Height="300px"
                                    MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Skin="Metro">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                            ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                                    </ClientSettings>
                                    <MasterTableView BorderStyle="None">
                                        <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                        <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="IsChecked" HeaderText="" UniqueName="IsChecked">
                                                <ItemStyle Width="5%" />
                                                <HeaderStyle Width="5%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectedResourceHeader" runat="server" Text=""
                                                        AutoPostBack="True" OnCheckedChanged="chkSelectedResourceHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectedResource" AutoPostBack="True" Checked='<%#Bind("IsChecked") %>' runat="server" OnCheckedChanged="chkSelectedResource_CheckedChanged" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Id" HeaderText="Id" UniqueName="Id" Display="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Name" HeaderText="Resource" UniqueName="Name">
                                                <ItemStyle Width="45%" />
                                                <HeaderStyle Width="45%" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn DataField="ExecuteInsert" HeaderText="Insert" UniqueName="ExecuteInsert">
                                                <ItemStyle Width="12%" />
                                                <HeaderStyle Width="12%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkExecuteInsertHeader" runat="server" Text="Insert"
                                                        AutoPostBack="True" OnCheckedChanged="chkExecuteInsertHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExecuteInsert" Checked='<%#Bind("ExecuteInsert") %>' runat="server" OnCheckedChanged="chkExecuteInsert_CheckedChanged" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="ExecuteRead" HeaderText="Read" UniqueName="ExecuteRead">
                                                <ItemStyle Width="12%" />
                                                <HeaderStyle Width="12%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkExecuteReadHeader" runat="server" Text="Read"
                                                        AutoPostBack="True" OnCheckedChanged="chkExecuteReadHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExecuteRead" Checked='<%#Bind("ExecuteRead") %>' runat="server" OnCheckedChanged="chkExecuteRead_CheckedChanged" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="ExecuteUpdate" HeaderText="Update" UniqueName="ExecuteUpdate">
                                                <ItemStyle Width="14%" />
                                                <HeaderStyle Width="14%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkExecuteUpdateHeader" runat="server" Text="Update"
                                                        AutoPostBack="True" OnCheckedChanged="chkExecuteUpdateHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExecuteUpdate" Checked='<%#Bind("ExecuteUpdate") %>' runat="server" OnCheckedChanged="chkExecuteUpdate_CheckedChanged" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="ExecuteDelete" HeaderText="Delete" UniqueName="ExecuteDelete">
                                                <ItemStyle Width="12%" />
                                                <HeaderStyle Width="12%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkExecuteDeleteHeader" runat="server" Text="Delete"
                                                        AutoPostBack="True" OnCheckedChanged="chkExecuteDeleteHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExecuteDelete" Checked='<%#Bind("ExecuteDelete") %>' runat="server" OnCheckedChanged="chkExecuteDelete_CheckedChanged" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="10" style="text-align: right">
                                <div style="text-align: right; clear: both;">
                                    <asp:Button ID="btnResetSPPermission" runat="server"  BorderStyle="None" alt="ajax" CssClass="buttonStyle"
                                        Text="Reset Special Permission" 
                                        OnClientClick="if ( !confirm('You will get only group permision. Are you sure you want to delete special permission?')) return false;" 
                                        OnClick="btnResetSPPermission_Click"></asp:Button>
                                    <asp:Button ID="btnSave" runat="server" alt="ajax"
                                        BorderStyle="None" CssClass="buttonStyle" Text="Save" OnClick="btnSave_Click"></asp:Button>
                                    <%--<asp:Button ID="btnReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Reset" OnClick="btnReset_Click"></asp:Button>--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
