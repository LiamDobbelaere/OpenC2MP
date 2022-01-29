using C2MP.Core.Shared.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Server.Modules.Multiplayer.Threads {
    public class ServerTcpDataReceiveThread {
        private Client client;
        private LoggingModule loggingModule;
        private MultiplayerModule multiplayerModule;

        private StreamReader tcpSocketReader;

        public ServerTcpDataReceiveThread(Client client, LoggingModule loggingModule, MultiplayerModule multiplayerModule) {
            this.client = client;
            this.client.tcpDataReceiveThread = this;

            this.loggingModule = loggingModule;
            this.multiplayerModule = multiplayerModule;
        }
        
        public void Run() {
            try {
                // TODO: add compression, see decomp. Java source
                tcpSocketReader = new StreamReader(new BufferedStream(client.tcpClientSocket.GetStream(), 1024));
            } catch (Exception ex) {
                loggingModule.LogException("Unable to connect to the client (TCP read error)", ex, LogMessageKind.ERROR);
            }

            client.connected = true;

            while (client.connected) {
                try {
                    string tcpMessage = tcpSocketReader.ReadLine();

                    if (tcpMessage == null) {

                    }
                } catch (Exception ex) {
                    if (client.connected) {
                        multiplayerModule.RemoveClient(client);

                        multiplayerModule.ResetClientCarNumbers();

                        // TODO: broadcast message to remove client

                        // TODO: other stuff, see Java decomp.

                        client.connected = false;

                        // TODO: actually need to broadcast this event to everyone
                        loggingModule.LogException($"{client.clientName} has left (TCP read error)", ex, LogMessageKind.ERROR);
                    }
                }
            }
        }
    }
}
