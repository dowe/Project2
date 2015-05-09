using System;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Smartphone.Driver
{
	public class SelectCarViewModel : ViewModelBase
	{

		private const string AllCarIDsProperty = "AvailableCars";
		private const string SelectedCarIndexProperty = "SelectedCarIndex";
		private const string StartKmProperty = "StartKm";
		private const string IsCommunicatingProperty = "IsCommunicating";
		private const string IsNotCommunicatingProperty = "IsNotCommunicating";

		private IClientConnection connection = null;

		private WrappedCars availableCars = null;
		private int selectedCarIndex = -1;
		private float startKm = 0;
		private bool isCommunicating = false;
		private RelayCommand selectCarCommand = null;

		public SelectCarViewModel (IClientConnection connection, WrappedCars availableCars)
		{
			this.connection = connection;
			AvailableCars = availableCars;
		}

		public WrappedCars AvailableCars
		{
			get
			{
				return availableCars;
			}
			set
			{
				if (value != availableCars)
				{
					availableCars = value;
					availableCars.Collection.CollectionChanged += AvailableCars_Collection_CollectionChanged;
					RaisePropertyChanged (AllCarIDsProperty);
				}
			}
		}

		public int SelectedCarIndex
		{
			get {
				return selectedCarIndex;
			}
			set {
				if (value != selectedCarIndex)
				{
					selectedCarIndex = value;
					RaisePropertyChanged (SelectedCarIndexProperty);
				}
			}
		}

		public float StartKm
		{
			get {
				return startKm;
			}
			set {
				if (Math.Abs (value - startKm) > float.Epsilon)
				{
					startKm = value;
					RaisePropertyChanged (StartKmProperty);
				}
			}
		}

		public bool IsCommunicating
		{
			get {
				return isCommunicating;
			}
			set {
				if (value != isCommunicating)
				{
					isCommunicating = value;
					RaisePropertyChanged (IsCommunicatingProperty);
					RaisePropertyChanged (IsNotCommunicatingProperty);
				}
			}
		}

		public bool IsNotCommunicating
		{
			get {
				return !IsCommunicating;
			}
		}

		public RelayCommand SelectCarCommand
		{
			get {
				if (selectCarCommand == null)
				{
					selectCarCommand = new RelayCommand (SelectCar);
				}
				return selectCarCommand;
			}
		}

		private void AvailableCars_Collection_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			// The picker does not support data binding so it's items have to be updated via code behind.
			MessengerInstance.Send (new MsgUpdateCarIDsPicker (AvailableCars.Collection));
		}

		private void SelectCar()
		{
			IsCommunicating = true;
			try
			{
				if (selectedCarIndex >= 0 && availableCars.Collection.Count > 0 && startKm > 0)
				{
					Car car = availableCars.Collection [selectedCarIndex];
					CmdSelectCar selectCar = new CmdSelectCar ("Ole", car.CarID, startKm); // TODO get username from session.
					CmdReturnSelectCar response = connection.SendWait<CmdReturnSelectCar>(selectCar);
					if (response.Success)
					{
						OnSuccessfulCarSelection ();
					}
					else
					{
						// Update the cars. Maybe the user selected a car that was already assigned to another user.
						CmdGetAvailableCars getAvailableCars = new CmdGetAvailableCars ();
						connection.Send (getAvailableCars);
					}
				}
			}
			finally
			{
				IsCommunicating = false;
			}
		}

		private void OnSuccessfulCarSelection()
		{
			// TODO update Session
			// Request all unfinished Orders of this driver.
			CmdGetDriversUnfinishedOrders getUnfinishedOrders = new CmdGetDriversUnfinishedOrders ("Ole"); // TODO get username from session.
			connection.Send (getUnfinishedOrders);

			// Switch page.
			Messenger.Default.Send<MsgSwitchOrdersPage> (new MsgSwitchOrdersPage ());
		}

	}
}

