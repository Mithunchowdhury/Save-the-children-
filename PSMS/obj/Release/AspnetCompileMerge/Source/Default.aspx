<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PSMS.Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .nodeStyle1 {
            color: #93b45c;
            font-weight: normal;
            font-size: 12px;
            cursor:pointer;
        }

        .nodeStyle2 {
            color: #c25a51;
            font-weight: normal;
            cursor:pointer;
        }

        .nodeStyle3 {
            color: #4f84bf;
            font-weight: normal;
            cursor:pointer;
        }   

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="baseForm" style="width: 96%; height: 1350px;">
        <div class="baseFormTitle" style="height: 2%;"><b>KNOW</b> YOUR BUSINESS</div>

        <div style="padding-bottom: 10px;">
            <div class="baseFormBodyFlex" style="width: 35%; float: left; height: auto; margin-right: 10px; padding-bottom: 0px;">
                <div style="font-size: 12px; font-weight: bold;">PROCESS TYPE (HBAR)</div>
                <div style="margin-top: 5px; margin-left: 5px;">
                    <asp:Panel ID="HtmlChartHolderBarHorTopLeft" runat="server"></asp:Panel>
                </div>
            </div>

            <div class="baseFormBodyFlex" style="width: 34%; float: left; height: auto;">
                <div style="font-size: 12px; font-weight: bold;">PROCESS TYPE (PER YEAR)</div>
                <div style="margin-top: -10px;">
                    <asp:Panel ID="HtmlChartHolder" runat="server"></asp:Panel>
                </div>
            </div>
            <div class="baseFormBodyFlex" style="background-position: right top; width: 20%; height: 205px; float: right; 
                                            padding-bottom:10px; background-image: url('images/TL2.png'); background-repeat: no-repeat;">
                <div style="font-size: 12px; font-weight: bold;padding-bottom:20px;padding-left:5px;">MY PENDING TASK</div>
                <div style="margin-left:-5px;">
                    <telerik:RadTreeView ID="rtvPendingTask" runat="server" OnNodeClick="rtvPendingTask_NodeClick"></telerik:RadTreeView>
                </div>
            </div>
        </div>

        <div style="height: 10px; clear: both;"></div>

        <div>
            <div class="baseFormBodyFlex" style="width: 30%; float: left; height: auto; margin-right: 10px; padding-bottom: 20px;">
                <div style="font-size: 12px; font-weight: bold;">PROCESS TYPE (PIE)</div>
                <div style="margin-top: -20px; margin-left: -20px;">
                    <asp:Panel ID="HtmlChartHolderPie" runat="server"></asp:Panel>
                </div>
            </div>
            <div class="baseFormBodyFlex" style="width: 30%; float: left; height: auto; margin-right: 10px;">
                <div style="font-size: 12px; font-weight: bold;">PROCESS TYPE (BAR)</div>
                <div style="margin-top: -10px; margin-left: -10px;">
                    <asp:Panel ID="HtmlChartHolderBar" runat="server"></asp:Panel>
                </div>
            </div>
            <div class="baseFormBodyFlex" style="width: 30%; float: right; height: auto;">
                <div style="font-size: 12px; font-weight: bold;">PROCESS TYPE (LINE)</div>
                <div style="margin-top: -10px; margin-left: -10px;">
                    <asp:Panel ID="HtmlChartHolderLine" runat="server"></asp:Panel>
                </div>
            </div>
        </div>

        <div style="height: 10px; clear: both;"></div>

        <div>
            <div class="baseFormBodyFlex" style="width: 45%; float: left; height: auto; margin-right: 10px;">
                <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">PR/PO (Quartly)</div>
                <div style="margin-top: 0px; margin-left: 0px;">
                    <telerik:RadGrid ID="rgPRPOQTR" runat="server" Skin="Silk">
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="baseFormBodyFlex" style="width: 45%; float: right; height: auto;">
                <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">PR/PO (QUEATERLY)</div>
                <div style="margin-top: 0px; margin-left: 0px;">
                    <telerik:RadGrid ID="RadGrid1" runat="server" Skin="Silk">
                    </telerik:RadGrid>
                </div>
            </div>
        </div>

        <div style="height: 10px; clear: both;"></div>

        <div>
            <div class="baseFormBodyFlex" style="width: 45%; float: left; height: auto; margin-right: 10px;">
                <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">PR/PO (THEAM)</div>
                <div style="margin-top: 0px; margin-left: 0px;">
                    <telerik:RadGrid ID="RadGrid2" runat="server" Skin="Silk">
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="baseFormBodyFlex" style="width: 45%; float: right; height: auto;">
                <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">PR/PO (DEPR)</div>
                <div style="margin-top: 0px; margin-left: 0px;">
                    <telerik:RadGrid ID="RadGrid3" runat="server" Skin="Silk">
                    </telerik:RadGrid>
                </div>
            </div>
        </div>

        <div style="height: 10px; clear: both;"></div>

        <div>
            <div class="baseFormBodyFlex" style="width: 60%; float: left; height: auto; margin-right: 10px;">
                 <div style="font-size: 12px; font-weight: bold; padding-bottom: 5px;">LEAD TIME</div>
                <div style="margin-top: 0px; margin-left: 0px;">
                    <telerik:RadGrid ID="RadGrid4" runat="server" Skin="Silk">
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="baseFormBodyFlex" style="width: 30%; float: right; height: auto;">
                 <div style="font-size: 12px; font-weight: bold;">PROCESS TYPE (HBAR)</div>
                <div style="margin-top: 5px; margin-left: 5px;">
                    <asp:Panel ID="HtmlChartHolderBarHorBotomRight" runat="server"></asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div>
        <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" OnClientClose="OnClientClose"
                NavigateUrl="Dialog1.aspx" OnClientAutoSizeEnd="autoSizeWithCalendar">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow2" runat="server" Width="650" Height="480" Modal="true" NavigateUrl="Dialog2.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    </div>
</asp:Content>
