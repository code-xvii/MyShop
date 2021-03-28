﻿using MyShop.Core.Models;
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
        private readonly ProductCategoryRepository context;

        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            var categories = context.Collection().ToList();

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
                context.Insert(productCategory);
                context.Commit();
                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(string id)
        {
            var category = context.Find(id);
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
            var productCategoryToEdit = context.Find(id);
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
                 
                    context.Commit();

                    return RedirectToAction("Index");
                }
            }
        }


        public ActionResult Delete(string id)
        {
            var category = context.Find(id);
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
            var category = context.Find(id);
            if (category == null)
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