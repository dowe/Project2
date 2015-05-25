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
using ManagementSoftware.Model;
using ManagementSoftware.Helper;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics.CodeAnalysis;

namespace ManagementSoftware.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IMessageBox, MessageBoxImpl>();
            SimpleIoc.Default.Register<IClientConnection>(CreateClientConnection);
            SimpleIoc.Default.Register<RegisterCustomerVM>();
            SimpleIoc.Default.Register<ShiftScheduleVM>();
            SimpleIoc.Default.Register<DailyStatisticVM>();
            SimpleIoc.Default.Register<TestsVM>();
            SimpleIoc.Default.Register<CustomerListVM>();
            SimpleIoc.Default.Register<MapVM>();
            SimpleIoc.Default.Register<CreateOrderVM>();

        }

        private static IClientConnection CreateClientConnection()
        {
            IClientConnection c = new ClientConnection("http://localhost:8080/commands");

            c.Start();
            c.Connect();

            return c;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public TestsVM Tests
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TestsVM>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public CustomerListVM CustomerList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CustomerListVM>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public MapVM Map
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MapVM>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public CreateOrderVM CreateOrder
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreateOrderVM>();
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