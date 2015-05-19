using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Common.DataTransferObjects;

namespace Smartphone.Driver.Messages
{
	public class MsgUpdateCarIDsPicker
	{

		public IEnumerable<Car> Cars
		{
			get;
			private set;
		}

		public MsgUpdateCarIDsPicker (IEnumerable<Car> cars)
		{
			Cars = cars;
		}

	}
}

