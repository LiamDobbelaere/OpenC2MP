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

            // TODO: spectator mode (delayed, low-priority)

            // TODO: set graphics wrapper (delayed, medium-priority)

            // TODO: mode record + listbox (delayed, low-priority)

            // TODO: car record + listbox
            eventModule.RaiseBuildCarRecord(this);

            // TODO: track record + listbox

            // TODO: set ip address

            // TODO: advertise to master server

            // TODO: spawn server listener thread

            // TODO: game manipulator disable drones

            // TODO: spawn game listener thread
        }
    }
}
