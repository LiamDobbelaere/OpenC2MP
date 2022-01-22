using C2MP.Core;
using C2MP.Core.Modules;
using C2MP.Core.Modules.GameData;
using ToxicRagers.Stainless.Formats;
using ToxicRagers.Carmageddon2.Formats;
using Car = C2MP.Core.Modules.GameData.Car;
using ToxicRagers.Carmageddon.Formats;

namespace C2MP {
    public partial class MainForm : Form {
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

        private void MainForm_Load(object sender, EventArgs e) {
            this.boldFont = new Font(this.rtbLog.Font, FontStyle.Bold);

            this.main = new Main();
            this.main.loggingModule.LogMessage += Main_LogMessage;
            this.main.eventModule.CarRecordBuilt += EventModule_CarRecordBuilt;
            this.main.Run();

            AutoCompleteStringCollection chatCommands = new AutoCompleteStringCollection();
            foreach (string chatCommandKey in this.main.chatCommands.Keys) {
                chatCommands.Add($"/{chatCommandKey}");
            }

            txtChatCommands.AutoCompleteCustomSource = chatCommands;
            txtChatCommands.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtChatCommands.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
            this.main.Shutdown();
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

            // think there's a bug in toxicRagers that makes pixies have a missing first byte ._.
            byte[] correctedPixies = new byte[pixiesEntry.Data.Length + 1];
            Array.Copy(pixiesEntry.Data, 0, correctedPixies, 1, pixiesEntry.Data.Length);

            Stream pixiesStream = new MemoryStream(correctedPixies);
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
            List<PIXIE> filteredPixies = pData.Pixies.FindAll((pixie) => validCarPicNames.Contains(pixie.Name.Split('.')[0]));
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
    }
}