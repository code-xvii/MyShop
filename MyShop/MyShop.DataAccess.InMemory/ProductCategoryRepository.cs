using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }


        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory category)
        {
            productCategories.Add(category);
        }

        public void Update(ProductCategory category)
        {
            ProductCategory categoryToUpdate = productCategories.Find(p => p.Id == category.Id);
            if (categoryToUpdate != null)
            {
                categoryToUpdate = category;
            }
            else
            {
                throw new Exception("Product Category not found");
            }

        }

        public ProductCategory Find(string id)
        {
            ProductCategory category = productCategories.Find(p => p.Id == id);
            if (category != null)
            {
                return category;
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string id)
        {
            ProductCategory categoryToDelete = productCategories.Find(p => p.Id == id);
            if (categoryToDelete != null)
            {
                productCategories.Remove(categoryToDelete);
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }
    }
}
