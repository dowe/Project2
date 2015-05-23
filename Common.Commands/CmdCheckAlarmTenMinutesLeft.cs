using Common.Communication;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class CmdCheckAlarmTenMinutesLeft : Command
    {
        public Guid TestID { get; set; }
        public long OrderID { get; set; }
        public CmdCheckAlarmTenMinutesLeft(Guid testID, long orderID)
        {
            OrderID = orderID;
            TestID = testID;
        }
    }
}
