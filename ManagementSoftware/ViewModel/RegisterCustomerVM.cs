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
using ManagementSoftware.Model;
using ManagementSoftware.Helper;

namespace ManagementSoftware.ViewModel
{
    public class RegisterCustomerVM : ViewModelBase
    {
        private RegisterCustomerM model;
        private IMessageBox _MessageBox;

        public RegisterCustomerVM(
            IClientConnection _ClientConnection, IMessageBox _MessageBox)
        {
            this._MessageBox = _MessageBox;
            this.model = new RegisterCustomerM(_ClientConnection);

            RegisterCustomerAction = new RelayCommand(RegisterCustomer);
        }

        public RelayCommand RegisterCustomerAction { get; set; }

        public Dictionary<ESMSRequested, string> ESMSRequestedValues
        {
            get
            {
                return model.ESMSRequestedValues;
            }
        }

        public Dictionary<ETitle, string> ETitleValues
        {
            get
            {
                return model.ETitleValues;
            }
        }

        public void RegisterCustomer()
        {
            _MessageBox.Show(model.RegisterCustomer());
        }

        public string LastName
        {
            get
            {
                return model.Customer.LastName;
            }
            set
            {
                model.Customer.LastName = value;
                RaisePropertyChanged();
            }
        }

        public string FirstName
        {
            get
            {
                return model.Customer.FirstName;
            }
            set
            {
                model.Customer.FirstName = value;
                RaisePropertyChanged();
            }
        }

        public string Label
        {
            get
            {
                return model.Customer.Label;
            }
            set
            {
                model.Customer.Label = value;
                RaisePropertyChanged();
            }
        }

        public KeyValuePair<ETitle, string> Title
        {
            get
            {
                return model.Title;
            }
            set
            {
                model.Title = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get
            {
                return model.Customer.UserName;
            }
            set
            {
                model.Customer.UserName = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return model.Customer.Password;
            }
            set
            {
                model.Customer.Password = value;
                RaisePropertyChanged();
            }
        }

        public string MobileNumber
        {
            get
            {
                return model.Customer.MobileNumber;
            }
            set
            {
                model.Customer.MobileNumber = value;
                RaisePropertyChanged();
            }
        }

        public KeyValuePair<ESMSRequested, string> SMSRequested
        {
            get
            {
                return model.SMSRequested;
            }
            set
            {
                model.SMSRequested = value;
                RaisePropertyChanged();
            }
        }

        public string BankAccountOwner
        {
            get
            {
                return model.Customer.BankAccount.AccountOwner;
            }
            set
            {
                model.Customer.BankAccount.AccountOwner = value;
                RaisePropertyChanged();
            }
        }

        public string IBAN
        {
            get
            {
                return model.Customer.BankAccount.IBAN;
            }
            set
            {
                model.Customer.BankAccount.IBAN = value;
                RaisePropertyChanged();
            }
        }

        public string City
        {
            get
            {
                return model.Customer.Address.City;
            }
            set
            {
                model.Customer.Address.City = value;
                RaisePropertyChanged();
            }
        }

        public string PostalCode
        {
            get
            {
                return model.Customer.Address.PostalCode;
            }
            set
            {
                model.Customer.Address.PostalCode = value;
                RaisePropertyChanged();
            }
        }

        public string Street
        {
            get
            {
                return model.Customer.Address.Street;
            }
            set
            {
                model.Customer.Address.Street = value;
                RaisePropertyChanged();
            }
        }

    }


}
