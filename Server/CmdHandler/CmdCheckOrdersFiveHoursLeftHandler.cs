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
    class CmdCheckOrdersFiveHoursLeftHandler : CommandHandler<CmdCheckOrdersFiveHoursLeft>
    {

        private readonly TimeSpan TIME_BEFORE_REMINDER = TimeSpan.FromHours(5);

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;
        private UsernameToConnectionIdMapping driverMapping = null;

        public CmdCheckOrdersFiveHoursLeftHandler(IServerConnection connection, IDatabaseCommunicator db, UsernameToConnectionIdMapping driverMapping)
        {
            this.connection = connection;
            this.db = db;
            this.driverMapping = driverMapping;
        }


        protected override void Handle(CmdCheckOrdersFiveHoursLeft command, string connectionIdOrNull)
        {
            db.StartTransaction();

            var timeCriticalOrders =
                db.GetAllOrders(CheckNeedsReminder);

            foreach (Order o in timeCriticalOrders)
            {
                string driverConnectionIDOrNull = driverMapping.ResolveConnectionIDOrNull(o.Driver.UserName);
                if (driverConnectionIDOrNull != null)
                {
                    var remindDriverOfOrder = new CmdRemindDriverOfOrder(o.OrderID);
                    connection.Unicast(remindDriverOfOrder, driverConnectionIDOrNull);
                }
                o.RemindedAfterFiveHours = true;
            }

            db.EndTransaction(TransactionEndOperation.SAVE);
        }

        private bool CheckNeedsReminder(Order o)
        {
            return o.CollectDate == null
                && o.OrderDate != null
                && DateTime.Now.Subtract(o.OrderDate.GetValueOrDefault()).TotalSeconds > TIME_BEFORE_REMINDER.TotalSeconds
                && o.Driver != null
                && !o.RemindedAfterFiveHours;
        }
    }
}
