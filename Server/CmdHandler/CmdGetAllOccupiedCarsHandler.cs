using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.Commands;
using Common.Communication.Server;
using Server.DatabaseCommunication;

namespace Server.CmdHandler
{
    class CmdGetAllOccupiedCarsHandler : CommandHandler<CmdGetAllOccupiedCars>
    {
        private IServerConnection _connection = null;
        private IDatabaseCommunicator _db;

        public CmdGetAllOccupiedCarsHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            _db = db;
            _connection = connection;
        }

        protected override void Handle(CmdGetAllOccupiedCars command, string connectionIdOrNull)
        {
            _db.StartTransaction();
            var data = _db.GetAllCars(c => c.CurrentDriver != null);
            _db.EndTransaction(TransactionEndOperation.READONLY);


            var response = new CmdReturnGetAllOccupiedCars(command.Id, data);
            _connection.Unicast(response, connectionIdOrNull);
        }
    }
}
