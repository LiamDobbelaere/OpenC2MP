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

        private void pbxHost_Click(object sender, EventArgs e) {
            MainForm form = new MainForm();
            form.chosenRole = ChosenRole.HOST;
            form.hostJoinSelection = this;
            form.Show();
            this.Hide();
        }

        private void pbxJoin_Click(object sender, EventArgs e) {
            MainForm form = new MainForm();
            form.chosenRole = ChosenRole.JOIN;
            form.hostJoinSelection = this;
            form.Show();
            this.Hide();
        }
    }
}
