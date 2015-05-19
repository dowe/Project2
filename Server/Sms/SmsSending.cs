using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Server.Sms
{
    public class SmsSending : ISmsSending
    {
        private const string fileName = "GMailAccount.txt";
        private string user;
        private string password;

        public SmsSending()
        {
            // Read in Login Credentials for the Gmail Account
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileName);
            try
            {
                string[] credentials = File.ReadAllLines(path);
                user = credentials[0];
                password = credentials[1];
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler beim Einlesen der GMail Account Daten (" + e.Message + ")");
                throw;
            }
        }

        public void Send(string number, string message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            NetworkCredential gmailLogin = new NetworkCredential(user, password);
            MailAddress fromAddress = new MailAddress(user);

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = gmailLogin;

            mail.From = fromAddress;
            mail.Subject = number.ToString();
            mail.Body = message;
            mail.To.Add(user);

            try
            {
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler beim Senden der Mail (" + e.Message + ")");
            }
        }
    }
}
