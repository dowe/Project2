﻿using System;
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
			foreach (Car car in msgUpdateCarIDPicker.Cars)
			{
				carPicker.Items.Add (car.CarID);
			}
		}

	}
}
