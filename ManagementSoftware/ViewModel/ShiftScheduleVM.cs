using Common.Communication.Client;
using Common.DataTransferObjects;
using Common.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleVM : ViewModelBase
    {

        private static readonly CultureInfo GERMAN_CULTURE_INFO = new CultureInfo("de-DE");
        private static readonly string NO_DATA_AVAILABLE = "Keine Daten geladen/vorhanden";

        private ViewModelBase _CurrentViewModel;
        private string _MonthText;

        private ShiftScheduleMonthVM _ShiftScheduleMonthVM;
        private ShiftScheduleDayVM _ShiftScheduleDayVM;

        private IClientConnection _Connection;
        private ShiftScheduleRawModel _ShiftScheduleRawModel;
        private RelayCommand _LoadDataCommand;

        public ShiftScheduleVM(IClientConnection _Connection)
        {
            this._Connection = _Connection;

            this._ShiftScheduleRawModel = new ShiftScheduleRawModel();

            RelayCommand _SwitchToMonthCommand = new RelayCommand(() => SwitchToShiftScheduleMonthVM());
            RelayCommand _SwitchToDayCommand = new RelayCommand(() => SwitchToShiftScheduleDayVM());
            
            this._LoadDataCommand = new RelayCommand(() => LoadRawModel());

            this._ShiftScheduleMonthVM = new ShiftScheduleMonthVM(_ShiftScheduleRawModel, _SwitchToDayCommand);
            this._ShiftScheduleDayVM = new ShiftScheduleDayVM(_ShiftScheduleRawModel, _SwitchToMonthCommand);


            this._ShiftScheduleRawModel.Change += RawModelChanged;
            this._CurrentViewModel = _ShiftScheduleMonthVM;
            this._MonthText = NO_DATA_AVAILABLE;
        }

        private void RawModelChanged(object sender, EventArgs e)
        {
            
            ShiftSchedule data = _ShiftScheduleRawModel.CurrentData;
            if (data == null)
            {
                MonthText = NO_DATA_AVAILABLE;
            }
            else
            {
                DateTime date = data.DayEntry[0].Date;
                MonthText = date.ToString("y", GERMAN_CULTURE_INFO);
            }
        }

        public ShiftScheduleMonthVM ShiftScheduleMonthVM
        {
            get
            {
                return _ShiftScheduleMonthVM;
            }
        }

        public ShiftScheduleDayVM ShiftScheduleDayVM
        {
            get
            {
                return _ShiftScheduleDayVM;
            }
        }

        private void SwitchToShiftScheduleMonthVM()
        {
            CurrentViewModel = _ShiftScheduleMonthVM;
        }

        private void SwitchToShiftScheduleDayVM()
        {
            CurrentViewModel = _ShiftScheduleDayVM;
        }

        public RelayCommand LoadRawModelCommand
        {
            get
            {
                return _LoadDataCommand;
            }
        }

        private void LoadRawModel()
        {
            //TODO _Connection ...

            DateTime now = DateTime.Now;
            int month = now.Month + 1;
            int year = now.Year;
            if (month > 12)
            {
                month = 1;
                year++;
            }
            DateTime next = new DateTime(year, month, 1);

            ShiftSchedule[] _Data = new ShiftSchedule[2];

            _Data[0] = ShiftScheduleRawModel.Create(now);
            _Data[1] = ShiftScheduleRawModel.Create(next);            

            _ShiftScheduleRawModel.Data = _Data;

            MessageBox.Show("Daten abgerufen");
        }

        public string MonthText
        {
            get
            {
                return _MonthText;
            }
            set
            {
                _MonthText = value;
                RaisePropertyChanged();
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _CurrentViewModel;
            }
            set
            {
                _CurrentViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}
