using System;
using Common.Commands;
using Common.Communication;

namespace Smartphone.Driver
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
			Xamarin.Forms.Device.BeginInvokeOnMainThread (new Action (() => orders.UpdateAll (command.UnfinishedOrders)));
		}

	}
}

