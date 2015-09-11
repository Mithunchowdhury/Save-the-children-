<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="GRNSCN.aspx.cs" Inherits="PSMS.GRNSCN" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>GRN/SCN</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="baseForm" style="width: auto; height: auto;">

                <div class="baseFormTitle"><b>MANAGE</b> GOODS RECEIVING NOTE (GRN)/ SERVICE COMPLETION NOTE (SCN)</div>

                <asp:HiddenField ID="hdfId" runat="server" Value="" />
                <asp:HiddenField ID="hdfVendorId" runat="server" Value="" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div class="baseFormBodyTitle">GRN/SCN Information</div>                    
                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Type</td>
                            <td style="width: 30%;">GRN/SCN No</td>
                            <td style="width: 30%;">Receive Date</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadComboBox ID="cbxType" runat="server" AutoPostBack="True" TabIndex="1" Skin="Silk" Width="100%" OnSelectedIndexChanged="cbxType_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select Type" Value="" />
                                        <telerik:RadComboBoxItem Text="GRN" Value="GRN" />
                                        <telerik:RadComboBoxItem Text="SCN" Value="SCN" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtGRNSCNNo" runat="server" TabIndex="2" MaxLength="50" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy" TabIndex="3" Skin="Silk" Width="100%" OnSelectedDateChanged="dtpDate_SelectedDateChanged">
                                    <Calendar runat="server">
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                        </SpecialDays>

                                    </Calendar>
                                </telerik:RadDatePicker>
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Ref.</td>
                            <td style="width: 30%;">PO No</td>
                            <td style="width: 30%;">Vendor</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtRef" runat="server" TabIndex="4" MaxLength="50" Skin="Silk" Width="100%" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxPONo" runat="server" AutoPostBack="True" TabIndex="5" Skin="Silk" Width="100%" OnSelectedIndexChanged="cbxPONo_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtVendor" runat="server" TabIndex="6" MaxLength="100" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">PR No</td>
                            <td style="width: 30%;">Requestor</td>
                            <td style="width: 30%;">Budget Source</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtPRNo" runat="server" TabIndex="7" MaxLength="50" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRequestor" runat="server" TabIndex="8" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBudgetSource" runat="server" TabIndex="9" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 30%;">Receive Location</td>
                            <td style="width: 30%;">Multi Location</td>

                        </tr>
                        <tr style="align-items: baseline">
                            <td>
                                <telerik:RadTextBox ID="txtReceiveLocation" runat="server" TabIndex="10" Skin="Silk" Width="100%" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkMultilocation" TabIndex="11" Skin="Silk" Width="100%" />
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
                                     AllowPaging="False" AllowMultiRowSelection="false" GridLines="Both" Width="100%" OnItemCommand="grdAttachment_ItemCommand" >
                                     <MasterTableView DataKeyNames="FilePath">
                                         <Columns>
                                             <telerik:GridBoundColumn DataField="GRNID" HeaderText="GRNID" Display="false">
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
                    <div class="baseFormBodyTitle">Item Info</div>
                    <telerik:RadGrid ID="grdItemInfo" runat="server" AllowSorting="false" AutoGenerateColumns="false"
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
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">                                  
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" Text="Select"
                                            AutoPostBack="True"  OnCheckedChanged="chkHeaderSelect_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                               
                                <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item Name">                                    
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Specification" HeaderText="Specification">                                    
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UnitName" HeaderText="Unit">                                    
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="POQty" HeaderText="PO Qty">                                    
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="ReceivedQty" HeaderText="Received Qty">                                    
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="ReceiveQty" HeaderText="Receive Quantity" UniqueName="Quantity">                                  
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtReceiveQty" Text='<%#Bind("ReceiveQty") %>' runat="server" Width="200px"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Remarks" HeaderText="Remarks" UniqueName="Remarks">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarks" Text='<%#Bind("Remarks") %>' runat="server" Width="250px"></telerik:RadTextBox>
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
                    <div class="baseFormBodyTitle">Vendor Evaluation</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 20%;" runat="server" id="tdDT">Delivery Time</td>                            
                            <td style="width: 20%;" runat="server" id="tdGQ">Goods Quality</td>
                            <td style="width: 20%;" runat="server" id="tdP">Presentable</td>
                            <td style="width: 20%;"></td>
                        </tr>
                        <tr style="align-items: baseline">
                             <td>
                                <telerik:RadComboBox ID="cbxDeliveryTime" runat="server" TabIndex="1" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>                           
                            <td>
                                <telerik:RadComboBox ID="cbxGoodsQuality" runat="server" TabIndex="3" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cbxPresentable" runat="server" TabIndex="4" Skin="Silk" Width="100%">
                                </telerik:RadComboBox>
                            </td>

                        </tr>


                    </table>

                </div>
                
                 <div class="baseFormBodyFlex" style="width: auto; height: auto;">
                    <div class="baseFormBodyTitle">Received and Checked By</div>

                    <table style="width: 100%;">
                        <tr style="align-items: baseline">
                            <td style="width: 20%;">Quality/Logistic/Procurement</td>
                            <td style="width: 20%;">Designation</td>
                            <td style="width: 20%;">Date</td>
                             <td style="width: 20%;"></td>
                        </tr>
                        <tr style="align-items: baseline">
                             <td>
                                <asp:Label ID="lblPreparedBy" runat="server" Visible="false"></asp:Label>
                                <telerik:RadTextBox ID="txtPreparedBy" runat="server" TabIndex="1" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                               <telerik:RadTextBox ID="txtPreparedByDesignation" runat="server" TabIndex="2" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                               <telerik:RadTextBox ID="txtPreparedDate" runat="server" TabIndex="3" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                           

                        </tr>
                        <tr style="align-items: baseline">
                            <td style="width: 33%;">Program/Requestor/Store</td>
                            <td style="width: 45%;">Designation</td>
                            <td style="width: 22%;">Date</td>                             
                        </tr>
                        <tr style="align-items: baseline">
                            <td>                                
                                <telerik:RadComboBox ID="cbxCheckedBy" runat="server" TabIndex="4" Skin="Silk" 
                                    Width="100%" AutoPostBack="true" OnSelectedIndexChanged="cbxCheckedBy_SelectedIndexChanged"
                                    Filter="Contains" MarkFirstMatch="true" >
                                </telerik:RadComboBox>                               
                            </td>
                            <td>
                               <telerik:RadTextBox ID="txtCheckedByDesignation" runat="server" TabIndex="5" Skin="Silk" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                               <telerik:RadTextBox ID="txtCheckedDate" runat="server" TabIndex="6" Skin="Silk" Width="100%" ReadOnly="true" />
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
                        <td>GRN Complete</td>
                    </tr>
                    <tr style="align-items: baseline">
                        <td>
                            <asp:CheckBox runat="server" ID="chkGRNComplete" Skin="Silk" />
                        </td>
                    </tr>
                </table>

                <div class="baseFormBodyFlex" style="width: auto; height: auto; margin-bottom: 5px;">
                    <div style="text-align: right;">
                        <%--<asp:Button ID="btnGRNComplete" runat="server" Text="Complete GRN" BorderStyle="None" CssClass="buttonStyle" OnClick="btnGRNComplete_Click" ></asp:Button>--%>
                        <asp:Button ID="btnSave" runat="server" Text="Save" BorderStyle="None" CssClass="buttonStyle" OnClick="btnSave_Click" ></asp:Button>
                        <asp:Button ID="btnPreview" runat="server" Text="Preview" BorderStyle="None" CssClass="buttonStyle" OnClick="btnPreview_Click" ></asp:Button>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click"></asp:Button>

                    </div>
                </div>



            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
