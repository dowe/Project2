using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Communication.Client;
using ManagementSoftware.ViewModel;
using ManagementSoftware.Helper;
using ManagementSoftware.Model;

namespace ManagementSoftware.Tests.ViewModel
{
    [TestClass]
    public class ShiftScheduleVMTests
    {
        [TestMethod]
        public void SetUpTest()
        {
            ShiftScheduleVM testee = CreateTestee(null, null);

            Assert.AreEqual(testee.ShiftScheduleMonthVM, testee.CurrentViewModel);
            Assert.AreEqual(testee.SwitchMonthButtonText, ShiftScheduleModel.SWITCH_TO_NEXT_MONTH_TEXT);
            Assert.AreEqual(testee.CurrentMonthText, ShiftScheduleModel.CURRENT_MONTH_TEXT_NO_DATA);
        }

        private ShiftScheduleVM CreateTestee(
            IClientConnection connection, IMessageBox messageBox)
        {
            //testee.LoadRawModelCommand
            //testee.SwitchMonthDataCommand
            //testee.SwitchToShiftScheduleMonthVM
            //testee.SwitchToShiftScheduleDayVM

            //testee.CurrentMonthText
            //testee.CurrentViewModel
            //testee.SwitchMonthButtonText

            return new ShiftScheduleVM(connection, messageBox);
        }
    }
}
