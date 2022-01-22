using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Modules.GameData {
    public class GameDataModule {
        private ConfigModule configModule;
        private LoggingModule loggingModule;
        private EventModule eventModule;

        private List<Car> carRecord;

        public List<Car> CarRecord {
            get { return carRecord; }
        }

        public GameDataModule(ConfigModule configModule, LoggingModule loggingModule, EventModule eventModule) {
            this.configModule = configModule;
            this.loggingModule = loggingModule;
            this.eventModule = eventModule;

            this.carRecord = new List<Car>();
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
    }
}
