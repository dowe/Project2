using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagementSoftware.ViewModel;
using Common.Communication.Client;
using ManagementSoftware.Helper;
using Common.DataTransferObjects;
using System.Collections.Generic;
using NSubstitute;
using Common.Commands;
using Tests.Data;
using System.Threading;
using ManagementSoftware.Model;
using Common.Util;
using System.Windows.Controls;


namespace ManagementSoftware.Tests.ViewModel
{
    [TestClass]
    public class CreateOrderVMTests
    {

        private MyListBox listBoxAvaibleAnalysis;
        private List<AnalysisM> listAvaibleAnalysis;

        private IClientConnection fakeConnection;
        private IMessageBox fakeMessageBox;

        private CreateOrderVM testee;

        [TestInitialize]
        public void SetUp()
        {
            listBoxAvaibleAnalysis = new MyListBox();
            listBoxAvaibleAnalysis.SelectionMode = SelectionMode.Multiple;

            List<Analysis> list = TestData.CreateAnalysis();
            listAvaibleAnalysis = To(list);
            listBoxAvaibleAnalysis.ItemsSource = listAvaibleAnalysis;


            listBoxAvaibleAnalysis.Select(Copy(listAvaibleAnalysis));

            CmdReturnGetAnalyses cmdAnalysis = new CmdReturnGetAnalyses(Guid.NewGuid(), list);

            fakeConnection = Substitute.For<IClientConnection>();
            fakeConnection.SendWait<CmdReturnGetAnalyses>(Arg.Any<CmdGetAnalyses>()).Returns<CmdReturnGetAnalyses>((x) => cmdAnalysis);

            fakeMessageBox = Substitute.For<IMessageBox>();

            testee = CreateTestee(fakeConnection, fakeMessageBox);
            testee.SetBox(listBoxAvaibleAnalysis);


            Thread.Sleep(200);
        }

        [TestMethod]
        public void CreateOrderVMSetUpTest()
        {
            Assert.AreEqual(null, To(null));
            Assert.AreEqual(null, Copy(null));

            List<AnalysisM> avaibleAnalysis = testee.AvaibleAnalysis;

            CollectionAssert.AreEqual(avaibleAnalysis, Copy(listAvaibleAnalysis));

            Assert.AreEqual(testee.Cost, String.Format(CreateOrderVM.TOTAL_COST_PATTERN, Util.ToCost(0.0F)));

            Assert.AreEqual(testee.CustomerAddressText, CreateOrderM.UNKNOWN_CUSTOMER_ADDRESS_TEXT);
            Assert.AreEqual(testee.CustomerUsername, "");
            Assert.AreEqual(testee.NewPatientID, "");
            CollectionAssert.AreEqual(testee.PatientIDs, new List<String>());
            Assert.AreEqual(testee.PatientIDText, CreateOrderM.UNSELECTED_PATIENT_TEXT);
            Assert.AreEqual(testee.RemovePatientAction.CanExecute(0), false);
            Assert.AreEqual(testee.CreateOrderAction.CanExecute(0), false);
            CollectionAssert.AreEqual(testee.SelectedAnalysis, new List<AnalysisM>());
            Assert.AreEqual(testee.SelectedPatient, null);


            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_USERNAME);
            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            validationList.Add(CreateOrderM.INVALID_PATIENT_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);

        }

        [TestMethod]
        public void NoResponseCustomerUsernameTest()
        {
            string unknownUsername = "UnknwonUser;asdcxcsdsdea02_a";
            fakeConnection.SendWait<CmdReturnGetCustomerAddress>(Arg.Any<CmdGetCustomerAddress>()).Returns<CmdReturnGetCustomerAddress>((x) => null);

            testee.CustomerUsername = unknownUsername;
            Thread.Sleep(200);


            fakeConnection.Received().SendWait<CmdReturnGetCustomerAddress>(Arg.Is<CmdGetCustomerAddress>((cmd) => cmd.CustomerUsername == unknownUsername));
            fakeMessageBox.Received().Show(Arg.Is<string>(s => s == CreateOrderM.NORESPONSE_LOAD_CUSTOMER_ADDRESS));

            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            validationList.Add(CreateOrderM.INVALID_PATIENT_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);
        }

        [TestMethod]
        public void UnknownCustomerUsernameTest()
        {
            CmdReturnGetCustomerAddress response = new CmdReturnGetCustomerAddress(Guid.NewGuid(), null);
            string unknownUsername = "UnknwonUser;asdcxcsdsdea02_a";
            fakeConnection.SendWait<CmdReturnGetCustomerAddress>(Arg.Any<CmdGetCustomerAddress>()).Returns<CmdReturnGetCustomerAddress>((x) => response);

            testee.CustomerUsername = unknownUsername;
            Thread.Sleep(200);

            fakeConnection.Received().SendWait<CmdReturnGetCustomerAddress>(Arg.Is<CmdGetCustomerAddress>((cmd) => cmd.CustomerUsername == unknownUsername));

            Assert.AreEqual(testee.CustomerAddressText, CreateOrderM.UNKNOWN_CUSTOMER_ADDRESS_TEXT);

            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            validationList.Add(CreateOrderM.INVALID_PATIENT_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);
        }

        [TestMethod]
        public void KnownCustomerUsernameTest()
        {
            Address knownAddress = new Address("street", "plz", "city");
            CmdReturnGetCustomerAddress response = new CmdReturnGetCustomerAddress(Guid.NewGuid(), knownAddress);
            string knownUsername = "KnwonUser;asdcxcsdsdea02_a";
            fakeConnection.SendWait<CmdReturnGetCustomerAddress>(Arg.Any<CmdGetCustomerAddress>()).Returns<CmdReturnGetCustomerAddress>((x) => response);

            testee.CustomerUsername = knownUsername;
            Thread.Sleep(200);

            fakeConnection.Received().SendWait<CmdReturnGetCustomerAddress>(Arg.Is<CmdGetCustomerAddress>((cmd) => cmd.CustomerUsername == knownUsername));

            string addressText = String.Format(CreateOrderM.CUSTOMER_ADRESS_TEXT_PATTERN,
                knownAddress.Street, knownAddress.PostalCode, knownAddress.City);
            Assert.AreEqual(testee.CustomerAddressText, addressText);
            Assert.AreEqual(testee.CustomerUsername, knownUsername);


            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_PATIENT_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);
        }

        [TestMethod]
        public void InvalidUsernameTest()
        {
            string invalidUsername = "     ";
            testee.CustomerUsername = invalidUsername;

            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_USERNAME);
            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            validationList.Add(CreateOrderM.INVALID_PATIENT_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);
        }

        [TestMethod]
        public void ValidUsernameTest()
        {
            string validUsername = "a     ";
            testee.CustomerUsername = validUsername;

            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            validationList.Add(CreateOrderM.INVALID_PATIENT_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);
        }

        [TestMethod]
        public void AddPatientTwiceTest()
        {
            string samePatient = "PatZwilling";
            List<string> patientList = new List<string>();
            patientList.Add(samePatient);

            testee.NewPatientID = samePatient;
            testee.AddPatientAction.Execute(0);
            testee.NewPatientID = samePatient;
            testee.AddPatientAction.Execute(0);

            CollectionAssert.AreEqual(testee.PatientIDs, patientList);
            Assert.AreEqual(testee.SelectedPatient, samePatient);
            Assert.AreEqual(testee.PatientIDText, String.Format(CreateOrderM.SELECTED_PATIENT_PATTERN, samePatient));

            List<string> validationList = new List<string>();

            validationList.Add(CreateOrderM.INVALID_USERNAME);
            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            validationList.Add(CreateOrderM.INVALID_TEST_COUNT);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);

        }

        [TestMethod]
        public void AddPatientsSelectAnalysesRemovePatientTest()
        {
            string a = "A";
            string b = "B";
            List<string> pIDs = new List<string>();
            List<AnalysisM> selAnalysis = new List<AnalysisM>();

            //Add A 
            testee.NewPatientID = a;
            testee.AddPatientAction.Execute(0);

            //?just A
            pIDs.Add(a);
            CollectionAssert.AreEqual(pIDs, testee.PatientIDs);
            Assert.AreEqual(testee.PatientIDText, String.Format(CreateOrderM.SELECTED_PATIENT_PATTERN, a));

            //Select analyses 0
            selAnalysis.Add(listAvaibleAnalysis[0]);
            listBoxAvaibleAnalysis.Select(selAnalysis);
            testee.SelectedAnalysisChanged();

            //?just 0 selected
            CollectionAssert.AreEqual(testee.SelectedAnalysis, Copy(selAnalysis));

            //Add B 
            testee.NewPatientID = b;
            testee.AddPatientAction.Execute(0);

            //?just A+B 
            pIDs.Add(b);
            CollectionAssert.AreEqual(pIDs, testee.PatientIDs);
            Assert.AreEqual(testee.PatientIDText, String.Format(CreateOrderM.SELECTED_PATIENT_PATTERN, b));

            //Select analyses 1
            selAnalysis.Clear();
            selAnalysis.Add(listAvaibleAnalysis[1]);
            listBoxAvaibleAnalysis.Select(selAnalysis);
            testee.SelectedAnalysisChanged();

            //?just 1 selected
            CollectionAssert.AreEqual(testee.SelectedAnalysis, Copy(selAnalysis));

            //?Cost
            float f = 0.0F;
            f += listAvaibleAnalysis[0].Analysis.PriceInEuro;
            f += listAvaibleAnalysis[1].Analysis.PriceInEuro;
            Assert.AreEqual(testee.Cost, String.Format(CreateOrderVM.TOTAL_COST_PATTERN, Util.ToCost(f)));

            //Remove B
            Assert.IsTrue(testee.RemovePatientAction.CanExecute(0));
            testee.RemovePatientAction.Execute(0);
            Assert.AreEqual(testee.PatientIDText, CreateOrderM.UNSELECTED_PATIENT_TEXT);
            Assert.AreEqual(testee.SelectedPatient, null);

            //?just A
            pIDs.Clear();
            pIDs.Add(a);
            CollectionAssert.AreEqual(pIDs, testee.PatientIDs);

            //Select patient A
            testee.SelectedPatient = a;
            Assert.AreEqual(a, testee.SelectedPatient);
            Assert.AreEqual(testee.PatientIDText, String.Format(CreateOrderM.SELECTED_PATIENT_PATTERN, a));

            //?just 0 selected
            selAnalysis.Clear();
            selAnalysis.Add(listAvaibleAnalysis[0]);
            CollectionAssert.AreEqual(testee.SelectedAnalysis, Copy(selAnalysis));

            //?validation
            List<string> validationList = new List<string>();
            validationList.Add(CreateOrderM.INVALID_USERNAME);
            validationList.Add(CreateOrderM.INVALID_ADDRESS);
            CollectionAssert.AreEqual(testee.ValidationText, validationList);

        }

        [TestMethod]
        public void AddInvalidPatientTest()
        {
            List<string> empty = new List<string>();
            testee.NewPatientID = " ";
            testee.AddPatientAction.Execute(0);
            CollectionAssert.AreEqual(testee.PatientIDs, empty);

            testee.NewPatientID = null;
            testee.AddPatientAction.Execute(0);
            CollectionAssert.AreEqual(testee.PatientIDs, empty);
        }

        [TestMethod]
        public void NullListBoxTests()
        {
            testee.SetBox(null);

            testee.SelectedAnalysisChanged();

            testee.SelectedPatient = null;

            CollectionAssert.AreEqual(testee.SelectedAnalysis, new List<AnalysisM>());
        }

        [TestMethod]
        public void SelectedPatientNullTest()
        {
            testee.SelectedPatient = null;

            List<AnalysisM> selAnalysis = new List<AnalysisM>();
            selAnalysis.Add(listAvaibleAnalysis[1]);
            listBoxAvaibleAnalysis.Select(selAnalysis);
            testee.SelectedAnalysisChanged();

            Assert.AreEqual(0, testee.SelectedAnalysis.Count);
        }

        [TestMethod]
        public void SetUpOrderAndCancelTest()
        {
            SetUpValidOrder();

            testee.CancelOrderAction.Execute(0);

            CreateOrderVMSetUpTest();
        }

        [TestMethod]
        public void SetUpOrderAndNoResponseTest()
        {
            SetUpValidOrder();

            testee.CreateOrderAction.Execute(0);

            fakeConnection.Received().SendWait<CmdReturnAddOrder>(Arg.Is<CmdAddOrder>((x) => Validate(x)));
            fakeMessageBox.Received().Show(Arg.Is<string>(s => s == CreateOrderM.NORESPONSE_CREATE_ORDER));
        }

        [TestMethod]
        public void SetUpOrderAndSuccessTest()
        {
            SetUpValidOrder();

            CmdReturnAddOrder response = new CmdReturnAddOrder(Guid.NewGuid(), 333);
            fakeConnection.SendWait<CmdReturnAddOrder>(Arg.Any<CmdAddOrder>()).Returns<CmdReturnAddOrder>((x) => response);

            testee.CreateOrderAction.Execute(0);

            fakeMessageBox.Received().Show(Arg.Is<string>(s => s == CreateOrderM.SUCCESS_CREATE_ORDER_PREFIX + 333));
        }

        private bool Validate(CmdAddOrder x)
        {

            Assert.AreEqual(x.CustomerUsername, "user");

            Dictionary<String, List<Analysis>> PatientTests = new Dictionary<string, List<Analysis>>();

            Assert.IsTrue(x.PatientTests.ContainsKey("A"));
            List<Analysis> list = x.PatientTests["A"];
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(list[0].Name, listAvaibleAnalysis[0].Analysis.Name);

            return true;
        }

        private void SetUpValidOrder()
        {
            string a = "A";
            testee.NewPatientID = a;
            testee.AddPatientAction.Execute(0);

            List<AnalysisM> sel = new List<AnalysisM>();
            sel.Add(listAvaibleAnalysis[0]);
            listBoxAvaibleAnalysis.Select(sel);
            testee.SelectedAnalysisChanged();

            Address knownAddress = new Address("street", "plz", "city");
            CmdReturnGetCustomerAddress response = new CmdReturnGetCustomerAddress(Guid.NewGuid(), knownAddress);
            string knownUsername = "user";
            fakeConnection.SendWait<CmdReturnGetCustomerAddress>(Arg.Any<CmdGetCustomerAddress>()).Returns<CmdReturnGetCustomerAddress>((x) => response);

            testee.CustomerUsername = knownUsername;
            Thread.Sleep(200);

            Assert.IsTrue(testee.CreateOrderAction.CanExecute(0));
            CollectionAssert.AreEqual(testee.ValidationText, new List<string>());
        }

        private CreateOrderVM CreateTestee(
            IClientConnection connection,
            IMessageBox messageBox)
        {
            return new CreateOrderVM(
                connection,
                messageBox);
        }

        private List<AnalysisM> To(List<Analysis> list)
        {
            if (list == null)
            {
                return null;
            }
            List<AnalysisM> toList = new List<AnalysisM>();
            foreach (Analysis item in list)
            {
                toList.Add(new AnalysisM(item));
            }
            return toList;
        }

        private List<AnalysisM> Copy(List<AnalysisM> list)
        {
            if (list == null)
            {
                return null;
            }
            List<AnalysisM> toList = new List<AnalysisM>();
            foreach (AnalysisM item in list)
            {
                toList.Add(item);
            }
            return toList;
        }


    }
}
