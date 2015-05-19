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
    public class CmdReturnGetAllOccupiedCars : ResponseCommand
    {
        public ReadOnlyCollection<Car> OccupiedCars { get; set; }

        public CmdReturnGetAllOccupiedCars(Guid requestId, IList<Car> occupiedCars) : base(requestId)
        {
            OccupiedCars = new ReadOnlyCollection<Car>(occupiedCars);
        }
    }
}
