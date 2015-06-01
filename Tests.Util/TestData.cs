using Common.DataTransferObjects;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data
{
    public class TestData
    {


        public static DailyStatistic CreateDailyStatistic(String date)
        {
            DailyStatistic _MyDailyStatistic = new DailyStatistic();
            _MyDailyStatistic.Date = Convert.ToDateTime(date);
            _MyDailyStatistic.NumberOfNewOrders = 22;
            _MyDailyStatistic.NumberOfCompletedTests = 11;
            _MyDailyStatistic.NumberOfCompletedOrders = 100;
            _MyDailyStatistic.NumberOfTestsInProgress = 33;
            _MyDailyStatistic.NumberOfOrdersInProgress = 42;
            return _MyDailyStatistic;
        }
        

        public static Customer CreateTestData(ETitle title, ESMSRequested smsRequested)
        {

            Customer _Customer = new Customer();
            _Customer.Title = Util.CreateValuePair<ETitle>(title).Value;
            _Customer.FirstName = "Hans";
            _Customer.LastName = "Feil";
            _Customer.UserName = "hfeil";
            _Customer.Password = "asdf";
            _Customer.SMSRequested = smsRequested == ESMSRequested.Yes;
            _Customer.MobileNumber = "016212345";
            _Customer.Label = "Praxis ABC";
            _Customer.Address = new Address("abc 8", "77656", "Offenburg");
            _Customer.BankAccount = new BankAccount("12345-DE", "Hans feil");
            return _Customer;
        }

        public static List<Analysis> CreateAnalysis()
        {
            List<Analysis> list = new List<Analysis>();

            list.Add(new Analysis("a", 0.0F, 100.0F, "m", 3.5F, SampleType.BLOOD));
            list.Add(new Analysis("b", 0.0F, 100.0F, "m", 3.0F, SampleType.BLOOD));
            list.Add(new Analysis("c", 0.0F, 100.0F, "m", 3.55F, SampleType.BLOOD));

            return list;
        }

        public static ShiftSchedule[] CreateShiftSchedules()
        {
            return new ShiftSchedule[] { Create(new DateTime(2015, 1, 1)), Create(new DateTime(2015, 2, 1)) };
        }

        private static ShiftSchedule Create(DateTime refDate)
        {
            ShiftSchedule obj = new ShiftSchedule();

            obj.Date = refDate;
            obj.DayEntry = new List<DayEntry>();

            List<EmpCont> emps = new List<EmpCont>();

            AdministrationAssistant admin0 = new AdministrationAssistant();
            admin0.FirstName = "Admin0FN";
            admin0.LastName = "Admin0LN";
            emps.Add(new EmpCont(admin0, 3));

            AdministrationAssistant admin1 = new AdministrationAssistant();
            admin1.FirstName = "Admin1FN";
            admin1.LastName = "Admin1LN";
            emps.Add(new EmpCont(admin1, 2));

            Driver driver0 = new Driver();
            driver0.FirstName = "Driver0FN";
            driver0.LastName = "Driver0LN";
            driver0.UserName = "Driver0UN";
            driver0.Password = "asdf";
            emps.Add(new EmpCont(driver0, 3));

            Driver driver1 = new Driver();
            driver1.FirstName = "Driver1FN";
            driver1.LastName = "Driver1LN";
            driver1.UserName = "Driver1UN";
            driver1.Password = "asdf";
            emps.Add(new EmpCont(driver1, 2));

            LabAssistant lab0 = new LabAssistant();
            lab0.FirstName = "Lab0FN";
            lab0.LastName = "Lab0LN";
            emps.Add(new EmpCont(lab0, 3));

            LabAssistant lab1 = new LabAssistant();
            lab1.FirstName = "Lab1FN";
            lab1.LastName = "Lab1LN";
            emps.Add(new EmpCont(lab1, 2));

            for (DateTime date = new DateTime(refDate.Year, refDate.Month, 1);
                 date.Month == refDate.Month; date = date.AddDays(1.0))
            {
                DayEntry entry = new DayEntry();
                entry.Date = date;

                entry.AM = new List<Employee>();
                entry.PM = new List<Employee>();

                foreach (EmpCont cont in emps)
                {
                    if (cont.Index == 0)
                    {
                        entry.AM.Add(cont.Emp);
                    }
                    else if (cont.Index == 1)
                    {
                        entry.PM.Add(cont.Emp);
                    }

                    cont.Tick();
                }
                
                obj.DayEntry.Add(entry);
            }

            return obj;
        }
    }

    class EmpCont
    {
        private int modulo;

        public EmpCont(Employee e, int m)
        {
            modulo = m;
            Emp = e;
            Index = 0;
        }

        public void Tick()
        {
            Index = (Index + 1) % modulo;
        }
        public int Index { get; set; }
        public Employee Emp { get; set; }
    }
}
