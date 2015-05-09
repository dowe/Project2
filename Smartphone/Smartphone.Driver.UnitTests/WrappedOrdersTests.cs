using System;
using Common.DataTransferObjects;
using NUnit.Framework;
using System.Collections.Generic;

namespace Smartphone.Driver.UnitTests
{
	[TestFixture ()]
	public class WrappedOrdersTests
	{

		private WrappedOrders CreateTestee()
		{
			return new WrappedOrders ();
		}

		private WrappedOrders CreateTestee(List<Order> orders)
		{
			return new WrappedOrders (orders);
		}

		[Test ()]
		public void UpdateAll_OnCall_ReplacesCompleteContent ()
		{
			long sameOrderId = 0;
			Order oldOrder = new Order { OrderID = sameOrderId };
			WrappedOrders testee = CreateTestee (new List<Order> () { oldOrder });
			Order newOrder = new Order { OrderID = sameOrderId };
			Order otherNewOrder = new Order { OrderID = 1 };
			List<Order> newOrders = new List<Order>() {newOrder, otherNewOrder};

			testee.UpdateAll (newOrders);

			CollectionAssert.AreEqual (newOrders, testee.Orders);
		}

		[Test ()]
		public void UpdateSingle_OnCall_ReplacesSingleItem ()
		{
			long sameOrderId = 0;
			Order oldOrder = new Order { OrderID = sameOrderId };
			Order otherOldOrder = new Order { OrderID = 1 };
			WrappedOrders testee = CreateTestee (new List<Order> () { oldOrder, otherOldOrder });
			Order newOrder = new Order { OrderID = sameOrderId };

			testee.UpdateSingle (newOrder);

			List<Order> expectedList = new List<Order> () { newOrder, otherOldOrder };
			CollectionAssert.AreEqual (expectedList, testee.Orders);
		}

		[Test ()]
		public void RemoveSingle_OnCall_RemoveSingleItem ()
		{
			long orderId = 0;
			Order order = new Order { OrderID = orderId };
			Order otherOrder = new Order { OrderID = 1 };
			WrappedOrders testee = CreateTestee (new List<Order> () { order, otherOrder });

			testee.RemoveSingle (orderId);

			List<Order> expectedList = new List<Order> () { otherOrder };
			CollectionAssert.AreEqual (expectedList, testee.Orders);
		}
	}
}

