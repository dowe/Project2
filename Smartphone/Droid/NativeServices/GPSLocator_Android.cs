﻿using System;
using Smartphone.Driver.NativeServices;
using Android.Locations;
using Android.Content;
using Java.Security;
using Smartphone.Driver.Droid.NativeServices;

[assembly: Xamarin.Forms.Dependency(typeof(GPSLocator_Android))]
namespace Smartphone.Driver.Droid.NativeServices
{
    public class GPSLocator_Android : Java.Lang.Object, IGPSLocator, ILocationListener
    {

        // See http://developer.xamarin.com/guides/android/platform_features/maps_and_location/location/

        private LocationManager locationManager = null;
        private TimeSpan updateInterval = TimeSpan.FromSeconds(0);
        private bool subscribedToUpdates = false;

        public event Action<NativeGPSPosition> LocationUpdated;

        public GPSLocator_Android()
        {
            locationManager = Xamarin.Forms.Forms.Context.GetSystemService(Context.LocationService) as LocationManager;
            subscribedToUpdates = false;
        }

        public void StartLocationUpdates(TimeSpan updateInterval)
        {
            subscribedToUpdates = true;
            this.updateInterval = updateInterval;
            RequestUpdates();
        }

        private void RequestUpdates()
        {
            string provider = LocationManager.GpsProvider;
            locationManager.RequestLocationUpdates(provider, (long)updateInterval.TotalMilliseconds, 0, this);
        }

        public void StopLocationUpdates()
        {
            subscribedToUpdates = false;
            RemoveUpdates();
        }

        private void RemoveUpdates()
        {
            subscribedToUpdates = false;
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            if (LocationUpdated != null)
            {
                NativeGPSPosition position = new NativeGPSPosition(location.Latitude, location.Longitude);
                LocationUpdated(position);
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }
        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Android.OS.Bundle extras)
        {
        }

    }
}

