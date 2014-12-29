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
using System.Globalization;

namespace client2
{
    public partial class Form1 : Form
    {
        
        System.Net.Sockets.TcpClient clientSocket;
        Thread thread;
        int i = 0;
        bool clear;
        DateTime t1;
        DateTime t2;
        
        NetworkStream serverStream;

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
            
            chart1.Series["Series2"].Points.Clear();
            thread = new Thread(new ThreadStart(getData));
            if (clientSocket.Connected)
            {
                
                t1 = DateTime.Now;
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
                byte[] outStream = result;
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                try
                {
                    float rec = (float)(Convert.ToInt32(returndata)) / 100;
                    SetText(rec.ToString());
                    t2 = DateTime.Now;
                    System.TimeSpan diff = t2 - t1;
                    float millis = (float)diff.TotalMilliseconds;
                    AddToChart(Math.Round(millis / 1000, 2), rec);
                }
                catch { }
                
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
        delegate void AddToChartCallback(double x, float y);

        private void AddToChart(double x, float y)
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
            clientSocket.Client.Disconnect(false);
            
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (clientSocket.Connected)
            {
                byte[] k = toByte(textBox6.Text, 100000);
                serverStream.Write(k, 0, k.Length);
                serverStream.Flush();

                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);

                byte[] ti = toByte(textBox7.Text, 300000);
                serverStream.Write(ti, 0, ti.Length);
                serverStream.Flush();

                inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);

                byte[] td = toByte(textBox8.Text, 500000);
                serverStream.Write(td, 0, td.Length);
                serverStream.Flush();

                inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);

                msg("Nastawy zmienione");
            }
        }

        private byte[] toByte(string text, int mul){
            double dVal = double.Parse(text, CultureInfo.InvariantCulture);
            dVal *= 100;
            int Val = mul + (int)dVal;
            byte[] intBytes = BitConverter.GetBytes(Val);
            Array.Reverse(intBytes);
            byte[] outStream = intBytes;
            return outStream;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox6.Text = (((float)trackBar2.Value / 100)).ToString(CultureInfo.InvariantCulture);

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox7.Text = (((float)trackBar3.Value / 100)).ToString(CultureInfo.InvariantCulture);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox8.Text = (((float)trackBar4.Value / 1000)).ToString(CultureInfo.InvariantCulture);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clientSocket = new System.Net.Sockets.TcpClient();
            
            try
            {
                clientSocket.Connect(textBox3.Text, Convert.ToInt32(textBox4.Text));
            }
            catch
            {
                msg("Błąd podczas łączenia. Sprawdź adres hosta i port");
            }

            if (clientSocket.Connected)
            {
                msg("Połaczono");
                serverStream = clientSocket.GetStream();
                button1.Enabled = true;
                button4.Enabled = true;
                
            }
        }

    }

}
