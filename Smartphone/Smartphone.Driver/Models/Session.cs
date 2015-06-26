using System;

namespace Smartphone.Driver.Models
{
    public class Session
    {

        public string Username { get; set; }
        public string CarID { get; set; }

        public Session()
        {
        }

        public void Reset()
        {
            Username = null;
            CarID = null;
        }

        public bool IsInitialized
        {
            get { return Username != null && CarID != null; }
        }

    }
}

