using C2MP.Core.Shared.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Server.Modules.Multiplayer {
    public class MultiplayerModule {
        private ConfigModule configModule;
        private LoggingModule loggingModule;
        private EventModule eventModule;

        private List<Client> clients;

        public MultiplayerModule(ConfigModule configModule, LoggingModule loggingModule, EventModule eventModule) {
            this.configModule = configModule;
            this.loggingModule = loggingModule;
            this.eventModule = eventModule;

            this.clients = new List<Client>();
        }

        public bool IsPortAvailable(int udpPort) {
            foreach (Client client in clients) {
                if (client.udpClientSocket != null && client.udpClientSocket.Client.GetPort() == udpPort) {
                    return false;
                }
            }

            return true;
        }

        public void AddClient(Client client) {
            this.clients.Add(client);
        }
    }
}
