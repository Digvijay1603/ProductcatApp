using System.Web.Mvc;
using ProductcatApp.DAL;
using ProductcatApp.Models;

namespace ProductcatApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryDAL dal = new CategoryDAL();

        public ActionResult Index()
        {
            var categories = dal.GetAllCategories();
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category c)
        {
            dal.InsertCategory(c);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var category = dal.GetCategoryById(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category c)
        {
            dal.UpdateCategory(c);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var category = dal.GetCategoryById(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(Category c)
        {
            dal.DeleteCategory(c.CategoryId);
            return RedirectToAction("Index");
        }
    }
}
