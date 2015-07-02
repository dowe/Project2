using Common.Commands;
using Common.Communication.Client;
using Common.Util;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Send
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main()
        {
            IClientConnection c = null;

            do
            {
                string command = Console.ReadLine();

                if (c == null)
                {
                    c = new ClientConnection("http://localhost:8080/commands");
                    c.Start();
                    c.Connect();
                }

                switch (command)
                {
                    case "SHIFT":
                        SendShift(c);
                        break;
                    case "COLLECTED":
                        SendCollected(c);
                        break;

                    default:
                        Console.WriteLine("Unknown Command <{0}>", command);
                        break;
                }
                
            } while (true);
        }

        private static void SendCollected(IClientConnection c)
        {
            Console.WriteLine("ORDER-ID: ");
            long orderId;
            long.TryParse(Console.ReadLine(), out orderId);

            CmdSetOrderCollected request = new CmdSetOrderCollected(orderId);
            CmdReturnSetOrderCollected response = c.SendWait<CmdReturnSetOrderCollected>(request);
            Console.WriteLine(Util.ToString(response));
        }

        private static void SendShift(IClientConnection c)
        {
            Console.WriteLine("MONTH (1-12): ");
            int month;
            int.TryParse(Console.ReadLine(), out month);

            CmdGenerateShiftSchedule request = new CmdGenerateShiftSchedule(new DateTime(DateTime.Now.Year, month, 1));
            c.Send(request);
            Console.WriteLine("SUCCESS");
        }
    }
}
