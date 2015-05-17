using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

using Xamarin.Forms;
using Common.Communication.Client;

namespace Kundenapp
{
	public partial class PatientView : ContentPage
	{
		public PatientView ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.PatientVM;
			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
			Messenger.Default.Register<PatientM> (this, "editPat", edit);
			Messenger.Default.Register<long> (this, ordercomp);
		}

		private void NotificationMessageReceived(NotificationMessage msg)
		{
			if (msg.Notification == "Add")
			{
				Navigation.PushModalAsync (new AddDialog ());
			}

			if (msg.Notification == "AddPop")
			{
				Navigation.PopModalAsync ();
			}

			if (msg.Notification == "exists") 
			{
				DisplayAlert ("Id exisitert schon", "Es existiert bereits ein Patient mit dieser PatientenID, bitte geben sie eine andere ID an", "OK");
			}
		}

		private void edit(PatientM Patient)
		{
			Messenger.Default.Send<PatientM> (Patient, "edit");
			Navigation.PushAsync (new AnalysisSelectionView ());
		}

		private void ordercomp(long id)
		{
			DisplayAlert ("Bestellung erfolgreich", "Ihre Bestellung wurde erfolgreich erstellt. Ihre BestellungsID lautet: " + id, "OK");
		}
	}
}