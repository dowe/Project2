using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class UsernameToConnectionIdMapping
    {

        private Dictionary<string, string> usernameToConnectionID = null;

        public UsernameToConnectionIdMapping()
        {
            usernameToConnectionID = new Dictionary<string, string>();
        }

        public void Set(string username, string connectionID)
        {
            usernameToConnectionID.Remove(username);
            usernameToConnectionID.Add(username, connectionID);
        }

        public void Remove(string username)
        {
            usernameToConnectionID.Remove(username);
        }

        public string ResolveConnectionIDOrNull(string username)
        {
            string connectionID = null;
            usernameToConnectionID.TryGetValue(username, out connectionID);

            return connectionID;
        }

    }
}
