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
                }
            }

            context.SaveChanges();
        }

        private static Category[] _categories =
        {
            new Category() {Name = "Cell Phone"},
            new Category() {Name = "Laptop"},
            new Category() {Name = "Television"},
            new Category() {Name = "Tablet"},
            new Category() {Name = "Component"},

        };

        private static Product[] _products =
        {
            new Product() {Name = "Msi Ge75 Gaming Laptop", Description = "This laptop is gaming laptop.", Price = 1200, ImageUrl = "b_msi-ge75-8se-230xtr-1.png",IsApproved=true},
            new Product() {Name = "Asus G731" , Description = "This laptop is nice", Price = 1000, ImageUrl = "m_asus-g731gw-ev019-1.jpg", IsApproved = true},
            new Product() {Name = "Huawei P40 Pro", Description = "Awesome cell phone", Price = 700, ImageUrl = "m_huawei-p40-pro-5.png",IsApproved=true},
            new Product() {Name = "Oneplus 8 Pro", Description = "Nice phone", Price = 600, ImageUrl ="m_oneplus-8-pro-256gb-14.png",IsApproved=true },
            new Product() {Name = "Samsung Galaxy Note 20 Ultra", Description = "Incredible cell phone", Price = 800, ImageUrl ="m_samsung-galaxy-note-20-ultra-1.png",IsApproved=true },
            new Product() {Name = "Samsung Galaxy S20 Ultra", Description = "Awesome cell phone", Price = 650, ImageUrl = "m_samsung-galaxy-s20-ultra-1.png",IsApproved=false},
        };
    }
}
