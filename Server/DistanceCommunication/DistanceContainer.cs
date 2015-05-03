using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DistanceCommunication
{
    public struct DistanceContainer
    {
        /// <summary>
        /// Distance in km
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Time in hours
        /// </summary>
        public float Time { get; set; }
    }
}
