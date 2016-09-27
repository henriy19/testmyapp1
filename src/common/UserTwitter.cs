using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.common
{
    public class UserTwitter
    {
        SqlConnection conn = new SqlConnection("SERVER=.;Database=socmed_analytics_db;integrated security=true");

        public int GetDataUser()
        {
            int rest = 0;
            string query = "";

            query = "select * from monitoringTimeline where hashtags='TestingPolling' order by id desc";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query,conn);

            try
            {
                rest = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return rest;


        }
    }
}