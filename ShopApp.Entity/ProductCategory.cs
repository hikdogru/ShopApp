namespace ShopApp.Entity
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<ProductCategory> ProductCategory { get; set; }

    }
}