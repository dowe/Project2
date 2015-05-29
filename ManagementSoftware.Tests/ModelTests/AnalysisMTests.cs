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

            AnalysisM testee = new AnalysisM(a);

            string text = String.Format(AnalysisM.FORMAT_PATTERN, a.Name, Util.ToCost(a.PriceInEuro));

            Assert.AreEqual(testee.ToString(), text);
        }
    }
}
