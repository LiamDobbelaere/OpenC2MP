namespace C2MP {
    partial class HostJoinSelection {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostJoinSelection));
            this.pbxHost = new System.Windows.Forms.PictureBox();
            this.pbxJoin = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxHost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxJoin)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxHost
            // 
            this.pbxHost.BackgroundImage = global::C2MP.Properties.Resources.host_button;
            this.pbxHost.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxHost.Location = new System.Drawing.Point(12, 12);
            this.pbxHost.Name = "pbxHost";
            this.pbxHost.Size = new System.Drawing.Size(469, 168);
            this.pbxHost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxHost.TabIndex = 1;
            this.pbxHost.TabStop = false;
            this.pbxHost.Click += new System.EventHandler(this.pbxHost_Click);
            // 
            // pbxJoin
            // 
            this.pbxJoin.BackgroundImage = global::C2MP.Properties.Resources.join_button;
            this.pbxJoin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxJoin.Location = new System.Drawing.Point(68, 186);
            this.pbxJoin.Name = "pbxJoin";
            this.pbxJoin.Size = new System.Drawing.Size(529, 168);
            this.pbxJoin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxJoin.TabIndex = 2;
            this.pbxJoin.TabStop = false;
            this.pbxJoin.Click += new System.EventHandler(this.pbxJoin_Click);
            // 
            // HostJoinSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(609, 363);
            this.Controls.Add(this.pbxJoin);
            this.Controls.Add(this.pbxHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HostJoinSelection";
            this.Text = "C2MP";
            ((System.ComponentModel.ISupportInitialize)(this.pbxHost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxJoin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private PictureBox pbxHost;
        private PictureBox pbxJoin;
    }
}