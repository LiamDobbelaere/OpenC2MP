using C2MP.Core.Modules;
using System.Net;
using System.Net.Sockets;

namespace C2MP.Core.Threads {
    internal class ServerListenerThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private EventModule eventModule;
        private C2MPOptions options;

        private int totalConnections;

        public ServerListenerThread(LoggingModule loggingModule, ConfigModule configModule, EventModule eventModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.eventModule = eventModule;
            this.options = options;
        }

        public void Run() {

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, configModule.Config.port);

            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.ReceiveBufferSize = 1024; // C2O does this and I don't know why yet

            tcpSocket.Bind(localEndPoint);
            tcpSocket.Listen();

            loggingModule.Log($"TCP server socket running on port {localEndPoint.Port}");

            // in case of shutdown, try to close stuff up cleanly
            eventModule.ShutdownServerListener += (object? sender, EventArgs e) => {
                tcpSocket.Close();
            };

            // Allows this thread to be shutdown cleanly
            while (options.isC2MPRunning) {
                try {
                    Socket tcpClientSocket = tcpSocket.Accept();

                    tcpClientSocket.NoDelay = true;
                    tcpClientSocket.ReceiveBufferSize = 1024;
                    tcpClientSocket.SendBufferSize = 1024;

                    totalConnections++;

                    loggingModule.Log($"Incoming client, creating UDP client socket..", LogMessageKind.INFO);

                    // TODO: ClientRecord stuff
                    // TODO: make random-ish udp server, but always start with the configged port (see decomp. Java code)

                    // TODO: create Client
                } catch (SocketException ex) {
                    loggingModule.Log($"TCP server socket accept was interrupted, reason: {ex.Message}", LogMessageKind.ERROR);
                }
            }

            //Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //tcpSocket.
        }
    }
}
