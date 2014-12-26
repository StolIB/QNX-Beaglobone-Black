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
using System.Threading;

namespace client2
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        Thread thread;
        int i = 0;
        bool clear;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void msg(string mesg)
        {
            textBox5.Text = textBox5.Text + Environment.NewLine + " >> " + mesg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(getData));
            try
            {
                clientSocket.Connect(textBox3.Text, Convert.ToInt32(textBox4.Text));
            }
            catch
            {
                msg("Error while connecting. Check host and port");
            }

            if (clientSocket.Connected)
            {
                msg("Connected");
                thread.Start();
                button1.Enabled = false;
            }
            
        }

        public void getData() {
            
            
            while (true)
            {
                if (clear)
                    clearChart();
                i++;
                int intValue = Convert.ToInt32(label1.Text);
                byte[] intBytes = BitConverter.GetBytes(intValue);
                Array.Reverse(intBytes);
                byte[] result = intBytes;
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = result;
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                float rec = (float)(Convert.ToInt32(returndata))/100;
                SetText(rec.ToString());
                AddToChart(i, rec);
                Thread.Sleep(50);
                 
            }
        }
        delegate void clearChartCallback();

        private void clearChart()
        {
            if (this.InvokeRequired)
            {
                clearChartCallback invc = new clearChartCallback(clearChart);
                this.Invoke(invc, new object[] { });
            }
            else
            {
                if (chart1.Series["Series2"].Points.Count > 100) {
                    chart1.Series["Series2"].Points.RemoveAt(0);              
                }
            }
        }
        delegate void AddToChartCallback(float x, float y);

        private void AddToChart(float x, float y)
        {
            if (this.InvokeRequired)
            {
                AddToChartCallback inv = new AddToChartCallback(AddToChart);
                this.Invoke(inv, new object[] { x,y });
            }
            else
                chart1.Series["Series2"].Points.AddXY(x, y);
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if (thread.IsAlive)
            {
                thread.Abort();
                thread.Join();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = textBox2.Text;
            trackBar1.Value = Convert.ToInt32(label1.Text);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            clear = checkBox1.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

    }

}
