using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{

    public delegate void SendHandler(string serializedMessage);

    public class Sendable<T>
    {

        private SendHandler sendHandler = null;
        private T value = default(T);

        public Sendable(T value, SendHandler sendHandler)
        {
            this.sendHandler = sendHandler;
            this.value = value;
        }

        public T Value
        {
            get
            {
                return value;
            }
        }

        public SendHandler Send
        {
            get
            {
                return sendHandler;
            }
        }

    }
}
