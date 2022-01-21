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

            if (!DisableTimePerPedKillAndRecoveryCost()) {
                return;
            }

            if (!PatchCrushData()) {
                return;
            }

            loggingModule.Log($"First time setup complete!", LogMessageKind.INFO);

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

        private bool DisableTimePerPedKillAndRecoveryCost() {
            loggingModule.Log($"Disable time per ped kill and recovery cost..", LogMessageKind.INFO);

            string generalPath = configModule.Config.GetDataFile("GENERAL.TXT");

            if (!File.Exists(generalPath)) {
                loggingModule.Log($"Could not find {generalPath}, cannot complete first time setup.", LogMessageKind.FATAL);
                return false;
            }

            List<string> generalData = new List<string>();
            string[] generalFileLines = File.ReadAllLines(generalPath);
            foreach (string line in generalFileLines) {
                if (line.EndsWith("// Recovery cost for each skill level")) {
                    generalData.Add("1,1,1\t\t\t\t\t// Recovery cost for each skill level");
                    continue;
                }

                if (line.EndsWith("// Time per ped kill for each skill level")) {
                    generalData.Add("0,0,0\t\t\t\t\t// Time per ped kill for each skill level");
                    continue;
                }

                generalData.Add(line);
            }

            File.Move(generalPath, configModule.Config.GetBackupFile("GENERAL.TXT"), true);

            File.WriteAllLines(generalPath, generalData);

            return true;
        }

        private bool PatchCrushData() {
            loggingModule.Log("Patching crush data..", LogMessageKind.INFO);

            string carsFolder = configModule.Config.GetDataDirectory("CARS");

            if (!Directory.Exists(carsFolder)) {
                loggingModule.Log($"Could not find {carsFolder}, cannot complete first time setup.", LogMessageKind.FATAL);
                return false;
            }

            bool foundFlapDetach = false;
            int carFileBytesLost = 0;

            // Honestly this is pretty cryptic, so I haven't refactored it much from C2O's code
            string[] carFiles = Directory.GetFiles(carsFolder);
            int carFileN = 0;
            foreach (string carFile in carFiles) {
                List<byte> outputBytes = new List<byte>();

                carFileN++;

                File.Copy(carFile, configModule.Config.GetCarBackupFile(Path.GetFileName(carFile)), true);

                byte[] carFileBytes = File.ReadAllBytes(carFile);

                for (int i = 0; i < carFileBytes.Length; i++) {
                    if (carFileBytes.Length - i >= 7 && i >= 3) {
                        if (foundFlapDetach) {
                            if (carFileBytes.MatchesTextASCII(i, "box")) {
                                carFileBytesLost += 2;

                                for (int j = 0; j < carFileBytesLost; j++) {
                                    outputBytes.Add(32); // space
                                }

                                i += 2;

                                foundFlapDetach = false;

                                carFileBytesLost = 0;
                            } else {
                                carFileBytesLost++;
                            }
                        } else if (!carFileBytes.MatchesOneOfASCII(i - 1, "/- ") && carFileBytes.MatchesTextASCII(i, "flap")) {
                            foundFlapDetach = true;

                            outputBytes.AddRange(Encoding.ASCII.GetBytes("boring"));

                            carFileBytesLost -= 2;

                            i += 2;
                        } else if (!carFileBytes.MatchesOneOfASCII(i - 1, "/- ")
                            && carFileBytes.MatchesTextASCII(i, "detach")
                            && !carFileBytes.MatchesOneOfASCII(i + 6, "?")) {

                            foundFlapDetach = true;

                            outputBytes.AddRange(Encoding.ASCII.GetBytes("boring"));

                            i += 4;
                        } else {
                            outputBytes.Add(carFileBytes[i]);
                        }
                    } else {
                        outputBytes.Add(carFileBytes[i]);
                    }
                }

                File.WriteAllBytes(carFile, outputBytes.ToArray());

                float percentage = MathF.Round((carFileN / (float)carFiles.Length) * 100.0f);

                loggingModule.Log($"Patching car crush data ({Path.GetFileName(carFile)}) {percentage}%..", LogMessageKind.STATIC);
            }

            loggingModule.Log("", LogMessageKind.STATIC);

            return true;
        }
    }
}
