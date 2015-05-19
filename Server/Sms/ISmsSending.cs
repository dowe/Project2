namespace Server.Sms
{
    public interface ISmsSending
    {
        void Send(string number, string message);
    }
}
