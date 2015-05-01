using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnSelectCar : ResponseCommand
    {

        public bool Success { get; private set; }

        public CmdReturnSelectCar(Guid requestId, bool success) : base(requestId)
        {
            Success = success;
        }

    }
}
