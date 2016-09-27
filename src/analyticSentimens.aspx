<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeBehind="analyticSentimens.aspx.cs" Inherits="WebApplication1.analyticSentimens" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript" src="js/libs/jquery.js"></script>
    <script type="text/javascript" src="js/libs/handlebars.js"></script>
    <script type="text/javascript" src="js/libs/angular.js"></script>
    <script type="text/javascript" src="js/libs/uikit.js"></script>
    <style type="text/css">
        .container
        {
            margin-left: 10px;
            margin-right: 10px;
        }
    </style>
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

    <form id="form1">
    <div id="divControl" runat="server" class="container">
        <h2>
            Sample Analytics</h2> <hr />
        <asp:Button ID="btnLoad" runat="server" Text="RetriveData" OnClick="btnLoad_Click" CssClass="inpBtn" /></div>
    <hr />
    <div id='pnlgrid' class="container">
        <div id="pnl3" style="width: 70%; float: left;">
            <asp:GridView ID="gvListData" runat="server" EmptyDataText="Tidak ada data" AutoGenerateColumns="false"
                AllowPaging="true" OnPageIndexChanging="gvListData_PageIndexChange" >
                <HeaderStyle BackColor="#3498DB" Font-Bold="true" ForeColor="White" Height="28px" />
                <PagerStyle BackColor="DeepSkyBlue" HorizontalAlign="Right" ForeColor="White" />
                <Columns>
                    <%--<asp:BoundField DataField="GroupId" HeaderText="CodeID" ItemStyle-HorizontalAlign=Center HeaderStyle-Width="5%" />--%>
                    <asp:BoundField DataField="docid" HeaderText="Document ID " ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="22%" />
                    <asp:BoundField DataField="sourceDescs" HeaderText="Source Descs" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="42%" />
                    <asp:BoundField DataField="positive" HeaderText="Positive" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="8%" />
                    <asp:BoundField DataField="negative" HeaderText="Negative" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="8%" />
                    <asp:BoundField DataField="result" HeaderText="Result" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="10%" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </form>
</asp:Content>
