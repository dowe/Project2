using Common.DataTransferObjects;
using Server.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public interface ILocalServerData
    {
        DailyStatistic DailyStatistic { get; set; }
        Address ZmsAddress { get; }
        string TaxiPhoneNumber { get; }
        float RoadCostInEuroPerKm { get; }
        InjectInternalTimed GenerateShiftScheduleTimer { get; set; }
        InjectInternalTimed CheckOrdersFiveHoursLeftScheduledTimer { get; set; }
        Dictionary<Guid, InjectInternalTimed> TimerList { get; }
    }
}
