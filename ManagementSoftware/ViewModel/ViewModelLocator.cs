/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ManagementSoftware.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using Common.Communication.Client;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ManagementSoftware.Communication;
using ManagementSoftware.Model;
using Microsoft.Practices.ServiceLocation;

namespace ManagementSoftware.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IClientConnection, ClientConnectionCreator>();
            SimpleIoc.Default.Register<RegisterCustomerVM>();
            SimpleIoc.Default.Register<ShiftScheduleVM>();
            SimpleIoc.Default.Register<DailyStatisticVM>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public RegisterCustomerVM RegisterCustomer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterCustomerVM>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public ShiftScheduleVM ShiftSchedule
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftScheduleVM>();
            }
        }
          [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public DailyStatisticVM DailyStatistic
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DailyStatisticVM>();
            }
        }


        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}