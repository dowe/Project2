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

namespace Server.CmdHandler
{
    public class CmdGetAllBillsOfUserHandler : CommandHandler<CmdGetAllBillsOfUser>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db;

        public CmdGetAllBillsOfUserHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetAllBillsOfUser command, string connectionIdOrNull)
        {
            List<Bill> billsRaw = new List<Bill>();
            if ("Ole".Equals(command.Username))
            {
                billsRaw = new List<Bill>()
            {
                new Bill(){Customer = null, Date = new DateTime(123,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},
                new Bill(){Customer = null, Date = new DateTime(133,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},
                new Bill(){Customer = null, Date = new DateTime(144,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},
                new Bill(){Customer = null, Date = new DateTime(155,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},

            };
            }

            var response = new CmdReturnGetAllBillsOfUser(command.Id, billsRaw);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
