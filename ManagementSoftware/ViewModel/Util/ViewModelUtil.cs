using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.ViewModel.Util
{
    class ViewModelUtil
    {
        public static Dictionary<T, string> EnumValues<T>()
        {
            Type enumType = typeof(T);
            Dictionary<T, string> dict = new Dictionary<T, string>();
            foreach (T value in Enum.GetValues(enumType) )
            {
                dict.Add(value, GetDescription(value, enumType));
            }

            return dict;
        }

        public static KeyValuePair<T, string> CreateValuePair<T>(T enumValue)
        {
            string desc = GetDescription(enumValue, typeof(T));
            return new KeyValuePair<T, string>(enumValue, desc);
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

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
