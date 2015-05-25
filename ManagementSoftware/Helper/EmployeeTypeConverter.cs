using Common.DataTransferObjects;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ManagementSoftware.Helper
{
    public class EmployeeTypeConverter : IValueConverter
    {
        public static readonly string TYPE_NULL = "NULL";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return TYPE_NULL;
            }
            if (IsType<Employee>(value))
            {
                Employee emp = (Employee)(value);

                return Util.CreateValuePair<EEmployeeType>(emp.EmployeeType).Value;
            }

            return value.GetType().Name;
        }

        private bool IsType<T>(object obj)
        {
            return typeof(T).IsAssignableFrom(obj.GetType());
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
