using System;
using System.Collections.Generic;
using Common.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DistanceCalculation;
using Server.DriverController;

namespace ServerTests.DriverController
{
    [TestClass]
    public class OptionsEvaluatorTests
    {

        private OptionsEvaluator CreateTestee()
        {
            return new OptionsEvaluator();
        }

        private DriverSendOption CreateDummyOption(string driverUsername, float duration, float distance)
        {
            return new DriverSendOption(new Driver
            {
                UserName = driverUsername
            }, new DistanceContainer { Distance = distance, Time = duration });
        }

        [TestMethod]
        public void ChooseBestOptionOrNull_WithoutOptions_ReturnsNull()
        {
            var testee = CreateTestee();
            var options = new List<DriverSendOption>();

            var result = testee.ChooseBestOptionOrNull(options);
            
            Assert.IsNull(result);
        }
    }
}
