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
            InitializeCustomer();
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
            List<Car> cars = new List<Car>();
            cars.Add(new Car() { CarID = "OG-LA-001", CarLogbook = new CarLogbook() { CarId = "OG-LA-001" }, Roadworthy = true });
            cars.Add(new Car() { CarID = "OG-LA-002", CarLogbook = new CarLogbook() { CarId = "OG-LA-002" }, Roadworthy = true });
            cars.Add(new Car() { CarID = "OG-LA-003", CarLogbook = new CarLogbook() { CarId = "OG-LA-003" }, Roadworthy = true });
            cars.Add(new Car() { CarID = "OG-LA-004", CarLogbook = new CarLogbook() { CarId = "OG-LA-004" }, Roadworthy = true });
            cars.Add(new Car() { CarID = "OG-LA-005", CarLogbook = new CarLogbook() { CarId = "OG-LA-005" }, Roadworthy = true });
            cars.Add(new Car() { CarID = "OG-LA-006", CarLogbook = new CarLogbook() { CarId = "OG-LA-006" }, Roadworthy = true });
            con.Car.AddRange(cars);

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

        private static void InitializeCustomer()
        {
            LaborContext con = new LaborContext();
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer() { UserName = "house", Password = "asdf", Address = new Address() { Street = "Am Arsch der Welt", PostalCode = "12345", City = "Springfield" } });
            con.Customer.AddRange(customers);
            con.SaveChanges();
        }
    }
}
