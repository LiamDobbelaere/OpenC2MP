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

        public ServerSetupThread(LoggingModule loggingModule)
        {
            this.loggingModule = loggingModule;
        }

        public void Run()
        {
            loggingModule.Log($"Welcome to C2MP {Main.version}!");
            while (true)
            {

            }
        }
    }
}
