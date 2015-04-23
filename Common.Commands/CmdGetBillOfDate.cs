using System;
using Common.Communication;

namespace Common.Commands
{
    public class CmdGetBillOfDate : Command
    {

        public string Username { get; private set; }
        public DateTime BillDate { get; private set; }

        public CmdGetBillOfDate(string username, DateTime billDate)
        {
            Username = username;
            BillDate = billDate;
        }

    }
}
