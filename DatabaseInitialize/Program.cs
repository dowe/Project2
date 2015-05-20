using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DatabaseCommunication;
using System.Data.Entity;
using Common.DataTransferObjects;
namespace DatabaseInitialize
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize();
        }

        private static void Initialize()
        {
            LaborContext context = new LaborContext();
            context.Database.Delete();
            InitializeAnalysis();
            InitializeEmployees();
            InitializeCars();
            InitializeDrivers();
            InitializeCustomer();
            InitializeOrders();
        }

        private static void InitializeOrders()
        {
            LaborContext con = new LaborContext();
            List<Order> orders = new List<Order>();
            var anal = new Dictionary<string, List<Analysis>>();
            anal.Add("IrgendeinPatient", new List<Analysis>()
            {
                con.Analysis.Find("Blut_Hämoglobin"),
                 con.Analysis.Find("Speichel_Cortisol")
            });

            orders.Add(new Order(anal, con.Customer.Where(c => c.UserName == "holzmichel").FirstOrDefault())
            {
               OrderDate = DateTime.Now,
               Driver = con.Driver.Where(d=>d.UserName=="Driv1").FirstOrDefault()
            });
            orders.Add(new Order()
            {
                OrderDate = DateTime.Now,
                Driver = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault(),
                BringDate = DateTime.Now,
                CollectDate = DateTime.Now,
                CompleteDate = DateTime.Now,
                Invoiced = false,
                RemindAfterFiveHours = false,
                Customer = con.Customer.Where(c => c.UserName == "holzmichel").FirstOrDefault(),
                Test = new List<Test>()
                {
                    new Test("NochNPatient", con.Analysis.Find("Blut_Hämoglobin"))
                    {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        ResultValue = 13f,
                        TestState = TestState.COMPLETED
                    },
                    new Test("NochNPatient", con.Analysis.Find("Urin_Gewicht"))
                    {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        ResultValue = 1000f,
                        AlarmState = AlarmState.FIRST_ALARM_CONFIRMED,
                        Critical = true,
                        TestState = TestState.COMPLETED
                    },
                    new Test("NochNPatient", con.Analysis.Find("Stuhl_Candida"))
                    {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        ResultValue = 11000f,
                        AlarmState = AlarmState.FIRST_ALARM_CONFIRMED,
                        Critical = true,
                        TestState = TestState.COMPLETED
                    },
                }
            });
            orders.Add(new Order(anal, con.Customer.Where(c => c.UserName == "ulli").FirstOrDefault())
            {
                OrderDate = DateTime.Now,
                Driver = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault()
            });
            
            con.Order.AddRange(orders);
            con.SaveChanges();
        }

        private static void InitializeAnalysis()
        {
            LaborContext con = new LaborContext();
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
            con.Analysis.AddRange(Analysis);
            con.SaveChanges();
        }

        private static void InitializeEmployees()
        {
            LaborContext con = new LaborContext();
            List<Employee> el = new List<Employee>();
            for (int i = 0; i < 20; i++)
            {
                el.Add(new LabAssistant() { FirstName = "Lab" + i, LastName = "Assistant" });
            }
            for (int i = 0; i < 5; i++)
            {
                el.Add(new AdministrationAssistant() { FirstName = "Admin" + i, LastName = "Assistant" });
            }
            for (int i = 0; i < 10; i++)
            {
                el.Add(new Driver() { FirstName = "Driver" + i, LastName = "Assistant", UserName = "Driv" + i, Password = "Driv" + i });
            }
            con.Employee.AddRange(el);
            con.SaveChanges();
        }

        private static void InitializeCars()
        {
            LaborContext con = new LaborContext();
            
            AddCar(con, "OG-LA-001");
            AddCar(con, "OG-LA-002");
            AddCar(con, "OG-LA-003");
            AddCar(con, "OG-LA-004");
            AddCar(con, "OG-LA-005");
            AddCar(con, "OG-LA-006");

            con.Car.Find("OG-LA-001").CurrentDriver = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault();
            con.Car.Find("OG-LA-002").CurrentDriver = con.Driver.Where(d => d.UserName == "Driv2").FirstOrDefault();
            var gps = con.GpsPosition.Find("OG-LA-002");
            gps.Latitude =48.4615593f;
            gps.Longitude =7.9511829f;

            try
            {
                con.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        private static void AddCar(LaborContext context, string carID)
        {
            GPSPosition position = new GPSPosition { CarID = carID, Latitude = 48.4580221f, Longitude = 7.9423354f };
            context.GpsPosition.Add(position);
            Car car = new Car() { CarID = carID, CarLogbook = new CarLogbook() { CarId = carID}, Roadworthy = true, LastPosition = position };
            context.Car.Add(car);
        }

        private static void InitializeCustomer()
        {
            LaborContext con = new LaborContext();
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer("Dr.", "House", "house", "asdf", new Address("Hauptstr. 88", "77652", "Offenburg"), "Dr. House imba Werkstatt"));
            customers.Add(new Customer("Alice", "Vette", "vette", "asdf", new Address("Hauptstr. 88", "77652", "Offenburg"), "Vetter Alice Fachärztin für Allgemeinmedizin", new BankAccount("1asdf243ew", "Alice Vette")));
            customers.Add(new Customer("Wolfgang", "Bätz", "lolo", "asdf", new Address("Am Marktplatz 7", "77652", "Offenburg"), "Bätz Wolfgang Dr.med. Gefäßchirurg", new BankAccount("ASDLF23456", "Wolfgang Bätz"), true, "107438570935"));
            customers.Add(new Customer("Michael", "Brake", "holzmichel", "asdf", new Address("Hauptstr. 98", "77652", "Offenburg"), "Brake Michael Dr. med. Arzt für Urologie", new BankAccount("ALKFJ34565768", "Michael Brake"), true, "9347983476"));
            customers.Add(new Customer("Elke", "Brüderle", "Elli", "asdf", new Address("Ebertplatz 12", "77652", "Offenburg"), "Brüderle Elke Dr. Frauenärztin", new BankAccount("LKFJGFG23456", "Brüderle Elke")));
            customers.Add(new Customer("Traunecker", "Ulrich", "ulli", "asdf", new Address("Leutkirchstraße 13", "77723", "Gengenbach"), "Dr. med. Ulrich Traunecker", new BankAccount("UZJH87698347", "Ulrich Traunecker"), true, "379786546"));
            customers.Add(new Customer("Matthias", "Ruff", "ruffi", "asdf", new Address("Hauptstraße 24", "77723", "Gengenbach"), "Dr. med. Matthias Ruff", new BankAccount("HUGZGU87687625", "Matthias Ruff")));
            customers.Add(new Customer("Stefan", "Leuthner", "leuti", "asdf", new Address("Hauptstraße 61", "77799", "Ortenberg"), "Herr Dr. med. Stefan Leuthner", new BankAccount("UIGUZ7868", "Leuthners Frau")));
            con.Customer.AddRange(customers);
            con.SaveChanges();
        }

        private static void InitializeDrivers()
        {
            LaborContext con = new LaborContext();
            List<Driver> drivers = new List<Driver>();
            drivers.Add(new Driver
            {
                EmployeeType = EEmployeeType.TypeDriver,
                FirstName = "Ole",
                LastName = "Berger",
                Password = "o",
                UserName = "Ole"
            });
            con.Driver.AddRange(drivers);
            con.SaveChanges();
        }
    }
}
