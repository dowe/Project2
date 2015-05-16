using System;

namespace Smartphone.Driver.NativeServices
{
	public interface INotificationController
	{
		void PutNotification (string title, string message);
	}
}