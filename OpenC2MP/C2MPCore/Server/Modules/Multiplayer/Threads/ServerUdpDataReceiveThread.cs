using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Server.Modules.Multiplayer.Threads {
    public class ServerUdpDataReceiveThread {
        private Client client;

        public ServerUdpDataReceiveThread(Client client) {
            this.client = client;
            this.client.udpDataReceiveThread = this;
        }

        public void Run() {

        }
    }
}
