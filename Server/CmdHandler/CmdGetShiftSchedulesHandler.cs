using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.CmdHandler
{
    public class CmdGetShiftSchedulesHandler : CommandHandler<CmdGetShiftSchedules>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetShiftSchedulesHandler(
            IServerConnection connection, 
            IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetShiftSchedules request, string connectionIdOrNull)
        {
       
            IList<ShiftSchedule> list;

            db.StartTransaction();
            list = db.GetShiftSchedules().ToList<ShiftSchedule>();
            
            
            ShiftSchedule currentMonth = null;
            ShiftSchedule nextMonth = null;
            DateTime current = DateTime.Now;
            DateTime next = current.AddMonths(1);

            foreach (ShiftSchedule obj in list)
            {
                if ( currentMonth == null 
                    && obj.Date.Month == current.Month 
                    && obj.Date.Year == current.Year) {
                        currentMonth = obj;
                }
                if (nextMonth == null
                    && obj.Date.Month == next.Month
                    && obj.Date.Year == next.Year)
                {
                    nextMonth = obj;
                }
            }

            ShiftSchedule[] array = {currentMonth, nextMonth};

            ResponseCommand response = new CmdReturnGetShiftSchedule(request.Id, array);
            connection.Unicast(response, connectionIdOrNull);
            db.EndTransaction(TransactionEndOperation.READONLY);
        }
    }
}
