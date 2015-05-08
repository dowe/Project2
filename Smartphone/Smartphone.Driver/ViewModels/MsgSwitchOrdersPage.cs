using System;

namespace Smartphone.Driver
{
	public class SwitchPageMsg
	{

		public Page NextPage
		{
			get;
			private set;
		}

		public SwitchPageMsg (Page nextPage)
		{
			NextPage = nextPage;
		}
			
	}

	public enum Page
	{
		Login,
		Orders
	}
}

