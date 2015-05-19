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

namespace ManagementSoftware.View
{
    /// <summary>
    /// Interaktionslogik für MapView.xaml
    /// </summary>

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
            WebBrowserGoogle.ObjectForScripting = new TestObject(WebBrowserGoogle, this);

            string curDir = Directory.GetCurrentDirectory();
           WebBrowserGoogle.NavigateToString(Properties.Resources.GoogleMaps);
            //this.WebBrowserGoogle.Navigate(new Uri(String.Format("file:///{0}/GoogleMaps.html", curDir)));
            TxtCar.Text = "Autooo";
        }

        private void RightArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LeftArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SetText()
        {
            TxtCar.Text = "funzt";
        }
    }

    //Klassen für C#<->webbrowser:JS kommunikation
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IApp
    {
        void MapInitiliazingFinished();
        void SetDistance(string distanceDuration);
        void CarClicked(string car);
    }

    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ITestObject
    {
        IApp App { get; }
    }

    [ComVisible(true)]
    public class Appl : IApp
    {
        private readonly WebBrowser _browser;
        private readonly MapView _window;

        public Appl(WebBrowser browser, MapView window)
        {
            _browser = browser;
            _window = window;
        }


        public void MapInitiliazingFinished()
        {
            _browser.InvokeScript("navigate", new Object[] { 48.3173913, 8.0490079, 48.3154922, 8.005414 });
            _browser.InvokeScript("addCar", new Object[] { 48.3173913, 8.0490079, "Auto A" });
            _browser.InvokeScript("addCar", new Object[] { 48.3531049, 8.0855398, "Auto B" });
            _browser.InvokeScript("addHouse", new Object[] { 48.3154922, 8.005414, "Dr.House" });
            _browser.InvokeScript("addHouse", new Object[] { 48.2807446, 8.0906837, "Dr. Sheldon Cooper" });
            _browser.InvokeScript("addHouse", new Object[] { 48.3817759, 8.1600809, "Zentrallabor" });
            _browser.InvokeScript("addHouse", new Object[] { 48.3770449, 8.0833999, "Dr. Velvet" });
            _browser.InvokeScript("addAddress", new Object[] { "Jauschbach 6, 77784 Oberharmersbach", "Auenland" });
        }

        public void SetDistance(string distanceDuration)
        {
            //_window.SetDistance(distanceDuration);
            _window.SetText();
        }

        public void CarClicked(string car)
        {
            //_window.SwitchCar(car);
        }
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(ITestObject))]
    public class TestObject : ITestObject
    {
        private readonly Appl _app;

        public TestObject(WebBrowser browser, MapView window)
        {
            _app = new Appl(browser, window);
        }

        public IApp App
        {
            get { return _app; }
        }
    }
}
