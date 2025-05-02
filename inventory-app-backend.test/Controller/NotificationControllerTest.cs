using FakeItEasy;
using inventory_app_backend.Controllers;
using inventory_app_backend.DTO;
using inventory_app_backend.Models;
using inventory_app_backend.Services;
using inventory_app_backend.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory_app_backend.test.Controller
{
    public class NotificationControllerTest
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;
        private NotificationController _controller;

        public NotificationControllerTest()
        {
            _notificationService = A.Fake<INotificationService>();
            _logger = A.Fake<ILogger<NotificationController>>();
            _controller = new NotificationController(_notificationService, _logger);
        }

        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var fakeNotifications = new List<Notification>
            {
                new Notification { IdNotification = 1, Title = "Notification 1", Description = "Description 1", IdAddresse = userId, IdStatus = 1 },
                new Notification { IdNotification = 2, Title = "Notification 2", Description = "Description 2", IdAddresse = userId, IdStatus = 1 }
            };

            A.CallTo(() => _notificationService.GetNotificationByUser(userId))
                .Returns(Task.FromResult(fakeNotifications));

            // Act
            var result = await _controller.Get(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedNotifications = Assert.IsType<List<Notification>>(okResult.Value);
            Assert.Equal(fakeNotifications.Count, returnedNotifications.Count);
            Assert.Equal(fakeNotifications, returnedNotifications);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var notificationDTO = new NotificationDTO
            {
                Title = "New Notification",
                Description = "This is a test notification",
                IdAddresse = 1,
                IdStatus = 1
            };

            var createdNotification = new Notification
            {
                IdNotification = 1,
                Title = notificationDTO.Title,
                Description = notificationDTO.Description,
                IdAddresse = notificationDTO.IdAddresse,
                IdStatus = notificationDTO.IdStatus
            };

            A.CallTo(() => _notificationService.AddNotification(notificationDTO))
                .Returns(Task.FromResult(createdNotification));

            // Act
            var result = await _controller.Post(notificationDTO);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.Get), createdAtResult.ActionName);
            var value = createdAtResult.Value;
            var messageProperty = value.GetType().GetProperty("message");
            var messageValue = (string)messageProperty.GetValue(value);
            Assert.Equal("Notificación creada satisfactoriamente", messageValue);
        }

        [Fact]
        public async Task Put_ReturnsOKResult()
        {
            // Arrange
            var notificationId = 1;
            var updatedNotificationDTO = new NotificationDTO
            {
                Title = "Updated Notification",
                Description = "This is an updated test notification",
                IdAddresse = 1,
                IdStatus = 2
            };

            A.CallTo(() => _notificationService.MarkNotificationAsRead(notificationId))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _controller.Put(notificationId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SendLowStockNotification_ReturnsOKResult()
        {
            // Arrange
            var notificationsSent = 5;
            A.CallTo(() => _notificationService.AddLowStockNotification())
                .Returns(Task.FromResult(notificationsSent));

            // Act
            var result = await _controller.SendLowStockNotification();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            var messageProperty = value.GetType().GetProperty("message");
            var messageValue = (string)messageProperty.GetValue(value);
            Assert.Equal("Notificaciones enviadas satisfactoriamente", messageValue);
        }

    }
}
