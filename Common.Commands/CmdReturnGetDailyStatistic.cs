using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetDailyStatistic : ResponseCommand
    {

        public DailyStatistic DailyStatistic { get; private set; }

        public CmdReturnGetDailyStatistic(Guid requestId, DailyStatistic dailyStatistic) : base(requestId)
        {
            DailyStatistic = dailyStatistic;
        }

    }
}
