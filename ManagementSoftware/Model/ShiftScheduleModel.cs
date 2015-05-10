using Common.Communication.Client;
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
            //TODO: GET SHIFT_SHEDULES FROM SERVER USE _Connection

            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, 1); //zum testen ändere Monat und Jahr
            DateTime next = now.AddMonths(1);

            ShiftSchedule[] _Data = new ShiftSchedule[2];

            _Data[0] = Util.CreateTestData(now);
            _Data[1] = Util.CreateTestData(next);

            ShiftScheduleRawModel.Data = _Data;

            MessageBox.Show("Daten abgerufen");
        }

        public void SwitchMonthData()
        {
            int index = ShiftScheduleRawModel.CurrentDataIndex;
            index = (index + 1) % 2;
            ShiftScheduleRawModel.CurrentDataIndex = index;
        }
    }
}
