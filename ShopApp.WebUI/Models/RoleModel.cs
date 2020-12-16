using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
    }
}