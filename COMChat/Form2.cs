using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMChat
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            ActiveControl = Nickname;
        }
        private void TryConnect()
        {
            DialogResult = DialogResult.None;
            if (string.IsNullOrWhiteSpace(Nickname.Text))
            {
                MessageBox.Show("Please enter your nickname.");
                return;
            }
            ((Form1)Owner).nickname = Nickname.Text;

            try
            {
                ((Form1)Owner).serialPort1.Open();
                ((Form1)Owner).connected = true;
            }
            catch
            {
                MessageBox.Show("Error opening COM Port, try closing all programs using it.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            TryConnect();
        }

        private void Nickname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                TryConnect();
        }
    }
}
