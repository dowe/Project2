using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public enum SampleState 
    {
        OPEN,
        COLLECTED,
        LABOR,
        DONE
    }

    public class Sample
    {

        private Guid id = Guid.Empty;
        private SampleState state = SampleState.OPEN;

        public Sample(Guid id, SampleState state)
        {
            this.id = id;
            this.state = state;
        }

        public Guid Id
        {
            get { return id; }
        }

        public SampleState State
        {
            get { return state; }
        }

    }
}
