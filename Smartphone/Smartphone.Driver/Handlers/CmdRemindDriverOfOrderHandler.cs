using System;
using Common.Commands;
using Common.Communication;
using Smartphone.Driver.NativeServices;
using Xamarin.Forms;
using Smartphone.Driver.Models;
using Smartphone.Driver.Const;

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
			string title = NotificationTexts.OrderReminderTitle (command.OrderId);
			string message = NotificationTexts.ORDER_REMINDER_TEXT;

			notificationController.PutNotification (title, message, command.OrderId);
		}

	}
}

