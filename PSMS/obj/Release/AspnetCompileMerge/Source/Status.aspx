﻿<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="PSMS.Status" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Status</title> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm">
                <div class="baseFormTitle"><b>MANAGE</b> PROCUREMENT STATUS</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />

                <div class="settingsFormBodyLeft">
                    <telerik:RadGrid ID="grd" runat="server" AutoGenerateColumns="False"
                        ClientSettings-Scrolling-AllowScroll="true"
                        MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCommand="grd_ItemCommand"
                        OnNeedDataSource="grd_NeedDataSource" PageSize="5" CellSpacing="-1" GridLines="Both" Height="100%"
                        Width="100%" Skin="Metro">
                        <GroupingSettings CaseSensitive="false" />
                        <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadDropDownList" />
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                        </ClientSettings>
                        <MasterTableView BorderStyle="None" DataKeyNames="StatusID">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="StatusID" HeaderText="ID" ReadOnly="true" UniqueName="StatusID" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Active" HeaderText="Active" ReadOnly="true" UniqueName="Active" Display="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OrderNo" HeaderText="OrderNo" ReadOnly="true" UniqueName="OrderNo" Display="false"></telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="StatusName" HeaderText="Name" UniqueName="StatusName">                                    
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn HeaderText="Edit" EditText="Edit">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete"
                                    ConfirmText="Delete this Record?" Text="Delete" UniqueName="DeleteColumn" Display="false">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridButtonColumn>

                            </Columns>
                            <PagerStyle PageSizeControlType="RadDropDownList" />
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="settingsFormBodyRight">
                    <table class="settingsEntryTable">
                        <tr>
                            <td style="width: 120px">Status Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxName" runat="server" MaxLength="100" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td>Active</td>
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
