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

		private NotificationManager notificationManager = null;

		public NotificationController_Android ()
		{
			notificationManager = Xamarin.Forms.Forms.Context.GetSystemService (Context.NotificationService) as NotificationManager;
		}

		public void PutNotification (string title, string message)
		{
			Notification.Builder builder = new Notification.Builder (Xamarin.Forms.Forms.Context)
				.SetContentTitle (title)
				.SetContentText (message);
			Notification notification = builder.Build ();

			notificationManager.Notify (0, notification);
		}
			
	}
}