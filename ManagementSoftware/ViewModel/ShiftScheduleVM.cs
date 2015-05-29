using Common.Communication.Client;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Helper;
using ManagementSoftware.Model;
using System;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleVM : ViewModelBase, ISwitchShiftScheduleView
    {
        private ShiftScheduleModel _ShiftScheduleModel;
        
        public ShiftScheduleVM(
            IClientConnection _Connection,
            IMessageBox _MessageBox)
        {
            this._ShiftScheduleModel = new ShiftScheduleModel(this, _Connection, _MessageBox);

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
            CurrentViewModel = ShiftScheduleMonthVM;
        }

        public void SwitchToShiftScheduleDayVM()
        {
            CurrentViewModel = ShiftScheduleDayVM;
        }
    }
}
