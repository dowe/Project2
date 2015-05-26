using Common.Communication;
using Common.Communication.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    public class InjectInternalTimed
    {
        private IServerConnection _Connection = null;
        private ITimer _Timer;
        private Func<Command> _Command;

        public InjectInternalTimed (
            IServerConnection _Connection,
            ITimer _Timer,
            Func<Command> _Command)
        {
            this._Connection = _Connection;
            this._Timer = _Timer;
            this._Command = _Command;
        }

        public void Start()
        {
            _Timer.Start(DoAction);
        }

        private void DoAction()
        {
            _Connection.InjectInternal(_Command());
        }

        public void Cacel()
        {
            _Timer.Cancel();
        }   
    }
}
