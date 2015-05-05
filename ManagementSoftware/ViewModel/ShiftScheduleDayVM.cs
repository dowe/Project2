using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleDayVM : ViewModelBase
    {
        private ListCollectionView _DataList;

        public ShiftScheduleDayVM(
            ShiftScheduleRawModel _ShiftScheduleRawModel,
            ISwitchShiftScheduleView _ISwitchShiftScheduleView)
        {
            this.ShiftScheduleRawModel = _ShiftScheduleRawModel;
            this.SwitchToMonthCommand = new RelayCommand(_ISwitchShiftScheduleView.SwitchToShiftScheduleMonthVM);

            _ShiftScheduleRawModel.Change += RawModelChanged;
        }

        private void RawModelChanged(object sender, EventArgs e)
        {
            //TODO
        }

        public ListCollectionView DataList
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

        private ShiftScheduleRawModel ShiftScheduleRawModel { get; set; }

        public RelayCommand SwitchToMonthCommand { get; private set; }
    }
}
