namespace C2MP.Core.Shared.Modules.GameData {
    public class GameDataModule {
        private ConfigModule configModule;
        private LoggingModule loggingModule;
        private EventModule eventModule;

        private List<Car> carRecord;
        private List<Track> trackRecord;

        public List<Car> CarRecord {
            get { return carRecord; }
        }

        public List<Track> TrackRecord {
            get { return trackRecord; }
        }

        public GameDataModule(ConfigModule configModule, LoggingModule loggingModule, EventModule eventModule) {
            this.configModule = configModule;
            this.loggingModule = loggingModule;
            this.eventModule = eventModule;

            this.carRecord = new List<Car>();
            this.trackRecord = new List<Track>();
        }

        public void BuildCarRecord() {
            string opponentFilePath = configModule.Config.GetDataFile("TEMP_OPPONENT.TXT");
            string[] opponentLines = File.ReadAllLines(opponentFilePath);

            for (int i = 0; i < opponentLines.Length; i++) {
                string line = opponentLines[i];

                if (!line.StartsWith("// Opponent")) {
                    continue;
                }

                Car newCar = new Car();

                newCar.longDriverName = opponentLines[++i].Trim();
                newCar.shortDriverName = opponentLines[++i].Trim();
                newCar.carName = opponentLines[++i].Trim();
                newCar.strengthRating = int.Parse(opponentLines[++i].EraseComment());
                newCar.costToBuy = int.Parse(opponentLines[++i].EraseComment());
                i++; // 'availability' key, skipped
                newCar.fileName = opponentLines[++i].EraseComment();

                // Skip the vehicle description comment
                i++;

                while (opponentLines[i].Trim() != string.Empty) {
                    newCar.carDescription += opponentLines[i] + Environment.NewLine;
                    i++;
                }

                carRecord.Add(newCar);
            }

            if (carRecord.Count > 0) {
                loggingModule.Log($"Successfully built car record containing {carRecord.Count} cars");
                this.eventModule.RaiseCarRecordBuilt(this);
            } else {
                loggingModule.Log($"No cars were found in {opponentFilePath}", LogMessageKind.ERROR);
            }
        }

        public void BuildTrackRecord() {
            string racesDirectory = configModule.Config.GetDataDirectory("RACES");

            if (!Directory.Exists(racesDirectory)) {
                loggingModule.Log($"Could not find {racesDirectory}, cannot complete first time setup.", LogMessageKind.FATAL);
                return;
            }

            string[] trackFiles = Directory.GetFiles(racesDirectory);

            foreach (string trackFile in trackFiles) {
                string trackFileName = Path.GetFileNameWithoutExtension(trackFile);

                if (!trackFile.ToLower().EndsWith(".twt") || trackFileName.ToLower().EndsWith("mission") || trackFileName.ToLower().EndsWith("net")) {
                    continue;
                }

                // Make sure it contains a race TXT first, this solves a bug in C2O where it includes texture-only TWTs
                bool hasTXT = File.ReadAllText(trackFile).Contains(".TXT", StringComparison.OrdinalIgnoreCase);

                if (!hasTXT) {
                    continue;
                }

                Track track = new Track {
                    fileName = trackFileName,
                };

                trackRecord.Add(track);
            }

            if (trackRecord.Count > 0) {
                loggingModule.Log($"Successfully built track record containing {trackRecord.Count} tracks");
                this.eventModule.RaiseTrackRecordBuilt(this);
            } else {
                loggingModule.Log($"No tracks were found in {racesDirectory}", LogMessageKind.ERROR);
            }
        }
    }
}
