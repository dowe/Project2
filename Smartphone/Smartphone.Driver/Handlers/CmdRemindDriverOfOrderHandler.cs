using System;
using Common.Commands;
using Common.Communication;
using Smartphone.Driver.NativeServices;
using Xamarin.Forms;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.Handlers
{
	public class CmdRemindDriverOfOrderHandler : CommandHandler<CmdRemindDriverOfOrder>
	{

		private INotificationController notificationController = null;

		public CmdRemindDriverOfOrderHandler (INotificationController notificationController)
		{
			this.notificationController = notificationController;
		}
			
		protected override void Handle (CmdRemindDriverOfOrder command, string connectionIdOrNull)
		{
			string title = "Order " + command.OrderId;
			string message = "Only one hour left to collect the samples. Hurry up!";

			notificationController.PutNotification (title, message);
		}

	}
}

