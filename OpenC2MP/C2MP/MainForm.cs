using C2MP.Core;
using C2MP.Core.Modules;
using C2MP.Core.Modules.GameData;
using C2MP.ToxicRagers;

namespace C2MP {
    public partial class MainForm : Form {
        // TODO: use this to make C2MPCore's main() spawn ClientSetupThread instead of ServerSetupThread
        public ChosenSettings chosenSettings = new ChosenSettings();
        public HostJoinSelection hostJoinSelection;

        private Main main;
        private Font boldFont;
        private Color infoColor = Color.FromArgb(250, 250, 250);
        private Color errorColor = Color.FromArgb(255, 128, 128);
        private Color warnColor = Color.FromArgb(255, 255, 16);
        private Color inputColor = Color.FromArgb(16, 255, 255);
        private Color prefixColor = Color.FromArgb(64, 64, 64);

        private double formOpacity = 0.98;

        public MainForm() {
            InitializeComponent();
#if DEBUG
            this.Opacity = formOpacity;
#endif
        }

        private void Main_LogMessage(object sender, string message, string prefix, LogMessageKind kind = LogMessageKind.INFO) {
            if (kind == LogMessageKind.STATIC) {
                lblStaticLog.Text = message;
            } else {
                Log(message, prefix, LogMessageKindToColor(kind));
            }
        }

        private Color LogMessageKindToColor(LogMessageKind kind) {
            switch (kind) {
                case LogMessageKind.INFO:
                    return infoColor;
                case LogMessageKind.ERROR:
                    return errorColor;
                case LogMessageKind.WARN:
                    return warnColor;
                case LogMessageKind.INPUT:
                    return inputColor;
                case LogMessageKind.FATAL:
                    return errorColor;
                default:
                    return Color.Cyan;
            }
        }

        private void Log(string message, string prefix, Color color) {
            this.rtbLog.Invoke(() => {
                if (this.rtbLog == null || this.rtbLog.IsDisposed) {
                    return;
                }

                this.rtbLog.SuspendLayout();
                this.rtbLog.SelectionColor = color;
                this.rtbLog.SelectionFont = this.boldFont;
                this.rtbLog.AppendText($">> {message}");
                this.rtbLog.ScrollToCaret();

                if (prefix.Length > 0) {
                    this.rtbLog.SuspendLayout();
                    this.rtbLog.SelectionColor = prefixColor;
                    this.rtbLog.SelectionFont = this.boldFont;
                    this.rtbLog.AppendText($"\t\t\t\t{prefix} {Environment.NewLine}");
                    this.rtbLog.ScrollToCaret();
                } else {
                    this.rtbLog.AppendText(Environment.NewLine);
                }

                this.rtbLog.ResumeLayout();
            });
        }

        private void txtChatCommands_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (txtChatCommands.Text.StartsWith("/")) {
                    main.ExecuteCommand(txtChatCommands.Text);
                }

                txtChatCommands.Clear();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private class TrackComboBoxItem {
            public Track Value { get; set; }

            public override string ToString() {
                return $"{Value.fileName}";
            }
        }

        private void EventModule_TrackRecordBuilt(object? sender, EventArgs e) {
            this.cboTracks.Invoke(() => {
                this.cboTracks.Items.Clear();

                foreach (Track track in main.gameDataModule.TrackRecord) {
                    this.cboTracks.Items.Add(new TrackComboBoxItem {
                        Value = track
                    });
                }

                this.cboTracks.SelectedIndex = 0;
            });
        }

        private class CarComboBoxItem {
            public Car Value { get; set; }

            public override string ToString() {
                return $"{Value.carName.ToTitleCase()} ({Value.longDriverName.ToTitleCase()})";
            }
        }

        private void EventModule_CarRecordBuilt(object? sender, EventArgs e) {
            this.cboCars.Invoke(() => {
                this.cboCars.Items.Clear();

                foreach (Car car in main.gameDataModule.CarRecord) {
                    this.cboCars.Items.Add(new CarComboBoxItem {
                        Value = car
                    });
                }

                this.cboCars.SelectedIndex = 0;
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            this.main.loggingModule.LogMessage -= Main_LogMessage;
            this.main.Shutdown();
            
            if (this.hostJoinSelection != null) {
                this.hostJoinSelection.Close();
            }
        }

        private void tmrShow_Tick(object sender, EventArgs e) {
            this.Opacity = formOpacity;
        }

        private void MainForm_Shown(object sender, EventArgs e) {
#if !DEBUG
            new SplashScreen().Show();
            this.tmrShow.Start();
#endif
        }

        private void cboCars_SelectedIndexChanged(object sender, EventArgs e) {
            CarComboBoxItem item = (CarComboBoxItem) cboCars.SelectedItem;

            if (item != null) {
                try {
                    pbxCarImage.Image = GetCarImage(item.Value);
                } catch {
                    pbxCarImage.Image = null;
                }
            }
        }

        private Bitmap GetCarImage(Car car) {
            // Haha, this is a mess, holy shit

            string simpleCarName = car.fileName.Split('.')[0].ToLower();
            string carTWTLocation = Path.Join(main.configModule.Config.GetDataDirectory("INTRFACE"), "CarImage", $"{simpleCarName}CI.TWT");

            TWT carImageTWT = TWT.Load(carTWTLocation);
            TWTEntry pixiesEntry = carImageTWT.Contents.Find((entry) => entry.Name.EndsWith(".P16"));

            Stream pixiesStream = new MemoryStream(pixiesEntry.Data);
            PIX pData = PIX.Load(pixiesStream);
            Bitmap result = new Bitmap(64 * 3, 64 * 3);
            Graphics g = Graphics.FromImage(result);

            string[] validCarPicNames = new string[] {
                simpleCarName + "a",
                simpleCarName + "b",
                simpleCarName + "c",
                simpleCarName + "d",
                simpleCarName + "e",
                simpleCarName + "f"
            };
            List<PIXIE> filteredPixies = pData.Pixies.FindAll((pixie) => validCarPicNames.Contains(pixie.Name.Split('.')[0].ToLower()));
            filteredPixies.Sort((a, b) => a.Name.CompareTo(b.Name));

            int currentPixieIndex = 0;
            for (int y = 0; y < 2; y++) {
                for (int x = 0; x < 3; x++) {
                    PIXIE pixie = filteredPixies[currentPixieIndex];

                    g.DrawImage(pixie.GetBitmap(), x * 64, y * 64);

                    currentPixieIndex++;
                }
            }
            
            return result;
        }

        private Bitmap GetTrackImage(Track track) {
            // Haha, this is also a mess, holy shit

            string trackTWTLocation = Path.Join(main.configModule.Config.GetDataDirectory("RACES"), $"{track.fileName}.TWT");

            TWT trackTWT = TWT.Load(trackTWTLocation);
            TWTEntry pixiesEntry = trackTWT.Contents.Find((entry) => entry.Name.EndsWith(".P16"));

            Stream pixiesStream = new MemoryStream(pixiesEntry.Data);
            PIX pData = PIX.Load(pixiesStream);
            PIXIE trackMap = pData.Pixies.Find((pixie) => pixie.Name.ToLower().Contains("map"));
            if (trackMap == null) {
                return null;
            }

            return trackMap.GetBitmap();
        }

        private void cboTracks_SelectedIndexChanged(object sender, EventArgs e) {
            TrackComboBoxItem item = (TrackComboBoxItem)cboTracks.SelectedItem;

            if (item != null) {
                try {
                    pbxTrackImage.Image = GetTrackImage(item.Value);
                } catch {
                    //main.loggingModule.Log(ex.Message, LogMessageKind.ERROR);
                    pbxTrackImage.Image = null;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            this.boldFont = new Font(this.rtbLog.Font, FontStyle.Bold);

            if (chosenSettings.role == Role.JOIN) {
                gboSettings.Text += " (Read-only)";
                cboTracks.Hide();

                lblMode.Hide();
                cboMode.Hide();
            }

            this.main = new Main();
            this.main.loggingModule.LogMessage += Main_LogMessage;
            this.main.eventModule.CarRecordBuilt += EventModule_CarRecordBuilt;
            this.main.eventModule.TrackRecordBuilt += EventModule_TrackRecordBuilt;
            this.main.Run(chosenSettings.role == Role.HOST, chosenSettings.ip);

            AutoCompleteStringCollection chatCommands = new AutoCompleteStringCollection();
            foreach (string chatCommandKey in this.main.chatCommands.Keys) {
                chatCommands.Add($"/{chatCommandKey}");
            }

            txtChatCommands.AutoCompleteCustomSource = chatCommands;
            txtChatCommands.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtChatCommands.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
    }
}