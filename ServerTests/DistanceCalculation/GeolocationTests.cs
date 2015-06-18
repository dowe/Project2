using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Server.DistanceCalculation.Tests
{
    [TestClass()]
    public class GeolocationTests
    {
        [TestMethod()]
        public void ConvertToGPSTest()
        {
            var gps = Geolocation.ConvertToGPS(new DistanceMatrixAddress(new Address("Jauschbach 6", "77784", "Oberharmersbach")));
            Assert.IsTrue(gps.Latitude > 48.3840300 - 100 && gps.Latitude < 48.3840300 + 100);
        }

        [TestMethod()]
        public void ConvertToGPSNullTest()
        {
            var gps = Geolocation.ConvertToGPS(new DistanceMatrixAddress(new Address()));
            Assert.IsNull(gps);
        }
    }
}
