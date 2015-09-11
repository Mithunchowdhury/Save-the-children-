<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Invitation.aspx.cs" Inherits="PSMS.Invitation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Invitation</title>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: 900px; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> INVITATION INFO</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="" />
                <asp:HiddenField ID="hdfInvId" runat="server" Value="" />
                <asp:HiddenField ID="hdfAmendNo" runat="server" Value="" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 25%">Type</td>
                            <td style="width: 25%">Invitation No</td>
                            <td style="width: 25%">Date</td>
                            <td style="width: 25%">Item Category</td>
                        </tr>
                        <tr>
                            <td style="width: 25%">
                                <telerik:RadComboBox ID="cbxType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbxType_SelectedIndexChanged"
                                    Skin="Silk" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select Type" Value="" />
                                        <telerik:RadComboBoxItem Text="RFQ" Value="RFQ" />
                                        <telerik:RadComboBoxItem Text="RFP" Value="RFP" />
                                        <telerik:RadComboBoxItem Text="IFT" Value="IFT" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 25%">
                                <telerik:RadTextBox ID="txtInvitationNo" runat="server" ReadOnly="true" Skin="Silk" Width="100%"></telerik:RadTextBox>
                            </td>
                            <td style="width: 25%">
                                <telerik:RadDatePicker ID="dtpDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy"
                                    OnSelectedDateChanged="dtpDate_SelectedDateChanged" Skin="Silk" Width="100%" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td style="width: 25%">
                                <telerik:RadComboBox ID="cbxCategory" runat="server" AutoPostBack="true"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false"
                                    OnSelectedIndexChanged="cbxCategory_SelectedIndexChanged" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">PR Information</div>
                    <telerik:RadGrid ID="grdPRInfo" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        AllowPaging="False" AllowMultiRowSelection="true" GridLines="Both" Width="100%" Skin="Metro" Height="200px">
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
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <HeaderStyle Width="70px" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" Text="Select"
                                            AutoPostBack="True" OnCheckedChanged="chkHeaderSelect_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="PRNo" HeaderText="PR No">
                                    <HeaderStyle Width="60px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PRRefNo" HeaderText="PR Ref. No">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UnitName" HeaderText="Unit">
                                    <HeaderStyle Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="Qty" HeaderText="Quantity" UniqueName="Quantity">
                                      <HeaderStyle Width="80px" />
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtQty" Text='<%#Bind("Qty") %>' runat="server" Skin="Silk"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Specification" HeaderText="Specification" UniqueName="Specification">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtSpecification" Text='<%#Bind("Specification") %>' runat="server" TextMode="MultiLine" Skin="Silk" Resize="Vertical" Height="30px" ></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="PRItemID" HeaderText="PRItemID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemID" HeaderText="ItemID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UnitID" HeaderText="UnitID" Display="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Vendor Information</div>
                    <telerik:RadGrid ID="grdVendorInfo" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        AllowPaging="False" AllowMultiRowSelection="true" GridLines="Both" Width="100%" Skin="Metro" Height="150px">
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
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                    <HeaderStyle Width="70px" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" Text="Select"
                                            AutoPostBack="True" OnCheckedChanged="chkHeaderSelect_CheckedChanged1" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged1" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code">
                                    <HeaderStyle Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorType" HeaderText="Vendor Type" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Address" HeaderText="Address" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="ContactPerson" HeaderText="Contact Person" UniqueName="ContactPerson" Display="false">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtContactPerson" Text='<%#Bind("ContactPerson") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="ContactPhone" HeaderText="Contact Phone" UniqueName="Phone" Display="false">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtContactPhone" Text='<%#Bind("Phone") %>' runat="server"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Email" HeaderText="Email" UniqueName="Email">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtEmail" Text='<%#Bind("Email") %>' runat="server" Skin="Silk"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="VendorID" HeaderText="VendorID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsEmailSend" HeaderText="IsEmailSend" Display="false">
                                </telerik:GridBoundColumn>

                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>

                    <table style="width: 100%">
                        <tr>
                            <td>Email To (CC)</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtCCEmailID" runat="server" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td>Email Body</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtEmailBody" runat="server" TextMode="MultiLine" Rows="2" Width="100%" Skin="Silk" Height="150px" />
                            </td>
                        </tr>
                    </table>

                </div>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Invitation Letter</div>
                    <table style="width: 100%">
                        <tr>
                            <td colspan="4">Title</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtSubject" runat="server" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Letter Body</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadTextBox ID="txtBody" runat="server" TextMode="MultiLine" Rows="2" Resize="Vertical" Skin="Silk" Height="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%">Last Date of Bidding</td>
                            <td style="width: 25%">Within</td>
                            <td style="width: 25%">Delivery Date</td>
                            <td style="width: 25%">Delivery Place</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadDatePicker ID="dtpLastBidDate" DateInput-DisplayDateFormat="dd/MM/yyyy" runat="server" Width="100%" Skin="Silk" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="tpWithin" runat="server" Width="100%" Skin="Silk" MinDate="1900-01-01">
                                    <Calendar EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                    </Calendar>
                                    <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                    <TimeView CellSpacing="-1">
                                    </TimeView>
                                    <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                    <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px">
                                        <EmptyMessageStyle Resize="None" />
                                        <ReadOnlyStyle Resize="None" />
                                        <FocusedStyle Resize="None" />
                                        <DisabledStyle Resize="None" />
                                        <InvalidStyle Resize="None" />
                                        <HoveredStyle Resize="None" />
                                        <EnabledStyle Resize="None" />
                                    </DateInput>
                                </telerik:RadTimePicker>

                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpDelivaryDate" DateInput-DisplayDateFormat="dd/MM/yyyy" runat="server" Width="100%" Skin="Silk" MinDate="1900-01-01">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDeliveryPlace" runat="server" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="vertical-align: top; width: 25%;">
                                <asp:CheckBox runat="server" ID="chkMultilocation" Text="Multilocation" />
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
                        <tr style="align-items: baseline; width: 100%;">
                            <td>
                                <telerik:RadGrid ID="grdAttachment" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                                    AllowPaging="False" AllowMultiRowSelection="false" GridLines="Both" Width="100%" OnItemCommand="grdAttachment_ItemCommand">
                                    <MasterTableView DataKeyNames="FilePath">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="InvitationID" HeaderText="InvitationID" Display="false">
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
                                <telerik:RadTextBox ID="txtAttachmentNote" runat="server" Skin="Silk" Width="100%" />
                            </td>
                        </tr>


                    </table>

                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">Approval</div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 25%">Checked By</td>
                            <td style="width: 25%">Checked Date</td>
                            <td style="width: 25%"></td>
                            <td style="width: 25%">Note</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <telerik:RadComboBox ID="cbxCheckedBy" runat="server" AutoPostBack="true" Height="200" Width="100%"
                                    Filter="Contains" MarkFirstMatch="true" ChangeTextOnKeyBoardNavigation="false" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                            <td style="vertical-align: top;">
                                <telerik:RadTextBox ID="txtCheckedDate" runat="server" Width="100%" Skin="Silk" ReadOnly="true" />
                                <%--<telerik:RadDatePicker ID="dtpCheckedDate" DateInput-DisplayDateFormat="dd/MM/yyyy" runat="server" Width="100%" Skin="Silk">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>
                                    </Calendar>
                                </telerik:RadDatePicker>--%>
                            </td>
                            <td style="vertical-align: top;">
                                <asp:RadioButtonList runat="server" ID="rdbListAR" AutoPostBack="true" Width="100%" RepeatDirection="Horizontal" Skin="Silk" OnSelectedIndexChanged="rdbListAR_SelectedIndexChanged">
                                    <asp:ListItem Text="Verify" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Reject" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="vertical-align: top;">
                                <telerik:RadTextBox ID="txtApprovalNote" runat="server" TextMode="MultiLine" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right">
                                <asp:Button ID="btnApprovalSend" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" Text="Send" OnClick="btnApprovalSend_Click"></asp:Button>
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
                                    OnItemCheck="lbxTC_ItemCheck" OnCheckAllCheck="lbxTC_CheckAllCheck" Height="200px" Width="100%" Skin="Silk">
                                </telerik:RadListBox>
                            </td>
                            <td style="width: 3%; text-align: center;">
                                <telerik:RadButton ID="btnTransferForm" runat="server" Text=">" Width="100%" Font-Bold="true" Enabled="false"
                                    OnClick="btnTransferForm_Click" Skin="Silk">
                                </telerik:RadButton>
                            </td>
                            <td style="width: 2%; text-align: center;"></td>
                            <td style="width: 65%; text-align: right; margin-left: 10px;">
                                <telerik:RadListBox runat="server" CheckBoxes="true" ID="lbxSTC" Width="100%" Height="200px"
                                    AllowReorder="true" ButtonSettings-AreaWidth="35px" AutoPostBack="true" CssClass="radListBoxLeftAligned"
                                    AllowDelete="true" SelectionMode="Single" Skin="Silk" OnItemCheck="lbxSTC_ItemCheck">
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
                                <asp:Button ID="btnUpdateTC" runat="server" BorderStyle="None" CssClass="buttonStyle" Text="Add/Update"
                                    Enabled="true" OnClick="btnUpdateTC_Click"></asp:Button>
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
                <div id="divIFT" runat="server" class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">For IFT</div>
                    <table style="width: 100%">
                        <tr>
                            <td>Publish Place</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtPublishPlace" runat="server" TextMode="MultiLine" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div style="text-align: right; clear: both; width: 100%;">
                        <asp:Button ID="btnPreview" runat="server" BorderStyle="None" CssClass="buttonStyle" Text="Preview" OnClick="btnPreview_Click"></asp:Button>
                        <asp:Button ID="btnInvSend" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" Text="Send" OnClick="btnInvSend_Click"></asp:Button>
                        <asp:Button ID="btnSave" runat="server" BorderStyle="None" alt="ajax" CssClass="buttonStyle" Text="Save" OnClick="btnSave_Click"></asp:Button>
                        <asp:Button ID="btnReset" runat="server" BorderStyle="None" CssClass="buttonStyle" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                    </div>
                </div>
            </div>

        </ContentTemplate>
                  <Triggers>
            <asp:PostBackTrigger ControlID="grdAttachment" />
            <asp:PostBackTrigger ControlID="btnInvSend" />
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
