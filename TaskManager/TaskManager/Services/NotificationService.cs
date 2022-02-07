using System;
using System.Collections.Generic;
using TaskManager.Models;
using Xamarin.Forms;

namespace TaskManager.Services
{
    class NotificationService
    {
        // A notification service instance.
        private static NotificationService _notificationServiceInstance;


        private INotificationManager _notificationManager;


        private static Dictionary<int, Tasks> _createdNotifications;

        /// <summary>
        /// A notification service instance.
        /// </summary>
        public static NotificationService NotificationServiceInstance
        {
            get
            {
                if (_notificationServiceInstance == null)
                {
                    _notificationServiceInstance = new NotificationService();
                }
                return _notificationServiceInstance;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NotificationService()
        {
            _notificationManager = DependencyService.Get<INotificationManager>();
            _createdNotifications = new Dictionary<int, Tasks>();

            /*notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var data = (NotificationEventArgs)eventArgs;
                ShowNotification(data.Title, data.Message);
            };*/
        }

        public void SetScheduleNotification(List<Tasks> tasks)
        {
            foreach (var notification in _createdNotifications)
            {
                //_notificationManager
            }

            string title = $"Deadline";
            string message = $"Deadline for the problem \"Bunch\" today!";
            _notificationManager.SendNotification(title, message, 0, DateTime.Now.AddSeconds(10));
        }
    }
}
