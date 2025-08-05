using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ProductcatApp.Models;

namespace ProductcatApp.DAL
{
    public class CategoryDAL
    {
        // Connection string from Web.config
        private readonly string conStr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        /// <summary>
        /// Fetch all categories from the database.
        /// </summary>
        /// <returns>List of Category objects</returns>
        public List<Category> GetAllCategories()
        {
            var list = new List<Category>();
            try
            {
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
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and rethrow
                throw new Exception("Error fetching categories: " + ex.Message, ex);
            }
            return list;
        }

        /// <summary>
        /// Fetch a single category by its ID.
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category object or null if not found</returns>
        public Category GetCategoryById(int id)
        {
            Category c = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching category by ID: " + ex.Message, ex);
            }
            return c;
        }

        /// <summary>
        /// Insert a new category into the database.
        /// </summary>
        /// <param name="cat">Category object to insert</param>
        public void InsertCategory(Category cat)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Category (CategoryName) VALUES (@CategoryName)", con);
                    cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting category: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="cat">Category object with updated values</param>
        public void UpdateCategory(Category cat)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error updating category: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete a category by its ID.
        /// </summary>
        /// <param name="id">Category ID to delete</param>
        public void DeleteCategory(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE CategoryId=@CategoryId", con);
                    cmd.Parameters.AddWithValue("@CategoryId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category: " + ex.Message, ex);
            }
        }
    }
}
