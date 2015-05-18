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

        [TestMethod]
        public void ChooseBestOptionOrNull_WithNoOptionPassingHardConstraint_ReturnsNull()
        {
            var testee = CreateTestee();
            testee.AddHardConstraint(o => o.TotalLeftDistance.Time < 6);
            var options = new List<DriverSendOption>()
            {
                CreateDummyOption("o1", 7f, 50000f)
            };

            var result = testee.ChooseBestOptionOrNull(options);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ChooseBestOptionOrNull_WithOneOptionPassesHardConstraint_ReturnNull()
        {
            var testee = CreateTestee();
            testee.AddHardConstraint(o => o.TotalLeftDistance.Time < 6);
            var options = new List<DriverSendOption>()
            {
                CreateDummyOption("o1", 5f, 50000f),
                CreateDummyOption("o2", 7f, 50000f)
            };

            var result = testee.ChooseBestOptionOrNull(options);

            Assert.AreEqual("o1", result.Driver.UserName);
        }
    }
}
