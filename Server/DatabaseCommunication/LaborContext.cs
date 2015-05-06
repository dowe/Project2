using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DatabaseCommunication
{
    public class LaborContext : DbContext
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<AdministrationAssistant> AdministrationAssistant { get; set; }
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Customer> Customer { get; set;}
        public DbSet<DailyStatistic> DailyStatistic { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<ShiftSchedule> ShiftSchedule { get; set; }
        public DbSet<Test> Test { get; set; }
    }
}
