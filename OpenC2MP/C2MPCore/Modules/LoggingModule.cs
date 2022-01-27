namespace C2MP.Core.Modules {
    public enum LogMessageKind {
        INFO, // generic
        WARN, // warning
        ERROR, // something went wrong, but execution can attempt to continue
        INPUT, // user input
        FATAL, // unresolvable error
        STATIC, // generic, but shown in the same place (like a status bar)
    }

    public class LoggingModule {
        private string prefix = String.Empty;

        public delegate void LogMessageEventHandler(object sender, string message, string prefix = "", LogMessageKind kind = LogMessageKind.INFO);
        public event LogMessageEventHandler LogMessage = delegate { };

        public void Log(string message, LogMessageKind kind = LogMessageKind.INFO) {
            LogMessage.Invoke(this, message, prefix, kind);
        }

        public void LogException(string message, Exception ex, LogMessageKind kind = LogMessageKind.ERROR) {
            LogMessage.Invoke(this, $"{message}{Environment.NewLine}{Environment.NewLine}{ex.ToString()}", prefix, kind);
        }

        public void Log(string message, string prefixOverload, LogMessageKind kind = LogMessageKind.INFO) {
            LogMessage.Invoke(this, message, prefixOverload, kind);
        }

        public LoggingModule Of(string prefix) {
            LoggingModule copyWithPrefix = new LoggingModule();

            #if DEBUG
                copyWithPrefix.prefix = prefix;
            #endif
            copyWithPrefix.LogMessage += CopyWithPrefix_LogMessage;
            
            return copyWithPrefix;
        }

        private void CopyWithPrefix_LogMessage(object sender, string message, string prefix, LogMessageKind kind = LogMessageKind.INFO) {
            Log(message, prefix, kind);
        }
    }
}
