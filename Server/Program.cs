using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication.Server;
using Common.Commands;
using Server.CmdHandler;
using Server.DatabaseCommunication;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Listens on all addresses.
            // Remember to start the app as admin or a 'Access Denied' exception will be thrown.
            ServerConnection connection = new ServerConnection("http://+:8080");
            connection.ServerStarted += OnServerStarted;

            Console.WriteLine("Registering Handlers...");
            RegisterHandlers(connection);

            Console.WriteLine("Starting server...");
            connection.RunForever();
        }

        private static void RegisterHandlers(ServerConnection connection)
        {
            IDatabaseCommunicator db = new DatabaseCommunicator();
            LocalServerData data = new LocalServerData();

            // TODO: REGISTER SERVER HANDLER HERE
            // Register all command handler to the connection here.
            connection.RegisterCommandHandler(new CmdLoginDriverHandler(connection));
            connection.RegisterCommandHandler(new CmdGetAvailableCarsHandler(connection));
            connection.RegisterCommandHandler(new CmdGetDriversUnfinishedOrdersHandler(connection));
            connection.RegisterCommandHandler(new CmdRegisterCustomerHandler(connection, db, data));
            connection.RegisterCommandHandler(new CmdGetAllBillsOfUserHandler(connection));
            connection.RegisterCommandHandler(new CmdGenerateShiftScheduleHandler(connection, db, data));
        }

        private static void OnServerStarted(object sender, EventArgs e)
        {
            
        }
    }
}
