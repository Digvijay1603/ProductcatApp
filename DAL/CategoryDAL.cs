using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ProductcatApp.Models;

namespace ProductcatApp.DAL
{
    public class CategoryDAL
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public List<Category> GetAllCategories()
        {
            var list = new List<Category>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Category
                    {
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = dr["CategoryName"].ToString()
                    });
                }
            }
            return list;
        }

        public Category GetCategoryById(int id)
        {
            Category c = null;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE CategoryId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    c = new Category
                    {
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = dr["CategoryName"].ToString()
                    };
                }
            }
            return c;
        }

        public void InsertCategory(Category cat)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Category (CategoryName) VALUES (@CategoryName)", con);
                cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCategory(Category cat)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Category SET CategoryName=@CategoryName WHERE CategoryId=@CategoryId", con);
                cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                cmd.Parameters.AddWithValue("@CategoryId", cat.CategoryId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE CategoryId=@CategoryId", con);
                cmd.Parameters.AddWithValue("@CategoryId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
