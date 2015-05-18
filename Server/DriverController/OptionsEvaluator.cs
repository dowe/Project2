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
        private Func<DriverSendOption, float> softConstraint = null;

        public OptionsEvaluator()
        {
            hardConstraints = new List<Func<DriverSendOption, bool>>();
        }

        public void AddHardConstraint(Func<DriverSendOption, bool> hardConstraint)
        {
            hardConstraints.Add(hardConstraint);
        }

        public void SetSoftConstraint(Func<DriverSendOption, float> softConstraint)
        {
            this.softConstraint = softConstraint;
        }

        public virtual DriverSendOption ChooseBestOptionOrNull(IList<DriverSendOption> options)
        {
            var leftRelevantOptions = new List<DriverSendOption>(options);
            // Remove all options that do not pass the hard constraints.
            leftRelevantOptions = leftRelevantOptions.FindAll(o =>
            {
                bool passes = true;

                foreach (Func<DriverSendOption, bool> hardConstraint in hardConstraints)
                {
                    passes &= hardConstraint.Invoke(o);
                }

                return passes;
            });

            // Order according to soft constraint evaluation. Highest score first.
            if (softConstraint != null)
            {
                var optionComparer = Comparer<DriverSendOption>.Create((l, r) =>
                {
                    return Math.Sign(softConstraint(r) - softConstraint(l));
                });
                leftRelevantOptions.Sort(optionComparer);
            }

            DriverSendOption bestOption = null;
            if (leftRelevantOptions.Count > 0)
            {
                bestOption = leftRelevantOptions[0];
            }

            return bestOption;
        }
    }
}
