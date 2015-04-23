using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdSetFirstAlertReceived : Command
    {

        public Guid TestId { get; private set; }

        public CmdSetFirstAlertReceived(Guid testId)
        {
            TestId = testId;
        }

    }
}
