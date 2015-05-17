using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Kundenapp_test
{
	public partial class MyPage : ContentPage
	{
		public MyPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.Main;
		}
	}
}

