using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class ShiftScheduleMonthEntry
    {
        public const string AM_SHIFT = "F";
        public const string PM_SHIFT = "S";
        public const string NO_SHIFT = "-";

        public int NumberOfDaysInMonth
        {
            get;
            set;
        }

        public ShiftScheduleMonthEntry(Employee e, int n)
        {
            NumberOfDaysInMonth = n;
            Employee = e;

            Days = new List<string>();

            for (int i = 0; i < n; i++)
            {
                Days.Add(NO_SHIFT);
            }

            for (int i = n; i < 31; i++)
            {
                Days.Add(null);
            }
        }

        public string EmployeeName
        {
            get
            {
                Employee e = Employee;
                if (e == null)
                {
                    return "";
                }
                return e.LastName + ", " + e.FirstName;
            }
        }

        public List<string> Days
        {
            get;
            set;
        }

        public Employee Employee
        {
            get;
            set;
        }
    }
}
