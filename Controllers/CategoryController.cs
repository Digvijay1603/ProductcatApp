using System;
using System.Web.Mvc;
using ProductcatApp.DAL;
using ProductcatApp.Models;

namespace ProductcatApp.Controllers
{
    /// <summary>
    /// Controller to handle CRUD operations for Category.
    /// </summary>
    public class CategoryController : Controller
    {
        // Data Access Layer for Category
        private readonly CategoryDAL dal = new CategoryDAL();

        /// <summary>
        /// Displays the list of all categories.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                // Fetch all categories from the database
                var categories = dal.GetAllCategories();
                return View(categories);
            }
            catch (Exception ex)
            {
                // Handle errors while loading categories
                ViewBag.Error = "Failed to load categories. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the Create Category form.
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles Create Category form submission.
        /// </summary>
        [HttpPost]
        public ActionResult Create(Category c)
        {
            try
            {
                if (ModelState.IsValid) // Check if model validation passed
                {
                    // Insert new category into the database
                    dal.InsertCategory(c);
                    return RedirectToAction("Index");
                }
                return View(c); // Reload form if validation fails
            }
            catch (Exception ex)
            {
                // Handle errors while creating a category
                ViewBag.Error = "Failed to create category. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the Edit Category form for a specific category.
        /// </summary>
        public ActionResult Edit(int id)
        {
            try
            {
                // Fetch category details by ID
                var category = dal.GetCategoryById(id);
                if (category == null)
                    return HttpNotFound(); // Return 404 if not found

                return View(category);
            }
            catch (Exception ex)
            {
                // Handle errors while loading category for edit
                ViewBag.Error = "Failed to load category for edit. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Handles Edit Category form submission.
        /// </summary>
        [HttpPost]
        public ActionResult Edit(Category c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Update the category in the database
                    dal.UpdateCategory(c);
                    return RedirectToAction("Index");
                }
                return View(c); // Reload form if validation fails
            }
            catch (Exception ex)
            {
                // Handle errors while updating a category
                ViewBag.Error = "Failed to update category. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the Delete Category confirmation page.
        /// </summary>
        public ActionResult Delete(int id)
        {
            try
            {
                // Fetch category details by ID
                var category = dal.GetCategoryById(id);
                if (category == null)
                    return HttpNotFound(); // Return 404 if not found

                return View(category);
            }
            catch (Exception ex)
            {
                // Handle errors while loading category for deletion
                ViewBag.Error = "Failed to load category for deletion. " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Handles Delete Category confirmation.
        /// </summary>
        [HttpPost]
        public ActionResult Delete(Category c)
        {
            try
            {
                // Delete the category from the database
                dal.DeleteCategory(c.CategoryId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle errors while deleting a category
                ViewBag.Error = "Failed to delete category. " + ex.Message;
                return View("Error");
            }
        }
    }
}
