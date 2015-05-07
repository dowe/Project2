using System;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Common.Communication.Client;

namespace Smartphone.Driver
{
	public class ViewModelLocator
	{
		
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			IClientConnection clientConnection = new ClientConnection ("http://192.168.43.45:8080/commands");
			clientConnection.Start ();
			SimpleIoc.Default.Register<IClientConnection> (() => clientConnection);
			SimpleIoc.Default.Register<LoginViewModel>();
		}
			
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public LoginViewModel Login
		{
			get
			{
				return ServiceLocator.Current.GetInstance<LoginViewModel>();
			}
		}

	}
}

