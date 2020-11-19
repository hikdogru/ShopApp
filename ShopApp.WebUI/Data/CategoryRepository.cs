using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Data
{
    public static class CategoryRepository
    {
        private static List<Category> _categories = null;


        static CategoryRepository()
        {
            _categories = new List<Category>()
            {

                new Category(){CategoryId = 1, Name = "Cell Phones", Description = "This is category of cell Phones."},
                new Category(){CategoryId = 2, Name = "Television", Description = "This is category of television."},
                new Category(){CategoryId = 3, Name = "Computers", Description = "This is category of Computers.",},
                new Category(){CategoryId = 4, Name = "Tablets", Description = "This is category of Tablets.",},
                new Category(){CategoryId = 5, Name = "Components", Description = "This is category of Components."},

            };
        }

        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public static void AddCategory(Category category)
        {
            _categories.Add(category);
        }

        public static Category GetCategoryById(int categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.CategoryId == categoryId);
            return category;
        }
    }
}
