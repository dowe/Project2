using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;

namespace Server.ShiftScheduleCreation
{

    public class ShiftScheduleCreator : IShiftScheduleCreator
    {
        public ShiftSchedule createShiftSchedule(ShiftSchedule last, DateTime firstOfMonth)
        {
            var context = new LaborContext();
            var empl = context.Employee.ToList();

            List<int> amountOfShifts = new List<int>();
            List<int> shiftsAM = new List<int>();

            for (int d = 0; d < empl.Count; d++)
            {
                amountOfShifts.Add(0);
                shiftsAM.Add(0);
            }

            ShiftSchedule schedule = new ShiftSchedule();
            schedule.Date = firstOfMonth;
            var entries = new List<DayEntry>();

            var daysCur = DateTime.DaysInMonth(schedule.Date.Year, schedule.Date.Month);

            DayEntry lastOfLastMonth = new DayEntry();
            lastOfLastMonth.Date = schedule.Date.AddDays(-1);
            var daysLastMonth = DateTime.DaysInMonth(lastOfLastMonth.Date.Year, lastOfLastMonth.Date.Month);
            lastOfLastMonth.PM = last.DayEntry[daysLastMonth - 1].PM;
            entries.Add(lastOfLastMonth);

            var entry = new DayEntry();
            for (int d = 0; d < daysCur; d++)
            {
                entry = new DayEntry();
                entry.Date = schedule.Date.AddDays(d);
                entry.AM = new List<Employee>();
                entry.PM = new List<Employee>();
                entries.Add(entry);
            }

            DayEntry offsetDays = new DayEntry();

            offsetDays.Date = firstOfMonth;

            int iOffset = 0;
            while (!offsetDays.Date.AddDays(-iOffset).DayOfWeek.ToString().Equals("Monday"))
            {
                iOffset++;
            }
            for (int d = (daysLastMonth - iOffset); d < daysLastMonth; d++)
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
            int cntEmployees = 0;
            int cntDriverAM = 0;
            int cntLabsAM = 0;
            int cntAdminsAM = 0;

            int cntDriverPM = 0;
            int cntLabsPM = 0;
            int cntAdminsPM = 0;

            for (int d = 1; d < daysCur+1; d++)
            {
                while (cntEmployees < 12)
                {
             
                    for (int e = 0; e < empl.Count; e++)
                    {
                        if (!entries[d - 1].PM.Contains(empl[e]) && amountOfShifts[e] < 3 && shiftsAM[e] <= 0)
                        {
                            if (empl[e].EmployeeType.Equals(EEmployeeType.TypeDriver) && cntDriverAM < 2)
                            {
                                entries[d].AM.Add(empl[e]);
                                Console.WriteLine("Added Driver AM " + empl[e].FirstName + " at" + entries[d].Date);
                                cntDriverAM++;
                                amountOfShifts[e]++;
                                shiftsAM[e]++;
                                cntEmployees++;
                            }
                            else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeAdministrationAssistant) && cntAdminsAM < 2)
                            {
                                entries[d].AM.Add(empl[e]);
                                Console.WriteLine("Added Admin AM " + empl[e].FirstName + " at" + entries[d].Date);
                                cntAdminsAM++;
                                amountOfShifts[e]++;
                                shiftsAM[e]++;
                                cntEmployees++;
                            }
                            else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeLabAssistant) && cntLabsAM < 2)
                            {
                                Console.WriteLine("Added Lab AM " + empl[e].FirstName  + " at" + entries[d].Date);
                                entries[d].AM.Add(empl[e]);
                                cntLabsAM++;
                                amountOfShifts[e]++;
                                shiftsAM[e]++;
                                cntEmployees++;
                            }
                        }
                        if (!entries[d].AM.Contains(empl[e]) && amountOfShifts[e] < 3 && shiftsAM[e] >= 0)
                        {
                            if (empl[e].EmployeeType.Equals(EEmployeeType.TypeDriver) && cntDriverPM < 2)
                            {
                                Console.WriteLine("Added Driver PM " + empl[e].FirstName  + " at" + entries[d].Date);
                                entries[d].PM.Add(empl[e]);
                                cntDriverPM++;
                                amountOfShifts[e]++;
                                shiftsAM[e]--;
                                cntEmployees++;
                            }
                            else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeAdministrationAssistant) && cntAdminsPM < 2)
                            {
                                Console.WriteLine("Added Admin PM " + empl[e].FirstName + " at" + entries[d].Date);
                                entries[d].PM.Add(empl[e]);
                                cntAdminsPM++;
                                amountOfShifts[e]++;
                                shiftsAM[e]--;
                                cntEmployees++;
                            }
                            else if (empl[e].EmployeeType.Equals(EEmployeeType.TypeLabAssistant) && cntLabsPM < 2)
                            {
                                Console.WriteLine("Added Lab PM " + empl[e].FirstName  + " at" + entries[d].Date);
                                entries[d].PM.Add(empl[e]);
                                cntLabsPM++;
                                amountOfShifts[e]++;
                                shiftsAM[e]--;
                                cntEmployees++;
                            }
                        }
                    }
                }

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

            entries.RemoveAt(0);
            schedule.DayEntry = entries;

            for (int d = 0; d < schedule.DayEntry.Count; d++)
            {
                for (int e = 0; e < schedule.DayEntry[d].AM.Count; e++)
                {
                    Console.WriteLine("Date: " + schedule.DayEntry[d].Date + "Employee AM " + e + " " + schedule.DayEntry[d].AM[e].FirstName);
                }

                for (int e = 0; e < schedule.DayEntry[d].PM.Count; e++)
                {
                    Console.WriteLine("Date: " + schedule.DayEntry[d].Date + "Employee PM " + e + " " + schedule.DayEntry[d].PM[e].FirstName);
                }
            
            }
            
            return schedule;
        }
    }
}