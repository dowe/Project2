using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;

namespace Kundenapp
{
	public class PatientM : ObservableObject
	{
		public PatientM (String id)
		{
			PatientID = id;
//			ListofAnalysis = new ObservableCollection<AnalysisM>();
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Blut_Leukozyten", 4, 10, "Tsd./µl", (float)29.99, SampleType.BLOOD)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Blut_Erythrozyten", (float)4.3, (float)5.9, "Mio./µl", (float)29.99, SampleType.BLOOD)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Blut_Hämoglobin", (float)12, (float)18, "g/dl", (float)15.95, SampleType.BLOOD)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Blut_Hämatonkrit", (float)37, (float)54, "%", (float)15.95, SampleType.BLOOD)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Blut_Thrombozyten", (float)150, (float)400, "Tsd./µl", (float)12.55, SampleType.BLOOD)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Urin_Osmolatität", (float)50, (float)1200, "mosm/kg", (float)10.95, SampleType.URINE)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Urin_ph-Wert", (float)4.8, (float)7.6, "-", (float)5.95, SampleType.URINE)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Urin_Gewicht", (float)1001, (float)1035, "g/l", (float)7.45, SampleType.URINE)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Urin_Albumin", (float)0, (float)20, "mg", (float)10.95, SampleType.URINE)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Speichel_Cortisol", (float)0, (float)9.8, "µg/l", (float)4.50, SampleType.SALIVA)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Speichel_DHEA", (float)130, (float)490, "pg/ml", (float)15.99, SampleType.SALIVA)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Speichel_Östradiol", (float)1, (float)2, "pg/ml", (float)20.50, SampleType.SALIVA)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Speichel_Testosteron", (float)47.2, (float)136.2, "pg/ml", (float)24.95, SampleType.SALIVA)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Speichel_Progesteron", (float)12.7, (float)57.4, "pg/ml", (float)24.95, SampleType.SALIVA)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Stuhl_Calprotectin", (float)10, (float)31, "µg/g", (float)15.95, SampleType.EXCREMENT)));
//			ListofAnalysis.Add(new AnalysisM(new Analysis("Stuhl_Candida", (float)0, (float)10000, "Pilze/g", (float)15.50, SampleType.EXCREMENT)));
		}

		public ObservableCollection<AnalysisM> ListofAnalysis {
			get;
			set;
		}

		public String PatientID {
			get;
			set;
		}

		public String Analysises
		{
			get
			{
				String ret = "";
				if (ListofAnalysis == null) 
				{
					return "";
				}
				foreach (AnalysisM Analysis in ListofAnalysis) 
				{
					if (Analysis.Selected) 
					{
						if (!String.IsNullOrEmpty(ret)) 
						{
							ret = ret + ", ";
						}
						ret = ret + Analysis.Analysis.Name;
					}
				}
				return ret;
			}
		}

		public String Samples
		{
			get
			{
				String ret = "";
				if (ListofAnalysis == null) 
				{
					return "";
				}
				foreach (AnalysisM Analysis in ListofAnalysis) 
				{
					if (Analysis.Selected) 
					{
						if (!ret.Contains (Analysis.Analysis.SampleType.ToString ()))
						{
							if (!String.IsNullOrEmpty(ret)) 
							{
								ret = ret + ", ";
							}
							ret = ret + Analysis.Analysis.SampleType.ToString ();
						}
						
					}
				}
				return ret;
			}
		}

		private RelayCommand delCMD;

		public RelayCommand DelCMD
		{
			get
			{
				return delCMD ?? (delCMD = new RelayCommand (()=>Messenger.Default.Send<PatientM> (this, "delPat")));
			}
		}

		private RelayCommand editCMD;

		public RelayCommand EditCMD
		{
			get
			{
				return editCMD ?? (editCMD = new RelayCommand (()=>
					{
						Messenger.Default.Send<PatientM> (this, "editPat");
					}));
			}
		}
			
		private RelayCommand changedCMD;

		public RelayCommand ChangedCMD
		{
			get
			{
				return changedCMD ?? (changedCMD = new RelayCommand (()=>Changed()));
			}
		}

		public void Changed()
		{
			RaisePropertyChanged("Samples");
			RaisePropertyChanged ("Analysises");
		}
	}
}