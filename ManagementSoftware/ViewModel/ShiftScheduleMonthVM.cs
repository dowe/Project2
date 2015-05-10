using Common.DataTransferObjects;
using Common.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementSoftware.Helper;
using ManagementSoftware.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace ManagementSoftware.ViewModel
{
    public class ShiftScheduleMonthVM : ViewModelBase
    {
        private ListCollectionView _DataList;
        private int _NumberOfDaysInMonth;

        public ShiftScheduleMonthVM(
            ShiftScheduleRawModel _ShiftScheduleRawModel,
            ISwitchShiftScheduleView _ISwitchShiftScheduleView)
        {
            this.ShiftScheduleRawModel = _ShiftScheduleRawModel;
            this.SwitchToDayCommand = new RelayCommand(_ISwitchShiftScheduleView.SwitchToShiftScheduleDayVM);
            this._DataList = null;
            this._NumberOfDaysInMonth = 0;

            _ShiftScheduleRawModel.Change += RawModelChanged;

            
        }
        
        private void RawModelChanged(object sender, EventArgs e)
        {
            ObservableCollection<ShiftScheduleMonthEntry> list = new ObservableCollection<ShiftScheduleMonthEntry>();
            ShiftSchedule rawData = ShiftScheduleRawModel.CurrentData;
            int numberOfDaysInMonth = 0;
            if (rawData != null)
            {
                
                numberOfDaysInMonth = rawData.DayEntry.Count;
                for (int i = 0; i < numberOfDaysInMonth ; i++)
                {
                    DayEntry entry = rawData.DayEntry[i];

                    AddEntry(entry.AM, ShiftScheduleMonthEntry.AM_SHIFT, list, entry.Date, numberOfDaysInMonth);
                    AddEntry(entry.PM, ShiftScheduleMonthEntry.PM_SHIFT, list, entry.Date, numberOfDaysInMonth);
                }
                
            }
            ListCollectionView listView = new ListCollectionView(list);
            listView.GroupDescriptions.Add(new PropertyGroupDescription("Employee", new EmployeeTypeConverter()));
            DataList = null;
            DataList = listView;
            NumberOfDaysInMonth = numberOfDaysInMonth;
        }

        private void AddEntry(IList<Employee> emps, string shift, IList<ShiftScheduleMonthEntry> list, DateTime date, int numberOfDaysInMonth)
        {
            foreach (Employee emp in emps)
            {
                ShiftScheduleMonthEntry entry = GetEntry(emp, list, numberOfDaysInMonth);
                entry.Days[date.Day - 1] = shift;
            }
        }

        private ShiftScheduleMonthEntry GetEntry(Employee emp, IList<ShiftScheduleMonthEntry> list, int numberOfDaysInMonth)
        {
            foreach (ShiftScheduleMonthEntry entry in list)
            {
                if (entry.Employee.FirstName.Equals(emp.FirstName)
                    && entry.Employee.LastName.Equals(emp.LastName))
                {
                    return entry;
                }
            }
            {
                ShiftScheduleMonthEntry entry = new ShiftScheduleMonthEntry(emp, numberOfDaysInMonth);
                list.Add(entry);
                return entry;
            }
        }

        public int NumberOfDaysInMonth
        {
            set
            {
                _NumberOfDaysInMonth = value;
                RaisePropertyChanged();
            }
            get
            {
                return _NumberOfDaysInMonth;
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

        public RelayCommand SwitchToDayCommand { get; private set; }
        private ShiftScheduleRawModel ShiftScheduleRawModel { get; set; }

    }
}
