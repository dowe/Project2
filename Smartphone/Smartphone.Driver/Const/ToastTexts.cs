using System;

namespace Smartphone.Driver.Const
{
	public static class ToastTexts
	{
	    public const string FAILED_LOGIN = "Falscher Username oder falsches Passwort.";
		public const string FAILED_EMERGENCY = "Notfall konnte nicht gemeldet werden.";
	    public const string FAILED_LOGOUT_ORDERS_LEFT = "Sie haben noch Bestellung abzuholen.";

	    public const string FAILED_LAUNCH_MAP =
	        "Google Maps konnte nicht gestartet werden. Haben Sie die App installiert?";

	    public const string FAILED_SET_COLLECTED = "Bestellung konnte nicht als abgeholt gemeldet werden.";
	    public const string FAILED_SELECT_CAR = "Auto konnte Ihnen nicht zugewiesen werden.";
	    public const string SERVER_NO_ANSWER = "Keine Antwort vom Server.";
		public static string AlreadyAssignedToCar(string carID)
		{
		    return "Sie sind bereits Auto " + carID + "zugewiesen.";
		}
	}
}

