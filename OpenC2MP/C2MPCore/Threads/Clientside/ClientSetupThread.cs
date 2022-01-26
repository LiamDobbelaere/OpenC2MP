using C2MP.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Threads.Clientside {
    internal class ClientSetupThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private EventModule eventModule;
        private C2MPOptions options;

        public ClientSetupThread(LoggingModule loggingModule, ConfigModule configModule, EventModule eventModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.eventModule = eventModule;
            this.options = options;
        }

        public void Run() {
            loggingModule.Log($"Welcome to C2MP Client {Main.version}!");
            loggingModule.Log($"Setting up, please be patient..", LogMessageKind.WARN);

            if (!File.Exists(configModule.Config.GetDataFile("TEMP_OPPONENT.txt"))) {
                eventModule.RaisePerformFirstTimeSetup(this);
            }

            // TODO: spectator mode (delayed, low-priority)

            // TODO: set graphics wrapper (delayed, medium-priority)

            // TODO: mode record + listbox (delayed, low-priority)

            eventModule.RaiseBuildCarRecord(this);

            eventModule.RaiseBuildTrackRecord(this);

            // TODO: game manipulator disable drones

            eventModule.RaiseSpawnTcpDataReceiveThread(this);

            // TODO: Spawn UdpDataReceiveThread

            // TODO: Spawn DataTransmissionThread

            // TODO: spawn game listener thread if data transmission thread is ready

            loggingModule.Log($"Finished setting up!", LogMessageKind.INFO);
        }
    }
}
