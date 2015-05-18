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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayEntry>().
              HasMany(d => d.AM).
              WithMany().Map(
               m =>
               {
                   m.MapLeftKey("ShiftDate");
                   m.MapRightKey(new string[]{"EmployeeLastName","EmployeeFirstName"});
                   m.ToTable("AMShiftEmployees");
               });

            modelBuilder.Entity<DayEntry>().
             HasMany(d => d.PM).
             WithMany().Map(
              m =>
              {
                  m.MapLeftKey("ShiftDate");
                  m.MapRightKey(new string[] { "EmployeeLastName", "EmployeeFirstName" });
                  m.ToTable("PMShiftEmployees");
              });


        }

        public DbSet<AdministrationAssistant> AdministrationAssistant { get; set; }
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<GPSPosition> GpsPosition { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<DailyStatistic> DailyStatistic { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<ShiftSchedule> ShiftSchedule { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<Driver> Driver { get; set; }
    }
}
