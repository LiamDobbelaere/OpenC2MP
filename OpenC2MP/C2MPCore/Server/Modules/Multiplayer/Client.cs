using C2MP.Core.Server.Modules.Multiplayer.Threads;
using C2MP.Core.Shared.Modules;
using C2MP.Core.Shared.Modules.GameData;
using System.Net.Sockets;


namespace C2MP.Core.Server.Modules.Multiplayer {
    public class Client {
        public TcpClient tcpClientSocket;
        public UdpClient udpClientSocket;
        
        public string clientName;
        public string ipAddress;
        public int carNumber;
        public bool isDead;
        public int opponentsKilled;
        public int pedestriansKilled;
        public int deaths;
        public int currentStanding;
        public int points;
        public int overallStanding;
        public bool finishedRace;
        public float finishTime;
        public bool isKnockedOut;
        public float knockoutTime;
        public bool isDownloading;

        public ServerTcpDataReceiveThread tcpDataReceiveThread;
        public ServerUdpDataReceiveThread udpDataReceiveThread;
        public ServerDataTransmissionThread dataTransmissionThread;

        public Car selectedCar;
        public bool connected;

        private EventModule eventModule;

        public Client(EventModule eventModule, TcpClient tcpClientSocket, UdpClient udpClientSocket, string clientName) {
            // C2O initializes a lot of stuff here normally, but I would only add things as needed
            // so I can think about restructuring it in a slightly better way if necessary
            this.eventModule = eventModule;
            
            this.clientName = clientName;
            this.tcpClientSocket = tcpClientSocket;
            this.udpClientSocket = udpClientSocket;

            this.ipAddress = tcpClientSocket.Client.RemoteEndPoint.ToString();

            this.eventModule.RaiseSpawnServerDataTransmissionThread(this, new ClientEventArgs(this));
        }

        public void SendMessage(string message, bool isUdp, bool isError) {
            this.dataTransmissionThread.SendMessage(message, isUdp, isError);
        }
    }
}
