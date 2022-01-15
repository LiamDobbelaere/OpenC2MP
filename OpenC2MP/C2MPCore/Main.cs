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
        public const string version = "0.1 alpha";

        public Dictionary<string, Action> chatCommands;

        public delegate void LogMessageEventHandler(object sender, string message, LogMessageKind kind = LogMessageKind.INFO);
        public event LogMessageEventHandler LogMessage = delegate { };

        public Main()
        {
            this.chatCommands = new Dictionary<string, Action>()
            {
                { "exit", this.Exit }
            };
        }

        public void Run()
        {
            LogMessage.Invoke(this, $"Welcome to C2MP {version}!");
        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
