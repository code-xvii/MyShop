﻿using Microsoft.SqlServer.Server;
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
        private readonly InMemoryRepository<Product> context;
        private readonly InMemoryRepository<ProductCategory> productCategoryRepo;

        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            productCategoryRepo = new InMemoryRepository<ProductCategory>();
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            var products = context.Collection().ToList();

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
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(string id)
        {
            var product = context.Find(id);
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
            var productToEdit = context.Find(id);
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

                    context.Commit();

                    return RedirectToAction("Index");
                }
            }
        }


        public ActionResult Delete(string id)
        {
            var product = context.Find(id);
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
            var product = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}