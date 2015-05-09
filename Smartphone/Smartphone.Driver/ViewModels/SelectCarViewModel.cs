using System;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight.Command;

namespace Smartphone.Driver
{
	public class SelectCarViewModel : ViewModelBase
	{

		private const string AllCarIDsProperty = "AvailableCars";
		private const string SelectedCarIndexProperty = "SelectedCarIndex";

		private IClientConnection connection = null;

		private WrappedCars availableCars = null;
		private int selectedCarIndex = -1;

		public SelectCarViewModel (IClientConnection connection, WrappedCars availableCars)
		{
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

		private void AvailableCars_Collection_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			// The picker does not support data binding so it's items have to be updated via code behind.
			MessengerInstance.Send (new MsgUpdateCarIDsPicker (AvailableCars.Collection));
		}

		private void OnSuccessfulCarSelection()
		{
			// TODO update Session
			// Request all unfinished Orders of this driver.
			CmdGetDriversUnfinishedOrders getUnfinishedOrders = new CmdGetDriversUnfinishedOrders ("Ole"); // TODO get username from session.
			connection.Send (getUnfinishedOrders);
		}

	}
}

