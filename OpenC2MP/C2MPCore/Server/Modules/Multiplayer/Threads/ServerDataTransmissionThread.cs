using C2MP.Core.Shared.Modules;

namespace C2MP.Core.Server.Modules.Multiplayer.Threads {
    public class ServerDataTransmissionThread {
        private Client client;
        private LoggingModule loggingModule;
        private EventModule eventModule;
        private MultiplayerModule multiplayerModule;

        private StreamWriter tcpWriterStream;

        private long lowQualityTransmissionDelay;
        private long highQualityTransmissionDelay;

        public ServerDataTransmissionThread(Client client, LoggingModule loggingModule, EventModule eventModule, MultiplayerModule multiplayerModule) {
            this.client = client;
            this.client.dataTransmissionThread = this;
            this.loggingModule = loggingModule;
            this.eventModule = eventModule;
            this.multiplayerModule = multiplayerModule;

            this.lowQualityTransmissionDelay = 50L;
            this.highQualityTransmissionDelay = 5000L;
        }

        public void Run() {
            long lowQualityTransmitTime = 0L;
            long highQualityTransmitTime = 0L;

            // TODO: add compression, see decomp. Java source
            try {
                tcpWriterStream = new StreamWriter(new BufferedStream(client.tcpClientSocket.GetStream(), 1024));
            } catch (Exception ex) {
                loggingModule.LogException("There was a problem starting the TCP writer stream", ex, LogMessageKind.ERROR);
                return;
            }

            eventModule.RaiseSpawnServerTcpDataReceiveThread(this, new ClientEventArgs(client));

            multiplayerModule.BroadcastAddClientExcept(client);

            // TODO: banning happens here if the Client is banned
            // TODO: client is added to the client record here, but I already do so in the ServerListenerThread to prevent garbage collection
        }

        public void SendMessage(string message, bool isUdp, bool isError) {
            try {
                if (isUdp) {
                    byte[] messageBytes = message.GetBytes();
                    client.udpClientSocket.Send(messageBytes, messageBytes.Length); // TODO: Do I have to provide IP & port here? Probably not?
                } else {
                    if (isError) {
                        tcpWriterStream.WriteLine("/error " + message);
                    } else {
                        tcpWriterStream.WriteLine(message);
                    }

                    tcpWriterStream.Flush();
                }
            } catch (Exception ex) {
                // TODO: fill in with the actual code instead of just this
                loggingModule.LogException(message, ex, LogMessageKind.ERROR);
            }
        }
    }
}
