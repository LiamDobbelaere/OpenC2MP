using C2MP.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core
{
    internal class ServerSetupThread
    {
        private LoggingModule loggingModule;
        private C2MPOptions options;

        public ServerSetupThread(LoggingModule loggingModule, C2MPOptions options)
        {
            this.loggingModule = loggingModule;
            this.options = options;
        }

        public void Run()
        {
            loggingModule.Log($"Welcome to C2MP {Main.version}!");
            while (options.isC2MPRunning)
            {

            }
        }
    }
}
