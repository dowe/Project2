using System;
using Common.DataTransferObjects;

namespace Smartphone.Driver.Messages
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

