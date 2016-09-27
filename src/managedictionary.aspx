<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="managedictionary.aspx.cs" Inherits="WebApplication1.managedictionary" %>
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
<form id="form1">
<div id="pnl1" class="container">
<h2>Mater Data - Manage Dictionary</h2> <hr />
<table style="width: 800px";>
    <tr>        
        <td Width="40%">
            <asp:Label ID="lbDesc" runat="server" Text="Description" ></asp:Label>
        </td>
        <td  Width="60%">
            <asp:TextBox ID="txdesc" runat="server" Width="60%"  CssClass="inpTxt"></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td>
            <asp:Label ID="Status" runat="server" Text="Status"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server" Width="40%"  CssClass="inpDdl">
                <asp:ListItem Value=1 Text="Positive"></asp:ListItem>
                <asp:ListItem Value=2 Text="Negative"></asp:ListItem>
                <%--<asp:ListItem Value=3 Text="Mixed"></asp:ListItem>--%>
                <%--<asp:ListItem Value=4 Text="Neutral"></asp:ListItem>--%>
            </asp:DropDownList>
        </td>
    </tr>
</table>
</div>
<hr />
<div id="pnl2"  class="container">
    <%--<asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="inpBtn" />--%>
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="inpBtn" />
    <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" />--%>
</div>
<hr />
<div id="pnl3"  class="container">
        <asp:GridView ID="gvListData" runat="server" EmptyDataText="Tidak ada data" AutoGenerateColumns="false" AllowPaging="true" 
        OnPageIndexChanging="gvListData_PageIndexChange" CellPadding="5" OnRowEditing="EditRow" onrowcancelingedit="CancelEdit">
            <HeaderStyle BackColor="#3498DB" Font-Bold="true" ForeColor="White" Height="28px" />
            <PagerStyle BackColor="DeepSkyBlue" HorizontalAlign="Right" ForeColor="White" />
            <Columns>
                <%--<asp:BoundField DataField="description" HeaderText="Description" HeaderStyle-Width="150px" />
                <asp:BoundField DataField="status" HeaderText="Status" HeaderStyle-Width="100px" />--%>
                <asp:TemplateField ItemStyle-Width = "30px"  HeaderText = "Description">
                   <ItemTemplate>
                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("description")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Sentiment">
                    <ItemTemplate>
                        <asp:Label ID="lblSentiment" runat="server" Text='<%# Eval("status")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID = "DropDownList1" runat = "server" CssClass="inpDdl">
                            <asp:ListItem Value=1 Text="Positive"></asp:ListItem>
                            <asp:ListItem Value=2 Text="Negative"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRemove" runat="server"
                            CommandArgument = '<%# Eval("description")%>'
                         OnClientClick = "return confirm('Do you want to delete?')"
                        Text = "Delete" OnClick = "DeleteDesc"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField  ShowEditButton="True" />
            </Columns>
        </asp:GridView>
    
    </div>  
</form>
</asp:Content>
