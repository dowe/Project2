using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace Smartphone.Driver
{
	public class NativeMap
	{
		
		public class Place {
			public string Name { get; set; }
			public string Vicinity { get; set; }
			public Position Location { get; set; }
			public Uri Icon { get; set; }
		}

		public void LaunchMapApp(Place place)
		{
			// Windows Phone doesn't like ampersands in the names and the normal URI escaping doesn't help
			var name = place.Name.Replace("&", "and"); // var name = Uri.EscapeUriString(place.Name);
			var loc = string.Format("{0},{1}", place.Location.Latitude, place.Location.Longitude);
			var addr = Uri.EscapeUriString(place.Vicinity);

			var request = Device.OnPlatform(
				// iOS doesn't like %s or spaces in their URLs, so manually replace spaces with +s
				string.Format("http://maps.apple.com/maps?q={0}&sll={1}", name.Replace(' ', '+'), loc),

				// pass the address to Android if we have it
				string.Format("geo:0,0?q={0}({1})", string.IsNullOrWhiteSpace(addr) ? loc : addr, name),

				// WinPhone
				string.Format("bingmaps:?cp={0}&q={1}", loc, name)
			);

			Device.OpenUri(new Uri(request));
		}

	}
}

