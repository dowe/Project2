using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server.DistanceCalculation
{
    public static class Geolocation
    {
        /// <summary>
        /// Converts an address to a GPSPosition
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static GPSPosition ConvertToGPS(DistanceMatrixAddress address)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(@"https://maps.googleapis.com/maps/api/geocode/xml?address=");
            urlBuilder.Append(address.FormatAsDistanceMatrixPlace());
            urlBuilder.Append(@"&key=AIzaSyDAdzdTF82kLM_-Rz0xdskVUpMGEVqG-WQ");
            string url = urlBuilder.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                if (!string.IsNullOrEmpty(result))
                {
                    var xmlResponse = XDocument.Parse(result);

                    if (xmlResponse.Descendants("status").FirstOrDefault().Value == "OK")
                    {
                        var gps = new GPSPosition();
                        gps.Latitude = (float)xmlResponse.Descendants("geometry").Descendants("location").Descendants("lat").FirstOrDefault();
                        gps.Longitude = (float)xmlResponse.Descendants("geometry").Descendants("location").Descendants("lng").FirstOrDefault();
                        return gps;
                    }
                }
            }

            return null;
        }
    }
}
