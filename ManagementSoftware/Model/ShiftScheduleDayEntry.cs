using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class ShiftScheduleDayEntry
    {

        public string EmployeeAMName
        {
            get
            {
                return ToName(EmployeeAM);
            }
        }

        public string EmployeePMName
        {
            get
            {
                return ToName(EmployeePM);
            }
        }

        public Employee EmployeeAM { get; set; }
        public Employee EmployeePM { get; set; }

        private string ToName(Employee e)
        {
            if (e == null)
            {
                return "";
            }
            return e.LastName + ", " + e.FirstName;
        }
    }
}
