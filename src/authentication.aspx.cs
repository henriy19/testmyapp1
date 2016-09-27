using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class authentication : System.Web.UI.Page
    {
        //SqlConnection conn = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constring"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                listdata();
            }
        }

        private void listdata() {
            conn.Open();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string qry1 = " select top 1 * from authentication";
            SqlCommand cmd = new SqlCommand(qry1, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);

            ids.Text = ds.Tables[0].Rows[0][0].ToString();
            txConsumerKey.Text = ds.Tables[0].Rows[0][2].ToString();
            txConsumerSecret.Text = ds.Tables[0].Rows[0][3].ToString();
            txToken.Text = ds.Tables[0].Rows[0][4].ToString();
            txTokenSecret.Text = ds.Tables[0].Rows[0][5].ToString();
            conn.Close();
        }
        protected void btnClear_Click(object seder, System.EventArgs e)
        {
            listdata();
        }

        protected void btnUpdate_Click(object seder, System.EventArgs e)
        {
            string _id = ids.Text;
            string _type = DropDownList1.SelectedItem.Value;
            string _consumerKey = txConsumerKey.Text;
            string _consumerSecret = txConsumerSecret.Text;
            string _token = txToken.Text;
            string _tokenSecret = txTokenSecret.Text;
            string _signature = DropDownList2.SelectedItem.Value;

            string query = "";
            query += "update authentication set type ='" + _type + "', costomerKey='" + _consumerKey + "',";
            query += "costomerSecret='" + _consumerSecret + "',token='" + _token + "', tokenSecret='" + _tokenSecret + "',";
            query += "signatureMethod ='" + _signature + "' where id=" + _id + "";

            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
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