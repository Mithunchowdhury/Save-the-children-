<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="VendorSelection.aspx.cs" Inherits="PSMS.VendorSelection" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Selection</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: auto; height: auto;">

                <div class="baseFormTitle"><b>MANAGE</b> VENDOR SELECTION</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2">PR Ref. No</td>
                            <td colspan="2">Ref. No</td>
                            <td colspan="2">Selection Date</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadComboBox ID="cbxPR" runat="server" AutoPostBack="true" Height="200" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false"
                                    OnSelectedIndexChanged="cbxPR_SelectedIndexChanged" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="cbxInvitation" runat="server" AutoPostBack="true" Height="200" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false"
                                    OnSelectedIndexChanged="cbxInvitation_SelectedIndexChanged" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2">
                                <telerik:RadDatePicker ID="dtpSelectionDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy"
                                    Width="100%" Skin="Silk" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>PR</td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <telerik:RadTextBox ID="txtPR" runat="server" Skin="Silk" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">Vendor</td>
                            <td colspan="2">Selection Process</td>
                            <td colspan="2">Framework No</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadComboBox ID="cbxVendor" runat="server" AutoPostBack="true" Height="200" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="cbxSelectionProcess" runat="server" AutoPostBack="true" Height="200" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="cbxFrameWorkNo" runat="server" AutoPostBack="true" Height="200" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                        </tr>

                    </table>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Items</div>
                    <telerik:RadGrid ID="grdItemInfo" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        AllowPaging="False" AllowMultiRowSelection="true" GridLines="Both" Width="100%" Height="250px"
                        OnItemDataBound="grdItemInfo_ItemDataBound" Skin="Metro">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        </ClientSettings>
                        <MasterTableView BorderStyle="None">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="SlNo" HeaderText="Sl. No">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" Text="Select"
                                            AutoPostBack="True" OnCheckedChanged="chkHeaderSelect_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item Name">
                                    <ItemStyle Width="15%" />
                                    <HeaderStyle Width="15%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Specification" HeaderText="Specification">
                                    <ItemStyle Width="35%" />
                                    <HeaderStyle Width="35%" />
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn DataField="UnitID" UniqueName="UnitID" HeaderText="Unit">
                                    <ItemStyle Width="17%" />
                                    <HeaderStyle Width="17%" />
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="cbxUnit" runat="server" Filter="Contains"
                                            MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false" Skin="Silk" Width="100%">
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Qty" HeaderText="Quantity" UniqueName="Quantity">
                                    <ItemStyle Width="8%" />
                                    <HeaderStyle Width="8%" />
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtQty" Text='<%#Bind("Qty") %>' runat="server" AutoPostBack="true"
                                            OnTextChanged="txtQty_TextChanged" Skin="Silk" Width="100%">
                                        </telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="UnitPrice" HeaderText="Unit Price" UniqueName="UnitPrice">
                                    <ItemStyle Width="8%" />
                                    <HeaderStyle Width="8%" />
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtUnitPrice" Text='<%#Bind("UnitPrice") %>' runat="server" AutoPostBack="true"
                                            OnTextChanged="txtUnitPrice_TextChanged" Skin="Silk" Width="100%">
                                        </telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TotalPrice" HeaderText="Total Price" UniqueName="TotalPrice">
                                    <ItemStyle Width="7%" />
                                    <HeaderStyle Width="7%" />
                                </telerik:GridBoundColumn>


                                <telerik:GridBoundColumn DataField="PRItemID" HeaderText="PRItemID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemID" HeaderText="ItemID" Display="false">
                                </telerik:GridBoundColumn>

                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Vendors</div>
                    <telerik:RadGrid ID="grdVendorInfo" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        AllowPaging="False" AllowMultiRowSelection="true" GridLines="Both" Width="100%" Height="200px" Skin="Metro">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        </ClientSettings>
                        <MasterTableView BorderStyle="None">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="SlNo" HeaderText="Sl. No">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code">
                                    <HeaderStyle Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <HeaderStyle Width="80px" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderParticipate" runat="server" Text="Participate"
                                            AutoPostBack="True" OnCheckedChanged="chkHeaderParticipate_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkParticipate" runat="server" AutoPostBack="True" OnCheckedChanged="chkParticipate_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn DataField="BidPosition" HeaderText="Bid Position" UniqueName="BidPosition">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtBidPosition" Text='<%#Bind("BidPosition") %>' runat="server" Skin="Silk"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Note" HeaderText="Selection Note" UniqueName="SelectionNote">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtSelectionNote" Text='<%#Bind("Note") %>' runat="server" Skin="Silk"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="VendorID" HeaderText="VendorID" Display="false">
                                </telerik:GridBoundColumn>


                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <table style="width: 100%">
                    <tr style="align-items: baseline">
                        <td>Special Note</td>
                    </tr>
                    <tr style="align-items: baseline">
                        <td>
                            <telerik:RadTextBox ID="txtSpecialNote" runat="server" Skin="Silk" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>

                <table style="width: 100%">
                    <tr style="align-items: baseline">
                        <td>Committee Member</td>
                    </tr>
                    <tr style="align-items: baseline">
                        <td style="width: 100%;">                           
                            <telerik:RadAutoCompleteBox runat="server" ID="tbxCommitteeMember"
                                Skin="Silk" Width="100%" CssClass="racInputCustom" DropDownWidth="100%" DropDownHeight="200px">
                                <ClientDropDownItemTemplate>
                                        #= Text # <br /> #= Attributes.Designation #, #= Attributes.Dept #
                                </ClientDropDownItemTemplate>
                                <TokensSettings AllowTokenEditing="false" />
                                <WebServiceSettings Path="VendorSelection.aspx" Method="GetStaffInfo" />
                            </telerik:RadAutoCompleteBox>

                        </td>
                    </tr>
                </table>


                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px; margin-top: 5px;">
                    <div class="baseFormBodyTitle">Attachment</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadAsyncUpload runat="server" ID="asyncUploadSelectionFile" PostbackTriggers="btnSave" TemporaryFileExpiration="00:20:00" MultipleFileSelection="Automatic" Localization-Select="Browse..."
                                    HideFileInput="true" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadGrid ID="grdAttachment" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                                    AllowPaging="False" AllowMultiRowSelection="false" GridLines="Both" Width="100%" OnItemCommand="grdAttachment_ItemCommand">
                                    <MasterTableView DataKeyNames="FilePath">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="SelectionID" HeaderText="SelectionID" Display="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn DataTextField="FilePath" CommandName="Open" UniqueName="FilePath">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="Delete">
                                            </telerik:GridButtonColumn>
                                        </Columns>

                                    </MasterTableView>

                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Attachment Note</td>
                            <td style="width: 30%;"></td>
                            <td style="width: 30%;"></td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td>

                                <telerik:RadTextBox ID="txtAttachmentNote" runat="server" Skin="Silk" Width="100%" />

                            </td>
                        </tr>


                    </table>

                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px; margin-top: 5px;">
                    <table style="width: 50%">
                        <tr style="align-items: baseline">
                            <td></td>
                            <td>Order Date</td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 50%;">
                                <asp:CheckBox ID="chkPO" runat="server" AutoPostBack="True" Text="Forward for Purchase Order" Width="100%" />
                            </td>
                            <td style="width: 50%;">
                                <telerik:RadDatePicker ID="dtpPODate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy" Skin="Silk" Width="100%" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                    </table>
                </div>


                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px; margin-top: 5px;">
                    <div style="text-align: right;">
                        <asp:Button ID="btnSave" runat="server"  BorderStyle="None" alt="ajax" CssClass="buttonStyle" Text="Save" TabIndex="5" OnClick="btnSave_Click"></asp:Button>
                        <asp:Button ID="btnPreview" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Preview" TabIndex="6" OnClick="btnPreview_Click" Visible="false"></asp:Button>
                        <asp:Button ID="btnReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Reset" TabIndex="7" OnClick="btnReset_Click"></asp:Button>

                    </div>
                </div>


            </div>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="grdAttachment" />           
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>
