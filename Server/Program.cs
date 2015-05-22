﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication.Server;
using Common.Commands;
using Server.CmdHandler;
using Server.DatabaseCommunication;
using Server.DistanceCalculation;
using Server.DriverControlling;
using Server.Sms;
using Server.Timer;
using Common.DataTransferObjects;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            // Listens on all addresses.
            // Remember to start the app as admin or a 'Access Denied' exception will be thrown.
            ServerConnection connection = new ServerConnection("http://+:8080");
            IDatabaseCommunicator db = new DatabaseCommunicator();
            LocalServerData data = new LocalServerData();
            DriverControllerSettings driverControllerSettings =
                new DriverControllerSettings(new DistanceMatrixAddress(data.ZmsAddress), TimeSpan.FromHours(6));
            IDriverController driverController = new DriverControlling.DriverController(driverControllerSettings);
            UsernameToConnectionIdMapping driverToConnectionIdMapping = new UsernameToConnectionIdMapping();
            ISmsSending smsSending = new SmsSending();

            connection.ServerStarted += (object sender, EventArgs e) => OnServerStarted(connection, db, data);
            connection.BeforeHandlingCommand += connection_BeforeHandlingCommand;

            Console.WriteLine("Registering Handlers...");
            RegisterHandlers(connection, db, data, driverController, driverToConnectionIdMapping, smsSending);

            Console.WriteLine("Starting server...");
            connection.RunForever();
        }

        static void connection_BeforeHandlingCommand(Common.Communication.Command obj)
        {
            Console.WriteLine("Handling {0}.", obj.GetType());
        }

        private static void RegisterHandlers(
            ServerConnection connection,
            IDatabaseCommunicator db,
            LocalServerData data,
            IDriverController driverController,
            UsernameToConnectionIdMapping driverMapping,
            ISmsSending smsSending)
        {

            // TODO: REGISTER SERVER HANDLER HERE
            // Register all command handler to the connection here.
            connection.RegisterCommandHandler(new CmdLoginDriverHandler(connection, db, driverMapping));
            connection.RegisterCommandHandler(new CmdLoginCustomerHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGetShiftSchedulesHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGetAvailableCarsHandler(connection, db));
            connection.RegisterCommandHandler(new CmdSelectCarHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGetDriversUnfinishedOrdersHandler(connection, db));
            connection.RegisterCommandHandler(new CmdCheckOrdersFiveHoursLeftHandler(connection, db, driverMapping));
            connection.RegisterCommandHandler(new CmdSetOrderCollectedHandler(connection, db));
            connection.RegisterCommandHandler(new CmdStoreDriverGPSPositionHandler(db));
            connection.RegisterCommandHandler(new CmdAnnounceEmergencyHandler(connection, db, driverController,
                driverMapping, smsSending, data));
            connection.RegisterCommandHandler(new CmdLogoutDriverHandler(connection, db, driverMapping));
            connection.RegisterCommandHandler(new CmdRegisterCustomerHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetAllBillsOfUserHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGenerateShiftScheduleHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetAllCustomersHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGetAllOrdersHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGetUsersOrderResultsHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGenerateDailyStatisticHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetDailyStatisticHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetAnalysesHandler(connection, db));
            connection.RegisterCommandHandler(new CmdAddOrderHandler(connection, db, driverController, driverMapping));
            connection.RegisterCommandHandler(new CmdGetCustomerAddressHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGenerateBillsHandler(connection, db));
            connection.RegisterCommandHandler(new CmdGetAllOccupiedCarsHandler(connection, db));
            connection.RegisterCommandHandler(new CmdSetOrderReceivedHandler(db));
        }

        private static void OnServerStarted(
            IServerConnection connection,
            IDatabaseCommunicator db,
            LocalServerData data)
        {
            data.GenerateShiftScheduleTimer = new GenerateShiftScheduleTimer(connection);
            data.CheckOrdersFiveHoursLeftScheduledTimer = new CheckOrdersFiveHoursLeftScheduledTimer(connection);
           // connection.InjectInternal(new CmdGenerateShiftSchedule(GenerateMonthMode.IMMEDIATELY_CURRENT_MONTH));
            connection.InjectInternal(new CmdGenerateDailyStatistic());
            connection.InjectInternal(new CmdGenerateBills());
        }
    }
}
