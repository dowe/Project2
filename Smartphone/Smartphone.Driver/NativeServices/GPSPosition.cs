using System;

namespace Smartphone.Driver.NativeServices
{
	public class GPSPosition
	{

		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public GPSPosition (double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

	}
}