using C2MP.Core.Modules;

namespace C2MP.Core {
    internal class ServerSetupThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private C2MPOptions options;

        public ServerSetupThread(LoggingModule loggingModule, ConfigModule configModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.options = options;
        }

        public void Run() {
            loggingModule.Log($"Welcome to C2MP {Main.version}!");

            if (!File.Exists(configModule.Config.GetDataFile("TEMP_OPPONENT.txt"))) {
                // TODO: actually send a message to main to run the PerformFirstTimeSetup thread
            }

            /*while (options.isC2MPRunning)
            {

            }*/
        }
    }
}
