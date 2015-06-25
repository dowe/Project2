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
            float minKm = 0;

            db.StartTransaction();
            Car car = db.GetCar(command.SelectedCarId);
            if (car.CurrentDriver == null)
            {
                if (car.CarLogbook.CarLogbookEntry.Count != 0)
                {
                    var lastEntry = car.CarLogbook.CarLogbookEntry.Last();
                    if (lastEntry.EndKMOrNull.HasValue)
                    {
                        minKm = lastEntry.EndKMOrNull.Value;
                    }
                    else
                    {
                        minKm = lastEntry.StartKM;
                    }
                }
                else
                {
                    minKm = 0;
                }
                if (command.StartKm >= minKm)
                {
                    Driver newDriver = db.GetDriver(command.Username);
                    car.Roadworthy = true;
                    car.CurrentDriver = newDriver;
                    var entry = new CarLogbookEntry
                    {
                        Driver = newDriver,
                        StartDate = DateTime.Now,
                        StartKM = command.StartKm,
                        EndDateOrNull = null,
                        EndKMOrNull = null
                    };
                    db.CreateCarLogbookEnry(entry);
                    car.CarLogbook.CarLogbookEntry.Add(entry);
                }
                else
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);

            CmdReturnSelectCar response = new CmdReturnSelectCar(command.Id, success, minKm);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
