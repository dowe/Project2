using System;
using Xamarin.Forms;
using Common.DataTransferObjects;
using System.Text;

namespace Smartphone.Driver.NativeServices
{
	public class NativeMapAppLauncher
	{

		public void LaunchMapApp(Address address)
		{
			StringBuilder place = new StringBuilder ();
			place.Append (address.Street);
			place.Append (" ");
			place.Append (address.PostalCode);
			place.Append (" ");
			place.Append (address.City);

			LaunchMapApp(place.ToString());
		}

		public void LaunchMapApp(GPSPosition position)
		{
			StringBuilder place = new StringBuilder ();
			place.Append (position.Latitude);
			place.Append (",");
			place.Append (position.Longitude);

			LaunchMapApp (place.ToString ());
		}

		public void LaunchMapApp(string place)
		{
			var addr = Uri.EscapeUriString(place);

			var request = string.Format ("geo:0,0?q={0}", addr);

			Device.OpenUri(new Uri(request));
		}

	}
}

