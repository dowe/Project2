using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Diagnostics.CodeAnalysis;

namespace ManagementSoftware
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
