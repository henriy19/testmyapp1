<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="process.aspx.cs" Inherits="WebApplication1.process" %>
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
    function paramshide(flag) {
        var xyz = "show";
        if (flag == xyz) {
            if (document.getElementById('pnlParams').style.visibility == 'hidden') {
                document.getElementById('pnlParams').style.visibility = 'visible';
            }
            else {
                document.getElementById('pnlParams').style.visibility = 'hidden';
            }
        }
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
<hr />
<div id="pnl1" class="container">
<h2>Process Crawling Data</h2> <hr />
<table style="width: 800px";>
    <tr>
        <td>
            <asp:Label ID="postget" runat="server" Text="Methods"></asp:Label>
        </td>
        <td style="width:40%">
            <asp:DropDownList ID="DropDownList1" runat="server" Width="80%" OnSelectedIndexChanged="methodsChanged"  CssClass="inpDdl" >
                <asp:ListItem Value="GET" Text="GET"></asp:ListItem>
                <asp:ListItem Value="POST" Text="POST"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="url" runat="server" Text="URL"></asp:Label>
        </td>
        <td style="width:40%">
            <asp:DropDownList ID="DropDownList2" runat="server" Width="80%" OnSelectedIndexChanged="intemChanged" AutoPostBack="true"  CssClass="inpDdl">
                <asp:ListItem Value="follower" Text="follower"></asp:ListItem>
                <asp:ListItem Value="timeline" Text="timeline"></asp:ListItem>
                <asp:ListItem Value="searching" Text="searching"></asp:ListItem>
                <asp:ListItem Value="retweet_by_me" Text="retweet_by_me"></asp:ListItem>
                <%--<asp:ListItem Value="friendsid" Text="friendsid"></asp:ListItem>--%>
                <asp:ListItem Value="friend_details" Text="friend_details"></asp:ListItem>
                <%--<asp:ListItem Value="update_profile" Text="update_profile"></asp:ListItem>--%>
                <asp:ListItem Value="posting_status" Text="posting_status"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="What happening?" Visible="false"></asp:Label>
        </td>
        <td style="width:40%">
            <asp:TextBox ID="tbAreaInput" TextMode="MultiLine" Rows="3" Columns="50" 
            runat="server" Visible="false" Width="80%" placeholder="What happening?"></asp:TextBox>
           
        </td>
    </tr>
</table>

</div>
<%--<hr />
<div id="pnl2">
    <asp:Button ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_Click" CssClass="inpBtn"/>
    <asp:Button ID="btnParameter" runat="server" Text="Parameter" OnClientClick="paramshide('show'); return false;" CssClass="inpBtn" />
</div>--%>
<div id="pnlParams" style="visibility:hidden"  class="container">
<hr />
<table style="width: 800px">
    <tr>        
        <td Width="40%">
             <asp:TextBox ID="txkey1" runat="server"  placeholder="key 1" ></asp:TextBox>
        </td>
        <td Width="60%">
             <asp:TextBox ID="txvalue1" runat="server" placeholder="value 1" ></asp:TextBox>
        </td>
    </tr>
    <tr>        
        <td Width="40%">
             <asp:TextBox ID="txkey2" runat="server" placeholder="key 2" ></asp:TextBox>
        </td>
        <td Width="60%">
             <asp:TextBox ID="txvalue2" runat="server"  placeholder="value 2" ></asp:TextBox>
        </td>
    </tr>
    <tr>        
        <td Width="40%">
             <asp:TextBox ID="txkey3" runat="server" placeholder="key 3" ></asp:TextBox>
        </td>
        <td Width="60%">
             <asp:TextBox ID="txvalue3" runat="server" placeholder="value 3" ></asp:TextBox>
        </td>
    </tr>
</table>
</div>

<hr />
<div id="pnl2"  class="container">
    <asp:Button ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_Click"  CssClass="inpBtn"/>
    <asp:Button ID="btnParameter" runat="server" Text="Parameter" OnClientClick="paramshide('show'); return false;" CssClass="inpBtn"  />
</div>

<hr />
<h3>Result Json Data</h3>
<div id="jsonData" runat="server"  class="container">

</div>
</asp:Content>

