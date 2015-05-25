using ManagementSoftware.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ShiftScheduleDayView.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ShiftScheduleDayView : UserControl
    {
        public ShiftScheduleDayView()
        {
            InitializeComponent();
        }

        public void NextDay(object sender, MouseButtonEventArgs e)
        {
            ShiftScheduleDayVM vm = (ShiftScheduleDayVM) DataContext;
            vm.NextDay();
        }

        public void PreviousDay(object sender, MouseButtonEventArgs e)
        {
            ShiftScheduleDayVM vm = (ShiftScheduleDayVM)DataContext;
            vm.PreviousDay();
        }
    }
}
