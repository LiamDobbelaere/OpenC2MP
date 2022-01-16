using C2MP.Core;
using C2MP.Core.Modules;

namespace C2MP {
    public partial class MainForm : Form {
        private Main main;
        private Font boldFont;
        private Color infoColor = Color.FromArgb(250, 250, 250);
        private Color errorColor = Color.FromArgb(255, 128, 128);
        private Color warnColor = Color.FromArgb(255, 255, 16);
        private Color inputColor = Color.FromArgb(16, 255, 255);

        public MainForm() {
            InitializeComponent();
        }

        private void Main_LogMessage(object sender, string message, LogMessageKind kind = LogMessageKind.INFO) {
            Log(message, LogMessageKindToColor(kind));
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

        private void Log(string message, Color color) {
            this.rtbLog.Invoke(() => {
                this.rtbLog.SuspendLayout();
                this.rtbLog.SelectionColor = color;
                this.rtbLog.SelectionFont = this.boldFont;
                this.rtbLog.AppendText($">> {message}{Environment.NewLine}");
                this.rtbLog.ScrollToCaret();
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
            this.main.Run();

            AutoCompleteStringCollection chatCommands = new AutoCompleteStringCollection();
            foreach (string chatCommandKey in this.main.chatCommands.Keys) {
                chatCommands.Add($"/{chatCommandKey}");
            }

            txtChatCommands.AutoCompleteCustomSource = chatCommands;
            txtChatCommands.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtChatCommands.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            this.main.Shutdown();
        }
    }
}