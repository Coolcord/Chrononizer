﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;
using System.Media;
using Luminescence.Xiph;
//using NAudio.Wave;
//using BigMansStuff.NAudio.FLAC;

namespace Chrononizer
{
    public partial class Form1 : Form
    {
        private string MusicLibrary = " ";
        private string DownscaledLibrary = " ";
        Boolean AutoHandle = true;
        Boolean RemoveImproper = true;
        Boolean ShowFiles = false;
        Boolean RemoveUnsupported = true;
        Boolean RemoveUnnecessary = true;
        Boolean RemoveEmpty = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form form1 = this;
            form1.FormBorderStyle = FormBorderStyle.FixedDialog;

            if (!Properties.Settings.Default.FirstBoot)
            {
                //load saved settings
                textBox1.Text = Properties.Settings.Default.MusicLibrary;
                textBox2.Text = Properties.Settings.Default.DownscaledLibrary;
                checkBox1.Checked = Properties.Settings.Default.RemoveImproper;
                checkBox2.Checked = Properties.Settings.Default.AutoHandle;
                checkBox3.Checked = Properties.Settings.Default.ShowFiles;
                checkBox4.Checked = Properties.Settings.Default.RemoveUnsupported;
                checkBox5.Checked = Properties.Settings.Default.RemoveUnnecessary;
                checkBox6.Checked = Properties.Settings.Default.RemoveEmpty;
            }
            else
            {
                //first time launching application
                Properties.Settings.Default.FirstBoot = false;

                //attempt to find the music library
                string drive = Environment.GetFolderPath(Environment.SpecialFolder.System);
                drive = drive.Substring(0, 1); //get drive letter
                string username = WindowsIdentity.GetCurrent().Name.Split('\\')[1]; //get username from login
                
                //load default settings
                textBox1.Text = drive + ":\\Users\\" + username + "\\Music\\";
                textBox2.Text = drive + ":\\Users\\" + username + "\\Music\\.downscaled\\";
                checkBox1.Checked = Properties.Settings.Default.RemoveImproper;
                checkBox2.Checked = Properties.Settings.Default.AutoHandle;
                checkBox3.Checked = Properties.Settings.Default.ShowFiles;
                checkBox4.Checked = Properties.Settings.Default.RemoveUnsupported;
                checkBox5.Checked = Properties.Settings.Default.RemoveUnnecessary;
                checkBox6.Checked = Properties.Settings.Default.RemoveEmpty;

                Properties.Settings.Default.Save();
            }

            //store the values
            MusicLibrary = textBox1.Text;
            DownscaledLibrary = textBox2.Text;
            AutoHandle = checkBox2.Checked;
            RemoveImproper = checkBox1.Checked;
            ShowFiles = checkBox3.Checked;
            RemoveUnsupported = checkBox4.Checked;
            RemoveUnnecessary = checkBox5.Checked;
            RemoveEmpty = checkBox6.Checked;
        }

        private void CheckSize_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ScannerSweep);
            aSoundPlayer.Play();  //Plays the sound in a new thread

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
            dSize = GetDownscaledSize(DownscaledLibrary, dSize, ref dFlac); //recurse through the downscaled files
            if (AutoHandle && Directory.Exists(MusicLibrary) && Directory.Exists(DownscaledLibrary)) //set the file attributes if auto handling is on
                File.SetAttributes(DownscaledLibrary, FileAttributes.Hidden | FileAttributes.System);
            double s1 = GetDirectorySize(MusicLibrary, 0, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf);
            label1.Text = "Library: " + BytesToSize(s1); //display the size
            label2.Text = "FLAC: " + Plural(flac, "file"); //display the number of flac songs
            label3.Text = "MP3: " + Plural(mp3, "file"); //display the number of mp3 songs
            label4.Text = "WMA: " + Plural(wma, "file"); //display the number of wma songs
            label5.Text = "M4A: " + Plural(m4a, "file"); //display the number of m4a songs
            label6.Text = "OGG: " + Plural(ogg, "file"); //display the number of ogg songs
            label7.Text = "WAV: " + Plural(wav, "file"); //display the number of wav songs
            label8.Text = "XM: " + Plural(xm, "file"); //display the number of xm songs
            label9.Text = "MOD: " + Plural(mod, "file"); //display the number of mod songs
            label10.Text = "NSF: " + Plural(nsf, "file"); //display the number of nsf songs
            audioTotal = flac + mp3 + wma + m4a + ogg + wav;
            chiptunesTotal = xm + mod + nsf;
            total = audioTotal + chiptunesTotal;
            label11.Text = "Library: " + Plural(audioTotal,  "song");
            label12.Text = "Chiptunes: " + Plural(chiptunesTotal, "song");
            label13.Text = "Total (without downscaled): " + Plural(total, "song");
            label15.Text = "Downscaled: " + BytesToSize(dSize);
            label16.Text = "Downscaled: " + Plural(dFlac, "file");
            allSize = s1 + dSize;
            label18.Text = "Total: " + BytesToSize(allSize);
            if (listBox1.Items.Count == 0)
            {
                listBox1.Items.Add("No files need downscaling!");
            }
            CheckSize.Text = "Rescan Library";
        }

        static string Plural(long value, string units)
        {
            string plural = " ";
            if (value == 1)
            {
                plural = value.ToString() + " " + units; //only one object
            }
            else
            {
                plural = value.ToString() + " " + units + "s"; //add an s if plural
            }
            return plural;
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
                    if (num == 1) s += " byte";
                    else s += " bytes";
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
            if (!Directory.Exists(root)) return num; //path is invalid
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
                        string downscaledFile = DownscaledLibrary + name.Substring(MusicLibrary.Length);
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
                if (Path.GetFullPath(name) == DownscaledLibrary.Substring(0, DownscaledLibrary.Length - 1)) continue; //don't scan through the downscaled files
                num = GetDirectorySize(name, num, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf); //recurse through the folders
            }
            return num;
        }

        private double GetDownscaledSize(string root, double num, ref long flac)
        {
            if (!Directory.Exists(root))
            {
                if (AutoHandle && Directory.Exists(MusicLibrary)) Directory.CreateDirectory(root); //create the directory if it doesn't exist
                return num;
            }
            string[] files = Directory.GetFiles(root, "*.*"); //get array of all file names
            string[] folders = Directory.GetDirectories(root); //get array of all folder names for this directory

            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name); //read in the file
                String ext = Path.GetExtension(name); //get the file's extension
                //increment count based upon file type and extension type
                if (ext == ".flac")
                {
                    string defaultFile = MusicLibrary + name.Substring(DownscaledLibrary.Length);
                    Luminescence.Xiph.FlacTagger flacTag = new FlacTagger(name); //get the flac's tag
                    if (!File.Exists(defaultFile))
                    {
                        if (RemoveUnnecessary)
                            File.Delete(name); //flac's upscaled file does not exist and is unnecessary
                        else
                        {
                            num += info.Length; //add to the size
                            flac++;
                        }
                    }
                    else if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                    {
                        if (RemoveImproper)
                            File.Delete(name); //downscaled flac not necessary
                        else
                        {
                            if (ShowFiles) listBox1.Items.Add(name); //show the file in the list
                            num += info.Length; //add to the size
                            flac++;
                        }
                    }
                    else
                    {
                        flacTag = new FlacTagger(defaultFile);
                        if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                        {
                            num += info.Length; //add to the size
                            flac++; //flac is ok to use
                        }
                        else
                        {
                            if (RemoveUnnecessary)
                                File.Delete(name); //flac did not need downscaling and is unnecessary
                            else
                            {
                                num += info.Length; //add to the size
                                flac++;
                            }
                        }
                    }
                    flacTag = null;
                }
                else if (RemoveUnsupported) File.Delete(name); //remove unsupported files
            }
            foreach (string name in folders)
            {
                num = GetDownscaledSize(name, num, ref flac); //recurse through the folders
            }

            if (RemoveEmpty && !Directory.EnumerateFileSystemEntries(root).Any() && Path.GetFullPath(root) != DownscaledLibrary)
            {
                Directory.Delete(root); //delete empty folders
                return num;
            }
            return num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
            aSoundPlayer.Play();  //Plays the sound in a new thread
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
            aSoundPlayer.Play();  //Plays the sound in a new thread
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
            aSoundPlayer.Play();  //Plays the sound in a new thread
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

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(2);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(1);
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(2);
        }

        private void OpenFolderDialog(int TextBox)
        {
            FolderBrowserDialog open = new FolderBrowserDialog();
            if (open.ShowDialog() != DialogResult.OK) return;
            //set the path accordingly
            if (TextBox == 1)
            {
                textBox1.Text = open.SelectedPath + "\\";
                if (checkBox2.Checked) textBox2.Text = textBox1.Text + ".downscaled\\";
            }
            else if (TextBox == 2) textBox2.Text = open.SelectedPath + "\\";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label22.Enabled = false;
                textBox2.Enabled = false;
                button5.Enabled = false;
                checkBox1.Enabled = false;
                checkBox1.Checked = true;
                checkBox3.Enabled = false;
                checkBox3.Checked = false;
                checkBox4.Enabled = false;
                checkBox4.Checked = true;
                checkBox5.Enabled = false;
                checkBox5.Checked = true;
                checkBox6.Enabled = false;
                checkBox6.Checked = true;
                textBox2.Text = textBox1.Text + ".downscaled\\";
            }
            else
            {
                label22.Enabled = true;
                textBox2.Enabled = true;
                button5.Enabled = true;
                checkBox1.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
            }
            AutoHandle = checkBox2.Checked;
            Properties.Settings.Default.AutoHandle = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MusicLibrary = textBox1.Text;
            Properties.Settings.Default.MusicLibrary = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            DownscaledLibrary = textBox2.Text;
            Properties.Settings.Default.DownscaledLibrary = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox3.Enabled = false;
                checkBox3.Checked = false;
            }
            else
            {
                checkBox3.Enabled = true;
            }
            RemoveImproper = checkBox1.Checked;
            Properties.Settings.Default.RemoveImproper = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ShowFiles = checkBox3.Checked;
            Properties.Settings.Default.ShowFiles = checkBox3.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnsupported = checkBox4.Checked;
            Properties.Settings.Default.RemoveUnsupported = checkBox4.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnnecessary = checkBox5.Checked;
            Properties.Settings.Default.RemoveUnnecessary = checkBox5.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            RemoveEmpty = checkBox6.Checked;
            Properties.Settings.Default.RemoveEmpty = checkBox6.Checked;
            Properties.Settings.Default.Save();
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
