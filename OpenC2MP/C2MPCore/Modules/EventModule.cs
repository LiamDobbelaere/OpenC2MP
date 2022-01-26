using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP.Core.Modules {
    public class EventModule {
        public event EventHandler PerformFirstTimeSetup = delegate { };
        internal void RaisePerformFirstTimeSetup(object sender) => PerformFirstTimeSetup.Invoke(sender, EventArgs.Empty);

        public event EventHandler BuildCarRecord = delegate { };
        internal void RaiseBuildCarRecord(object sender) => BuildCarRecord.Invoke(sender, EventArgs.Empty);

        public event EventHandler CarRecordBuilt = delegate { };
        internal void RaiseCarRecordBuilt(object sender) => CarRecordBuilt.Invoke(sender, EventArgs.Empty);

        public event EventHandler BuildTrackRecord = delegate { };
        internal void RaiseBuildTrackRecord(object sender) => BuildTrackRecord.Invoke(sender, EventArgs.Empty);

        public event EventHandler TrackRecordBuilt = delegate { };
        internal void RaiseTrackRecordBuilt(object sender) => TrackRecordBuilt.Invoke(sender, EventArgs.Empty);

        public event EventHandler SpawnServerListener = delegate { };
        internal void RaiseSpawnServerListener(object sender) => SpawnServerListener.Invoke(sender, EventArgs.Empty);

        public event EventHandler ShutdownServerListener = delegate { };
        internal void RaiseShutdownServerListener(object sender) => ShutdownServerListener.Invoke(sender, EventArgs.Empty);

        public event EventHandler SpawnTcpDataReceiveThread = delegate { };
        internal void RaiseSpawnTcpDataReceiveThread(object sender) => SpawnTcpDataReceiveThread.Invoke(sender, EventArgs.Empty);
    }
}
