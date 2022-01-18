using C2MP.Core.Modules;

namespace C2MP.Core {
    public class C2MPOptions {
        public bool isC2MPRunning;
    }

    public class Main {
        public static string version = "0.1 alpha";

        // Modules
        public LoggingModule loggingModule;
        public ConfigModule configModule;
        public EventModule eventModule;

        public C2MPOptions options;

        public Dictionary<string, Action> chatCommands;

        public Main() {
            loggingModule = new LoggingModule();
            eventModule = new EventModule();

            // don't use the constructor for initializing stuff,
            // we want the GUI to be able to hook up their events first
            // hence why only the logging and event module are instantiated, so we can hook up their events
        }

        private void Initialize() {
            options = new C2MPOptions() {
                isC2MPRunning = true
            };

            loggingModule.LogMessage += LoggingModule_LogMessage;
            eventModule.PerformFirstTimeSetup += EventModule_PerformFirstTimeSetup;
            configModule = new ConfigModule(loggingModule);


            this.chatCommands = new Dictionary<string, Action>()
            {
                { "configlocation", () => this.loggingModule.Log(this.configModule.ConfigFileLocation) },
                { "exit", this.Exit }
            };
        }

        private void EventModule_PerformFirstTimeSetup(object? sender, EventArgs e) {
            Thread performFirstTimeSetupThread = 
                new Thread(() => new PerformFirstTimeSetupThread(
                    loggingModule.Of("PerformFirstTimeSetupThread"), configModule, options).Run());
            performFirstTimeSetupThread.Name = "PerformFirstTimeSetupThread";
            performFirstTimeSetupThread.Start();
        }

        private void LoggingModule_LogMessage(object sender, string message, string prefix, LogMessageKind kind = LogMessageKind.INFO) {
            if (kind == LogMessageKind.FATAL) {
                Shutdown();
            }
        }

        public void Shutdown() {
            options.isC2MPRunning = false;
        }

        public void Run() {
            Initialize();

            Thread serverSetupThread = 
                new Thread(() => new ServerSetupThread(
                    loggingModule.Of("ServerSetupThread"), configModule, eventModule, options).Run());
            serverSetupThread.Name = "ServerSetupThread";
            serverSetupThread.Start();
        }

        private void Exit() {
            Environment.Exit(0);
        }

        public void ExecuteCommand(string command) {
            loggingModule.Log(command, LogMessageKind.INPUT);
            chatCommands[command.Substring(1)]();
        }
    }
}
