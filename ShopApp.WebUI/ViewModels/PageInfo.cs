using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.ViewModels
{
    public class PageInfo
    {
        public int TotalItems { get; set; } //Toplam ürün sayısı
        public int ItemsPerPage { get; set; } // Sayfa başına ürün sayısı
        public int CurrentPage { get; set; } // O an aktif olan sayfa
        public string CurrentCategory { get; set; } // O an aktif olan kategori

        // Ürünlerin toplam kaç sayfada gösterilmesini hesaplar.
        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        }
    }
}
