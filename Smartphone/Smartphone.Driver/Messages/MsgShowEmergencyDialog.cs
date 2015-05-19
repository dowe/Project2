using System;

namespace Smartphone.Driver.Messages
{
	public class MsgShowEmergencyDialog
	{
		
		public Action EmergencyConfirmed = null;
		public Action EmergencyCanceled = null;

		public MsgShowEmergencyDialog(Action emergencyConfirmed, Action emergencyCanceled)
		{
			EmergencyConfirmed = emergencyConfirmed;
			EmergencyCanceled = emergencyCanceled;
		}

	}
}

