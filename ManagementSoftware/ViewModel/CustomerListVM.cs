using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using Common.Commands;
using Common.Communication;
using System.Windows;
using Common.Util;

namespace ManagementSoftware.ViewModel
{
    public class CustomerListVM : ViewModelBase
    {
        private IClientConnection _ClientConnection;
        private IList<Customer> _DataList;

        public CustomerListVM(IClientConnection connection)
        {
            this._ClientConnection = connection;

            this.LoadCommand = new RelayCommand(LoadData);
        }

        public IList<Customer> DataList
        {
            get
            {
                return _DataList;
            }
            set
            {
                _DataList = value;
                RaisePropertyChanged();
            }
        }

        private void LoadData()
        {
            Command request = new CmdGetAllCustomers();
            CmdReturnGetAllCustomers response = _ClientConnection.SendWait<CmdReturnGetAllCustomers>(request);
            if (response == null)
            {
                MessageBox.Show("Fehler beim versenden der Anfrage zum Laden der Kundenliste. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
            }
            else
            {
                DataList = null;
                DataList = response.Customers;

                MessageBox.Show("Daten abgerufen");
            }
        }

        public RelayCommand LoadCommand { get; set; }
    }
}
