using System;

namespace Smartphone.Driver.Const
{
	public static class NotificationTexts
	{

		public const string ORDER_REMINDER_TEXT = "Nur noch eine Stunde üprig, um die Bestellung einzusammeln. Beeilen Sie sich!";
		public static string OrderReminderTitle(long orderID)
		{
			return "Bestellung " + orderID;
		}

	}
}

