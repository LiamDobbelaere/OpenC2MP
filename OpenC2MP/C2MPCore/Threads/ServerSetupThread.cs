using C2MP.Core.Modules;

namespace C2MP.Core.Threads {
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
            loggingModule.Log($"Setting up, please be patient..", LogMessageKind.WARN);

            if (!File.Exists(configModule.Config.GetDataFile("TEMP_OPPONENT.txt"))) {
                eventModule.RaisePerformFirstTimeSetup(this);
            }

            // TODO: spectator mode (delayed, low-priority)

            // TODO: set graphics wrapper (delayed, medium-priority)

            // TODO: mode record + listbox (delayed, low-priority)

            eventModule.RaiseBuildCarRecord(this);

            eventModule.RaiseBuildTrackRecord(this);

            // TODO: set ip address (-> determine importance)

            // TODO: advertise to master server (delayed, low-priority)

            eventModule.RaiseSpawnServerListener(this);

            // TODO: game manipulator disable drones

            // TODO: spawn game listener thread

            loggingModule.Log($"Finished setting up!", LogMessageKind.INFO);
        }
    }
}
