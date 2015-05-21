using Common.Communication;

namespace Common.Commands
{
    public class CmdRemindDriverOfOrder : Command
    {

        public long OrderId { get; private set; }

        public CmdRemindDriverOfOrder(long orderId)
        {
            OrderId = orderId;
        }

    }
}
