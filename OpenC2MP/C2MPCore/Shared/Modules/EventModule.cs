namespace C2MP.Core.Shared.Modules {
    public class ClientEventArgs : EventArgs {
        public ClientEventArgs(Server.Modules.Multiplayer.Client client) {
            Client = client;
        }

        public Server.Modules.Multiplayer.Client Client { get; set; } 
    }

    public class EventModule {
        public event EventHandler PerformFirstTimeSetup = delegate { };
        internal void RaisePerformFirstTimeSetup(object sender) => PerformFirstTimeSetup.Invoke(sender, EventArgs.Empty);

        public event EventHandler BuildCarRecord = delegate { };
        internal void RaiseBuildCarRecord(object sender) => BuildCarRecord.Invoke(sender, EventArgs.Empty);

        public event EventHandler CarRecordBuilt = delegate { };
        internal void RaiseCarRecordBuilt(object sender) => CarRecordBuilt.Invoke(sender, EventArgs.Empty);

        public event EventHandler BuildTrackRecord = delegate { };
        internal void RaiseBuildTrackRecord(object sender) => BuildTrackRecord.Invoke(sender, EventArgs.Empty);

        public event EventHandler TrackRecordBuilt = delegate { };
        internal void RaiseTrackRecordBuilt(object sender) => TrackRecordBuilt.Invoke(sender, EventArgs.Empty);

        public event EventHandler SpawnServerListener = delegate { };
        internal void RaiseSpawnServerListener(object sender) => SpawnServerListener.Invoke(sender, EventArgs.Empty);

        public event EventHandler ShutdownServerListener = delegate { };
        internal void RaiseShutdownServerListener(object sender) => ShutdownServerListener.Invoke(sender, EventArgs.Empty);

        public event EventHandler SpawnClientTcpDataReceiveThread = delegate { };
        internal void RaiseSpawnClientTcpDataReceiveThread(object sender) => SpawnClientTcpDataReceiveThread.Invoke(sender, EventArgs.Empty);

        public event EventHandler<ClientEventArgs> SpawnServerDataTransmissionThread = delegate { };
        internal void RaiseSpawnServerDataTransmissionThread(object sender, ClientEventArgs args) => SpawnServerDataTransmissionThread.Invoke(sender, args);

        public event EventHandler<ClientEventArgs> SpawnServerTcpDataReceiveThread = delegate { };
        internal void RaiseSpawnServerTcpDataReceiveThread(object sender, ClientEventArgs args) => SpawnServerTcpDataReceiveThread.Invoke(sender, args);

        public event EventHandler<ClientEventArgs> SpawnServerUdpDataReceiveThread = delegate { };
        internal void RaiseSpawnServerUdpDataReceiveThread(object sender, ClientEventArgs args) => SpawnServerUdpDataReceiveThread.Invoke(sender, args);
    }
}
