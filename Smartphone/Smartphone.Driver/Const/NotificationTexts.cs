using System;

namespace Smartphone.Driver.Const
{
	public static class NotificationTexts
	{

		public const string ORDER_REMINDER_TEXT = "Only one hour left to collect the samples. Hurry up!";
		public static string OrderReminderTitle(long orderID)
		{
			return "Order " + orderID;
		}

	}
}

