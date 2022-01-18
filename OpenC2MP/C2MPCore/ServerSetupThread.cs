using C2MP.Core.Modules;

namespace C2MP.Core {
    internal class ServerSetupThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private EventModule eventModule;
        private C2MPOptions options;

        public ServerSetupThread(LoggingModule loggingModule, ConfigModule configModule, EventModule eventModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.eventModule = eventModule;
            this.options = options;
        }

        public void Run() {
            loggingModule.Log($"Welcome to C2MP {Main.version}!");

            if (!File.Exists(configModule.Config.GetDataFile("TEMP_OPPONENT.txt"))) {
                eventModule.RaisePerformFirstTimeSetup(this);
            }

            /*while (options.isC2MPRunning)
            {

            }*/
        }
    }
}
