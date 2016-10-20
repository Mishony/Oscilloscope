namespace Oscilloscope
{
    partial class MainForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbRate = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBoxSelection = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelCh2Freq = new System.Windows.Forms.Label();
            this.labelCh1Freq = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCh2Duty = new System.Windows.Forms.Label();
            this.labelCh1Duty = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCh2Max = new System.Windows.Forms.Label();
            this.labelCh1Max = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelCh2Avg = new System.Windows.Forms.Label();
            this.labelCh1Avg = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCh2Min = new System.Windows.Forms.Label();
            this.labelCh1Min = new System.Windows.Forms.Label();
            this.labelChannel2 = new System.Windows.Forms.Label();
            this.labelChannel1 = new System.Windows.Forms.Label();
            this.labelSamplesSelected = new System.Windows.Forms.Label();
            this.cbSmoothZoomOut = new System.Windows.Forms.CheckBox();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelSamples = new System.Windows.Forms.Label();
            this.labelSPS = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelPWM = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sliderPWM = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxSelection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderPWM)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbRate);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSelection);
            this.splitContainer1.Panel1.Controls.Add(this.cbSmoothZoomOut);
            this.splitContainer1.Panel1.Controls.Add(this.cbMode);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.labelPort);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.btnPause);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Size = new System.Drawing.Size(1021, 538);
            this.splitContainer1.SplitterDistance = 219;
            this.splitContainer1.TabIndex = 0;
            // 
            // cbRate
            // 
            this.cbRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRate.Enabled = false;
            this.cbRate.FormattingEnabled = true;
            this.cbRate.Items.AddRange(new object[] {
            "Fastest (~47KHz)",
            "Fast (~29KHz)",
            "Slow (~16KHz)",
            "Slowest (~9KHz)"});
            this.cbRate.Location = new System.Drawing.Point(38, 73);
            this.cbRate.Name = "cbRate";
            this.cbRate.Size = new System.Drawing.Size(168, 21);
            this.cbRate.TabIndex = 15;
            this.cbRate.SelectedIndexChanged += new System.EventHandler(this.cbRate_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Rate:";
            // 
            // groupBoxSelection
            // 
            this.groupBoxSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSelection.Controls.Add(this.label5);
            this.groupBoxSelection.Controls.Add(this.labelCh2Freq);
            this.groupBoxSelection.Controls.Add(this.labelCh1Freq);
            this.groupBoxSelection.Controls.Add(this.label3);
            this.groupBoxSelection.Controls.Add(this.labelCh2Duty);
            this.groupBoxSelection.Controls.Add(this.labelCh1Duty);
            this.groupBoxSelection.Controls.Add(this.label6);
            this.groupBoxSelection.Controls.Add(this.labelCh2Max);
            this.groupBoxSelection.Controls.Add(this.labelCh1Max);
            this.groupBoxSelection.Controls.Add(this.label2);
            this.groupBoxSelection.Controls.Add(this.labelCh2Avg);
            this.groupBoxSelection.Controls.Add(this.labelCh1Avg);
            this.groupBoxSelection.Controls.Add(this.label4);
            this.groupBoxSelection.Controls.Add(this.labelCh2Min);
            this.groupBoxSelection.Controls.Add(this.labelCh1Min);
            this.groupBoxSelection.Controls.Add(this.labelChannel2);
            this.groupBoxSelection.Controls.Add(this.labelChannel1);
            this.groupBoxSelection.Controls.Add(this.labelSamplesSelected);
            this.groupBoxSelection.Location = new System.Drawing.Point(3, 348);
            this.groupBoxSelection.Name = "groupBoxSelection";
            this.groupBoxSelection.Size = new System.Drawing.Size(209, 182);
            this.groupBoxSelection.TabIndex = 13;
            this.groupBoxSelection.TabStop = false;
            this.groupBoxSelection.Text = "On Screen";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Freq:";
            // 
            // labelCh2Freq
            // 
            this.labelCh2Freq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCh2Freq.AutoSize = true;
            this.labelCh2Freq.ForeColor = System.Drawing.Color.Red;
            this.labelCh2Freq.Location = new System.Drawing.Point(134, 149);
            this.labelCh2Freq.Name = "labelCh2Freq";
            this.labelCh2Freq.Size = new System.Drawing.Size(26, 13);
            this.labelCh2Freq.TabIndex = 23;
            this.labelCh2Freq.Text = "0Hz";
            // 
            // labelCh1Freq
            // 
            this.labelCh1Freq.AutoSize = true;
            this.labelCh1Freq.Location = new System.Drawing.Point(44, 149);
            this.labelCh1Freq.Name = "labelCh1Freq";
            this.labelCh1Freq.Size = new System.Drawing.Size(26, 13);
            this.labelCh1Freq.TabIndex = 22;
            this.labelCh1Freq.Text = "0Hz";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Duty:";
            // 
            // labelCh2Duty
            // 
            this.labelCh2Duty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCh2Duty.AutoSize = true;
            this.labelCh2Duty.ForeColor = System.Drawing.Color.Red;
            this.labelCh2Duty.Location = new System.Drawing.Point(134, 128);
            this.labelCh2Duty.Name = "labelCh2Duty";
            this.labelCh2Duty.Size = new System.Drawing.Size(21, 13);
            this.labelCh2Duty.TabIndex = 20;
            this.labelCh2Duty.Text = "0%";
            // 
            // labelCh1Duty
            // 
            this.labelCh1Duty.AutoSize = true;
            this.labelCh1Duty.Location = new System.Drawing.Point(44, 128);
            this.labelCh1Duty.Name = "labelCh1Duty";
            this.labelCh1Duty.Size = new System.Drawing.Size(21, 13);
            this.labelCh1Duty.TabIndex = 19;
            this.labelCh1Duty.Text = "0%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Max:";
            // 
            // labelCh2Max
            // 
            this.labelCh2Max.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCh2Max.AutoSize = true;
            this.labelCh2Max.ForeColor = System.Drawing.Color.Red;
            this.labelCh2Max.Location = new System.Drawing.Point(133, 106);
            this.labelCh2Max.Name = "labelCh2Max";
            this.labelCh2Max.Size = new System.Drawing.Size(35, 13);
            this.labelCh2Max.TabIndex = 17;
            this.labelCh2Max.Text = "0.00V";
            this.labelCh2Max.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VoltageLabel_Click);
            this.labelCh2Max.MouseEnter += new System.EventHandler(this.VoltageLabel_MouseEnter);
            this.labelCh2Max.MouseLeave += new System.EventHandler(this.VoltageLabel_MouseLeave);
            // 
            // labelCh1Max
            // 
            this.labelCh1Max.AutoSize = true;
            this.labelCh1Max.Location = new System.Drawing.Point(44, 106);
            this.labelCh1Max.Name = "labelCh1Max";
            this.labelCh1Max.Size = new System.Drawing.Size(35, 13);
            this.labelCh1Max.TabIndex = 16;
            this.labelCh1Max.Text = "0.00V";
            this.labelCh1Max.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VoltageLabel_Click);
            this.labelCh1Max.MouseEnter += new System.EventHandler(this.VoltageLabel_MouseEnter);
            this.labelCh1Max.MouseLeave += new System.EventHandler(this.VoltageLabel_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Avg:";
            // 
            // labelCh2Avg
            // 
            this.labelCh2Avg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCh2Avg.AutoSize = true;
            this.labelCh2Avg.ForeColor = System.Drawing.Color.Red;
            this.labelCh2Avg.Location = new System.Drawing.Point(133, 83);
            this.labelCh2Avg.Name = "labelCh2Avg";
            this.labelCh2Avg.Size = new System.Drawing.Size(35, 13);
            this.labelCh2Avg.TabIndex = 14;
            this.labelCh2Avg.Text = "0.00V";
            this.labelCh2Avg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VoltageLabel_Click);
            this.labelCh2Avg.MouseEnter += new System.EventHandler(this.VoltageLabel_MouseEnter);
            this.labelCh2Avg.MouseLeave += new System.EventHandler(this.VoltageLabel_MouseLeave);
            // 
            // labelCh1Avg
            // 
            this.labelCh1Avg.AutoSize = true;
            this.labelCh1Avg.Location = new System.Drawing.Point(43, 83);
            this.labelCh1Avg.Name = "labelCh1Avg";
            this.labelCh1Avg.Size = new System.Drawing.Size(35, 13);
            this.labelCh1Avg.TabIndex = 13;
            this.labelCh1Avg.Text = "0.00V";
            this.labelCh1Avg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VoltageLabel_Click);
            this.labelCh1Avg.MouseEnter += new System.EventHandler(this.VoltageLabel_MouseEnter);
            this.labelCh1Avg.MouseLeave += new System.EventHandler(this.VoltageLabel_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Min:";
            // 
            // labelCh2Min
            // 
            this.labelCh2Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCh2Min.AutoSize = true;
            this.labelCh2Min.ForeColor = System.Drawing.Color.Red;
            this.labelCh2Min.Location = new System.Drawing.Point(133, 62);
            this.labelCh2Min.Name = "labelCh2Min";
            this.labelCh2Min.Size = new System.Drawing.Size(35, 13);
            this.labelCh2Min.TabIndex = 11;
            this.labelCh2Min.Text = "0.00V";
            this.labelCh2Min.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VoltageLabel_Click);
            this.labelCh2Min.MouseEnter += new System.EventHandler(this.VoltageLabel_MouseEnter);
            this.labelCh2Min.MouseLeave += new System.EventHandler(this.VoltageLabel_MouseLeave);
            // 
            // labelCh1Min
            // 
            this.labelCh1Min.AutoSize = true;
            this.labelCh1Min.Location = new System.Drawing.Point(43, 62);
            this.labelCh1Min.Name = "labelCh1Min";
            this.labelCh1Min.Size = new System.Drawing.Size(35, 13);
            this.labelCh1Min.TabIndex = 10;
            this.labelCh1Min.Text = "0.00V";
            this.labelCh1Min.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VoltageLabel_Click);
            this.labelCh1Min.MouseEnter += new System.EventHandler(this.VoltageLabel_MouseEnter);
            this.labelCh1Min.MouseLeave += new System.EventHandler(this.VoltageLabel_MouseLeave);
            // 
            // labelChannel2
            // 
            this.labelChannel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelChannel2.AutoSize = true;
            this.labelChannel2.ForeColor = System.Drawing.Color.Red;
            this.labelChannel2.Location = new System.Drawing.Point(133, 43);
            this.labelChannel2.Name = "labelChannel2";
            this.labelChannel2.Size = new System.Drawing.Size(55, 13);
            this.labelChannel2.TabIndex = 9;
            this.labelChannel2.Text = "Channel 2";
            this.labelChannel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelChannel1
            // 
            this.labelChannel1.AutoSize = true;
            this.labelChannel1.Location = new System.Drawing.Point(45, 43);
            this.labelChannel1.Name = "labelChannel1";
            this.labelChannel1.Size = new System.Drawing.Size(55, 13);
            this.labelChannel1.TabIndex = 8;
            this.labelChannel1.Text = "Channel 1";
            // 
            // labelSamplesSelected
            // 
            this.labelSamplesSelected.AutoSize = true;
            this.labelSamplesSelected.Location = new System.Drawing.Point(1, 20);
            this.labelSamplesSelected.Name = "labelSamplesSelected";
            this.labelSamplesSelected.Size = new System.Drawing.Size(59, 13);
            this.labelSamplesSelected.TabIndex = 7;
            this.labelSamplesSelected.Text = "Samples: 0";
            // 
            // cbSmoothZoomOut
            // 
            this.cbSmoothZoomOut.AutoSize = true;
            this.cbSmoothZoomOut.Location = new System.Drawing.Point(11, 234);
            this.cbSmoothZoomOut.Name = "cbSmoothZoomOut";
            this.cbSmoothZoomOut.Size = new System.Drawing.Size(112, 17);
            this.cbSmoothZoomOut.TabIndex = 10;
            this.cbSmoothZoomOut.Text = "Smooth Zoom Out";
            this.cbSmoothZoomOut.UseVisualStyleBackColor = true;
            this.cbSmoothZoomOut.CheckedChanged += new System.EventHandler(this.cbSmoothZoomOut_CheckedChanged);
            // 
            // cbMode
            // 
            this.cbMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Channel 1 (0-28V)",
            "Channel 2 (0-28V)",
            "Channels 1 & 2 (0-28V)",
            "Channel 2 - Channel1",
            "Channel 3 (0-5V)"});
            this.cbMode.Location = new System.Drawing.Point(38, 43);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(168, 21);
            this.cbMode.TabIndex = 6;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mode:";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(4, 21);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(89, 13);
            this.labelPort.TabIndex = 1;
            this.labelPort.Text = "Port: Searching...";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.labelSamples);
            this.groupBox2.Controls.Add(this.labelSPS);
            this.groupBox2.Location = new System.Drawing.Point(3, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 71);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Total";
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.Location = new System.Drawing.Point(1, 20);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(59, 13);
            this.labelSamples.TabIndex = 7;
            this.labelSamples.Text = "Samples: 0";
            // 
            // labelSPS
            // 
            this.labelSPS.AutoSize = true;
            this.labelSPS.Location = new System.Drawing.Point(1, 44);
            this.labelSPS.Name = "labelSPS";
            this.labelSPS.Size = new System.Drawing.Size(40, 13);
            this.labelSPS.TabIndex = 8;
            this.labelSPS.Text = "SPS: 0";
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Enabled = false;
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.Location = new System.Drawing.Point(7, 106);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(199, 56);
            this.btnPause.TabIndex = 9;
            this.btnPause.Text = "Start";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelPWM);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.sliderPWM);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 262);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setup";
            // 
            // labelPWM
            // 
            this.labelPWM.AutoSize = true;
            this.labelPWM.Location = new System.Drawing.Point(78, 172);
            this.labelPWM.Name = "labelPWM";
            this.labelPWM.Size = new System.Drawing.Size(21, 13);
            this.labelPWM.TabIndex = 10;
            this.labelPWM.Text = "0%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "PWM on pin 5:";
            // 
            // sliderPWM
            // 
            this.sliderPWM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderPWM.AutoSize = false;
            this.sliderPWM.Enabled = false;
            this.sliderPWM.LargeChange = 1;
            this.sliderPWM.Location = new System.Drawing.Point(0, 189);
            this.sliderPWM.Name = "sliderPWM";
            this.sliderPWM.Size = new System.Drawing.Size(209, 27);
            this.sliderPWM.TabIndex = 0;
            this.sliderPWM.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.sliderPWM.Scroll += new System.EventHandler(this.sliderPWM_Scroll);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnPause;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 538);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Oscilloscope";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxSelection.ResumeLayout(false);
            this.groupBoxSelection.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderPWM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.Label labelSPS;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.CheckBox cbSmoothZoomOut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxSelection;
        private System.Windows.Forms.Label labelChannel2;
        private System.Windows.Forms.Label labelChannel1;
        private System.Windows.Forms.Label labelSamplesSelected;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCh2Min;
        private System.Windows.Forms.Label labelCh1Min;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCh2Avg;
        private System.Windows.Forms.Label labelCh1Avg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelCh2Max;
        private System.Windows.Forms.Label labelCh1Max;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelCh2Freq;
        private System.Windows.Forms.Label labelCh1Freq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCh2Duty;
        private System.Windows.Forms.Label labelCh1Duty;
        private System.Windows.Forms.TrackBar sliderPWM;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelPWM;
        private System.Windows.Forms.ComboBox cbRate;
        private System.Windows.Forms.Label label10;
    }
}

