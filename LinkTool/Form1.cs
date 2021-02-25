using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace LinkTool
{
    public partial class Form1 : Form
    {
        public int devnum = -999;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int a = WaveIn.DeviceCount;
            for (int i = 0; i < a; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                if(deviceInfo.ProductName.Contains("Oculus"))
                {
                    devnum = i;
                }

            }
        }
        public WaveIn waveIn;
        public bool rn;
        private void button1_Click(object sender, EventArgs e)
        {
            if(devnum == -999)
            {
                MessageBox.Show("Oculus Headset mic cannot be found. Are you sure you have Oculus software installed?");
            }
            if(rn == true)
            {
                waveIn.StopRecording();
                timer1.Stop();
                button1.Text = "Start";
                rn = false;
            } else
            {
                waveIn = new WaveIn();
                waveIn.DeviceNumber = devnum;
                // Set device channels and samplerate
                int sampleRate = 48000; // 48000 Hz
                int channels = 1; // mono
                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                waveIn.StartRecording();
                timer1.Start(); 
                button1.Text = "Stop";
                rn = true;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("vrserver"); // Detect SteamVR
            if(pname.Length > 0)
            {
                timer1.Stop(); 
                waveIn.StopRecording();
                Process.Start(@"OculusDebugToolCLI.exe", "-f \"aswoff.txt\""); // Disable Asynchronous Spacewarp (vomit)
                Application.Exit();
            }
        }
    }
}
