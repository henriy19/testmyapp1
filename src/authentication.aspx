<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="authentication.aspx.cs" Inherits="WebApplication1.authentication" %>
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

<hr />
<div id="pnl1" class="container">
<h2>Authentication</h2> <hr />
<asp:Label ID="ids" runat="server" Text="" Width="40%" Visible=false></asp:Label>
<table style="width: 800px";>
    <tr>
        <td>
            <asp:Label ID="type" runat="server" Text="Type" Width="40%"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server" Width="40%" CssClass="inpDdl">
                <%--<asp:ListItem Value="NoAuth" Text="No Auth"></asp:ListItem>
                <asp:ListItem Value="BasicAuth" Text="Basic Auth"></asp:ListItem>
                <asp:ListItem Value="DigestAuth" Text="Digest Auth"></asp:ListItem>--%>
                <asp:ListItem Value="OAuth1.0" Text="OAuth 1.0"></asp:ListItem>
                <%--<asp:ListItem Value="OAuth2.0" Text="OAuth 2.0"></asp:ListItem>
                <asp:ListItem Value="HawkAuthentication" Text="Hawk Authentication"></asp:ListItem>
                <asp:ListItem Value="AWSSignature" Text="AWS Signature"></asp:ListItem>--%>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lbConsumerKey" runat="server" Text="Consumer Key" Width="40%"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txConsumerKey" runat="server" Width="60%"  CssClass="inpTxt"></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td>
            <asp:Label ID="lbConsumerSecret" runat="server" Text="Consumer Secret" Width="40%"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txConsumerSecret" runat="server" Width="60%"  CssClass="inpTxt"></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td>
            <asp:Label ID="lbToken" runat="server" Text="Token" Width="40%"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txToken" runat="server" Width="60%"  CssClass="inpTxt"></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td>
            <asp:Label ID="lbTokenSecret" runat="server" Text="Token Secret" Width="40%"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txTokenSecret" runat="server" Width="60%"  CssClass="inpTxt"></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td>
            <asp:Label ID="signature" runat="server" Text="Signature Methods" Width="40%"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="DropDownList2" runat="server" Width="40%"  CssClass="inpDdl">
                <asp:ListItem Value="HMAC-SHA1" Text="HMAC-SHA1"></asp:ListItem>
                <%--<asp:ListItem Value="HMAC-SHA256" Text="HMAC-SHA256"></asp:ListItem>
                <asp:ListItem Value="PLAINTTEXT" Text="PLAINTTEXT"></asp:ListItem>--%>
            </asp:DropDownList>
        </td>

    </tr>
</table>
</div>
<hr />
<div id="pnl2" class="container">
    <asp:Button ID="btnClear" runat="server" Text="Refresh" OnClick="btnClear_Click" CssClass="inpBtn"/>
    <asp:Button ID="btnUpdate" runat="server" Text="Update Auth" OnClick="btnUpdate_Click" CssClass="inpBtn"/>
</div>


    

</asp:Content>
