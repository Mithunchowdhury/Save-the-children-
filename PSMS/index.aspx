<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="PSMS.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="UserLogin"></asp:Label>
        <asp:TextBox ID="nameTextBox" runat="server"></asp:TextBox>
    <div>
    
        <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
        <asp:TextBox ID="passwordTextBox" runat="server"></asp:TextBox>
    
    </div>
        <asp:Button ID="saveButton" runat="server" OnClick="saveButton_Click" Text="save" />
        <p>
            <asp:Label ID="msgLabel" runat="server" Text="Label"></asp:Label>
        </p>
    </form>
</body>
</html>
