<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="FWT.aspx.cs" Inherits="PSMS.FWT" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var iDiv; var innerDiv2; var isCreated;
        function showMessage(code, msg, msgType) {
            if (!isCreated) {
                iDiv = document.createElement('div');
                iDiv.id = 'divErrorMsg';
                iDiv.className = 'errorBox';
                iDiv.innerHTML = '<b> ERROR - ' + code + ' : </b><br />' + msg;
                document.getElementsByTagName('body')[0].appendChild(iDiv);
                if (msgType == 0)
                    iDiv.setAttribute("style", "width:" + (screen.width - 20) + "px;background-color: red;");
                else
                    iDiv.setAttribute("style", "width:" + (screen.width - 20) + "px;background-color: green;");

                iDiv.style.display = 'block';

                innerDiv2 = document.createElement('div');
                innerDiv2.id = 'divErrorClose';
                innerDiv2.className = 'errorBoxRight';
                innerDiv2.innerText = 'X';
                innerDiv2.setAttribute("style", "height:30px");
                innerDiv2.style.visibility = 'visible';
                innerDiv2.addEventListener("click", hideMessage);
                iDiv.appendChild(innerDiv2);
                isCreated = true;
            }
        }

        function hideMessage() {
            var div = document.getElementById("divErrorMsg");
            div.parentNode.removeChild(div);
            isCreated = false;
        }

    </script>

    
    <style type="text/css">
        body {
            margin: 0;
        }

        .errorBox {
            color: white;
            background-color: red;
            font-size: 14px;
            padding: 5px;
            border: 5px solid #c00;
            line-height: 15px;
            position: fixed;
            height: 40px;
            bottom: 0;
            left: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .errorBoxRight {
            float: right;
            margin-right: 4px;
            text-align: center;
            font: bold 30px arial;
            margin-top: -14px;
            cursor: pointer;
            border: 4px solid white;
            width: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>

            <div style="width: 100%">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Connect To DB"></asp:Label>
                            <br />
                        </td>

                        <td>
                            <asp:Button ID="Button2" runat="server" Text="Insert Test" OnClick="Button2_Click" />
                        </td>
                        <td>
                            <asp:Button ID="Button3" runat="server" Text="Update Test" OnClick="Button3_Click" />
                        </td>
                        <td>
                            <asp:Button ID="Button4" runat="server" Text="Delete Test" OnClick="Button4_Click" />
                        </td>


                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="Button5" runat="server" Text="Read Test" OnClick="Button5_Click" />
                        </td>
                        <td>
                            <asp:Button ID="Button22" runat="server" Text="Error Test" OnClick="Button22_Click" />
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" alt="ajax" Text="Alert Test" OnClick="Button1_Click" />
                        </td>
                        <td>
                            <asp:Button ID="Button6" runat="server" Text="Cookie Test" OnClick="Button6_Click" />
                            <asp:Button ID="Button23" runat="server" OnClick="Button23_Click" Text="Button" />
                        </td>
                    </tr>
                </table>
                <a href="Docs/PO649_.pdf">Docs/PO649_.pdf</a>
                <a href="bin/POAttachment/PO649_.pdf">bin/POAttachment/PO649_.pdf</a>
                <telerik:RadGrid ID="grd" runat="server" AutoGenerateColumns="False" ClientSettings-Scrolling-AllowScroll="true" ClientSettings-Scrolling-ScrollHeight="300px" MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCommand="grd_ItemCommand" OnNeedDataSource="grd_NeedDataSource">
                    <ClientSettings>
                        <Scrolling AllowScroll="True" />
                        <Resizing AllowColumnResize="True" AllowResizeToFit="true" AllowRowResize="false" ClipCellContentOnResize="true" EnableRealTimeResize="false" ResizeGridOnColumnResize="false" />
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="DonorID">
                        <Columns>
                            <telerik:GridBoundColumn DataField="DonorID" Display="false" HeaderText="ID" ReadOnly="true" UniqueName="DonorID">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Active" Display="false" HeaderText="Active" ReadOnly="true" UniqueName="Active">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DonorName" HeaderText="Name" UniqueName="DonorName">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MinAmount" HeaderText="Min. Amount" UniqueName="MinAmount">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridEditCommandColumn EditText="Edit" HeaderStyle-Width="50px" HeaderText="Edit" />
                            <telerik:GridButtonColumn CommandName="Delete" HeaderStyle-Width="50px" HeaderText="Delete" Text="Delete" UniqueName="DeleteColumn" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
