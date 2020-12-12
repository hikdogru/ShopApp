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
        [Required(ErrorMessage = "Category name is required field!")]
        [StringLength(50, MinimumLength = 3 ,ErrorMessage = "Category name must between 5 and 50 characters!")]
        public string Name { get; set; }

        [Display(Name = "Category Url")]
        [Required(ErrorMessage = "Category Url is required field!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category url must between 5 and 50 characters!")]
        public string Url { get; set; }

        public List<Product> Products { get; set; } 
       

    }
}
