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
    public class CmdReturnGetAllBillsOfUser : ResponseCommand
    {
        public ReadOnlyCollection<Bill> Bills { get; private set; }

        public CmdReturnGetAllBillsOfUser(Guid requestId, IList<Bill> bills)
            : base(requestId)
        {
            Bills = new ReadOnlyCollection<Bill>(bills);
        }
    }
}
