using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetDailyStatistic : Command
    {

        public DailyStatistic DailyStatistic { get; private set; }

        public CmdReturnGetDailyStatistic(DailyStatistic dailyStatistic)
        {
            DailyStatistic = dailyStatistic;
        }

    }
}
