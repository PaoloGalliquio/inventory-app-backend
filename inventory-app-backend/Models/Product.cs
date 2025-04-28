using System;
using System.Collections.Generic;

namespace inventory_app_backend.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int IdCategory { get; set; }

    public int IdStatus { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;
}
