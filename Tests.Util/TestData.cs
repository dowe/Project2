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

        public static List<Order> CreateOrderList()
        {
            List<Order> orders = new List<Order>();
            List<Analysis> Analysis = new List<Analysis>();
            Analysis.Add(new Analysis("Blut_Leukozyten", 4, 10, "Tsd./µl", (float)29.99, SampleType.BLOOD));
            Analysis.Add(new Analysis("Blut_Erythrozyten", (float)4.3, (float)5.9, "Mio./µl", (float)29.99, SampleType.BLOOD));
            Analysis.Add(new Analysis("Blut_Hämoglobin", (float)12, (float)18, "g/dl", (float)15.95, SampleType.BLOOD));
            Analysis.Add(new Analysis("Blut_Hämatonkrit", (float)37, (float)54, "%", (float)15.95, SampleType.BLOOD));
            Analysis.Add(new Analysis("Blut_Thrombozyten", (float)150, (float)400, "Tsd./µl", (float)12.55, SampleType.BLOOD));
            Analysis.Add(new Analysis("Urin_Osmolatität", (float)50, (float)1200, "mosm/kg", (float)10.95, SampleType.URINE));
            Analysis.Add(new Analysis("Urin_ph-Wert", (float)4.8, (float)7.6, "-", (float)5.95, SampleType.URINE));
            Analysis.Add(new Analysis("Urin_Gewicht", (float)1001, (float)1035, "g/l", (float)7.45, SampleType.URINE));
            Analysis.Add(new Analysis("Urin_Albumin", (float)0, (float)20, "mg", (float)10.95, SampleType.URINE));
            Analysis.Add(new Analysis("Speichel_Cortisol", (float)0, (float)9.8, "µg/l", (float)4.50, SampleType.SALIVA));
            Analysis.Add(new Analysis("Speichel_DHEA", (float)130, (float)490, "pg/ml", (float)15.99, SampleType.SALIVA));
            Analysis.Add(new Analysis("Speichel_Östradiol", (float)1, (float)2, "pg/ml", (float)20.50, SampleType.SALIVA));
            Analysis.Add(new Analysis("Speichel_Testosteron", (float)47.2, (float)136.2, "pg/ml", (float)24.95, SampleType.SALIVA));
            Analysis.Add(new Analysis("Speichel_Progesteron", (float)12.7, (float)57.4, "pg/ml", (float)24.95, SampleType.SALIVA));
            Analysis.Add(new Analysis("Stuhl_Calprotectin", (float)10, (float)31, "µg/g", (float)15.95, SampleType.EXCREMENT));
            Analysis.Add(new Analysis("Stuhl_Candida", (float)0, (float)10000, "Pilze/g", (float)15.50, SampleType.EXCREMENT));

            List<Driver> el = new List<Driver>();
          
            for (int i = 0; i < 10; i++)
            {
                el.Add(new Driver() { FirstName = "Driver" + i, LastName = "Assistant", UserName = "Driv" + i, Password = "Driv" + i });
            }
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer("Dr.", "House", "house", "asdf", new Address("Hauptstr. 88", "77652", "Offenburg"), "Dr. House imba Werkstatt", new BankAccount("SDLFKJSDLKFJ", "Dr. House")) { TwoWayRoadCostInEuro = 11.11f });
            customers.Add(new Customer("Alice", "Vette", "vette", "asdf", new Address("Hauptstr. 88", "77652", "Offenburg"), "Vetter Alice Fachärztin für Allgemeinmedizin", new BankAccount("1asdf243ew", "Alice Vette")) { TwoWayRoadCostInEuro = 1.11f });
            customers.Add(new Customer("Wolfgang", "Bätz", "lolo", "asdf", new Address("Am Marktplatz 7", "77652", "Offenburg"), "Bätz Wolfgang Dr.med. Gefäßchirurg", new BankAccount("ASDLF23456", "Wolfgang Bätz"), true, "107438570935") { TwoWayRoadCostInEuro = 2.11f });
            customers.Add(new Customer("Michael", "Brake", "holzmichel", "asdf", new Address("Hauptstr. 98", "77652", "Offenburg"), "Brake Michael Dr. med. Arzt für Urologie", new BankAccount("ALKFJ34565768", "Michael Brake"), true, "9347983476") { TwoWayRoadCostInEuro = 44.11f });
            customers.Add(new Customer("Elke", "Brüderle", "Elli", "asdf", new Address("Ebertplatz 12", "77652", "Offenburg"), "Brüderle Elke Dr. Frauenärztin", new BankAccount("LKFJGFG23456", "Brüderle Elke")) { TwoWayRoadCostInEuro = 1.11f });
            customers.Add(new Customer("Traunecker", "Ulrich", "ulli", "asdf", new Address("Leutkirchstraße 13", "77723", "Gengenbach"), "Dr. med. Ulrich Traunecker", new BankAccount("UZJH87698347", "Ulrich Traunecker"), true, "379786546") { TwoWayRoadCostInEuro = 0.11f });
            customers.Add(new Customer("Matthias", "Ruff", "ruffi", "asdf", new Address("Hauptstraße 24", "77723", "Gengenbach"), "Dr. med. Matthias Ruff", new BankAccount("HUGZGU87687625", "Matthias Ruff")) { TwoWayRoadCostInEuro = 5.11f });
            customers.Add(new Customer("Stefan", "Leuthner", "leuti", "asdf", new Address("Hauptstraße 61", "77799", "Ortenberg"), "Herr Dr. med. Stefan Leuthner", new BankAccount("UIGUZ7868", "Leuthners Frau")) { TwoWayRoadCostInEuro = 6.11f });

            Order Order1 = new Order();
            orders.Add(new Order()
            {
                OrderDate = DateTime.Now.AddHours(-2),
                Invoiced = false,
                RemindedAfterFiveHours = false,
                Customer = customers[2],
                Driver = el[3],
                Test = new List<Test>()
                {
                    new Test("GanzAnderer", Analysis.Find(e => e.Name.Equals("Blut_Hämoglobin")))
                    {
                        TestState = TestState.WAITING_FOR_DRIVER
                    }
                }
            });
            orders.Add(new Order()
            {
                OrderDate = DateTime.Now.AddHours(-2),
                Invoiced = false,
                RemindedAfterFiveHours = false,
                Customer = customers[1],
                Driver = null,
                Test = new List<Test>()
                {
                    new Test("GanzAnderer", Analysis.Find(e => e.Name.Equals("Blut_Hämoglobin")))
                    {
                        TestState = TestState.WAITING_FOR_DRIVER
                    }
                }
            });
            orders.Add(new Order()
            {

                OrderDate = DateTime.Now.AddDays(-2),
                BringDate = DateTime.Now.AddDays(-1),
                CollectDate = DateTime.Now.AddDays(-1),
                CompleteDate = DateTime.Now,
                Invoiced = false,
                 Driver = el[1],
                RemindedAfterFiveHours = false,
                Customer = customers[0],
                Test = new List<Test>()
                {
                    new Test("NochNPatient", Analysis.Find(e => e.Name.Equals("Blut_Hämoglobin")))
                    {
                        StartDate = DateTime.Now.AddHours(-10),
                        EndDate = DateTime.Now,
                        ResultValue = 130000f,
                        TestState = TestState.COMPLETED,
                        AlarmState = AlarmState.FIRST_ALARM_TRANSMITTED,
                         Critical = true

                    },
                    new Test("NochNPatient", Analysis.Find(e => e.Name.Equals("Urin_Gewicht")))
                    {
                        StartDate = DateTime.Now.AddHours(-5),
                        EndDate = DateTime.Now.AddHours(-2),
                        ResultValue = 1000f,
                        AlarmState = AlarmState.FIRST_ALARM_CONFIRMED,
                        Critical = true,
                        TestState = TestState.COMPLETED
                    },
                      new Test("NochNPatient", Analysis.Find(e => e.Name.Equals("Urin_Albumin")))
                    {
                        StartDate = DateTime.Now.AddHours(-1),
                        
                        AlarmState = AlarmState.NO_ALARM,
                        Critical = true,
                        TestState = TestState.IN_PROGRESS
                    },
                    new Test("AndererPatient", Analysis.Find(e => e.Name.Equals("Stuhl_Candida")))
                    {
                        TestState = TestState.IN_PROGRESS
                    },
                }
            });

            return orders;
        }

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
