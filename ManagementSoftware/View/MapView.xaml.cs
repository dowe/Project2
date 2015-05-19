using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
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
using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using Common.Util;
using GalaSoft.MvvmLight.Ioc;
using Server;
using Server.DistanceCalculation;

namespace ManagementSoftware.View
{
    /// <summary>
    /// Interaktionslogik für MapView.xaml
    /// </summary>

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class MapView : UserControl
    {
        List<Car> _cars;
        List<Customer> _customers;
        List<Order> _driversOrders;
        private int _carIndex = 0;
        private readonly IClientConnection _connection;
        private readonly Address _laborPos;

        public MapView()
        {
            InitializeComponent();

            _connection = SimpleIoc.Default.GetInstance<IClientConnection>();
            _laborPos = new LocalServerData().ZmsAddress;
            WebBrowserGoogle.ObjectForScripting = new ExposedJSObject(WebBrowserGoogle, this);

            
            WebBrowserGoogle.NavigateToString(Properties.Resources.GoogleMaps);
            //string curDir = Directory.GetCurrentDirectory();
            //this.WebBrowserGoogle.Navigate(new Uri(String.Format("file:///{0}/GoogleMaps.html", curDir)));
            TxtCar.Text = "Autooo";
            LblCar.Content = "RoflCar";
        }



        public void SetCarText(DistanceContainer distance)
        {
            Car car = _cars[_carIndex];

            LblCar.Content = car.CarID;
            StringBuilder sb = new StringBuilder();
            sb.Append("Fahrer: " + car.CurrentDriver.FirstName + " " + car.CurrentDriver.LastName + "\n");
            sb.Append("Nächstes Ziel:\n");
            if (_driversOrders.Any())
            {
                var cust = _driversOrders.FirstOrDefault().Customer;
                sb.Append(cust.Label ?? cust.FirstName + " " + cust.LastName + "\n");
                sb.Append(cust.Address.Street + "\n");
                sb.Append(cust.Address.PostalCode + " " + cust.Address.City + "\n");
            }
            else
            {
                sb.Append("Zentrallabor\n");
                sb.Append(_laborPos.Street + "\n");
                sb.Append(_laborPos.PostalCode + " " + _laborPos.City + "\n");
            }
            sb.Append("Entfernung: " + distance.Distance + "km in " + distance.Time + "h\n");

            if (_driversOrders.Any() && _driversOrders.Count > 1)
            {
                sb.Append("________________________\n\n");
                sb.Append("Weitere Ziele:\n\n");

                for (int i = 1; i < _driversOrders.Count; i++)
                {
                    var cust = _driversOrders[i].Customer;
                    sb.Append(cust.Label ?? cust.FirstName + " " + cust.LastName + "\n");
                    sb.Append(cust.Address.Street + "\n");
                    sb.Append(cust.Address.PostalCode + " " + cust.Address.City + "\n\n");
                }
            }
            
            TxtCar.Text = sb.ToString();
        }

        public void RefreshData()
        {
            _cars = _connection.SendWait<CmdReturnGetAllOccupiedCars>(new CmdGetAllOccupiedCars()).OccupiedCars.ToList();
            _customers = _connection.SendWait<CmdReturnGetAllCustomers>(new CmdGetAllCustomers()).Customers.ToList();

            SetMapIcons();
            RefreshDriver(_carIndex);
        }

        private void RefreshDriver(int carid)
        {
            if (!_cars.Any())
                return;
            var car = _cars[carid];

            GetDriverDestinations(car);
            
            NavigateOnMap(car.LastPosition,
                _driversOrders.FirstOrDefault().Customer.Address ?? _laborPos);

            var distance = DistanceCalculation.CalculateDistanceInKm(car.LastPosition,
                _driversOrders.FirstOrDefault().Customer.Address ?? _laborPos);
            SetCarText(distance);
        }

        private void NavigateOnMap(GPSPosition from, Address to)
        {
            WebBrowserGoogle.InvokeScript("navigateToAddress", new Object[] { from.Latitude, from.Longitude, to.Street + ", " + to.PostalCode + " " + to.City });
        }

        private void GetDriverDestinations(Car car)
        {
            _driversOrders = _connection.SendWait<CmdReturnGetDriversUnfinishedOrders>(
                new CmdGetDriversUnfinishedOrders(car.CurrentDriver.UserName)).UnfinishedOrders.ToList();
        }

        private void SetMapIcons()
        {
            foreach (var car in _cars)
            {
                WebBrowserGoogle.InvokeScript("addCar", new Object[] { car.LastPosition.Latitude, car.LastPosition.Longitude, car.CarID });
            }

            foreach (var cust in _customers)
            {
                WebBrowserGoogle.InvokeScript("addAddress", new Object[] { cust.Address.Street + ", " + cust.Address.PostalCode + " " + cust.Address.City, cust.Label ?? cust.FirstName + " " + cust.LastName });
            }

            //WebBrowserGoogle.InvokeScript("addCar", new Object[] { 48.3173913, 8.0490079, "Auto A" });
            //WebBrowserGoogle.InvokeScript("addCar", new Object[] { 48.3531049, 8.0855398, "Auto B" });
            //WebBrowserGoogle.InvokeScript("addHouse", new Object[] { 48.3154922, 8.005414, "Dr.House" });
            //WebBrowserGoogle.InvokeScript("addHouse", new Object[] { 48.2807446, 8.0906837, "Dr. Sheldon Cooper" });
            //WebBrowserGoogle.InvokeScript("addHouse", new Object[] { 48.3817759, 8.1600809, "Zentrallabor" });
            //WebBrowserGoogle.InvokeScript("addHouse", new Object[] { 48.3770449, 8.0833999, "Dr. Velvet" });
            //WebBrowserGoogle.InvokeScript("addAddress", new Object[] { "Jauschbach 6, 77784 Oberharmersbach", "Auenland" });
        }

        private void RightArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _carIndex++;
            if (_carIndex >= _cars.Count)
                _carIndex = 0;

            RefreshDriver(_carIndex);
        }

        private void LeftArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _carIndex--;
            if (_carIndex < 0)
                _carIndex = _cars.Count - 1;

            RefreshDriver(_carIndex);
        }
    }

    //Klassen für C#<->webbrowser:JS kommunikation
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMapFunctions
    {
        void MapInitiliazingFinished();
        void CarClicked(string car);
    }

    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IExposedJSObject
    {
        IMapFunctions App { get; }
    }

    [ComVisible(true)]
    public class MapFunctions : IMapFunctions
    {
        private readonly WebBrowser _browser;
        private readonly MapView _window;

        public MapFunctions(WebBrowser browser, MapView window)
        {
            _browser = browser;
            _window = window;
        }


        public void MapInitiliazingFinished()
        {
            _window.RefreshData();
        }

        public void CarClicked(string car)
        {
            //_window.SwitchCar(car);
        }
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(IExposedJSObject))]
    public class ExposedJSObject : IExposedJSObject
    {
        private readonly MapFunctions _app;

        public ExposedJSObject(WebBrowser browser, MapView window)
        {
            _app = new MapFunctions(browser, window);
        }

        public IMapFunctions App
        {
            get { return _app; }
        }
    }
}
