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
        /// <summary>
        /// Calculates distance (km) and time (h) between the two given coodinates.
        /// </summary>
        /// <param name="positionA"></param>
        /// <param name="positionB"></param>
        /// <returns>distance in km between the two points and time in hours to travel</returns>
        public static DistanceContainer CalculateDistanceInKm(GPSPosition positionA, GPSPosition positionB)
        {
            string url = string.Format(@"http://maps.googleapis.com/maps/api/distancematrix/xml?origins={0},{1}&destinations={2},{3}&mode=driving&sensor=false&language=en-EN", Regex.Replace(positionA.Latitude.ToString(), ",", "."), Regex.Replace(positionA.Longitude.ToString(), ",", "."), Regex.Replace(positionB.Latitude.ToString(), ",", "."), Regex.Replace(positionB.Longitude.ToString(), ",", "."));

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
                        container.Distance = (float)xmlResponse.Descendants("distance").Descendants("value").FirstOrDefault() / 1000;
                        container.Time = (float)xmlResponse.Descendants("duration").Descendants("value").FirstOrDefault() / 3600;
                        return container;
                    }

                }
            }

            return new DistanceContainer();
        }

        /// <summary>
        /// Calculates distance (km) and time (h) between the two given coodinates.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="address"></param>
        /// <returns>distance in km between the two points and time in hours to travel</returns>
        public static DistanceContainer CalculateDistanceInKm(GPSPosition position, Address address)
        {
            string url = string.Format(@"http://maps.googleapis.com/maps/api/distancematrix/xml?origins={0},{1}&destinations={2}+{3}+{4}&mode=driving&sensor=false&language=en-EN", Regex.Replace(position.Latitude.ToString(), ",", "."), Regex.Replace(position.Longitude.ToString(), ",", "."), address.Street, address.PostalCode, address.City);

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
                        container.Distance = (float)xmlResponse.Descendants("distance").Descendants("value").FirstOrDefault() / 1000;
                        container.Time = (float)xmlResponse.Descendants("duration").Descendants("value").FirstOrDefault() / 3600;
                        return container;
                    }

                }
            }

            return new DistanceContainer();
        }
    }
}
