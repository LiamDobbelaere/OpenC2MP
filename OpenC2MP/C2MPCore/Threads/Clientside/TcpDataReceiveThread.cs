using C2MP.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Threads.Clientside {
    internal class TcpDataReceiveThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private C2MPOptions options;

        public TcpDataReceiveThread(LoggingModule loggingModule, ConfigModule configModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.options = options;
        }

        public void Run() {
            loggingModule.Log($"Connecting to {options.ip}..");

            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.NoDelay = true;

            // C2O does this and I don't know why yet
            tcpSocket.SendBufferSize = 1024;
            tcpSocket.ReceiveBufferSize = 1024;

            while (!tcpSocket.Connected) {
                try {
                    tcpSocket.Connect(options.ip, configModule.Config.port);
                    loggingModule.Log($"TCP connection ok with {options.ip}:{configModule.Config.port}");
                } catch (Exception ex) {
                    loggingModule.Log($"Could not establish a TCP connection with {options.ip}:{configModule.Config.port}", LogMessageKind.ERROR);
                }
            }
        }
    }
}
