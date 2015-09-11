<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="PSMS.Payment" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Payment</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: auto; height: auto;">

                <div class="baseFormTitle"><b>MANAGE</b> PAYMENT</div>

                <asp:HiddenField ID="hdfId" runat="server" Value="" />
                <asp:HiddenField ID="hdfVendorId" runat="server" Value="" />
                <asp:HiddenField ID="hdfVendorCode" runat="server" Value="" />
                <asp:HiddenField ID="hdfPRAmount" runat="server" Value="" />
                <asp:HiddenField ID="hdfPreparedById" runat="server" Value="" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Payment Information</div>
                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">PO No</td>
                            <td style="width: 30%;">PR No</td>
                            <td style="width: 30%;">GRN/SCN No</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadComboBox ID="cbxPONo" runat="server" AutoPostBack="True" TabIndex="1" Skin="Silk" Width="100%" OnSelectedIndexChanged="cbxPONo_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPRNo" runat="server" TabIndex="2" Skin="Silk" Width="100%" ReadOnly="true" />

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtGRNSCNNo" runat="server" TabIndex="3" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Vendor</td>
                            <td style="width: 30%;">Power Payment No</td>
                            <td style="width: 30%;">Payment Location</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtVendor" runat="server" TabIndex="4" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTrackingNo" runat="server" TabIndex="5" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxLocation" runat="server" TabIndex="6" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Bill No</td>
                            <td style="width: 30%;">Entry Date</td>
                            <td style="width: 30%;">Currency</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtBillNo" runat="server" TabIndex="7" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy" TabIndex="8" Skin="Silk" Width="100%" OnSelectedDateChanged="dtpDate_SelectedDateChanged" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>

                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxCurrency" runat="server" TabIndex="9" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Invoice No</td>
                            <td style="width: 30%;">Invoice Date</td>
                            <td style="width: 30%;">Invoice Total</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtInvoiceNo" runat="server" TabIndex="10" Skin="Silk" Width="100%" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpInvoiceDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy" TabIndex="11" Skin="Silk" Width="100%" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtInvoiceTotal" runat="server" TabIndex="12" Skin="Silk" Width="100%" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">PO Amount</td>
                            <td style="width: 30%;">Paid Amount</td>
                            <td style="width: 30%;">Payment Amount</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtPOAmount" runat="server" TabIndex="13" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPaidAmount" runat="server" TabIndex="14" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPaymentAmount" runat="server" TabIndex="15" Skin="Silk" Width="100%" AutoPostBack="true" OnTextChanged="txtPaymentAmount_TextChanged" />
                            </td>

                        </tr>

                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Payment Type</td>
                            <td style="width: 30%;">Invoice Received Date</td>
                            <td style="width: 30%;">Description</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadComboBox ID="cbxPaymentType" runat="server" TabIndex="16" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpInvoiceReceivedDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy" TabIndex="17" Skin="Silk" Width="100%" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDescription" runat="server" TabIndex="18" Skin="Silk" TextMode="MultiLine" Rows="1" Width="100%" />
                            </td>


                        </tr>


                    </table>

                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Payment Charges</div>
                    <telerik:RadGrid ID="grdPaymentCharges" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        AllowPaging="False" AllowMultiRowSelection="true" GridLines="Both" Width="100%" Skin="Metro" Height="100px">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        </ClientSettings>
                        <MasterTableView BorderStyle="None">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="SlNo" HeaderText="Sl. No">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="Narrative" HeaderText="Narrative" UniqueName="Narrative">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtNarrative" Text='<%#Bind("Narrative") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Account" HeaderText="Account" UniqueName="Account">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtAccount" Text='<%#Bind("Account") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="CostCenter" HeaderText="Cost Center" UniqueName="CostCenter">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtCostCenter" Text='<%#Bind("CostCenter") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Project" HeaderText="Project" UniqueName="Project">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtProject" Text='<%#Bind("Project") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="SOF" HeaderText="SOF" UniqueName="SOF">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtSOF" Text='<%#Bind("SOF") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="DEA" HeaderText="DEA" UniqueName="DEA">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtDEA" Text='<%#Bind("DEA") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Analysis" HeaderText="Analysis" UniqueName="Analysis">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtAnalysis" Text='<%#Bind("Analysis") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Amount" HeaderText="Amount" UniqueName="Amount">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtAmount" Text='<%#Bind("PartialAmount") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Attachment</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadAsyncUpload runat="server" ID="asyncUploadPaymentFile" PostbackTriggers="btnSave" 
                                    TemporaryFileExpiration="00:20:00" MultipleFileSelection="Automatic" Localization-Select="Browse..."
                                    HideFileInput="true" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadGrid ID="grdAttachment" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                                    AllowPaging="False" AllowMultiRowSelection="false" GridLines="Both" Width="100%" OnItemCommand="grdAttachment_ItemCommand">
                                    <MasterTableView DataKeyNames="FilePath">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="PId" HeaderText="PId" Display="false">
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
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Vendor Evaluation</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 20%;">Follow Up</td>
                            <td style="width: 20%;">Commitment</td>
                            <td style="width: 20%;"></td>
                            <td style="width: 20%;"></td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadComboBox ID="cbxFollowUp" runat="server" TabIndex="1" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxCommitment" runat="server" TabIndex="2" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>

                        </tr>


                    </table>

                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Approval</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 20%;">Prepared By</td>
                            <td style="width: 20%;">Designation</td>
                            <td style="width: 20%;">Authorized By</td>
                            <td style="width: 20%;">Designation</td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtPreparedBy" runat="server" TabIndex="1" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPreparedByDesignation" runat="server" TabIndex="2" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxAuthorizedBy" runat="server" TabIndex="3" Skin="Silk" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="cbxAuthorizedBy_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCheckedByDesignation" runat="server" TabIndex="4" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>

                        </tr>


                    </table>

                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Checklist</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 20%;">Invoice Checked</td>
                            <td style="width: 20%;">Correct Coding</td>
                            <td style="width: 20%;">Goods Service Received</td>
                            <td style="width: 20%;">Procurement Followed</td>
                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <asp:CheckBox runat="server" ID="chkInvoiceChecked" TabIndex="1" Skin="Silk" Width="100%" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkCorrectCoding" TabIndex="2" Skin="Silk" Width="100%" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkGoodsServiceReceived" TabIndex="3" Skin="Silk" Width="100%" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkProcurementFollowed" TabIndex="4" Skin="Silk" Width="100%" />
                            </td>

                        </tr>


                    </table>

                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto;">
                    <div class="baseFormBodyTitle">Payment Option</div>

                    <table>
                        <tr style="align-items: baseline">
                            <td>
                                <asp:RadioButtonList runat="server" ID="rdbListPaymentMethod" AutoPostBack="true"
                                    Width="100%" RepeatDirection="Horizontal" Skin="Silk">
                                    <asp:ListItem Text="Cheque" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Bank Transfer" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Cash" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>


                        </tr>



                    </table>

                </div>

                <table style="width: 100%">
                    <tr style="align-items: baseline">
                        <td>Remarks</td>
                    </tr>
                    <tr style="align-items: baseline">
                        <td>
                            <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr style="align-items: baseline">
                        <td>File Close</td>
                    </tr>
                    <tr style="align-items: baseline">
                        <td>
                            <asp:CheckBox runat="server" ID="chkFileClose" Skin="Silk" />
                        </td>
                    </tr>
                </table>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div style="text-align: right;">
                        <asp:Button ID="btnGRNPreview" runat="server" Text="GRN Preview" BorderStyle="None" CssClass="buttonStyle" OnClick="btnGRNPreview_Click"></asp:Button>
                        <asp:Button ID="btnSave" runat="server" Text="Save" BorderStyle="None" alt="ajax" CssClass="buttonStyle" OnClick="btnSave_Click"></asp:Button>
                        <asp:Button ID="btnForward" runat="server" alt="ajax" Text="Forward for Payment" BorderStyle="None" CssClass="buttonStyle" OnClick="btnForward_Click"></asp:Button>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click"></asp:Button>

                    </div>
                </div>



            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGRNPreview" />
            <asp:PostBackTrigger ControlID="grdAttachment" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
