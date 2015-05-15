using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Smartphone.Driver.Models
{
	public class WrappedOrders : WrappedUpdatableList<Order>
	{

		public WrappedOrders () : base (AreEquals, new List<Order>())
		{
		}

		private static bool AreEquals(Order o1, Order o2)
		{
			return o1.OrderID.Equals (o2.OrderID);
		}

	}
}

