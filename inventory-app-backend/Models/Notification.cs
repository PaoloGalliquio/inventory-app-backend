using System;
using System.Collections.Generic;

namespace inventory_app_backend.Models;

public partial class Notification
{
    public int IdNotification { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int IdAddresse { get; set; }

    public int IdStatus { get; set; }

    public virtual User IdAddresseNavigation { get; set; } = null!;
}
