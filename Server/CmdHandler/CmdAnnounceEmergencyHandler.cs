using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using Server.DriverControlling;
using Server.Sms;

namespace Server.CmdHandler
{
    class CmdAnnounceEmergencyHandler : CommandHandler<CmdAnnounceEmergency>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;
        private IDriverController driverController = null;
        private UsernameToConnectionIdMapping driverMapping = null;
        private ISmsSending smsSending = null;
        private LocalServerData serverData = null;

        public CmdAnnounceEmergencyHandler(IServerConnection connection, IDatabaseCommunicator db, IDriverController driverController, UsernameToConnectionIdMapping driverMapping, ISmsSending smsSending, LocalServerData serverData)
        {
            this.connection = connection;
            this.db = db;
            this.driverController = driverController;
            this.driverMapping = driverMapping;
            this.smsSending = smsSending;
            this.serverData = serverData;
        }

        protected override void Handle(CmdAnnounceEmergency command, string connectionIdOrNull)
        {
            bool success = true;

            db.StartTransaction();
            Car car = db.GetCar(command.CarID);
            if (car != null)
            {
                car.Roadworthy = false;
                CarLogbookEntry latestEntry = car.CarLogbook.CarLogbookEntry.LastOrDefault();
                if (latestEntry != null)
                {
                    latestEntry.EndDateOrNull = DateTime.Now;
                }
                if (car.CurrentDriver != null && car.CurrentDriver.UserName.Equals(command.Username))
                {
                    car.CurrentDriver = null;
                }

                // Forward all left unfinished orders to another driver. All to one driver as the destination is all the same.
                IList<Order> leftUnfinishedOrders = db.GetAllOrders(o => o.Driver.UserName.Equals(command.Username) && o.BringDate == null);
                Driver optimalDriverOrNull = driverController.DetermineDriverOrNullInsideTransaction(db, command.DriverGPSPosition);
                foreach (Order o in leftUnfinishedOrders)
                {
                    o.Driver = optimalDriverOrNull;
                    if (o.CollectDate != null)
                    {
                        // Emergency occured after having collected the order.
                        GPSPosition emergencyPosition = db.CreateGPSPosition(command.DriverGPSPosition);
                        o.EmergencyPosition = emergencyPosition;
                        // Update TestStates.
                        if (optimalDriverOrNull != null)
                        {
                            // Employee driver
                            foreach (Test test in o.Test)
                            {
                                test.TestState = TestState.ORDERED;
                            }
                        }
                        else
                        {
                            o.CollectDate = DateTime.Now;
                            foreach (Test test in o.Test)
                            {
                                // Taxi driver
                                test.TestState = TestState.WAITING_FOR_DRIVER;
                            }
                        }
                    }
                    o.CollectDate = null;
                    PushNotificationToDriverOrTaxi(o, optimalDriverOrNull);
                }
            }
            else
            {
                success = false;
            }
            if (car.LastPosition != null)
            {
                car.LastPosition.Latitude = command.DriverGPSPosition.Latitude;
                car.LastPosition.Longitude = command.DriverGPSPosition.Longitude;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);

            CmdReturnAnnounceEmergency response = new CmdReturnAnnounceEmergency(command.Id, success);
            connection.Unicast(response, connectionIdOrNull);
        }

        private void PushNotificationToDriverOrTaxi(Order orderToPush, Driver driverOrNull)
        {
            if (driverOrNull != null)
            {
                string connectionIDOrNull = driverMapping.ResolveConnectionIDOrNull(driverOrNull.UserName);
                if (connectionIDOrNull != null)
                {
                    CmdSendNotification sendNotification = new CmdSendNotification(orderToPush);
                    connection.Unicast(sendNotification, connectionIDOrNull);
                }
            }
            else
            {
                smsSending.Send(serverData.TaxiPhoneNumber,
                    "New order " + orderToPush.OrderID + ". Please collect at " + orderToPush.EmergencyPosition.Latitude +
                    ", " + orderToPush.EmergencyPosition.Longitude + ".");
            }
        }
    }
}
