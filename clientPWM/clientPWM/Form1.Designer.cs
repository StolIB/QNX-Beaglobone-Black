namespace clientPWM
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.hostTxt = new System.Windows.Forms.TextBox();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.console = new System.Windows.Forms.TextBox();
            this.freqTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radio1 = new System.Windows.Forms.RadioButton();
            this.radio10 = new System.Windows.Forms.RadioButton();
            this.radio100 = new System.Windows.Forms.RadioButton();
            this.radio1000 = new System.Windows.Forms.RadioButton();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(310, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Połącz";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // hostTxt
            // 
            this.hostTxt.Location = new System.Drawing.Point(24, 37);
            this.hostTxt.Name = "hostTxt";
            this.hostTxt.Size = new System.Drawing.Size(137, 20);
            this.hostTxt.TabIndex = 1;
            this.hostTxt.Text = "10.9.112.160";
            // 
            // portTxt
            // 
            this.portTxt.Location = new System.Drawing.Point(181, 37);
            this.portTxt.Name = "portTxt";
            this.portTxt.Size = new System.Drawing.Size(103, 20);
            this.portTxt.TabIndex = 2;
            this.portTxt.Text = "5002";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Host";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(289, 180);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 36);
            this.button2.TabIndex = 5;
            this.button2.Text = "Wyślij";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // console
            // 
            this.console.Location = new System.Drawing.Point(25, 81);
            this.console.Multiline = true;
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.Size = new System.Drawing.Size(388, 62);
            this.console.TabIndex = 6;
            // 
            // freqTxt
            // 
            this.freqTxt.Location = new System.Drawing.Point(25, 196);
            this.freqTxt.Name = "freqTxt";
            this.freqTxt.Size = new System.Drawing.Size(185, 20);
            this.freqTxt.TabIndex = 7;
            this.freqTxt.TextChanged += new System.EventHandler(this.freqTxt_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Częstotliwość (Hz)";
            // 
            // radio1
            // 
            this.radio1.AutoSize = true;
            this.radio1.Checked = true;
            this.radio1.Location = new System.Drawing.Point(34, 324);
            this.radio1.Name = "radio1";
            this.radio1.Size = new System.Drawing.Size(36, 17);
            this.radio1.TabIndex = 9;
            this.radio1.TabStop = true;
            this.radio1.Text = "x1";
            this.radio1.UseVisualStyleBackColor = true;
            this.radio1.CheckedChanged += new System.EventHandler(this.radio1_CheckedChanged);
            // 
            // radio10
            // 
            this.radio10.AutoSize = true;
            this.radio10.Location = new System.Drawing.Point(107, 324);
            this.radio10.Name = "radio10";
            this.radio10.Size = new System.Drawing.Size(42, 17);
            this.radio10.TabIndex = 10;
            this.radio10.Text = "x10";
            this.radio10.UseVisualStyleBackColor = true;
            this.radio10.CheckedChanged += new System.EventHandler(this.radio10_CheckedChanged);
            // 
            // radio100
            // 
            this.radio100.AutoSize = true;
            this.radio100.Location = new System.Drawing.Point(181, 324);
            this.radio100.Name = "radio100";
            this.radio100.Size = new System.Drawing.Size(48, 17);
            this.radio100.TabIndex = 11;
            this.radio100.Text = "x100";
            this.radio100.UseVisualStyleBackColor = true;
            this.radio100.CheckedChanged += new System.EventHandler(this.radio100_CheckedChanged);
            // 
            // radio1000
            // 
            this.radio1000.AutoSize = true;
            this.radio1000.Location = new System.Drawing.Point(279, 324);
            this.radio1000.Name = "radio1000";
            this.radio1000.Size = new System.Drawing.Size(54, 17);
            this.radio1000.TabIndex = 12;
            this.radio1000.Text = "x1000";
            this.radio1000.UseVisualStyleBackColor = true;
            this.radio1000.CheckedChanged += new System.EventHandler(this.radio1000_CheckedChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(24, 260);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(389, 45);
            this.trackBar1.TabIndex = 13;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 480);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.radio1000);
            this.Controls.Add(this.radio100);
            this.Controls.Add(this.radio10);
            this.Controls.Add(this.radio1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.freqTxt);
            this.Controls.Add(this.console);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portTxt);
            this.Controls.Add(this.hostTxt);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox hostTxt;
        private System.Windows.Forms.TextBox portTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox console;
        private System.Windows.Forms.TextBox freqTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radio1;
        private System.Windows.Forms.RadioButton radio10;
        private System.Windows.Forms.RadioButton radio100;
        private System.Windows.Forms.RadioButton radio1000;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}

