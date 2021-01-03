using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ShopApp.Entity;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //[Display(Name = "Product Name")]
        //[Required(ErrorMessage = "Product name is required field!")]
        //[StringLength(50, MinimumLength = 5, ErrorMessage = "Product name must to between 5 and 50 caracters!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Url is required field!")]
        public string Url { get; set; }
        //[Range(1,10000,ErrorMessage="You must enter between 1 and 10000!")]
        [Required(ErrorMessage = "Price is required field!")]
        public double? Price { get; set; }
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must to between 5 and 500 caracters!")]
        [Required(ErrorMessage = "Description is required field!")]
        public string Description { get; set; }
        [Display(Name = "Image Url")]
        //[Required(ErrorMessage = "Image Url is required field!")]
        [DataType(DataType.Upload)]
        public string ImageUrl { get; set; }

        public bool IsHome { get; set; }

        public bool IsApproved { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}
