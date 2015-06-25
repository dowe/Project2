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
        public float MinKm { get; private set; }

        public CmdReturnSelectCar(Guid requestId, bool success, float minKm) : base(requestId)
        {
            Success = success;
            MinKm = minKm;
        }

    }
}
