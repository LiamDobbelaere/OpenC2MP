using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Modules {
    public class EventModule {
        public event EventHandler PerformFirstTimeSetup = delegate { };
        public void RaisePerformFirstTimeSetup(object sender) => PerformFirstTimeSetup.Invoke(sender, EventArgs.Empty);
    }
}
