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
            int day = 0; //TODO: IMPL DAY CHANGE

            ObservableCollection<ShiftScheduleDayEntry> list = new ObservableCollection<ShiftScheduleDayEntry>();
            ShiftSchedule rawData = ShiftScheduleRawModel.CurrentData;

            if (rawData != null)
            {
                if ( day < 0 ) {
                    day = 0;
                } else if ( day >= rawData.DayEntry.Count ) {
                    day = rawData.DayEntry.Count - 1;
                }
                
                DayEntry    entry = rawData.DayEntry[day];
              
                AddEntry(entry.AM, list, entry.Date, true);
                AddEntry(entry.PM, list, entry.Date, false);
            }

            ListCollectionView listView = new ListCollectionView(list);
            listView.GroupDescriptions.Add(new PropertyGroupDescription("EmployeeAM", new EmployeeTypeConverter()));
            DataList = null;
            DataList = listView;
        }

        private void AddEntry(IList<Employee> emps, IList<ShiftScheduleDayEntry> list, DateTime dateTime, bool amShift)
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

        public string DayText
        {
            get
            {
                //TODO: RETURN DAY TEXT + BINDING
                return "";
            }
        }

        private ShiftScheduleRawModel ShiftScheduleRawModel { get; set; }

        public RelayCommand SwitchToMonthCommand { get; private set; }
    }
}
