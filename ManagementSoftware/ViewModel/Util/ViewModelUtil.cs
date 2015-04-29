using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.ViewModel.Util
{
    class ViewModelUtil
    {
        public static Dictionary<object, string> EnumValues(Type enumType)
        {
            Dictionary<object, string> dict = new Dictionary<object, string>();
            foreach (SMSWanted value in Enum.GetValues(enumType).Cast<SMSWanted>())
            {
                dict.Add(value, GetDescription(value, enumType));
            }

            return dict;
        }

        public static string GetDescription(object enumValue, Type enumType)
        {
            var descriptionAttribute = enumType
              .GetField(enumValue.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
              ? descriptionAttribute.Description
              : enumValue.ToString();
        }
    }
}
