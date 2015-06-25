using System;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Common.Communication.Client;

namespace Kundenapp
{
	public class ViewModelLocator
	{
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			IClientConnection clientConnection = new ClientConnection ("http://langy.duckdns.org:8008/commands");
			clientConnection.Start ();
			clientConnection.Connect ();
			SimpleIoc.Default.Register<IClientConnection> (() => clientConnection);
			SimpleIoc.Default.Register<PatientVM>();
			SimpleIoc.Default.Register<AddDialogVM>();
			SimpleIoc.Default.Register<AnalysisSelectionVM> ();
			SimpleIoc.Default.Register<LoginVM> ();
		}

		public PatientVM PatientVM
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PatientVM>();
			}
		}

		public AddDialogVM AddDialogVM
		{
			get
			{
				return ServiceLocator.Current.GetInstance<AddDialogVM>();
			}
		}

		public AnalysisSelectionVM AnalysisVM
		{
			get
			{
				return ServiceLocator.Current.GetInstance<AnalysisSelectionVM>();
			}
		}

		public LoginVM LoginVM
		{
			get
			{
				return ServiceLocator.Current.GetInstance<LoginVM> ();
			}
		}
	}
}

