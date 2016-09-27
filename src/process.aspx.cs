using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.IO.Compression;
using TweetSharp.Serialization;
using Newtonsoft.Json;
using ASPSnippets.TwitterAPI;
using Twitterizer;
using System.Configuration;

namespace WebApplication1
{
    public partial class process : System.Web.UI.Page
    {
        //SqlConnection conn = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constring"].ConnectionString);
        string key = "";
        string value = "";
        public string urls;
        //public string urls = "https://api.twitter.com/1.1/users/search.json";
        //public string urls = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        //public string urls = "https://api.twitter.com/1.1/followers/list.json";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnProcess_Click(object sender, System.EventArgs e)
        {
            key = txkey1.Text;
            value = txvalue1.Text;

            string postget = DropDownList1.SelectedItem.Text;
            string descs = DropDownList2.SelectedItem.Value;
            string url1 = "";
            string ext = "";
            string req = "";

            string query1 = "";
            query1 += "select * from urlData where descs='" + descs + "'";
            conn.Open();
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            DataSet ds = new DataSet();
            sda1.Fill(ds);

            url1 = ds.Tables[0].Rows[0][1].ToString();
            ext = ds.Tables[0].Rows[0][4].ToString();
            req = ds.Tables[0].Rows[0][3].ToString();

            urls = url1 + ext;

            conn.Close();


            if (txvalue1.Text == "")
            {
                value = "me";
            }
            if (req != "")
            {
                key = req;
            }


            DataSet dset = getAuth();
            string oauth_token = dset.Tables[0].Rows[0][4].ToString();
            string oauth_token_secret = dset.Tables[0].Rows[0][5].ToString();
            string oauth_consumer_key = dset.Tables[0].Rows[0][2].ToString();
            string oauth_consumer_secret = dset.Tables[0].Rows[0][3].ToString();

            if (postget == "GET")
            {
                getDataFromAPI(urls, key, value, oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret, descs);
            }
            else
            {
                
                if (tbAreaInput.Text=="")
                {
                    value = "me";
                }
                else
                {
                    value = tbAreaInput.Text;
                }
                postDataFromAPI(urls, key, value, oauth_token, oauth_token_secret, oauth_consumer_key, oauth_consumer_secret);
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

        public void getDataFromAPI(string resource_url, string q, string v, string token, string token_secret, string consumer_key, string consumer_secret, string info)
        {
            var postget = DropDownList1.SelectedItem.Text;
            // oauth application keys
            var oauth_token = token; //"insert here...";
            var oauth_token_secret = token_secret; //"insert here...";
            var oauth_consumer_key = consumer_key;// = "insert here...";
            var oauth_consumer_secret = consumer_secret;// = "insert here...";

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
            if (q == string.Empty || q == "")
            {
                var postBody = "q=" + Uri.EscapeDataString(v);
                resource_url += "?" + postBody;
            }
            else
            {
                var postBody = "" + q + "=" + Uri.EscapeDataString(v);
                resource_url += "?" + postBody;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = postget;
            request.ContentType = "application/x-www-form-urlencoded";
            //jsonData.InnerHtml = objText;/**/
            string html = "";
            try
            {
                //JArray jsonDat = JArray.Parse(objText);
                //for (int x = 0; x < jsonDat.Count(); x++)
                //{
                //    html += jsonDat[x]["id"].ToString() + "<br/>";
                //    //html += jsonDat[x]["text"].ToString() + "<br/>";
                //    html += jsonDat[x]["name"].ToString() + "<br/>";
                //    //html += jsonDat[x]["created_at"].ToString() + "<br/>";

                //}
                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var objText = reader.ReadToEnd();
                insertRawData(objText, oauth_timestamp);
                jsonData.InnerHtml = objText;

                if (info == "timeline")
                {
                    JArray jsonDat = JArray.Parse(objText);
                    for (int x = 0; x < jsonDat.Count(); x++)
                    {
                        html += "text : " + jsonDat[x]["text"].ToString() + "<br/>";
                        html += "created_at : " + jsonDat[x]["created_at"].ToString() + "<br/>";
                        html += "name : " + jsonDat[x]["user"]["name"].ToString() + "<br/>";
                        html += "location : " + jsonDat[x]["user"]["location"].ToString() + "<br/>";
                        html += "time_zone : " + jsonDat[x]["user"]["time_zone"].ToString() + "<br/>";
                        //html += "retweet_count : " + jsonDat[x]["retweeted_status"]["possibly_sensitive"].ToString() + "<br/>";
                        //html += "favorite_count : " + jsonDat[x]["retweeted_status"]["favorite_count"].ToString() + "<br/>";
                        //html += "screen_name : " + jsonDat[x]["retweeted_status"]["user"]["screen_name"].ToString() + "<br/>";
                        //html += "followers_count : " + jsonDat[x]["retweeted_status"]["user"]["followers_count"].ToString() + "<br/>";
                        html += "<hr />";

                    }
                    jsonData.InnerHtml = html;
                }

                if (info == "follower")
                {
                    JObject results = JObject.Parse(objText);
                    html += "<hr />";
                    foreach (var result in results["users"])
                    {
                        html += "id : " + (string)result["id"] + "<br/>";
                        html += "name : " + (string)result["name"] + "<br/>";
                        html += "screen_name : " + (string)result["screen_name"] + "<br/>";
                        html += "location : " + (string)result["location"] + "<br/>";
                        html += "description : " + (string)result["description"] + "<br/>";
                        html += "followers_count : " + (string)result["followers_count"] + "<br/>";
                        html += "statuses_count : " + (string)result["statuses_count"] + "<br/>";
                        html += "favourites_count : " + (string)result["favourites_count"] + "<br/>";
                        html += "time_zone : " + (string)result["time_zone"] + "<br/>";
                        html += "<hr />";
                    }
                    jsonData.InnerHtml = html;
                }

                if (info == "searching")
                {
                    JObject results = JObject.Parse(objText);
                    html += "<hr />";
                    foreach (var result in results["statuses"])
                    {
                        html += "Tweet Date : " + (string)result["created_at"] + "<br/>";
                        html += "ID : " + (string)result["id"] + "<br/>";
                        string _text =(string)result["text"];
                        html += "Tweet : " + _text + "<br/>";
                        string _tweetBy = "";
                        string sourceStatus = "";
                        string sourceText = "";
                                                

                        JToken _users = result["user"];

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
                                _tweetBy = jsonDat[x]["screen_name"].ToString();
                                html += "Tweet by : " + _tweetBy + "<br/>";
                            }
                        }

                        JToken _entities = result["entities"];
                        if (_entities is JValue)
                        {
                            html += "entities : " + (string)_entities + "<br/>";
                        }
                        else if (_entities is JObject)
                        {
                            string d_entities = "[" + _entities + "]";

                            bool checkMedia = d_entities.Contains("media");
                            if (checkMedia)
                            {
                                JArray jsonDat = JArray.Parse(d_entities);
                                for (int x = 0; x < jsonDat.Count(); x++)
                                {
                                    string medias = jsonDat[x]["media"].ToString();

                                    bool check = medias.Contains("source_status_id");
                                    if (check)
                                    {
                                        JArray jsonDat1 = JArray.Parse(medias);
                                        for (int y = 0; y < jsonDat1.Count(); y++)
                                        {
                                            sourceStatus = jsonDat1[x]["source_status_id"].ToString();
                                            html += " Source ID: " + sourceStatus + "<br/>";
                                        }
                                    }
                                }
                            }
                            
                        }

                        JToken _retweeted_status = result["retweeted_status"];
                        if (_retweeted_status is JObject)
                        {
                            string d_retweeted_status = "[" + _retweeted_status + "]";
                            JArray jsonDat = JArray.Parse(d_retweeted_status);
                            for (int x = 0; x < jsonDat.Count(); x++)
                            {
                                sourceText = jsonDat[x]["text"].ToString();
                                //html += "Source_text : " + sourceText + "<br/>";
                            }
                        }

                        html += "<hr />";

                        string docid = oauth_timestamp + "-" + sourceStatus;
                        string query = " ";
                        query += "insert into rawAnalytics(timestamp,descs,tweetby,sourceId,sourceDescs,docId,isActive) ";
                        query += "values ('" + oauth_timestamp + "','" + _text + "','" + _tweetBy + "','" + sourceStatus + "', ";
                        query += "'" + sourceText + "','" + docid + "',1)";

                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);

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
                    jsonData.InnerHtml = html;
                }

                if (info == "friend_details")
                {
                    JObject results = JObject.Parse(objText);
                    html += "<hr />";
                    foreach (var result in results["users"])
                    {
                        html += "id : " + (string)result["id"] + "<br/>";
                        html += "name : " + (string)result["name"] + "<br/>";
                        html += "screen_name : " + (string)result["screen_name"] + "<br/>";
                        html += "location : " + (string)result["location"] + "<br/>";
                        html += "description : " + (string)result["description"] + "<br/>";
                        html += "url : " + (string)result["url"] + "<br/>";
                        html += "followers_count : " + (string)result["followers_count"] + "<br/>";
                        html += "friends_count : " + (string)result["friends_count"] + "<br/>";
                        html += "listed_count : " + (string)result["listed_count"] + "<br/>";
                        html += "created_at : " + (string)result["created_at"] + "<br/>";
                        html += "favourites_count : " + (string)result["favourites_count"] + "<br/>";
                        html += "time_zone : " + (string)result["time_zone"] + "<br/>";
                        html += "statuses_count : " + (string)result["statuses_count"] + "<br/>";

                        html += "<hr />";
                    }
                    jsonData.InnerHtml = html;
                }
            }
            catch (Exception twit_error)
            {
                jsonData.InnerHtml = html + twit_error.ToString();
            }
        }

        public void postDataFromAPI(string resource_url, string q, string v, string token, string token_secret, string consumer_key, string consumer_secret)
        {
            var postget = DropDownList1.SelectedItem.Text;
            // oauth application keys
            var oauth_token = token; //"insert here...";
            var oauth_token_secret = token_secret; //"insert here...";
            var oauth_consumer_key = consumer_key;// = "insert here...";
            var oauth_consumer_secret = consumer_secret;// = "insert here...";

            var tokens = new OAuthTokens()
            {
                AccessToken = oauth_token,
                AccessTokenSecret = oauth_token_secret,
                ConsumerKey = oauth_consumer_key,
                ConsumerSecret = oauth_consumer_secret
            };

            string html = "";
            if (v == "" || v == string.Empty)
            {
                v = "no-status";
            }

            try
            {
                var response = TwitterStatus.Update(tokens, v);
                string rawdata = response.Content.ToString();

                JObject results = JObject.Parse(rawdata);

                html += "<hr />";

                html += "created_at : " + (string)results["created_at"] + "<br/>";
                html += "id : " + (string)results["id"] + "<br/>";
                html += "text : " + (string)results["text"] + "<br/>";

                html += "<hr />";
                jsonData.InnerHtml = html;
            }
            catch (Exception twit_error)
            {
                jsonData.InnerHtml = html + twit_error.ToString();
            }

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

        protected void intemChanged(object sender, EventArgs e)
        {
            string poststatus = "";
            poststatus = DropDownList2.SelectedItem.Text;
            if (poststatus == "posting_status")
            {
                Label1.Visible = true;
                tbAreaInput.Visible = true;
                DropDownList1.SelectedItem.Text = "POST";
            }
            else
            {
                Label1.Visible = false;
                tbAreaInput.Visible = false;
                DropDownList1.SelectedItem.Text = "GET";
            }
            
        }

        protected void methodsChanged(object sender, EventArgs e)
        {
            string method = "";
            method = DropDownList1.SelectedItem.Text;
            //if (method == "POST")
            //{
            //    Label1.Visible = true;
            //    tbAreaInput.Visible = true;
            //    DropDownList1.SelectedItem.Text = "POST";
            //    DropDownList2.SelectedItem.Text = "posting_status";
            //}
            //else
            //{
            //    Label1.Visible = false;
            //    tbAreaInput.Visible = false;
            //    DropDownList1.SelectedItem.Text = "GET";
            //    DropDownList2.SelectedItem.Text = "follower";
            //}
        }
    }
}