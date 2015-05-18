﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DistanceCalculation
{
    public interface IRouteDistanceCalculator
    {
        DistanceContainer CalculateRouteDistance(IList<IDistanceMatrixPlace> waypoints);
    }
}
