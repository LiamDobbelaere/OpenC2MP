using C2MP.Core.Modules;
using C2MP.Core.Modules.Multiplayer;
using System.Net;
using System.Net.Sockets;

namespace C2MP.Core.Threads {
    internal class ServerListenerThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private EventModule eventModule;
        private C2MPOptions options;
        private MultiplayerModule multiplayerModule;

        private int totalConnections;

        public ServerListenerThread(
            LoggingModule loggingModule, 
            ConfigModule configModule, 
            EventModule eventModule, 
            MultiplayerModule multiplayerModule, 
            C2MPOptions options
        ) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.eventModule = eventModule;
            this.multiplayerModule = multiplayerModule;

            this.options = options;
        }

        public void Run() {
            Random random = new Random();

            TcpListener tcpSocket = new TcpListener(IPAddress.Any, configModule.Config.port);
            tcpSocket.Server.ReceiveBufferSize = 1024; // C2O does this and I don't know why yet
            tcpSocket.Start();

            loggingModule.Log($"TCP server socket running on port {tcpSocket.Server.GetPort()}");

            // in case of shutdown, try to close stuff up cleanly
            eventModule.ShutdownServerListener += (object? sender, EventArgs e) => {
                tcpSocket.Stop();
            };

            UdpClient udpClientSocket = null;

            // Allows this thread to be shutdown cleanly
            while (options.isC2MPRunning) {
                try {
                    TcpClient tcpClientSocket = tcpSocket.AcceptTcpClient();

                    tcpClientSocket.NoDelay = true;
                    tcpClientSocket.ReceiveBufferSize = 1024;
                    tcpClientSocket.SendBufferSize = 1024;

                    totalConnections++;

                    loggingModule.Log($"Incoming client, creating UDP client socket..", LogMessageKind.INFO);

                    int udpPort = configModule.Config.port;
                    while (!multiplayerModule.IsPortAvailable(udpPort)) {
                        udpPort++;
                    }

                    // I don't know why C2O did this just yet
                    if (udpClientSocket != null) {
                        if (udpClientSocket.Client.Connected && udpClientSocket.Client.GetPort() == udpPort) {
                            udpClientSocket.Close();
                        }
                    }

                    udpClientSocket = new UdpClient(udpPort);

                    udpClientSocket.Client.SendBufferSize = 4096;
                    udpClientSocket.Client.ReceiveBufferSize = 4096;
                    
                    //tcpSocket.Bind(localUdpEndpoint);
                    //tcpSocket.Connect();

                    loggingModule.Log($"Client UDP socket OK on port {udpClientSocket.Client.GetPort()}..", LogMessageKind.INFO);

                    multiplayerModule.AddClient(new Client {
                        tcpClientSocket = tcpClientSocket,
                        udpClientSocket = udpClientSocket,
                        clientName = $"NewClient-{totalConnections}"
                    });
                } catch (SocketException ex) {
                    loggingModule.Log($"TCP server socket accept was interrupted, reason: {ex.Message}", LogMessageKind.ERROR);
                }
            }

            //Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //tcpSocket.
        }
    }
}
