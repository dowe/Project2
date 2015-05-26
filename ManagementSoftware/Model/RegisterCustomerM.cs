using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using Common.Util;
using ManagementSoftware.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

namespace ManagementSoftware.Model
{



    public class RegisterCustomerM
    {
        public static readonly string CONNECTTION_FAILED_MESSAGE = "Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut";
        public static readonly string ILLEGAL_STATE_MESSAGE = "Success is false and Error null. Illegal State";
        public static readonly string SUCCESS_MESSAGE = "Kunde wurde angelegt";

        private IClientConnection _ClientConnection;
        
        private KeyValuePair<ETitle, string> _Title;
        private KeyValuePair<ESMSRequested, string> _SMSRequested;


        public RegisterCustomerM(
            IClientConnection _ClientConnection)
        {
            this._ClientConnection = _ClientConnection;

            Customer = new Customer();
            Customer.BankAccount = new BankAccount();
            Customer.Address = new Address();

            ETitleValues = Util.EnumValues<ETitle>();
            ESMSRequestedValues = Util.EnumValues<ESMSRequested>();
            Title = Util.CreateValuePair<ETitle>(ETitle.Mr);
            SMSRequested = Util.CreateValuePair<ESMSRequested>(ESMSRequested.No);
        }

        public string RegisterCustomer()
        {
            Customer copyOfCustomer = new Customer();
            copyOfCustomer.Address = new Address();
            copyOfCustomer.BankAccount = new BankAccount();

            copyOfCustomer.LastName = Customer.LastName;
            copyOfCustomer.FirstName = Customer.FirstName;
            copyOfCustomer.Label = Customer.Label;
            copyOfCustomer.Title = Customer.Title;
            copyOfCustomer.UserName = Customer.UserName;
            copyOfCustomer.Password = Customer.Password;
            copyOfCustomer.MobileNumber = Customer.MobileNumber;
            copyOfCustomer.SMSRequested = Customer.SMSRequested;
            copyOfCustomer.Address.Street = Customer.Address.Street;
            copyOfCustomer.Address.City = Customer.Address.City;
            copyOfCustomer.Address.PostalCode = Customer.Address.PostalCode;
            copyOfCustomer.BankAccount.AccountOwner = Customer.BankAccount.AccountOwner;
            copyOfCustomer.BankAccount.IBAN = Customer.BankAccount.IBAN;

            CmdRegisterCustomer request = new CmdRegisterCustomer(copyOfCustomer);
            CmdReturnRegisterCustomer response = _ClientConnection.SendWait<CmdReturnRegisterCustomer>(request);

            if (response == null)
            {
                return CONNECTTION_FAILED_MESSAGE;
            }
            else if (response.Success)
            {
                return (response.Message != null ? response.Message : SUCCESS_MESSAGE);
            }
            else if (response.Message == null)
            {
                return ILLEGAL_STATE_MESSAGE;
            }
            else
            {
                return response.Message;
            }
        }

        public Dictionary<ETitle, string> ETitleValues { get; set; }
        public Dictionary<ESMSRequested, string> ESMSRequestedValues { get; set; }
        public Customer Customer { get; set; }
        public KeyValuePair<ETitle, string> Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                Customer.Title = _Title.Value;
            }
        }
        public KeyValuePair<ESMSRequested, string> SMSRequested
        {
            get
            {
                return _SMSRequested;
            }
            set
            {
                _SMSRequested = value;
                Customer.SMSRequested = _SMSRequested.Key == ESMSRequested.Yes;
            }
        }
    }

    

}
