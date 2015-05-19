using System;

namespace Smartphone.Driver.Droid.NativeServices
{
	public class IncIntGenerator
	{

		private int current = 0;

		public IncIntGenerator (int startValue)
		{
			current = startValue;
		}

		public int IncrementAndGet()
		{
			current++;
			if (current < 0)
			{
				current = 0;
			}

			return current;
		}

	}
}

