using System;
using Common.DataTransferObjects;

namespace Kundenapp
{
	public class AnalysisM
	{
		public AnalysisM (Analysis analysis)
		{
			Analysis = analysis;
			Selected = false;
		}

		public Analysis Analysis {
			get;
			private set;
		}

		public bool Selected {
			get;
			set;
		}
	}
}

