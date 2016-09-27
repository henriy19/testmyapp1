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
    public partial class managerules : System.Web.UI.Page
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
            string qry1 = " select * from managerules where isActive=1";
            SqlCommand cmd = new SqlCommand(qry1, conn);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string x = ids.Text;
            string v1 = ddlVar1.SelectedItem.Text;
            string v2 = ddlvar2.SelectedItem.Text;
            string cond = ddlCondition.SelectedItem.Text;
            string rest = ddlResult.SelectedItem.Text;
            string descs = v1 + " " + cond + " " + v2 + " then " + rest;


            string query = "";
            if (x == "" || x == string.Empty)
            {
                query += "declare @x int; ";
                query += "set @x = (select count(*) from managerules where variable1='" + v1 + "' and variable2='" + v2 + "' and ";
                query += "condition='" + cond + "' and result='" + rest + "') ";
                query += "if(@x=0) ";
                query += "begin ";
                query += "insert into managerules (variable1,variable2,condition,result,descs,isActive) ";
                query += "values('" + v1 + "','" + v2 + "','" + cond + "','" + rest + "','" + descs + "',1)";
                query += "end";
            }
            else
            {
                query += "update managerules set variable1='" + v1 + "', variable2='" + v2 + "', condition='" + cond + "',";
                query += "result='" + rest + "',descs='" + descs + "',isActive=1 where id=" + x + " ";

            }
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

            getDataList();
            ids.Text = "";
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string sa = e.Row.DataItem.ToString();
            }
        }

        protected void gvListData_SelectedIndexChanged(object sender, EventArgs e)
        {
            ids.Text = gvListData.SelectedRow.Cells[0].Text;
            ddlVar1.SelectedItem.Text = gvListData.SelectedRow.Cells[1].Text;
            ddlvar2.SelectedItem.Text = gvListData.SelectedRow.Cells[3].Text;
            ddlCondition.SelectedItem.Text = gvListData.SelectedRow.Cells[2].Text;
            ddlResult.SelectedItem.Text = gvListData.SelectedRow.Cells[4].Text;
        }
    }
}