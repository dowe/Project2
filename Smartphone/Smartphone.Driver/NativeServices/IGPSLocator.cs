using System;

namespace Smartphone.Driver.NativeServices
{
	public interface IGPSLocator
	{

		// See http://developer.xamarin.com/guides/cross-platform/xamarin-forms/dependency-service/

		event Action<GPSPosition> LocationUpdated;

		void StartLocationUpdates (TimeSpan timeInterval);
		void StopLocationUpdates();

	}
}