using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        private readonly IRepository<ProductCategory> productCategoryRepo;

        public ProductCategoryManagerController(IRepository<ProductCategory> productCategoryRepo)
        {
           this.productCategoryRepo = productCategoryRepo;
        }

        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            var categories = productCategoryRepo.Collection().ToList();

            return View(categories);
        }

        public ActionResult Create()
        {
            var category = new ProductCategory();
            return View(category);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                productCategoryRepo.Insert(productCategory);
                productCategoryRepo.Commit();
                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(string id)
        {
            var category = productCategoryRepo.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            var productCategoryToEdit = productCategoryRepo.Find(id);
            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }
                else
                {
                    productCategoryToEdit.Category = productCategory.Category;
                 
                    productCategoryRepo.Commit();

                    return RedirectToAction("Index");
                }
            }
        }


        public ActionResult Delete(string id)
        {
            var category = productCategoryRepo.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var category = productCategoryRepo.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                productCategoryRepo.Delete(id);
                productCategoryRepo.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}