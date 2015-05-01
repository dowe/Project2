using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication.Server;
using Common.Commands;
using Server.CmdHandler;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Listens on all addresses.
            // Remember to start the app as admin or a 'Access Denied' exception will be thrown.
            ServerConnection connection = new ServerConnection("http://+:8080");

            Console.WriteLine("Registering Handlers...");
            RegisterHandlers(connection);

            Console.WriteLine("Starting server...");
            connection.RunForever();
        }

        private static void RegisterHandlers(ServerConnection connection)
        {
            // TODO
            // Register all command handler to the connection here.
           
            connection.RegisterCommandHandler(new CmdRegisterCustomerHandler());
        }
    }
}
