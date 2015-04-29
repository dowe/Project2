using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ManagementSoftware.ViewModel.Util;
using System;
using System.ComponentModel;
using Common.DataTransferObjects;
using System.Collections.Generic;

namespace ManagementSoftware.ViewModel
{
    public class RegisterCustomerVM : ViewModelBase
    {
        private static Dictionary<ETitle, string> _ETitleValues = ViewModelUtil.EnumValues<ETitle>();
        private static Dictionary<ESMSRequested, string> _ESMSRequestedValues = ViewModelUtil.EnumValues<ESMSRequested>();

        private Customer _Customer;
        private KeyValuePair<ETitle, string> _Title = ViewModelUtil.CreateValuePair<ETitle>(ETitle.Mr);
        private KeyValuePair<ESMSRequested, string> _SMSRequested = ViewModelUtil.CreateValuePair<ESMSRequested>(ESMSRequested.No);

        public RegisterCustomerVM()
        {
            _Customer = new Customer();
            _Customer.BankAccount = new BankAccount();
            _Customer.Address = new Address();
        }

        public RelayCommand CreateAction
        {
            get
            {
                return new RelayCommand(Create);
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

        public void Create()
        {
            //TODO
            //Customer copy = ViewModelUtil.DeepClone<Customer>(_Customer); ?? [Serializable]
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

        public KeyValuePair<ESMSRequested,string> SMSRequested
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
