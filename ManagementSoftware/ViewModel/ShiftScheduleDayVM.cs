using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleDayVM : ViewModelBase
    {
        private ShiftScheduleRawModel _ShiftScheduleRawModel;
        private RelayCommand _SwitchToMonthCommand;

        public ShiftScheduleDayVM(ShiftScheduleRawModel _ShiftScheduleRawModel, RelayCommand _SwitchToMonthCommand)
        {
            this._ShiftScheduleRawModel = _ShiftScheduleRawModel;
            this._SwitchToMonthCommand = _SwitchToMonthCommand;

            _ShiftScheduleRawModel.Change += RawModelChanged;
        }

        private void RawModelChanged(object sender, ShiftScheduleRawModelChangeEventArgs e)
        {
            //TODO
        }

        public RelayCommand SwitchToMonthCommand
        {
            get
            {
                return _SwitchToMonthCommand;
            }
        }
    }
}
