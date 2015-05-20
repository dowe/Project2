using System;
using Common.Commands;
using Common.Communication;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.Handlers
{
	public class CmdReturnSetOrderCollectedHandler : CommandHandler<CmdReturnSetOrderCollected>
	{

		private WrappedOrders orders = null;

		public CmdReturnSetOrderCollectedHandler (WrappedOrders orders)
		{
			this.orders = orders;
		}
			
		protected override void Handle (CmdReturnSetOrderCollected command, string connectionIdOrNull)
		{
			orders.RemoveSingle (o => o.OrderID == command.OrderID);
		}

	}
}

