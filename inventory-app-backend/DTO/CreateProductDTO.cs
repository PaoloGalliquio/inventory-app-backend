namespace inventory_app_backend.DTO
{
    public class CreateProductDTO
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int IdCategory { get; set; }
    }
}
