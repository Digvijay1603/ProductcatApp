using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ProductcatApp.Models;

namespace ProductcatApp.DAL
{
    public class ProductDAL
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public List<Product> GetPagedProducts(int page, int pageSize, out int totalCount)
        {
            var list = new List<Product>();
            totalCount = 0;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM Product", con);
                con.Open();
                totalCount = (int)countCmd.ExecuteScalar();
            }

            int start = (page - 1) * pageSize + 1;
            int end = page * pageSize;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
            WITH OrderedProducts AS
            (
                SELECT ROW_NUMBER() OVER (ORDER BY ProductId) AS RowNum,
                       p.ProductId, p.ProductName, p.CategoryId, c.CategoryName
                FROM Product p
                INNER JOIN Category c ON p.CategoryId = c.CategoryId
            )
            SELECT * FROM OrderedProducts WHERE RowNum BETWEEN @Start AND @End";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@End", end);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = dr["CategoryName"].ToString()
                    });
                }
            }

            return list;
        }

        public Product GetProductById(int productId)
        {
            Product p = null;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName
                      FROM Product p
                      INNER JOIN Category c ON p.CategoryId = c.CategoryId
                      WHERE p.ProductId = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    p = new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = dr["CategoryName"].ToString()
                    };
                }
            }
            return p;
        }

        public void InsertProduct(Product p)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Product (ProductName, CategoryId) VALUES (@ProductName, @CategoryId)", con);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", p.CategoryId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product p)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Product SET ProductName=@ProductName, CategoryId=@CategoryId WHERE ProductId=@ProductId", con);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", p.CategoryId);
                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE ProductId=@ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
