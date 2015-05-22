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
        private LocalServerData data;

        public CmdGenerateShiftScheduleHandler(
            IServerConnection connection,
            IDatabaseCommunicator db,
            LocalServerData data)
        {
            this.connection = connection;
            this.db = db;
            this.data = data;
        }

        protected override void Handle(CmdGenerateShiftSchedule command, string connectionIdOrNull)
        {

            DateTime refDate;

            switch (command.Mode)
            {
                case GenerateMonthMode.DEFAULT_NEXT_MONTH:
                    DateTime now = DateTime.Now;
                    DateTime nextMonth = now.AddMonths(1);
                    DateTime dateInSevenDays = now.AddDays(7.0);
                    if (dateInSevenDays.Month == nextMonth.Month
                            && dateInSevenDays.Year == nextMonth.Year)
                    {
                        refDate = nextMonth;
                    }
                    else
                    {
                        return;
                    }
                    break;

                case GenerateMonthMode.IMMEDIATELY_CURRENT_MONTH:
                    refDate = DateTime.Now;
                    break;

                case GenerateMonthMode.IMMEDIATELY_NEXT_MONTH:
                    refDate = DateTime.Now.AddMonths(1);
                    break;
                default:
                    throw new Exception("Unknonw GenerateMonthMode <" + command.Mode + ">");
            }

            CreateForMonth(refDate);
        }

        private void CreateForMonth(DateTime refDate)
        {

            ShiftSchedule previousShiftSchedule = new ShiftSchedule();
            DateTime previousMonth = refDate.AddMonths(-1);

            //GET ALL ShiftSchedules
            db.StartTransaction();
            IList<ShiftSchedule> list = db.GetShiftSchedules();
            bool existShiftSchedule = false;
            foreach (ShiftSchedule obj in list)
            {
                //test ShiftSchedule for Month not created
                if (obj.Date.Month == refDate.Month
                    && obj.Date.Year == refDate.Year)
                {
                    existShiftSchedule = true;
                    break;
                }

                //search previousShiftSchedule
                if (obj.Date.Month == previousMonth.Month
                    && obj.Date.Year == previousMonth.Year)
                {
                    previousShiftSchedule = obj;
                }
            }
            db.EndTransaction(TransactionEndOperation.READONLY);

            if (existShiftSchedule)
            {
                return;
            }

            //TODO: create real ShiftSchedule use (refDate, previousMonth) and dedicated Unit
            ShiftSchedule newShiftSchedule = Util.CreateTestData(refDate);

            IShiftScheduleCreator creator = new ShiftScheduleCreator();
            if (previousShiftSchedule.DayEntry == null)
            {
                previousShiftSchedule = newShiftSchedule;
            }
            ShiftSchedule cur = creator.createShiftSchedule(previousShiftSchedule, previousShiftSchedule.DayEntry[previousShiftSchedule.DayEntry.Count - 1].Date.AddDays(1));


            //store ShiftSchedule in db
            db.StartTransaction();
            db.CreateShiftSchedule(cur);
            db.EndTransaction(TransactionEndOperation.SAVE);

            Console.WriteLine("SHIFT_SCHEDULE CREATED FOR <{0}>", refDate);
        }
    }
}
