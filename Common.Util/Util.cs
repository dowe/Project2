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
            if (elem == null)
            {
                Console.WriteLine("NULL detected");
                return "null\n";
            }

            Type type = elem.GetType();

            if (typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                Console.WriteLine("LIST detected");
                List<object> l = new List<object>(elem as IEnumerable<object>);
                if (l.Count == 0)
                {
                    return "[ ]\n";
                }

                string text = "[\n";
                int index = 0;
                foreach (object item in l)
                {
                    text += index + " = " + ToString(item);
                    index++;
                }

                return Tab(text) + "]\n";
            }
            else if (IsPrimitive(type))
            {
                Console.WriteLine("PRIMITIVE detected");
                return elem.ToString() + "\n";
            }
            else
            {
                Console.WriteLine("OBJECT detected");


                PropertyInfo[] list = type.GetProperties();
                string text = "{\n";

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].CanRead)
                    {
                        Console.WriteLine("ELEM " + list[i].Name);

                        object child = list[i].GetValue(elem);
                        text += list[i].Name + "=" + ToString(child);

                    }
                }

                return Tab(text) + "}\n";
            }
        }

        private static bool IsPrimitive(Type type)
        {
            return type.IsPrimitive
                       || type.ToString().Equals("System.String")
                       || typeof(DateTime).IsAssignableFrom(type);
        }

        public static String Tab(String txt)
        {
            int n = 0;
            do
            {
                
                n = txt.IndexOf('\n', n);
                if (n < 0)
                {
                    return txt;
                }
                n++;
                if (txt.IndexOf('\n', n) >= 0)
                {
                    txt = txt.Insert(n, "   ");
                }
            }
            while (true);
        }
    }
}
