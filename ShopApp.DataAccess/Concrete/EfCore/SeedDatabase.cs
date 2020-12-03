using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(_categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(_products);
                    context.AddRange(_productCategories);
                }
            }

            context.SaveChanges();
        }

        private static Category[] _categories =
        {
            new Category() {Name = "Cell Phone", Url = "Cell-Phone"},
            new Category() {Name = "Laptop", Url = "Laptop"},
            new Category() {Name = "Television", Url = "Television"},
            new Category() {Name = "Tablet", Url="Tablet"},
            new Category() {Name = "Component", Url="Component"},

        };

        private static Product[] _products =
        {
            new Product() {Name = "Msi Ge75 Gaming Laptop",Url="Msi-Ge75-Gaming-Laptop", Description = "This laptop is gaming laptop.", Price = 1200, ImageUrl = "b_msi-ge75-8se-230xtr-1.png",IsApproved=true,IsHome=true},
            new Product() {Name = "Asus G731" , Url="Asus-G731", Description = "This laptop is nice", Price = 1000, ImageUrl = "m_asus-g731gw-ev019-1.jpg", IsApproved = true,IsHome=true},
            new Product() {Name = "Huawei P40 Pro",Url="Huawei-P40-Pro", Description = "Awesome cell phone", Price = 700, ImageUrl = "m_huawei-p40-pro-5.png",IsApproved=true},
            new Product() {Name = "Oneplus 8 Pro",Url="Oneplus-8-Pro", Description = "Nice phone", Price = 600, ImageUrl ="m_oneplus-8-pro-256gb-14.png",IsApproved=true,IsHome=true},
            new Product() {Name = "Samsung Galaxy Note 20 Ultra",Url="Samsung-Galaxy-Note-20-Ultra", Description = "Incredible cell phone", Price = 800, ImageUrl ="m_samsung-galaxy-note-20-ultra-1.png",IsApproved=true },
            new Product() {Name = "Samsung Galaxy S20 Ultra",Url="Samsung-Galaxy-S20-Ultra", Description = "Awesome cell phone", Price = 650, ImageUrl = "m_samsung-galaxy-s20-ultra-1.png",IsApproved=false},
        };

        static ProductCategory[] _productCategories = {
            new ProductCategory(){Product = _products[0], Category = _categories[1]},
            new ProductCategory(){Product = _products[0], Category = _categories[4]},

            new ProductCategory(){Product = _products[1], Category = _categories[1]},
            new ProductCategory(){Product = _products[1], Category = _categories[4]},

            new ProductCategory(){Product = _products[2], Category = _categories[0]},
            new ProductCategory(){Product = _products[2], Category = _categories[4]},

            new ProductCategory(){Product = _products[3], Category = _categories[0]},
            new ProductCategory(){Product = _products[3], Category = _categories[4]},

            new ProductCategory(){Product = _products[4], Category = _categories[0]},
            new ProductCategory(){Product = _products[4], Category = _categories[4]},

            new ProductCategory(){Product = _products[5], Category = _categories[0]},
            new ProductCategory(){Product = _products[5], Category = _categories[4]},
   };
    }
}
