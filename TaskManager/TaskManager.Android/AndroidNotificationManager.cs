using System;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using TaskManager.Services;
using Android.Support.V4.App;

[assembly: Dependency(typeof(TaskManager.Droid.AndroidNotificationManager))]
namespace TaskManager.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;

        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public AndroidNotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        public void SendNotification(string title, string message, int id, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                var intent = new Intent(Android.App.Application.Context, typeof(AlarmHandler));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);

                var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, id /**/, intent, PendingIntentFlags.CancelCurrent);
                var triggerTime = GetNotifyTime(notifyTime.Value);


                var alarmManager = Android.App.Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Show(title, message, id);
            }
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void Show(string title, string message, int id)
        {
            var intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            var pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, id /**/, intent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(Android.App.Application.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

                //.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Android.Resource.Drawable))
                //.SetSmallIcon(Resource.Drawable.xamagonBlue)

            var notification = builder.Build();
            manager.Notify(id, notification);
        }

        public void DeleteAllNotifications()
        {
            var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
            notificationManager.CancelAll();
        }

        /*
        public void DeleteNotification(int id)
        {
            var intent = new Intent(Android.App.Application.Context, typeof(AlarmHandler));
            var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, id, intent, PendingIntentFlags.CancelCurrent);
            var alarmManager = Android.App.Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            alarmManager.Cancel(pendingIntent);
        }*/

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        long GetNotifyTime(DateTime notifyTime)
        {
            var time = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            var difference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            var alarmTime = time.AddSeconds(-difference).Ticks / 10000;
            return alarmTime;
        }
    }
}