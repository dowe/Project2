using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.Timer;

namespace Server
{
    public class LocalServerDataImpl : ILocalServerData
    {

        //key value test id -> timer
        public LocalServerDataImpl()
        {
            ZmsAddress = new Address();
            ZmsAddress.City = "Offenburg";
            ZmsAddress.PostalCode = "77652";
            ZmsAddress.Street = "Badstraße 24";

            TaxiPhoneNumber = "123123123123"; // TODO: Replace dummy value.

            RoadCostInEuroPerKm = 0.80F;

            TimerList = new Dictionary<Guid, InjectInternalTimed>();
        }

        public DailyStatistic DailyStatistic { get; set; }
        public Address ZmsAddress { get; private set; }
        public string TaxiPhoneNumber { get; private set; }
        public float RoadCostInEuroPerKm { get; private set; }
        public InjectInternalTimed GenerateShiftScheduleTimer { get; set; }
        public InjectInternalTimed CheckOrdersFiveHoursLeftScheduledTimer { get; set; }
        public Dictionary<Guid, InjectInternalTimed> TimerList { get; private set; }
    }
}
