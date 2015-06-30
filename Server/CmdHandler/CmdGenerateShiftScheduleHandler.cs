using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Common.Util;
using Server.DatabaseCommunication;
using Server.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.ShiftScheduleCreation;

namespace Server.CmdHandler
{
    public class CmdGenerateShiftScheduleHandler : CommandHandler<CmdGenerateShiftSchedule>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;
        private ILocalServerData data;
        private IShiftScheduleCreator creator;

        public CmdGenerateShiftScheduleHandler(
            IServerConnection connection,
            IDatabaseCommunicator db,
            ILocalServerData data, 
            IShiftScheduleCreator creator)
        {
            this.creator = creator;
            this.connection = connection;
            this.db = db;
            this.data = data;
        }

        protected override void Handle(CmdGenerateShiftSchedule command, string connectionIdOrNull)
        {
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime previousMonth = currentMonth.AddMonths(-1);
            ShiftSchedule previousShiftSchedule = new ShiftSchedule();

            Remove(db, currentMonth);
            
            //GET PREVIOUS ShiftSchedules and create new
            db.StartTransaction();
            previousShiftSchedule = Get(db.GetShiftSchedules(), previousMonth);
            if (previousShiftSchedule == null)
            {
                previousShiftSchedule = new ShiftSchedule();
            } 

            List<Employee> emps = db.GetAllEmployee();
            if (emps.Count == 0)
            {
                Console.WriteLine("\n   DB not initialized\n   Employee.Count == 0 !!!\n");
                db.EndTransaction(TransactionEndOperation.READONLY);
                return;
            }

            ShiftSchedule cur = creator.createShiftSchedule(previousShiftSchedule, emps, currentMonth);

            //store ShiftSchedule in db
            db.CreateShiftSchedule(cur);
            db.EndTransaction(TransactionEndOperation.SAVE);

            Console.WriteLine("SHIFT_SCHEDULE CREATED FOR <{0}> at <{1}>", cur.Date, DateTime.Now);
            
        }

        private ShiftSchedule Get(IList<ShiftSchedule> list, DateTime date)
        {
            foreach (ShiftSchedule obj in list)
            {
                //test ShiftSchedule for Month not created
                if (obj.Date.Month == date.Month
                    && obj.Date.Year == date.Year)
                {
                    return obj;
                }
            }
            return null;
        }

        private void Remove(IDatabaseCommunicator db, DateTime date)
        {
            db.StartTransaction();

            ShiftSchedule shift = Get(db.GetShiftSchedules(), date);

            if (shift == null)
            {
                db.EndTransaction(TransactionEndOperation.READONLY);
                return;
            }

            db.RemoveShiftSchedule(shift);
            db.EndTransaction(TransactionEndOperation.SAVE);
        }
    }
}
