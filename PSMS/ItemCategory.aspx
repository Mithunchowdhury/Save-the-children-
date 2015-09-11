<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="ItemCategory.aspx.cs" Inherits="PSMS.ItemCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Category</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm">
                <div class="baseFormTitle"><b>MANAGE</b> ITEM CATEGORY</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft">
                    <telerik:RadListBox ID="rlbItems" runat="server" AutoPostBack="True" Height="100%"
                        OnSelectedIndexChanged="rlbItems_SelectedIndexChanged" Width="100%" Skin="Silk">
                    </telerik:RadListBox>
                </div>
                <div class="settingsFormBodyRight">
                    <table class="settingsEntryTable">
                        <tr>
                            <td>Category Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxName" runat="server" MaxLength="100" Width="100%" Skin="Silk" />
                            </td>
                        </tr>
                        <tr>
                            <td>Category Type</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxCatType" runat="server" Width="100%" Skin="Silk">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Goods" Value="Goods" />
                                        <telerik:RadComboBoxItem Text="Service" Value="Service" />
                                        <telerik:RadComboBoxItem Text="Work" Value="Work" />
                                        <telerik:RadComboBoxItem Text="" Value="0" />
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
                                    OnClientClick="if ( !confirm('Are you sure you want to delete this record?')) return false;"
                                    OnClick="btnDelete_Click" Text="Delete"></asp:Button>
                                <asp:Button ID="btnReset" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click" Text="Reset"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
