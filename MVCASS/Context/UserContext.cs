using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCASS.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;
namespace MVCASS.Context
{
    public class UserContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public UserContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public void AddUser(User u)
        {
            string connectionString =
            ConfigurationManager.ConnectionStrings["UserContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddNewUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@username";
                paramName.Value = u.username;
                cmd.Parameters.Add(paramName);

                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "@email";
                paramEmail.Value = u.email;
                cmd.Parameters.Add(paramEmail);

                SqlParameter paramPassword = new SqlParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = u.password;
                cmd.Parameters.Add(paramPassword);

                con.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public bool Login(string email, string password)

        {
           
            bool loginSuccessful = false;
           
            string connectionString =
           ConfigurationManager.ConnectionStrings["UserContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    

                    string query = "Select * from tbl_user Where email = '" + email + "' and password = '" + password + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataTable dtbl = new DataTable();
                    sda.Fill(dtbl);
                    if (dtbl.Rows.Count == 1)
                    {
                        foreach (DataRow row in dtbl.Rows)
                        {
                            HttpContext.Current.Session["UID"] = row["id"].ToString();
                            HttpContext.Current.Session["UUSERNAME"] = row["username"].ToString();
                            HttpContext.Current.Session["UEMAIL"] = row["email"].ToString();
                        }
                        loginSuccessful = true;
                    }
                }
                catch(Exception ex)
                {

                }    
                return loginSuccessful;
            }
        }
      

    }
}
