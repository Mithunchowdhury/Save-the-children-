<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="PCMember.aspx.cs" Inherits="PSMS.PCMember" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>PC Member</title> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm">
                <div class="baseFormTitle"><b>MANAGE</b> PC MEMBER</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft">
                    <telerik:RadGrid ID="grd" runat="server" AutoGenerateColumns="False"
                        ClientSettings-Scrolling-AllowScroll="true"
                        MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCommand="grd_ItemCommand" OnNeedDataSource="grd_NeedDataSource" 
                        CellSpacing="-1" GridLines="Both" Height="98%" Width="98%" Skin="Metro">
                        <GroupingSettings CaseSensitive="false" />
                        <%--<PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadDropDownList" />--%>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                        </ClientSettings>
                        <MasterTableView BorderStyle="None" DataKeyNames="StaffID">
                             <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <CommandItemSettings ShowAddNewRecordButton="False"  ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="StaffID" HeaderText="StaffID" ReadOnly="true" UniqueName="StaffID" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Active" HeaderText="Active" ReadOnly="true" UniqueName="Active" Display="false"></telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="StaffName" HeaderText="Staff Name" ReadOnly="true" UniqueName="StaffName">
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
                            <%--<PagerStyle PageSizeControlType="RadDropDownList" />--%>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="settingsFormBodyRight">
                    <table class="settingsEntryTable">
                        <tr>
                            <td style="width: 120px">Staff</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxStaff" runat="server" AutoPostBack="True" Width="100%" Skin="Silk" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
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
