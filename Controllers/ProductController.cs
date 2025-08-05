using System.Web.Mvc;
using ProductcatApp.DAL;
using ProductcatApp.Models;

namespace ProductcatApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDAL prodDal = new ProductDAL();
        private readonly CategoryDAL catDal = new CategoryDAL();

        public ActionResult Index(int page = 1)
        {
            int pageSize = 10;
            int totalCount;
            var products = prodDal.GetPagedProducts(page, pageSize, out totalCount);

            ViewBag.TotalPages = (totalCount + pageSize - 1) / pageSize;
            ViewBag.CurrentPage = page;

            return View(products);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = catDal.GetAllCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            prodDal.InsertProduct(p);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Categories = catDal.GetAllCategories();
            var product = prodDal.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            prodDal.UpdateProduct(p);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = prodDal.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Product p)
        {
            prodDal.DeleteProduct(p.ProductId);
            return RedirectToAction("Index");
        }
    }
}
