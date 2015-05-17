﻿using System;
using Smartphone.Driver.NativeServices;
using Smartphone.Driver.Droid.NativeServices;
using Android.App;
using Android.Content;

[assembly: Xamarin.Forms.Dependency (typeof (NotificationController_Android))]
namespace Smartphone.Driver.Droid.NativeServices
{
	public class NotificationController_Android : INotificationController
	{

		private IncIntGenerator notificationIDGenerator = null;
		private NotificationManager notificationManager = null;

		public NotificationController_Android ()
		{
			notificationIDGenerator = new IncIntGenerator (1000);
			notificationManager = Xamarin.Forms.Forms.Context.GetSystemService (Context.NotificationService) as NotificationManager;
		}

		public void PutNotification (string title, string message)
		{
			Notification.Builder builder = new Notification.Builder (Xamarin.Forms.Forms.Context)
				.SetContentTitle (title)
				.SetContentText (message)
				.SetSmallIcon (Resource.Drawable.notification)
				.SetContentIntent (CreateAppLaunchPendingIntent())
				.SetAutoCancel(true);
			Notification notification = builder.Build ();

			notificationManager.Notify (notificationIDGenerator.IncrementAndGet(), notification);
		}

		private PendingIntent CreateAppLaunchPendingIntent()
		{
			Intent intent = new Intent (Xamarin.Forms.Forms.Context, typeof(MainActivity));
			const int pendingIntentId = 0;
			PendingIntent pendingIntent = 
				PendingIntent.GetActivity (Xamarin.Forms.Forms.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

			return pendingIntent;
		}
			
	}
}