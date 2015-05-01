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
    public class CmdReturnGetAllCars : ResponseCommand
    {

        public ReadOnlyCollection<Car> Cars { get; private set; }

        public CmdReturnGetAllCars(Guid requestId, List<Car> cars) : base(requestId)
        {
            Cars = cars.AsReadOnly();
        }

    }
}
