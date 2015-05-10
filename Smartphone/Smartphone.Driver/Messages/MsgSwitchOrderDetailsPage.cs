using System;
using Common.DataTransferObjects;

namespace Smartphone.Driver
{
	public class MsgSwitchOrderDetailsPage
	{

		public Order Order
		{
			get;
			private set;
		}

		public MsgSwitchOrderDetailsPage (Order order)
		{
			this.Order = order;
		}

	}
}

