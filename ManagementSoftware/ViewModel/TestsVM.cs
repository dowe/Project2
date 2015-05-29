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
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ManagementSoftware.ViewModel
{
    
    public class TestsVM :ViewModelBase
    {
         private IClientConnection _ClientConnection;
         private IList<TestEntryModel> _DataList;
         private IList<Order> _OrderList;
         private TestDetailModel _Detail;
         private String _ResultatDetailEnabled;
         private String _ResultatDetail;
         private String _ButtonDetail;
         public enum DetailButtonStatus {ORDERED, RECEIVED, COMPLETED, ALARM_CONFIRMED };
         private String _ButtonDetailVisible;

         private TestEntryModel _SelectedTestEntry;
         private Order currentOrder;

         public TestsVM(IClientConnection _ClientConnection)
        {
            this._ClientConnection = _ClientConnection;
            this.LoadCommand = new RelayCommand(LoadData);
            _SelectedTestEntry = new TestEntryModel();
            _Detail = new TestDetailModel();
            ButtonPressAction = new RelayCommand(ButtonPress);
            DetailButton = DetailButtonStatus.ORDERED;
            DetailButtonText = "Eingetroffen";
            ButtonDetailVisible = "Hidden";
            ResultatDetailEnabled ="False";
            ResultatDetail = "";
        }

         private void ButtonPress()
         {
             switch(SelectedTestEntry.Test.TestState)
             {
                 case TestState.ORDERED:
                     _ClientConnection.Send(new CmdSetOrderCollected(null, currentOrder.OrderID));
                     break;
                 case TestState.IN_PROGRESS:
                     if (ResultatDetail != "")
                     {
                         _ClientConnection.Send(new CmdSetTestResult(SelectedTestEntry.Test.TestID, Convert.ToSingle(ResultatDetail)));
                     }
                         break;
                 case TestState.COMPLETED:
                     if(SelectedTestEntry.Test.AlarmState == AlarmState.FIRST_ALARM_TRANSMITTED)
                     { 
                         _ClientConnection.Send(new CmdSetFirstAlertReceived(SelectedTestEntry.Test.TestID));
                     } 
                     break;

             }

             LoadData();
             RefreshDetail();   
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
         public RelayCommand ButtonPressAction { get; set; }
         private void RefreshDetail()
         {
             if (_SelectedTestEntry != null)
             {

                 if (_SelectedTestEntry.Test.ResultValue != 0)
                     ResultatDetail = SelectedTestEntry.Test.ResultValue.ToString();
                 else
                     ResultatDetail = "";

                if (SelectedTestEntry.BringDate != "")
                    BringDatumDetail = SelectedTestEntry.BringDate;
                else
                    BringDatumDetail = "#";

                if (SelectedTestEntry.TestID != null)
                    TestIDDetail = SelectedTestEntry.TestID;
                else
                    TestIDDetail = "#";
                if (SelectedTestEntry.Test.Analysis.UnitOfMeasure != null)
                    Detail.Einheit = SelectedTestEntry.Test.Analysis.UnitOfMeasure;
                else
                    Detail.Einheit = "#";
                
                Detail.Grenzwerte = SelectedTestEntry.Test.Analysis.ExtremeMinValue + " - " + SelectedTestEntry.Test.Analysis.ExtremeMaxValue;
                
                if (SelectedTestEntry.Test.ResultValue != 0)
                    Detail.Resultat = SelectedTestEntry.Test.ResultValue.ToString();
                else
                    Detail.Resultat = "#";

                //Get Info for Selected Test from the Order
                Detail.BestellDatum = "#";
                Detail.KundenAdresse = "#";
                Detail.Telefon = "#";
                foreach(Order o in _OrderList)
                {
                    //If this Test is from the current Order, get all Infos
                    if (o.Test.Any(x => x.TestID.ToString().Equals(SelectedTestEntry.TestID)))
                    {
                        //save current order for more infos later
                        currentOrder = o;

                        if (o.OrderDate != default(DateTime))
                            Detail.BestellDatum = o.OrderDate.GetValueOrDefault().ToString("dd.MM.yyyy HH:mm");
                        if (o.Customer.Address != null)
                            Detail.KundenAdresse = o.Customer.Address.Street + ", " + o.Customer.Address.PostalCode + " " +o.Customer.Address.City;
                        if (o.Customer.MobileNumber != null)
                            Detail.Telefon = o.Customer.MobileNumber;
                    }

                    
                }

                if (currentOrder.Driver != null)
                    DriverDetail = currentOrder.Driver.UserName;
                else
                    DriverDetail = "Taxi";

                if (currentOrder.Customer.UserName != "")
                    CustomerDetail = currentOrder.Customer.UserName;
                else
                    CustomerDetail = "#";
                //ButtonDetails
                if (SelectedTestEntry.Test.AlarmState == AlarmState.FIRST_ALARM_CONFIRMED || SelectedTestEntry.Test.AlarmState == AlarmState.SECOND_ALARM_TRANSMITTED)
                {
                    ButtonDetailVisible = "Hidden";
                    ResultatDetailEnabled = "False";

                }
                else if (SelectedTestEntry.Test.AlarmState == AlarmState.FIRST_ALARM_TRANSMITTED && SelectedTestEntry.Test.TestState == TestState.COMPLETED)
                {
                    DetailButtonText = "Alarm Bestätigt";
                    ButtonDetailVisible = "Visible";
                    ResultatDetailEnabled = "False";

                }
                else if(SelectedTestEntry.Test.TestState == TestState.COMPLETED)
                {
                    ButtonDetailVisible = "Hidden";
                    ResultatDetailEnabled = "False";
                }
                else if(SelectedTestEntry.Test.TestState == TestState.IN_PROGRESS)
                {

                    ResultatDetailEnabled = "True";
                    ButtonDetailVisible = "Visible";
                    DetailButtonText = "Test fertig";
                }
                
                else if (SelectedTestEntry.Test.TestState == TestState.ORDERED && currentOrder.Driver == null)
                {
                    ResultatDetailEnabled = "False";
                    DetailButtonText = "Eingetroffen";
                    ButtonDetailVisible = "Visible";
                }
                else if (SelectedTestEntry.Test.TestState == TestState.ORDERED)
                {
                    ResultatDetailEnabled = "False";
                    DetailButtonText = "Eingetroffen";
                    ButtonDetailVisible = "Hidden";
                }


                RaisePropertyChanged();
                RaisePropertyChanged(() => Detail);
                RaisePropertyChanged(() => BringDatumDetail);
                RaisePropertyChanged(() => TelefonDetail);
                RaisePropertyChanged(() => ResultatDetail);
                RaisePropertyChanged(() => ResultatDetailEnabled);
                RaisePropertyChanged(() => KundenAdresseDetail);
                RaisePropertyChanged(() => GrenzwerteDetail);
                RaisePropertyChanged(() => EinheitDetail);
                RaisePropertyChanged(() => BestellDatumDetail);
                RaisePropertyChanged(() => TestIDDetail);
                RaisePropertyChanged(() => DetailButtonText);
                RaisePropertyChanged(() => ButtonDetailVisible);
             }
         }
         private void LoadData()
         {
             new CmdGenerateBills();
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
                         temp.CustomerLabel = o.Customer.Label;
                         temp.SampleTypeAndAnalysis = t.Analysis.Name;
                         temp.TestID = t.TestID.ToString();
                         temp.Test = t;
                         //Eingetroffen?
                         if (t.TestState.Equals(TestState.COMPLETED) || t.TestState.Equals(TestState.IN_PROGRESS))
                             temp.SampleCollected = true;
                         else
                             temp.SampleCollected = false;
                         //Teststate Converten
                         switch(t.TestState)
                         {
                             case  TestState.COMPLETED:
                                 temp.TestState = "Fertig";
                                 break;
                             case TestState.IN_PROGRESS:
                                 temp.TestState = "In Bearbeitung";
                                 break;
                             case TestState.ORDERED:
                                 temp.TestState = "Bestellt";
                                 break;
                             case TestState.WAITING_FOR_DRIVER:
                                 temp.TestState = "Warten auf Fahrer";
                                 break;
                             default:
                                 temp.TestState = "Kein Status vorhanden";
                             break;
                         }
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
             
             }
         }
       
         public String TelefonDetail
         {
             get
             {
                 return _Detail.Telefon;
             }
             set
             {
                 _Detail.Telefon = value;
                 RaisePropertyChanged();
             }
         }
         public String ResultatDetail
         {
             get
             {
                 return _ResultatDetail;
             }
             set
             {
                 _ResultatDetail = value;
                 RaisePropertyChanged();
             }
         }
         public String ResultatDetailEnabled
         {
             get
             {
                 return _ResultatDetailEnabled;
             }
             set
             {
                 _ResultatDetailEnabled = value;
                 RaisePropertyChanged();
             }
         }
         public String KundenAdresseDetail
         {
             get
             {
                 return _Detail.KundenAdresse;
             }
             set
             {
                 _Detail.KundenAdresse = value;
                 RaisePropertyChanged();
             }
         }
         public String GrenzwerteDetail
         {
             get
             {
                 return _Detail.Grenzwerte;
             }
             set
             {
                 _Detail.Grenzwerte = value;
                 RaisePropertyChanged();
             }
         }
         public String EinheitDetail
         {
             get
             {
                 return _Detail.Einheit;
             }
             set
             {
                 _Detail.Einheit = value;
                 RaisePropertyChanged();
             }
         }
         public String BestellDatumDetail
         {
             get
             {
                 return _Detail.BestellDatum;
             }
             set
             {
                 _Detail.BestellDatum = value;
                 RaisePropertyChanged();
             }
         }
         public String BringDatumDetail
         {
             get
             {
                 return _Detail.BringDatum;
             }
             set
             {
                 _Detail.BringDatum = value;
                 RaisePropertyChanged();
             }
         }
         public String DriverDetail
         {
             get
             {
                 return _Detail.Driver;
             }
             set
             {
                 _Detail.Driver = value;
                 RaisePropertyChanged();
             }
         }
         public String CustomerDetail
         {
             get
             {
                 return _Detail.Customer;
             }
             set
             {
                 _Detail.Customer = value;
                 RaisePropertyChanged();
             }
         }
         public String TestIDDetail
         {
             get
             {
                 return _Detail.TestID;
             }
             set
             {
                 _Detail.TestID = value;
                 RaisePropertyChanged();
             }
         }
         public TestEntryModel SelectedTestEntry
         {
             get
             {
                 return _SelectedTestEntry;
             }
             set
             {

                 _SelectedTestEntry = value;
                 RefreshDetail();
                 RaisePropertyChanged();

             }
         }
         public TestDetailModel Detail
         {
             get
             {
                 return _Detail;
             }
             set
             {
                 _Detail = value;
                 RaisePropertyChanged();
             }
         }
         public DetailButtonStatus DetailButton
         { get; set; }
         public String DetailButtonText
         {
             get
             {
                 return _ButtonDetail;
             }
             set
             {
                 _ButtonDetail = value;
                 RaisePropertyChanged();
             }
         }

         public String ButtonDetailVisible
         {
             get
             {
                 return _ButtonDetailVisible;
             }
             set
             {
                 _ButtonDetailVisible = value;
                 RaisePropertyChanged();
             }
         }
    }
}
