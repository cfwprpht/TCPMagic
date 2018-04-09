namespace TCPMagic {
    partial class TCPMagic {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPMagic));
            rtb = new System.Windows.Forms.RichTextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.comboIP = new System.Windows.Forms.ComboBox();
            this.comboEncoding = new System.Windows.Forms.ComboBox();
            this.labelEncoding = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.checkServer = new System.Windows.Forms.CheckBox();
            this.checkDrop = new System.Windows.Forms.CheckBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.save2BinMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noCMsgMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.socketNameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboPort = new System.Windows.Forms.ComboBox();
            this.buttonSelFile = new System.Windows.Forms.Button();
            this.buttonSelectProfile = new System.Windows.Forms.Button();
            this.buttonSaveProfile = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb
            // 
            rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            rtb.BackColor = System.Drawing.SystemColors.WindowText;
            rtb.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            rtb.ForeColor = System.Drawing.Color.Yellow;
            rtb.Location = new System.Drawing.Point(1, 77);
            rtb.Name = "rtb";
            rtb.Size = new System.Drawing.Size(864, 613);
            rtb.TabIndex = 0;
            this.rtb.Text = "";
            this.rtb.ReadOnlyChanged += new System.EventHandler(this.Rtb_ReadOnlyChanged);
            this.rtb.TextChanged += new System.EventHandler(this.Rtb_TextChanged);
            this.rtb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Rtb_KeyDown);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(151, 34);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port:";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(12, 34);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(20, 13);
            this.labelIP.TabIndex = 3;
            this.labelIP.Text = "IP:";
            // 
            // comboIP
            // 
            this.comboIP.FormattingEnabled = true;
            this.comboIP.Location = new System.Drawing.Point(13, 50);
            this.comboIP.Name = "comboIP";
            this.comboIP.Size = new System.Drawing.Size(118, 21);
            this.comboIP.TabIndex = 4;
            this.comboIP.TextChanged += new System.EventHandler(this.ComboIP_TextChanged);
            // 
            // comboEncoding
            // 
            this.comboEncoding.FormattingEnabled = true;
            this.comboEncoding.Location = new System.Drawing.Point(234, 50);
            this.comboEncoding.Name = "comboEncoding";
            this.comboEncoding.Size = new System.Drawing.Size(71, 21);
            this.comboEncoding.TabIndex = 5;
            // 
            // labelEncoding
            // 
            this.labelEncoding.AutoSize = true;
            this.labelEncoding.Location = new System.Drawing.Point(232, 33);
            this.labelEncoding.Name = "labelEncoding";
            this.labelEncoding.Size = new System.Drawing.Size(55, 13);
            this.labelEncoding.TabIndex = 6;
            this.labelEncoding.Text = "Encoding:";
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(749, 29);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(106, 36);
            this.buttonRun.TabIndex = 7;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.ButtonRun_Click);
            // 
            // checkServer
            // 
            this.checkServer.AutoSize = true;
            this.checkServer.Location = new System.Drawing.Point(329, 34);
            this.checkServer.Name = "checkServer";
            this.checkServer.Size = new System.Drawing.Size(57, 17);
            this.checkServer.TabIndex = 8;
            this.checkServer.Text = "Server";
            this.checkServer.UseVisualStyleBackColor = true;
            this.checkServer.CheckedChanged += new System.EventHandler(this.CheckServer_CheckedChanged);
            // 
            // checkDrop
            // 
            this.checkDrop.AutoSize = true;
            this.checkDrop.Location = new System.Drawing.Point(329, 56);
            this.checkDrop.Name = "checkDrop";
            this.checkDrop.Size = new System.Drawing.Size(49, 17);
            this.checkDrop.TabIndex = 9;
            this.checkDrop.Text = "Drop";
            this.checkDrop.UseVisualStyleBackColor = true;
            this.checkDrop.CheckedChanged += new System.EventHandler(this.CheckDrop_CheckedChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.optionsMenuItem,
            this.infoMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(867, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetMenuItem,
            this.killMenuItem,
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "File";
            // 
            // resetMenuItem
            // 
            this.resetMenuItem.Name = "resetMenuItem";
            this.resetMenuItem.Size = new System.Drawing.Size(102, 22);
            this.resetMenuItem.Text = "Reset";
            this.resetMenuItem.Click += new System.EventHandler(this.ResetMenuItem_Click);
            // 
            // killMenuItem
            // 
            this.killMenuItem.Name = "killMenuItem";
            this.killMenuItem.Size = new System.Drawing.Size(102, 22);
            this.killMenuItem.Text = "Kill";
            this.killMenuItem.Click += new System.EventHandler(this.KillMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(102, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logMenuItem,
            this.save2BinMenuItem,
            this.formatMenuItem,
            this.noCMsgMenuItem,
            this.socketNameMenuItem,
            this.colorsMenuItem,
            this.fontMenuItem});
            this.optionsMenuItem.Name = "optionsMenuItem";
            this.optionsMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsMenuItem.Text = "Options";
            // 
            // logMenuItem
            // 
            this.logMenuItem.Name = "logMenuItem";
            this.logMenuItem.Size = new System.Drawing.Size(164, 22);
            this.logMenuItem.Text = "Log";
            this.logMenuItem.Click += new System.EventHandler(this.LogMenuItem_Click);
            // 
            // save2BinMenuItem
            // 
            this.save2BinMenuItem.Name = "save2BinMenuItem";
            this.save2BinMenuItem.Size = new System.Drawing.Size(164, 22);
            this.save2BinMenuItem.Text = "Save2Bin";
            this.save2BinMenuItem.Click += new System.EventHandler(this.Save2BinMenuItem_Click);
            // 
            // formatMenuItem
            // 
            this.formatMenuItem.Name = "formatMenuItem";
            this.formatMenuItem.Size = new System.Drawing.Size(164, 22);
            this.formatMenuItem.Text = "Format On";
            this.formatMenuItem.CheckedChanged += new System.EventHandler(this.FormatMenuItem_CheckedChanged);
            this.formatMenuItem.Click += new System.EventHandler(this.FormatMenuItem_Click);
            // 
            // noCMsgMenuItem
            // 
            this.noCMsgMenuItem.Name = "noCMsgMenuItem";
            this.noCMsgMenuItem.Size = new System.Drawing.Size(164, 22);
            this.noCMsgMenuItem.Text = "No Connect Msg";
            this.noCMsgMenuItem.Click += new System.EventHandler(this.NoCMsgMenuItem_Click);
            // 
            // socketNameMenuItem
            // 
            this.socketNameMenuItem.Name = "socketNameMenuItem";
            this.socketNameMenuItem.Size = new System.Drawing.Size(164, 22);
            this.socketNameMenuItem.Text = "Socket Name";
            this.socketNameMenuItem.Click += new System.EventHandler(this.SocketNameMenuItem_Click);
            // 
            // colorsMenuItem
            // 
            this.colorsMenuItem.Name = "colorsMenuItem";
            this.colorsMenuItem.Size = new System.Drawing.Size(164, 22);
            this.colorsMenuItem.Text = "Colors";
            this.colorsMenuItem.Click += new System.EventHandler(this.ColorsMenuItem_Click);
            // 
            // fontMenuItem
            // 
            this.fontMenuItem.Name = "fontMenuItem";
            this.fontMenuItem.Size = new System.Drawing.Size(164, 22);
            this.fontMenuItem.Text = "Font";
            this.fontMenuItem.Click += new System.EventHandler(this.FontMenuItem_Click);
            // 
            // infoMenuItem
            // 
            this.infoMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenuItem,
            this.helpMenuItem,
            this.registerMenuItem});
            this.infoMenuItem.Name = "infoMenuItem";
            this.infoMenuItem.Size = new System.Drawing.Size(24, 20);
            this.infoMenuItem.Text = "?";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutMenuItem.Text = "About";
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(116, 22);
            this.helpMenuItem.Text = "Help";
            this.helpMenuItem.Click += new System.EventHandler(this.HelpMenuItem_Click);
            // 
            // registerMenuItem
            // 
            this.registerMenuItem.Name = "registerMenuItem";
            this.registerMenuItem.Size = new System.Drawing.Size(116, 22);
            this.registerMenuItem.Text = "Register";
            this.registerMenuItem.Click += new System.EventHandler(this.RegisterMenuItem_Click);
            // 
            // comboPort
            // 
            this.comboPort.FormattingEnabled = true;
            this.comboPort.Location = new System.Drawing.Point(152, 50);
            this.comboPort.MaxLength = 5;
            this.comboPort.Name = "comboPort";
            this.comboPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboPort.Size = new System.Drawing.Size(57, 21);
            this.comboPort.TabIndex = 12;
            this.comboPort.TextChanged += new System.EventHandler(this.ComboPort_TextChanged);
            // 
            // buttonSelFile
            // 
            this.buttonSelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelFile.Enabled = false;
            this.buttonSelFile.Location = new System.Drawing.Point(637, 29);
            this.buttonSelFile.Name = "buttonSelFile";
            this.buttonSelFile.Size = new System.Drawing.Size(94, 36);
            this.buttonSelFile.TabIndex = 13;
            this.buttonSelFile.Text = "Select File";
            this.buttonSelFile.UseVisualStyleBackColor = true;
            this.buttonSelFile.Click += new System.EventHandler(this.ButtonSelFile_Click);
            // 
            // buttonSelectProfile
            // 
            this.buttonSelectProfile.Location = new System.Drawing.Point(392, 27);
            this.buttonSelectProfile.Name = "buttonSelectProfile";
            this.buttonSelectProfile.Size = new System.Drawing.Size(82, 23);
            this.buttonSelectProfile.TabIndex = 14;
            this.buttonSelectProfile.Text = "Select Profile";
            this.buttonSelectProfile.UseVisualStyleBackColor = true;
            this.buttonSelectProfile.Click += new System.EventHandler(this.ButtonSelectProfile_Click);
            // 
            // buttonSaveProfile
            // 
            this.buttonSaveProfile.Location = new System.Drawing.Point(392, 51);
            this.buttonSaveProfile.Name = "buttonSaveProfile";
            this.buttonSaveProfile.Size = new System.Drawing.Size(82, 23);
            this.buttonSaveProfile.TabIndex = 15;
            this.buttonSaveProfile.Text = "Save Profile";
            this.buttonSaveProfile.UseVisualStyleBackColor = true;
            this.buttonSaveProfile.Click += new System.EventHandler(this.ButtonSaveProfile_Click);
            // 
            // TCPMagic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 691);
            this.Controls.Add(this.buttonSaveProfile);
            this.Controls.Add(this.buttonSelectProfile);
            this.Controls.Add(this.buttonSelFile);
            this.Controls.Add(this.comboPort);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.checkDrop);
            this.Controls.Add(this.checkServer);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.labelEncoding);
            this.Controls.Add(this.comboEncoding);
            this.Controls.Add(this.comboIP);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.rtb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "TCPMagic";
            this.Text = "TCPMagic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPMagic_FormClosing);
            this.Load += new System.EventHandler(this.TCPMagic_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TCPMagic_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TCPMagic_DragEnter);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.ComboBox comboIP;
        private System.Windows.Forms.ComboBox comboEncoding;
        private System.Windows.Forms.Label labelEncoding;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.CheckBox checkServer;
        private System.Windows.Forms.CheckBox checkDrop;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logMenuItem;
        private System.Windows.Forms.ToolStripMenuItem save2BinMenuItem;
        private System.Windows.Forms.ComboBox comboPort;
        private System.Windows.Forms.Button buttonSelFile;
        private System.Windows.Forms.Button buttonSelectProfile;
        private System.Windows.Forms.Button buttonSaveProfile;
        private System.Windows.Forms.ToolStripMenuItem noCMsgMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatMenuItem;
        private System.Windows.Forms.ToolStripMenuItem socketNameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontMenuItem;
    }
}

