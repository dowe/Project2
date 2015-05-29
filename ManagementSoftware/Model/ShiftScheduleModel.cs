using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using ManagementSoftware.ViewModel;
using System;
using System.Globalization;
using ManagementSoftware.Helper;

namespace ManagementSoftware.Model
{
    public class ShiftScheduleModel
    {
        public static readonly string SWITCH_TO_NEXT_MONTH_TEXT = "Zum nächsten Monat";
        public static readonly string SWITCH_TO_CURRENT_MONTH_TEXT = "Zum aktuellen Monat";
        public static readonly string CURRENT_MONTH_TEXT_NO_DATA = "Keine Daten geladen/vorhanden";

        public static readonly CultureInfo GERMAN_CULTURE_INFO = new CultureInfo("de-DE");

        private IClientConnection _Connection;
        private IMessageBox _MessageBox;

        public ShiftScheduleModel(
            ISwitchShiftScheduleView _ISwitchShiftScheduleView,
            IClientConnection _Connection,
            IMessageBox _MessageBox)
        {
            this._MessageBox = _MessageBox;
            this._Connection = _Connection;
            this.ShiftScheduleRawModel = new ShiftScheduleRawModel(); ;

            this.ShiftScheduleMonthVM = new ShiftScheduleMonthVM(ShiftScheduleRawModel, _ISwitchShiftScheduleView);
            this.ShiftScheduleDayVM = new ShiftScheduleDayVM(ShiftScheduleRawModel, _ISwitchShiftScheduleView);

            this.CurrentViewModel = ShiftScheduleMonthVM;
        }

        public ShiftScheduleRawModel ShiftScheduleRawModel { get; private set; }

        public string CurrentMonthText
        {
            get
            {
                ShiftSchedule data = ShiftScheduleRawModel.CurrentData;
                if (data == null)
                {
                    return CURRENT_MONTH_TEXT_NO_DATA;
                }

                DateTime date = data.Date;
                return date.ToString("y", GERMAN_CULTURE_INFO);
            }

        }

        public string SwitchMonthButtonText
        {
            get
            {
                if (ShiftScheduleRawModel.CurrentDataIndex == 0)
                {
                    return SWITCH_TO_NEXT_MONTH_TEXT;
                }
                else
                {
                    return SWITCH_TO_CURRENT_MONTH_TEXT;
                }
            }
        }


        public ViewModelBase CurrentViewModel { get; set; }
        public ShiftScheduleMonthVM ShiftScheduleMonthVM { get; private set; }
        public ShiftScheduleDayVM ShiftScheduleDayVM { get; private set; }

        public void LoadRawModel()
        {
            CmdGetShiftSchedules request = new CmdGetShiftSchedules();
            CmdReturnGetShiftSchedule response = _Connection.SendWait<CmdReturnGetShiftSchedule>(request);

            if (response == null)
            {
                _MessageBox.Show("Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
            }
            else
            {
                ShiftSchedule[] _Data = new ShiftSchedule[2];

                _Data[0] = response.Schedules[0];
                _Data[1] = response.Schedules[1];

                ShiftScheduleRawModel.Data = _Data;

                _MessageBox.Show("Daten abgerufen");
            }
        }

        public void SwitchMonthData()
        {
            int index = ShiftScheduleRawModel.CurrentDataIndex;
            index = (index + 1) % 2;
            ShiftScheduleRawModel.CurrentDataIndex = index;
        }
    }
}
