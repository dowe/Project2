namespace Server.Sms
{
    public interface ISmsSending
    {
        bool Enabled { get; }
        void Send(string number, string message);
    }
}
