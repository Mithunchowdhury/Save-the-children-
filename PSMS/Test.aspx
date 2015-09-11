<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="PSMS.Test" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>USERINFO</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm">
                <div class="baseFormTitle"><b>MANAGE</b> UserInfo</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft">
                    <telerik:RadGrid ID="grdUserInfo" runat="server" AutoGenerateColumns="false"
                        AllowPaging="False" PageSize="10" Skin="Metro" OnItemCommand="grd_ItemCommand">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="UserID" BorderStyle="None">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserID" HeaderText="UserID" SortExpression="UserID" UniqueName="UserID">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="UserName" SortExpression="UserName" UniqueName="UserName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Password" HeaderText="Password" SortExpression="Password" UniqueName="Password">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FullName" HeaderText="FullName" SortExpression="FullName" UniqueName="FullName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StaffCode" HeaderText="StaffCode" SortExpression="StaffCode" UniqueName="StaffCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GroupID" HeaderText="GroupID" SortExpression="GroupID" UniqueName="GroupID">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LocationID" HeaderText="LocationID" SortExpression="LocationID" UniqueName="LocationID">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Active" HeaderText="Active" SortExpression="Active" UniqueName="Active">
                                </telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn HeaderText="Edit" EditText="Edit">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" ConfirmText="Delete this Record?" Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadComboBox" BorderColor="White" />
                        </MasterTableView>
                    </telerik:RadGrid>

                </div>
                <div class="settingsFormBodyRight">
                    <table class="settingsEntryTable">
                        <tr>
                            <td>UserID</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cboUserID" runat="server" Skin="Silk" Width="100%"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>UserName</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxUserName" runat="server" Skin="Silk" Width="100%"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td>Password</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxPassword" runat="server" Skin="Silk" Width="100%"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td>FullName</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxFullName" runat="server" Skin="Silk" Width="100%"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td>StaffCode</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxStaffCode" runat="server" Skin="Silk" Width="100%"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td>GroupID</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cboGroupID" runat="server" Skin="Silk" Width="100%"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>LocationID</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cboLocationID" runat="server" Skin="Silk" Width="100%"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Active</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxActive" runat="server" Width="100%" Checked="True" /></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Button ID="btnSave" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnSave_Click" Text="Save"></asp:Button>
                                <asp:Button ID="btnDelete" runat="server" BorderStyle="None" CssClass="buttonStyle"
                                    OnClientClick="if ( !confirm('Are you sure you want to delete this record?')) return false;"
                                    OnClick="btnDelete_Click" Text="Delete"></asp:Button>
                                <asp:Button ID="btnReset" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click" Text="Reset"></asp:Button>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
