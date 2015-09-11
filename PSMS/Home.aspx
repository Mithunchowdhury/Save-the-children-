<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PSMS.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Label ID="Label1" runat="server" Text="This is home page"></asp:Label>
        <p>
            <asp:Button ID="showButton" runat="server" OnClick="showButton_Click" Text="Show" />
        </p>
    </form>
</body>
</html>
