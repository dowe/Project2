using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DriverController
{
    public class OptionsEvaluator
    {

        private IList<Func<DriverSendOption, bool>> hardConstraints = null;
        private IList<Func<DriverSendOption, float>> softConstraints = null;

        public OptionsEvaluator()
        {
            hardConstraints = new List<Func<DriverSendOption, bool>>();
            softConstraints = new List<Func<DriverSendOption, float>>();
        }

        public void AddHardConstraint(Func<DriverSendOption, bool> hardConstraint)
        {
            hardConstraints.Add(hardConstraint);
        }

        public void AddSoftConstraint(Func<DriverSendOption, float> softConstraint)
        {
            softConstraints.Add(softConstraint);
        }

        public virtual DriverSendOption ChooseBestOptionOrNull(IList<DriverSendOption> options)
        {
            return null;
        }
    }
}
