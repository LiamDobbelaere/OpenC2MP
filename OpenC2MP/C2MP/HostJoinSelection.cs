using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C2MP {
    public partial class HostJoinSelection : Form {
        public HostJoinSelection() {
            InitializeComponent();
        }

        private void btnHost_Click(object sender, EventArgs e) {
            MainForm form = new MainForm();
            form.chosenSettings.role = Role.HOST;
            form.chosenSettings.ip = String.Empty;
            form.hostJoinSelection = this;
            form.Show();
            this.Hide();
        }

        private void btnJoin_Click(object sender, EventArgs e) {
            MainForm form = new MainForm();
            form.chosenSettings.role = Role.JOIN;
            form.chosenSettings.ip = txtIP.Text;
            form.hostJoinSelection = this;
            form.Show();
            this.Hide();
        }
    }
}
