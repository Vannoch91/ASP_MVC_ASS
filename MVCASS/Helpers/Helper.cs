using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MVCASS.Helpers
{
    public class Helper
    {
        public static string strKey = "U2A9/R*41FD412+4-123";
       public static  string connectionString =
          ConfigurationManager.ConnectionStrings["UserContext"].ConnectionString;
        public static string MyEncrypt(string strData)
        {
            // return Encrypt.EncryptString(strData, strKey);
            return strData;
        }
        public static string MyDecrypt(string strData)
        {
            return strData;
            return Encrypt.DecryptString(strData, strKey);
        }
        public static bool checkSession()
        {
            if (HttpContext.Current.Session["UID"] == null)
            {
                return false;
            }
            return true;
        }
        public static string getAuthorName(int uid)
        {
            string authorName = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "Select username from tbl_user Where id =" + uid;
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataTable dtbl = new DataTable();
                    sda.Fill(dtbl);
                    if (dtbl.Rows.Count == 1)
                    {
                        foreach (DataRow row in dtbl.Rows)
                        {
                            authorName = row["username"].ToString();

                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return authorName;
            }
        }




        
    }
}