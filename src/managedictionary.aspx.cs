using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public partial class managedictionary : System.Web.UI.Page
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

        public void getDataList()
        {
            conn.Open();
            string query = "";
            query += "select description, ";
            query += "case when status=1 then 'Positive' ";
            query += "when status=2 then 'Negative' end as Status ";
            query += "from dictionary where isActive=1 ";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            gvListData.DataSource = ds;
            gvListData.DataBind();
            conn.Close();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txdesc.Text = "";
            txdesc.Focus();
        }

        protected void gvListData_PageIndexChange(object sender, GridViewPageEventArgs e)
        {
            gvListData.PageIndex = e.NewPageIndex;
            getDataList();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string descs = txdesc.Text;
            string status= DropDownList1.SelectedItem.Value.ToString();
            conn.Open();
            string query = "";
            query += "insert into dictionary (description,status,isActive) ";
            query += "values('" + descs + "'," + status + ",1) ";
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
            txdesc.Text = "";
        }

        protected void EditRow(object sender, GridViewEditEventArgs e)
        {
            gvListData.EditIndex = e.NewEditIndex;
            getDataList();
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvListData.EditIndex = -1;
            getDataList();
        }

        protected void DeleteDesc(object sender, EventArgs e)
        {
            LinkButton lnkRemove = (LinkButton)sender;
            string query = "update dictionary set isActive=0 where description='" + lnkRemove.CommandArgument + "' ";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            conn.Close();
            getDataList();
        }

    }
}