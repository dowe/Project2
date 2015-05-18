using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Server.DistanceCalculation
{
    /// <summary>
    /// This calculaltor uses the google maps directions web api.
    /// </summary>
    public class DirectionsRouteDistanceCalculator : IRouteDistanceCalculator
    {

        private bool optimizeOrder = false;

        public DirectionsRouteDistanceCalculator(bool optimizeOrder)
        {
            this.optimizeOrder = optimizeOrder;
        }

        public async Task<DistanceContainer> CalculateRouteDistance(IList<IDistanceMatrixPlace> waypoints)
        {
            DistanceContainer result = new DistanceContainer();
            if (waypoints.Count > 1)
            {
                StringBuilder urlBuilder = new StringBuilder();
                urlBuilder.Append(@"http://maps.googleapis.com/maps/api/directions/xml?origin=");
                urlBuilder.Append(waypoints[0].FormatAsDistanceMatrixPlace());
                urlBuilder.Append(@"&destination=");
                urlBuilder.Append(waypoints[waypoints.Count - 1].FormatAsDistanceMatrixPlace());
                if (waypoints.Count != 2)
                {
                    urlBuilder.Append(@"&waypoints=");
                    if (optimizeOrder)
                    {
                        urlBuilder.Append(@"optimize:true|");
                    }
                    for (int i = 1; i < waypoints.Count - 1; i++)
                    {
                        if (i != 1)
                        {
                            urlBuilder.Append(@"|");
                        }
                        urlBuilder.Append(waypoints[i].FormatAsDistanceMatrixPlace());
                    }
                }
                urlBuilder.Append(@"&mode=driving&sensor=false&language=en-EN");
                string url = urlBuilder.ToString();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseString = streamReader.ReadToEnd();

                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var xmlResponse = XDocument.Parse(responseString);

                        if (xmlResponse.Descendants("status").FirstOrDefault().Value == "OK")
                        {
                            // Sum up all distances and durations.
                            result.Distance =
                                xmlResponse.XPathSelectElements("/DirectionsResponse/route/leg/distance/value")
                                    .Sum(x => float.Parse(x.Value) / 1000);
                            result.Time =
                                xmlResponse.XPathSelectElements("/DirectionsResponse/route/leg/duration/value")
                                .Sum(x => float.Parse(x.Value) / 3600);
                        }

                    }
                }
            }

            return result;
        }
    }
}
