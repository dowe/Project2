﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.Timer;

namespace Server
{
    public class LocalServerData
    {

        public LocalServerData()
        {
            ZmsAddress = new Address();
            ZmsAddress.City = "Offenburg";
            ZmsAddress.PostalCode = "77652";
            ZmsAddress.Street = "Badstraße 24";

            RoadCostInEuroPerKm = 0.80F;

        }

        public Address ZmsAddress { get; private set; }
        public float RoadCostInEuroPerKm { get; private set; }
        public ScheduledTimer GenerateShiftScheduleTimer { get; set; }
    }
}
