using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.ViewModel
{
    public class RegisterCustomerVM : ViewModelBase
    {
        public object SMSWantedValues
        {
            get
            {
                return EnumValues(typeof(SMSWanted));
            }
        }

        public object GenderTitleValues
        {
            get
            {
                return EnumValues(typeof(GenderTitle));
            }
        }

        private static Dictionary<object, string> EnumValues(Type enumType)
        {
            Dictionary<object, string> dict = new Dictionary<object, string>();
            foreach (SMSWanted value in Enum.GetValues(enumType).Cast<SMSWanted>())
            {
                dict.Add(value, GetDescription(value, enumType));
            }

            return dict;
        }

        private static string GetDescription(object enumValue, Type enumType)
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

    public enum SMSWanted
    {
        [Description("Ja")]
        Yes,
        [Description("Nein")]
        No
    }

    public enum GenderTitle
    {
        [Description("Herr")]
        Mr,
        [Description("Frau")]
        Mrs
    }
}
