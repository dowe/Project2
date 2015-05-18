using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using Common.Communication.Client;
using ManagementSoftware.Model;
using Common.Communication;
using Common.Commands;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace ManagementSoftware.ViewModel
{
    
    public class TestsVM :ViewModelBase
    {
         private IClientConnection _ClientConnection;
         private IList<TestEntryModel> _DataList;
         private IList<Order> _OrderList;
         public TestsVM(IClientConnection _ClientConnection)
        {
            this._ClientConnection = _ClientConnection;
            this.LoadCommand = new RelayCommand(LoadData);
          
        }
         public IList<TestEntryModel> DataList
         {
             get
             {
                 return _DataList;
             }
             set
             {
                 _DataList = value;
                 RaisePropertyChanged();
             }
         }
         public RelayCommand LoadCommand { get; set; }

         private void LoadData()
         {
             IList<TestEntryModel> _TestList = new List<TestEntryModel>();
             Command request = new CmdGetAllOrders();
             CmdReturnGetAllOrders response = _ClientConnection.SendWait<CmdReturnGetAllOrders>(request);
             if (response == null)
             {
                 MessageBox.Show("Fehler beim versenden der Anfrage zum Laden der Probenübersicht. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
             }
             else
             {
                //Aus Orderlist nun alle nötigen testentry daten übernehmen
                 _OrderList = null;
                 _OrderList = response.Orders;
                 foreach (Order o in _OrderList)
                 {
                     foreach (Test t in o.Test)
                     {
                         TestEntryModel temp = new TestEntryModel();
                         temp.OrderID = o.OrderID.ToString();
                         temp.PatientID = t.PatientID;
                         temp.BringDate = o.BringDate.ToString(); 
                         temp.CustomerLabel = o.Customer.ToString();
                         temp.SampleTypeAndAnalysis = t.Analysis.ToString();
                         temp.TestID = t.TestID.ToString();

                         //Eingetroffen?
                         if (t.TestState.Equals(TestState.COMPLETED) || t.TestState.Equals(TestState.IN_PROGRESS))
                             temp.SampleCollected = true;
                         else
                             temp.SampleCollected = false;

                         //Alarmstatus converten
                         switch (t.AlarmState)
                         {
                             case AlarmState.NO_ALARM:
                                 temp.AlarmState = "Kein Alarm gesendet";
                                 break;
                             case AlarmState.FIRST_ALARM_TRANSMITTED:
                                 temp.AlarmState = "Erster Alarm gesendet";
                                 break;
                             case AlarmState.FIRST_ALARM_CONFIRMED:
                                 temp.AlarmState = "Erster Alarm bestätigt";
                                 break;
                             case AlarmState.SECOND_ALARM_TRANSMITTED:
                                 temp.AlarmState = "Zweiter Alarm gesendet";
                                 break;
                             default:
                                 temp.AlarmState = "Fehler in Alarmstatus";
                                 break;
                         }
                        _TestList.Add(temp);
                     }
                 }

                 DataList = _TestList;
                 Console.WriteLine(DataList[0].TestID.ToString());
                 Console.WriteLine(DataList[0].OrderID.ToString());
                 MessageBox.Show("Daten abgerufen");
             }
         }
      
    }
}
