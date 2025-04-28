using System;
using System.Collections.Generic;

namespace inventory_app_backend.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdStatus { get; set; }

    public int IdUserRole { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
