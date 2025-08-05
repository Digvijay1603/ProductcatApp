using System;
using System.Web.Mvc;
using ProductcatApp.DAL;
using ProductcatApp.Models;

namespace ProductcatApp.Controllers
{
    /// <summary>
    /// Controller to handle CRUD operations for Products with pagination.
    /// </summary>
    public class ProductController : Controller
    {
        // Data Access Layers for Product and Category
        private readonly ProductDAL prodDal = new ProductDAL();
        private readonly CategoryDAL catDal = new CategoryDAL();

        /// <summary>
        /// Displays a paginated list of products.
        /// </summary>
        /// <param name="page">Current page number (default is 1)</param>
        public ActionResult Index(int page = 1)
        {
            try
            {
                int pageSize = 10; // Number of records per page
                int totalCount;    // Total number of products in the database

                // Fetch paginated products
                var products = prodDal.GetPagedProducts(page, pageSize, out totalCount);

                // Calculate total pages and set current page for the view
                ViewBag.TotalPages = (totalCount + pageSize - 1) / pageSize;
                ViewBag.CurrentPage = page;

                return View(products);
            }
            catch (Exception ex)
            {
                // Handle errors while loading products
                ViewBag.Error = "Failed to load products. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the Create Product form with category dropdown.
        /// </summary>
        public ActionResult Create()
        {
            try
            {
                // Load all categories to populate the dropdown
                ViewBag.Categories = catDal.GetAllCategories();
                return View();
            }
            catch (Exception ex)
            {
                // Handle errors while loading categories
                ViewBag.Error = "Failed to load categories for product creation. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Handles Create Product form submission.
        /// </summary>
        [HttpPost]
        public ActionResult Create(Product p)
        {
            try
            {
                if (ModelState.IsValid) // Validate input
                {
                    // Insert new product into the database
                    prodDal.InsertProduct(p);
                    return RedirectToAction("Index");
                }

                // Reload categories if validation fails
                ViewBag.Categories = catDal.GetAllCategories();
                return View(p);
            }
            catch (Exception ex)
            {
                // Handle errors while creating product
                ViewBag.Error = "Failed to create product. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the Edit Product form with category dropdown.
        /// </summary>
        /// <param name="id">Product ID to edit</param>
        public ActionResult Edit(int id)
        {
            try
            {
                // Load categories for dropdown
                ViewBag.Categories = catDal.GetAllCategories();

                // Fetch product details by ID
                var product = prodDal.GetProductById(id);

                if (product == null)
                    return HttpNotFound(); // Return 404 if product not found

                return View(product);
            }
            catch (Exception ex)
            {
                // Handle errors while loading product for editing
                ViewBag.Error = "Failed to load product for editing. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Handles Edit Product form submission.
        /// </summary>
        [HttpPost]
        public ActionResult Edit(Product p)
        {
            try
            {
                if (ModelState.IsValid) // Validate input
                {
                    // Update product in the database
                    prodDal.UpdateProduct(p);
                    return RedirectToAction("Index");
                }

                // Reload categories if validation fails
                ViewBag.Categories = catDal.GetAllCategories();
                return View(p);
            }
            catch (Exception ex)
            {
                // Handle errors while updating product
                ViewBag.Error = "Failed to update product. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the Delete Product confirmation page.
        /// </summary>
        /// <param name="id">Product ID to delete</param>
        public ActionResult Delete(int id)
        {
            try
            {
                // Fetch product details by ID
                var product = prodDal.GetProductById(id);

                if (product == null)
                    return HttpNotFound(); // Return 404 if product not found

                return View(product);
            }
            catch (Exception ex)
            {
                // Handle errors while loading product for deletion
                ViewBag.Error = "Failed to load product for deletion. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Handles Delete Product confirmation.
        /// </summary>
        [HttpPost]
        public ActionResult Delete(Product p)
        {
            try
            {
                // Delete the product from the database
                prodDal.DeleteProduct(p.ProductId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle errors while deleting product
                ViewBag.Error = "Failed to delete product. " + ex.Message;
                return View("Error");
            }
        }
    }
}
