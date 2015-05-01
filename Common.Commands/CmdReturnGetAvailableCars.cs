using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetAvailableCars : ResponseCommand
    {

        public ReadOnlyCollection<Car> AvailableCars { get; private set; }

        public CmdReturnGetAvailableCars(Guid requestId, List<Car> availableCars) : base(requestId)
        {
            AvailableCars = availableCars.AsReadOnly();
        }

    }
}
