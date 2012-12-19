using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using NAudio.Wave;
using Luminescence.Xiph;
//using BigMansStuff.NAudio.FLAC;

namespace Chrononizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form form1 = this;
            form1.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void CheckSize_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); //clear out previous items

            long flac = 0;
            long mp3 = 0;
            long wma = 0;
            long m4a = 0;
            long ogg = 0;
            long wav = 0;
            long xm = 0;
            long mod = 0;
            long nsf = 0;
            long audioTotal = 0;
            long chiptunesTotal = 0;
            long total = 0;
            double dSize = 0;
            long dFlac = 0;
            double allSize = 0;
            double s1 = GetDirectorySize("E:\\Music\\", 0, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf, ref dSize, ref dFlac);
            label1.Text = "Library: " + BytesToSize(s1); //display the size
            label2.Text = "FLAC: " + flac.ToString() + " files"; //display the number of flac songs
            label3.Text = "MP3: " + mp3.ToString() + " files"; //display the number of mp3 songs
            label4.Text = "WMA: " + wma.ToString() + " files"; //display the number of wma songs
            label5.Text = "M4A: " + m4a.ToString() + " files"; //display the number of m4a songs
            label6.Text = "OGG: " + ogg.ToString() + " files"; //display the number of ogg songs
            label7.Text = "WAV: " + wav.ToString() + " files"; //display the number of wav songs
            label8.Text = "XM: " + xm.ToString() + " files"; //display the number of xm songs
            label9.Text = "MOD: " + mod.ToString() + " files"; //display the number of mod songs
            label10.Text = "NSF: " + nsf.ToString() + " files"; //display the number of nsf songs
            audioTotal = flac + mp3 + wma + m4a + ogg + wav;
            chiptunesTotal = xm + mod + nsf;
            total = audioTotal + chiptunesTotal;
            label11.Text = "Library: " + audioTotal.ToString() + " songs";
            label12.Text = "Chiptunes: " + chiptunesTotal.ToString() + " songs";
            label13.Text = "Total (without downscaled): " + total.ToString() + " songs";
            label15.Text = "Downscaled: " + BytesToSize(dSize);
            label16.Text = "Downscaled: " + dFlac.ToString() + " files";
            allSize = s1 + dSize;
            label18.Text = "Total: " + BytesToSize(allSize);

            ///////////////////////
            //Testing Area
            ///////////////////////




            if (listBox1.Items.Count == 0)
            {
                listBox1.Items.Add("No files need downscaling!");
            }
            ///////////////////////
            //Testing Area
            ///////////////////////

            CheckSize.Text = "Rescan Library";
        }

        static string BytesToSize(double num)
        {
            int type = 0;
            string s = " ";
            while (num > 1024)
            {
                num /= 1024;
                type++;
                if (type >= 5)
                {
                    break; //don't calculate beyond PB
                }
            }
            num = Math.Round(num, 2); //round to two decimal places
            s = num.ToString(); //put the number in a string
            switch (type)
            {
                case 0:
                    s += " bytes";
                    break;
                case 1:
                    s += " KB";
                    break;
                case 2:
                    s += " MB";
                    break;
                case 3:
                    s += " GB";
                    break;
                case 4:
                    s += " TB";
                    break;
                case 5:
                    s += " PB";
                    break;
                default:
                    break;
            }
            return s;
        }

        static double GetDirectorySize(string root, double num, ref long flac, ref long mp3, ref long wma, ref long m4a, ref long ogg, ref long wav, ref long xm, ref long mod, ref long nsf, ref double dSize, ref long dFlac)
        {
            string[] files = Directory.GetFiles(root, "*.*"); //get array of all file names
            string[] folders = Directory.GetDirectories(root); //get array of all folder names for this directory

            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name); //read in the file
                String ext = Path.GetExtension(name); //get the file's extension
                num += info.Length; //get the length
                //increment count based upon file type and extension type
                if (ext == ".flac")
                {
                    flac++;
                }
                else if (ext == ".mp3")
                {
                    mp3++;
                }
                else if (ext == ".wma")
                {
                    wma++;
                }
                else if (ext == ".m4a")
                {
                    m4a++;
                }
                else if (ext == ".ogg")
                {
                    ogg++;
                }
                else if (ext == ".wav")
                {
                    wav++;
                }
                else if (ext == ".xm")
                {
                    xm++;
                }
                else if (ext == ".mod")
                {
                    mod++;
                }
                else if (ext == ".nsf")
                {
                    nsf++;
                }
            }
            foreach (string name in folders)
            {
                if (Path.GetFullPath(name) == "E:\\Music\\.downscaled")
                {
                    dSize = GetDownscaledSize(name, dSize, ref dFlac); //recurse through the downscaled files
                    continue;
                }
                num = GetDirectorySize(name, num, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf, ref dSize, ref dFlac); //recurse through the folders
            }

            return num;
        }

        static double GetDownscaledSize(string root, double num, ref long flac)
        {
            string[] files = Directory.GetFiles(root, "*.*"); //get array of all file names
            string[] folders = Directory.GetDirectories(root); //get array of all folder names for this directory

            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name); //read in the file
                String ext = Path.GetExtension(name); //get the file's extension
                num += info.Length; //get the length
                //increment count based upon file type and extension type
                if (ext == ".flac")
                {
                    flac++;
                }
            }
            foreach (string name in folders)
            {
                num = GetDownscaledSize(name, num, ref flac); //recurse through the folders
            }

            return num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Luminescence.Xiph.FlacTagger flacTagTest = new FlacTagger("E:\\PinkiePieSwear\\Test\\TestBadFile2.flac");

            int bitDepth = flacTagTest.BitsPerSample; //this gets the file's bit depth
            int bitRate = flacTagTest.SampleRate; //this gets the file's bit rate
            button1.Text = bitRate.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The Code below is for testing and learning.
        /// It ultimately serves no purpose.
        /// </summary>








        /*
        private NAudio.Wave.WaveFileReader waveFile = null;
        private NAudio.Wave.DirectSoundOut output = null;
        private BigMansStuff.NAudio.FLAC.FLACFileReader flacFile = null;

        private void DisposeWave()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        private void DisposeFLAC()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (flacFile != null)
            {
                flacFile.Dispose();
                flacFile = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisposeWave();
            DisposeFLAC();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "FLAC File (*.flac)|*.flac;";
            if (open.ShowDialog() != DialogResult.OK) return;

            flacFile = new BigMansStuff.NAudio.FLAC.FLACFileReader(open.FileName);

            
            //OpenFileDialog open = new OpenFileDialog();
            //open.Filter = "Wave File (*.wav)|*.wav;";
            //if (open.ShowDialog() != DialogResult.OK) return;

            //waveFile = new NAudio.Wave.WaveFileReader(open.FileName);
            //output = new NAudio.Wave.DirectSoundOut();
            //output.Init(new NAudio.Wave.WaveChannel32(wave));
            //output.Play();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
         * 
         * */
    }
}
