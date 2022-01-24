using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Modules.Multiplayer {
    public class Client {
        public Socket tcpClientSocket;
        public Socket udpClientSocket;
        public string clientName;
    }
}
