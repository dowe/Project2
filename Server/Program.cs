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
            ServerConnection connection = new ServerConnection("http://localhost:8080");

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
