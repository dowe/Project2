﻿using System;
using Smartphone.Driver.NativeServices;
using Xamarin.Forms;
using Common.Communication.Client;
using Common.Commands;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.GPS
{
	public class GPSPositionSender
	{

		private IClientConnection connection = null;
		private Session session = null;

		private TimeSpan updateInterval = TimeSpan.FromSeconds(5);
		private IGPSLocator locator = null;

		public GPSPositionSender (IClientConnection connection, Session session, IGPSLocator locator)
		{
			this.connection = connection;
			this.session = session;
			this.locator = locator;
			locator.LocationUpdated += OnLocationUpdate;
		}

		public void Start()
		{
			locator.StartLocationUpdates (updateInterval);
		}

		public void Stop()
		{
			locator.StopLocationUpdates ();
		}

		private void OnLocationUpdate(NativeGPSPosition position)
		{
			Common.DataTransferObjects.GPSPosition dtoPosition = new Common.DataTransferObjects.GPSPosition {Latitude = (float)position.Latitude, Longitude = (float)position.Longitude};
			CmdStoreDriverGPSPosition cmdStorePosition = new CmdStoreDriverGPSPosition (session.CarID, dtoPosition);

			connection.Send (cmdStorePosition);
		}
	}
}

