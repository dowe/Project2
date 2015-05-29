using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagementSoftware.ViewModel;
using Common.Communication.Client;
using ManagementSoftware.Helper;
using Common.DataTransferObjects;
using ManagementSoftware.Model;
using Common.Util;
using NSubstitute;
using Common.Commands;
using Common.Communication;
using ManagementSoftware.View;
using Tests.Data;

namespace ManagementSoftware.Tests.ViewModel
{
    [TestClass]
    public class RegisterCustomerVMTests
    {
        [TestMethod]
        public void EnumListTest()
        {
            
            RegisterCustomerVM vm = CreateTestee(null, null);

            CollectionAssert.AreEqual(vm.ESMSRequestedValues, Util.EnumValues<ESMSRequested>());
            CollectionAssert.AreEqual(vm.ETitleValues, Util.EnumValues<ETitle>());
        }

        [TestMethod]
        public void RegisterCustomerInputValuesTest()
        {
            RegisterCustomerVM vm = CreateTestee(null, null);

            ETitle title = ETitle.Mr;
            ESMSRequested smsRequested = ESMSRequested.Yes;
            Customer c = TestData.CreateTestData(title, smsRequested);

            SetValues(vm, c, title, smsRequested);

            Assert.AreEqual(vm.FirstName, c.FirstName);
            Assert.AreEqual(vm.Label, c.Label);
            Assert.AreEqual(vm.LastName, c.LastName);
            Assert.AreEqual(vm.UserName, c.UserName);
            Assert.AreEqual(vm.Password, c.Password);
            Assert.AreEqual(vm.MobileNumber, c.MobileNumber);
            Assert.AreEqual(vm.Title.Key, title);
            Assert.AreEqual(vm.SMSRequested.Key, smsRequested);

            Assert.AreEqual(vm.City, c.Address.City);
            Assert.AreEqual(vm.Street, c.Address.Street);
            Assert.AreEqual(vm.PostalCode, c.Address.PostalCode);

            Assert.AreEqual(vm.IBAN, c.BankAccount.IBAN);
            Assert.AreEqual(vm.BankAccountOwner, c.BankAccount.AccountOwner);
        }

        [TestMethod]
        public void RegisterCustomerCmdInputValuesTest()
        {

            var fakeConnection = Substitute.For<IClientConnection>();
            fakeConnection.SendWait<CmdReturnRegisterCustomer>(Arg.Any<CmdRegisterCustomer>()).Returns<CmdReturnRegisterCustomer>((x) => null);

            var fakeMessageBox = Substitute.For<IMessageBox>();

            RegisterCustomerVM vm = CreateTestee(fakeConnection, fakeMessageBox);


            ETitle title = ETitle.Mr;
            ESMSRequested smsRequested = ESMSRequested.Yes;
            Customer c = TestData.CreateTestData(title, smsRequested);

            SetValues(vm, c, title, smsRequested);

            vm.RegisterCustomerAction.Execute(0);

            fakeConnection.Received().SendWait<CmdReturnRegisterCustomer>(Arg.Is<CmdRegisterCustomer>(a => VerifyCustomer(a, c, title, smsRequested)));
            fakeMessageBox.Received().Show(Arg.Is<string>(s => s == RegisterCustomerModel.CONNECTTION_FAILED_MESSAGE));
        }

        private bool VerifyCustomer(CmdRegisterCustomer a, Customer c2, ETitle title, ESMSRequested smsRequested)
        {
            Customer c1 = a.Customer;

            Assert.AreEqual(c1.FirstName, c2.FirstName);
            Assert.AreEqual(c1.Label, c2.Label);
            Assert.AreEqual(c1.LastName, c2.LastName);
            Assert.AreEqual(c1.UserName, c2.UserName);
            Assert.AreEqual(c1.Password, c2.Password);
            Assert.AreEqual(c1.MobileNumber, c2.MobileNumber);
            Assert.AreEqual(c1.Title, c2.Title);
            Assert.AreEqual(c1.SMSRequested, c2.SMSRequested);
            Assert.AreEqual(c1.Address.City, c2.Address.City);
            Assert.AreEqual(c1.Address.Street, c2.Address.Street);
            Assert.AreEqual(c1.Address.PostalCode, c2.Address.PostalCode);

            Assert.AreEqual(c1.BankAccount.IBAN, c2.BankAccount.IBAN);
            Assert.AreEqual(c1.BankAccount.AccountOwner, c2.BankAccount.AccountOwner);

            return true;
        }

        private void SetValues(
            RegisterCustomerVM vm, 
            Customer c, ETitle t, ESMSRequested s)
        {
         
            vm.Title = Util.CreateValuePair<ETitle>(t);
            vm.SMSRequested = Util.CreateValuePair<ESMSRequested>(s);

            vm.FirstName = c.FirstName;
            vm.LastName = c.LastName;
            vm.UserName = c.UserName;
            vm.Password = c.Password;
            vm.MobileNumber = c.MobileNumber;
            vm.Label = c.Label;
            vm.Street = c.Address.Street;
            vm.PostalCode = c.Address.PostalCode;
            vm.City = c.Address.City;
            vm.IBAN = c.BankAccount.IBAN;
            vm.BankAccountOwner = c.BankAccount.AccountOwner;
        }

        private RegisterCustomerVM CreateTestee(
            IClientConnection connection, IMessageBox messageBox)
        {
            return new RegisterCustomerVM(connection, messageBox);
        }

        
    }
}
