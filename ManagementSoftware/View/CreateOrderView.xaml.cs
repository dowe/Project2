using Common.DataTransferObjects;
using ManagementSoftware.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManagementSoftware.View
{
    /// <summary>
    /// Interaction logic for CreateOrderView.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class CreateOrderView : UserControl
    {
        public CreateOrderView()
        {
            InitializeComponent();

            CreateOrderVM vm = (CreateOrderVM) DataContext;

            vm.SetBox(AllSamplesListBox);
        }

        private void AllSamplesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateOrderVM vm = (CreateOrderVM)DataContext;
            vm.SelectedAnalysisChanged();
        }

        
    }
}
