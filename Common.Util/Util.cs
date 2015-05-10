using Common.DataTransferObjects;
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
        private static Random rnd = new Random(DateTime.Now.Second);

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
            return ToString(elem, false);
        }

        public static string ToString(object elem, bool debug)
        {
            if (elem == null)
            {
                Info(debug, "NULL detected");
                return "null\n";
            }

            Type type = elem.GetType();

            if (typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                Info(debug, "LIST detected");
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
                Info(debug, "PRIMITIVE detected");
                return elem.ToString() + "\n";
            }
            else
            {
                Info(debug, "OBJECT detected");

                PropertyInfo[] list = type.GetProperties();
                string text = "{\n";

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].CanRead)
                    {
                        Info(debug, "ELEM " + list[i].Name);

                        object child = list[i].GetValue(elem);
                        text += list[i].Name + "=" + ToString(child);

                    }
                }

                return Tab(text) + "}\n";
            }
        }

        public static void Info(bool debug, string text)
        {
            if (debug)
            {
                Console.WriteLine(text);
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

        public static ShiftSchedule CreateTestData(DateTime refDate)
        {
            ShiftSchedule obj = new ShiftSchedule();

            obj.Date = refDate;
            obj.DayEntry = new List<DayEntry>();

            List<Employee> admins = CreateEmployees<AdministrationAssistant>(10);
            List<Employee> driver = CreateEmployees<Driver>(20);
            List<Employee> lab = CreateEmployees<LabAssistant>(10);

            List<Employee> empty = new List<Employee>();

            for (DateTime date = new DateTime(refDate.Year, refDate.Month, 1);
                 date.Month == refDate.Month; date = date.AddDays(1.0))
            {
                DayEntry entry = new DayEntry();
                entry.Date = date;

                entry.AM = new List<Employee>();
                entry.PM = new List<Employee>();

                Add(entry.AM, admins, 3, rnd, empty);
                Add(entry.AM, lab, 3, rnd, empty);
                Add(entry.AM, driver, 6, rnd, empty);

                Add(entry.PM, admins, 3, rnd, entry.AM);
                Add(entry.PM, lab, 3, rnd, entry.AM);
                Add(entry.PM, driver, 6, rnd, entry.AM);

                obj.DayEntry.Add(entry);
            }

            return obj;
        }


        private static void Add(IList<Employee> to, IList<Employee> from, int p, Random rnd, IList<Employee> not)
        {
            for (int i = 0; i < p; i++)
            {
                Employee e;

                do
                {
                    e = from[rnd.Next(from.Count)];
                } while (to.Contains(e) || not.Contains(e));

                to.Add(e);
            }
        }

        public static List<Employee> CreateEmployees<T>(int n) where T : Employee
        {
            Type type = typeof(T);

            List<Employee> list = new List<Employee>();
            string text = type.Name;
            for (int i = 0; i < n; i++)
            {
                T employee = (T)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                list.Add(employee);

                employee.FirstName = "F" + text + i;
                employee.LastName = "N" + text + i;
            }

            return list;
        }
    }

}
