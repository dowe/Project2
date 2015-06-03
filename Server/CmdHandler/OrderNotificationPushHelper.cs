using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.Sms;

namespace Server.CmdHandler
{
    static class OrderNotificationPushHelper
    {
        
        public static void PushNotificationToDriverOrTaxi(IServerConnection connection, UsernameToConnectionIdMapping driverMapping, ISmsSending smsSending, ILocalServerData serverData, Order orderToPush, Driver driverOrNull)
        {
            if (driverOrNull != null)
            {
                string connectionIDOrNull = driverMapping.ResolveConnectionIDOrNull(driverOrNull.UserName);
                if (connectionIDOrNull != null)
                {
                    CmdSendNotification sendNotification = new CmdSendNotification(orderToPush);
                    connection.Unicast(sendNotification, connectionIDOrNull);
                }
            }
            else
            {
                if (orderToPush.EmergencyPosition != null)
                {
                    smsSending.Send(serverData.TaxiPhoneNumber,
                        "New order " + orderToPush.OrderID + ". Please collect at " +
                        orderToPush.EmergencyPosition.Latitude +
                        ", " + orderToPush.EmergencyPosition.Longitude + ".");
                }
                else
                {
                    smsSending.Send(serverData.TaxiPhoneNumber,
                        "New order " + orderToPush.OrderID + ". Please collect at " +
                        orderToPush.Customer.Address.Street +
                        ", " + orderToPush.Customer.Address.PostalCode + ", " + orderToPush.Customer.Address.City + ".");
                }
            }
        }
    }
}
