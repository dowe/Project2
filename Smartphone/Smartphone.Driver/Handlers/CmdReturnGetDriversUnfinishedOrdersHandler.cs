using System;
using Common.Commands;
using Common.Communication;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.Handlers
{
	public class CmdReturnGetDriversUnfinishedOrdersHandler : CommandHandler<CmdReturnGetDriversUnfinishedOrders>
	{

		private WrappedOrders orders = null;

		public CmdReturnGetDriversUnfinishedOrdersHandler (WrappedOrders orders)
		{
			this.orders = orders;
		}

		protected override void Handle (CmdReturnGetDriversUnfinishedOrders command, string connectionIdOrNull)
		{
			// Update Orders model.
			Xamarin.Forms.Device.BeginInvokeOnMainThread (new Action (() => orders.UpdateAll (command.UnfinishedOrders)));
		}

	}
}

