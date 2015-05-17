using System;

using Xamarin.Forms;

namespace Kundenapp_test
{
	public class PatientView : ContentPage
	{
		public PatientView ()
		{
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


