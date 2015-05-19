namespace Server.Sms
{
    interface ISmsSending
    {
        void Send(string number, string message);
    }
}
