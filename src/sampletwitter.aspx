<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeBehind="sampletwitter.aspx.cs" Inherits="WebApplication1.sampletwitter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="js/libs/jquery.js"></script>
    <script type="text/javascript" src="js/libs/handlebars.js"></script>
    <script type="text/javascript" src="js/libs/angular.js"></script>
    <script type="text/javascript" src="js/libs/uikit.js"></script>
    <style type="text/css">
        .btn-Image
        {
            margin-top: 5px;
        }
        
        .btn-Image > img
        {
            cursor: pointer;
        }
        
        .btn-Image > img:hover
        {
            background: #ddd;
        }
        .container
        {
            margin-left: 10px;
            margin-right: 10px;
        }
        
        .containTimelines
        {
            margin-top: 2px;
            padding: 5px 5px 5px 5px;
            background-color: White;
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        .containTimelines .itemTimeline
        {
            margin-top: 2px;
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        .containTimelines .itemTimeline .itemTimelineLeft
        {
            width: 20%;
            float: left;
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        .containTimelines .itemTimeline .itemTimelineRight
        {
            width: 75%;
            float: right;
        }
        
        .containTimelines:hover, .containTimelines:focus
        {
            background-color: #ebebeb;
        }
        
        .containLeft
        {
            width: 95%;
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        .containerContent
        {
            margin-top: 10px;
            margin-bottom: 10px;
        }
        
        .containerPolling
        {
            margin-top: 10px;
        }
        
        .containLeft .contentleftHeader
        {
            background-color: #0091ea;
            height: 40px;
            padding-top: 15px;
            padding-left: 20px;
            font-size: 20px;
            font-weight: bold;
            color: White;
            border-radius: 5px 5px 0px 0px;
            
        }
        
        .containLeft .contentleft1
        {
            width: 35%;
            float: left;
            height: 90%;
            padding-top: 5px;
            padding-left: 10px;
        }
        
        .containLeft .contentleft2
        {
            width: 55%;
            float: right;
            height: 90%;
            padding-top: 5px;
        }
        
        .hideIdReply
        {
            position: absolute;
            top: 70px;
            z-index: -99;
        }
        .hideIdRetweet
        {
            position: absolute;
            top: -50px;
            z-index: -99;
        }
        
        .btnTweet
        {
            margin-top: 5px;
        }
        
        .tweetInput
        {
            padding-top: 5px;
            padding-left: 15px;
            font-size: 15px;
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        /* Rounded Corners */
        .tBox
        {
            border: 1px solid #765942;
            border-radius: 3px;
            height: 15px;
            padding-left: 15px; /*width: 300px;*/
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        .tArea
        {
            border: 1px solid #765942;
            border-radius: 5px;
            height: 30px;
            width: 300px;
            font-size: 13px;
            font-family: "Century Gothic", "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        }
        
        .tArea:hover
        {
            border: 1px solid #765942;
            border-radius: 5px;
            height: 60px;
            width: 300px;
        }
        
        .inpBtn
        {
            font-family: Century Gothic;
            font-size: 15px;
            height: 28px;
            border-radius: 5px;
        }
    </style>
    <script type="text/javascript">
        function replyTweet(id) {
            var modal = document.getElementById('replyTweetModal');
            modal.style.display = "block";
            document.getElementById('<%=tbreplyId.ClientID %>').value = id;
            ShowIDReply(id)
        }

        function closed() {
            var modal = document.getElementById('replyTweetModal');
            modal.style.display = "none";
        }

        function retweet(id) {
            var _this = $(this);
            //            console.log('id', _this);
            var modal = document.getElementById('retweetModal');
            modal.style.display = "block";
            $('#textID').text('ID : ' + id);
            document.getElementById('<%=tbID.ClientID %>').value = id;
            ShowID(id);
        }

        function closedRT() {
            var modal = document.getElementById('retweetModal');
            modal.style.display = "none";
        }

        function ShowID(id) {
            $.ajax({
                type: "POST",
                url: "sampletwitter.aspx/retweet",
                data: '{name: "' + id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function ShowIDReply(id) {
            $.ajax({
                type: "POST",
                url: "sampletwitter.aspx/replyTweet",
                data: '{name: "' + id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess2,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccess(response) {
            ////            alert(response.d);
            //            var data = JSON.parse(response.d);
            ////            var check  = data.indexOf(id)
            //            console.log(data[0].id_str);

            console.log(response.d);

            $("div #ContentPlaceHolder1_infoContentRTD").html('Tweet : ' + response.d);

        }

        function OnSuccess2(response) {
            //            var data = JSON.parse(response.d);
            var data = response.d;
            var rest = data.split("^");

            console.log(response.d, rest);
            $("div #headerRepltTo").html("Reply to " + rest[1]);
            $("div #ContentPlaceHolder1_detailInfod").html(rest[0]);
            document.getElementById('<%=tbReplyTweet.ClientID %>').value = rest[1] + " ";
        }

        function processRT() {

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
            $('#ContentPlaceHolder1_pnlTimeline [data-name=str_id]').on('click', function () {
                console.log($(this).data());
                //                retweet($(this).data().id);
            });
            $('#ContentPlaceHolder1_pnlTimeline [data-name=retweet]').on('click', function () {
                console.log($(this).data());
                retweet($(this).data().id);

            });
            $('#ContentPlaceHolder1_pnlTimeline [data-name=replyTweet]').on('click', function () {
                console.log($(this).data());
                replyTweet($(this).data().id);
            });

            $('#ContentPlaceHolder1_btnSendRT1').on('click', function () {
                var inputText = $("#ContentPlaceHolder1_tbRetweet").val();
                var text = $("#ContentPlaceHolder1_infoContentRTD").text();
                var id = $("#textID").text();
                closedRT();
                console.log(inputText, text, id);
            });

//            checkSession();

        });

    </script>
    <div ui-view>
        <form id="form1">
        <div id="pnl1" style="width: 100%; font-family: Segoe UI" class="container">
            <h2>
                Twitter</h2>
            <div id="pnlLeft" style="width: 40%; float: left; font-family: Segoe UI">
                <div id="containerLeft1" class="containLeft">
                    <div class="contentleftHeader">
                        Profile
                    </div>
                    <div class="contentleft1">
                        <div id='picImg' runat="server" style="height: 110px; width: 130px; float: left;">
                        </div>
                    </div>
                    <div class="contentleft2">
                        <header>
                    <h4><asp:Label ID="profile" runat="server" Text="" Font-Size="X-Large" ForeColor="#0d47a1"></asp:Label></h4>
                </header>
                        <p style="margin-top: -10px;">
                            <asp:Label ID="tweet" runat="server" Text="" Font-Size="Medium"></asp:Label>
                        </p>
                    </div>
                    <div id="infotweet" class="containLeft">
                        <table width="100%">
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="FollowerCount" runat="server" Text="" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 30%">
                                    <asp:Label ID="tweetCount" runat="server" Text="" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 40%">
                                    <asp:Label ID="FollowCount" runat="server" Text="" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </div>
                    <div id="polling">
                    </div>
                </div>
                <div class="containLeft">
                    <div>
                        <asp:Button ID="btnViewPolling" Text="Start Polling" runat="server" class="inpBtn btnTweet"
                            OnClick="btnViewPolling_Click" />
                    </div>
                </div>
                <div id="containerLeft2" class="containLeft" runat="server" style="visibility: hidden">
                    <div class="contentleft1">
                        <div runat="server" class="containerPolling">
                            <asp:Label ID="hastag" runat="server" Text="Hashtag" Font-Size="Medium"></asp:Label>
                        </div>
                        <div runat="server" class="containerPolling">
                            <asp:Label ID="pertanyaan" runat="server" Text="Pertanyaan" Font-Size="Medium"></asp:Label>
                        </div>
                    </div>
                    <div class="contentleft2">
                        <div runat="server" class="containerPolling">
                            <asp:TextBox ID="tbHastag" runat="server" Width="81%" placeholder="What's hastag ?"
                                CssClass="tBox"></asp:TextBox>
<%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbHastag"
                                ErrorMessage="hashtag is required" ForeColor="Red">
                            </asp:RequiredFieldValidator>--%>
                        </div>
                        <div id="Div2" runat="server" class="containerPolling">
                            <asp:TextBox ID="tbQuestion" runat="server" Width="80%" placeholder="What's question ?"
                                TextMode="MultiLine" Columns="50" Rows="3" CssClass="tweetInput tArea"></asp:TextBox>
<%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbQuestion"
                                ErrorMessage="hashtag is required" ForeColor="Red">
                            </asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="contentleft2">
                        <div runat="server">
                            <asp:Button ID="btnSendPoll" runat="server" Text="Polling" class="inpBtn btnTweet"
                                OnClick="btnSendPoll_Click" />
                            <asp:Button ID="btnClosedPoll" runat="server" Text="Cancel" class="inpBtn btnTweet"
                                OnClick="btnClosedPoll_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="pnlRight" style="width: 60%; float: right; font-family: Segoe UI">
                <div id="containerRight1" style="width: 95%; height: 100px">
                    <div id="contentRight1" style="width: 100%; padding-bottom: 5px;">
                        <asp:TextBox ID="tbTweet" TextMode="MultiLine" runat="server" Width="80%" Columns="50"
                            Rows="3" placeholder="What's happening ?" CssClass="tweetInput tArea"></asp:TextBox><br />
                        <asp:Button ID="btnTweet" runat="server" Text="Tweet" Width="15%" OnClick="btnTweet_Click"
                            class="inpBtn btnTweet" />
                        <footer>
                    <hr  />
                </footer>
                    </div>
                </div>
                <div id="Div1" style="width: 95%;">
                    <div id="pnlTimeline" runat="server" style="width: 100%;">
                    </div>
                </div>
            </div>
        </div>
        <!-- The Modal -->
        <div id="replyTweetModal" class="modal">
            <!-- Modal content -->
            <div class="modal-content">
                <div class="modal-header">
                    <span class="close" onclick="closed()">×</span>
                    <div id="headerRepltTo">
                    </div>
                </div>
                <div class="modal-body">
                    <div id="detailInfoH" runat="server" class="containerContent">
                        <asp:TextBox ID="tbreplyId" runat="server" placeholder="Add a comment..." CssClass="hideIdReply"></asp:TextBox>
                        <div id="detailInfod" runat="server">
                        </div>
                    </div>
                    <div id="inputContent" runat="server" class="containerContent">
                        <div id="inputTweet">
                            <asp:TextBox ID="tbReplyTweet" TextMode="MultiLine" Columns="50" Rows="3" runat="server"
                                placeholder="What's happening ?" CssClass="tweetInput tArea"></asp:TextBox>
                        </div>
                        <hr />
                        <div id="controlTweet" runat="server" style="height: 30px">
                            <div id="ctLeft" style="float: right; width: 60%;">
                            </div>
                            <div id="ctRight" style="float: right; width: 40%; text-align: right">
                                <asp:Button ID="btnSend" runat="server" Text="Tweet" OnClick="btnSend_Click" CssClass="inpBtn" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="retweetModal" class="modal">
            <!-- Modal content -->
            <div class="modal-content">
                <div class="modal-header">
                    <span class="close" onclick="closedRT()">×</span>
                    <div>
                        Retweet this to your followers?</div>
                </div>
                <div class="modal-body">
                    <div id="inputRetweet" class="containerContent">
                        <asp:TextBox ID="tbRetweet" TextMode="MultiLine" Columns="50" Rows="2" runat="server"
                            placeholder="Add a comment..." CssClass="tweetInput tArea"></asp:TextBox>
                    </div>
                    <div id="inputContentRT" runat="server" class="containerContent">
                        <div id="infoContentRT" runat="server">
                            <asp:TextBox ID="tbID" runat="server" placeholder="Add a comment..." CssClass="hideIdRetweet"></asp:TextBox>
                            <span id="textID"></span>
                            <div id="infoContentRTD" runat="server">
                            </div>
                        </div>
                        <hr />
                        <div id="controlRetweet" runat="server" style="height: 30px">
                            <div id="crLeft" style="float: right; width: 60%;">
                            </div>
                            <div id="crRight" style="float: right; width: 40%; text-align: right">
                                <asp:Button ID="btnSendRT" runat="server" Text="Tweet" OnClick="btnSendRT_Click"
                                    CssClass="inpBtn" />
                                <%--<asp:Image ImageUrl="images/retweet.png" width="28" height="28" ID="btnSendRT1" runat="server" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </form>
    </div>
    <%--    <script type="text/javascript">
        console.log('test');
        (function () {
            uikit.polling('#polling', {
                name: 'polling1',
                text: 'Pertanyaan',
                data: [
                    { id: 'Y', text: 'Ya' },
                    { id: 'T', text: 'Tidak' },
                ],
                onSend: function(value) {
                    console.log(value);
                }
            });

        } ());
</script>--%>
</asp:Content>
