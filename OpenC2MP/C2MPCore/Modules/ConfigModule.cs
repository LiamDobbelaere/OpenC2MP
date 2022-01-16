namespace C2MP.Core.Modules {
    public class C2MPConfig {
        // The values that are set here are the default values
        public string nickname = "BuzzLightweight";
        public string gamePath = "C:\\Games\\Carmageddon II";
        public string masterServerAddress = "http://c2mp.liam.host";

        // c2o compat. not totally sure if these will be used yet
        public bool usingMod = false;
        public string modName = String.Empty;
        public string gameVersion = "USH";
        public string glideWrapperPath = "C:\\GlideWrapper";
        public short recoveryKey = 64;

        public string GetDataFile(string filename) {
            return Path.Combine(gamePath, filename);
        }

        public string GetExecutable() {
            return Path.Combine(gamePath, "CARMA2_HW.exe");
        }
    }

    public class ConfigModule {
        public string ConfigFileLocation {
            get { return configFileLocation; }
        }

        public C2MPConfig Config {
            get { return config; }
        }

        private const string configFileName = "C2MP_config.txt";
        private string configFileLocation = Path.Combine(Directory.GetCurrentDirectory(), configFileName);

        private C2MPConfig config = new C2MPConfig();
        private LoggingModule loggingModule;

        public ConfigModule(LoggingModule loggingModule) {
            this.loggingModule = loggingModule;

            LoadConfig();
        }

        private void LoadConfig() {

            if (!File.Exists(configFileLocation)) {
                loggingModule.Log($"Config file {configFileName} did not exist, creating..", LogMessageKind.WARN);

                WriteConfig();
            }

            string[] configOptions = File.ReadAllLines(configFileLocation);
            List<string> seenConfigOptions = new List<string>();

            int currentLine = 0;
            foreach (string configOption in configOptions) {
                currentLine++;

                int spaceIndex = configOption.IndexOf('=');

                if (spaceIndex == -1) {
                    if (configOption.Trim() != String.Empty) {
                        loggingModule.Log($"Unknown config setting at line {currentLine} in {configFileName}, ignoring..",
                            LogMessageKind.WARN);
                    }

                    continue;
                }

                string configKey = configOption.Substring(0, spaceIndex).Trim().ToLower();
                string configValue = configOption.Substring(configOption.IndexOf('=') + 1).Trim();

                if (seenConfigOptions.Contains(configKey)) {
                    loggingModule.Log($"The key {configKey} appears more than once in {configFileName}! This is probably unintentional.",
                        LogMessageKind.WARN);
                }

                // TODO: deal with missing keys

                switch (configKey) {
                    case "nickname":
                        config.nickname = configValue;
                        break;
                    case "gamepath":
                        config.gamePath = configValue;

                        if (!File.Exists(config.GetExecutable())) {
                            loggingModule.Log($"The game executable was not found at {config.GetExecutable()}! Please fix the configuration and try again.",
                                LogMessageKind.FATAL);
                            return;
                        }
                        break;
                    case "masterserveraddress":
                        config.masterServerAddress = configValue;
                        break;
                    case "usingmod":
                        config.usingMod = Convert.ToBoolean(Convert.ToInt32(configValue));
                        break;
                    case "modname":
                        config.modName = configValue;
                        break;
                    case "gameversion":
                        config.gameVersion = configValue;
                        break;
                    case "glidewrapperpath":
                        config.glideWrapperPath = configValue;
                        break;
                    case "recoverykey":
                        config.recoveryKey = Convert.ToInt16(configValue);
                        break;
                    default:
                        loggingModule.Log($"Unknown key {configKey} in {configFileName}", LogMessageKind.WARN);
                        break;
                }

                seenConfigOptions.Add(configKey);
            }
        }

        private void WriteConfig() {
            StreamWriter stream = File.CreateText(configFileLocation);
            stream.WriteLine($"nickname={config.nickname}");
            stream.WriteLine($"gamePath={config.gamePath}");
            stream.WriteLine($"masterServerAddress={config.masterServerAddress}");
            stream.WriteLine($"usingMod={Convert.ToInt32(config.usingMod)}");
            stream.WriteLine($"modName={config.modName}");
            stream.WriteLine($"gameVersion={config.gameVersion}");
            stream.WriteLine($"glideWrapperPath={config.glideWrapperPath}");
            stream.WriteLine($"recoveryKey={config.recoveryKey}");

            stream.Close();
        }
    }
}
