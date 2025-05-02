using inventory_app_backend.DTO;
using inventory_app_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _logger.LogInformation("Fetching notifications for user {UserId}", id);
                var notifications = await _notificationService.GetNotificationByUser(id);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving notifications");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationDTO notification)
        {
            try
            {
                _logger.LogInformation("Adding a new notification");
                if (notification == null)
                {
                    return BadRequest("Notification cannot be null");
                }
                var result = await _notificationService.AddNotification(notification);
                if (result != null)
                {
                    _logger.LogInformation("Notification added successfully");
                    return CreatedAtAction(nameof(Get), null, new { notification, message = "Notificación creada satisfactoriamente" });
                }
                _logger.LogWarning("Failed to add notification");
                return BadRequest("Failed to add notification");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a notification");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id)
        {
            try
            {
                _logger.LogInformation("Marking notification {NotificationId} as read", id);
                var result = await _notificationService.MarkNotificationAsRead(id);
                if (result > 0)
                {
                    _logger.LogInformation("Notification marked as read successfully");
                    return Ok();
                }
                _logger.LogWarning("Failed to mark notification as read");
                return BadRequest(new { message = "Error al marcar la notificación como leída" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while marking notification as read");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("SendLowStockNotification")]
        public async Task<IActionResult> SendLowStockNotification()
        {
            try
            {
                _logger.LogInformation("Adding LowStockNotification");
                var result = await _notificationService.AddLowStockNotification();
                if (result > 0)
                {
                    _logger.LogInformation("Notifications added successfully");
                    return Ok(new { message = "Notificaciones enviadas satisfactoriamente" });
                }
                _logger.LogInformation("No notifications to send");
                return Ok(new { message = "No hay notificaciones para enviar" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a notification");
                return BadRequest(new { message = "Error al enviar notificaciones" });
            }
        }
    }
}
