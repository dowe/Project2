using System;
using Common.Communication.Client;

namespace Kundenapp
{
	public class ClientConnectionCreator : ClientConnection
	{
		public ClientConnectionCreator()
			: base("http://192.168.0.13:8080/commands")
		{
			Start();
			Connect();
		}
	}
}

