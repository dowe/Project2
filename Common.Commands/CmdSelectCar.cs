using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdSelectCar : Command
    {

        public string SelectedCarId { get; private set; }
        public float StartKm { get; private set; }

        public CmdSelectCar(string selectedCarId, float startKm)
        {
            SelectedCarId = selectedCarId;
            StartKm = startKm;
        }

    }
}
