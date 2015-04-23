using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdSetTestResult : Command
    {
        public Guid TestId { get; private set; }
        public float Result { get; private set; }

        public CmdSetTestResult(Guid testId, float result)
        {
            TestId = testId;
            Result = result;
        }
    }
}
