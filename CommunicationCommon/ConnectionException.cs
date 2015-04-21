using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCommon
{
    public class ConnectionException : Exception
    {
        public ConnectionException(string message, Exception e) : base(message, e)
        {
        }
    }
}
