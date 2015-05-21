using System;

namespace Smartphone.Driver.NativeServices
{
	public class NativeGPSPosition
	{

		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public NativeGPSPosition (double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

	}
}