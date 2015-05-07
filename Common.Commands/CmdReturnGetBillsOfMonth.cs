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
    public class CmdReturnGetBillsOfMonth : ResponseCommand
    {
        public ReadOnlyCollection<Bill> BillsOfMonth { get; private set; }

        public CmdReturnGetBillsOfMonth(Guid requestId, IList<Bill> billsOfMonth)
            : base(requestId)
        {
            BillsOfMonth = new ReadOnlyCollection<Bill>(billsOfMonth);
        }
    }
}
