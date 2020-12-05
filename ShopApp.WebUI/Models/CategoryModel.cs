using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Entity;

namespace ShopApp.WebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [Display(Name = "Category Url")]
        public string Url { get; set; }

        public List<Product> Products { get; set; } 
       

    }
}
