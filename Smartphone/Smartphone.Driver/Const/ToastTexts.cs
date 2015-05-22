using System;

namespace Smartphone.Driver.Const
{
	public static class ToastTexts
	{
		public const string FAILED_LOGIN = "Login failed.";
		public const string FAILED_EMERGENCY = "Emergency failed.";
		public const string FAILED_LOGOUT_ORDERS_LEFT = "You still have orders left.";
		public const string FAILED_LAUNCH_MAP = "Could not start the map. Make sure you have Google Maps installed.";
		public const string FAILED_SET_COLLECTED = "Set collected failed.";
		public const string FAILED_SELECT_CAR = "Select car failed.";
		public const string SERVER_NO_ANSWER = "Server did not answer.";
		public static string AlreadyAssignedToCar(string carID)
		{
			return "Already assigned to " + carID;
		}
	}
}

