using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagementSoftware.ViewModel;
using Common.Communication.Client;
using ManagementSoftware.Helper;
using NSubstitute;
using Common.Commands;
using Common.DataTransferObjects;
using System.Collections.Generic;

namespace ManagementSoftware.Tests.ViewModel
{
    [TestClass]
    public class CustomerListVMTests
    {
        [TestMethod]
        public void LoadDataSuccess()
        {

            List<Customer> _List = new List<Customer>();

            _List.Add(new Customer("a", "b", "c", "d", new Address("e", "f", "g"), "h", new BankAccount("i", "j"), true, "k"));
            _List.Add(new Customer("a", "b", "c", "d", new Address("e", "f", "g"), "h", new BankAccount("i", "j"), true, "k"));
            _List.Add(new Customer("a", "b", "c", "d", new Address("e", "f", "g"), "h", new BankAccount("i", "j"), true, "k"));

            List<Customer> _ListCopy = new List<Customer>();
            foreach (Customer c in _List)
            {
                _ListCopy.Add(c);
            }

            CmdReturnGetAllCustomers response = new CmdReturnGetAllCustomers(Guid.NewGuid(), _List);
            var fakeConnection = Substitute.For<IClientConnection>();
            fakeConnection.SendWait<CmdReturnGetAllCustomers>(Arg.Any<CmdGetAllCustomers>()).Returns<CmdReturnGetAllCustomers>((x) => response);

            var fakeMessageBox = Substitute.For<IMessageBox>();

            CustomerListVM testee = CreateTestee(fakeConnection, fakeMessageBox);

            testee.LoadCommand.Execute(0);

            CollectionAssert.AreEqual(new List<Customer>(testee.DataList), _ListCopy);
            fakeMessageBox.Received().Show(Arg.Is<string>(s => s == CustomerListVM.LOAD_SUCCESS));
        }

        [TestMethod]
        public void LoadDataFailure()
        {

            var fakeConnection = Substitute.For<IClientConnection>();
            fakeConnection.SendWait<CmdReturnGetAllCustomers>(Arg.Any<CmdGetAllCustomers>()).Returns<CmdReturnGetAllCustomers>((x) => null);

            var fakeMessageBox = Substitute.For<IMessageBox>();

            CustomerListVM testee = CreateTestee(fakeConnection, fakeMessageBox);

            testee.LoadCommand.Execute(0);

            fakeMessageBox.Received().Show(Arg.Is<string>(s => s == CustomerListVM.LOAD_FAILURE));
        }


        private CustomerListVM CreateTestee(
            IClientConnection connection, IMessageBox messageBox)
        {
            return new CustomerListVM(connection, messageBox);
        }
    }
}
