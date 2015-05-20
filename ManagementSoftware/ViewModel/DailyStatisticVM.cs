using Common.Commands;
using Common.Communication;
using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSoftware.ViewModel
{
    public class DailyStatisticVM : ViewModelBase
    {
        private IClientConnection _ClientConnection;
        private DailyStatistic _DailyStatistic;

         public DailyStatisticVM(IClientConnection _ClientConnection)
        {
            this._ClientConnection = _ClientConnection;
            _DailyStatistic = new DailyStatistic();


            this.LoadCommand = new RelayCommand(LoadData);
            LoadData();
        }

         private void LoadData()
         {
             Command request = new CmdGetDailyStatistic();
             CmdReturnGetDailyStatistic response = _ClientConnection.SendWait<CmdReturnGetDailyStatistic>(request);
             if (response == null)
             {
                 MessageBox.Show("Fehler beim aktualisieren der Tagesstatistik. \n - Überprüfen Sie ihre Internetverbindung\n - Wenden Sie sich an den Kundendienst");
             }
             else
             {
                 _DailyStatistic = response.DailyStatistic;
                 
                
             }
         }

         public RelayCommand LoadCommand { get; set; }


        public String TimeSpan
         {
             get
             {
                 if (_DailyStatistic.Date == default(DateTime))
                     return "Fehler";
                 else
                     return _DailyStatistic.Date.AddDays(-1).ToString("dd-MM-yyyy") + " bis " + _DailyStatistic.Date.ToString("dd-MM-yyyy") ;
             }
             set
             {
                 TimeSpan = _DailyStatistic.Date.AddDays(-1).ToString("dd-MM-yyyy") + " bis " + _DailyStatistic.Date.ToString("dd-MM-yyyy");
                 RaisePropertyChanged();

             }
            
         }

        public int NewOrders 
        {
            get
            {
                return _DailyStatistic.NumberOfNewOrders;
            }
            set
            {
                _DailyStatistic.NumberOfNewOrders = value;
                RaisePropertyChanged();
            }
        }

        public int CompletedOrders
        {
            get
            {
                return _DailyStatistic.NumberOfCompletedOrders;
            }
            set
            {
                _DailyStatistic.NumberOfCompletedOrders = value;
                RaisePropertyChanged();
            }
        }

        public int OrdersInProgress
        {
            get
            {
                return _DailyStatistic.NumberOfOrdersInProgress;
            }
            set
            {
                _DailyStatistic.NumberOfOrdersInProgress = value;
                RaisePropertyChanged();
            }
        }
        public int TestsCompleted
        {
            get
            {
                return _DailyStatistic.NumberOfCompletedTests;
            }
            set
            {
                _DailyStatistic.NumberOfCompletedTests = value;
                RaisePropertyChanged();
            }
        }
        public int TestsInProgress
        {
            get
            {
                return _DailyStatistic.NumberOfTestsInProgress;
            }
            set
            {
                _DailyStatistic.NumberOfTestsInProgress = value;
                RaisePropertyChanged();
            }
        }
    }
}
