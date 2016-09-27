using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPSnippets.TwitterAPI;
using System.Data.SqlClient;
using System.Data;
using Twitterizer;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Globalization;
using HtmlAgilityPack;
using System.Web.Services;
using System.Configuration;

namespace WebApplication1
{
    public partial class sampletwitter : System.Web.UI.Page
    {
        //SqlConnection conn = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constring"].ConnectionString);
        private static sampletwitter _me = new sampletwitter();

        string idStatus = "";
        string status = "";
        string idUserTweet = "";
        string userNameTweet = "";
        string idReplyStatus = "";
        string idReplyUser = "";
        string idReplyUsername = "";
        string idMentions = "";
        string userMentions = "";
        string hashtags = "";
        int _retweet = 0;
        int _retweetCount = 0;

        int followers = 0;
        int friends = 0;
        int favorites = 0;
        int statusCount = 0;
        string location = "";
        string description = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!ClientScript.IsStartupScriptRegistered("alert"))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(),
            //        "alert", "alertMe();", true);
            //}

            if (!IsPostBack)
            {
                getProfile();
                getHomeTimeLine();
            }
        }

        private void getProfileTwitter()
        {
            DataSet dset = getAuth();
            string oauth_token = dset.Tables[0].Rows[0][4].ToString();
            string oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            string oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            string oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            TwitterConnect.API_Key = oauth_token;
            TwitterConnect.API_Secret = oauth_token_secret;

            if (TwitterConnect.IsAuthorized)
            {
                TwitterConnect twitter = new TwitterConnect();
                DataTable dt = twitter.FetchProfile();
            }
        }

        public DataSet getAuth()
        {
            DataSet ds2 = new DataSet();

            conn.Open();
            string query = "";
            query += "select top 1 * from authentication ";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds2);
            conn.Close();

            return ds2;
        }

        public void getProfile()
        {
            string v = "";
            var postget = "GET";
            var resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
            DataSet dset = getAuth();
            // oauth application keys
            var oauth_token = dset.Tables[0].Rows[0][4].ToString();
            var oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            var oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            var oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();


            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version,
                                        Uri.EscapeDataString(v)
                                        );

            baseString = string.Concat("" + postget + "&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );

            ServicePointManager.Expect100Continue = false;

            // make the request

            var postBody = "q=" + Uri.EscapeDataString(v);
            resource_url += "?" + postBody;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = postget;
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var objText = reader.ReadToEnd();

                string html = "";
                string img = "";
                JArray jsonDat = JArray.Parse(objText);
                profile.Text = "@" + jsonDat[0]["user"]["screen_name"].ToString();
                tweet.Text = jsonDat[0]["text"].ToString();
                tweetCount.Text = "Followings : " + jsonDat[0]["user"]["friends_count"].ToString();
                FollowCount.Text = "Followers : " + jsonDat[0]["user"]["followers_count"].ToString();
                FollowerCount.Text = "Tweets : " + jsonDat[0]["user"]["statuses_count"].ToString();

                string image1 = jsonDat[0]["user"]["profile_image_url"].ToString();
                string image2 = image1.Remove(image1.Length - 11) + ".jpg";
                img = "<img src=" + image2 + " height=90 width=100 />";
                picImg.InnerHtml = img;

                html = "";
                #region --- di remark dulu -----
                //for (int x = 0; x < 7; x++)
                //{
                //    string tweetby = "";
                //    string follower = "";
                //    string status = jsonDat[x]["retweeted"].ToString();
                //    double _followers = 0.0;
                //    string followers = "";
                //    if (status == "False")
                //    {
                //        tweetby = jsonDat[x]["user"]["screen_name"].ToString();
                //        follower = jsonDat[x]["user"]["followers_count"].ToString();
                //        _followers = Convert.ToDouble(follower);
                //    }
                //    else
                //    {
                //        tweetby = jsonDat[x]["retweeted_status"]["user"]["screen_name"].ToString();
                //        follower = jsonDat[x]["retweeted_status"]["user"]["followers_count"].ToString();
                //        _followers = Convert.ToDouble(follower);
                //    }

                //    if (_followers >= 1000)
                //    {
                //        followers = _followers.ToString("#,##0,K", CultureInfo.InvariantCulture);
                //    }
                //    else
                //    {
                //        followers = _followers.ToString("#,#", CultureInfo.InvariantCulture);
                //    }

                //    string imgreply = "images/reply_tweet.png";
                //    string retweet = "images/retweet.png";
                //    string favorites = "images/heart.png";
                //    string id = jsonDat[x]["id_str"].ToString();

                //    html += "<span data-id='" + jsonDat[x]["id"].ToString() + "' data-name='str_id'>id : " + jsonDat[x]["id"].ToString() + "</span><br/>";
                //    html += "<span >Tweet Data Time : " + jsonDat[x]["created_at"].ToString() + "<br/>";
                //    html += "<span>Tweet           : " + jsonDat[x]["text"].ToString() + "</span><br/>";
                //    html += "<span data-name='" + tweetby + "'>Tweetby         : " + tweetby + "</span><br/>";
                //    html += "<span>Followers       : " + followers + "</span><br/>";
                //    html += "<div class='btn-Image'>";
                //    html += "<span><img type=image src=" + imgreply + " alt=Submit width=28 height=28 data-id=" + id + " data-name='replyTweet'></span>";
                //    html += "&nbsp;&nbsp;&nbsp;&nbsp;";
                //    html += "<span><img type=image src=" + retweet + " alt=Submit width=28 height=28 data-id=" + id + " data-name='retweet'></span>";
                //    html += "&nbsp;&nbsp;&nbsp;&nbsp;";
                //    html += "<span><img type=image src=" + favorites + " alt=Submit width=28 height=28 ></span>";
                //    html +="</div>";
                //    html += "<hr />";
                //}

                #endregion

                pnlTimeline.InnerHtml = html;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void getHomeTimeLine()
        {
            string v = "";
            var postget = "GET";
            //var resource_url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
            var resource_url = "https://api.twitter.com/1.1/statuses/mentions_timeline.json";
            DataSet dset = getAuth();
            // oauth application keys
            var oauth_token = dset.Tables[0].Rows[0][4].ToString();
            var oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            var oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            var oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();


            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version,
                                        Uri.EscapeDataString(v)
                                        );

            baseString = string.Concat("" + postget + "&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );

            ServicePointManager.Expect100Continue = false;

            // make the request

            var postBody = "q=" + Uri.EscapeDataString(v);
            resource_url += "?" + postBody;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = postget;
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var objText = reader.ReadToEnd();

                string html = "";
                JArray jsonDat = JArray.Parse(objText);

                html = "";
                for (int x = 0; x < 7; x++)
                {
                    string tweetby = "";
                    string follower = "";
                    string status = jsonDat[x]["retweeted"].ToString();
                    double _followers = 0.0;
                    string followers = "";
                    string retweetCount = "";

                    tweetby = jsonDat[x]["user"]["screen_name"].ToString();
                    follower = jsonDat[x]["user"]["followers_count"].ToString();
                    _followers = Convert.ToDouble(follower);
                    retweetCount = jsonDat[x]["retweet_count"].ToString();

                    if (_followers >= 1000)
                    {
                        followers = _followers.ToString("#,##0,K", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        followers = _followers.ToString("#,#", CultureInfo.InvariantCulture);
                    }

                    string imgreply = "images/reply_tweet.png";
                    string retweet = "images/retweet.png";
                    string favorites = "images/heart.png";
                    string id = jsonDat[x]["id_str"].ToString();

                    string dateStr = jsonDat[x]["created_at"].ToString();
                    string format = "ddd MMM dd yyyy HH:mm:ss ";
                    string dateTime = "";
                    DateTime date;
                    bool validFormat = DateTime.TryParseExact(dateStr, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                    if (validFormat)
                    {
                        dateTime = date.ToString();
                    }


                    html += "<div class='containTimelines'>";
                    html += "<div class='itemTimeline'><div class='itemTimelineLeft'>ID</div><span data-id='" + jsonDat[x]["id"].ToString() + "' data-name='str_id'> " + jsonDat[x]["id"].ToString() + "</span> </div>";
                    html += "<div class='itemTimeline'><div class='itemTimelineLeft'>Tweet Data Time</div><span > " + jsonDat[x]["created_at"].ToString() + "</span></div>";
                    html += "<div class='itemTimeline'><div class='itemTimelineLeft'>Tweet</div><span > " + jsonDat[x]["text"].ToString() + "</span></div>";
                    html += "<div class='itemTimeline'><div class='itemTimelineLeft'>Tweetby</div><span data-name='" + tweetby + "'> " + tweetby + "</span></div>";
                    html += "<div class='itemTimeline'><div class='itemTimelineLeft'>Followers</div><span >" + followers + "</span></div>";
                    html += "</div>";
                    html += "<div class='btn-Image'>";
                    html += "<span><img type=image src=" + imgreply + " alt=Submit width=28 height=28 data-id=" + id + " data-name='replyTweet'></span>";
                    html += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    html += "<span><img type=image src=" + retweet + " alt=Submit width=28 height=28 data-id=" + id + " data-name='retweet'> " + retweetCount + "</span>";
                    html += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    html += "<span><img type=image src=" + favorites + " alt=Submit width=28 height=28 ></span>";
                    html += "</div>";
                    html += "<hr />";
                }


                pnlTimeline.InnerHtml = html;

                string statusText = "";

                JArray jsonDat1 = JArray.Parse(objText);

                for (int i = 0; i < jsonDat1.Count(); i++)
                {
                    hashtags = "";
                    idMentions = "";
                    userMentions = "";

                    idStatus = jsonDat1[i]["id_str"].ToString();
                    statusText = jsonDat1[i]["text"].ToString();
                    idUserTweet = jsonDat1[i]["user"]["id_str"].ToString();
                    userNameTweet = jsonDat1[i]["user"]["screen_name"].ToString();
                    idReplyStatus = (jsonDat1[i]["in_reply_to_status_id_str"].ToString()) ?? "";
                    idReplyUser = (jsonDat1[i]["in_reply_to_user_id_str"].ToString()) ?? "";
                    idReplyUsername = (jsonDat1[i]["in_reply_to_screen_name"].ToString()) ?? "";
                    _retweetCount = Convert.ToInt32(jsonDat1[i]["retweet_count"].ToString());

                    followers = Convert.ToInt32(jsonDat1[i]["user"]["followers_count"].ToString());
                    friends = Convert.ToInt32(jsonDat1[i]["user"]["friends_count"].ToString());
                    favorites = Convert.ToInt32(jsonDat1[i]["user"]["favourites_count"].ToString());
                    statusCount = Convert.ToInt32(jsonDat1[i]["user"]["statuses_count"].ToString());
                    location = jsonDat1[i]["user"]["location"].ToString();
                    description = jsonDat1[i]["user"]["description"].ToString();

                    GetDataUser(idUserTweet, userNameTweet, followers, friends, favorites, location, description, statusCount);

                    int checkHastags = jsonDat1[i]["entities"]["hashtags"].Count();
                    int checkMentions = jsonDat1[i]["entities"]["user_mentions"].Count();

                    if (checkHastags > 0)
                    {
                        for (int a = 0; a < checkHastags; a++)
                        {
                            if (a == 0)
                            {
                                hashtags = jsonDat1[i]["entities"]["hashtags"][a]["text"].ToString();
                            }
                            else
                            {
                                hashtags += "," + jsonDat1[i]["entities"]["hashtags"][a]["text"].ToString();
                            }
                        }
                    }

                    if (checkMentions > 0)
                    {
                        for (int b = 0; b < checkMentions; b++)
                        {
                            if (b == 0)
                            {
                                idMentions = jsonDat1[i]["entities"]["user_mentions"][b]["id_str"].ToString();
                                userMentions = jsonDat1[i]["entities"]["user_mentions"][b]["screen_name"].ToString();
                            }
                            else
                            {
                                idMentions += "," + jsonDat1[i]["entities"]["user_mentions"][b]["id_str"].ToString();
                                userMentions += "," + jsonDat1[i]["entities"]["user_mentions"][b]["screen_name"].ToString();
                            }
                        }
                    }

                    insertMontitoringTimeline(idStatus, statusText, idUserTweet, userNameTweet, idMentions, userMentions, hashtags, idReplyStatus, idReplyUser, idReplyUsername, _retweet, _retweetCount);
                }

                insertRawData(objText, oauth_timestamp);

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnTweet_Click(object sender, EventArgs e)
        {
            DataSet dset = getAuth();
            // oauth application keys
            var oauth_token = dset.Tables[0].Rows[0][4].ToString();
            var oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            var oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            var oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            string v = tbTweet.Text;

            var tokens = new OAuthTokens()
            {
                AccessToken = oauth_token,
                AccessTokenSecret = oauth_token_secret,
                ConsumerKey = oauth_consumer_key,
                ConsumerSecret = oauth_consumer_secret
            };

            if (v == "" || v == string.Empty)
            {
                v = "no-status";
            }

            try
            {
                var response = TwitterStatus.Update(tokens, v);

                string dataJson = response.Content.ToString();

                JObject results = JObject.Parse(dataJson);
                idStatus = (string)results["id_str"];
                status = (string)results["text"];
                idReplyStatus = ((string)results["in_reply_to_status_id_str"]) ?? "";
                idReplyUser = (string)results["in_reply_to_user_id_str"] ?? "";
                idReplyUsername = (string)results["in_reply_to_screen_name"] ?? "";
                _retweetCount = Convert.ToInt32((string)results["retweet_count"]);

                string xxx = (string)results["retweeted"];

                if ((string)results["retweeted"] == "False")
                {
                    _retweet = 0;
                }
                else
                {
                    _retweet = 1;
                }

                JToken _users = results["user"];

                if (_users is JValue)
                {
                    //html += "Tweet : " + (string)_users + "<br/>";
                }
                else if (_users is JObject)
                {
                    string dusers = "[" + _users + "]";
                    JArray jsonDat = JArray.Parse(dusers);
                    for (int x = 0; x < jsonDat.Count(); x++)
                    {
                        idUserTweet = jsonDat[x]["id_str"].ToString();
                        userNameTweet = jsonDat[x]["screen_name"].ToString();
                    }
                }

                JToken _entities = results["entities"];

                if (_entities is JObject)
                {
                    hashtags = "";
                    idMentions = "";
                    userMentions = "";

                    string d_entities = "[" + _entities + "]";
                    JArray jsonDat = JArray.Parse(d_entities);
                    
                    int a = jsonDat[0]["hashtags"].Count();
                    int b = jsonDat[0]["user_mentions"].Count();

                    if (a > 0)
                    {
                        JArray jsonDat1 = JArray.Parse(jsonDat[0]["hashtags"].ToString());
                        for (int i = 0; i < jsonDat1.Count(); i++)
                        {
                            if (i == 0)
                            {
                                hashtags = jsonDat1[i]["text"].ToString();
                            }
                            else
                            {
                                hashtags += "," + jsonDat1[i]["text"].ToString();
                            }
                        }
                    }
                    if (b > 0)
                    {
                        JArray jsonDat2 = JArray.Parse(jsonDat[0]["user_mentions"].ToString());
                        for (int i = 0; i < jsonDat2.Count(); i++)
                        {
                            if (i == 0)
                            {
                                idMentions = jsonDat2[i]["id_str"].ToString();
                                userMentions = jsonDat2[i]["screen_name"].ToString();
                            }
                            else
                            {
                                idMentions += "," + jsonDat2[i]["id_str"].ToString();
                                userMentions += "," + jsonDat2[i]["screen_name"].ToString();
                            }
                        }
                    }

                }

                insertMontitoringTimeline(idStatus, status, idUserTweet, userNameTweet, idMentions, userMentions, hashtags, idReplyStatus, idReplyUser, idReplyUsername, _retweet, _retweetCount);
                getProfile();
                getHomeTimeLine();
                tbTweet.Text = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnSendRT_Click(object sender, EventArgs e)
        {
            string idnumber = tbID.Text;
            processRetweet(idnumber);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {            
            DataSet dset = getAuth();
            // oauth application keys
            var oauth_token = dset.Tables[0].Rows[0][4].ToString();
            var oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            var oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            var oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            string v = tbReplyTweet.Text;

            var tokens = new OAuthTokens()
            {
                AccessToken = oauth_token,
                AccessTokenSecret = oauth_token_secret,
                ConsumerKey = oauth_consumer_key,
                ConsumerSecret = oauth_consumer_secret
            };

            string strId = tbreplyId.Text;
            decimal dclId = Convert.ToDecimal(strId);

            StatusUpdateOptions option = new StatusUpdateOptions();
            option.InReplyToStatusId = dclId;

            if (v == "" || v == string.Empty)
            {
                v = "no-status";
            }

            try
            {
                //var response = TwitterStatus.Update(tokens, v);
                TwitterResponse<TwitterStatus> replyResult = TwitterStatus.Update(tokens, v, option);

                string dataJson = replyResult.Content.ToString();

                JObject results = JObject.Parse(dataJson);

                idStatus = (string)results["id_str"];
                status = (string)results["text"];
                idReplyStatus = ((string)results["in_reply_to_status_id_str"]) ?? "";
                idReplyUser = (string)results["in_reply_to_user_id_str"] ?? "";
                idReplyUsername = (string)results["in_reply_to_screen_name"] ?? "";
                _retweetCount = Convert.ToInt32((string)results["retweet_count"]);

                if ((string)results["retweeted"] == "False")
                {
                    _retweet = 0;
                }
                else
                {
                    _retweet = 1;
                }

                JToken _users = results["user"];

                if (_users is JValue)
                {
                    //html += "Tweet : " + (string)_users + "<br/>";
                }
                else if (_users is JObject)
                {
                    string dusers = "[" + _users + "]";
                    JArray jsonDat = JArray.Parse(dusers);
                    for (int x = 0; x < jsonDat.Count(); x++)
                    {
                        idUserTweet = jsonDat[x]["id_str"].ToString();
                        userNameTweet = jsonDat[x]["screen_name"].ToString();
                    }
                }

                JToken _entities = results["entities"];

                if (_entities is JObject)
                {
                    hashtags = "";
                    idMentions = "";
                    userMentions = "";

                    string d_entities = "[" + _entities + "]";
                    JArray jsonDat = JArray.Parse(d_entities);

                    int a = jsonDat[0]["hashtags"].Count();
                    int b = jsonDat[0]["user_mentions"].Count();

                    if (a > 0)
                    {
                        JArray jsonDat1 = JArray.Parse(jsonDat[0]["hashtags"].ToString());
                        for (int i = 0; i < jsonDat1.Count(); i++)
                        {
                            if (i == 0)
                            {
                                hashtags = jsonDat1[i]["text"].ToString();
                            }
                            else
                            {
                                hashtags += "," + jsonDat1[i]["text"].ToString();
                            }
                        }

                    }
                    if (b > 0)
                    {
                        JArray jsonDat2 = JArray.Parse(jsonDat[0]["user_mentions"].ToString());
                        for (int i = 0; i < jsonDat2.Count(); i++)
                        {
                            if (i == 0)
                            {
                                idMentions = jsonDat2[i]["id_str"].ToString();
                                userMentions = jsonDat2[i]["screen_name"].ToString();
                            }
                            else
                            {
                                idMentions += "," + jsonDat2[i]["id_str"].ToString();
                                userMentions += "," + jsonDat2[i]["screen_name"].ToString();
                            }
                        }
                    }

                }
                insertMontitoringTimeline(idStatus, status, idUserTweet, userNameTweet, idMentions, userMentions, hashtags, idReplyStatus, idReplyUser, idReplyUsername, _retweet, _retweetCount);
                getProfile();
                getHomeTimeLine();
                tbTweet.Text = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        [WebMethod]
        public static string retweet(string name)
        {
            return _me.coba(name);
        }

        [WebMethod]
        public static string replyTweet(string name)
        {
            string rawdata = getTweetById(name);
            string html1 = "";
            string html2 = "";
            string data = "";
            JArray jsonDat = JArray.Parse(rawdata);
            for (int x = 0; x < jsonDat.Count(); x++)
            {
                string hashtags = "";
                int a = jsonDat[x]["entities"]["hashtags"].Count();
                if (a > 0)
                {
                    JArray jsonDat1 = JArray.Parse(jsonDat[x]["entities"]["hashtags"].ToString());
                    for (int z = 0; z < jsonDat1.Count(); z++)
                    {
                        if (z == 0)
                        {
                            hashtags = " #"+jsonDat1[z]["text"].ToString();
                        }
                        else
                        {
                            hashtags += " #" + jsonDat1[z]["text"].ToString();
                        }
                    }
                    
                }
                string checkId = jsonDat[x]["id"].ToString();
                if (name == checkId)
                {
                    //html += "<div id='infoContentRTD' runat='server'>";
                    html1 += "<span>" + jsonDat[x]["text"].ToString() + "</span>";
                    html1 += "<div></div>";
                    //html += "</div>";
                    html2 += "@" + jsonDat[x]["user"]["screen_name"].ToString();
                    html2 += hashtags;

                    data = html1 + "^" + html2;
                }
            }

            return data;
        }

        public static string getTweetById(string id)
        {
            string data = "";
            SqlConnection conn1 = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");
            DataSet ds = new DataSet();

            conn1.Open();
            string query = "";
            query += "select top 1 * from dbo.rawData where rawdata like '%" + id + "%' order by id desc ";
            SqlCommand cmd = new SqlCommand(query, conn1);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            conn1.Close();

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            data = dt.Rows[0][1].ToString();

            return data;

        }

        public void insertRawData(string raws, string timestamps)
        {
            string rd = raws.Replace("'", "`");
            conn.Open();
            string query = "";
            query += "insert into rawData (rawdata,timestamp,created_date) ";
            query += "values('" + rd + "','" + timestamps + "',getdate())";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void processRetweet(string id)
        {
            DataSet dset = getAuth();
            // oauth application keys
            var oauth_token = dset.Tables[0].Rows[0][4].ToString();
            var oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            var oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            var oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            string v = id;

            var tokens = new OAuthTokens()
            {
                AccessToken = oauth_token,
                AccessTokenSecret = oauth_token_secret,
                ConsumerKey = oauth_consumer_key,
                ConsumerSecret = oauth_consumer_secret
            };

            if (v == "" || v == string.Empty)
            {
                v = "0";
            }

            decimal param = Convert.ToDecimal(v);

            try
            {
                //var response = TwitterStatus.Update(tokens, v);
                var response = TwitterStatus.Retweet(tokens, param);

                string dataJson = response.Content.ToString();

                JObject results = JObject.Parse(dataJson);

                idStatus = (string)results["id_str"];
                status = (string)results["text"];
                idReplyStatus = ((string)results["in_reply_to_status_id_str"]) ?? "";
                idReplyUser = (string)results["in_reply_to_user_id_str"] ?? "";
                idReplyUsername = (string)results["in_reply_to_screen_name"] ?? "";
                _retweetCount = Convert.ToInt32((string)results["retweet_count"]);

                if ((string)results["retweeted"] == "False")
                {
                    _retweet = 0;
                }
                else
                {
                    _retweet = 1;
                }

                JToken _users = results["user"];

                if (_users is JValue)
                {
                    //html += "Tweet : " + (string)_users + "<br/>";
                }
                else if (_users is JObject)
                {
                    string dusers = "[" + _users + "]";
                    JArray jsonDat = JArray.Parse(dusers);
                    for (int x = 0; x < jsonDat.Count(); x++)
                    {
                        idUserTweet = jsonDat[x]["id_str"].ToString();
                        userNameTweet = jsonDat[x]["screen_name"].ToString();
                    }
                }

                JToken _entities = results["entities"];

                if (_entities is JObject)
                {
                    hashtags = "";
                    idMentions = "";
                    userMentions = "";

                    string d_entities = "[" + _entities + "]";
                    JArray jsonDat = JArray.Parse(d_entities);

                    int a = jsonDat[0]["hashtags"].Count();
                    int b = jsonDat[0]["user_mentions"].Count();

                    if (a > 0)
                    {
                        JArray jsonDat1 = JArray.Parse(jsonDat[0]["hashtags"].ToString());
                        for (int i = 0; i < jsonDat1.Count(); i++)
                        {
                            if (i == 0)
                            {
                                hashtags = jsonDat1[i]["text"].ToString();
                            }
                            else
                            {
                                hashtags += "," + jsonDat1[i]["text"].ToString();
                            }
                        }

                    }
                    if (b > 0)
                    {
                        JArray jsonDat2 = JArray.Parse(jsonDat[0]["user_mentions"].ToString());
                        for (int i = 0; i < jsonDat2.Count(); i++)
                        {
                            if (i == 0)
                            {
                                idMentions = jsonDat2[i]["id_str"].ToString();
                                userMentions = jsonDat2[i]["screen_name"].ToString();
                            }
                            else
                            {
                                idMentions += "," + jsonDat2[i]["id_str"].ToString();
                                userMentions += "," + jsonDat2[i]["screen_name"].ToString();
                            }
                        }
                    }

                }
                insertMontitoringTimeline(idStatus, status, idUserTweet, userNameTweet, idMentions, userMentions, hashtags, idReplyStatus, idReplyUser, idReplyUsername, _retweet, _retweetCount);

                getProfile();
                getHomeTimeLine();
                tbTweet.Text = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void insertMontitoringTimeline(string idStatus, string status, string idUserTweet, string userNameTweet, string idMentions,
            string userMentions, string hashtags, string idReplyStatus, string idReplyUser, string idReplyUsername, int _retweet, int _retweetCount)
        {
            string query = "";

            string textStatus = status.Replace("'", "`");

            query = "sp_synchDataMonitoringTimeline";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idStatus", idStatus);
            cmd.Parameters.AddWithValue("@status", textStatus);
            cmd.Parameters.AddWithValue("@idUserTweet", idUserTweet);
            cmd.Parameters.AddWithValue("@userNameTweet", userNameTweet);
            cmd.Parameters.AddWithValue("@idUserMention", idMentions);
            cmd.Parameters.AddWithValue("@userMention", userMentions);
            cmd.Parameters.AddWithValue("@hashtags", hashtags);
            cmd.Parameters.AddWithValue("@idReplyStatus", idReplyStatus);
            cmd.Parameters.AddWithValue("@idReplyUser", idReplyUser);
            cmd.Parameters.AddWithValue("@idReplyUsername", idReplyUsername);
            cmd.Parameters.AddWithValue("@retweet", _retweet);
            cmd.Parameters.AddWithValue("@retweetCount", _retweetCount);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

        }

        private string coba(string name)
        {
            string testing = getTweetById(name);
            string html1 = "";

            JArray jsonDat = JArray.Parse(testing);
            for (int x = 0; x < jsonDat.Count(); x++)
            {
                string checkId = jsonDat[x]["id"].ToString();
                if (name == checkId)
                {
                    //html += "<div id='infoContentRTD' runat='server'>";
                    html1 += "<span>" + jsonDat[x]["text"].ToString() + "</span>";
                    html1 += "<div></div>";
                    //html += "</div>";

                }
            }

            return html1;
        }

        protected void btnViewPolling_Click(object sender, EventArgs e)
        {
            tbHastag.Text = "";
            tbQuestion.Text = "";
            containerLeft2.Attributes["style"] = "visibility:vissible";
            btnViewPolling.Attributes["style"] = "visibility:hidden";
        }

        protected void btnSendPoll_Click(object sender, EventArgs e)
        {
            hashtags = "";
            string hastag = "#" + tbHastag.Text;
            string question = " " + tbQuestion.Text;

            DataSet dset = getAuth();
            // oauth application keys
            var oauth_token = dset.Tables[0].Rows[0][4].ToString();
            var oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            var oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            var oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            string v = hastag + question;

            var tokens = new OAuthTokens()
            {
                AccessToken = oauth_token,
                AccessTokenSecret = oauth_token_secret,
                ConsumerKey = oauth_consumer_key,
                ConsumerSecret = oauth_consumer_secret
            };

            if (v == "" || v == string.Empty)
            {
                v = "no-status";
            }

            try
            {
                var response = TwitterStatus.Update(tokens, v);

                string dataJson = response.Content.ToString();

                JObject results = JObject.Parse(dataJson);
                idStatus = (string)results["id_str"];
                status = (string)results["text"];
                idReplyStatus = ((string)results["in_reply_to_status_id_str"]) ?? "";
                idReplyUser = (string)results["in_reply_to_user_id_str"] ?? "";
                idReplyUsername = (string)results["in_reply_to_screen_name"] ?? "";
                _retweetCount = Convert.ToInt32((string)results["retweet_count"]);

                string xxx = (string)results["retweeted"];

                if ((string)results["retweeted"] == "False")
                {
                    _retweet = 0;
                }
                else
                {
                    _retweet = 1;
                }

                JToken _users = results["user"];

                if (_users is JValue)
                {
                    //html += "Tweet : " + (string)_users + "<br/>";
                }
                else if (_users is JObject)
                {
                    string dusers = "[" + _users + "]";
                    JArray jsonDat = JArray.Parse(dusers);
                    for (int x = 0; x < jsonDat.Count(); x++)
                    {
                        idUserTweet = jsonDat[x]["id_str"].ToString();
                        userNameTweet = jsonDat[x]["screen_name"].ToString();
                    }
                }

                JToken _entities = results["entities"];

                if (_entities is JObject)
                {
                    string d_entities = "[" + _entities + "]";
                    JArray jsonDat = JArray.Parse(d_entities);

                    int a = jsonDat[0]["hashtags"].Count();
                    int b = jsonDat[0]["user_mentions"].Count();

                    if (a > 0)
                    {
                        JArray jsonDat1 = JArray.Parse(jsonDat[0]["hashtags"].ToString());
                        for (int i = 0; i < jsonDat1.Count(); i++)
                        {
                            if (i == 0)
                            {
                                hashtags = jsonDat1[i]["text"].ToString();
                            }
                            else
                            {
                                hashtags += "," + jsonDat1[i]["text"].ToString();
                            }
                        }
                    }
                    if (b > 0)
                    {
                        JArray jsonDat2 = JArray.Parse(jsonDat[0]["user_mentions"].ToString());
                        for (int i = 0; i < jsonDat2.Count(); i++)
                        {
                            if (i == 0)
                            {
                                idMentions = jsonDat2[i]["id_str"].ToString();
                                userMentions = jsonDat2[i]["screen_name"].ToString();
                            }
                            else
                            {
                                idMentions += "," + jsonDat2[i]["id_str"].ToString();
                                userMentions += "," + jsonDat2[i]["screen_name"].ToString();
                            }
                        }
                    }

                }

                insertMontitoringTimeline(idStatus, status, idUserTweet, userNameTweet, idMentions, userMentions, hashtags, idReplyStatus, idReplyUser, idReplyUsername, _retweet, _retweetCount);
                getProfile();
                getHomeTimeLine();
                tbTweet.Text = "";
            }
            catch (Exception)
            {
                throw;
            }

            containerLeft2.Attributes["style"] = "visibility:hidden";
            btnViewPolling.Attributes["style"] = "visibility:vissible";
        }

        protected void btnClosedPoll_Click(object sender, EventArgs e)
        {
            containerLeft2.Attributes["style"] = "visibility:hidden";
            btnViewPolling.Attributes["style"] = "visibility:vissible";
        }

        public void GetDataUser(string idString, string userName, int followers, int friends, int favorites, string location, 
            string description, int statusCount)
        {
            string query = "";

            query = "sp_SynchUserTwitter";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idString", idString);
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@followers", followers);
            cmd.Parameters.AddWithValue("@friends", friends);
            cmd.Parameters.AddWithValue("@favorites", favorites);
            cmd.Parameters.AddWithValue("@statusCount", statusCount);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            
        }
    }
}