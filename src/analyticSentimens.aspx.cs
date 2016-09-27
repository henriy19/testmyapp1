using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebApplication1
{
    public partial class analyticSentimens : System.Web.UI.Page
    {
        //SqlConnection conn = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constring"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getDataList();
            }
        }

        private void getDataList()
        {
            conn.Open();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("sp_analyticsSentiment", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            gvListData.DataSource = ds;
            gvListData.DataBind();
            conn.Close();
        }

        protected void gvListData_PageIndexChange(object sender, GridViewPageEventArgs e)
        {
            gvListData.PageIndex = e.NewPageIndex;
            getDataList();
        }

        private void getAnalyticsData()
        {
            string query1 = "";
            string query2 = "";
            string query3 = "";
            string query4 = "";
            string query5 = "";

            query1 = "select docid from [rawAnalytics] where sourcedescs <> '' group by docid ";
            conn.Open();
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            sda1.Fill(ds1);
            conn.Close();

            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];

            string docid = "";

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                docid = dt1.Rows[i][0].ToString();

                query2 = " select * from rawAnalytics where docid='" + docid + "' and sourcedescs <> ''";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                sda2.Fill(ds2);
                conn.Close();

                DataTable dt2 = new DataTable();
                dt2 = ds2.Tables[0];

                string word1 = "";

                for (int z = 0; z < dt2.Rows.Count; z++)
                {
                    int var1 = 0;
                    int var2 = 0;
                    string rawData = dt2.Rows[z][2].ToString();
                    string[] aRowData = rawData.Split(' ');

                    query3 = "select * from dictionary where status=1 and isActive=1 ";
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    SqlDataAdapter sda3 = new SqlDataAdapter(cmd3);
                    DataSet ds3 = new DataSet();
                    sda3.Fill(ds3);
                    conn.Close();

                    DataTable dt3 = new DataTable();
                    dt3 = ds3.Tables[0];

                    for (int q = 0; q < dt3.Rows.Count; q++)
                    {
                        word1 = dt3.Rows[q][1].ToString();

                        for (int a = 0; a < aRowData.Count(); a++)
                        {
                            string word2 = aRowData[a].ToString();
                            if (word1 == word2)
                            {
                                var1 = var1 + 1;
                            }
                        }
                    }

                    query4 = "select * from dictionary where status=2 and isActive=1 ";
                    conn.Open();
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    SqlDataAdapter sda4 = new SqlDataAdapter(cmd4);
                    DataSet ds4 = new DataSet();
                    sda4.Fill(ds4);
                    conn.Close();

                    DataTable dt4 = new DataTable();
                    dt4 = ds4.Tables[0];

                    for (int r = 0; r < dt4.Rows.Count; r++)
                    {
                        word1 = dt4.Rows[r][1].ToString();

                        for (int a = 0; a < aRowData.Count(); a++)
                        {
                            string word2 = aRowData[a].ToString();
                            if (word1 == word2)
                            {
                                var2 = var2 + 1;
                            }
                        }
                    }

                    query5 = " insert into analyticsVariable(docid,rawDesc,v1,v2,inserted,isActive) ";
                    query5 += "values('" + docid + "','" + rawData + "'," + var1 + "," + var2 + ",0, 1) ";
                    conn.Open();
                    SqlCommand cmd5 = new SqlCommand(query5, conn);
                    try
                    {
                        cmd5.ExecuteNonQuery();
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

                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_insertAnalyticsCondition", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@docid", docid);

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

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            getAnalyticsData();
            getDataList();
        }
    }
}