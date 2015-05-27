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
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private bool executed = false;



        public MapView()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            _connection = SimpleIoc.Default.GetInstance<IClientConnection>();
            _laborPos = new LocalServerDataImpl().ZmsAddress;
            WebBrowserGoogle.ObjectForScripting = new ExposedJSObject(WebBrowserGoogle, this);
            //WebBrowserGoogle.NavigateToString(Properties.Resources.GoogleMaps);
            string curDir = Directory.GetCurrentDirectory();
            this.WebBrowserGoogle.Navigate(new Uri(String.Format("file:///{0}/Resources/GoogleMaps.html", curDir)));
            TxtCar.Text = "Fahrer: Max Mustermann";
            LblCar.Content = "DummyCar";


        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RefreshData();
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
                sb.Append(String.IsNullOrWhiteSpace(cust.Label)
                    ? cust.FirstName + " " + cust.LastName + "\n"
                    : cust.Label + "\n");
                if (_driversOrders.FirstOrDefault().EmergencyPosition == null)
                {
                    sb.Append(cust.Address.Street + "\n");
                    sb.Append(cust.Address.PostalCode + " " + cust.Address.City + "\n");
                }
                else
                {
                    //Emergency Position
                    sb.Append("Außergewöhnliche Abholung bei\n");
                    sb.Append("GPS Position: " + _driversOrders.FirstOrDefault().EmergencyPosition.Latitude + "/" + _driversOrders.FirstOrDefault().EmergencyPosition.Longitude + "\n");
                }
                sb.Append("Proben abzuholen: " + _driversOrders.FirstOrDefault().Test.Count + "\n");
            }
            else
            {
                sb.Append("Zentrallabor\n");
                sb.Append(_laborPos.Street + "\n");
                sb.Append(_laborPos.PostalCode + " " + _laborPos.City + "\n");
            }
            sb.Append("Entfernung: " + distance.Distance + "km in " + distance.Time.ToString("0.00") + "h\n");

            if (_driversOrders.Any() && _driversOrders.Count > 1)
            {
                sb.Append("______________________________________\n\n");
                sb.Append("Weitere Ziele:\n\n");

                for (int i = 1; i < _driversOrders.Count; i++)
                {
                    var cust = _driversOrders[i].Customer;
                    sb.Append(cust.Label ?? cust.FirstName + " " + cust.LastName + "\n");
                    if (_driversOrders[i].EmergencyPosition == null)
                    {
                        sb.Append(cust.Address.Street + "\n");
                        sb.Append(cust.Address.PostalCode + " " + cust.Address.City + "\n\n");
                    }
                    else
                    {
                        sb.Append("Außergewöhnliche Abholung bei\n");
                        sb.Append("GPS Position: " + _driversOrders[i].EmergencyPosition.Latitude + "/" + _driversOrders[i].EmergencyPosition.Longitude + "\n");
                    }
                }
            }

            TxtCar.Text = sb.ToString();
        }

        public void RefreshData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var cars = _connection.SendWait<CmdReturnGetAllOccupiedCars>(new CmdGetAllOccupiedCars());
            var cust = _connection.SendWait<CmdReturnGetAllCustomers>(new CmdGetAllCustomers());
            if (cars == null || cust == null)
            {
                ShowErrorMessagebox();
                return;
            }
            Car oldcar = null;
            if(_cars!=null && _cars.Count > _carIndex)
                 oldcar = _cars[_carIndex];
            _cars = cars.OccupiedCars.ToList();
            _customers = cust.Customers.ToList();

            //carindex evtl anpassen
            if (oldcar != null)
            {
                var car = _cars.FirstOrDefault(c => c.CarID == oldcar.CarID);
                _carIndex = car != null ? _cars.IndexOf(car) : 0;
            }

            SetMapIcons();
            RefreshDriver(_carIndex);
            Mouse.OverrideCursor = null;
        }

        private void RefreshDriver(int carindex)
        {
            if (!_cars.Any())
                return;

            if (_cars.Count <= carindex)
                _carIndex = carindex = 0;             

            var car = _cars[carindex];


            var cmd = _connection.SendWait<CmdReturnGetDriversUnfinishedOrders>(
                new CmdGetDriversUnfinishedOrders(car.CurrentDriver.UserName));

            if (cmd == null)
            {
                ShowErrorMessagebox();
                return;
            }

            _driversOrders = cmd.UnfinishedOrders.ToList();

            var address = _laborPos;
            Order order = _driversOrders.FirstOrDefault();
            if (order!=null)
            {
                address = order.Customer.Address;
                if (order.EmergencyPosition == null)
                    NavigateOnMap(car.LastPosition, order.Customer.Address);
                else
                    NavigateOnMap(car.LastPosition, order.EmergencyPosition);
            }
            else
                NavigateOnMap(car.LastPosition, _laborPos);

            IDistanceMatrixPlace destination = (order != null && order.EmergencyPosition != null)
                ? (IDistanceMatrixPlace) new DistanceMatrixGPSPosition(order.EmergencyPosition)
                : new DistanceMatrixAddress(address);
            var distance = DistanceCalculation.CalculateDistanceInKm(new DistanceMatrixGPSPosition(car.LastPosition), destination);
           
            SetCarText(distance);
        }

        

        private void ShowErrorMessagebox()
        {
            Mouse.OverrideCursor = null;
            MessageBox.Show(
                    "Fehler: Bitte überprüfen Sie ihre Internetverbindung oder kontaktieren Sie den nicht vorhandenen Kundendienst.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void NavigateOnMap(GPSPosition from, GPSPosition to)
        {
            WebBrowserGoogle.InvokeScript("navigate", new Object[] { from.Latitude, from.Longitude, to.Latitude, to.Longitude });
        }

        private void NavigateOnMap(GPSPosition from, Address to)
        {
            WebBrowserGoogle.InvokeScript("navigateToAddress", new Object[] { from.Latitude, from.Longitude, to.Street + ", " + to.PostalCode + " " + to.City });
        }

        private void GetDriverDestinations(Car car)
        {

        }

        private void SetMapIcons()
        {
            WebBrowserGoogle.InvokeScript("addLaboratory", new Object[] { _laborPos.Street + ", " + _laborPos.PostalCode + " " + _laborPos.City, "Zentrallabor", "Zentrallabor<br/>" + _laborPos.Street + "<br/>" + _laborPos.PostalCode + " " + _laborPos.City });

            if (_customers != null)
                foreach (var cust in _customers)
                {
                    var name = String.IsNullOrWhiteSpace(cust.Label) ? cust.FirstName + " " + cust.LastName : cust.Label;
                    WebBrowserGoogle.InvokeScript("addAddress", new Object[] { cust.Address.Street + ", " + cust.Address.PostalCode + " " + cust.Address.City, name, name + "<br/>" + cust.Address.Street + "<br/>" + cust.Address.PostalCode + " " + cust.Address.City });
                }

            if (_cars != null)
                foreach (var car in _cars)
                {
                    WebBrowserGoogle.InvokeScript("addCar", new Object[] { car.LastPosition.Latitude, car.LastPosition.Longitude, car.CarID, "Kennzeichen: " + car.CarID + "<br/>Fahrer: " + car.CurrentDriver.FirstName + " " + car.CurrentDriver.LastName });
                }
        }

        public void SwitchCar(string carid)
        {
            var car = _cars.Where(c => c.CarID == carid).FirstOrDefault();
            if (car != null && _carIndex != _cars.IndexOf(car))
            {
                _carIndex = _cars.IndexOf(car);
                RefreshDriver(_carIndex);
            }
        }

        private void RightArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_cars == null)
                return;

            var old = _carIndex;
            _carIndex++;

            if (_carIndex >= _cars.Count)
                _carIndex = 0;

            if (old != _carIndex)
                RefreshDriver(_carIndex);
        }

        private void LeftArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_cars == null)
                return;

            var old = _carIndex;
            _carIndex--;

            if (_carIndex < 0)
                _carIndex = _cars.Count - 1;

            if (old != _carIndex)
                RefreshDriver(_carIndex);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && executed == false)
            {
                executed = true;
                dispatcherTimer.Start();
            }
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
            _window.SwitchCar(car);
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
