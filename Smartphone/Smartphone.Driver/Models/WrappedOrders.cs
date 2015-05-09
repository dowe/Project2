using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Smartphone.Driver
{
	public class WrappedOrders
	{

		public ObservableCollection<Order> Orders
		{
			get;
			set;
		}

		public WrappedOrders () : this (new List<Order>())
		{
		}

		public WrappedOrders(List<Order> orders)
		{
			Orders = new ObservableCollection<Order> (orders);
		}
			
		public void UpdateAll(IReadOnlyCollection<Order> allOrders)
		{
			Orders.Clear ();
			foreach (Order order in allOrders)
			{
				Orders.Add (order);
			}
		}
			
		public void UpdateSingle(Order order)
		{
			for (int i = 0; i < Orders.Count; i++)
			{
				if (Orders [i].OrderID.Equals (order.OrderID))
				{
					Orders.RemoveAt (i);
					Orders.Insert (i, order);
					break;
				}
			}
		}
			
		public void RemoveSingle(long orderId)
		{
			for (int i = 0; i < Orders.Count; i++)
			{
				if (Orders [i].OrderID.Equals (orderId))
				{
					Orders.RemoveAt (i);
					break;
				}
			}
		}

	}
}

