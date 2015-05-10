using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication.Server;
using Common.Commands;
using Server.CmdHandler;
using Server.DatabaseCommunication;
using Server.Timer;

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
            connection.ServerStarted += (object sender, EventArgs e) => OnServerStarted(connection, db, data);

            Console.WriteLine("Registering Handlers...");
            RegisterHandlers(connection, db, data);

            Console.WriteLine("Starting server...");
            connection.RunForever();
        }

        private static void RegisterHandlers(
            ServerConnection connection,
            IDatabaseCommunicator db,
            LocalServerData data)
        {

            // TODO: REGISTER SERVER HANDLER HERE
            // Register all command handler to the connection here.
            connection.RegisterCommandHandler(new CmdLoginDriverHandler(connection));
            connection.RegisterCommandHandler(new CmdGetAvailableCarsHandler(connection));
            connection.RegisterCommandHandler(new CmdGetDriversUnfinishedOrdersHandler(connection));
            connection.RegisterCommandHandler(new CmdRegisterCustomerHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetAllBillsOfUserHandler(connection));
            connection.RegisterCommandHandler(new CmdGenerateShiftScheduleHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetAllCustomersHandler(connection, db));
        }

        private static void OnServerStarted(
            IServerConnection connection,
            IDatabaseCommunicator db,
            LocalServerData data)
        {
            data.GenerateShiftScheduleTimer = new GenerateShiftScheduleTimer(connection);
            connection.InjectInternal(new CmdGenerateShiftSchedule(GenerateMonthMode.IMMEDIATELY_CURRENT_MONTH));
        }
    }
}
