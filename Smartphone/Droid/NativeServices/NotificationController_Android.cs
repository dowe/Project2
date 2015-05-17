using System;
using Smartphone.Driver.NativeServices;
using Smartphone.Driver.Droid.NativeServices;
using Android.App;
using Android.Content;

[assembly: Xamarin.Forms.Dependency (typeof (NotificationController_Android))]
namespace Smartphone.Driver.Droid.NativeServices
{
	public class NotificationController_Android : INotificationController
	{

		private const int NotificationId = 1000;

		private NotificationManager notificationManager = null;

		public NotificationController_Android ()
		{
			notificationManager = Xamarin.Forms.Forms.Context.GetSystemService (Context.NotificationService) as NotificationManager;
		}

		public void PutNotification (string title, string message)
		{
			Notification.Builder builder = new Notification.Builder (Xamarin.Forms.Forms.Context)
				.SetContentTitle (title)
				.SetContentText (message)
				.SetSmallIcon (Resource.Drawable.notification);
			Notification notification = builder.Build ();

			notificationManager.Notify (NotificationId, notification);
		}
			
	}
}