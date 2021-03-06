﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Wenn der Code neu generiert wird, gehen alle Änderungen an dieser Datei verloren
// </auto-generated>
//------------------------------------------------------------------------------
namespace Server.DatabaseCommunication
{
    using Common.DataTransferObjects;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    public class DatabaseCommunicator : IDatabaseCommunicator
    {

        private LaborContext Context;
        private List<Analysis> Analysises;
        public List<Analysis> GetAllAnalysis(Func<Analysis, bool> lambda = null)
        {
            if (lambda != null)
            {
                return Context.Analysis.Where(lambda).ToList();
            }
            return Context.Analysis.ToList();

        }

        public List<ShiftSchedule> GetShiftSchedules()
        {
            return Context.ShiftSchedule.ToList();
        }

        public List<Order> GetAllOrders(Func<Order, bool> lambda)
        {
            if (lambda != null)
            {
                return Context.Order.Where(lambda).ToList();
            }
            return Context.Order.ToList();
        }

        public Order GetOrder(long orderID)
        {
            return Context.Order.Find(orderID);
        }

        public Test GetTest(Guid testID)
        {
            return Context.Test.Find(testID);
        }

        public void EndTransaction(TransactionEndOperation operation)
        {
            if (Context == null)
            {
                throw new Exception("no transaction started, when trying to end transaction");
            }
            if (operation == TransactionEndOperation.SAVE)
            {
                try
                {
                    Context.SaveChanges();
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
            Context.Dispose();
            Context = null;
            Analysises = null;
        }

        public void StartTransaction()
        {
            if (Context != null)
            {
                throw new Exception("Transaction is already started");
            }
            Context = new LaborContext();
            Analysises = new List<Analysis>();
        }

        public void CreateCustomer(Customer customer)
        {
            Context.Customer.Add(customer);
        }

        public List<Test> GetAllTests(Func<Test, bool> lambda)
        {
            if (lambda != null)
            {
                return Context.Test.Where(lambda).ToList();
            }
            return Context.Test.ToList();
        }

        public List<Car> GetAllCars(Func<Car, bool> lambda)
        {
            if (lambda != null)
            {
                return Context.Car.Where(lambda).ToList();
            }
            return Context.Car.ToList();
        }

        public List<Customer> GetAllCustomer(Func<Customer, bool> lambda)
        {
            if (lambda != null)
            {
                return Context.Customer.Where(lambda).ToList();
            }
            return Context.Customer.ToList();
        }

        public Customer GetCustomer(string userName)
        {
            return Context.Customer.Find(userName);
        }

        public Address GetCustomerAddress(string userName)
        {
            return Context.Customer.Find(userName).Address;
        }

        public void CreateOrder(Order order)
        {
            Context.Order.Add(order);
        }

        public List<Bill> GetAllBills(Func<Bill, bool> lambda)
        {
            if (lambda != null)
            {
                return Context.Bill.Where(lambda).ToList();
            }
            return Context.Bill.ToList();
        }

        public List<DateTime> GetBillDatesOfMonth(DateTime month)
        {
            throw new NotImplementedException();
        }

        public Bill GetBill(Customer customer, DateTime date)
        {
            return Context.Bill.Find(customer, date);
        }

        public List<Driver> GetAllDriver(Func<Driver, bool> lambda)
        {
            if (lambda != null)
            {
                return Context.Driver.Where(lambda).ToList();
            }
            return Context.Driver.ToList();
        }

        public Driver GetDriver(string userName)
        {
            return Context.Driver.Where(d => d.UserName.Equals(userName)).FirstOrDefault();
        }

        public Car GetCar(string CarID)
        {
            return Context.Car.Find(CarID);
        }

        public Car GetCarbyDriver(string driverUserName)
        {
            return Context.Car.Where(c => c.CurrentDriver.UserName == driverUserName).FirstOrDefault();
        }

        public CarLogbookEntry GetLastCarLogbookEntry(string carID)
        {
            return Context.Car.Find(carID).CarLogbook.CarLogbookEntry.FirstOrDefault();
        }

        public void CreateCarLogbookEnry(CarLogbookEntry entry)
        {
            Context.CarLogbookEntries.Add(entry);
        }

        public GPSPosition CreateGPSPosition(GPSPosition position)
        {
            return Context.GpsPosition.Add(position);
        }

        public void CreateShiftSchedule(ShiftSchedule shift)
        {
            Context.ShiftSchedule.Add(shift);
        }

        public void RemoveShiftSchedule(ShiftSchedule shift)
        {
            foreach (DayEntry d in shift.DayEntry)
            {
                Context.Database.ExecuteSqlCommand("DELETE dbo.AMShiftEmployees WHERE ShiftDate = @date", new SqlParameter("@date", d.Date));
                Context.Database.ExecuteSqlCommand("DELETE dbo.PMShiftEmployees WHERE ShiftDate = @date", new SqlParameter("@date", d.Date));
                Context.Database.ExecuteSqlCommand("DELETE dbo.DayEntries WHERE Date = @date", new SqlParameter("@date", d.Date));
            }
            
            Context.Database.ExecuteSqlCommand("DELETE dbo.ShiftSchedules WHERE Date = @date", new SqlParameter("@date", shift.Date));
        }


        public void AttachAnalysises(List<Analysis> analysises)
        {
            List<Analysis> listtorem = new List<Analysis>();
            List<Analysis> listtoadd = new List<Analysis>();

            foreach (Analysis an in analysises)
            {
                Analysis found = this.Analysises.Find((a) => a.Name == an.Name);
                if (found == null)
                {
                    Context.Analysis.Attach(an);
                    Analysises.Add(an);
                }
                else
                {
                    listtorem.Add(an);
                    listtoadd.Add(found);
                }
            }
            foreach (Analysis an in listtorem)
            {
                analysises.Remove(an);
            }
            analysises.AddRange(listtoadd);
        }

        public void AttachOrder(Order order)
        {
            Context.Order.Attach(order);
        }

        public void CreateBill(Bill bill)
        {
            Context.Bill.Add(bill);
        }


        public List<Employee> GetAllEmployee()
        {
            return Context.Employee.ToList();
        }
    }
}

