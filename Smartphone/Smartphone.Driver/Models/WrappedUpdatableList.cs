using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Smartphone.Driver.Models
{
	public class WrappedUpdatableList<T>
	{

		private Func<T, T, bool> itemsEqual = null;

		public ObservableCollection<T> Collection
		{
			get;
			private set;
		}

		public WrappedUpdatableList(Func<T, T, bool> itemsEqual) : this(itemsEqual, new List<T>())
		{
		}

		public WrappedUpdatableList (Func<T, T, bool> itemsEqual, IList<T> list)
		{
			this.itemsEqual = itemsEqual;
			Collection = new ObservableCollection<T>(list);
		}

		public void UpdateAll(IReadOnlyCollection<T> updatedItems)
		{
			Collection.Clear ();
			foreach (T item in updatedItems)
			{
				Collection.Add (item);
			}
		}

		public void UpdateSingle(T item)
		{
			for (int i = 0; i < Collection.Count; i++)
			{
				if (itemsEqual(Collection [i], item))
				{
					Collection.RemoveAt (i);
					Collection.Insert (i, item);
					break;
				}
			}
		}

		public void RemoveSingle(Func<T, bool> shallRemove)
		{
			for (int i = 0; i < Collection.Count; i++)
			{
				if (shallRemove(Collection [i]))
				{
					Collection.RemoveAt (i);
					break;
				}
			}
		}

		public void RemoveSingle(T item)
		{
			RemoveSingle ((i) => itemsEqual (i, item));
		}

	}
}

