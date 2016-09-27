using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.ComponentModel;
using System.Configuration;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        //SqlConnection conn = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constring"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindChart1();
                BindChart2();
                BindChart3();
                BindChart4();

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "getName()", true);
            }
        }

        private void BindChart1()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            conn.Open();

            //SqlCommand cmd = new SqlCommand("sp_ResultChart", conn);
            SqlCommand cmd = new SqlCommand("sp_ResultChart1_Dummy", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            conn.Close();

            string info = "";
            dt = ds.Tables[0];
            info = dt.Rows[0][3].ToString();
            infoChart1.InnerHtml = info;
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }

            cTestChart1.Series[0].Points.DataBindXY(x, y);
            cTestChart1.Series[0].ChartType = SeriesChartType.Doughnut;
            cTestChart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            cTestChart1.Legends[0].Enabled = true;

        }

        private void BindChart2()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_resultChart2", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            conn.Close();

            string info = "";
            dt = ds.Tables[0];
            info = "What People Talking About";
            infoChart2.InnerHtml = info;
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][1].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][2]);
            }

            cTestChart2.Series[0].Points.DataBindXY(x, y);
            cTestChart2.Series[0].ChartType = SeriesChartType.Pie;
            cTestChart2.ChartAreas["ChartArea2"].Area3DStyle.Enable3D = false;
            cTestChart2.Legends[0].Enabled = true;

        }

        private void BindChart3()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_resultChart3", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            conn.Close();

            string info = "";
            dt = ds.Tables[0];
            info = "Mentions";
            infoChart3.InnerHtml = info;
            infoDay1.InnerHtml = "<span><h1>" + dt.Rows[0][2].ToString() + "</h1> </span> ";
            infoDay2.InnerHtml = "<span><h3> /" + dt.Rows[0][1].ToString() + "</h3>  </span>";
            infoHour1.InnerHtml = "<span><h2>" + dt.Rows[1][2].ToString() + "</h2> </span> ";
            infoHour2.InnerHtml = "<span><h3> /" + dt.Rows[1][1].ToString() + "</h3>  </span>";
            infoMinute1.InnerHtml = "<span><h3>" + dt.Rows[2][2].ToString() + "</h3> </span> ";
            infoMinute2.InnerHtml = "<span><h3> /Min</h3>  </span>";
        }

        private void BindChart4()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_ResultChart4", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            conn.Close();

            string info = "";
            dt = ds.Tables[0];
            info = "Positive";
            P1.InnerHtml = info;
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][1].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][2]);
            }

            ChartDiv1.Series[0].Points.DataBindXY(x, y);
            ChartDiv1.Series[0].ChartType = SeriesChartType.Column;
            ChartDiv1.ChartAreas["ChartAreaDiv1"].Area3DStyle.Enable3D = false;
            //ChartDiv1.Legends[0].Enabled = true;

            string info1 = "";
            dt = ds.Tables[1];
            info1 = "Negative";
            P2.InnerHtml = info1;
            string[] x1 = new string[dt.Rows.Count];
            int[] y1 = new int[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x1[i] = dt.Rows[i][1].ToString();
                y1[i] = Convert.ToInt32(dt.Rows[i][2]);
            }

            ChartDiv2.Series[0].Points.DataBindXY(x1, y1);
            ChartDiv2.Series[0].ChartType = SeriesChartType.Column;
            ChartDiv2.ChartAreas["ChartAreaDiv2"].Area3DStyle.Enable3D = false;
            //ChartDiv2.Legends[0].Enabled = true;

        }
    }
}