using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Diagnostics;

namespace Oscilloscope
{
    public partial class MainForm : Form
    {
        static MainForm s_this;
        private SerialPort Serial = null;
        private Thread thSerial = null;
        private enum TSerialStatus {
            SEARCHING,
            PINGING,
            READY,
        };
        TSerialStatus eSerialStatus = TSerialStatus.SEARCHING;
        //int nBytesReceived = 0;
        private List<int> Samples1 = new List<int>();
        private List<int> Samples2 = new List<int>();
        Stopwatch stopwatch = new Stopwatch();
        bool bPaused = true;
        bool bSmoothZoomOut = false;
        byte mode = 0;
        Point ofs = new Point(0, 0);
        PointF scale = new PointF(1, 1);
        const int margin = 24;
        Font font = new Font("Times New Roman", margin - 8, GraphicsUnit.Pixel);
        bool bLBDown = false;
        Point ptLBDown;
        Point ofsLBDown;
        bool bRBDown = false;
        Point ptRBDown;
        Point ofsRBDown;
        int idxSelStart = -1;
        int idxSelEnd = -1;

        Label[] Ch1Labels;
        Label[] Ch2Labels;
        Label[] VoltageLabels;
        List<float> lstHLineVoltages = new List<float>();
        List<Color> lstHLineColors = new  List<Color>();

        Color[] colors = new Color[] { Color.Green, Color.Red, Color.Green, Color.Purple, Color.Blue };
        Pen[] pens = new Pen[] { Pens.LightGreen, Pens.Red, Pens.LightGreen, Pens.Purple, Pens.Blue };
        Brush[] brushes = new Brush[] { Brushes.LightGreen, Brushes.Red, Brushes.LightGreen, Brushes.Purple, Brushes.Blue };

        private string f2s(float f, string format = "0.00")
        {
            return f.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }

        private float SampleToVolts(int sample)
        {
            if (mode >= 0 && mode <= 3)
                return sample * 2.56f * 11.0f / 1023;
            if (mode == 4)
                return sample * 5.0f / 1023;
            return 0;
        }

        private int VoltsToSample(float v)
        {
            if (mode >= 0 && mode <= 3)
                return (int) (v * 1023 / (2.56f * 11.0f));
            if (mode == 4)
                return (int)(v * 1023 / 5.0f);
            return 0;
        }

        private void ClearSelection()
        {
            idxSelStart = idxSelEnd = -1;
            splitContainer1.Panel2.Invalidate();
        }

        private void SetSelection(int idxStart, int idxEnd)
        {
            if (idxStart < 0)
                idxStart = 0;
            else if (idxStart >= Samples1.Count)
                idxStart = Samples1.Count - 1;
            if (idxEnd < 0)
                idxEnd = 0;
            else if (idxEnd >= Samples1.Count)
                idxEnd = Samples1.Count - 1;
            idxSelStart = idxStart;
            idxSelEnd = idxEnd;
            if (idxSelStart >= 0 && idxSelEnd != idxSelStart)
                groupBoxSelection.Text = "Selection";
            else
                groupBoxSelection.Text = "On Screen";
            splitContainer1.Panel2.Invalidate();
        }

        private bool GetSelection(ref int idx_start, ref int idx_end)
        {
            if (idxSelStart >= 0 && idxSelEnd != idxSelStart)
            {
                if (idxSelEnd >= idxSelStart)
                {
                    idx_start = idxSelStart;
                    idx_end = idxSelEnd;
                }
                else
                {
                    idx_start = idxSelEnd;
                    idx_end = idxSelStart;
                }
                return true;
            }
            return false;
        }

        private void SetPortText(string text)
        {
            if (s_this == null) return;
            MethodInvoker inv = delegate
            {
                labelPort.Text = "Port: " + text;
            };
            s_this.Invoke(inv);
        }

        private void EnableControls(bool enable)
        {
            if (s_this == null) return;
            MethodInvoker inv = delegate
            {
                btnPause.Enabled = enable;
                cbRate.Enabled = enable;
                sliderPWM.Enabled = enable;
            };
            s_this.Invoke(inv);
        }

        private void FocusPanel()
        {
            if (s_this != null)
            {
                splitContainer1.Panel2.Invalidate();
                MethodInvoker inv = delegate
                {
                    splitContainer1.Panel2.Focus();
                };
                s_this.Invoke(inv);
            }
        }

        private void Pause(bool bPause)
        {
            FocusPanel();
            if (bPaused == bPause)
                return;
            bPaused = bPause;
            if (bPaused)
            {
                MethodInvoker inv = delegate
                {
                    btnPause.Text = "Start";
                };
                s_this.Invoke(inv);
                
                stopwatch.Stop();
                SendSerial('0');
                Thread.Sleep(50);
                SendSerial('0');
            }
            else
            {
                ClearSelection();
                MethodInvoker inv = delegate
                {
                    btnPause.Text = "Stop";
                };
                s_this.Invoke(inv);

                Samples1.Clear();
                Samples2.Clear();
                stopwatch.Reset();
                stopwatch.Start();
                RequestData();
            }
        }

        private bool SendSerial(char cmd, char param = (char) 0)
        {
            if (Serial == null)
                return false;
            try
            {
                byte[] buf = new byte[2];
                buf[0] = (byte)cmd;
                buf[1] = (byte)param;
                Serial.Write(buf, 0, (param == (char) 0) ? 1 : 2);
            }
            catch (Exception)
            {
                if (Serial != null)
                {
                    try
                    {
                        Serial.Close();
                    }
                    catch (Exception) { }
                    Serial = null;
                }
                return false;
            }
            return true;
        }

        private void RequestData()
        {
            char cmd = (char)(mode + '1');
            SendSerial(cmd);
            Thread.Sleep(50);
            SendSerial(cmd);
        }

        private int Read(byte[] data, ref int idx)
        {
            byte bt1 = data[idx++];
            byte bt2 = data[idx++];
            return (Int16) (bt1 | (UInt16) (bt2 << 8));
        }

        private void OnSerial(byte[] data)
        {
            if (eSerialStatus == TSerialStatus.READY)
            {
                if (s_this == null) return;
                if (!bPaused)
                {
                    for (int i = 0; i < data.Length; )
                    {
                        if (mode == 2)
                        {
                            int s1 = Read(data, ref i);
                            int s2 = Read(data, ref i);

                            int diff = (int)((s2 - s1) * 0.18f);
                            s1 -= diff;
                            s2 += diff;

                            if (s1 < 0) s1 = 0; else if (s1 > 1023) s1 = 1023;
                            if (s2 < 0) s2 = 0; else if (s2 > 1023) s2 = 1023;

                            Samples1.Add(s1);
                            Samples2.Add(s2);
                        }
                        else if (mode == 3)
                        {
                            int s = Read(data, ref i);
                            s = (int)(s * 1.4);
                            if (s < -1023) s = -1023; else if (s > 1023) s = 1023;
                            Samples1.Add(s);
                        } 
                        else
                            Samples1.Add(Read(data, ref i));
                    }
                    splitContainer1.Panel2.Invalidate();
                }
                return;
            }
        }

        void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                byte[] data = new byte[sp.BytesToRead];
                sp.Read(data, 0, data.Length);
                s_this.OnSerial(data);
            }
            catch (Exception)
            {
            }
        }

        void Serial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
        }
        
        private void SerialThread()
        {
            while (true)
            {
                Thread.Sleep(200);

                if (Serial != null && Serial.IsOpen)
                {
                    if (!bPaused)
                        SendSerial('.');
                }
                else
                {
                    Pause(true);
                    EnableControls(false);
                    eSerialStatus = TSerialStatus.SEARCHING;
                    SetPortText("Searching ...");
                    foreach (string port in SerialPort.GetPortNames())
                    {
                        try
                        {
                            Serial = new SerialPort(port, 921600);
                            Serial.ErrorReceived += Serial_ErrorReceived;
                            Serial.WriteTimeout = 500;
                            Serial.ReadTimeout = 500;
                            Serial.DtrEnable = true;
                            Serial.RtsEnable = true;
                            Serial.Open();
                            Thread.Sleep(100);
                        }
                        catch (Exception)
                        {
                            Serial = null;
                            Thread.Sleep(500);
                            continue;
                        }
                        eSerialStatus = TSerialStatus.PINGING;
                        if (SendSerial('?'))
                        {
                            Thread.Sleep(100);
                            char[] buf = new char[6];
                            buf[0] = ' ';
                            try
                            {
                                Serial.Read(buf, 0, 6);
                            }
                            catch (Exception) { }
                            if (buf[0] == 'O' && buf[1] == 'S' && buf[2] == 'C' && buf[3] == 'L')
                            {
                                char rate = buf[4];
                                char pwm = buf[5];
                                eSerialStatus = TSerialStatus.READY;
                                Serial.DataReceived += Serial_DataReceived;
                                MethodInvoker inv = delegate
                                {
                                    cbRate.SelectedIndex = rate - '0';
                                    sliderPWM.Value = pwm - '0';
                                    int val = 10 * sliderPWM.Value;
                                    labelPWM.Text = val.ToString() + "%";
                                };
                                s_this.Invoke(inv);
                            }
                        }
                        if (eSerialStatus != TSerialStatus.READY)
                        {
                            eSerialStatus = TSerialStatus.SEARCHING;
                            if (Serial != null)
                            {
                                try
                                {
                                    Serial.Close();
                                }
                                catch (Exception) { }
                                Serial = null;
                            }
                            continue;
                        }
                        SetPortText(port);
                        EnableControls(true);
                        break;
                    }
                }
            }
        }

        public MainForm()
        {            
            InitializeComponent();

            var p = splitContainer1.Panel2;
            p.Paint += Panel2_Paint;
            p.MouseMove += Panel2_MouseMove;
            p.MouseDown += Panel2_MouseDown;
            p.MouseUp += Panel2_MouseUp;
            p.MouseWheel += Panel2_MouseWheel;
            p.PreviewKeyDown += Panel2_PreviewKeyDown;
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, p, new object[] { true });
            p.Focus();

            this.MouseWheel += Mainform_MouseWheel;

            Ch1Labels = new Label[] { labelChannel1, labelCh1Min, labelCh1Avg, labelCh1Max, labelCh1Duty, labelCh1Freq };
            Ch2Labels = new Label[] { labelChannel2, labelCh2Min, labelCh2Avg, labelCh2Max, labelCh2Duty, labelCh2Freq };
            VoltageLabels = new Label[] { labelCh1Min, labelCh1Avg, labelCh1Max, labelCh2Min, labelCh2Avg, labelCh2Max };

            cbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMode.SelectedIndex = mode;

            cbRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRate.SelectedIndex = 1;

            s_this = this;

            thSerial = new Thread(new ThreadStart(SerialThread));
            thSerial.Start();
        }

        private Point ClientToSample(Point ptClient)
        {
            var p = splitContainer1.Panel2;
            int x = Samples1.Count - (int)(((p.Width - 1) + ofs.X - ptClient.X) / scale.X);
            int y;
            if (mode == 3)
                y = (int)((p.Height / 2 + ofs.Y - ptClient.Y) * 1023.0f / (scale.Y * (p.Height / 2 - margin)));
            else
                y = (int)((p.Height - margin + ofs.Y - ptClient.Y) * 1023.0f / (scale.Y * (p.Height - 2 * margin)));
            return new Point(x, y);
        }

        private Point SampleToClient(Point ptSample)
        {
            var p = splitContainer1.Panel2;
            int x = (int)((p.Width - 1) + ofs.X - (Samples1.Count - ptSample.X) * scale.X);
            int y;
            if (mode == 3)
                y = (int)(p.Height / 2 + ofs.Y - ptSample.Y * scale.Y * (p.Height / 2 - margin) / 1023);
            else
                y = (int)(p.Height - margin + ofs.Y - ptSample.Y * scale.Y * (p.Height - 2 * margin) / 1023);
            return new Point(x, y);
        }

        private void DrawString(string s, int x, int y, bool bRight, bool bBottom, Brush brush, Graphics g)
        {
            SizeF sz = g.MeasureString(s, font);
            RectangleF rc = new RectangleF(x - (bRight ? sz.Width : 0), y - (bBottom ? sz.Height : 0), sz.Width, sz.Height);
            g.FillRectangle(Brushes.Black, rc);
            g.DrawString(s, font, brush, rc.Left, rc.Top);
        }

        private void DrawVLine(int x, Graphics g)
        {
            var p = splitContainer1.Panel2;
            if (x < 0 || x >= p.Width)
                return;
            Point pts = ClientToSample(new Point(x, 0));
            if (pts.X < 0 || pts.X >= Samples1.Count)
                return;

            Point ptc = SampleToClient(pts);
            g.DrawLine(Pens.Gray, ptc.X, 0, ptc.X, p.Height - 1);
            string sv1 = f2s(SampleToVolts(Samples1[pts.X])) + "V";
            DrawString(sv1, ptc.X - 2, 0, true, false, brushes[mode], g);

            if (mode == 2)
            {
                string sv2 = f2s(SampleToVolts(Samples2[pts.X])) + "V";
                DrawString(sv2, ptc.X + 2, 0, false, false, brushes[1], g);
            }

            string sTime;
            float elapsed = stopwatch.ElapsedMilliseconds;
            if (idxSelStart < 0 || pts.X == idxSelStart)
                sTime = f2s(pts.X * elapsed / Samples1.Count, "0.000") + "ms";
            else
            {
                sTime = f2s((pts.X - idxSelStart) * elapsed / Samples1.Count, "0.000") + "ms";
                if (elapsed > 0)
                    sTime = "+" + sTime;
            }
            DrawString(sTime, ptc.X + 2, p.Height - 1, false, true, Brushes.Gray, g);
        }

        private void DrawHLine(float v, Color clr, Graphics g)
        {
            var p = splitContainer1.Panel2;
            int y = SampleToClient(new Point(0, VoltsToSample(v))).Y;
            g.DrawLine(new Pen(clr), 0, y, p.Width - 1, y);
            bool bBottom = y < p.Height / 2;
            DrawString(f2s(v) + "V", p.Width - 2, y + (bBottom ? -2 : 2), true, bBottom, new SolidBrush(clr), g);
        }

        private void SetVoltage(Label l, float v)
        {
            l.Text = f2s(v) + "V";
            if (!l.Visible || l.BorderStyle == BorderStyle.None) return;
            lstHLineVoltages.Add(v);
            lstHLineColors.Add(l.ForeColor);
        }

        private void CalcStats(int idx_start, int idx_end)
        {
            int cnt = idx_end - idx_start;
            float elapsed = stopwatch.ElapsedMilliseconds;
            float time_ms = elapsed * cnt / Samples1.Count;
            labelSamplesSelected.Text = "Samples: " + cnt + " / " + f2s(time_ms, "0.000") + "ms";

            float v01 = SampleToVolts(Samples1[idx_start]);
            float min1 = v01;
            float max1 = v01;
            float avg1 = v01;

            float v02 = 0;
            float min2 = 0;
            float max2 = 0;
            float avg2 = 0;
            if (mode == 2)
            {
                v02 = SampleToVolts(Samples2[idx_start]);
                min2 = v02;
                max2 = v02;
                avg2 = v02;
            }

            for (int idx = idx_start + 1; idx < idx_end; ++idx)
            {
                float v1 = SampleToVolts(Samples1[idx]);
                if (v1 < min1) min1 = v1;
                if (v1 > max1) max1 = v1;
                avg1 += v1;
                if (mode == 2)
                {
                    float v2 = SampleToVolts(Samples2[idx]);
                    if (v2 < min2) min2 = v2;
                    if (v2 > max2) max2 = v2;
                    avg2 += v2;
                }
            }
            avg1 /= cnt;
            avg2 /= cnt;

            int cross1 = 0;
            int cross2 = 0;
            int last1 = -1;
            int last2 = -1;
            float mid1 = avg1; //(max1 + min1) / 2; //avg1
            float mid2 = avg2; //(max2 + min2) / 2; //avg2
            float thres1 = (mid1 - min1) * 0.1f;
            float thres2 = (mid2 - min2) * 0.1f;
            for (int idx = idx_start; idx < idx_end; ++idx)
            {
                float v1 = SampleToVolts(Samples1[idx]);
                if (v1 < mid1 - thres1)
                {
                    if (last1 == 1)
                        ++cross1;
                    last1 = 0;
                }
                else if (v1 > mid1 + thres1)
                {
                    if (last1 == 0)
                        ++cross1;
                    last1 = 1;
                }
                if (mode == 2)
                {
                    float v2 = SampleToVolts(Samples2[idx]);
                    if (v2 < mid2 - thres2)
                    {
                        if (last2 == 1)
                            ++cross2;
                        last2 = 0;
                    }
                    else if (v2 > mid2 + thres2)
                    {
                        if (last2 == 0)
                            ++cross2;
                        last2 = 1;
                    }
                }
            }
            float freq1 = (cross1 * 1000 / time_ms) / 2;
            float freq2 = (cross2 * 1000 / time_ms) / 2;

            SetVoltage(labelCh1Min, min1);
            SetVoltage(labelCh1Avg, avg1);
            SetVoltage(labelCh1Max, max1);
            labelCh1Duty.Text = (max1 <= min1) ? "100%" : f2s((avg1 - min1) * 100 / (max1 - min1)) + "%";
            labelCh1Freq.Text = f2s(freq1) + "Hz";

            SetVoltage(labelCh2Min, min2);
            SetVoltage(labelCh2Avg, avg2);
            SetVoltage(labelCh2Max, max2);
            labelCh2Duty.Text = (max2 <= min2) ? "100%" : f2s((avg2 - min2) * 100 / (max2 - min2)) + "%";
            labelCh2Freq.Text = f2s(freq2) + "Hz";

            if (!bPaused && cross1 > 5)
            {
                int idxHigh = -1;
                int idxLow = -1;
                for (int idx = idx_end - 1; idx > idx_end - (idx_end - idx_start) / 6; --idx)
                {
                    float v = SampleToVolts(Samples1[idx]);
                    if (v > mid1 + thres1)
                        idxHigh = idx;
                    if (idxHigh >= 0)
                    {
                        if (v > mid1 - thres1) continue;
                        idxLow = idx;
                        break;
                    }
                }

                if (idxLow >= 0)
                {
                    int x = SampleToClient(new Point(idxHigh, 0)).X;
                    ofs.X = splitContainer1.Panel2.Width - x;
                }
            }
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            float elapsed = stopwatch.ElapsedMilliseconds;
            labelSamples.Text = "Samples: " + Samples1.Count.ToString() + " / " + f2s(elapsed / 1000.0f, "0.000") + "s";
            if (Samples1.Count > 0 && elapsed > 0)
                labelSPS.Text = "SPS: " + ((int)(Samples1.Count * 1000.0f / elapsed)).ToString() + " (every " + f2s(elapsed * 1000 / Samples1.Count, "0.000") + "us)";
            else
                labelSPS.Text = "SPS: 0";

            var p = splitContainer1.Panel2;
            var g = e.Graphics;

            lstHLineVoltages.Clear();
            lstHLineColors.Clear();

            if (!bPaused)
            {
                ofs.X = 0;
                //ofs.Y = 0;
            }

            int idx_start = ClientToSample(new Point(0, 0)).X - 1;
            if (idx_start < 0) idx_start = 0;

            int idx_end = ClientToSample(new Point(p.Width, 0)).X + 2;
            if (idx_end > Samples1.Count) idx_end = Samples1.Count;

            int idx_sel_start = idx_start;
            int idx_sel_end = idx_end;
            bool bSelection = GetSelection(ref idx_sel_start, ref idx_sel_end);

            if (bSelection)
            {
                int x1 = SampleToClient(new Point(idx_sel_start, 0)).X;
                int x2 = SampleToClient(new Point(idx_sel_end, 0)).X;
                g.FillRectangle(Brushes.DarkSlateGray, x1, 0, x2 - x1, p.Height);
            }

            float vmax = SampleToVolts(ClientToSample(new Point(0, margin)).Y);
            g.DrawLine(Pens.Gray, 0, margin - 1, p.Width - 1, margin - 1);
            DrawString(f2s(vmax) + "V", 0, 0, false, false, Brushes.Gray, g);

            float vmin = SampleToVolts(ClientToSample(new Point(0, p.Height - margin)).Y);
            g.DrawLine(Pens.Gray, 0, p.Height - margin + 1, p.Width - 1, p.Height - margin + 1);
            DrawString(f2s(vmin) + "V", 0, p.Height, false, true, Brushes.Gray, g);

            if (mode == 3)
            {
                float vmid = SampleToVolts(ClientToSample(new Point(0, p.Height / 2)).Y);
                g.DrawLine(Pens.Gray, 0, p.Height / 2, p.Width - 1, p.Height / 2);
                DrawString(f2s(vmid) + "V", 0, p.Height / 2 + 2, false, false, Brushes.Gray, g);
            }

            if (idx_start >= idx_end - 1)
                return;

            CalcStats(idx_sel_start, idx_sel_end);

            Point[] points1 = new Point[(idx_end - idx_start) * 4];
            Point[] points2 = null;
            if (mode == 2)
                points2 = new Point[(idx_end - idx_start) * 4];
            int cnt = 0;
            int last_x = 0;
            int sum1 = 0;
            int min1 = 0;
            int max1 = 0;
            int sum2 = 0;
            int min2 = 0;
            int max2 = 0;
            int div = 0;
            for (int idx = idx_start; idx < idx_end; ++idx)
            {
                int y1 = Samples1[idx];
                Point pt1 = SampleToClient(new Point(idx, y1));
                if (idx == idx_start)
                    last_x = pt1.X;

                if (div != 0 && pt1.X != last_x)
                {
                    points1[cnt] = new Point(last_x, sum1 / div);
                    if (mode == 2)
                        points2[cnt] = new Point(last_x, sum2 / div);
                    ++cnt;
                    if (div > 1 && !bSmoothZoomOut)
                    {
                        points1[cnt] = new Point(last_x, max1);
                        if (mode == 2)
                            points2[cnt] = new Point(last_x, max2);
                        ++cnt;
                        points1[cnt] = new Point(last_x, min1);
                        if (mode == 2)
                            points2[cnt] = new Point(last_x, min2);
                        ++cnt;
                        points1[cnt] = new Point(last_x, sum1 / div);
                        if (mode == 2)
                            points2[cnt] = new Point(last_x, sum2 / div);
                        ++cnt;
                    }
                    sum1 = 0;
                    min1 = 0;
                    max1 = 0;
                    sum2 = 0;
                    min2 = 0;
                    max2 = 0;
                    div = 0;
                    last_x = pt1.X;
                }
                ++div;
                sum1 += pt1.Y;
                if (div == 1 || pt1.Y > max1)
                    max1 = pt1.Y;
                if (div == 1 || pt1.Y < min1)
                    min1 = pt1.Y;
                if (mode == 2)
                {
                    int y2 = Samples2[idx];
                    Point pt2 = SampleToClient(new Point(idx, y2));
                    sum2 += pt2.Y;
                    if (div == 1 || pt2.Y > max2)
                        max2 = pt2.Y;
                    if (div == 1 || pt2.Y < min2)
                        min2 = pt2.Y;
                }
            }
            if (cnt < 2)
                return;

            Array.Resize(ref points1, cnt);
            g.DrawLines(pens[mode], points1);
            if (mode == 2)
            {
                Array.Resize(ref points2, cnt);
                g.DrawLines(pens[1], points2);
            }

            for (int i = 0; i < lstHLineVoltages.Count(); ++i)
            {
               DrawHLine(lstHLineVoltages[i], lstHLineColors[i], g);
            }

            if (idxSelEnd == idxSelStart)
            {
                Point ptcSel = SampleToClient(new Point(idxSelStart, 0));
                DrawVLine(ptcSel.X, g);
            }

            Point ptcMouse = p.PointToClient(Control.MousePosition);
            DrawVLine(ptcMouse.X, g);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Pause(true);
            s_this = null;
            if (thSerial != null)
            {
                thSerial.Abort();
                thSerial = null;
            }
        }

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            splitContainer1.Panel2.Invalidate();

            if (bRBDown)
            {
                if ((Control.ModifierKeys & Keys.Shift) == 0)
                    ofs.X = ofsRBDown.X + e.Location.X - ptRBDown.X;
                if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                    ofs.Y = ofsRBDown.Y + e.Location.Y - ptRBDown.Y;
            }

            if (bLBDown && bPaused && idxSelStart >= 0)
            {
                Point pts = ClientToSample(e.Location);
                SetSelection(idxSelStart, pts.X);
                splitContainer1.Panel2.Update();
            }

        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            FocusPanel();

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                bLBDown = true;
                ptLBDown = e.Location;
                ofsLBDown = ofs;
                if (bPaused)
                {
                    Point pts = ClientToSample(e.Location);
                    SetSelection(pts.X, pts.X);
                }
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                bRBDown = true;
                ptRBDown = e.Location;
                ofsRBDown = ofs;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                Point pts = ClientToSample(e.Location);

                if ((Control.ModifierKeys & Keys.Shift) == 0)
                    scale.X = 1;
                if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                    scale.Y = 1;

                Point ptcNew = SampleToClient(pts);
                if ((Control.ModifierKeys & Keys.Shift) == 0)
                    ofs.X = 0; //ofs.X -= ptcNew.X - e.Location.X;
                if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                    ofs.Y = 0; //ofs.Y -= ptcNew.Y - e.Location.Y;

                splitContainer1.Panel2.Invalidate();
                return;
            }
        }

        private void Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            FocusPanel();

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (bLBDown && bPaused && idxSelStart >= 0)
                {
                    Point pts = ClientToSample(e.Location);
                    SetSelection(idxSelStart, pts.X);
                }
                bLBDown = false;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                bRBDown = false;
                return;
            }
        }

        private void Mainform_MouseWheel(object sender, MouseEventArgs e)
        {
            //Panel2_MouseWheel(sender, e);
        }

        private void Panel2_MouseWheel(object sender, MouseEventArgs e)
        {
            Point pts = ClientToSample(e.Location);
            Point ptc = SampleToClient(pts);

            float zoom;
            if (e.Delta > 0) zoom = e.Delta / 100.0f; else zoom = -100.0f / e.Delta;
            //if (e.Delta > 0) zoom = 1.01f; else zoom = 1 / 1.01f;
            if ((Control.ModifierKeys & Keys.Shift) == 0)
                scale.X *= zoom;
            if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                scale.Y *= zoom;

            Point ptcNew = SampleToClient(pts);
            if ((Control.ModifierKeys & Keys.Shift) == 0)
                ofs.X -= ptcNew.X - ptc.X;
            if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                ofs.Y -= ptcNew.Y - e.Location.Y;

            Point ptcFinal = SampleToClient(pts);
            splitContainer1.Panel2.Invalidate();
        }

        private void ResetData()
        {
            Samples1.Clear();
            Samples2.Clear();
            stopwatch.Reset();
            if (s_this == null) return;
            for (int i = 1; i < Ch1Labels.Length; ++i)
            {
                Ch1Labels[i].Text = "0.00V";
                Ch2Labels[i].Text = "0.00V";
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool was_paused = bPaused;
            Pause(true);
            ResetData();

            mode = (byte) cbMode.SelectedIndex;

            Color clr1 = Color.Black;
            if (mode == 0)
            {
                labelChannel1.Text = "Channel 1";
            }
            else if (mode == 1)
            {
                labelChannel1.Text = "Channel 2";
            }
            else if (mode == 2)
            {
                labelChannel1.Text = "Channel 1";
            }
            else if (mode == 3)
            {
                labelChannel1.Text = "Channel 2 - Channel 1";
            }
            else if (mode == 4)
            {
                labelChannel1.Text = "Channel 3";
            }
            foreach (Label l in Ch1Labels) l.ForeColor = colors[mode];

            bool b2Channel = (mode == 2);
            foreach (Label l in Ch2Labels) l.Visible = b2Channel;

            FocusPanel();            

            if (!was_paused)
            {
                Thread.Sleep(50);
                Pause(false);               
            }
        }

        private void cbRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool was_paused = bPaused;
            Pause(true);
            ResetData();

            char rate = (char)('0' + cbRate.SelectedIndex);
            SendSerial('R', rate);

            FocusPanel();

            if (!was_paused)
            {
                Thread.Sleep(50);
                Pause(false);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            Pause(!bPaused);
        }

        private void OnKeyDown(Keys key)
        {
            if (key == Keys.Space)
            {
                Pause(!bPaused);
                return;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyCode);
        }

        void Panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            OnKeyDown(e.KeyCode);
        }

        private void cbSmoothZoomOut_CheckedChanged(object sender, EventArgs e)
        {
            bSmoothZoomOut = cbSmoothZoomOut.Checked;
            FocusPanel();
        }

        private void sliderPWM_Scroll(object sender, EventArgs e)
        {
            int val = sliderPWM.Value;
            if (val == 0)
                labelPWM.Text = "Off";
            else if (val < 10)
                labelPWM.Text = (val * 10).ToString() + "%";
            else
                labelPWM.Text = "Every 32 samples";
            char pwm = (char) ('0' + val);
            SendSerial('P', pwm);
        }

        private void VoltageLabel_Click(object sender, EventArgs e)
        {
            Label l = sender as Label;
            l.BorderStyle = (l.BorderStyle == BorderStyle.Fixed3D) ? BorderStyle.None : BorderStyle.Fixed3D;
            splitContainer1.Panel2.Invalidate();
        }

        private void VoltageLabel_MouseEnter(object sender, EventArgs e)
        {
            //Label l = sender as Label;
            //foreach (Label l2 in VoltageLabels)
            //{
            //    if (l2 != l)
            //        VoltageLabel_MouseLeave(l2, e);
            //}
            //if (l.BorderStyle == BorderStyle.None) l.BorderStyle = BorderStyle.FixedSingle;
            //splitContainer1.Panel2.Invalidate();
        }

        private void VoltageLabel_MouseLeave(object sender, EventArgs e)
        {
            //Label l = sender as Label;
            //if (l.BorderStyle == BorderStyle.FixedSingle) l.BorderStyle = BorderStyle.None;
            //splitContainer1.Panel2.Invalidate();
        }

    }
}
