using GalaSoft.MvvmLight;

namespace ManagementSoftware.ViewModel
{
    public class MapVM : ViewModelBase
    {
        public string Test { get; set; }

        /// <summary>
        ///     Initializes a new instance of the MapVM class.
        /// </summary>
        public MapVM()
        {
            Test = "test erfolgreich";
        }
    }
}
