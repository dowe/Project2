using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GalaSoft.MvvmLight.Messaging;
using Common.DataTransferObjects;

namespace Smartphone.Driver
{
	public partial class SelectCarPage : ContentPage
	{
		public SelectCarPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.SelectCar;
			Messenger.Default.Register<MsgUpdateCarIDsPicker> (this, UpdateCarIDsPicker);
		}

		private void UpdateCarIDsPicker(MsgUpdateCarIDsPicker msgUpdateCarIDPicker)
		{
			carPicker.Items.Clear ();
			while (carPicker.Items.Count != 0)
			{
				carPicker.Items.RemoveAt (0);
			}
			foreach (Car car in msgUpdateCarIDPicker.Cars)
			{
				carPicker.Items.Add (car.CarID);
			}
		}

		protected override bool OnBackButtonPressed ()
		{
			Messenger.Default.Send<MsgSwitchLoginPage> (new MsgSwitchLoginPage ());

			return true;
		}

	}
}

