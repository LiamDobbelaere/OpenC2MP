using C2MP.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core {
    internal class PerformFirstTimeSetupThread {
        private LoggingModule loggingModule;
        private ConfigModule configModule;
        private C2MPOptions options;

        public PerformFirstTimeSetupThread(LoggingModule loggingModule, ConfigModule configModule, C2MPOptions options) {
            this.loggingModule = loggingModule;
            this.configModule = configModule;
            this.options = options;
        }

        public void Run() {
            loggingModule.Log("Performing first time setup..", LogMessageKind.WARN);

            if (!EnableEagleOpponents()) {
                return;
            }
            
            // TODO: there's more to first time setup that I haven't written yet
        }

        private bool EnableEagleOpponents() {
            loggingModule.Log($"Enable Eagle opponents..", LogMessageKind.INFO);

            string opponentPath = configModule.Config.GetDataFile("OPPONENT.TXT");

            if (!File.Exists(opponentPath)) {
                loggingModule.Log($"Could not find {opponentPath}, cannot complete first time setup.", LogMessageKind.FATAL);
                return false;
            }

            List<string> opponentData = new List<string>();
            string[] opponentFileLines = File.ReadAllLines(opponentPath);
            foreach (string line in opponentFileLines) {
                if (line.StartsWith("eagle") && line.EndsWith("// Network availability ('eagle', or 'all')")) {
                    opponentData.Add("all\t\t\t\t// Network availability ('eagle', or 'all')");
                    continue;
                }

                if (line.StartsWith("-1") && line.EndsWith("// Strength rating (1-5)")) {
                    opponentData.Add("1\t\t\t\t// Strength rating (1-5)");
                    continue;
                }

                opponentData.Add(line);
            }

            File.Move(opponentPath, configModule.Config.GetBackupFile("OPPONENT.TXT"), true);

            string tempOpponentPath = configModule.Config.GetDataFile("TEMP_OPPONENT.TXT");
            File.WriteAllLines(tempOpponentPath, opponentData);

            return true;
        }
    }
}
