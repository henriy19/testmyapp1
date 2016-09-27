<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="managerules.aspx.cs" Inherits="WebApplication1.managerules" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript" src="js/libs/jquery.js"></script>
    <script type="text/javascript" src="js/libs/handlebars.js"></script>
    <script type="text/javascript" src="js/libs/angular.js"></script>
    <script type="text/javascript" src="js/libs/uikit.js"></script>

<script type="text/javascript">
    function visible() {
        document.getElementById('pnl1').style.visibility = 'visible';
//        document.getElementById('btnAdd').style.display = "none";
    }

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
//        checkSession();
    });

</script>

    <style type="text/css">
        .container
        {
            margin-left: 10px;
            margin-right: 10px;
        }
    </style>

<hr />
<form id="form1" >
<div id="pnl1"  class="container" >
<h2>Master Data - Manage Rule</h2> <hr />
<asp:Label ID="ids" runat="server" Text="" Width="40%" Visible="false"></asp:Label>
<table style="width: 1000px";>
     <tr>
        <td Width="20%">
            <asp:Label ID="variable1" runat="server" Text="Variable 1" ></asp:Label>
        </td>
        <td Width="30%">
            <asp:Label ID="conditions" runat="server" Text="Conditions" ></asp:Label>
        </td>
        <td Width="20%">
            <asp:Label ID="variable2" runat="server" Text="Variable 2" ></asp:Label>
        </td>
        <td Width="20%">
            <asp:Label ID="then" runat="server" Text="" ></asp:Label>
        </td>
        <td Width="10%">
            <asp:Label ID="result" runat="server" Text="Results" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlVar1" runat="server"  CssClass="inpDdl">
                <asp:ListItem Value="var1" Text="VAR-1"></asp:ListItem>
                <asp:ListItem Value="var2" Text="VAR-2"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td >
            <asp:DropDownList ID="ddlCondition" runat="server" CssClass="inpDdl" >
                <asp:ListItem Value="cond1" Text="greater than"></asp:ListItem>
                <asp:ListItem Value="cond2" Text="lesser than"></asp:ListItem>
                <asp:ListItem Value="cond3" Text="equal"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td >
            <asp:DropDownList ID="ddlvar2" runat="server" CssClass="inpDdl">
                <asp:ListItem Value="var1" Text="VAR-1"></asp:ListItem>
                <asp:ListItem Value="var2" Text="VAR-2"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Then" ></asp:Label>
        </td>
        <td >
            <asp:DropDownList ID="ddlResult" runat="server" CssClass="inpDdl">
                <asp:ListItem Value="rest1" Text="Positive"></asp:ListItem>
                <asp:ListItem Value="rest2" Text="Negative"></asp:ListItem>
                <asp:ListItem Value="rest3" Text="Neutral"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>

</table>
</div>
<hr />
<div id="pnl2"  class="container">
    <%--<asp:Button ID="btnAdd" runat="server" Text="Add" OnClientClick="visible(); return false;" CssClass="inpBtn"  />--%>
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="inpBtn" />
    <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="inpBtn" />--%>
    <div id="myForm"></div>
</div>
<hr />
<div id='pnlgrid'  class="container">
<div id="pnl3" style="width:70%; float:left;">
        <asp:GridView ID="gvListData" runat="server" EmptyDataText="Tidak ada data" AutoGenerateColumns="false" AllowPaging="true" 
        OnPageIndexChanging="gvListData_PageIndexChange" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="gvListData_SelectedIndexChanged">
            <HeaderStyle BackColor="#3498DB" Font-Bold="true" ForeColor="White" Height="28px"  />
            <PagerStyle BackColor="DeepSkyBlue" HorizontalAlign="Right" ForeColor="White" />
            <Columns>
                <asp:BoundField DataField="id" HeaderText="No" HeaderStyle-Width="5%" />
                <asp:BoundField DataField="variable1" HeaderText="Variable 1" HeaderStyle-Width="15%" />
                <asp:BoundField DataField="condition" HeaderText="Condition" HeaderStyle-Width="15%" />
                <asp:BoundField DataField="variable2" HeaderText="Variable 2" HeaderStyle-Width="15%" />
                <asp:BoundField DataField="result" HeaderText="Result" HeaderStyle-Width="15%" />
                <asp:BoundField DataField="descs" HeaderText="Description" HeaderStyle-Width="35%" />
                <asp:CommandField SelectText="Select" ShowSelectButton="true" Visible="true" />
            </Columns>
        </asp:GridView>
    
    </div>
<div id="pnl4" style="width:30%; float:right">
<div id="infogrid" style="width:95%; float:right">
    Keterangan
    <p>VAR-1 = Variable 1 (count Positif) </p>
    <p>VAR-1 = Variable 1 (count Positif) </p>
</div> 

</div>
</div>

<div >
</div>
</form>

</asp:Content>
