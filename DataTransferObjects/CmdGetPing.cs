using CommunicationCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CmdGetPing : Command
    {
        private DateTime timeStamp = DateTime.MinValue;

        public CmdGetPing()
        {
            timeStamp = new DateTime();
        }

        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }
        }

    }
}
