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
    using System.Linq;
    using System.Text;

	public interface IDatabaseCommunicator 
	{
		List<Analysis> GetAllAnalysis(Func<Analysis, bool> lambda);

		List<ShiftSchedule> GetShiftSchedules();

		List<Order> GetAllOrders(Func<Order, bool> lambda);

		Order GetOrder(long orderID);

		Test GetTest(Guid testID);

		void EndTransaction(TransactionEndOperation operation);

		void StartTransaction();

		void CreateCustomer(Customer customer);

		List<Test> GetAllTests(Func<Test, bool> lambda);

		List<Car> GetAllCars(Func<Car, bool> lambda);

		List<Customer> GetAllCustomer(Func<Customer, bool> lambda);

        List<Employee> GetAllEmployee();

		Customer GetCustomer(string userName);

		Address GetCustomerAddress(string userName);

		void CreateOrder(Order order);

		List<Bill> GetAllBills(Func<Bill, bool> lambda);

		List<DateTime> GetBillDatesOfMonth(DateTime month);

		Bill GetBill(Customer  customer, DateTime date);

        List<Driver> GetAllDriver(Func<Driver, bool> lambda);

		Driver GetDriver(string userName);

		Car GetCarbyDriver(string driverUserName);

		Car GetCar(string carId);

		CarLogbookEntry GetLastCarLogbookEntry(string carID);

	    GPSPosition CreateGPSPosition(GPSPosition position);

		void CreateCarLogbookEnry(CarLogbookEntry entry);

        void CreateShiftSchedule(ShiftSchedule shift);

        void RemoveShiftSchedule(ShiftSchedule shift);

        void AttachAnalysises(List<Analysis> analysises);

	    void AttachOrder(Order order);

	    void CreateBill(Bill bill);

	}
}

