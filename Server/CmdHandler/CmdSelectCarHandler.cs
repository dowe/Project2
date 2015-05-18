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
    class CmdSelectCarHandler : CommandHandler<CmdSelectCar>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdSelectCarHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdSelectCar command, string connectionIdOrNull)
        {
            bool success = true;

            db.StartTransaction();
            Car car = db.GetCar(command.SelectedCarId);
            if (car.CurrentDriver == null && car.Roadworthy)
            {
                Driver newDriver = db.GetDriver(command.Username);
                car.CurrentDriver = newDriver;
                if (car.CarLogbook == null)
                {
                    car.CarLogbook = new CarLogbook();
                }
                car.CarLogbook.CarLogbookEntry.Add(new CarLogbookEntry
                {
                    Driver = newDriver,
                    StartDate = DateTime.Now,
                    StartKM = command.StartKm,
                    EndDateOrNull = null,
                    EndKMOrNull = null
                });
            }
            else
            {
                success = false;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);
            
            CmdReturnSelectCar response = new CmdReturnSelectCar(command.Id, success);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
