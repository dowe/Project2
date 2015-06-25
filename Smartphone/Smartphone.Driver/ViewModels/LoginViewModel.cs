using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Client;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.AspNet.SignalR.Client;
using Smartphone.Driver.GPS;
using Smartphone.Driver.Models;
using Smartphone.Driver.Messages;
using Smartphone.Driver.NativeServices;
using Smartphone.Driver.Const;

namespace Smartphone.Driver.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public const string ServerURLProperty = "ServerURL";
        public const string UsernameProperty = "Username";
        public const string PasswordProperty = "Password";
        public const string CanConnectProperty = "CanConnect";
        public const string IsCommunicatingProperty = "IsCommunicating";
        public const string IsNotCommunicatingProperty = "IsNotCommunicating";

        private IClientConnection connection = null;
        private Session session = null;
        private IToaster toaster = null;
        private GPSPositionSender positionSender = null;

        private string serverURL = null;
        private string username = null;
        private string password = null;
        private bool communicating = false;
        private RelayCommand loginCommand = null;

        public LoginViewModel(IClientConnection connection, Session session, IToaster toaster, GPSPositionSender positionSender)
        {
            this.connection = connection;
            this.session = session;
            this.toaster = toaster;
            this.positionSender = positionSender;

            serverURL = "http://langy.duckdns.org:8008/commands";
            username = "Driv3";
            password = "Driv3";
            communicating = false;
        }

        public string ServerURL
        {
            get { return serverURL; }
            set
            {
                if (!string.Equals(serverURL, value))
                {
                    serverURL = value;
                    RaisePropertyChanged(ServerURLProperty);
                }
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                if (!string.Equals(username, value))
                {
                    username = value;
                    RaisePropertyChanged(UsernameProperty);
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (!string.Equals(password, value))
                {
                    password = value;
                    RaisePropertyChanged(PasswordProperty);
                }
            }
        }

        public bool IsCommunicating
        {
            get { return communicating; }
            set
            {
                if (communicating != value)
                {
                    communicating = value;
                    RaisePropertyChanged(IsCommunicatingProperty);
                    RaisePropertyChanged(IsNotCommunicatingProperty);
                }
            }
        }

        public bool IsNotCommunicating
        {
            get { return !IsCommunicating; }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand(Login));
            }
        }

        private async void Login()
        {
            IsCommunicating = true;
            if (!connection.ConnectionState.Equals(ConnectionState.Connected) || !string.Equals(serverURL, connection.ServerURL))
            {
                await Connect();
            }
            if (connection.ConnectionState.Equals(ConnectionState.Connected))
            {
                var cmdLogin = new CmdLoginDriver(username, password);
                var response = connection.SendWait<CmdReturnLoginDriver>(cmdLogin);
                if (response != null)
                {
                    if (response.Success)
                    {
                        OnLoginSuccessful(response.AssignedCarIDOrNull);
                    }
                    else
                    {
                        toaster.MakeToast(TextDefinitions.FAILED_LOGIN);
                    }
                }
                else
                {
                    toaster.MakeToast(TextDefinitions.SERVER_NO_ANSWER);
                }
            }
            else
            {
                toaster.MakeToast(TextDefinitions.SERVER_NO_ANSWER);
            }
            IsCommunicating = false;
        }

        private Task<bool> Connect()
        {

            return Task.Run(() =>
            {
                bool success = false;
                try
                {
                    connection.Connect(serverURL);
                    success = true;
                }
                catch (ConnectionException)
                {
                    success = false;
                }

                return success;
            });
        }

        private void OnLoginSuccessful(string assignedCarIDOrNull)
        {
            session.Username = username;
            if (assignedCarIDOrNull != null)
            {
                // Driver already has a car assigned. Skip the car selection.
                session.CarID = assignedCarIDOrNull;
                toaster.MakeToast(TextDefinitions.AlreadyAssignedToCar(assignedCarIDOrNull));

                CmdGetDriversUnfinishedOrders cmdGetOrders = new CmdGetDriversUnfinishedOrders(session.Username);
                connection.Send(cmdGetOrders);

                positionSender.Start();

                Messenger.Default.Send<MsgSwitchOrdersPage>(new MsgSwitchOrdersPage());
            }
            else
            {
                // Driver does not yet have a car assigned.
                CmdGetAvailableCars getAvailableCars = new CmdGetAvailableCars();
                connection.Send(getAvailableCars);

                Messenger.Default.Send<MsgSwitchSelectCarPage>(new MsgSwitchSelectCarPage());
            }
        }

    }
}