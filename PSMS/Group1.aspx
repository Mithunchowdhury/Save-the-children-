<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Group1.aspx.cs" Inherits="PSMS.Group1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <title>Group</title> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

 <ContentTemplate>
            <div class="settingsForm" style="width:800px;">
                <div class="baseFormTitle"><b>MANAGE</b> USER GROUP</div>
                <asp:HiddenField ID="hdfId" runat="server" Value="0" />
                <div class="settingsFormBodyLeft" style="width:55%;">
                    <telerik:RadGrid ID="grd" runat="server" AutoGenerateColumns="False"
                        ClientSettings-Scrolling-AllowScroll="true" ClientSettings-Scrolling-ScrollHeight="300px"
                        MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCommand="grd_ItemCommand" 
                        OnNeedDataSource="grd_NeedDataSource" Height="100%" Width="100%" >
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="GroupID" BorderStyle="None">
                            <HeaderStyle BackColor="#b0c0d0" ForeColor="Black" Font-Bold="true" />
                            <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="GroupID" HeaderText="ID" ReadOnly="true" UniqueName="GroupID" Display="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Active" HeaderText="Active" ReadOnly="true" UniqueName="Active" Display="false"></telerik:GridBoundColumn>


                                <telerik:GridBoundColumn DataField="GroupName" HeaderText="Name" UniqueName="GroupName">   
                                    <HeaderStyle Width="40%" />                                
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" UniqueName="Description">
                                    <HeaderStyle Width="40%" />
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ActiveStr" HeaderText="Active" UniqueName="ActiveStr">
                                    <HeaderStyle Width="50px" />
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn HeaderStyle-Width="50px" HeaderText="Edit" EditText="Edit">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn HeaderStyle-Width="50px" HeaderText="Delete"
                                    ConfirmText="Delete this Record?" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="50px" />
                                </telerik:GridButtonColumn>

                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <div class="settingsFormBodyRight" style="width:35%;">
                    <table class="settingsEntryTable">
                        <tr>
                            <td style="width: 120px">Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxName" runat="server" MaxLength="50" Width="100%"/>
                            </td>
                        </tr>
                        <tr>
                            <td>Description</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="tbxDescription" runat="server" MaxLength="50" Resize="None" TextMode="MultiLine" Width="100%" />
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
                                <asp:Button ID="btnReset" runat="server" BorderStyle="None" CssClass="buttonStyle" OnClick="btnReset_Click" Text="Reset"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>

 </asp:UpdatePanel>


    </asp:Content>







   







 




