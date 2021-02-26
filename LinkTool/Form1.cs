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
        public int devnum = -5; // Default is -5 so in the event that the Oculus headset mic isn't detected, -5 will remain to be the value so it can be detected.
        public WaveIn waveIn;
        public bool started;
        public Form1()
        {
            InitializeComponent();
        }
        public void StartMic()
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = devnum;
            // Set device channels and samplerate
            int sampleRate = 48000; // 48000 Hz
            int channels = 1; // mono
            waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            waveIn.StartRecording();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                if(WaveIn.GetCapabilities(i).ProductName.Contains("Oculus")) // This shouldn't conflict with any other input devices as no other audio INPUT devices should have Oculus in the ProductName
                {
                    devnum = i;
                    break; // Don't need to go through the entire loop if we've found it already!
                }
            }
            switch(devnum)
            {
                case -5:
                    MessageBox.Show("Oculus Headset Mic Virtual Device not found. Is Oculus installed? Is the device disabled?");
                break;
                default:
                    StartMic();
                    timer1.Start();
                    button1.Text = "Stop";
                    started = true;
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch(devnum)
            {
                case -5:
                    MessageBox.Show("Oculus Headset Mic Virtual Device not found. Is Oculus installed? Is the device disabled?");
                break;
                default:
                    switch(started)
                    {
                        case true:
                            waveIn.StopRecording();
                            timer1.Stop();
                            button1.Text = "Start";
                            started = false;
                        break;
                        case false:
                            StartMic();
                            timer1.Start();
                            button1.Text = "Stop";
                            started = true;
                            break;
                    }
                break;
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
