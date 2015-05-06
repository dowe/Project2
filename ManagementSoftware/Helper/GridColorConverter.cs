using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using ManagementSoftware.Model;
using System.Windows.Media;
using System.Windows;

namespace ManagementSoftware.Helper
{
    public class GridColorConverter : IMultiValueConverter
    {

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = "";

            DataGridCell dgc = (DataGridCell)values[0];
            ShiftScheduleMonthEntry rowView = (ShiftScheduleMonthEntry)(dgc.DataContext);
            int index = dgc.Column.DisplayIndex - 1;

            if (index >= 0 && index < rowView.Days.Count)
            {
                input = (string)rowView.Days[index];
            }

            switch (input)
            {
                case ShiftScheduleMonthEntry.AM_SHIFT: return Brushes.LightCoral;
                case ShiftScheduleMonthEntry.PM_SHIFT: return Brushes.LightBlue;
                case ShiftScheduleMonthEntry.NO_SHIFT: return Brushes.LightGray;
                default: return Brushes.Khaki;
            }
        }
    }
}
