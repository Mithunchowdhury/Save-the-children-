<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="PRAssign.aspx.cs" Inherits="PSMS.PRAssign" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>PR Assign</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: auto; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> PR</div>              
                <asp:HiddenField ID="hdfId" runat="server" Value="" />
                <asp:HiddenField ID="hdfPriorityId" runat="server" Value="" />
                <asp:HiddenField ID="hdfDonorSpecification" runat="server" Value="" />
                <asp:HiddenField ID="hdfDeliveryMethod" runat="server" Value="" />
                <asp:HiddenField ID="hdfIsPlanned" runat="server" Value="" />
                <asp:HiddenField ID="hdfPlannedQuarter" runat="server" Value="" />
                <asp:HiddenField ID="hdfReceiverAddress" runat="server" Value="" />
                <asp:HiddenField ID="hdfRemarks" runat="server" Value="" />
                <asp:HiddenField ID="hdfRequistorId" runat="server" Value="" />

                <div id="PRSelect" runat="server" class="baseFormBodyFlex" style="width: 98%; margin-bottom: 5px;font-size:12px;">
                    PR <telerik:RadComboBox ID="cbxPR" runat="server" AutoPostBack="true" Width="30%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false"
                                    OnSelectedIndexChanged="cbxPR_SelectedIndexChanged" Skin="Silk">
                                </telerik:RadComboBox>
                </div>
                <div class="baseFormBodyFlex" style="width: 98%; margin-bottom: 5px;font-size:12px;">
                    <div class="cellPadThin">
                        <table style="width: 100%;">
                        <tr>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Ref. No</span><br /><asp:Label id="txtPRRefNo" runat="server" Text=""></asp:Label></td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">PR No</span><br />
                                <asp:Label ID="txtPRNo" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Receiver Name &amp; Email</span><br />
                                <asp:Label ID="txtReceiverName" runat="server" Text=""></asp:Label>,
                                <asp:Label ID="txtReceiverEmail" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Country</span><br />
                                <asp:Label ID="txtCountry" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Priority</span><br />
                                <asp:Label ID="txtPriority" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">PR Date</span><br />
                                <asp:Label ID="txtPRDate" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Receiver Location</span><br />
                                <asp:Label ID="txtReceiverLocation" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Delivery Method</span><br />
                                <asp:Label ID="txtDeliveryMethod" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Donor Name</span><br />
                                <asp:Label ID="txtDonorName" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Award End Date</span><br />
                                <asp:Label ID="txtAwardEndDate" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Goods Required Date</span><br />
                                <asp:Label ID="txtGoodsRequiredDate" runat="server" Text=""></asp:Label>
                            </td>
                            <td><span style="color:#3d3d3d;font-weight:bold;">Receiver Phone No</span><br />
                                <asp:Label ID="txtReceiverPhoneNo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                </div>
                <div class="baseFormBodyFlex" style="width: 98%; margin-bottom: 5px;">
                    <telerik:RadGrid ID="grdItemInfo" runat="server" AllowSorting="false" AutoGenerateColumns="false" ShowFooter="true"
                        AllowPaging="False" AllowMultiRowSelection="true" GridLines="Both" Width="100%" OnItemDataBound="grdItemInfo_ItemDataBound"
                        Skin="Metro" BorderStyle="None">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView BorderStyle="None">
                            <%--<HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <FooterStyle BorderStyle="None" BorderColor="White" />--%>
                            <Columns>
                                <telerik:GridBoundColumn DataField="SlNo" HeaderText="Sl. No"><HeaderStyle Width="5%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item Name"><HeaderStyle Width="12%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Specification" HeaderText="Specification">
                                    <HeaderStyle Width="35%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackSize" HeaderText="Pack Size"><HeaderStyle Width="6%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Qty" HeaderText="Quantity"><HeaderStyle Width="6%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UnitName" HeaderText="Unit"><HeaderStyle Width="6%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Amount" HeaderText="Unit Price" FooterText="Total:"><HeaderStyle Width="6%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TotalAmount" HeaderText="Total Price" Aggregate="Sum" FooterText=" "><HeaderStyle Width="6%" /></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CurrencyName" HeaderText="Currency"><HeaderStyle Width="6%" /></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <HeaderStyle Width="6%" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" Text="Select" OnCheckedChanged="chkHeaderSelect_CheckedChanged"
                                            AutoPostBack="True" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <HeaderStyle Width="6%" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderCancel" runat="server" OnCheckedChanged="chkHeaderCancel_CheckedChanged" Text="Cancel"
                                            AutoPostBack="True" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCancel" runat="server" OnCheckedChanged="chkCancel_CheckedChanged"
                                            AutoPostBack="True" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <HeaderStyle Width="6%" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderReject" runat="server" OnCheckedChanged="chkHeaderReject_CheckedChanged" Text="Reject"
                                            AutoPostBack="True" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkReject" runat="server" OnCheckedChanged="chkReject_CheckedChanged"
                                            AutoPostBack="True" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ProcessType" HeaderText="Process Type" Display="false">
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="cbxProcessType" runat="server" Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false">
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="PRItemID" HeaderText="PRItemID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemID" HeaderText="ItemID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SubCategoryID" HeaderText="SubCategoryID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UnitID" HeaderText="UnitID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CurrencyID" HeaderText="CurrencyID" Display="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <FooterStyle BorderColor="White"/>
                        </MasterTableView>

                    </telerik:RadGrid>
                </div>
                <div>
                    <div class="baseFormBodyFlex" style="width: 98%; margin-bottom: 5px;font-size:12px;float:left;width:25%;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <b>Requstor</b><br /><asp:Label ID="txtRequestorName" runat="server" Text=""></asp:Label>
                                ,
                                <asp:Label ID="txtRequestorPosition" runat="server" Text=""></asp:Label><br />
                                <asp:Label ID="txtRequestorDeptSector" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Label ID="txtRequestorDate" runat="server" Text=""></asp:Label>
                                <br />
                                <br />
                                <b>Program Manager</b><br />
                                <asp:Label ID="txtProgramManagerName" runat="server" Text=""></asp:Label>
                                ,
                                <asp:Label ID="txtProgramManagerPosition" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Label ID="txtProgramManagerDeptSector" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Label ID="txtProgramManagerDate" runat="server" Text=""></asp:Label>
                                <br />
                                <br />
                                <b>Logistics</b>
                                <br />
                                <asp:Label ID="txtLogisticsName" runat="server" Text=""></asp:Label>
                                ,
                                <asp:Label ID="txtLogisticsPosition" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Label ID="txtLogisticsDeptSector" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Label ID="txtLogisticsDate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                    <div class="baseFormBodyFlex" style="width: 98%; margin-bottom: 5px;float:right;width:70%;font-size:12px;">
                    <table style="width: 100%;">
                        <tr>
                            <td>Assign To</td>
                            <td>
                                Assign Date</td>
                            <td style="width:30px;">&nbsp;</td>
                            <td style="width:200px;">Note to Requestor</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxAssignTo" runat="server" AutoPostBack="true" ChangeTextOnKeyBoardNavigation="false" Filter="Contains" 
                                    MarkFirstMatch="true" Width="250" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAssignDate" runat="server" ReadOnly="true" Skin="Silk" />
                            </td>
                            <td rowspan="3">
                                &nbsp;</td>
                            <td rowspan="3">
                                <telerik:RadTextBox ID="txtPRRequestorNote" runat="server" Rows="3" TextMode="MultiLine" Height="120px" Skin="Silk"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Assign Note</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtAssignNote" runat="server" Rows="3" TextMode="MultiLine" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:right;">
                                <asp:RadioButton ID="chkSendEmail" runat="server" AutoPostBack="false" ButtonType="ToggleButton" Checked="true" Text="Send E-mail" ToggleType="CheckBox" Visible="False" />
                                <asp:Button ID="btnA1" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnA1_Click" Text="Attach 1"  />
                                <asp:Button ID="btnA2" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnA2_Click" Text="Attach 2"  />
                                <asp:Button ID="btnA3" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnA3_Click" Text="Attach 3" />
                                <asp:Button ID="btnPreview" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnPreview_Click" Text="Preview PR" />
                                <asp:Button ID="btnSave" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" OnClick="btnSave_Click" Text="Assign PR" />
                            </td>
                            <td>&nbsp;</td>
                            <td style="text-align:right;">
                                <asp:Button ID="btnSendMail" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" OnClick="btnSendMail_Click" Text="Send EMail Note" />
                            </td>
                        </tr>
                    </table>
                </div>
                </div>
                <div class="baseFormBodyFlex" style="width: 98%; margin-bottom: 5px;float:right;width:70%;">
                    <telerik:RadGrid ID="grdCharging" runat="server" AllowSorting="false"
                        AllowPaging="False" GridLines="None" Width="100%" Skin="Metro">
                    </telerik:RadGrid>
                </div>
                <div style="clear:both;"></div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
            <asp:PostBackTrigger ControlID="btnA1" />
            <asp:PostBackTrigger ControlID="btnA2" />
            <asp:PostBackTrigger ControlID="btnA3" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
