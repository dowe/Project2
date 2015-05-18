using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.DataTransferObjects;

namespace Server.DistanceCalculation
{
    public class DistanceMatrixGPSPosition : IDistanceMatrixPlace
    {

        private GPSPosition adaptee = null;

        public DistanceMatrixGPSPosition(GPSPosition adaptee)
        {
            this.adaptee = adaptee;
        }

        public string FormatAsDistanceMatrixPlace()
        {
            return string.Format("{0},{1}", Regex.Replace(adaptee.Latitude.ToString(), ",", "."),
                Regex.Replace(adaptee.Longitude.ToString(), ",", "."));
        }

    }
}
