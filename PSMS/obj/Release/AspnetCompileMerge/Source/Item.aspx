<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="PSMS.Item" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>Item</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm">
                <div class="baseFormTitle"><b>MANAGE</b> ITEM INFO</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCatId" runat="server" Value="0" />
                <asp:HiddenField ID="hdfSubCatId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft">
                    <div style="height: 8%; width: 100%; padding-bottom: 5px;">
                        <telerik:RadTextBox ID="txtFilter" runat="server" MaxLength="300" Width="80%" Skin="Silk" />
                        <%--<asp:Button ID="btnItemSearch" runat="server" Width="30%"
                            BorderStyle="None" CssClass="buttonStyle" Text="Search" OnClick="btnItemSearch_Click"></asp:Button>--%>
                        <asp:ImageButton ID="btnSearch" runat="server" Width="25px" Height="25px" CssClass="searchButton"
                            AlternateText="Search" ImageAlign="Middle" ImageUrl="images/search.png" OnClick="btnSearch_Click" />
                    </div>
                    <div style="height: 90%; width: 100%;">
                        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="100%" OnNodeClick="RadTreeView1_NodeClick"
                            Width="100%" Skin="Silk">
                            <DataBindings>
                                <telerik:RadTreeNodeBinding Expanded="False" />
                            </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                </div>
                <div class="settingsFormBodyRight">
                    <table class="settingsEntryTable">
                        <tr>
                            <td style="width: 150px">Subcategory Name </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxSubCat" runat="server" Enabled="False" Width="100%" Skin="Silk">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Item Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxName" runat="server" MaxLength="300" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td>Item Alias</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxAlias" runat="server" MaxLength="10" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td>Item Type</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxItemType" runat="server" Width="100%" Skin="Silk">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="ASSET" Value="ASSET" />
                                        <telerik:RadComboBoxItem Text="NON ASSET" Value="NON ASSET" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Description</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxDescription" runat="server" MaxLength="100" Resize="None" TextMode="MultiLine" Width="100%" Skin="Silk">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Active</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkActive" runat="server" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Button ID="btnSave" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnSave_Click" Text="Save"></asp:Button>
                                <asp:Button ID="btnDelete" runat="server" BorderStyle="None" CssClass="buttonStyle"
                                    OnClick="btnDelete_Click" OnClientClick="if ( !confirm('Are you sure you want to delete this record?')) return false;" Text="Delete"></asp:Button>
                                <asp:Button ID="btnReset" runat="server" BorderStyle="None" CssClass="buttonStyle"
                                    OnClick="btnReset_Click" Text="Reset"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
