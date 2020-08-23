using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsRestAPI
{
    public partial class AddCommand : Form
    {
        public AddCommand()
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
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\";
            file.Filter = "exe files (*.exe)|*.exe";
            file.ShowDialog();

            txtPath.Text = file.FileName;
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            Commands command = new Commands();

            if (txtPath.Text.Length > 0 && txtName.Text.Length > 0)
            {
                command.AddCommand(txtName.Text, txtPath.Text, txtArguments.Text);
                Close();
            }
            Close();            
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }
    }
}
