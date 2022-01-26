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
            this.btnHost = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHost
            // 
            this.btnHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnHost.Location = new System.Drawing.Point(12, 12);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(238, 23);
            this.btnHost.TabIndex = 0;
            this.btnHost.Text = "Host";
            this.btnHost.UseVisualStyleBackColor = false;
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
            // 
            // txtIP
            // 
            this.txtIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.txtIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtIP.Location = new System.Drawing.Point(12, 41);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(164, 23);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnJoin
            // 
            this.btnJoin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnJoin.Location = new System.Drawing.Point(182, 41);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(68, 23);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // HostJoinSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(262, 80);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HostJoinSelection";
            this.Text = "C2MP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnHost;
        private TextBox txtIP;
        private Button btnJoin;
    }
}