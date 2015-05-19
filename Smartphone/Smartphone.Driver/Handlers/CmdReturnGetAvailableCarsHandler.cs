using System;
using Common.Commands;
using Common.Communication;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.Handlers
{
	public class CmdReturnGetAvailableCarsHandler : CommandHandler<CmdReturnGetAvailableCars>
	{

		private WrappedCars cars = null;

		public CmdReturnGetAvailableCarsHandler (WrappedCars cars)
		{
			this.cars = cars;
		}
			
		protected override void Handle (CmdReturnGetAvailableCars command, string connectionIdOrNull)
		{
			// Update Available cars model.
			Xamarin.Forms.Device.BeginInvokeOnMainThread (() => cars.UpdateAll (command.AvailableCars));
		}

	}
}

