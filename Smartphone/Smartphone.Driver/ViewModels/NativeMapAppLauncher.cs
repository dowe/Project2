using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Common.DataTransferObjects;
using System.Text;

namespace Smartphone.Driver
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

		public void LaunchMapApp(string place)
		{
			// Windows Phone doesn't like ampersands in the names and the normal URI escaping doesn't help
			var addr = Uri.EscapeUriString(place);

			var request = string.Format ("geo:0,0?q={0}", addr);

			Device.OpenUri(new Uri(request));
		}

	}
}

