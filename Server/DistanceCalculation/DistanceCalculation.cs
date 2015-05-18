using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Common.DataTransferObjects;
using Newtonsoft.Json;

namespace Server.DistanceCalculation
{
    /// <summary>
    /// Static class, containing functions to calculate the distance between two points, using the Google Distance Matrix API
    /// </summary>
    public static class DistanceCalculation
    {
        public static DistanceContainer CalculateDistanceInKm(IDistanceMatrixPlace origin, IDistanceMatrixPlace destination)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(@"http://maps.googleapis.com/maps/api/distancematrix/xml?origins=");
            urlBuilder.Append(origin.FormatAsDistanceMatrixPlace());
            urlBuilder.Append(@"&destinations=");
            urlBuilder.Append(destination.FormatAsDistanceMatrixPlace());
            urlBuilder.Append(@"&mode=driving&sensor=false&language=en-EN");
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
                        var container = new DistanceContainer();
                        container.Distance = xmlResponse.Descendants("distance").Descendants("value").Sum(x => (float)x / 1000);
                        container.Time = xmlResponse.Descendants("duration").Descendants("value").Sum(x => (float) x / 3600);
                        return container;
                    }

                }
            }

            return new DistanceContainer();
        }

        /// <summary>
        /// Calculates distance (km) and time (h) between the two given coodinates.
        /// </summary>
        /// <param name="positionA"></param>
        /// <param name="positionB"></param>
        /// <returns>distance in km between the two points and time in hours to travel. 0km, 0h if it failed</returns>
        public static DistanceContainer CalculateDistanceInKm(GPSPosition positionA, GPSPosition positionB)
        {
            IDistanceMatrixPlace origin = new DistanceMatrixGPSPosition(positionA);
            IDistanceMatrixPlace destination = new DistanceMatrixGPSPosition(positionB);

            return CalculateDistanceInKm(origin, destination);
        }

        /// <summary>
        /// Calculates distance (km) and time (h) between the a coordiate and an address.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="address"></param>
        /// <returns>distance in km between the two points and time in hours to travel. 0km, 0h if it failed</returns>
        public static DistanceContainer CalculateDistanceInKm(GPSPosition position, Address address)
        {
            IDistanceMatrixPlace origin = new DistanceMatrixGPSPosition(position);
            IDistanceMatrixPlace destination = new DistanceMatrixAddress(address);

            return CalculateDistanceInKm(origin, destination);
        }

        /// <summary>
        /// Calculates distance (km) and time (h) between the two given addresses.
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <returns>distance in km between the two points and time in hours to travel. 0km, 0h if it failed</returns>
        public static DistanceContainer CalculateDistanceInKm(Address address1, Address address2)
        {
            IDistanceMatrixPlace origin = new DistanceMatrixAddress(address1);
            IDistanceMatrixPlace destination = new DistanceMatrixAddress(address2);

            return CalculateDistanceInKm(origin, destination);
        }
    }
}
