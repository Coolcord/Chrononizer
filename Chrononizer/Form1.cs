using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            dSize = GetDownscaledSize("E:\\Music\\.downscaled", dSize, ref dFlac); //recurse through the downscaled files
            double s1 = GetDirectorySize("E:\\Music\\", 0, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf);
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

        private double GetDirectorySize(string root, double num, ref long flac, ref long mp3, ref long wma, ref long m4a, ref long ogg, ref long wav, ref long xm, ref long mod, ref long nsf)
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
                    Luminescence.Xiph.FlacTagger flacTag = new FlacTagger(name); //get the flac's tag
                    if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                    {
                        string musicLibrary = "E:\\Music\\";
                        string downscaledLibrary = "E:\\Music\\.downscaled\\";
                        string downscaledFile = downscaledLibrary + name.Substring(musicLibrary.Length);
                        if (File.Exists(downscaledFile))
                        {
                            flacTag = new FlacTagger(downscaledFile);
                            if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                            {
                                listBox1.Items.Add(name); //if it does not meet the minimum requirements, it needs to be downscaled
                            }
                        }
                        else listBox1.Items.Add(name); //no downscaled file exists
                    }
                    flacTag = null; //make sure it is not accessed again
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
                if (Path.GetFullPath(name) == "E:\\Music\\.downscaled") continue; //don't scan through the downscaled files
                num = GetDirectorySize(name, num, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf); //recurse through the folders
            }
            return num;
        }

        private double GetDownscaledSize(string root, double num, ref long flac)
        {
            string[] files = Directory.GetFiles(root, "*.*"); //get array of all file names
            string[] folders = Directory.GetDirectories(root); //get array of all folder names for this directory
            if (!Directory.EnumerateFileSystemEntries(root).Any())
            {
                Directory.Delete(root); //delete empty folders
                return num;
            }

            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name); //read in the file
                String ext = Path.GetExtension(name); //get the file's extension
                //increment count based upon file type and extension type
                if (ext == ".flac")
                {
                    string musicLibrary = "E:\\Music\\";
                    string downscaledLibrary = "E:\\Music\\.downscaled\\";
                    string defaultFile = musicLibrary + name.Substring(downscaledLibrary.Length);
                    Luminescence.Xiph.FlacTagger flacTag = new FlacTagger(name); //get the flac's tag
                    if (!File.Exists(defaultFile) || flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000) File.Delete(name); //downscaled flac not necessary
                    else
                    {
                        flacTag = new FlacTagger(defaultFile);
                        if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                        {
                            num += info.Length; //add to the size
                            flac++; //flac is ok to use
                        }
                        else File.Delete(name); //flac did not need downscaling
                    }
                    flacTag = null;
                }
                else File.Delete(name);
            }
            foreach (string name in folders)
            {
                num = GetDownscaledSize(name, num, ref flac); //recurse through the folders
            }

            return num;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex; //determine what is selected
            string file = listBox1.Items[index].ToString(); //get the selected filename
            if (file == "No files need downscaling!") return; //don't try to open windows explorer
            if (!File.Exists(file)) //check to see if the file is still in the folder
            {
                listBox1.Items.RemoveAt(index); //remove the value if it no longer exists
                if (listBox1.Items.Count == 0)
                    listBox1.Items.Add("No files need downscaling!"); //no more files need downscaling
                return;
            }
            Process.Start("explorer.exe", @"/select, " + file); //open windows explorer and selected the file
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
