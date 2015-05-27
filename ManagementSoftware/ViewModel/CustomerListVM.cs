using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Command;
using Common.DataTransferObjects;
using Common.Commands;
using Common.Communication;
using ManagementSoftware.Helper;


namespace ManagementSoftware.ViewModel
{
    public class CustomerListVM : ViewModelBase
    {
        public static readonly string LOAD_SUCCESS = "Daten abgerufen";
        public static readonly string LOAD_FAILURE = "Fehler beim versenden der Anfrage zum Laden der Kundenliste. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut";


        private IClientConnection _ClientConnection;
        private IMessageBox _MessageBox;
        private IList<Customer> _DataList;

        public CustomerListVM(
            IClientConnection connection,
            IMessageBox _MessageBox)
        {
            this._MessageBox = _MessageBox;
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
                _MessageBox.Show(LOAD_FAILURE);
            }
            else
            {
                DataList = null;
                DataList = response.Customers;

                _MessageBox.Show(LOAD_SUCCESS);
            }
        }

        public RelayCommand LoadCommand { get; set; }
    }
}
