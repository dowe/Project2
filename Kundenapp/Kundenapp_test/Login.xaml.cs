using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

using Xamarin.Forms;

namespace Kundenapp
{
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.LoginVM;
			Messenger.Default.Register<String> (this, "Login", (str) => {
				Messenger.Default.Send<String>(str, "Username");
				Navigation.PushAsync(new PatientView());
			});
			Messenger.Default.Register<NotificationMessage> (this, (msg) => {
				if(msg.Notification == "LoginFail")
				{
					DisplayAlert("Login Fehlgeschlagen", "Falscher Nutzername oder Passwort","OK");
				}
			});
		}
	}
}

