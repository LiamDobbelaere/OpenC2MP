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
        ERROR
    }

    public class Main
    {
        public static string version = "0.1 alpha";

        public LoggingModule loggingModule = new LoggingModule();
        public Dictionary<string, Action> chatCommands;

        public Main()
        {
            this.chatCommands = new Dictionary<string, Action>()
            {
                { "exit", this.Exit }
            };
        }

        public void Run()
        {
            Thread thread = new Thread(() => new ServerSetupThread(loggingModule).Run());
            //thread.Name = "ServerSetupThread";
            thread.Start();
        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
