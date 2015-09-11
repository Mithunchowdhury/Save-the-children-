<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="PSMS.User" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>User</title> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm" style="width:800px;">
                <div class="baseFormTitle"><b>MANAGE</b> USER ACCOUNT</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft" style="width:55%;">
                    <telerik:RadGrid ID="grd" runat="server" AutoGenerateColumns="False"
                        ClientSettings-Scrolling-AllowScroll="true"
                        MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false"
                        OnItemCommand="grd_ItemCommand"  OnNeedDataSource="grd_NeedDataSource"
                        CellSpacing="-1" GridLines="Both" Height="100%" Skin="Metro">  
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="UserID" BorderStyle="None"  AllowFilteringByColumn="true">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserID" HeaderText="ID" ReadOnly="true" UniqueName="UserID" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GroupID" HeaderText="GroupID" ReadOnly="true" UniqueName="GroupID" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Active" HeaderText="Active" ReadOnly="true" UniqueName="Active" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="UserName" HeaderText="Code" UniqueName="UserName">
                                    <HeaderStyle Width="120px" />
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FullName" HeaderText="Name" UniqueName="FullName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StaffCode" HeaderText="Staff Code" UniqueName="StaffCode" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>                               
                                <telerik:GridBoundColumn DataField="ActiveStr" HeaderText="Active" UniqueName="ActiveStr" AllowFiltering="false">
                                    <HeaderStyle Width="50px" />                                    
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn HeaderText="Edit" EditText="Edit">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" ConfirmText="Delete this Record?" Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridButtonColumn>

                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" UniqueName="EditCommandColumn1">
                                </EditColumn>
                            </EditFormSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadComboBox" BorderColor="White" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="settingsFormBodyRight" style="width:35%;">
                    <table class="settingsEntryTable">
                        <tr>
                            <td style="width: 120px">Login Name</td>
                        </tr>
                        <tr>
                            <td>

                                <telerik:RadTextBox ID="tbxUserName" runat="server" MaxLength="50" Width="100%" Skin="Silk" />

                            </td>
                        </tr>                       
                        <tr>
                            <td style="width: 120px">Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxFullName" runat="server" MaxLength="100" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px">Staff ID</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxStaffCode" runat="server" MaxLength="5" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px">Group</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxGroups" runat="server" Width="100%" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px">Location</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxLocations" runat="server" Width="100%" Skin="Silk" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px">Active</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkActive" runat="server" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Button ID="btnSave" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnSave_Click" Text="Save"></asp:Button>
                                <asp:Button ID="btnReset" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click" Text="Reset"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
