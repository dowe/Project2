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
    public class ShiftScheduleVM : ViewModelBase, ISwitchShiftScheduleView
    {
        private ShiftScheduleModel _ShiftScheduleModel;
        
        public ShiftScheduleVM(IClientConnection _Connection)
        {
            this._ShiftScheduleModel = new ShiftScheduleModel(this, _Connection);

            this.LoadRawModelCommand = new RelayCommand(() => LoadRawModel());
            this.SwitchMonthDataCommand = new RelayCommand(() => SwitchMonthData());

            this._ShiftScheduleModel.ShiftScheduleRawModel.Change += RawModelChanged;
        }

        private void RawModelChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => CurrentMonthText);
            RaisePropertyChanged(() => SwitchMonthButtonText);
        }

        public ShiftScheduleMonthVM ShiftScheduleMonthVM
        {
            get
            {
                return _ShiftScheduleModel.ShiftScheduleMonthVM;
            }
        }

        public ShiftScheduleDayVM ShiftScheduleDayVM
        {
            get
            {
                return _ShiftScheduleModel.ShiftScheduleDayVM;
            }
        }

        public RelayCommand LoadRawModelCommand { get; private set; }
        public RelayCommand SwitchMonthDataCommand { get; private set; }

        private void LoadRawModel()
        {
            _ShiftScheduleModel.LoadRawModel();
        }

        private void SwitchMonthData()
        {
            _ShiftScheduleModel.SwitchMonthData();
        }

        public string CurrentMonthText
        {
            get
            {
                return _ShiftScheduleModel.CurrentMonthText;
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _ShiftScheduleModel.CurrentViewModel;
            }
            set
            {
                _ShiftScheduleModel.CurrentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public string SwitchMonthButtonText
        {
            get
            {
                return _ShiftScheduleModel.SwitchMonthButtonText;
            }
        }

        public void SwitchToShiftScheduleMonthVM()
        {
            CurrentViewModel = _ShiftScheduleModel.ShiftScheduleMonthVM;
        }

        public void SwitchToShiftScheduleDayVM()
        {
            CurrentViewModel = _ShiftScheduleModel.ShiftScheduleDayVM;
        }
    }
}
