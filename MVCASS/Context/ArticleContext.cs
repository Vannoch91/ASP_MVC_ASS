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
using System.Reflection.Emit;

namespace MVCASS.Context
{
    public class ArticleContext: DbContext
    {
        public static string connectionString =
            ConfigurationManager.ConnectionStrings["UserContext"].ConnectionString;
        public DbSet<Models.Articles> Articles { get; set; }
        public ArticleContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer<ArticleContext>(null);
            //base.OnModelCreating(modelBuilder);
            Database.SetInitializer<ArticleContext>(null);
        }

        public void AddNewArticle(Articles a)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("AddNewArticle", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter paramTitle = new SqlParameter();
                    paramTitle.ParameterName = "@title";
                    paramTitle.Value = a.title;
                    cmd.Parameters.Add(paramTitle);

                    SqlParameter paramDescription = new SqlParameter();
                    paramDescription.ParameterName = "@description";
                    paramDescription.Value = a.description;
                    cmd.Parameters.Add(paramDescription);

                    SqlParameter paramDate = new SqlParameter();
                    paramDate.ParameterName = "@created_at";
                    paramDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add(paramDate);

                    SqlParameter paramAuthor = new SqlParameter();
                    paramAuthor.ParameterName = "@author";
                    paramAuthor.Value = a.author;
                    cmd.Parameters.Add(paramAuthor);
                    con.Open();
                    cmd.ExecuteNonQuery();
                  
                }catch(Exception ex)
                {

                }
            }
        }
    }
}