using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Data;
using ManagementSoftware.Model;
using Common.DataTransferObjects;
using Common.Util;

namespace ManagementSoftware.Tests.Model
{
    [TestClass]
    public class AnalysisMTests
    {
        [TestMethod]
        public void ToStringTest()
        {
            Analysis a = TestData.CreateAnalysis()[0];

            AnalysisModel testee = new AnalysisModel(a);

            string text = String.Format(AnalysisModel.FORMAT_PATTERN, a.Name, Util.ToCost(a.PriceInEuro));

            Assert.AreEqual(testee.ToString(), text);
            Assert.AreEqual(testee.GetHashCode(), a.Name.GetHashCode());
        }
    }
}
