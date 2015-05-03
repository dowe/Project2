using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleVM : ViewModelBase
    {
        private ViewModelBase _CurrentViewModel;

        private ShiftScheduleMonthVM _ShiftScheduleMonthVM;
        private ShiftScheduleDayVM _ShiftScheduleDayVM;

        public ShiftScheduleVM()
        {
            ShiftScheduleRawModel _ShiftScheduleRawModel = new ShiftScheduleRawModel();

            RelayCommand _SwitchToMonthCommand = new RelayCommand(() => SwitchToShiftScheduleMonthVM());
            RelayCommand _SwitchToDayCommand = new RelayCommand(() => SwitchToShiftScheduleDayVM());

            _ShiftScheduleMonthVM = new ShiftScheduleMonthVM(_ShiftScheduleRawModel, _SwitchToDayCommand);
            _ShiftScheduleDayVM = new ShiftScheduleDayVM(_ShiftScheduleRawModel, _SwitchToMonthCommand);

            _CurrentViewModel = _ShiftScheduleMonthVM;
        }

        public ViewModelBase ShiftScheduleMonthVM
        {
            get
            {
                return _ShiftScheduleMonthVM;
            }
        }

        public ViewModelBase ShiftScheduleDayVM
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
