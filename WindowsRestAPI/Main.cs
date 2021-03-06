﻿using Grapevine.Server;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using WindowsRestAPI.Properties;

namespace WindowsRestAPI
{
    public partial class Main : Form
    {
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        public Main()
        {
            InitializeComponent();
            DesktopNotificationManagerCompat.RegisterActivator<MyNotificationActivator.ThisNotificationActivator>();
            notify.ContextMenuStrip = taskbarMenu;
        }

        public RestServer Rest;
        public void runServer(bool state)
        {
            try
            {
                if (state)
                {
                    Rest = new RestServer();
                    Rest.Port = Settings.Default.Port;
                    Rest.Host = Settings.Default.HostIP;

                    if (Authentication.Enabled)
                    {
                        Rest.Advanced.AuthenticationSchemes = AuthenticationSchemes.Basic;
                    }
                    Rest.UseHttps = Settings.Default.HTTPS;
      
                    Rest.Start();
                    setRunningStatus();
                }
                else
                {
                    Rest.Stop();
                    Rest.Dispose();
                    setStoppedStatus();
                }
            }
            catch (Exception error)
            {               
                DialogResult result = MessageBox.Show(error.ToString(), "Unexpected error.", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        public void setStoppedStatus()
        {
            picStatus.Image = Resources.icon_stop_15;
            btnStartStop.Text = "Start";
            btnStartStop.FlatAppearance.MouseOverBackColor = Color.Green;
            mnuStartStop.Text = "Start";
            mnuStatus.Text = "Status: Stopped";
        }

        public void setRunningStatus()
        {
            picStatus.Image = Resources._02_play_2_512;
            btnStartStop.Text = "Stop";
            if (btnStartStop.Text == "Stop")
            btnStartStop.FlatAppearance.MouseOverBackColor = Color.Red;
            mnuStartStop.Text = "Stop";
            mnuStatus.Text = "Status: Running";
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Commands.CopyCommands();

            if (Rest is null)
            {
                setStoppedStatus();
                btnStartStop.Text = "Start";

            }
            else if (Rest.IsListening || Rest != null)
            {
                setRunningStatus();
                btnStartStop.Text = "Stop";
            }

            if (Settings.Default.StartServer)
            {
                runServer(true);
            }

            if (Settings.Default.StartMinimized)
            {
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notify.Visible = true;
            }
        }

        private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notify.Visible = false;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Hide();
            ShowInTaskbar = false;
            notify.Visible = true;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            config.ShowDialog();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            StartStopServer();
        }

        private void mnuStartStop_Click(object sender, EventArgs e)
        {
            StartStopServer();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }
        
        private void ExitApplication()
        {
            if (Rest is null)
            {
                Application.Exit();

            }
            else if (Rest.IsListening || Rest != null)
            {
                runServer(false);
                Application.Exit();
            }
        }
        
        private void StartStopServer()
        {
            if (Rest is null || !Rest.IsListening)
            {
                runServer(true);
                btnStartStop.FlatAppearance.MouseOverBackColor = Color.Red;
            }
            else
            {
                runServer(false);
                btnStartStop.FlatAppearance.MouseOverBackColor = Color.Green;
            }
        }
    }
}
