using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //TODO: Fehlendes Attribut CompletedOrders?
        public int OrdersCompleted
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
