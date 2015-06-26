using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Messaging;
using Smartphone.Driver.Const;
using Smartphone.Driver.Messages;
using Smartphone.Driver.Models;
using Smartphone.Driver.NativeServices;

namespace Smartphone.Driver.Handlers
{
    internal class CmdForceDriverLogoutHandler : CommandHandler<CmdForceDriverLogout>
    {

        private IClientConnection connection = null;
        private Session session = null;
        private IToaster toaster = null;

        public CmdForceDriverLogoutHandler(IClientConnection connection, Session session, IToaster toaster)
        {
            this.connection = connection;
            this.session = session;
            this.toaster = toaster;
        }

        protected override void Handle(CmdForceDriverLogout command, string connectionIdOrNull)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(
                () =>
                {
                    if (session.IsInitialized)
                    {
                        session.Reset();
                        Messenger.Default.Send<MsgSwitchLoginPage>(new MsgSwitchLoginPage());
                        toaster.MakeToast(TextDefinitions.FORCED_LOGOUT);
                    }
                });
        }

    }
}
