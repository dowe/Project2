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
    public class CmdGetAnalysesHandler : CommandHandler<CmdGetAnalyses>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db;
        public CmdGetAnalysesHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetAnalyses command, string connectionIdOrNull)
        {
            db.StartTransaction();
            var analyses = db.GetAllAnalysis(null);
            db.EndTransaction(TransactionEndOperation.READONLY);
            
            var response = new CmdReturnGetAnalyses(command.Id, analyses); //TODO: analysis
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
