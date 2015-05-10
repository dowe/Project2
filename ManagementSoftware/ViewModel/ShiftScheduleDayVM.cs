using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Helper;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleDayVM : ViewModelBase
    {
        private static readonly double OPACITY_VISIBLE = 1.0d;
        private static readonly double OPACITY_HIDDEN = 0.1d;

        private int _DayEntryIndex = 0;
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
            LoadDataForDayEntry(DayEntryIndex);
        }

        private void LoadDataForDayEntry(int index)
        {

            ObservableCollection<ShiftScheduleDayEntry> list = new ObservableCollection<ShiftScheduleDayEntry>();
            ShiftSchedule rawData = ShiftScheduleRawModel.CurrentData;

            if (rawData != null)
            {
                if (index < 0)
                {
                    index = 0;
                }
                else if (index >= rawData.DayEntry.Count)
                {
                    index = rawData.DayEntry.Count - 1;
                }

                DayEntry entry = rawData.DayEntry[index];

                AddEntry(entry.AM, list, true);
                AddEntry(entry.PM, list, false);
            }

            ListCollectionView listView = new ListCollectionView(list);
            listView.GroupDescriptions.Add(new PropertyGroupDescription("EmployeeAM", new EmployeeTypeConverter()));
            DataList = null;
            DataList = listView;
            DayEntryIndex = index;
        }

        private void AddEntry(IList<Employee> emps, IList<ShiftScheduleDayEntry> list, bool amShift)
        {
            foreach (Employee emp in emps)
            {
                ShiftScheduleDayEntry entry = GetEntry(emp, list, amShift);
                if (amShift)
                {
                    entry.EmployeeAM = emp;
                }
                else
                {
                    entry.EmployeePM = emp;
                }
            }
        }

        private ShiftScheduleDayEntry GetEntry(Employee emp, IList<ShiftScheduleDayEntry> list, bool amShift)
        {
            foreach (ShiftScheduleDayEntry entry in list)
            {
                if (!amShift && entry.EmployeePM == null
                    && entry.EmployeeAM != null
                    && entry.EmployeeAM.GetType().IsAssignableFrom(emp.GetType()))
                {
                    return entry;
                }
                if (amShift && entry.EmployeeAM == null
                    && entry.EmployeePM != null
                    && entry.EmployeePM.GetType().IsAssignableFrom(emp.GetType()))
                {
                    return entry;
                }
            }
            {
                ShiftScheduleDayEntry entry = new ShiftScheduleDayEntry();
                list.Add(entry);
                return entry;
            }
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

        private int DayEntryIndex
        {
            get
            {
                return _DayEntryIndex;
            }
            set
            {
                _DayEntryIndex = value;
                RaisePropertyChanged(() => DayText);
                RaisePropertyChanged(() => NextDayButtonOpacity);
                RaisePropertyChanged(() => PreviousDayButtonOpacity);
            }
        }

        public string DayText
        {
            get
            {
                ShiftSchedule rawData = ShiftScheduleRawModel.CurrentData;
                if (rawData == null)
                {
                    return "Keine Daten";
                }
                return "Tag " + rawData.DayEntry[DayEntryIndex].Date.Day;
            }
        }

        public double NextDayButtonOpacity
        {
            get
            {
                ShiftSchedule rawData = ShiftScheduleRawModel.CurrentData;
                if (rawData == null)
                {
                    return OPACITY_HIDDEN;
                }
                if (DayEntryIndex + 1 < rawData.DayEntry.Count)
                {
                    return OPACITY_VISIBLE;
                }
                return OPACITY_HIDDEN;
            }
        }

        public double PreviousDayButtonOpacity
        {
            get
            {
                ShiftSchedule rawData = ShiftScheduleRawModel.CurrentData;
                if (rawData == null)
                {
                    return OPACITY_HIDDEN;
                }
                if (DayEntryIndex - 1 >= 0)
                {
                    return OPACITY_VISIBLE;
                }
                return OPACITY_HIDDEN;
            }
        }

        public void NextDay()
        {
            LoadDataForDayEntry(DayEntryIndex + 1);
        }

        public void PreviousDay()
        {
            LoadDataForDayEntry(DayEntryIndex - 1);
        }

        private ShiftScheduleRawModel ShiftScheduleRawModel { get; set; }

        public RelayCommand SwitchToMonthCommand { get; private set; }


    }
}
