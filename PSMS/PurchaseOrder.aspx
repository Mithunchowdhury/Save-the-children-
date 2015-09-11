<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs" Inherits="PSMS.PurchaseOrder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Purchase Order</title>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: 1000px; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> PURCHASE ORDER</div>
                
                <asp:HiddenField ID="hdfId" runat="server" Value="" />
                <asp:HiddenField ID="hdfAmendNo" runat="server" Value="" />
                <asp:HiddenField ID="hdfVendorId" runat="server" Value="" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Purchase Order Info</div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 25%;">PO No</td>
                            <td style="width: 25%;">Ref. No</td>
                            <td style="width: 25%;">Date</td>
                            <td style="width: 25%;"></td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <telerik:RadTextBox ID="txtPONo" runat="server" ReadOnly="true" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 25%;">
                                <telerik:RadComboBox ID="cbxRefNo" runat="server" AutoPostBack="true" ChangeTextOnKeyBoardNavigation="false"
                                    Filter="Contains" Height="200" MarkFirstMatch="true" OnSelectedIndexChanged="cbxRefNo_SelectedIndexChanged" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 25%;">
                                <telerik:RadDatePicker ID="dtpDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy"
                                    OnSelectedDateChanged="dtpDate_SelectedDateChanged" Skin="Silk" Width="100%" MinDate="1900-01-01">
                                    <Calendar ID="cdrDate" runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay ItemStyle-BackColor="Bisque" Repeatable="Today" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td style="width: 25%;"></td>
                        </tr>
                        <tr>
                            <td>Vendor</td>
                            <td colspan="2">Vendor Address</td>
                             <td>Delivery Address</td>                           
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtVendor" runat="server" ReadOnly="true" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtVendorAddress" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDeliveryAddress" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Attention</td>
                            <td>Cell No</td>
                            <td>Email</td>
                            
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtAttention" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCellNo" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>  
                            </td>
                            <td>
                                 <telerik:RadTextBox ID="txtEmail" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Currency</td>
                            <td>Conversion Rate</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxCurrency" runat="server" AutoPostBack="true"
                                    ChangeTextOnKeyBoardNavigation="false" Filter="Contains" Height="200" MarkFirstMatch="true" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCoversionRate" runat="server" Text="0" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="4">Subject</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtSubject" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;Email (CC)</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtCCEmail" runat="server" Skin="Silk" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Letter Body</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtPOBody" runat="server" Resize="Vertical" TextMode="MultiLine" Skin="Silk" Width="100%" MaxLength="500"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Email Body</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtEmailBody" runat="server" Resize="Vertical" TextMode="MultiLine" Skin="Silk" Width="100%" MaxLength="500" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Attachment</div>
                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td>                             
                                <telerik:RadAsyncUpload runat="server" ID="asyncUploadInvitationFile" PostbackTriggers="btnSave" TemporaryFileExpiration="00:20:00" MultipleFileSelection="Automatic" Localization-Select="Browse..."
                                    HideFileInput="true" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadGrid ID="grdAttachment" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                                    AllowPaging="False" AllowMultiRowSelection="false" GridLines="Both" Width="100%" OnItemCommand="grdAttachment_ItemCommand">
                                    <MasterTableView DataKeyNames="FilePath">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="POID" HeaderText="POID" Display="false">
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
                            <td style="">Attachment Note</td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtAttachmentNote" runat="server" Width="100%" />
                            </td>
                        </tr>


                    </table>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Purchase Item</div>
                    <telerik:RadGrid ID="grdPurchaseItem" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        AllowPaging="False" AllowMultiRowSelection="false" GridLines="Both" Width="100%" ShowFooter="true" Skin="Metro" Height="200px">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        </ClientSettings>
                        <MasterTableView BorderStyle="None">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <FooterStyle BorderStyle="None" BorderColor="White" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="SlNo" HeaderText="Sl. No">
                                    <ItemStyle Width="5%" />
                                    <HeaderStyle Width="5%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <ItemStyle Width="6%" />
                                    <HeaderStyle Width="6%" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" Text="Select"
                                            AutoPostBack="True" OnCheckedChanged="chkHeaderSelect_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item Name">
                                    <ItemStyle Width="14%" />
                                    <HeaderStyle Width="14%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RefNo" HeaderText="Ref. No">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="Specification" HeaderText="Specification" UniqueName="Specification">
                                    <ItemStyle Width="15%" />
                                    <HeaderStyle Width="15%" />
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtSpecification" Text='<%#Bind("Specification") %>' runat="server"
                                            Skin="Silk">
                                        </telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="UnitName" HeaderText="Unit">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Qty" HeaderText="Quantity">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="UnitPrice" HeaderText="Unit Price" UniqueName="UnitPrice" FooterText="Total:">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtUnitPrice" Text='<%#Bind("UnitPrice") %>' runat="server" Skin="Silk"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TotalPrice" HeaderText="Total Price" Aggregate="Sum" FooterText=" ">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="Note" HeaderText="Note" UniqueName="Note">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtNote" Text='<%#Bind("Note") %>' runat="server" Skin="Silk"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="UnitID" HeaderText="UnitID" Display="false">
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
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">VAT Amount</td>
                            <td style="width: 25%;">TAX Amount</td>
                            <td style="width: 25%;">Discount Amount</td>
                            <td style="width: 25%;">Service Charge Amount</td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                 <telerik:RadTextBox ID="txtVAT" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox> 
                            </td>
                            <td style="width: 25%;">
                                <telerik:RadTextBox ID="txtTAX" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox> 
                            </td>
                            <td style="width: 25%;">  
                                <telerik:RadTextBox ID="txtDiscount" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox> 
                            </td>
                            <td style="width: 25%;">  
                                <telerik:RadTextBox ID="txtServiceCharge" runat="server" Skin="Silk" Width="100%">
                                </telerik:RadTextBox> 
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Terms & Conditions</div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 30%; text-align: left; margin-right: 10px;">
                                <telerik:RadListBox ID="lbxTC" runat="server" CheckBoxes="true" ShowCheckAll="true" AutoPostBack="true"
                                    OnItemCheck="lbxTC_ItemCheck" OnCheckAllCheck="lbxTC_CheckAllCheck" Height="300px" Width="100%" Skin="Silk">
                                </telerik:RadListBox>
                            </td>
                            <td style="width: 3%; text-align: center;">
                                <telerik:RadButton ID="btnTransferForm" runat="server" Text=">" Width="100%" Font-Bold="true" Enabled="false"
                                    OnClick="btnTransferForm_Click" Skin="Silk">
                                </telerik:RadButton>
                            </td>
                            <td style="width: 2%; text-align: center;"></td>
                            <td style="width: 65%; text-align: right; margin-left: 10px;">
                                <telerik:RadListBox runat="server" ID="lbxSTC" Width="100%" CheckBoxes="true" Height="300px" AllowReorder="true" ButtonSettings-AreaWidth="35px"
                                    AllowDelete="true" SelectionMode="Multiple" Skin="Silk" CssClass="radListBoxLeftAligned" AutoPostBack="true" OnItemCheck="lbxSTC_ItemCheck">
                                </telerik:RadListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Add/Edit terms & conditions:</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtUpdateTC" runat="server" Width="100%" TextMode="MultiLine"
                                    Rows="2" Resize="Vertical" AutoPostBack="true" Skin="Silk" />
                            </td>

                        </tr>
                        <tr>
                            <td style="text-align: left;" colspan="4">
                                <asp:Button ID="btnUpdateTC" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Add/Update"
                                    Enabled="true" OnClick="btnUpdateTC_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <%--<table>
                        <tr>
                            <td style="width: 10%;">Add New</td>
                            <td style="width: 30%;">
                                <telerik:RadTextBox ID="txtAddNew" runat="server" Width="100%" Skin="Silk" />
                            </td>
                            <td style="width: 5%;">
                                <asp:Button ID="btnAddNew" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Add" Enabled="true" OnClick="btnAddNew_Click"></asp:Button>
                            </td>
                            <td style="width: 55%;"></td>
                        </tr>
                    </table>--%>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Approval</div>
                    <asp:Label ID="lblPreparedById" runat="server" Visible="false"></asp:Label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 33%;">Prepared By</td>
                            <td style="width: 34%;">Checked By</td>
                            <td style="width: 33%;">Approved By</td>
                        </tr>
                        <tr>
                            <td style="width: 33%;">
                                <asp:Label ID="lblPreparedBy" runat="server"></asp:Label>
                            </td>
                            <td style="width: 34%;">
                                <telerik:RadComboBox ID="cbxCheckedBy" runat="server" AutoPostBack="true" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false"
                                    OnSelectedIndexChanged="cbxCheckedBy_SelectedIndexChanged" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 33%;">  
                                <telerik:RadComboBox ID="cbxApprovedBy" runat="server" AutoPostBack="true" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false"
                                    OnSelectedIndexChanged="cbxApprovedBy_SelectedIndexChanged" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 33%;">
                                <asp:Label ID="lblPreparedByDesignation" runat="server"></asp:Label>
                            </td>
                            <td style="width: 34%;">
                                <asp:Label ID="lblCheckedByDesignation" runat="server"></asp:Label>
                            </td>
                            <td style="width: 33%;">
                                <asp:Label ID="lblApprovedByDesignation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 33%;"></td>
                            <td style="width: 34%;">Check Date</td>
                            <td style="width: 33%;">Approve Date</td>
                        </tr>
                        <tr>
                            <td style="width: 33%;"></td>
                             <td style="vertical-align: top;">
                                <telerik:RadTextBox ID="txtCheckedDate" runat="server" Width="100%" Skin="Silk" ReadOnly="true" />                               
                            </td>
                             <td style="vertical-align: top;">
                                <telerik:RadTextBox ID="txtApproveDate" runat="server" Width="100%" Skin="Silk" ReadOnly="true" />                               
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width: 33%;"></td>
                            <td style="width: 34%;">
                                <asp:RadioButtonList ID="rdbListAR" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdbListAR_SelectedIndexChanged" RepeatDirection="Horizontal" Skin="Silk" Width="100%">
                                    <asp:ListItem Text="Verify" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Reject" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="vertical-align: top;">
                                &nbsp;</td>

                        </tr>

                    </table>
                </div>


                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;">Mail Send Date</td>
                            <td style="width: 50%;">Note</td>
                        </tr>
                        <tr>
                            <td style="width: 50%; vertical-align:top;">
                                <telerik:RadTextBox ID="txtMailSendDate" runat="server" ReadOnly="true" Width="100%" Skin="Silk" />
                            </td>
                            <td style="width: 50%; vertical-align:top;">
                                <telerik:RadTextBox ID="txtPONote" runat="server" TextMode="MultiLine" Resize="Vertical" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr style="align-items: baseline">
                        <td>Amend</td>
                    </tr>
                    <tr style="align-items: baseline">
                        <td>
                            <asp:CheckBox runat="server" ID="chkAmend" Skin="Silk" />
                        </td>
                    </tr>
                    </table>
                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div style="text-align: right; clear: both; width: 100%;">
                        <%--<asp:Button ID="btnSendMail" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" Text="PO Mail Send" OnClick="btnSendMail_Click"/>--%>
                        <asp:Button ID="btnMailSend" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" Text="PO Mail Send" OnClick="btnMailSend_Click" ></asp:Button>
                        <asp:Button ID="btnSave" runat="server"  BorderStyle="None"  CssClass="buttonStyle" Text="Save" OnClick="btnSave_Click"></asp:Button>
                        <asp:Button ID="btnPreview" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Preview" OnClick="btnPreview_Click"></asp:Button>
                        <asp:Button ID="btnReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                    </div>
                </div>

                
            </div>
            
        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="grdAttachment" />
            <asp:PostBackTrigger ControlID="btnMailSend" />
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
