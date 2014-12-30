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
using System.Net;

namespace client2
{
    public partial class Form1 : Form
    {
        
        UdpClient udpClient;

        Thread thread;
        int i = 0;
        bool clear;
        DateTime t1;
        DateTime t2;
        
       
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
            t1 = DateTime.Now;
            thread.Start();
            button6.Enabled = true;
        }

        public void getData() {
            float rec;
            while (true) {
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, Convert.ToInt32(textBox4.Text));
                byte[] content = udpClient.Receive(ref remoteIPEndPoint);
                string returndata = System.Text.Encoding.ASCII.GetString(content);
                
                rec = ((float)(Convert.ToInt32(returndata))) / 100;
                t2 = DateTime.Now;
                System.TimeSpan diff = t2 - t1;
                float millis = (float)diff.TotalMilliseconds;
                AddToChart(Math.Round(millis / 1000, 2), rec);
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
            if (this.textBox5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox5.Text = text;
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
            if (textBox2.Text.Length > 0)
            {
                label1.Text = textBox2.Text;
                trackBar1.Value = Convert.ToInt32(label1.Text);
            }
            
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
            byte[] k = toByte(textBox6.Text, 100000);
            udpClient.Send(k, k.Length);

            byte[] ti = toByte(textBox7.Text, 300000);
            udpClient.Send(ti, ti.Length);

            byte[] td = toByte(textBox8.Text, 500000);
            udpClient.Send(td, td.Length);
            
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
            try
            {
                udpClient.Connect(textBox3.Text, Convert.ToInt32(textBox4.Text));

            }
            catch
            {
                msg("Błąd przy łaczeniu z hostem");
            }
            if (udpClient.Client.Connected)
            {
                msg("Połączono");
                button4.Enabled = true;
                button1.Enabled = true;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            udpClient = new UdpClient();
        }
        private void SendF()
        {
            if (label1.Text.Length > 0)
            {
                int Val = 1;
                try
                {
                    Val = Convert.ToInt32(label1.Text);
                }
                catch
                {
                    msg("Wartośc zadana musi byc liczba całkowitą");
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
                    msg(exc.ToString());
                }

            }
        }

       

        private void button6_Click(object sender, EventArgs e)
        {
            SendF();
        }

    }

}
