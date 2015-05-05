using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class ShiftScheduleRawModel
    {
        private ShiftSchedule[] _Data;
        private int _CurrentDataIndex;

        public event EventHandler Change;

        public ShiftScheduleRawModel()
        {
            _Data = new ShiftSchedule[2];
            _CurrentDataIndex = 0;
        }

        private void Notify()
        {
            if (CurrentData == null)
            {
                _CurrentDataIndex = 0;
            }
            Change(this, null);
        }

        public ShiftSchedule[] Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                Notify();
            }
        }

        public int CurrentDataIndex
        {
            get
            {
                return _CurrentDataIndex;
            }
            set
            {
                _CurrentDataIndex = value;
                Notify();
            }
        }

        public ShiftSchedule CurrentData
        {
            get
            {
                return _Data[_CurrentDataIndex];
            }
        }

        public static ShiftSchedule Create(DateTime refDate)
        {
            ShiftSchedule obj = new ShiftSchedule();

            obj.DayEntry = new List<DayEntry>();

            List<Employee> admins = CreateEmployees<AdministrationAssistant>(10);
            List<Employee> driver = CreateEmployees<Driver>(20);
            List<Employee> lab = CreateEmployees<LabAssistant>(10);

            Random rnd = new Random();

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

        private static List<Employee> CreateEmployees<T>(int n) where T : Employee
        {
            Type type = typeof(T);

            List<Employee> list = new List<Employee>();
            string text = type.Name;
            for (int i = 0; i < n; i++)
            {
                T employee;
                try
                {
                    employee = (T)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                catch
                {
                    employee = default(T);
                }

                list.Add(employee);

                employee.FirstName = "F" + text + i;
                employee.LastName = "N" + text + i;
            }

            return list;
        }

    }
}
