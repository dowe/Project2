﻿using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.DistanceCalculation;

namespace DatabaseInitialize
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize();
            RemoveAllBills();
        }

        private static void RemoveAllBills()
        {
            try
            {
                string workingDir = Directory.GetCurrentDirectory();
                Console.WriteLine("INFO: Running from " + workingDir + " (works only if you run it from the bin/debug directory).");
                DirectoryInfo appData = new DirectoryInfo("../../../ASPServer/App_Data");
                if (appData.Exists)
                {
                    foreach (DirectoryInfo dir in appData.GetDirectories())
                    {
                        dir.Delete(true);
                        Console.WriteLine("INFO: " + dir.FullName + " deleted");
                    }
                }
                else
                {
                    Console.WriteLine("WARNING: ASPServer/App_Data directory does not exist or was not found.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Deleting failed (" + e.Message + ").");
            }
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
            //InitializeOrdersForDailyStatistics();
        }

        private static void InitializeOrdersForDailyStatistics()
        {
            LaborContext con = new LaborContext();
            List<Order> orders = new List<Order>();


            //Start Creating a Mockorder for testing
            Order MockOrder1 = new Order();
            MockOrder1.CollectDate = DateTime.Now;
            MockOrder1.Invoiced = false;
            Customer MockCustomer = new Customer();
            MockCustomer.UserName = "myUsername";
            MockCustomer.LastName = "Müller";
            MockCustomer.FirstName = "Hans";

            MockCustomer.BankAccount = new BankAccount("DE 2323 1212 3333 1111", "Hans Müller");
            MockCustomer.Address = new Address("Kurzestr", "3333", "Offenburg");

            MockCustomer.TwoWayRoadCostInEuro = 42.11f;
            MockOrder1.Customer = MockCustomer;
            Analysis Anal1 = new Analysis("Blutzeug", 1.0f, 11.0f, "Liter", 42.42f, SampleType.BLOOD);
            Analysis Anal2 = new Analysis("Spermazeug", 5.0f, 22.0f, "Kilo", 16.20f, SampleType.SPERM);
            Analysis Anal3 = new Analysis("Urinzeug", 2.0f, 3.0f, "Liter", 4.20f, SampleType.URINE);

            MockOrder1.OrderID = 1111;
            List<Test> MockTest = new List<Test>();
            MockTest.Add(new Test("Patientenid123", Anal1));
            MockTest.Add(new Test("Patientenid123", Anal2));
            MockTest.Add(new Test("Patientenid123", Anal3));
            MockOrder1.CompleteDate = DateTime.Now;
            MockOrder1.Test = MockTest;
            MockOrder1.Test[0].EndDate = DateTime.Now;
            MockOrder1.Test[1].EndDate = DateTime.Now;
            MockOrder1.Test[2].EndDate = DateTime.Now;



            orders.Add(MockOrder1);

            //Zweite MockOrder
            Order MockOrder2 = new Order();
            MockOrder2.CollectDate = DateTime.Now;
            MockOrder2.OrderDate = DateTime.Now.AddDays(-1);


            MockOrder2.Invoiced = false;
            Customer MockCustomer2 = new Customer();
            MockCustomer2.UserName = "otherUsername";
            MockCustomer2.LastName = "Maier";
            MockCustomer2.FirstName = "Peter";

            MockCustomer2.BankAccount = new BankAccount("DE 2323 1212 XXXX 1111", "Maier Peter");
            MockCustomer2.Address = new Address("Langestr", "3333", "Offenburg");
            MockCustomer2.TwoWayRoadCostInEuro = 69.96f;
            MockOrder2.Customer = MockCustomer2;

            MockOrder2.OrderID = 2222;
            List<Test> MockTest2 = new List<Test>();
            MockTest2.Add(new Test("Patientenid333", Anal1));
            MockTest2.Add(new Test("Patientenid222", Anal2));

            MockOrder2.Test = MockTest2;

            orders.Add(MockOrder2);

            //Start MockOrder 3, same customer as 1
            Order MockOrder3 = new Order();
            MockOrder3.CollectDate = DateTime.Now;
            MockOrder3.Invoiced = false;
            MockOrder3.Customer = MockCustomer;

            MockOrder3.OrderID = 333;
            List<Test> MockTest3 = new List<Test>();


            MockTest3.Add(new Test("Patientenid1223", Anal1));
            MockTest3.Add(new Test("Patientenid1223", Anal2));
            MockTest3.Add(new Test("Patientenid1223", Anal3));
            MockOrder3.Test = MockTest3;
            orders.Add(MockOrder3);
            //finish Mockorder
            con.Order.AddRange(orders);
            con.SaveChanges();
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
            orders.Add(new Order()
           {
               OrderDate = DateTime.Now,
               Driver = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault(),
               

               Invoiced = false,
               RemindedAfterFiveHours = false,
               Customer = con.Customer.Where(c => c.UserName == "holzmichel").FirstOrDefault(),

               Test = new List<Test>()
                {
            new Test("Hypochonda", con.Analysis.Find("Stuhl_Candida"))
                    {
                        
                        TestState = TestState.WAITING_FOR_DRIVER,
                        
                    }
                }
           });

            orders.Add(new Order(anal, con.Customer.Where(c => c.UserName == "holzmichel").FirstOrDefault())
            {
                OrderDate = DateTime.Now,
            });
            orders.Add(new Order()
            {
                OrderDate = DateTime.Now,
                Driver = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault(),
                BringDate = DateTime.Now,
                CollectDate = DateTime.Now,
                CompleteDate = DateTime.Now,

                Invoiced = false,
                RemindedAfterFiveHours = false,
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
                      new Test("NochNPatient", con.Analysis.Find("Urin_Albumin"))
                    {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        ResultValue = 15f,
                        AlarmState = AlarmState.NO_ALARM,
                        Critical = false,
                        TestState = TestState.COMPLETED
                    },
                    new Test("NochNPatient", con.Analysis.Find("Stuhl_Candida"))
                    {
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        ResultValue = 11000f,
                        AlarmState = AlarmState.FIRST_ALARM_TRANSMITTED,
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
            orders.Add(new Order(anal, con.Customer.Where(c => c.UserName.Equals("house")).FirstOrDefault())
            {
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromHours(5)),
                Driver = con.Driver.Where(d => d.UserName == "Driv3").FirstOrDefault()
            });
            orders.Add(new Order(anal, con.Customer.Where(c => c.UserName.Equals("house")).FirstOrDefault())
            {
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromHours(3)),
                Driver = con.Driver.Where(d => d.UserName == "Driv3").FirstOrDefault()
            });
            orders.Add(new Order(anal, con.Customer.Where(c => c.UserName.Equals("house")).FirstOrDefault())
            {
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromHours(5)),
                Driver = con.Driver.Where(d => d.UserName == "Driv3").FirstOrDefault(),
            });
            orders.Add(new Order()
            {
                OrderDate = DateTime.Now,
                Driver = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault(),
                CollectDate = DateTime.Now,

                Invoiced = false,
                RemindedAfterFiveHours = false,
                Customer = con.Customer.Where(c => c.UserName == "Elli").FirstOrDefault(),

                Test = new List<Test>()
                {
                    new Test("Peterle", con.Analysis.Find("Blut_Hämoglobin"))
                    {
                        TestState = TestState.WAITING_FOR_DRIVER
                    },
                    new Test("Peterle", con.Analysis.Find("Urin_Gewicht"))
                    {
                         TestState = TestState.WAITING_FOR_DRIVER

                    },
                      new Test("Hansi", con.Analysis.Find("Urin_Albumin"))
                    {
                         TestState = TestState.WAITING_FOR_DRIVER

                    },
                    new Test("Hansi", con.Analysis.Find("Stuhl_Candida"))
                    {
                        TestState = TestState.WAITING_FOR_DRIVER
                    },
                   
                }
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

            var driver1 = con.Driver.Where(d => d.UserName == "Driv1").FirstOrDefault();
            var car1 = con.Car.Find("OG-LA-001");
            car1.CurrentDriver = driver1;
            var entry1 = new CarLogbookEntry
            {
                Driver = driver1,
                StartDate = DateTime.Now.Subtract(TimeSpan.FromHours(3)),
                StartKM = 2
            };
            con.CarLogbookEntries.Add(entry1);
            car1.CarLogbook.CarLogbookEntry.Add(entry1);

            var driver2 = con.Driver.Where(d => d.UserName == "Driv2").FirstOrDefault();
            var car2 = con.Car.Find("OG-LA-002");
            car2.CurrentDriver = driver2;
            var entry2 = new CarLogbookEntry
            {
                Driver = driver2,
                StartDate = DateTime.Now.Subtract(TimeSpan.FromHours(3)),
                StartKM = 2
            };
            con.CarLogbookEntries.Add(entry2);
            car2.CarLogbook.CarLogbookEntry.Add(entry2);

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
            GPSPosition position = new GPSPosition { Latitude = 48.4580221f, Longitude = 7.9423354f };
            context.GpsPosition.Add(position);
            Car car = new Car() { CarID = carID, CarLogbook = new CarLogbook() { CarId = carID, CarLogbookEntry = new List<CarLogbookEntry>() }, Roadworthy = true, LastPosition = position };
            context.Car.Add(car);
        }

        private static void InitializeCustomer()
        {
            LaborContext con = new LaborContext();
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer("Dr.", "House", "house", "asdf", new Address("Hauptstr. 88", "77652", "Offenburg"), "Dr. House imba Werkstatt", new BankAccount("SDLFKJSDLKFJ", "Dr. House")) { TwoWayRoadCostInEuro = 11.11f, GpsPosition = new GPSPosition(48.46973f, 7.94229f) });
            customers.Add(new Customer("Alice", "Vette", "vette", "asdf", new Address("Hauptstr. 88", "77652", "Offenburg"), "Vetter Alice Fachärztin für Allgemeinmedizin", new BankAccount("1asdf243ew", "Alice Vette")) { TwoWayRoadCostInEuro = 1.11f, GpsPosition = new GPSPosition(48.46973f, 7.94229f) });
            customers.Add(new Customer("Wolfgang", "Bätz", "lolo", "asdf", new Address("Am Marktplatz 7", "77652", "Offenburg"), "Bätz Wolfgang Dr.med. Gefäßchirurg", new BankAccount("ASDLF23456", "Wolfgang Bätz"), true, "107438570935") { TwoWayRoadCostInEuro = 2.11f, GpsPosition = new GPSPosition(48.4696894f, 7.9417479f) });
            customers.Add(new Customer("Michael", "Brake", "holzmichel", "asdf", new Address("Hauptstr. 98", "77652", "Offenburg"), "Brake Michael Dr. med. Arzt für Urologie", new BankAccount("ALKFJ34565768", "Michael Brake"), true, "017655524473") { TwoWayRoadCostInEuro = 44.11f, GpsPosition = new GPSPosition(48.46874f, 7.94229f) });
            customers.Add(new Customer("Elke", "Brüderle", "Elli", "asdf", new Address("Ebertplatz 12", "77652", "Offenburg"), "Brüderle Elke Dr. Frauenärztin", new BankAccount("LKFJGFG23456", "Brüderle Elke")) { TwoWayRoadCostInEuro = 1.11f, GpsPosition = new GPSPosition(48.47598f, 7.95548f) });
            customers.Add(new Customer("Traunecker", "Ulrich", "ulli", "asdf", new Address("Leutkirchstraße 13", "77723", "Gengenbach"), "Dr. med. Ulrich Traunecker", new BankAccount("UZJH87698347", "Ulrich Traunecker"), true, "379786546") { TwoWayRoadCostInEuro = 0.11f, GpsPosition = new GPSPosition(48.40413f, 8.01221f) });
            customers.Add(new Customer("Matthias", "Ruff", "ruffi", "asdf", new Address("Hauptstraße 24", "77723", "Gengenbach"), "Dr. med. Matthias Ruff", new BankAccount("HUGZGU87687625", "Matthias Ruff")) { TwoWayRoadCostInEuro = 5.11f, GpsPosition = new GPSPosition(48.404f, 8.01388f) });
            customers.Add(new Customer("Stefan", "Leuthner", "leuti", "asdf", new Address("Hauptstraße 61", "77799", "Ortenberg"), "Herr Dr. med. Stefan Leuthner", new BankAccount("UIGUZ7868", "Leuthners Frau")) { TwoWayRoadCostInEuro = 6.11f, GpsPosition = new GPSPosition(48.44926f, 7.97155f) });
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
