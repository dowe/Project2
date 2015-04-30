using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class Util
    {
        public static Dictionary<T, string> EnumValues<T>()
        {
            Type enumType = typeof(T);
            Dictionary<T, string> dict = new Dictionary<T, string>();
            foreach (T value in Enum.GetValues(enumType))
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

        public static string ToString(object elem)
        {

            Type type = elem.GetType();
            PropertyInfo[] list = type.GetProperties();
            string text = "";
            Boolean seperate = false;

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].CanRead)
                {
                    if (seperate)
                    {
                        text += "\n";
                    }
                    seperate = true;


                    object child = list[i].GetValue(elem);
                    Type childType = list[i].PropertyType;

                    text += list[i].Name + "=";

                    if (child == null)
                    {
                        text += "null";
                    }
                    else
                    {

                        if (typeof(IEnumerable<object>).IsAssignableFrom(childType))
                        {

                            List<object> l = new List<object>(child as IEnumerable<object>);
                            if (l.Count == 0)
                            {
                                text += "[ ]";
                            }
                            else
                            {
                                text += "[\n";
                                int index = 0;
                                foreach (object item in l)
                                {
                                    text += Tab(index + " = {\n" + Tab(ToString(item)) + "\n}") + "\n";
                                    index++;
                                }
                                text += "]";
                            }

                        }
                        else if (childType.IsPrimitive
                            || childType.ToString().Equals("System.String")
                            || typeof(DateTime).IsAssignableFrom(childType))
                        {
                            text += child.ToString();
                        }
                        else
                        {
                            text += "{\n" + Tab(ToString(child)) + "\n}";
                        }
                    }
                }
            }

            if (list.Length == 0)
            {
                return elem.ToString();
            }

            return text;

        }

        public static String Tab(String txt)
        {
            int n = 0;
            do
            {
                txt = txt.Insert(n, "   ");
                n = txt.IndexOf('\n', n);
                if (n < 0)
                {
                    return txt;
                }
                n++;
            }
            while (true);
        }
    }
}
