namespace C2MP.Core.Modules {
    public enum LogMessageKind {
        INFO, // generic
        WARN, // warning
        ERROR, // something went wrong, but execution can attempt to continue
        INPUT, // user input
        FATAL // unresolvable error
    }

    public class LoggingModule {
        public delegate void LogMessageEventHandler(object sender, string message, LogMessageKind kind = LogMessageKind.INFO);
        public event LogMessageEventHandler LogMessage = delegate { };

        public void Log(string message, LogMessageKind kind = LogMessageKind.INFO) {
            LogMessage.Invoke(this, message, kind);
        }
    }
}
