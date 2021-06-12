using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsRestAPI.Properties;


namespace WindowsRestAPI
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }
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
        public void PopulateCommandList()
        {
            lstConfig.Items.Clear();
            Commands commands = new Commands();
            if (commands.GetCommands() != null)
            {
                foreach (string s in commands.GetCommands())
                {
                    if (s == "")
                    {
                        continue;
                    }
                    else
                    {
                        lstConfig.Items.Add(commands.Decoded(s).Name);
                    }
                }
            }
        }
        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            Settings.Default.StartMinimized = chkStartMinimized.Checked;
            Settings.Default.StartServer = chkStartServer.Checked;

            Settings.Default.HTTPS = chkHTTPS.Checked;
            Settings.Default.HostIP = txtHostIP.Text;
            Settings.Default.Port = numPort.Value.ToString();

            Settings.Default.MessageBox = rdoMessageBox.Checked;
            Settings.Default.NotificationCenter = rdoNotificationCenter.Checked;

            Settings.Default.Authentication = chkAuth.Checked;
            Authentication.Enabled = chkAuth.Checked;
            Authentication.Username = txtUsername.Text;
            Authentication.Password = Authentication.EncryptString(Authentication.ToSecureString(txtPassword.Text));
            Settings.Default.Username = txtUsername.Text;
            Settings.Default.Password = Authentication.EncryptString(Authentication.ToSecureString(txtPassword.Text));

            Settings.Default.Save();
            Settings.Default.Reload();
            Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            lstConfig.Sorted = true;

            PopulateCommandList();

            chkStartMinimized.Checked = Settings.Default.StartMinimized;
            chkStartServer.Checked = Settings.Default.StartServer;

            rdoMessageBox.Checked = Settings.Default.MessageBox;
            rdoNotificationCenter.Checked = Settings.Default.NotificationCenter;

            chkHTTPS.Checked = Settings.Default.HTTPS;
            txtHostIP.Text = Settings.Default.HostIP;
            numPort.Value = Convert.ToInt64(Settings.Default.Port);

            chkAuth.Checked = Settings.Default.Authentication;
            txtUsername.Text = Settings.Default.Username;
            txtPassword.Text = Authentication.ToInsecureString(Authentication.DecryptString(Settings.Default.Password));
        }

        private void btnAddRun_Click(object sender, EventArgs e)
        {
            AddCommand addCommand = new AddCommand();
            addCommand.ShowDialog();
            PopulateCommandList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Commands command = new Commands();
            if (lstConfig.SelectedItem != null)
            {
                command.RemoveCommand(lstConfig.SelectedItem.ToString());
                PopulateCommandList();
            }
        }
    }
}
