using System;
using Common.DataTransferObjects;

namespace Smartphone.Driver
{
	public class MsgSetOrderDetailsModel
	{

		public Order Order
		{
			get;
			private set;
		}

		public MsgSetOrderDetailsModel (Order order)
		{
			Order = order;
		}

	}
}

