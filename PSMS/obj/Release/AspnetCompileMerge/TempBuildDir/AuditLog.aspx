<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="PSMS.AuditLog" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Audit Log</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="baseForm" style="width: 900px; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> AUDIT LOG</div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto;">
                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 20%;">User</td>
                            <td style="width: 20%;">Screen</td>
                            <td style="width: 20%;">From</td>
                            <td style="width: 20%;">To</td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadComboBox ID="cbxUser" runat="server" TabIndex="1" Skin="Silk" Width="100%" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxScreen" runat="server" TabIndex="2" Skin="Silk" Width="100%" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadDateTimePicker ID="dprFrom" runat="server" TabIndex="3" Skin="Silk" Width="100%">
                                </telerik:RadDateTimePicker>
                            </td>
                            <td>
                                <telerik:RadDateTimePicker ID="dprTo" runat="server" TabIndex="3" Skin="Silk" Width="100%">
                                </telerik:RadDateTimePicker>
                            </td>
                        </tr>                       
                    </table>
                    <div style="text-align: right; margin-top: 5px; padding-bottom: 5px;">
                        <asp:Button ID="btnSearch" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Search" OnClick="btnSearch_Click"></asp:Button>
                        <asp:Button ID="btnReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                    </div>
                    <div class="baseFormBodyFlex" style="height: 500px">
                        <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">Audit Log List</div>
                        <telerik:RadGrid ID="grdAuditLogs" runat="server" AutoGenerateColumns="False"
                            ClientSettings-Scrolling-AllowScroll="true" ClientSettings-Scrolling-ScrollHeight="200px"
                            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Height="90%"
                            AllowPaging="True" AllowSorting="True" CellSpacing="-1" GridLines="Both" PageSize="5" Skin="Metro" 
                            OnPageIndexChanged="grdAuditLogs_PageIndexChanged" OnPageSizeChanged="grdAuditLogs_PageSizeChanged" 
                            OnItemCommand="grdAuditLogs_ItemCommand">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                    ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="Id" BorderStyle="None" AllowFilteringByColumn="true">
                                
                                <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                <FilterItemStyle BorderColor="White" />
                                <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Id" HeaderText="Id" ReadOnly="true" UniqueName="Id" Display="false">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Date" UniqueName="CreatedDate">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="USER" HeaderText="USER" UniqueName="USER" DataType="System.String">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SCREEN" HeaderText="SCREEN" UniqueName="SCREEN">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TABLE" HeaderText="TABLE" UniqueName="TABLE">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="OPRETATION" HeaderText="OPRETATION" UniqueName="OPRETATION">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="REFFIELD" HeaderText="FIELD" UniqueName="REFFIELD">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="REFVALUE" HeaderText="VALUE" UniqueName="REFVALUE">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>

                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" UniqueName="EditCommandColumn1">
                                    </EditColumn>
                                </EditFormSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="RadDropDownList" BorderColor="White" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
