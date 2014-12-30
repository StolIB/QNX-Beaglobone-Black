using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace clientPWM
{
    public partial class Form1 : Form
    {
        UdpClient udpClient;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            udpClient = new UdpClient();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendF();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                udpClient.Connect(hostTxt.Text, Convert.ToInt32(portTxt.Text));

            }
            catch
            {
                log("Błąd przy łaczeniu z hostem");
            }
            if (udpClient.Client.Connected) {
                log("Połączono");
            }
        }

        private void log(string txt) {
            console.Text += txt + Environment.NewLine;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int val = trackBar1.Value;
            if (radio1.Checked)
                freqTxt.Text = val.ToString();
            else if (radio10.Checked)
                freqTxt.Text = (val * 10).ToString();
            else if (radio100.Checked)
                freqTxt.Text = (val * 100).ToString();
            else if (radio1000.Checked)
                freqTxt.Text = (val * 1000).ToString();
        }

        private void radio1_CheckedChanged(object sender, EventArgs e)
        {
            freqTxt.Text = trackBar1.Value.ToString();
        }

        private void radio10_CheckedChanged(object sender, EventArgs e)
        {
            freqTxt.Text = (trackBar1.Value*10).ToString();
        }

        private void radio100_CheckedChanged(object sender, EventArgs e)
        {
            freqTxt.Text = (trackBar1.Value * 100).ToString();
        }

        private void radio1000_CheckedChanged(object sender, EventArgs e)
        {
            freqTxt.Text = (trackBar1.Value * 1000).ToString();
        }

        private void freqTxt_TextChanged(object sender, EventArgs e)
        {
            SendF();
        }
        private void SendF() {
            if (freqTxt.Text.Length > 0)
            {
                int Val = 1;
                try
                {
                    Val = Convert.ToInt32(freqTxt.Text);
                }
                catch
                {
                    log("Częstotilowść musi byc liczba całkowitą");
                }
                byte[] intBytes = BitConverter.GetBytes(Val);
                Array.Reverse(intBytes);
                byte[] outStream = intBytes;
                try
                {
                    udpClient.Send(outStream, outStream.Length);
                }
                catch (Exception exc)
                {
                    log(exc.ToString());
                }

            }
        }
    }
}
