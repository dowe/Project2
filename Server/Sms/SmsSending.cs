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
        public bool Enabled { get; private set; }

        public SmsSending()
        {
            // Read in Login Credentials for the Gmail Account
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileName);
            try
            {
                string[] credentials = File.ReadAllLines(path);
                user = credentials[0];
                password = credentials[1];
                Enabled = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler beim Einlesen der GMail Account Daten (" + e.Message +
                                  "). SMS Sending ist disabled.");
                Enabled = false;
            }
        }

        /// <summary>
        ///     Sends a sms to the specified number.
        ///     works by sending out an email to an Sms Gateway
        /// </summary>
        /// <param name="number">the phone number</param>
        /// <param name="message">the message to send</param>
        public void Send(string number, string message)
        {
            if (Enabled)
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                NetworkCredential gmailLogin = new NetworkCredential(user, password);
                MailAddress fromAddress = new MailAddress(user);

                // Gmail specific smtp setup
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = gmailLogin;

                // Make the mail
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
            else
            {
                Console.WriteLine("SMS to {0}: {1}", number, message);
            }
        }
    }
}
