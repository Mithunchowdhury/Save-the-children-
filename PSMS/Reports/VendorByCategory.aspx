<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="VendorByCategory.aspx.cs" Inherits="PSMS.Reports.VendorByCategory" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="http://fonts.googleapis.com/css?family=Yanone+Kaffeesatz:400,700" rel="stylesheet" type="text/css" />
    <link href="../css/AppStyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../css/menu.css" />
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/AppScript.js"></script>
    <style type="text/css">
        .appView {
            width: 1150px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="baseForm" style="width: 1100px; height: auto;">
                <div class="baseFormTitle" style="height: 5%;"><b>REPORT</b> - VENDOR BY CATEGORY</div>
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" Visible="true" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
