<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="js/libs/jquery.js"></script>
    <script type="text/javascript" src="js/libs/handlebars.js"></script>
    <script type="text/javascript" src="js/libs/angular.js"></script>
    <script type="text/javascript" src="js/libs/uikit.js"></script>
    <script type="text/javascript">

        var parentUrl = window.parent.location.pathname;
        var check = $('#divID', window.parent.document).context.cookie;

        function checkSession() {
            $.ajax({
                type: "POST",
                url: parentUrl + "/CheckSession",
                data: '{name: "test" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessSession,
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.status);
                    console.log(textStatus);
                    console.log(errorThrown);
                    if (jqXHR.status = '401') {
                        window.parent.location.reload();
                    }
                }
            });
        }

        function OnSuccessSession(response) {
            console.log(response.d);
        }

        $(document).ready(function () {
            //            checkSession();
        });

    </script>
    <style type="text/css">
        .container
        {
            width: 100%;
            font-size: 12px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        .container .contentChartLeft
        {
            height: 400px;
            width: 48%;
            float: left;
            padding: 5px 10px 10px 10px;
            padding: 10px;
        }
        
        .container .contentChartRight
        {
            height: 400px;
            width: 48%;
            float: right;
            padding: 5px 10px 10px 10px;
            padding: 10px;
        }
        
        .Info
        {
            width: 150px;
            margin-left: 100px;
            border: 1px solid#999;
            font-size: 12px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
    </style>
    <form id="form1">
    <div id="pnl1" class="container">
        <h2>
            Dashboard</h2>
        <hr />
        <div id="pnlLeft" class="contentChartLeft">
            <div id="pnlLeft1">
                <center>
                    <h2>
                        <p id="infoChart1" runat="server">
                    </h2>
                    </p>
                    <asp:Chart ID="cTestChart1" runat="server">
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" Font="Century Gothic" 
                                LegendStyle="Column"/>
                        </Legends>
                        <Series>
                            <asp:Series Name="Default" Font="Century Gothic" >
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                        </ChartAreas>
                    </asp:Chart>
                </center>
            </div>
            <div id="pnlLeft2" class="contentChartLeft">
                <br />
                <center>
                    <h2>
                        <p id="infoChart3" runat="server">
                    </h2>
                    </p>
                    <div id="chartInfo" class="Info">
                        <div id="infoDay1" runat="server" style="width: 45%; float: left; margin-left: 15px;">
                        </div>
                        <div id="infoDay2" runat="server" style="width: 45%; float: Right; margin-top: 12px;">
                        </div>
                        <div id="infoHour1" runat="server" style="width: 45%; float: left; margin-right: 10px;
                            text-align: right">
                        </div>
                        <div id="infoHour2" runat="server" style="width: 45%; float: Right; margin-top: 5px;">
                        </div>
                        <div id="infoMinute1" runat="server" style="width: 45%; float: left; margin-right: 10px;
                            text-align: right">
                        </div>
                        <div id="infoMinute2" runat="server" style="width: 45%; float: left;">
                        </div>
                    </div>
                </center>
            </div>
        </div>
        <div id="pnlRight" class="contentChartRight">
            <div id="pnlRight1">
                <center>
                    <div id="div-1" style="width: 50%; float: left">
                        <h2>
                            <p id="P1" runat="server">
                            </p>
                        </h2>
                        <asp:Chart ID="ChartDiv1" runat="server">
                            <Legends>
                                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" Font="Century Gothic" 
                                    LegendStyle="Column" />
                            </Legends>
                            <Series>
                                <asp:Series Name="Positive"  Font="Century Gothic"  >
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartAreaDiv1" BorderWidth="0" />
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </center>
                <div id="div-2" style="width: 50%; float: right">
                    <h2>
                        <p id="P2" runat="server">
                        </p>
                    </h2>
                    <asp:Chart ID="ChartDiv2" runat="server">
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" Font="Century Gothic" 
                                LegendStyle="Column" />
                        </Legends>
                        <Series>
                            <asp:Series Name="Negative" Font="Century Gothic" >
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartAreaDiv2" BorderWidth="0" BackColor="DarkBlue" />
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
            <div id="pnlRight2" class="contentChartRight" style="width: 100%; margin: auto">
                <h2>
                    <p id="infoChart2" runat="server">
                    </p>
                </h2>
                <asp:Chart ID="cTestChart2" runat="server">
                    <Legends>
                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default"
                            LegendStyle="Column" Font="Century Gothic" />
                    </Legends>
                    <Series>
                        <asp:Series Name="Default"  Font="Century Gothic" >
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea2" BorderWidth="0" />
                    </ChartAreas>
                </asp:Chart>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
