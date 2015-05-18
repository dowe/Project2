using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;

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
            var leftRelevantOptions = new List<DriverSendOption>(options);
            leftRelevantOptions = leftRelevantOptions.FindAll(o =>
            {
                bool passes = true;

                foreach (Func<DriverSendOption, bool> hardConstraint in hardConstraints)
                {
                    passes &= hardConstraint.Invoke(o);
                }

                return passes;
            });

            DriverSendOption bestOption = null;
            if (leftRelevantOptions.Count > 0)
            {
                bestOption = leftRelevantOptions[0];
            }

            return bestOption;
        }
    }
}
