using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ManagementSoftware.Helper
{
    public class EmployeeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ( value == null ) {
                return "NULL";
            }
            if (IsType<Driver>(value))
            {
                return "Fahrer";
            }
            if (IsType<LabAssistant>(value))
            {
                return "Labor";
            }
            if (IsType<AdministrationAssistant>(value))
            {
                return "Verwaltung";
            }

            return value.GetType().Name;
        }

        private bool IsType<T>(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return typeof(T).IsAssignableFrom(obj.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
