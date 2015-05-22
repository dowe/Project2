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
        public Test Test { get; set; }
        public Order Order { get; set; }
        public CmdCheckAlarmTenMinutesLeft(Test test, Order order)
        {
            Order = order;
            Test = test;
        }
    }
}
