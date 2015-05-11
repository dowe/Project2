using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;
using Common.Util;
using GalaSoft.MvvmLight;
using ManagementSoftware.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSoftware.Model
{
    public class ShiftScheduleModel
    {
        private static readonly CultureInfo GERMAN_CULTURE_INFO = new CultureInfo("de-DE");
        private IClientConnection _Connection;

        public ShiftScheduleModel(
            ISwitchShiftScheduleView _ISwitchShiftScheduleView,
            IClientConnection _Connection)
        {
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
                    return "Keine Daten geladen/vorhanden";
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
                    return "Zum nächsten Monat";
                }
                else
                {
                    return "Zum aktuellen Monat";
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
                MessageBox.Show("Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
            }
            else
            {
                ShiftSchedule[] _Data = new ShiftSchedule[2];

                _Data[0] = response.Schedules[0];
                _Data[1] = response.Schedules[1];

                ShiftScheduleRawModel.Data = _Data;

                MessageBox.Show("Daten abgerufen");
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
