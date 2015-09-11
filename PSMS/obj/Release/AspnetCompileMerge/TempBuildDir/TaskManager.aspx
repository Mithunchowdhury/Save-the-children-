<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="TaskManager.aspx.cs" Inherits="PSMS.TaskManager" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Task Manager</title>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="baseForm" style="width: 96%; height: auto;">

                <div class="baseFormTitle"><b>MANAGE</b> YOUR TASK</div>

                <div style="width: 100%; height: auto;">
                    <div id="divSearch" runat="server" class="baseFormBodyFlex" style="width: 98%; margin-bottom: 10px; float: left;">
                        <div style="font-size: 12px; font-weight: bold; float: left; width: 20%;">SEARCH RECORD</div>
                        <div style="width: 80%; float: right; font-size: 12px; font-weight: bold; margin-top: -5px;">
                            <asp:RadioButtonList ID="rblType" runat="server"
                                AutoPostBack="True" CellPadding="0" CssClass="inline-rb" OnSelectedIndexChanged="rblType_SelectedIndexChanged"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%">
                                <asp:ListItem Selected="True" Value="PR">PR (0)</asp:ListItem>
                                <asp:ListItem Value="RFQ">INVITATION (0)</asp:ListItem>
                                <asp:ListItem Value="SELECTION">SELECTION (0)</asp:ListItem>
                                <asp:ListItem Value="PO">PO (0)</asp:ListItem>
                                <asp:ListItem Value="GRN">GRN/SCN (0)</asp:ListItem>
                                <asp:ListItem Value="PAYMENT">PAYMENT (0)</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div style="font-size: 12px; clear: both; margin-top: -5px;">
                            <asp:Panel ID="pnPR" runat="server">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 130px;">STATUS</td>
                                        <td style="">
                                            <telerik:RadComboBox ID="cboStatus" runat="server" Skin="Silk">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 120px;">
                                            <asp:Label ID="lblRefNO" runat="server" Text="PR NO"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cboRefNO" runat="server" Skin="Silk" AllowCustomText="True" Filter="Contains" DropDownWidth="200px">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 80px;">PR REF</td>
                                        <td>
                                            <telerik:RadComboBox ID="cboREFID" runat="server" Skin="Silk" AllowCustomText="True" Filter="Contains" DropDownWidth="200px">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReqVen" runat="server" Text="REQUESTOR"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cboReqVen" runat="server" Skin="Silk" AllowCustomText="True" Filter="Contains" DropDownWidth="300px">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>DATE FROM</td>
                                        <td>
                                            <telerik:RadDatePicker ID="dtForm" runat="server" Skin="Silk">
                                                <Calendar EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" Skin="Silk" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                </Calendar>
                                                <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="40%">
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </DateInput>
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>DATE TO</td>
                                        <td class="">
                                            <telerik:RadDatePicker ID="dtTo" runat="server" Skin="Silk">
                                                <Calendar EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" Skin="Silk" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                </Calendar>
                                                <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="40%">
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </DateInput>
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPONO" runat="server" Text="PO NO" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cboPONO" runat="server" Skin="Silk" Visible="False">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>OFFICER</td>
                                        <td>
                                            <telerik:RadComboBox ID="cboOfficer" runat="server" Skin="Silk">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <div style="text-align: right; padding-right: 6px; padding-top: 5px;">
                                <asp:Button ID="cmdExport" runat="server" BorderStyle="None" Text="EXPORT" Width="120px" 
                                    CssClass="buttonStyle" OnClick="cmdExport_Click" Visible="false" />
                                <asp:Button ID="cmdSearch" runat="server" BorderStyle="None" Text="SEARCH" Width="120px" CssClass="buttonStyle" OnClick="cmdSearch_Click" />
                            </div>
                        </div>
                    </div>
                    
                    <div style="clear: both;"></div>
                    <telerik:RadGrid ID="rgTaskManager" runat="server" OnItemCommand="rgTaskManager_ItemCommand"  
                        Skin="Metro" AllowPaging="true" AutoGenerateColumns="False" CellSpacing="-1" GridLines="Both" BorderStyle="None" OnPageIndexChanged="rgTaskManager_PageIndexChanged" OnPageSizeChanged="rgTaskManager_PageSizeChanged">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings EnablePostBackOnRowClick="True" EnableRowHoverStyle="True" AllowKeyboardNavigation="true">
                            <Scrolling UseStaticHeaders="true" />
                            <Selecting AllowRowSelect="True"></Selecting>
                        </ClientSettings>
                        <MasterTableView ShowHeader="false" AutoGenerateColumns="false" BorderStyle="None">                            
                            <Columns>
<%--                                <telerik:GridBoundColumn FilterControlAltText="PRNO" UniqueName="PRNO" DataField="PRNO" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <ItemStyle Width="1px" />
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn FilterControlAltText="REFNO" UniqueName="REFNO" DataField="REFNO" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <ItemStyle Width="1px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn FilterControlAltText="Filter COL1 column" UniqueName="COL1" DataField="COL1">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <ItemStyle Width="38%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn FilterControlAltText="Filter COL2 column" UniqueName="COL2" DataField="COL2">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <ItemStyle Width="28%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn FilterControlAltText="Filter COL3 column" UniqueName="COL3" DataField="COL3">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <ItemStyle Width="21%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter CONTROLS column" UniqueName="CONTROLS">
                                    <ItemStyle Width="12%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Button ID="cmdView" runat="server" Text="VIEW" BorderStyle="None"
                                            Font-Size="10px" Width="37px" CssClass="buttonStyle" OnClick="cmdView_Click" />
                                        <asp:Button ID="cmdManage" runat="server" Text="MANAGE" BorderStyle="None" 
                                            Font-Size="10px" Width="53px" CssClass="buttonStyle" OnClick="cmdManage_Click" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadComboBox" BorderColor="White" />                            
                        </MasterTableView>                        
                        
                    </telerik:RadGrid>

                </div>
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />        
        </Triggers>--%>
    </asp:UpdatePanel>

</asp:Content>

