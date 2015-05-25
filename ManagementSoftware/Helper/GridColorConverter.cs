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
using System.Diagnostics.CodeAnalysis;

namespace ManagementSoftware.Helper
{
    
    public class GridColorConverter : IMultiValueConverter
    {
        public static readonly SolidColorBrush AM_SHIFT_BRUSH = Brushes.LightCoral;
        public static readonly SolidColorBrush PM_SHIFT_BRUSH = Brushes.LightBlue;
        public static readonly SolidColorBrush NO_SHIFT_BRUSH = Brushes.LightGray;
        public static readonly SolidColorBrush DEFAULT_BRUSH = Brushes.Khaki;

        [ExcludeFromCodeCoverage]
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = "";

            int index = Index(values[0]);

            List<string> days = (List<string>)values[1];

            if (index >= 0 && index < days.Count)
            {
                input = days[index];
            }

            switch (input)
            {
                case ShiftScheduleMonthEntry.AM_SHIFT: return AM_SHIFT_BRUSH;
                case ShiftScheduleMonthEntry.PM_SHIFT: return PM_SHIFT_BRUSH;
                case ShiftScheduleMonthEntry.NO_SHIFT: return NO_SHIFT_BRUSH;
                default: return DEFAULT_BRUSH;
            }
        }

        [ExcludeFromCodeCoverage]
        private int Index(object obj)
        {
            if (obj is DataGridCell)
            {
                DataGridCell dgc = (DataGridCell)obj;
                ShiftScheduleMonthEntry rowView = (ShiftScheduleMonthEntry)(dgc.DataContext);
                return dgc.Column.DisplayIndex - 1;
            }
            else
            {
                return (Int32)obj - 1;
            }
        }
    }
}
