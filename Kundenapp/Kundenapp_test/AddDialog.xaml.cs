using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

using Xamarin.Forms;

namespace Kundenapp
{
	public partial class AddDialog : ContentPage
	{
		public AddDialog ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.AddDialogVM;
		}
	}
}

