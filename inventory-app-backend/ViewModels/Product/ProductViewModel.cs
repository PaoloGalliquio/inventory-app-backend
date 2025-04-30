namespace inventory_app_backend.ViewModels.Product
{
    public class ProductViewModel
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public int IdStatus { get; set; }
    }
}
