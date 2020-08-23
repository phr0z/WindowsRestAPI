namespace WindowsRestAPI
{
    partial class Config
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Config));
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.chkStartServer = new System.Windows.Forms.CheckBox();
            this.lstConfig = new System.Windows.Forms.ListBox();
            this.txtHostIP = new System.Windows.Forms.TextBox();
            this.btnSaveClose = new System.Windows.Forms.Button();
            this.lblHostIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblRun = new System.Windows.Forms.Label();
            this.btnAddRun = new System.Windows.Forms.Button();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Location = new System.Drawing.Point(12, 72);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(167, 17);
            this.chkStartMinimized.TabIndex = 0;
            this.chkStartMinimized.Text = "Start Application Minimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // chkStartServer
            // 
            this.chkStartServer.AutoSize = true;
            this.chkStartServer.Location = new System.Drawing.Point(12, 95);
            this.chkStartServer.Name = "chkStartServer";
            this.chkStartServer.Size = new System.Drawing.Size(183, 17);
            this.chkStartServer.TabIndex = 1;
            this.chkStartServer.Text = "Start Rest Server Automatically";
            this.chkStartServer.UseVisualStyleBackColor = true;
            // 
            // lstConfig
            // 
            this.lstConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lstConfig.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstConfig.ForeColor = System.Drawing.Color.White;
            this.lstConfig.FormattingEnabled = true;
            this.lstConfig.Location = new System.Drawing.Point(215, 25);
            this.lstConfig.Name = "lstConfig";
            this.lstConfig.Size = new System.Drawing.Size(427, 247);
            this.lstConfig.TabIndex = 2;
            // 
            // txtHostIP
            // 
            this.txtHostIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtHostIP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHostIP.ForeColor = System.Drawing.Color.White;
            this.txtHostIP.Location = new System.Drawing.Point(12, 27);
            this.txtHostIP.Name = "txtHostIP";
            this.txtHostIP.Size = new System.Drawing.Size(100, 15);
            this.txtHostIP.TabIndex = 4;
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.FlatAppearance.BorderSize = 0;
            this.btnSaveClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveClose.Location = new System.Drawing.Point(12, 253);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(100, 23);
            this.btnSaveClose.TabIndex = 5;
            this.btnSaveClose.Text = "Save && Close";
            this.btnSaveClose.UseVisualStyleBackColor = true;
            this.btnSaveClose.Click += new System.EventHandler(this.btnSaveClose_Click);
            // 
            // lblHostIP
            // 
            this.lblHostIP.AutoSize = true;
            this.lblHostIP.Location = new System.Drawing.Point(42, 9);
            this.lblHostIP.Name = "lblHostIP";
            this.lblHostIP.Size = new System.Drawing.Size(43, 13);
            this.lblHostIP.TabIndex = 6;
            this.lblHostIP.Text = "Host IP";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(138, 9);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(28, 13);
            this.lblPort.TabIndex = 7;
            this.lblPort.Text = "Port";
            // 
            // lblRun
            // 
            this.lblRun.AutoSize = true;
            this.lblRun.Location = new System.Drawing.Point(384, 9);
            this.lblRun.Name = "lblRun";
            this.lblRun.Size = new System.Drawing.Size(87, 13);
            this.lblRun.TabIndex = 8;
            this.lblRun.Text = "Run Commands";
            // 
            // btnAddRun
            // 
            this.btnAddRun.FlatAppearance.BorderSize = 0;
            this.btnAddRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRun.Location = new System.Drawing.Point(118, 253);
            this.btnAddRun.Name = "btnAddRun";
            this.btnAddRun.Size = new System.Drawing.Size(91, 23);
            this.btnAddRun.TabIndex = 9;
            this.btnAddRun.Text = "Add CMD";
            this.btnAddRun.UseVisualStyleBackColor = true;
            this.btnAddRun.Click += new System.EventHandler(this.btnAddRun_Click);
            // 
            // numPort
            // 
            this.numPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.numPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numPort.ForeColor = System.Drawing.Color.White;
            this.numPort.Location = new System.Drawing.Point(128, 25);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(56, 18);
            this.numPort.TabIndex = 10;
            // 
            // btnRemove
            // 
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Location = new System.Drawing.Point(118, 223);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(91, 23);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove CMD";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(654, 283);
            this.ControlBox = false;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.btnAddRun);
            this.Controls.Add(this.lblRun);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblHostIP);
            this.Controls.Add(this.btnSaveClose);
            this.Controls.Add(this.txtHostIP);
            this.Controls.Add(this.lstConfig);
            this.Controls.Add(this.chkStartServer);
            this.Controls.Add(this.chkStartMinimized);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Config";
            this.Text = "Windows Rest API: Config";
            this.Load += new System.EventHandler(this.Config_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkStartMinimized;
        private System.Windows.Forms.CheckBox chkStartServer;
        public System.Windows.Forms.ListBox lstConfig;
        private System.Windows.Forms.TextBox txtHostIP;
        private System.Windows.Forms.Button btnSaveClose;
        private System.Windows.Forms.Label lblHostIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblRun;
        private System.Windows.Forms.Button btnAddRun;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Button btnRemove;
    }
}