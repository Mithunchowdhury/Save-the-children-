<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Framework.aspx.cs" Inherits="PSMS.Framework" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Framework</title> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAttachFile" />
        </Triggers>
        <ContentTemplate>
            <div class="baseForm" style="width: 900px; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> FRAMEWORK</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="baseFormBodyFlex" style="width: auto; height: auto;">
                    <div class="baseFormBodyFlex" style="height: 985px">
                        <table style="width: 100%;">
                            <tr style="align-items: baseline">
                                <td style="width: 33%;">Framework No</td>
                                <td style="width: 33%;">Category</td>
                                <td style="width: 33%;">Process Person</td>
                            </tr>
                            <tr style="align-items: baseline">
                                <td>
                                    <telerik:RadTextBox ID="tbxFWNo" runat="server" TabIndex="1" Skin="Silk" Width="100%"
                                        Enabled="false" ReadOnly="true">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="cbxCategoryType" runat="server" TabIndex="3" Skin="Silk" Width="100%"
                                        AutoPostBack="true" OnSelectedIndexChanged="cbxCategoryType_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="cbxProcessPerson" runat="server" TabIndex="3" Skin="Silk" Width="100%"
                                        Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr style="align-items: baseline">
                                <td>Vendor</td>
                                <td>Contract Start Date</td>
                                <td>Contract End Date</td>
                            </tr>
                            <tr style="align-items: baseline">
                                <td>
                                    <telerik:RadComboBox ID="cbxVendor" runat="server" TabIndex="3" Skin="Silk" Width="100%"></telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dtpFWStartDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy"
                                        Skin="Silk" Width="100%">
                                        <Calendar runat="server">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dtpFWSEndDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy"
                                        Skin="Silk" Width="100%">
                                        <Calendar runat="server">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr style="align-items: baseline">
                                <td>Sign Date</td>
                                <td>Active</td>
                                <td>Multiple Location</td>
                            </tr>
                            <tr style="align-items: baseline">
                                <td>
                                    <telerik:RadDatePicker ID="dtpFWSSignDate" runat="server" DateInput-DisplayDateFormat="dd/MM/yyyy"
                                        Skin="Silk" Width="100%">
                                        <Calendar runat="server">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Bisque" />
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkMultipleLocation" runat="server" Checked="True" />
                                </td>
                            </tr>
                        </table>
                        <div class="baseFormBodyFlex" style="height: 500px; margin-top: 5px;">
                            <div><b>Framework Item</b></div>
                            <table style="width: 100%;">
                                <tr style="align-items: baseline">
                                    <td style="width: 33%;">Sub Category</td>
                                    <td style="width: 33%;">Item</td>
                                    <td style="width: 33%;">Specification</td>
                                </tr>
                                <tr style="align-items: baseline">
                                    <td>
                                        <telerik:RadComboBox ID="cbxSubCategories" runat="server" TabIndex="3" Skin="Silk" Width="100%"
                                            AutoPostBack="true" OnSelectedIndexChanged="cbxSubCategories_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cbxItems" runat="server" TabIndex="3" Skin="Silk" Width="100%"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxSpecification" runat="server" TabIndex="1" Skin="Silk" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr style="align-items: baseline">
                                    <td>Unit</td>
                                    <td>Quantity</td>
                                    <td>Price</td>
                                </tr>
                                <tr style="align-items: baseline">
                                    <td>
                                        <telerik:RadComboBox ID="cbxUnit" runat="server" TabIndex="3" Skin="Silk" Width="100%"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxQuantity" runat="server" TabIndex="1" Skin="Silk" Width="100%"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxPrice" runat="server" TabIndex="1" Skin="Silk" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: right; margin-top: 5px; padding-bottom: 5px;">
                                <asp:Button ID="btnItemSave" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Add" OnClick="btnItemSave_Click"></asp:Button>
                                <asp:Button ID="btnItemReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Clear" OnClick="btnItemReset_Click"></asp:Button>
                            </div>
                            <div class="baseFormBodyFlex" style="height: 300px">
                                <telerik:RadGrid ID="rgdFWItems" runat="server" AutoGenerateColumns="False"
                                    ClientSettings-Scrolling-AllowScroll="true" MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Height="98%"
                                    AllowSorting="True" CellSpacing="-1" GridLines="Both" Skin="Metro" OnItemCommand="rgdFWItems_ItemCommand">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                            ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="FWDID" BorderStyle="None">

                                        <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                        <FilterItemStyle BorderColor="White" />
                                        <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="FWDID" HeaderText="FWDID" ReadOnly="true" UniqueName="FWDID" Display="false">
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="FrameWorkID" HeaderText="FrameWorkID" ReadOnly="true" UniqueName="FrameWorkID" Display="false">
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>


                                            <telerik:GridBoundColumn DataField="ItemID" HeaderText="ItemID" UniqueName="ItemID" Display="false">
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item Name" UniqueName="ItemName">
                                                <HeaderStyle Width="15%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Specification" HeaderText="Specification" UniqueName="Specification">
                                                <HeaderStyle Width="15%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="UnitIn" HeaderText="UnitIn" UniqueName="UnitIn" Display="false">
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UnitName" HeaderText="Unit" UniqueName="UnitName">
                                                <HeaderStyle Width="15%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="PackSize" HeaderText="PackSize" UniqueName="PackSize">
                                                <HeaderStyle Width="15%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="Qty" HeaderText="Quantity" UniqueName="Qty">
                                                <HeaderStyle Width="10%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Price" HeaderText="Price" UniqueName="Price">
                                                <HeaderStyle Width="10%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="FWItemEdit" HeaderText="FWItemEdit" UniqueName="FWItemEdit" Display="false">
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="" CommandName="Edit" Text="Edit">
                                                <HeaderStyle Width="10%" />
                                            </telerik:GridButtonColumn>

                                            <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="" CommandName="Delete" Text="Delete">
                                                <HeaderStyle Width="10%" />
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                        </EditFormSettings>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>
                        <div class="baseFormBodyFlex" style="height: 250px; margin-top: 5px;">
                            <div><b>Attachment</b></div>
                            <table style="width: 100%;">
                                <tr style="align-items: baseline">
                                    <td>
                                        <%--<telerik:RadAsyncUpload runat="server" ID="asyncUploadAttachment" MultipleFileSelection="Automatic" Localization-Select="Browse..."
                                            HideFileInput="true" OnClientFileUploading="uploading_file" OnClientFilesUploaded="uploaded_file" />--%>
                                        <asp:FileUpload ID="UploadAttachment" runat="server" AllowMultiple="true" />
                                        <%--onchange="uploaded_file()"--%>
                                        <asp:Button ID="btnAttachFile" runat="server" Text="Upload" OnClick="btnAttachFile_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div class="baseFormBodyFlex" style="height: 175px">
                                <telerik:RadGrid ID="rgdAttachment" runat="server" AutoGenerateColumns="False"
                                    ClientSettings-Scrolling-AllowScroll="true" MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Height="98%"
                                    AllowSorting="True" CellSpacing="-1" GridLines="Both" PageSize="5" Skin="Metro" OnItemCommand="rgdAttachment_ItemCommand">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                            ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="FrameWorkID" BorderStyle="None">

                                        <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                        <FilterItemStyle BorderColor="White" />
                                        <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="FrameWorkID" HeaderText="FrameWorkID" ReadOnly="true" UniqueName="FrameWorkID" Display="false">
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="FrameWorkAttachLocation" HeaderText="FilePath" UniqueName="FrameWorkAttachLocation" Display="false">
                                                <HeaderStyle Width="30%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="FileName" HeaderText="File" UniqueName="FileName">
                                                <HeaderStyle Width="30%" />
                                                <ColumnValidationSettings>
                                                    <ModelErrorMessage Text="" />
                                                </ColumnValidationSettings>
                                            </telerik:GridBoundColumn>

                                            <telerik:GridTemplateColumn DataField="Note" HeaderText="Note" UniqueName="Note">
                                                <ItemStyle Width="50%" />
                                                <HeaderStyle Width="50%" />
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="txtAttachmentNote" Text='<%#Bind("Note") %>' runat="server"
                                                        Skin="Silk">
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="" CommandName="Delete" Text="Delete">
                                                <HeaderStyle Width="10%" />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="" CommandName="Download" Text="Download">
                                                <HeaderStyle Width="10%" />
                                            </telerik:GridButtonColumn>
                                            <%--<telerik:GridTemplateColumn>
                                                <HeaderStyle Width="10%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Download" 
                                                        Text='<%#Bind("FrameWorkAttachLocation") %>' CommandArgument='<%#Bind("FrameWorkAttachLocation") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>--%>
                                        </Columns>
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                        </EditFormSettings>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: right; margin-top: 5px; padding-bottom: 5px;">
                        <asp:Button ID="btnSave" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Save" OnClick="btnSave_Click"></asp:Button>
                        <asp:Button ID="btnReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                    </div>
                    <div class="baseFormBodyFlex" style="height: 350px">
                        <telerik:RadGrid ID="grdFWs" runat="server" AutoGenerateColumns="False"
                            ClientSettings-Scrolling-AllowScroll="true" MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Height="98%"
                            AllowPaging="True" AllowSorting="True" CellSpacing="-1" GridLines="Both" PageSize="5" Skin="Metro" OnItemCommand="grdFWs_ItemCommand">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                    ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="FrameWorkID" BorderStyle="None" AllowFilteringByColumn="true">

                                <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                <FilterItemStyle BorderColor="White" />
                                <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="FrameWorkID" HeaderText="FrameWorkID" ReadOnly="true" UniqueName="FrameWorkID" Display="false">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="FrameWorkNo" HeaderText="FrameWork No" UniqueName="FrameWorkNo">
                                        <HeaderStyle Width="15%" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category Name" UniqueName="CategoryName">
                                        <HeaderStyle Width="15%" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="VendorName">
                                        <HeaderStyle Width="15%" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PreparedPerson" HeaderText="Prepared By" UniqueName="PreparedPerson">
                                        <HeaderStyle Width="15%" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="Attachment" CommandName="Download" Text="Download">
                                        <HeaderStyle Width="10%" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date" UniqueName="StartDate" AllowFiltering="false">
                                        <HeaderStyle Width="10%" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date" UniqueName="EndDate" AllowFiltering="false">
                                        <HeaderStyle Width="10%" />
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn HeaderStyle-Width="50px" DataTextField="Action" HeaderText="Action"
                                        CommandName="Action" Display="false">
                                        <HeaderStyle Width="10%" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="" CommandName="Edit" Text="Edit">
                                        <HeaderStyle Width="10%" />
                                    </telerik:GridButtonColumn>
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
