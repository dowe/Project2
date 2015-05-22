using System;
using Smartphone.Driver.NativeServices;
using Android.Widget;
using Smartphone.Driver.Droid.NativeServices;

[assembly: Xamarin.Forms.Dependency (typeof (Toaster_Android))]
namespace Smartphone.Driver.Droid.NativeServices
{
	public class Toaster_Android : IToaster
	{
			
		public void MakeToast (string message)
		{
			Toast.MakeText (Xamarin.Forms.Forms.Context, message, ToastLength.Long).Show ();
		}

	}
}

