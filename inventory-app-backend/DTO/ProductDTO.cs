namespace inventory_app_backend.DTO
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
