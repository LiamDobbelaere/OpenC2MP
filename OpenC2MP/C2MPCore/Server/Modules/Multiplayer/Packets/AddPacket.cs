using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Server.Modules.Multiplayer.Packets {
    internal class AddPacket {
        private Client client;

        public AddPacket(Client client) {
            this.client = client;
        }

        public string Pack() {
            string selectedCar = "";

            if (client.selectedCar != null) {
                selectedCar = $",selectedCar={client.selectedCar.fileName}";
            }

            return $"/add clientName={client.clientName},ipAddress={client.ipAddress}{selectedCar},carNumber={client.carNumber},isDead={client.isDead}"
                + $",opponentsKilled={client.opponentsKilled},pedestriansKilled={client.pedestriansKilled},deaths={client.deaths}"
            + $",currentStanding={client.currentStanding},points={client.points},overallStanding={client.overallStanding}"
            + $",finishedRace={client.finishedRace},finishTime={client.finishTime},isKnockedOut={client.isKnockedOut}"
            + $",knockoutTime={client.knockoutTime},isDownloading={client.isDownloading},isServer=false";
        }
    }
}
