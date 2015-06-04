using System;
using Common.Communication.Client;
using Common.Commands;
using Common.Communication;
using Microsoft.AspNet.SignalR.Client;
using Kundenapp;
using Common.DataTransferObjects;
using System.Collections.Generic;
using System.Collections;

namespace KundenappTest
{
	public class ClientConnectionMock : IClientConnection
	{
		public ClientConnectionMock ()
		{
		}

		public bool retbool;

		public CmdAddOrder cmd;

		#region IClientConnection implementation

		public event Action Closed;

		public event Action Reconnecting;

		public event Action Reconnected;

		public void Start ()
		{
			throw new NotImplementedException ();
		}

		public void Stop ()
		{
			throw new NotImplementedException ();
		}

		public void Connect ()
		{
			throw new NotImplementedException ();
		}

		public void Send (Common.Communication.Command command)
		{
			throw new NotImplementedException ();
		}

		public T SendWait<T> (Common.Communication.Command command) where T : Common.Communication.Command
		{
			if (command.GetType() == typeof(CmdLoginCustomer)) 
			{
				return (T)((Command)new CmdReturnLoginCustomer (command.Id, retbool));
			}
			if (command.GetType () == typeof(CmdAddOrder)) 
			{
				cmd = (CmdAddOrder)command;
				return (T)((Command)new CmdReturnAddOrder(command.Id,1));
			}
			if (command.GetType () == typeof(CmdGetAnalyses)) 
			{
				List<Analysis> ListofAnalysis = new List<Analysis>();
				ListofAnalysis.Add(new Analysis("Blut_Leukozyten", 4, 10, "Tsd./µl", (float)29.99, SampleType.BLOOD));
				ListofAnalysis.Add(new Analysis("Blut_Erythrozyten", (float)4.3, (float)5.9, "Mio./µl", (float)29.99, SampleType.BLOOD));
				ListofAnalysis.Add(new Analysis("Blut_Hämoglobin", (float)12, (float)18, "g/dl", (float)15.95, SampleType.BLOOD));
				ListofAnalysis.Add(new Analysis("Blut_Hämatonkrit", (float)37, (float)54, "%", (float)15.95, SampleType.BLOOD));
				ListofAnalysis.Add(new Analysis("Blut_Thrombozyten", (float)150, (float)400, "Tsd./µl", (float)12.55, SampleType.BLOOD));
				ListofAnalysis.Add(new Analysis("Urin_Osmolatität", (float)50, (float)1200, "mosm/kg", (float)10.95, SampleType.URINE));
				ListofAnalysis.Add(new Analysis("Urin_ph-Wert", (float)4.8, (float)7.6, "-", (float)5.95, SampleType.URINE));
				ListofAnalysis.Add(new Analysis("Urin_Gewicht", (float)1001, (float)1035, "g/l", (float)7.45, SampleType.URINE));
				ListofAnalysis.Add(new Analysis("Urin_Albumin", (float)0, (float)20, "mg", (float)10.95, SampleType.URINE));
				ListofAnalysis.Add(new Analysis("Speichel_Cortisol", (float)0, (float)9.8, "µg/l", (float)4.50, SampleType.SALIVA));
				ListofAnalysis.Add(new Analysis("Speichel_DHEA", (float)130, (float)490, "pg/ml", (float)15.99, SampleType.SALIVA));
				ListofAnalysis.Add(new Analysis("Speichel_Östradiol", (float)1, (float)2, "pg/ml", (float)20.50, SampleType.SALIVA));
				ListofAnalysis.Add(new Analysis("Speichel_Testosteron", (float)47.2, (float)136.2, "pg/ml", (float)24.95, SampleType.SALIVA));
				ListofAnalysis.Add(new Analysis("Speichel_Progesteron", (float)12.7, (float)57.4, "pg/ml", (float)24.95, SampleType.SALIVA));
				ListofAnalysis.Add(new Analysis("Stuhl_Calprotectin", (float)10, (float)31, "µg/g", (float)15.95, SampleType.EXCREMENT));
				ListofAnalysis.Add(new Analysis("Stuhl_Candida", (float)0, (float)10000, "Pilze/g", (float)15.50, SampleType.EXCREMENT));
				return (T)((Command)new CmdReturnGetAnalyses (command.Id, (IList<Analysis>)ListofAnalysis));
			}
			return null;
		}

		public T SendWait<T> (Common.Communication.Command command, int timeout) where T : Common.Communication.Command
		{
			throw new NotImplementedException ();
		}

		public void RegisterCommandHandler (Common.Communication.ICommandHandler handler)
		{
			throw new NotImplementedException ();
		}

		public ConnectionState ConnectionState {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

