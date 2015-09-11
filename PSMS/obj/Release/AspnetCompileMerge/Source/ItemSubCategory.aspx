<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="ItemSubCategory.aspx.cs" Inherits="PSMS.ItemSubCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>Sub Category</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="settingsForm">
                <div class="baseFormTitle"><b>MANAGE</b> ITEM SUB CATEGORY</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft">
                    <telerik:RadListBox runat="server" ID="rlbItems" Height="100%" Width="100%" AutoPostBack="True" 
                        OnSelectedIndexChanged="rlbItems_SelectedIndexChanged" Skin="Silk">
                    </telerik:RadListBox>
                </div>
                <div class="settingsFormBodyRight">
                    <table class="settingsEntryTable">
                        <tr>
                            <td>Category Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="cbxCat" runat="server" AutoPostBack="True"
                                     OnSelectedIndexChanged="cbxCat_SelectedIndexChanged1" Width="100%" Skin="Silk" Sort="Ascending">                                    
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Subcategory Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxName" runat="server" MaxLength="100" Width="100%" Skin="Silk" /></td>
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
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Button ID="Button1" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnSave_Click" Text="Save" />
                                    <asp:Button ID="Button2" runat="server" BorderStyle="None" CssClass="buttonStyle" 
                                        OnClick="btnDelete_Click" 
                                        OnClientClick="if ( !confirm('Are you sure you want to delete this record?')) return false;"
                                        Text="Delete" />
                                    <asp:Button ID="Button3" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click" Text="Reset" />
                                </td>
                            </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
