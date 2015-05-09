using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Common.Util;
using System;
using System.ComponentModel;
using Common.DataTransferObjects;
using System.Collections.Generic;
using Common.Communication.Client;
using Common.Commands;
using System.Windows;

namespace ManagementSoftware.ViewModel
{
    public class RegisterCustomerVM : ViewModelBase
    {
        private static Dictionary<ETitle, string> _ETitleValues = Util.EnumValues<ETitle>();
        private static Dictionary<ESMSRequested, string> _ESMSRequestedValues = Util.EnumValues<ESMSRequested>();

        private Customer _Customer;
        private KeyValuePair<ETitle, string> _Title;
        private KeyValuePair<ESMSRequested, string> _SMSRequested;
        private RelayCommand _RegisterCustomerAction;
        private IClientConnection _ClientConnection;

        public RegisterCustomerVM(IClientConnection _ClientConnection)
        {
            this._ClientConnection = _ClientConnection;

            _Customer = new Customer();
            _Customer.BankAccount = new BankAccount();
            _Customer.Address = new Address();

            _RegisterCustomerAction = new RelayCommand(RegisterCustomer);
            Title = Util.CreateValuePair<ETitle>(ETitle.Mr);
            SMSRequested = Util.CreateValuePair<ESMSRequested>(ESMSRequested.No);
        }

        public RelayCommand RegisterCustomerAction
        {
            get
            {
                return _RegisterCustomerAction;
            }
        }

        public object ESMSRequestedValues
        {
            get
            {
                return _ESMSRequestedValues;
            }
        }

        public object ETitleValues
        {
            get
            {
                return _ETitleValues;
            }
        }

        public void RegisterCustomer()
        {
            Customer copyOfCustomer = new Customer();
            copyOfCustomer.Address = new Address();
            copyOfCustomer.BankAccount = new BankAccount();

            copyOfCustomer.LastName = _Customer.LastName;
            copyOfCustomer.FirstName = _Customer.FirstName;
            copyOfCustomer.Label = _Customer.Label;
            copyOfCustomer.Title = _Customer.Title;
            copyOfCustomer.UserName = _Customer.UserName;
            copyOfCustomer.Password = _Customer.Password;
            copyOfCustomer.MobileNumber = _Customer.MobileNumber;
            copyOfCustomer.SMSRequested = _Customer.SMSRequested;
            copyOfCustomer.Address.Street = _Customer.Address.Street;
            copyOfCustomer.Address.City = _Customer.Address.City;
            copyOfCustomer.Address.PostalCode = _Customer.Address.PostalCode;
            copyOfCustomer.BankAccount.AccountOwner = _Customer.BankAccount.AccountOwner;
            copyOfCustomer.BankAccount.IBAN = _Customer.BankAccount.IBAN;

            CmdRegisterCustomer request = new CmdRegisterCustomer(copyOfCustomer);
            CmdReturnRegisterCustomer response = _ClientConnection.SendWait<CmdReturnRegisterCustomer>(request);
            if (response == null)
            {
                MessageBox.Show("Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
            }
            else if ( response.Success )
            {
                MessageBox.Show(response.Message != null ? response.Message : "Kunde wurde angelegt");
            }
            else if (!response.Success && response.Message == null)
            {
                throw new Exception("Success is false and Error null. Illegal State");
            }
            else
            {
                MessageBox.Show(response.Message);
            }

        }

        public string LastName
        {
            get
            {
                return _Customer.LastName;
            }
            set
            {
                _Customer.LastName = value;
                RaisePropertyChanged();
            }
        }

        public string FirstName
        {
            get
            {
                return _Customer.FirstName;
            }
            set
            {
                _Customer.FirstName = value;
                RaisePropertyChanged();
            }
        }

        public string Label
        {
            get
            {
                return _Customer.Label;
            }
            set
            {
                _Customer.Label = value;
                RaisePropertyChanged();
            }
        }

        public KeyValuePair<ETitle, string> Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                _Customer.Title = _Title.Value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get
            {
                return _Customer.UserName;
            }
            set
            {
                _Customer.UserName = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return _Customer.Password;
            }
            set
            {
                _Customer.Password = value;
                RaisePropertyChanged();
            }
        }

        public string MobileNumber
        {
            get
            {
                return _Customer.MobileNumber;
            }
            set
            {
                _Customer.MobileNumber = value;
                RaisePropertyChanged();
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
                _Customer.SMSRequested = (_SMSRequested.Key == ESMSRequested.Yes);
                RaisePropertyChanged();
            }
        }

        public string BankAccountOwner
        {
            get
            {
                return _Customer.BankAccount.AccountOwner;
            }
            set
            {
                _Customer.BankAccount.AccountOwner = value;
                RaisePropertyChanged();
            }
        }

        public string IBAN
        {
            get
            {
                return _Customer.BankAccount.IBAN;
            }
            set
            {
                _Customer.BankAccount.IBAN = value;
                RaisePropertyChanged();
            }
        }

        public string City
        {
            get
            {
                return _Customer.Address.City;
            }
            set
            {
                _Customer.Address.City = value;
                RaisePropertyChanged();
            }
        }

        public string PostalCode
        {
            get
            {
                return _Customer.Address.PostalCode;
            }
            set
            {
                _Customer.Address.PostalCode = value;
                RaisePropertyChanged();
            }
        }

        public string Street
        {
            get
            {
                return _Customer.Address.Street;
            }
            set
            {
                _Customer.Address.Street = value;
                RaisePropertyChanged();
            }
        }

    }

    public enum ESMSRequested
    {
        [Description("Ja")]
        Yes,
        [Description("Nein")]
        No
    }

    public enum ETitle
    {
        [Description("Herr")]
        Mr,
        [Description("Frau")]
        Mrs
    }
}
