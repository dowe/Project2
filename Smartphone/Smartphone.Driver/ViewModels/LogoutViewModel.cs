using System;
using GalaSoft.MvvmLight;
using Common.Communication.Client;

namespace Smartphone.Driver
{
	public class LogoutViewModel : ViewModelBase
	{

		private const string EndKmProperty = "EndKm";
		private const string IsCommunicatingProperty = "IsCommunicating";
		private const string IsNotCommunicatingProperty = "IsNotCommunicating";

		private IClientConnection connection = null;
		private float endKm = 3;
		private bool isCommunicating = false;

		public LogoutViewModel (IClientConnection connection)
		{
			this.connection = connection;
		}

		public float EndKm
		{
			get {
				return endKm;
			}
			set {
				if (Math.Abs (value - endKm) > float.Epsilon)
				{
					endKm = value;
					RaisePropertyChanged (EndKmProperty);
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

	}
}

