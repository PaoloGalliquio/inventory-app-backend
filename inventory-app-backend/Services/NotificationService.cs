using inventory_app_backend.Constants;
using inventory_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_app_backend.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationByUser(int id);
        Task<int> AddNotification(Notification Notification);
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
            return await _dbSet.Where(x => x.IdAddresse == id).ToListAsync();
        }

        public async Task<int> AddNotification(Notification Notification)
        {
            await _context.Notifications.AddAsync(Notification);
            int afectedRows = await _context.SaveChangesAsync();
            return afectedRows;
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
    }
}
