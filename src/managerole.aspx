<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="managerole.aspx.cs" Inherits="WebApplication1.managerole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>Master Data - Manage Rule</h2>

    <style type="text/css">
        .container
        {
            margin-left: 10px; 
            margin-right: 10px; 
        }
    </style>

<script type="text/javascript">
    var i = 0; /* Set Global Variable i */
    function increment() {
        i += 1; /* Function for automatic increment of field's "Name" attribute. */
    }

    /*
    -----------------------------------------------------------------------------

    Functions that will be called upon, when user click on the E-mail text field.

    ------------------------------------------------------------------------------
    */
    function emailFunction() {
        var data = ['VAR-1', 'VAR-2'];
        var conditions =['greater than', 'lesser than', 'equal']
        var s = document.createElement('div');
        var r = document.createElement('span');
        var y = document.createElement("INPUT");
        var a = document.createElement("SELECT");
        var b = document.createElement("SELECT");
        var c = document.createElement("SELECT");
        var e = document.createElement("SPAN");

        y.setAttribute("type", "text");
        y.setAttribute("value", "Email");
        for (var i = 0; i < data.length; i++) {
            a.options[a.options.length] = new Option(data[i], data[i]);
        }

        for (var i = 0; i < conditions.length; i++) {
            b.options[b.options.length] = new Option(conditions[i], i);
        }

        for (var i = 0; i < data.length; i++) {
            c.options[c.options.length] = new Option(data[i], data[i]);
        }
          
        var g = document.createElement("IMG");
        g.setAttribute("src", "images/delete.png");
        increment();
        y.setAttribute("Name", "textelement_" + i);
        a.setAttribute("Name", "cbVar1_" + i);
        c.setAttribute("Name", "cbVar2_" + i);
        b.setAttribute("Name", "condition_" + i);

        r.appendChild(a);
        r.appendChild(b);
        r.appendChild(c);
        r.appendChild(y);

        g.setAttribute("onclick", "removeElement('myForm','id_" + i + "')");
//        r.appendChild(g);
        r.setAttribute("id", "id_" + i);
        s.setAttribute("id", "div_" + i);
        s.appendChild(r);
        document.getElementById("myForm").appendChild(s);

        console.log('test',r,y,i,a,c);
    }

    function paramshide(flag) {
        var xyz = "show";
        if (flag == xyz) {
            console.log('test');
        }

    }
</script>



<hr />
<div id="pnl1"  class="container">
<table style="width: 800px";>
    
     <tr>
        <td Width="40%">
            <asp:Label ID="lbDesc" runat="server" Text="Description Roles" ></asp:Label>
        </td>
        <td Width="60%">
            <asp:TextBox ID="txdesc" runat="server" Width="60%" CssClass="inpTxt"></asp:TextBox>
        </td>

    </tr>
</table>
</div>
<hr />
<div id="pnl2"  class="container">
    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClientClick="emailFunction(); return false;" />
    <asp:Button ID="btnSave" runat="server" Text="Save" />
    <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" />--%>
    <div id="myForm"></div>
</div>



</asp:Content>
