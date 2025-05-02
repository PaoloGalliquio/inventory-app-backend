using inventory_app_backend.Constants;
using inventory_app_backend.DTO;
using inventory_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_app_backend.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationByUser(int id);
        Task<Notification> AddNotification(NotificationDTO Notification);
        Task<int> AddLowStockNotification();
        Task<int> MarkNotificationAsRead(int id);
    }

    public class NotificationService : INotificationService
    {
        private readonly InventoryContext _context;
        private readonly DbSet<Notification> _dbSet;

        public NotificationService(InventoryContext context)
        {
            _context = context;
            _dbSet = context.Set<Notification>();
        }

        public async Task<List<Notification>> GetNotificationByUser(int id)
        {
            return await _dbSet.Where(x => x.IdAddresse == id && x.IdStatus == (int)Status.Pending).ToListAsync();
        }

        public async Task<Notification> AddNotification(NotificationDTO notification)
        {
            var newNotification = new Notification
            {
                Title = notification.Title,
                Description = notification.Description,
                IdAddresse = notification.IdAddresse,
                IdStatus = (int)Status.Pending
            };
            _context.Set<Notification>().Add(newNotification);
            await _context.SaveChangesAsync();
            return newNotification;
        }

        public async Task<int> MarkNotificationAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return 0;
            }
            notification.IdStatus = (int)Status.Completed;
            _context.Notifications.Update(notification);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddLowStockNotification()
        {
            var productsLowInStock = await _context.Products
                    .Where(o => o.IdStatus == (int)Status.Active && o.Quantity < 5)
                    .ToListAsync();
            var administrators = await _context.Users
                    .Where(o => o.IdStatus == (int)Status.Active && o.IdUserRole == (int)Roles.Admin)
                    .ToListAsync();
            var notifications = new List<Notification>();
            foreach (var product in productsLowInStock)
            {
                foreach (var admin in administrators)
                {
                    var newNotification = new Notification
                    {
                        Title = "Alerta de producto con existencias bajas",
                        Description = $"El producto {product.Name} se encuentra con existencias bajas ({product.Quantity} en almacén).",
                        IdAddresse = admin.IdUser,
                        IdStatus = (int)Status.Pending
                    };
                    notifications.Add(newNotification);
                }
            }
            _context.Set<Notification>().AddRange(notifications);
            return await _context.SaveChangesAsync();
        }
    }
}
