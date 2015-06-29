using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using Common.Util;

namespace Server.ShiftScheduleCreation
{
    public class DummyShiftScheduleCreator : IShiftScheduleCreator
    {

        public ShiftSchedule createShiftSchedule(ShiftSchedule last, List<Employee> emps, DateTime date)
        {
            return Util.CreateTestData(date, emps);
        }

    }

    public class ShiftScheduleCreator : IShiftScheduleCreator
    {
        public ShiftSchedule createShiftSchedule(ShiftSchedule last, List<Employee> empl, DateTime firstOfMonth)
        {
            var context = new LaborContext();
            List<Driver> drivers = context.Driver.ToList();
            List<AdministrationAssistant> admins = context.AdministrationAssistant.ToList();
            List<Employee> allEmpl = context.Employee.ToList();
            var amountOfLabAssistants = allEmpl.Count - admins.Count - drivers.Count;

            // Check if enough Employees of each type are available
            if (drivers.Count < 10)
            {
                Console.WriteLine("Could not create Shift Schedule - Not enough Drivers available");
            }
            else if (admins.Count < 5)
            {
                Console.WriteLine("Could not create Shift Schedule - Not enough Administration Assistants available");
            }
            else if (amountOfLabAssistants < 20)
            {
                Console.WriteLine("Could not create Shift Schedule - Not enough Laboratory Assistants available");
            }
            else
            {
                // List to count amount of shifts for each Employee
                List<int> amountOfShifts = new List<int>();
                // List to check AM/PM rate
                List<int> shiftsAM = new List<int>();

                for (int d = 0; d < empl.Count; d++)
                {
                    amountOfShifts.Add(0);
                    shiftsAM.Add(0);
                }

                ShiftSchedule schedule = new ShiftSchedule();
                schedule.Date = firstOfMonth;
                var entries = new List<DayEntry>();

                // Get amount of days for current month
                var daysCur = DateTime.DaysInMonth(schedule.Date.Year, schedule.Date.Month);

                // add last day of last month to list, to check if Employees for 
                // AM shift at the first of Month already worked at the last of last Month PM
                DayEntry lastOfLastMonth = new DayEntry();
                lastOfLastMonth.Date = schedule.Date.AddDays(-1);
                var daysLastMonth = DateTime.DaysInMonth(lastOfLastMonth.Date.Year, lastOfLastMonth.Date.Month);

                // check if Day Entries of last Month are not null
                if (last.DayEntry != null)
                {
                    lastOfLastMonth.PM = last.DayEntry[daysLastMonth - 1].PM;
                }

                entries.Add(lastOfLastMonth);

                // add Day Entries to shift Schedule
                var entry = new DayEntry();
                for (int d = 0; d < daysCur; d++)
                {
                    entry = new DayEntry();
                    entry.Date = schedule.Date.AddDays(d);
                    entry.AM = new List<Employee>();
                    entry.PM = new List<Employee>();
                    entries.Add(entry);
                }

                // Count Offset to Check Current week's already passed shifts
                DayEntry offsetDays = new DayEntry();

                offsetDays.Date = firstOfMonth;

                int iOffset = 0;
                while (!offsetDays.Date.AddDays(-iOffset).DayOfWeek.ToString().Equals("Monday"))
                {
                    iOffset++;
                }

                // Get Data for Current Week from last Month
                for (int d = (daysLastMonth - iOffset); d < daysLastMonth; d++)
                {
                    if (last.DayEntry != null)
                    {
                        for (int e = 0; e < empl.Count; e++)
                        {
                            if (last.DayEntry[d].AM.Contains(empl[e]))
                            {
                                amountOfShifts[e]++;
                                shiftsAM[e]++;
                            }
                            else if (last.DayEntry[d].PM.Contains(empl[e]))
                            {
                                amountOfShifts[e]++;
                                shiftsAM[e]--;
                            }
                        }
                    }
                }

                //Check if PM shifts from last of previous Month is not null
                if (entries[0].PM == null)
                {
                    List<Employee> pmLastOfLastMonth = new List<Employee>();
                    entries[0].PM = pmLastOfLastMonth;
                }


                int cntEmployees = 0;
                int cntDriverAM = 0;
                int cntLabsAM = 0;
                int cntAdminsAM = 0;

                int cntDriverPM = 0;
                int cntLabsPM = 0;
                int cntAdminsPM = 0;



                for (int d = 1; d < daysCur + 1; d++)
                {
                    while (cntEmployees < 10)
                    {

                        for (int e = 0; e < empl.Count; e++)
                        {
                            // Check if Employee has not worked too many Shifts and amount of AM is less then PM
                            if (!entries[d - 1].PM.Contains(empl[e]) && amountOfShifts[e] < 4 && shiftsAM[e] <= 0)
                            {
                                // Check amount of Drivers already added at Current Day
                                if (empl[e].EmployeeType.Equals(EEmployeeType.TypeDriver) && cntDriverAM < 2)
                                {
                                    entries[d].AM.Add(empl[e]);
                                    cntDriverAM++;
                                    amountOfShifts[e]++;
                                    shiftsAM[e]++;
                                    cntEmployees++;
                                }
                                // Check amount of Administration Assistants already added
                                else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeAdministrationAssistant) && cntAdminsAM < 1)
                                {
                                    entries[d].AM.Add(empl[e]);
                                    cntAdminsAM++;
                                    amountOfShifts[e]++;
                                    shiftsAM[e]++;
                                    cntEmployees++;
                                }
                                // Check amount of Lab Assistants already added
                                else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeLabAssistant) && cntLabsAM < 3)
                                {
                                    entries[d].AM.Add(empl[e]);
                                    cntLabsAM++;
                                    amountOfShifts[e]++;
                                    shiftsAM[e]++;
                                    cntEmployees++;
                                }
                            }
                            // Amount of Shifts PM is less then AM
                            if (!entries[d].AM.Contains(empl[e]) && amountOfShifts[e] < 4 && shiftsAM[e] >= 0)
                            {
                                // do the same for PM
                                if (empl[e].EmployeeType.Equals(EEmployeeType.TypeDriver) && cntDriverPM < 2)
                                {
                                    entries[d].PM.Add(empl[e]);
                                    cntDriverPM++;
                                    amountOfShifts[e]++;
                                    shiftsAM[e]--;
                                    cntEmployees++;
                                }
                                else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeAdministrationAssistant) && cntAdminsPM < 1)
                                {
                                    entries[d].PM.Add(empl[e]);
                                    cntAdminsPM++;
                                    amountOfShifts[e]++;
                                    shiftsAM[e]--;
                                    cntEmployees++;
                                }
                                else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeLabAssistant) && cntLabsPM < 3)
                                {
                                    entries[d].PM.Add(empl[e]);
                                    cntLabsPM++;
                                    amountOfShifts[e]++;
                                    shiftsAM[e]--;
                                    cntEmployees++;
                                }
                            }
                        }
                    }

                    // reset shifts for new Week
                    if ((iOffset + d) % 7 == 0)
                    {
                        for (int e = 0; e < empl.Count; e++)
                        {
                            amountOfShifts[e] = 0;
                        }
                    }

                    cntEmployees = 0;
                    cntAdminsAM = 0;
                    cntAdminsPM = 0;
                    cntDriverAM = 0;
                    cntDriverPM = 0;
                    cntLabsAM = 0;
                    cntLabsPM = 0;
                }

                // remove last of previous schedule
                entries.RemoveAt(0);
                schedule.DayEntry = entries;


                return schedule;
            }
            return null;
        }
    }
}