using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Smartphone.Driver
{
	public class WrappedCars : WrappedUpdatableList<Car>
	{

		public WrappedCars () : base(AreEquals)
		{
		}

		private static bool AreEquals(Car c1, Car c2)
		{
			return c1.CarID.Equals (c2.CarID);
		}
			
	}
}

