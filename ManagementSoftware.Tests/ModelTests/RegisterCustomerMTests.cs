using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using Common.Util;
using ManagementSoftware.Helper;
using ManagementSoftware.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Tests.Model
{
    [TestClass]
    public class RegisterCustomerMTests
    {

        [TestMethod]
        public void RegisterCustomerSuccessWithMessageTest() {

            string successMessage = "HI;";
            CmdReturnRegisterCustomer response;
            response = new CmdReturnRegisterCustomer(Guid.NewGuid(), true, successMessage);
            Assert.AreEqual(RegisterCustomer(response), successMessage);
        }

        [TestMethod]
        public void RegisterCustomerSuccessWithoutMessageTest() {

            CmdReturnRegisterCustomer response;
            response = new CmdReturnRegisterCustomer(Guid.NewGuid(), true, null);
            Assert.AreEqual(RegisterCustomer(response), RegisterCustomerM.SUCCESS_MESSAGE);
        }

        [TestMethod]
        public void RegisterCustomerError()
        {
            CmdReturnRegisterCustomer response;
            string errorMessage = "HI;";
            response = new CmdReturnRegisterCustomer(Guid.NewGuid(), false, errorMessage);
            Assert.AreEqual(RegisterCustomer(response), errorMessage);
        }

        [TestMethod]
        
        public void RegisterCustomerIllegalState()
        {
                CmdReturnRegisterCustomer response;
                response = new CmdReturnRegisterCustomer(Guid.NewGuid(), false, null);
                Assert.AreEqual(RegisterCustomer(response), RegisterCustomerM.ILLEGAL_STATE_MESSAGE);
        }

        public string RegisterCustomer(CmdReturnRegisterCustomer cmd)
        {

            var fakeConnection = Substitute.For<IClientConnection>();
            fakeConnection.SendWait<CmdReturnRegisterCustomer>(Arg.Any<CmdRegisterCustomer>()).Returns<CmdReturnRegisterCustomer>(cmd);

            RegisterCustomerM model = CreateTestee(fakeConnection);

            model.Customer = CreateTestData(ETitle.Mr, ESMSRequested.Yes);

            return model.RegisterCustomer();
        }


        private RegisterCustomerM CreateTestee(
            IClientConnection connection)
        {
            return new RegisterCustomerM(connection);
        }

        private Customer CreateTestData(ETitle title, ESMSRequested smsRequested)
        {

            Customer _Customer = new Customer();
            _Customer.Title = Util.CreateValuePair<ETitle>(title).Value;
            _Customer.FirstName = "Hans";
            _Customer.LastName = "Feil";
            _Customer.UserName = "hfeil";
            _Customer.Password = "asdf";
            _Customer.SMSRequested = smsRequested == ESMSRequested.Yes;
            _Customer.MobileNumber = "016212345";
            _Customer.Label = "Praxis ABC";
            _Customer.Address = new Address("abc 8", "77656", "Offenburg");
            _Customer.BankAccount = new BankAccount("12345-DE", "Hans feil");
            return _Customer;
        }
    }
}
