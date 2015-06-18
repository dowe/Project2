using Common.Commands;
using Common.Communication.Client;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Send
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            IClientConnection c = null;
            CmdReturnSetOrderCollected response;

            do
            {
                string user = Console.ReadLine();
                long l;
                long.TryParse(Console.ReadLine(), out l);
                if (c == null)
                {
                    c = new ClientConnection("http://localhost:8080/commands");
                    c.Start();
                    c.Connect();
                }
                CmdSetOrderCollected request = new CmdSetOrderCollected(user, l);
                response = c.SendWait<CmdReturnSetOrderCollected>(request);
                Console.WriteLine(Util.ToString(response));
            } while (true);
        }
    }
}
