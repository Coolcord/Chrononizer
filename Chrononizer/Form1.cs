using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Principal;
using System.Media;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using Luminescence.Xiph;

namespace Chrononizer
{
    public partial class Form1 : Form
    {
        private string MusicLibrary = " ";
        private string DownscaledLibrary = " ";
        private string ChiptunesLibrary = " ";
        Boolean EnableChiptunes = true;
        Boolean AutoHandle = true;
        Boolean RemoveImproper = true;
        Boolean ShowFiles = false;
        Boolean RemoveUnsupported = true;
        Boolean RemoveUnnecessary = true;
        Boolean RemoveEmpty = true;
        Dictionary<string, Boolean> checkedFiles = new Dictionary<string, Boolean>();

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
                textBox3.Text = Properties.Settings.Default.ChiptunesLibrary;
                checkBox1.Checked = Properties.Settings.Default.RemoveImproper;
                checkBox2.Checked = Properties.Settings.Default.AutoHandle;
                checkBox3.Checked = Properties.Settings.Default.ShowFiles;
                checkBox4.Checked = Properties.Settings.Default.RemoveUnsupported;
                checkBox5.Checked = Properties.Settings.Default.RemoveUnnecessary;
                checkBox6.Checked = Properties.Settings.Default.RemoveEmpty;
                checkBox7.Checked = Properties.Settings.Default.EnableChiptunes;
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
                textBox3.Text = drive + ":\\Users\\" + username + "\\Music\\Chiptunes";
                checkBox1.Checked = Properties.Settings.Default.RemoveImproper;
                checkBox2.Checked = Properties.Settings.Default.AutoHandle;
                checkBox3.Checked = Properties.Settings.Default.ShowFiles;
                checkBox4.Checked = Properties.Settings.Default.RemoveUnsupported;
                checkBox5.Checked = Properties.Settings.Default.RemoveUnnecessary;
                checkBox6.Checked = Properties.Settings.Default.RemoveEmpty;
                checkBox7.Checked = Properties.Settings.Default.EnableChiptunes;

                Properties.Settings.Default.Save();
            }

            //store the values
            MusicLibrary = textBox1.Text;
            DownscaledLibrary = textBox2.Text;
            ChiptunesLibrary = textBox3.Text;
            AutoHandle = checkBox2.Checked;
            RemoveImproper = checkBox1.Checked;
            ShowFiles = checkBox3.Checked;
            RemoveUnsupported = checkBox4.Checked;
            RemoveUnnecessary = checkBox5.Checked;
            RemoveEmpty = checkBox6.Checked;
            EnableChiptunes = checkBox7.Checked;
        }

        private void CheckSize_Click(object sender, EventArgs e)
        {
            if (ScanBW.IsBusy != true)
            {
                CheckSize.Text = "Scanning...";
                ScanBW.RunWorkerAsync();
            }
            else
            {
                CheckSize.Text = "Not done yet!";
            }
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
                        checkedFiles.Add(name, false); //mark that it has been checked and is proper
                        string downscaledFile = DownscaledLibrary + name.Substring(MusicLibrary.Length);
                        if (File.Exists(downscaledFile))
                        {
                            flacTag = new FlacTagger(downscaledFile);
                            if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                            {
                                this.BeginInvoke (new MethodInvoker(() => listBox1.Items.Add(name))); //if it does not meet the minimum requirements, it needs to be downscaled
                                checkedFiles.Add(downscaledFile, false); //mark that it has been checked and is not proper
                            }
                            else checkedFiles.Add(downscaledFile, true); //mark that it has been checked and is proper
                        }
                        else this.BeginInvoke (new MethodInvoker(() => listBox1.Items.Add(name))); //no downscaled file exists
                    }
                    else checkedFiles.Add(name, true); //mark that it has been checked and is not proper
                    flacTag = null; //make sure it is not accessed again
                }
                else if (ext == ".mp3") mp3++;
                else if (ext == ".wma") wma++;
                else if (ext == ".m4a") m4a++;
                else if (ext == ".ogg") ogg++;
                else if (ext == ".wav") wav++;
                else if (ext == ".xm") xm++;
                else if (ext == ".mod") mod++;
                else if (ext == ".nsf") nsf++;
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
                    Boolean valid = false;
                    Boolean scanned = false;
                    scanned = checkedFiles.TryGetValue(name, out valid);
                    if (scanned && valid)
                    {
                        num += info.Length; //add to the size
                        flac++;
                    }
                    else if (scanned && !valid)
                    {
                        if (RemoveImproper)
                            File.Delete(name); //downscaled flac not necessary
                        else
                        {
                            if (ShowFiles) this.BeginInvoke (new MethodInvoker(() => listBox1.Items.Add(name))); //show the file in the list
                            num += info.Length; //add to the size
                            flac++;
                        }
                    }
                    else
                    {
                        string defaultFile = MusicLibrary + name.Substring(DownscaledLibrary.Length);
                        valid = false;
                        scanned = false;
                        scanned = checkedFiles.TryGetValue(name, out valid);
                        checkedFiles.TryGetValue(defaultFile, out valid);
                        if (scanned && !valid)
                        {
                            num += info.Length; //add to the size
                            flac++;
                        }
                        else if (scanned && valid)
                        {
                            if (RemoveUnnecessary) File.Delete(name); //file is unnecessary
                        }
                        else
                        {
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
                                    if (ShowFiles) this.BeginInvoke (new MethodInvoker(() => listBox1.Items.Add(name))); //show the file in the list
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
                    }
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

        private void RemoveEmptyDirectories(string root)
        {
            string[] folders = Directory.GetDirectories(root); //get array of all folder names for this directory
            if (!Directory.EnumerateFileSystemEntries(root).Any() && Path.GetFullPath(root) != MusicLibrary)
            {
                Directory.Delete(root); //delete empty folders
                return;
            }
            foreach (string name in folders)
            {
                RemoveEmptyDirectories(name); //recurse through the folders
            }
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            ProgressBar Progress = progressBar1;
            Progress.Maximum = 5;
            Progress.Value = 0;
            Progress.Increment(1);
            Progress.Increment(1);
            Progress.Increment(1);
            Progress.Increment(1);
            Progress.Increment(1);
            */

            if (!PMPSyncBW.IsBusy && !LaptopSyncBW.IsBusy)
            {
                PMPSyncBW.RunWorkerAsync();
                LaptopSyncBW.RunWorkerAsync();
                button1.Text = "Running...";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!PMPSyncBW.IsBusy && !LaptopSyncBW.IsBusy)
            {
                PMPSyncBW.RunWorkerAsync();
                button2.Text = "Running...";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!PMPSyncBW.IsBusy && !LaptopSyncBW.IsBusy)
            {
                LaptopSyncBW.RunWorkerAsync();
                button3.Text = "Running...";
            }
        }

        public void ShowSyncStatus(Boolean visible)
        {
            if (visible)
            {
                panel1.Visible = true;
                tabControl1.Visible = false;
            }
            else
            {
                tabControl1.Visible = true;
                panel1.Visible = false;
            }
        }

        public void PrepareSyncPMP()
        {
            DirectoryInfo PMPDrive = null;
            Boolean PMPFound = false;
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                try
                {
                    if (drive.VolumeLabel == "X7 HDD")
                    {
                        PMPDrive = drive.RootDirectory;
                        if (!Directory.Exists(PMPDrive + "System\\") || !Directory.Exists(PMPDrive + "Music\\") || !File.Exists(PMPDrive + "DID.bin") || !File.Exists(PMPDrive + "nonce.bin"))
                            PMPFound = false; //PMP is missing some core system files
                        else
                            PMPFound = true; //PMP found
                        break;
                    }
                }
                catch (Exception) { }
            }

            if (PMPFound)
            {
                DialogResult result = MessageBox.Show("PMP found on " + PMPDrive + "\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
                    aSoundPlayer.Play();  //Plays the sound in a new thread
                    ShowSyncStatus(true);
                    SyncPMP(MusicLibrary, PMPDrive + "Music");
                    MessageBox.Show("Done!");
                    ShowSyncStatus(false);
                }
            }
            else MessageBox.Show("PMP could not be found! Make sure that it is connected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void PrepareSyncLaptop()
        {
            string LaptopName = "SUPERMOBILEROB";
            string LaptopUsername = "Cord";

            if (Directory.Exists("\\\\" + LaptopName + "\\Users\\" + LaptopUsername + "\\Music"))
            {
                DialogResult result = MessageBox.Show(LaptopName + " logged in as " + LaptopUsername + " is mounted.\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
                    aSoundPlayer.Play();  //Plays the sound in a new thread
                    ShowSyncStatus(true);
                    Sync(MusicLibrary, "\\\\" + LaptopName + "\\Users\\" + LaptopUsername + "\\Music");
                    MessageBox.Show("Done!");
                    ShowSyncStatus(false);
                }
            }
            else MessageBox.Show("Laptop is not connected! Make sure that it is mounted properly!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SyncPMP(string sourcePath, string destinationPath)
        {
            bool dirExisted = DirExists(destinationPath);

            //get the source files
            string[] srcFiles = Directory.GetFiles(sourcePath);
            foreach (string sourceFile in srcFiles)
            {
                string correctFile = sourceFile;

                if (Path.GetExtension(sourceFile) == ".flac")
                {
                    Luminescence.Xiph.FlacTagger flacTag = new FlacTagger(correctFile); //get the flac's tag
                    if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                    {
                        if (File.Exists(DownscaledLibrary + sourceFile.Substring(MusicLibrary.Length)))
                            correctFile = DownscaledLibrary + sourceFile.Substring(MusicLibrary.Length); //redirect to downscaled file
                    }
                }

                FileInfo sourceInfo = new FileInfo(correctFile);
                string destFile = Path.Combine(destinationPath, sourceInfo.Name);
                if (!dirExisted && File.Exists(destFile))
                {
                    FileInfo destInfo = new FileInfo(destFile);
                    if (sourceInfo.LastWriteTime > destInfo.LastWriteTime)
                    {
                        //file is newer, so copy it
                        File.Copy(correctFile, Path.Combine(destinationPath, sourceInfo.Name), true);
                    }
                }
                else
                {
                    File.Copy(correctFile, Path.Combine(destinationPath, sourceInfo.Name));
                }
            }

            DeleteOldDestinationFiles(srcFiles, destinationPath);

            //now process the directories if exist
            string[] dirs = Directory.GetDirectories(sourcePath);
            DeleteOldDestinationDirectories(dirs, destinationPath);
            foreach (string dir in dirs)
            {
                if (dir == DownscaledLibrary.Substring(0, DownscaledLibrary.Length - 1))
                    continue; //skip downscaled folder
                if (EnableChiptunes && dir == ChiptunesLibrary.Substring(0, ChiptunesLibrary.Length - 1))
                    continue; //skip chiptunes folder
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                //recursive do the directories
                SyncPMP(dir, Path.Combine(destinationPath, dirInfo.Name));
            }
        }

        public void Sync(string sourcePath, string destinationPath)
        {
            bool dirExisted = DirExists(destinationPath);

            //get the source files
            string[] srcFiles = Directory.GetFiles(sourcePath);
            foreach (string sourceFile in srcFiles)
            {
                FileInfo sourceInfo = new FileInfo(sourceFile);
                string destFile = Path.Combine(destinationPath, sourceInfo.Name);
                if (!dirExisted && File.Exists(destFile))
                {
                    FileInfo destInfo = new FileInfo(destFile);
                    if (sourceInfo.LastWriteTime > destInfo.LastWriteTime)
                    {
                        //file is newer, so copy it
                        File.Copy(sourceFile, Path.Combine(destinationPath, sourceInfo.Name), true);
                    }
                }
                else
                {
                    File.Copy(sourceFile, Path.Combine(destinationPath, sourceInfo.Name));
                }
            }

            DeleteOldDestinationFiles(srcFiles, destinationPath);

            //now process the directories if exist
            string[] dirs = Directory.GetDirectories(sourcePath);
            DeleteOldDestinationDirectories(dirs, destinationPath);
            foreach (string dir in dirs)
            {
                if (dir == DownscaledLibrary.Substring(0, DownscaledLibrary.Length-1))
                    continue;
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                //recursive do the directories
                Sync(dir, Path.Combine(destinationPath, dirInfo.Name));
            }
        }

        private bool DirExists(string path)
        {
            //create destination directory if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void DeleteOldDestinationFiles(string[] sourceFiles, string destinationPath)
        {
            //get the destination files
            string[] dstFiles = Directory.GetFiles(destinationPath);

            foreach (string dstFile in dstFiles)
            {
                FileInfo f = new FileInfo(dstFile);
                string[] found = Array.FindAll(sourceFiles, str => GetFileName(str).Equals(f.Name));

                if (found.Length == 0)
                {
                    //delete file if not found in destination
                    File.Delete(dstFile);
                }
            }
        }

        private static void DeleteOldDestinationDirectories(string[] sourceDirectories, string destinationPath)
        {
            //get the destination files
            string[] dstDirectories = Directory.GetDirectories(destinationPath);

            foreach (string dstDirectory in dstDirectories)
            {
                FileInfo f = new FileInfo(dstDirectory);
                string[] found = Array.FindAll(sourceDirectories, str => GetDirectoryName(str).Equals(f.Name));

                if (found.Length == 0)
                {
                    //delete file if not found in destination
                    Directory.Delete(dstDirectory, true);
                }
            }
        }

        private static string GetFileName(string path)
        {
            FileInfo i = new FileInfo(path);
            return i.Name;
        }

        private static string GetDirectoryName(string path)
        {
            FileInfo i = new FileInfo(path);
            return i.Name;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex; //determine what is selected
            if (index < 0) return;
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

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(3);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(1);
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(2);
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(3);
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
            else if (TextBox == 3) textBox3.Text = open.SelectedPath + "\\";
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ChiptunesLibrary = textBox3.Text;
            Properties.Settings.Default.ChiptunesLibrary = textBox3.Text;
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

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                label23.Enabled = true;
                textBox3.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                label23.Enabled = false;
                textBox3.Enabled = false;
                button6.Enabled = false;
            }
            EnableChiptunes = checkBox7.Checked;
            Properties.Settings.Default.EnableChiptunes = checkBox7.Checked;
            Properties.Settings.Default.Save();
        }

        private void PMPSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            PrepareSyncPMP();
            this.BeginInvoke(new MethodInvoker(() =>
            {
                button2.Text = "Synchronize PMP";
                if (LaptopSyncBW.IsBusy == false)
                    button1.Text = "Synchronize Both";
            }));
        }

        private void ScanBW_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ScannerSweep);
            aSoundPlayer.Play();  //Plays the sound in a new thread

            this.BeginInvoke (new MethodInvoker(() => listBox1.Items.Clear())); //clear out previous items

            long flac, mp3, wma, m4a, ogg, wav, xm, mod, nsf, audioTotal, chiptunesTotal, total, dFlac;
            flac = mp3 = wma = m4a = ogg = wav = xm = mod = nsf = audioTotal = chiptunesTotal = total = dFlac = 0;
            double dSize, allSize, s1;
            dSize = allSize = s1 = 0;

            s1 = GetDirectorySize(MusicLibrary, 0, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf);
            dSize = GetDownscaledSize(DownscaledLibrary, dSize, ref dFlac); //recurse through the downscaled files
            if (AutoHandle && Directory.Exists(MusicLibrary) && Directory.Exists(DownscaledLibrary)) //set the file attributes if auto handling is on
                File.SetAttributes(DownscaledLibrary, FileAttributes.Hidden | FileAttributes.System);
            audioTotal = flac + mp3 + wma + m4a + ogg + wav;
            chiptunesTotal = xm + mod + nsf;
            total = audioTotal + chiptunesTotal;
            allSize = s1 + dSize;
            this.BeginInvoke(new MethodInvoker(() =>
            {
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
                label11.Text = "Library: " + Plural(audioTotal, "song");
                label12.Text = "Chiptunes: " + Plural(chiptunesTotal, "song");
                label13.Text = "Total (without downscaled): " + Plural(total, "song");
                label15.Text = "Downscaled: " + BytesToSize(dSize);
                label16.Text = "Downscaled: " + Plural(dFlac, "file");
                label18.Text = "Total: " + BytesToSize(allSize);
                if (listBox1.Items.Count == 0)
                {
                    listBox1.Items.Add("No files need downscaling!");
                }
                CheckSize.Text = "Rescan Library";
                checkedFiles.Clear();
            }));
        }

        private void LaptopSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            PrepareSyncLaptop();
            this.BeginInvoke (new MethodInvoker(() => 
            {
                button3.Text = "Synchronize Laptop";
                if (PMPSyncBW.IsBusy == false)
                    button1.Text = "Synchronize Both";
            }));
        }
    }
}
