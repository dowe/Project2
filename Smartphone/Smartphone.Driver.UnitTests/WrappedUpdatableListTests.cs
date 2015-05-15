using System;
using Common.DataTransferObjects;
using NUnit.Framework;
using System.Collections.Generic;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.UnitTests
{
	[TestFixture ()]
	public class WrappedUpdatableListTests
	{

		private WrappedUpdatableList<KeyValuePair<int, string>> CreateTestee()
		{
			return new WrappedUpdatableList<KeyValuePair<int, string>> ((i1, i2) => i1.Key == i2.Key);
		}

		private WrappedUpdatableList<KeyValuePair<int, string>> CreateTestee(List<KeyValuePair<int, string>> list)
		{
			return new WrappedUpdatableList<KeyValuePair<int, string>> ((i1, i2) => i1.Key == i2.Key, list);
		}

		private KeyValuePair<int, string> CreateItem(int key, string value)
		{
			return new KeyValuePair<int, string> (key, value);
		}

		[Test ()]
		public void UpdateAll_OnCall_ReplacesCompleteContent ()
		{
			int sameKey = 0;
			var oldItem = CreateItem (sameKey, "old");
			var testee = CreateTestee (new List<KeyValuePair<int, string>> () { oldItem });
			var newItem = CreateItem (sameKey, "new");
			var otherNewItem = CreateItem (1, "other");
			var newOrders = new List<KeyValuePair<int, string>> () {newItem, otherNewItem};

			testee.UpdateAll (newOrders);

			CollectionAssert.AreEqual (newOrders, testee.Collection);
		}

		[Test ()]
		public void UpdateSingle_OnCall_ReplacesSingleItem ()
		{
			int sameKey = 0;
			var oldItem = CreateItem (sameKey, "old");
			var otherOldItem = CreateItem (1, "other");
			var testee = CreateTestee (new List<KeyValuePair<int, string>>() { oldItem, otherOldItem });
			var newItem = CreateItem (sameKey, "new");

			testee.UpdateSingle (newItem);

			var expectedList = new List<KeyValuePair<int, string>> () { newItem, otherOldItem };
			CollectionAssert.AreEqual (expectedList, testee.Collection);
		}

		[Test ()]
		public void RemoveSingle_OnCall_RemoveSingleItem ()
		{
			int itemId = 0;
			var item = CreateItem (itemId, "item");
			var otherItem = CreateItem (1, "other");
			var testee = CreateTestee (new List<KeyValuePair<int, string>> () { item, otherItem});

			testee.RemoveSingle ((i) => i.Key == itemId);

			var expectedList = new List<KeyValuePair<int, string>> () { otherItem };
			CollectionAssert.AreEqual (expectedList, testee.Collection);
		}

	}
}

