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
    public partial class Form1 : Form
    {
        public bool connected;
        public string nickname;
        public Form1()
        {
            InitializeComponent();

            connected = false;

            ActiveControl = Input;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            TryConnect();
        }

        private void TryConnect()
        {
            if (connected)
                return;

            Form2 form2 = new Form2();
            form2.ShowDialog(this);
            form2.Dispose();
        }

        private void EnvoyMessage(string text)
        {
            if (!connected)
            {
                Chat.AppendText("Please connect before chatting.\r\n");
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
                return;
        
            Input.Text = "";
            serialPort1.WriteLine(string.Format("{0}: {1}", nickname, text));
            Chat.AppendText(string.Format("You: {0}\r\n", text));
        }

        delegate void ReceiveMessageCallback(string text);
        private void ReceiveMessage(string text)
        {
            if (InvokeRequired)
            {
                ReceiveMessageCallback rmc = new ReceiveMessageCallback(ReceiveMessage);
                Invoke(rmc, new object[] { text });
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
                return;

            Chat.AppendText(string.Format("{0}\r\n", text));
        }
        private void Enter_Click(object sender, EventArgs e)
        {
            EnvoyMessage(Input.Text);
        }

        private void Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                EnvoyMessage(Input.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connected = false;
            serialPort1.Close();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (!connected)
                return;

            System.IO.Ports.SerialPort serialPort = (System.IO.Ports.SerialPort)sender;
            string inp = serialPort.ReadLine();
            ReceiveMessage(inp);
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            connected = false;
            serialPort1.Close();
            TryConnect();
        }
    }
}
