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

        public string Username { get; private set; }
        public string SelectedCarId { get; private set; }
        public float StartKm { get; private set; }

        public CmdSelectCar(string username, string selectedCarId, float startKm)
        {
            Username = username;
            SelectedCarId = selectedCarId;
            StartKm = startKm;
        }

    }
}
