using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Modules
{
    public class LoggingModule
    {
        public delegate void LogMessageEventHandler(object sender, string message, LogMessageKind kind = LogMessageKind.INFO);
        public event LogMessageEventHandler LogMessage = delegate { };

        public void Log(string message, LogMessageKind kind = LogMessageKind.INFO)
        {
            LogMessage.Invoke(this, message, kind);
        }
    }
}
