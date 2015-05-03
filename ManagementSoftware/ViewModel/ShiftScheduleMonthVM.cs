using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleMonthVM : ViewModelBase
    {
        private ShiftScheduleRawModel _ShiftScheduleRawModel;
        private RelayCommand _SwitchToDayCommand;

        public ShiftScheduleMonthVM(ShiftScheduleRawModel _ShiftScheduleRawModel, RelayCommand _SwitchToDayCommand)
        {
            this._ShiftScheduleRawModel = _ShiftScheduleRawModel;
            this._SwitchToDayCommand = _SwitchToDayCommand;

            _ShiftScheduleRawModel.Change += RawModelChanged;
        }

        private void RawModelChanged(object sender, ShiftScheduleRawModelChangeEventArgs e)
        {
            //TODO
        }

        public RelayCommand SwitchToDayCommand
        {
            get
            {
                return _SwitchToDayCommand;
            }
        }

    }
}
