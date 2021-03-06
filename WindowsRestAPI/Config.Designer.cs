﻿namespace WindowsRestAPI
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
            this.rdoMessageBox = new System.Windows.Forms.RadioButton();
            this.rdoNotificationCenter = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkHTTPS = new System.Windows.Forms.CheckBox();
            this.chkAuth = new System.Windows.Forms.CheckBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
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
            this.lstConfig.Size = new System.Drawing.Size(427, 390);
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
            this.btnSaveClose.Location = new System.Drawing.Point(11, 390);
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
            this.lblRun.Size = new System.Drawing.Size(65, 13);
            this.lblRun.TabIndex = 8;
            this.lblRun.Text = "Commands";
            // 
            // btnAddRun
            // 
            this.btnAddRun.FlatAppearance.BorderSize = 0;
            this.btnAddRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRun.Location = new System.Drawing.Point(117, 390);
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
            this.btnRemove.Location = new System.Drawing.Point(117, 360);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(91, 23);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove CMD";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // rdoMessageBox
            // 
            this.rdoMessageBox.AutoSize = true;
            this.rdoMessageBox.Location = new System.Drawing.Point(12, 154);
            this.rdoMessageBox.Name = "rdoMessageBox";
            this.rdoMessageBox.Size = new System.Drawing.Size(128, 17);
            this.rdoMessageBox.TabIndex = 12;
            this.rdoMessageBox.Text = "Simple MessageBox";
            this.rdoMessageBox.UseVisualStyleBackColor = true;
            // 
            // rdoNotificationCenter
            // 
            this.rdoNotificationCenter.AutoSize = true;
            this.rdoNotificationCenter.Checked = true;
            this.rdoNotificationCenter.Location = new System.Drawing.Point(11, 178);
            this.rdoNotificationCenter.Name = "rdoNotificationCenter";
            this.rdoNotificationCenter.Size = new System.Drawing.Size(121, 17);
            this.rdoNotificationCenter.TabIndex = 13;
            this.rdoNotificationCenter.TabStop = true;
            this.rdoNotificationCenter.Text = "Notification Center";
            this.rdoNotificationCenter.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Message Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Authentication/SSL";
            // 
            // chkHTTPS
            // 
            this.chkHTTPS.AutoSize = true;
            this.chkHTTPS.Location = new System.Drawing.Point(11, 240);
            this.chkHTTPS.Name = "chkHTTPS";
            this.chkHTTPS.Size = new System.Drawing.Size(58, 17);
            this.chkHTTPS.TabIndex = 16;
            this.chkHTTPS.Text = "HTTPS";
            this.chkHTTPS.UseVisualStyleBackColor = true;
            // 
            // chkAuth
            // 
            this.chkAuth.AutoSize = true;
            this.chkAuth.Location = new System.Drawing.Point(12, 263);
            this.chkAuth.Name = "chkAuth";
            this.chkAuth.Size = new System.Drawing.Size(189, 17);
            this.chkAuth.TabIndex = 17;
            this.chkAuth.Text = "Authentication (User/Pass):Basic";
            this.chkAuth.UseVisualStyleBackColor = true;
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location = new System.Drawing.Point(12, 295);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(183, 15);
            this.txtUsername.TabIndex = 18;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(12, 324);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(183, 15);
            this.txtPassword.TabIndex = 19;
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(654, 425);
            this.ControlBox = false;
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.chkAuth);
            this.Controls.Add(this.chkHTTPS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoNotificationCenter);
            this.Controls.Add(this.rdoMessageBox);
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
        private System.Windows.Forms.RadioButton rdoMessageBox;
        private System.Windows.Forms.RadioButton rdoNotificationCenter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkHTTPS;
        private System.Windows.Forms.CheckBox chkAuth;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
    }
}