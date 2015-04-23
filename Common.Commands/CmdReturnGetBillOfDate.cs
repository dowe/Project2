using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnGetBillOfDate : Command
    {

        public byte[] PDF { get; private set; }

        public CmdReturnGetBillOfDate(byte[] pdf)
        {
            PDF = pdf;
        }

    }
}
