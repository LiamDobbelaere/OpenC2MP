using C2MP.Core.Modules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core
{
    public enum LogMessageKind
    {
        INFO,
        WARN,
        ERROR,
        INPUT
    }

    public class C2MPOptions
    {
        public bool isC2MPRunning;
    }

    public class Main
    {
        public static string version = "0.1 alpha";

        // Modules
        public LoggingModule loggingModule;
        public ConfigModule configModule;
        
        public C2MPOptions options;

        public Dictionary<string, Action> chatCommands;

        private Thread serverSetupThread;

        public Main()
        {
            loggingModule = new LoggingModule();

            // don't use the constructor for initializing stuff,
            // we want the GUI to be able to hook up their events first
            // hence why only the logging module is instantiated, so we can hook up its events
        }

        private void Initialize()
        {
            options = new C2MPOptions()
            {
                isC2MPRunning = true
            };

            configModule = new ConfigModule(loggingModule);


            this.chatCommands = new Dictionary<string, Action>()
            {
                { "configlocation", () => this.loggingModule.Log(this.configModule.ConfigFileLocation) },
                { "exit", this.Exit }
            };
        }

        public void Shutdown()
        {
            options.isC2MPRunning = false;
        }

        public void Run()
        {
            Initialize();

            serverSetupThread = new Thread(() => new ServerSetupThread(loggingModule, options).Run());
            serverSetupThread.Name = "ServerSetupThread";
            serverSetupThread.Start();
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        public void ExecuteCommand(string command)
        {
            loggingModule.Log(command, LogMessageKind.INPUT);
            chatCommands[command.Substring(1)]();
        }
    }
}
