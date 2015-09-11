<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="PSMS.Vendor" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>Vendor</title>    
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="baseForm" style="width: 900px; height: auto;">
                <div class="baseFormTitle"><b>MANAGE</b> VENDOR INFO</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />

                <div class="baseFormBodyFlex" style="width: auto; height: auto;">
                    <telerik:RadTabStrip runat="server" ID="tabMain" SelectedIndex="0" MultiPageID="mpgPages" Skin="Silk">
                        <Tabs>
                            <telerik:RadTab Text="VENDOR" Font-Size="14px">
                            </telerik:RadTab>
                            <telerik:RadTab Text="CATEGORY" Font-Size="14px">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage runat="server" ID="mpgPages" SelectedIndex="0">
                        <telerik:RadPageView runat="server" ID="RadPageView1" Height="200px" BorderStyle="Solid" BorderColor="#afbfcf" BorderWidth="1px">
                            <table style="width: 100%;">
                                <tr style="align-items: baseline">
                                    <td style="width: 25%;">Vendor Code</td>
                                    <td style="width: 25%;">Address</td>
                                    <td style="width: 25%;text-align:right;"><asp:CheckBox ID="chkActive" runat="server" Checked="True" /></td>
                                    <td style="width: 25%;">Active</td>
                                    
                                </tr>
                                <tr style="align-items: baseline">
                                    <td>
                                        <telerik:RadTextBox ID="tbxCode" runat="server" TabIndex="1" MaxLength="10" Skin="Silk" Width="100%" Text="N/A" />
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="tbxAddress" runat="server" TabIndex="4" MaxLength="200" Resize="None" Skin="Silk" Width="100%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Vendor Name</td>
                                    <td>Postal Code</td>
                                    <td>City</td>
                                    <td>Country</td>

                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadTextBox ID="tbxName" runat="server" TabIndex="2" MaxLength="100" Skin="Silk" Width="100%" />
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxPostalCode" runat="server" TabIndex="5" MaxLength="50" Skin="Silk" Width="100%" />
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxCity" runat="server" TabIndex="6" MaxLength="50" Skin="Silk" Width="100%" />
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxCountry" runat="server" TabIndex="7" MaxLength="50" Skin="Silk" Width="100%" />
                                    </td>

                                </tr>
                                <tr>
                                    <td>Vendor Type</td>
                                    <td>Fax</td>
                                    <td>Web Address</td>
                                    <td>Email</td>

                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadComboBox ID="cbxVendorType" runat="server" TabIndex="3" Skin="Silk" Width="100%">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Listed" Value="Listed" />
                                                <telerik:RadComboBoxItem Text="Non Enlisted" Value="Non Enlisted" />
                                                <telerik:RadComboBoxItem Text="Delisted" Value="Delisted" />
                                                <telerik:RadComboBoxItem Text="Blacklisted" Value="Blacklisted" />
                                                <telerik:RadComboBoxItem Text="" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxFax" runat="server" TabIndex="8" MaxLength="50" Skin="Silk" Width="100%" />
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxWebsite" runat="server" TabIndex="9" MaxLength="50" Skin="Silk" Width="100%" />
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="tbxEmail" runat="server" TabIndex="10" MaxLength="50" Skin="Silk" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="RadPageView2" Height="200px" BorderStyle="Solid" BorderColor="#afbfcf" BorderWidth="1px">
                            <div style="font-size: 12px; font-weight: bold; padding: 3px;">Vendor Category List</div>
                            <telerik:RadGrid ID="grdVendorCategory" runat="server" AutoGenerateColumns="False" ClientSettings-Scrolling-AllowScroll="true"
                                ClientSettings-Scrolling-ScrollHeight="170px" GridLines="Both" Height="170px" BorderStyle="None"
                                MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Skin="Metro" OnItemDataBound="grdVendorCategory_ItemDataBound">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="True" AllowResizeToFit="true" AllowRowResize="false" ClipCellContentOnResize="true" EnableRealTimeResize="false" ResizeGridOnColumnResize="false" />
                                </ClientSettings>
                                <MasterTableView BorderStyle="None">
                                    <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                                    <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false"/>
                                    <Columns>
                                        <telerik:GridTemplateColumn DataField="IsChecked" HeaderText="Select" UniqueName="IsChecked">
                                            <HeaderStyle Width="5%" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelected" runat="server" Checked='<%#Bind("IsChecked") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CategoryID" Display="False" HeaderText="CategoryID" UniqueName="CategoryID">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Category" HeaderText="Category" UniqueName="Category">
                                            <HeaderStyle Width="35%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="ContactPerson" HeaderText="Contact Person" UniqueName="ContactPerson">
                                            <HeaderStyle Width="15%" />
                                            <ItemTemplate>
                                                <telerik:RadTextBox ID="tbxContactPerson" runat="server" MaxLength="50" Text='<%#Bind("ContactPerson") %>' Skin="Silk">
                                                </telerik:RadTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="ContactDesignation" HeaderText="Designation" UniqueName="ContactDesignation">
                                            <HeaderStyle Width="15%" />
                                            <ItemTemplate>
                                                <telerik:RadTextBox ID="tbxContactDesignation" runat="server" MaxLength="50" Text='<%#Bind("ContactDesignation") %>' Skin="Silk">
                                                </telerik:RadTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="ContactPhone" HeaderText="Phone" UniqueName="ContactPhone">
                                            <HeaderStyle Width="15%" />
                                            <ItemTemplate>
                                                <telerik:RadTextBox ID="tbxContactPhone" runat="server" MaxLength="50" Text='<%#Bind("ContactPhone") %>' Skin="Silk">
                                                </telerik:RadTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="ContactEmail" HeaderText="Email" UniqueName="ContactEmail">
                                            <HeaderStyle Width="15%" />
                                            <ItemTemplate>
                                                <telerik:RadTextBox ID="tbxContactEmail" runat="server" MaxLength="100" Text='<%#Bind("ContactEmail") %>' Skin="Silk">
                                                </telerik:RadTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                    <div style="text-align: right; margin-top: 5px; padding-bottom: 5px;">
                        <asp:Button ID="btnSave" runat="server"  BorderStyle="None" CssClass="buttonStyle" OnClick="btnSave_Click" Text="Save"></asp:Button>
                        <asp:Button ID="btnReset" runat="server"  BorderStyle="None" CssClass="buttonStyle" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                    </div>
                    <div class="baseFormBodyFlex" style="height: 250px">
                        <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">Vendor List</div>
                        <telerik:RadGrid ID="grdVendors" runat="server" AutoGenerateColumns="False"
                            ClientSettings-Scrolling-AllowScroll="true" ClientSettings-Scrolling-ScrollHeight="200px"
                            OnItemCommand="grdVendors_ItemCommand" OnNeedDataSource="grdVendors_NeedDataSource"
                            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" Height="90%"
                            AllowPaging="True" AllowSorting="True" CellSpacing="-1" GridLines="Both" PageSize="5" Skin="Metro">
                             <GroupingSettings CaseSensitive="false" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                    ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />                                
                            </ClientSettings>
                            <MasterTableView DataKeyNames="VendorID"  BorderStyle="None" AllowFilteringByColumn="true">
                                
                                <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true"/>
                                <FilterItemStyle BorderColor="White" />
                                <CommandItemSettings ShowAddNewRecordButton="False"  ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="VendorID" HeaderText="ID" ReadOnly="true" UniqueName="VendorID" Display="false">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="VendorCode" HeaderText="Vendor Code" UniqueName="VendorCode">                                        
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" UniqueName="VendorName">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VendorType" HeaderText="VendorType" ReadOnly="true" UniqueName="VendorType">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="Address" HeaderText="Address" ReadOnly="true" UniqueName="Address" Display="False">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PostCode" HeaderText="PostCode" ReadOnly="true" UniqueName="PostCode" Display="false">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="City" HeaderText="City" ReadOnly="true" UniqueName="City" Display="False">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ReadOnly="true" UniqueName="Country" Display="False">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Fax" HeaderText="Fax" ReadOnly="true" UniqueName="Fax" Display="False">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Email" HeaderText="Email" ReadOnly="true" UniqueName="Email">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="Category" HeaderText="Category" UniqueName="Category">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridEditCommandColumn HeaderStyle-Width="50px" HeaderText="Edit" EditText="Edit">
                                        <HeaderStyle Width="50px" />
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="Delete" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" ConfirmText="Delete this Record?">

                                        <HeaderStyle Width="50px" />
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
