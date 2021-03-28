using Microsoft.SqlServer.Server;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<ProductCategory> productCategoryRepo;

        public ProductManagerController(IRepository<Product> productRepo, IRepository<ProductCategory> productCategoryRepo)
        {
            this.productRepo = productRepo;
            this.productCategoryRepo = productCategoryRepo;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            var products = productRepo.Collection().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();
            productManagerViewModel.Product = new Product();
            productManagerViewModel.ProductCategories = productCategoryRepo.Collection();

          
            return View(productManagerViewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                productRepo.Insert(product);
                productRepo.Commit();
                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(string id)
        {
            var product = productRepo.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();
                productManagerViewModel.Product = product;
                productManagerViewModel.ProductCategories = productCategoryRepo.Collection();

                return View(productManagerViewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            var productToEdit = productRepo.Find(id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    productRepo.Commit();

                    return RedirectToAction("Index");
                }
            }
        }


        public ActionResult Delete(string id)
        {
            var product = productRepo.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var product = productRepo.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                productRepo.Delete(id);
                productRepo.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}