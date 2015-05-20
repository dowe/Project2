using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Kundenapp
{
	public partial class AnalysisSelectionView : ContentPage
	{
		public AnalysisSelectionView ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.AnalysisVM;
			this.Disappearing += (sender, e) => {
				AnalysisSelectionVM vm = (BindingContext as AnalysisSelectionVM);
				vm.Patient.ChangedCMD.Execute(null);
			};
		}
	}
}

