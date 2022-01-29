using C2MP.Core.Shared.Modules;
using System.Net.Sockets;


namespace C2MP.Core.Server.Modules.Multiplayer {
    public class Client {
        public TcpClient tcpClientSocket;
        public UdpClient udpClientSocket;
        public string clientName;

        private EventModule eventModule;

        public Client(EventModule eventModule) {
            // C2O initializes a lot of stuff here normally, but I would only add things as needed
            // so I can think about restructuring it in a slightly better way if necessary
            this.eventModule = eventModule;

            this.eventModule.RaiseSpawnServerDataTransmissionThread(this, new ClientEventArgs(this));
        }
    }
}
