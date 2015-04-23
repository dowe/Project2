using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnGetBillDatesOfMonth : Command
    {
        public ReadOnlyCollection<DateTime> BillDatesOfMonth { get; private set; }

        public CmdReturnGetBillDatesOfMonth(List<DateTime> billDatesOfMonth)
        {
            BillDatesOfMonth = billDatesOfMonth.AsReadOnly();
        }
    }
}
