using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    public class ResponseCommand : Command
    {

        public ResponseCommand(Guid requestId)
            : base(requestId)
        {
        }

    }
}
