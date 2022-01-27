using C2MP.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Threads.Clientside {
    // NOTE: Serverside also has a TcpDataReceiveThread w/ different logic! Should probably name that one ServerTcpDataReceiveThread
    // But don't confuse the two in the Java decompilation, they are obviously different as the server shouldn't run the client's logic

    internal class TcpDataReceiveThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private C2MPOptions options;

        private Action notYetImplemented = () => { }; // purely used to have one unique reference to this function
        private Dictionary<string, Action> tcpClientCommands;

        public TcpDataReceiveThread(LoggingModule loggingModule, ConfigModule configModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.options = options;

            this.tcpClientCommands = new Dictionary<string, Action>() {
                { "/start", notYetImplemented },

                { "/pinballmode", notYetImplemented },
                { "/frozenopponents", notYetImplemented },
                { "/frozencops", notYetImplemented },
                { "/drugs", notYetImplemented },
                { "/drunkdriving", notYetImplemented },
                { "/turboopponents", notYetImplemented },

                { "/timelimit", notYetImplemented },
                { "/nodrones", notYetImplemented },
                { "/drones", notYetImplemented },
                { "/nocar", notYetImplemented },
                { "/newClientName", notYetImplemented },
                { "/endtimer", notYetImplemented },
                { "/laps", notYetImplemented },
                { "/banner", notYetImplemented },
                { "/error", notYetImplemented },
                { "/nocoords", notYetImplemented },
                { "/grid", notYetImplemented },
                { "/noinvite", notYetImplemented },
                { "/invite", notYetImplemented },
                { "/udpport", notYetImplemented },
                { "/file", notYetImplemented },
                { "/cardata", notYetImplemented },
                { "/track", notYetImplemented },
                { "/mode", notYetImplemented },

                { "/norepair", notYetImplemented },
                { "/repair", notYetImplemented },
                { "/nocrushsync", notYetImplemented },
                { "/crushsync", notYetImplemented },
                { "/nocollisions", notYetImplemented },
                { "/collisions", notYetImplemented },
                { "/nopeds", notYetImplemented },
                { "/peds", notYetImplemented },
                { "/norecovery", notYetImplemented },
                { "/recovery", notYetImplemented },

                { "/car", notYetImplemented },
                { "/edit", notYetImplemented },
                { "/clients", notYetImplemented },
                { "/add", notYetImplemented },
                { "/remove", notYetImplemented }
            };
        }

        public void Run() {
            loggingModule.Log($"Connecting to {options.ip}..");

            TcpClient tcpSocket = new TcpClient();
            tcpSocket.NoDelay = true;

            // C2O does this and I don't know why yet
            tcpSocket.SendBufferSize = 1024;
            tcpSocket.ReceiveBufferSize = 1024;

            while (!tcpSocket.Connected) {
                try {
                    tcpSocket.Connect(options.ip, configModule.Config.port);
                    loggingModule.Log($"TCP connection ok with {options.ip}:{configModule.Config.port}");

                    // TODO: add compression, see decomp. Java source
                    StreamReader tcpReaderStream = new StreamReader(new BufferedStream(tcpSocket.GetStream(), 1024));

                    try {
                        while (options.isC2MPRunning) {
                            string line = tcpReaderStream.ReadLine();
                            string command = line.Split(' ')[0];

                            Action commandAction = tcpClientCommands[command];

                            if (commandAction != null) {
                                if (commandAction == notYetImplemented) {
                                    loggingModule.Log($"Command received that is not yet implemented: {line}", LogMessageKind.WARN);
                                }

                                commandAction();
                            } else {
                                loggingModule.Log($"Unknown command received over TCP: {line}", LogMessageKind.WARN);
                            }
                        }
                    } catch (Exception ex) {
                        loggingModule.LogException($"Error occurred during TCP receive loop", ex, LogMessageKind.FATAL);
                    }
                } catch (Exception ex) {
                    loggingModule.LogException($"Could not establish a TCP connection with {options.ip}:{configModule.Config.port}", ex, LogMessageKind.ERROR);
                    tcpSocket.Close();
                }
            }
        }
    }
}
