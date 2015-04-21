using CommunicationCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CmdReturnPing : Command
    {

        private DateTime initTime = new DateTime();

        public CmdReturnPing(Guid requestId, DateTime initTime) : base(requestId)
        {
            this.initTime = initTime;
        }

        public DateTime InitTime
        {
            get
            {
                return initTime;
            }
        }

    }
}
