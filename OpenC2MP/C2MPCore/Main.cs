using C2MP.Core.Client.Threads;
using C2MP.Core.Server.Modules.Multiplayer;
using C2MP.Core.Server.Modules.Multiplayer.Threads;
using C2MP.Core.Server.Threads;
using C2MP.Core.Shared.Modules;
using C2MP.Core.Shared.Modules.GameData;

namespace C2MP.Core {
    public class C2MPOptions {
        public bool isServer = true;
        public string ip = String.Empty;
        public bool isC2MPRunning;
    }

    public class Main {
        public static string version = "0.1 alpha";

        // Modules
        public LoggingModule loggingModule;
        public ConfigModule configModule;
        public EventModule eventModule;
        public GameDataModule gameDataModule;
        public MultiplayerModule multiplayerModule;

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
            loggingModule.LogMessage += LoggingModule_LogMessage;
            eventModule.PerformFirstTimeSetup += EventModule_PerformFirstTimeSetup;
            eventModule.SpawnServerListener += EventModule_SpawnServerListener;
            eventModule.SpawnClientTcpDataReceiveThread += EventModule_SpawnClientTcpDataReceiveThread;
            eventModule.SpawnServerDataTransmissionThread += EventModule_SpawnServerDataTransmissionThread;
            eventModule.SpawnServerTcpDataReceiveThread += EventModule_SpawnServerTcpDataReceiveThread;
            eventModule.SpawnServerUdpDataReceiveThread += EventModule_SpawnServerUdpDataReceiveThread;
            eventModule.BuildCarRecord += EventModule_BuildCarRecord;
            eventModule.BuildTrackRecord += EventModule_BuildTrackRecord;

            configModule = new ConfigModule(loggingModule);
            multiplayerModule = new MultiplayerModule(configModule, loggingModule.Of("MultiplayerModule"), eventModule);
            gameDataModule = new GameDataModule(configModule, loggingModule.Of("GameDataModule"), eventModule);
            this.chatCommands = new Dictionary<string, Action>()
            {
                { "forcefirsttimesetup", () => this.DoFirstTimeSetup() },
                { "configlocation", () => this.loggingModule.Log(this.configModule.ConfigFileLocation) },
                { "exit", this.Exit }
            };
        }

        private void EventModule_SpawnServerUdpDataReceiveThread(object? sender, ClientEventArgs e) {
            throw new NotImplementedException();
        }

        private void EventModule_SpawnServerTcpDataReceiveThread(object? sender, ClientEventArgs e) {
            Thread serverTcpDataReceiveThread =
                new Thread(() => new ServerTcpDataReceiveThread(e.Client, loggingModule, multiplayerModule).Run());
            serverTcpDataReceiveThread.Name = e.Client.clientName + "-ServerTcpDataReceiveThread";
            serverTcpDataReceiveThread.Start();
        }

        private void EventModule_SpawnServerDataTransmissionThread(object? sender, ClientEventArgs e) {
            Thread serverDataTransmissionThread =
                new Thread(() => new ServerDataTransmissionThread(e.Client, loggingModule, eventModule, multiplayerModule).Run());
            serverDataTransmissionThread.Name = e.Client.clientName + "-ServerDataTransmissionThread";
            serverDataTransmissionThread.Start();
        }

        private void EventModule_SpawnClientTcpDataReceiveThread(object? sender, EventArgs e) {
            Thread tcpDataReceiveThread =
                new Thread(() => new ClientTcpDataReceiveThread(
                    loggingModule.Of("ClientTcpDataReceiveThread"), configModule, options).Run());
            tcpDataReceiveThread.Name = "ClientTcpDataReceiveThread";
            tcpDataReceiveThread.Start();
        }

        private void EventModule_SpawnServerListener(object? sender, EventArgs e) {
            Thread serverListenerThread =
                new Thread(() => new ServerListenerThread(
                    loggingModule.Of("ServerListenerThread"), configModule, eventModule, multiplayerModule, options).Run());
            serverListenerThread.Name = "ServerListenerThread";
            serverListenerThread.Start();
        }

        private void EventModule_BuildTrackRecord(object? sender, EventArgs e) {
            gameDataModule.BuildTrackRecord();
        }

        private void EventModule_BuildCarRecord(object? sender, EventArgs e) {
            gameDataModule.BuildCarRecord();
        }

        private void EventModule_PerformFirstTimeSetup(object? sender, EventArgs e) {
            DoFirstTimeSetup();
        }

        private void DoFirstTimeSetup() {
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
            eventModule.RaiseShutdownServerListener(this);
        }

        public void Run(bool isServer = true, string ip = "") {
            options = new C2MPOptions() {
                ip = ip,
                isServer = isServer,
                isC2MPRunning = true
            };

            Initialize();

            if (!options.isC2MPRunning) {
                // Something went wrong that caused an early abort, don't continue
                return;
            }

            if (options.isServer) {
                Thread serverSetupThread =
                    new Thread(() => new ServerSetupThread(
                        loggingModule.Of("ServerSetupThread"), configModule, eventModule, options).Run());
                serverSetupThread.Name = "ServerSetupThread";
                serverSetupThread.Start();
            } else {
                Thread clientSetupThread =
                    new Thread(() => new ClientSetupThread(
                        loggingModule.Of("ClientSetupThread"), configModule, eventModule, options).Run());
                clientSetupThread.Name = "ClientSetupThread";
                clientSetupThread.Start();
            }
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
