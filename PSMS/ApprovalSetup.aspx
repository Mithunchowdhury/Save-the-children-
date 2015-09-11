<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="ApprovalSetup.aspx.cs" Inherits="PSMS.ApprovalSetup" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Approval</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="settingsForm" style="width: 700px;">
                <div class="baseFormTitle"><b>MANAGE</b>
                    <asp:Label ID="appLabel" runat="server" Text="APPROVAL"></asp:Label></div>
                <asp:HiddenField ID="hdfTechId" runat="server" Value="0" />
                <asp:HiddenField ID="hdfGrantsId" runat="server" Value="0" />

                <div style="visibility: hidden;">
                    <telerik:RadButton ID="rdbTechApp" AutoPostBack="true" runat="server" GroupName="ApprovalType"
                        ToggleType="Radio" ButtonType="ToggleButton" Text="Technical Approval" Checked="true" OnClick="rdbTechApp_Click">
                    </telerik:RadButton>
                    <telerik:RadButton ID="rdbGrantsApp" AutoPostBack="true" runat="server" GroupName="ApprovalType"
                        ToggleType="Radio" ButtonType="ToggleButton" Text="Grants Approval" OnClick="rdbGrantsApp_Click">
                    </telerik:RadButton>
                </div>

                <div id="pnlTechApproval" runat="server" style="height: 100%;">

                    <div class="settingsFormBodyLeft">
                        <telerik:RadGrid ID="grdTech" runat="server" AutoGenerateColumns="False"
                            ClientSettings-Scrolling-AllowScroll="true" ClientSettings-Scrolling-ScrollHeight="300px"
                            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCommand="grdTech_ItemCommand" OnNeedDataSource="grdTech_NeedDataSource"
                            AllowPaging="False" AllowSorting="True" PageSize="5" CellSpacing="-1" GridLines="Both" Height="100%">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadDropDownList" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                    ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="TechApprovalID" BorderStyle="None">
                                <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="TechApprovalID" HeaderText="TechApprovalID" ReadOnly="true" UniqueName="TechApprovalID" Display="false">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Active" HeaderText="Active" ReadOnly="true" UniqueName="Active" Display="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="StaffName" HeaderText="Approved By" ReadOnly="true" UniqueName="StaffName">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TechReviewBy" HeaderText="Reviewed By" ReadOnly="true" UniqueName="TechReviewBy">
                                        <HeaderStyle Width="65px" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>

                                    <telerik:GridEditCommandColumn HeaderText="Edit" EditText="Edit">
                                        <HeaderStyle Width="50px" />
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete"
                                        ConfirmText="Delete this Record?" Text="Delete" UniqueName="DeleteColumn">
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
                                <td>Technical Approval</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadComboBox ID="cbxTechApproval" runat="server" AutoPostBack="True" Width="100%" Skin="Silk"
                                        Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Reviewed By</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadComboBox ID="cbxTechReviewedBy" runat="server" AutoPostBack="True" Width="100%" Skin="Silk">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="0" Text="" />
                                            <telerik:RadComboBoxItem Value="ICT" Text="ICT" />
                                            <telerik:RadComboBoxItem Value="Admin" Text="Admin" />
                                            <telerik:RadComboBoxItem Value="SS" Text="SS" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Active</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkTechActive" runat="server" Checked="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnTechSave" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnTechSave_Click" Text="Save"></asp:Button>
                                    <asp:Button ID="btnTechReset" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnTechReset_Click" Text="Reset"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>

                <div id="pnlGrantsApproval" runat="server" style="height: 100%;">
                    <div class="settingsFormBodyLeft">
                        <telerik:RadGrid ID="grdGrants" runat="server" AutoGenerateColumns="False"
                            ClientSettings-Scrolling-AllowScroll="true" ClientSettings-Scrolling-ScrollHeight="300px"
                            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCommand="grdGrants_ItemCommand" OnNeedDataSource="grdGrants_NeedDataSource"
                            AllowPaging="False" AllowSorting="True" PageSize="5" CellSpacing="-1" GridLines="Both" Height="100%">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadDropDownList" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                    ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="GrantsApprovalID" BorderStyle="None">
                                <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="GrantsApprovalID" HeaderText="GrantsApprovalID" ReadOnly="true" UniqueName="GrantsApprovalID" Display="false">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="StaffName" HeaderText="Approved By" ReadOnly="true" UniqueName="StaffName">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Active" HeaderText="Active" ReadOnly="true" UniqueName="Active" Display="false"></telerik:GridBoundColumn>

                                    <telerik:GridEditCommandColumn HeaderStyle-Width="50px" HeaderText="Edit" EditText="Edit" />
                                    <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="Delete" CommandName="Delete" ConfirmText="Delete this Record?" Text="Delete" UniqueName="DeleteColumn" />
                                    <%-- <telerik:GridTemplateColumn>
                                            <ItemStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:Button ID="del" runat="server" Text="Delete" OnClick="Delete_Click" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                </Columns>
                                <PagerStyle PageSizeControlType="RadDropDownList" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>

                    <div class="settingsFormBodyRight">
                        <table class="settingsEntryTable">
                            <tr>
                                <td>Grants Approval</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadComboBox ID="cbxGrantsApproval" runat="server" AutoPostBack="True" Width="100%" Skin="Silk"
                                        Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Active</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkGrantsActive" runat="server" Checked="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnGrantsSave" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnGrantsSave_Click" Text="Save"></asp:Button>
                                    <asp:Button ID="btnGrantsReset" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnGrantsReset_Click" Text="Reset"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
