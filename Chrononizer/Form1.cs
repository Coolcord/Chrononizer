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
        Boolean PreventSynchingUpscaled = true;
        Dictionary<string, Boolean> checkedFiles = new Dictionary<string, Boolean>();

        ProgressBar pb1, LTpb;
        Label lbl1, lbl2, LTlbl1, LTlbl2;
        ListBox lb1, LTlb;
        FlowLayoutPanel flow1, LTflow;

        DirectoryInfo PMPDrive = null;

        public struct UpdateLocation
        {
            public string SourceFile;
            public string DestinationFile;
        }

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
                tbLibraryLocation.Text = Properties.Settings.Default.MusicLibrary;
                tbDownscaledLocation.Text = Properties.Settings.Default.DownscaledLibrary;
                tbChiptunesLocation.Text = Properties.Settings.Default.ChiptunesLibrary;
                cbRemoveImproper.Checked = Properties.Settings.Default.RemoveImproper;
                cbAutoHandle.Checked = Properties.Settings.Default.AutoHandle;
                cbShowImproper.Checked = Properties.Settings.Default.ShowFiles;
                cbRemoveUnsupported.Checked = Properties.Settings.Default.RemoveUnsupported;
                cbRemoveUnnecessary.Checked = Properties.Settings.Default.RemoveUnnecessary;
                cbRemoveEmpty.Checked = Properties.Settings.Default.RemoveEmpty;
                cbPreventSynchingUpscaled.Checked = Properties.Settings.Default.PreventSynchingUpscaled;
                cbChiptunesLibrary.Checked = Properties.Settings.Default.EnableChiptunes;
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
                tbLibraryLocation.Text = drive + ":\\Users\\" + username + "\\Music\\";
                tbDownscaledLocation.Text = drive + ":\\Users\\" + username + "\\Music\\.downscaled\\";
                tbChiptunesLocation.Text = drive + ":\\Users\\" + username + "\\Music\\Chiptunes";
                cbRemoveImproper.Checked = Properties.Settings.Default.RemoveImproper;
                cbAutoHandle.Checked = Properties.Settings.Default.AutoHandle;
                cbShowImproper.Checked = Properties.Settings.Default.ShowFiles;
                cbRemoveUnsupported.Checked = Properties.Settings.Default.RemoveUnsupported;
                cbRemoveUnnecessary.Checked = Properties.Settings.Default.RemoveUnnecessary;
                cbRemoveEmpty.Checked = Properties.Settings.Default.RemoveEmpty;
                cbChiptunesLibrary.Checked = Properties.Settings.Default.EnableChiptunes;

                Properties.Settings.Default.Save();
            }

            //store the values
            MusicLibrary = tbLibraryLocation.Text;
            DownscaledLibrary = tbDownscaledLocation.Text;
            ChiptunesLibrary = tbChiptunesLocation.Text;
            AutoHandle = cbAutoHandle.Checked;
            RemoveImproper = cbRemoveImproper.Checked;
            ShowFiles = cbShowImproper.Checked;
            RemoveUnsupported = cbRemoveUnsupported.Checked;
            RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            RemoveEmpty = cbRemoveEmpty.Checked;
            PreventSynchingUpscaled = cbPreventSynchingUpscaled.Checked;
            EnableChiptunes = cbChiptunesLibrary.Checked;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (ScanBW.IsBusy != true)
            {
                btnScan.Text = "Scanning...";
                ScanBW.RunWorkerAsync();
            }
            else
            {
                btnScan.Text = "Not done yet!";
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

                                this.Invoke(new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //if it does not meet the minimum requirements, it needs to be downscaled
                                checkedFiles.Add(downscaledFile, false); //mark that it has been checked and is not proper
                            }
                            else checkedFiles.Add(downscaledFile, true); //mark that it has been checked and is proper
                        }
                        else
                            this.Invoke(new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //no downscaled file exists
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
                            if (ShowFiles) this.Invoke (new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //show the file in the list
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
                                    if (ShowFiles) this.Invoke (new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //show the file in the list
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

        private void btnSyncBoth_Click(object sender, EventArgs e)
        {
            if (!PMPSyncBW.IsBusy && !LaptopSyncBW.IsBusy)
            {
                btnSyncBoth.Text = "Running...";
                ShowSyncStatus(true, true, true);
                PMPSyncTBW.RunWorkerAsync();
                LaptopSyncTBW.RunWorkerAsync();
            }
        }

        private void btnSyncPMP_Click(object sender, EventArgs e)
        {
            if (!PMPSyncBW.IsBusy && !LaptopSyncBW.IsBusy)
            {
                PMPSyncBW.RunWorkerAsync();
                btnSyncPMP.Text = "Running...";
            }
        }

        private void btnSyncLaptop_Click(object sender, EventArgs e)
        {
            if (!PMPSyncBW.IsBusy && !LaptopSyncBW.IsBusy)
            {
                LaptopSyncBW.RunWorkerAsync();
                btnSyncLaptop.Text = "Running...";
            }
        }

        public void ShowSyncStatus(Boolean visible, Boolean pmp, Boolean laptop)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (visible)
                {
                    panel1.Visible = true;
                    tabControl.Visible = false;

                    if (!pmp && !laptop) return; //nothing to make

                    if (pmp)
                    {
                        //Status text
                        flow1 = new FlowLayoutPanel();
                        flow1.FlowDirection = FlowDirection.LeftToRight;
                        flow1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        flow1.AutoSize = true;
                        lbl1 = new Label();
                        lbl1.Text = "Synchronizing and Preparing PMP...";
                        lbl1.AutoSize = true;
                        flow1.Controls.Add(lbl1);
                        lbl2 = new Label();
                        lbl2.Text = "0%";
                        lbl2.AutoSize = true;
                        flow1.Controls.Add(lbl2);

                        //Progressbar
                        pb1 = new ProgressBar();
                        pb1.Maximum = 100000;
                        pb1.Value = 0;
                        pb1.Width = 765;
                        pb1.Height = 38;
                        pb1.Value = 0;

                        //Listbox
                        lb1 = new ListBox();
                        lb1.Width = 765;
                        if (pmp && laptop)
                            lb1.Height = 180; //when there is two
                        else
                            lb1.Height = 450; //when there is one

                        //Add the objects to the layout
                        flowLayoutPanel2.Controls.Add(flow1);
                        flowLayoutPanel2.Controls.Add(pb1);
                        flowLayoutPanel2.Controls.Add(lb1);
                    }

                    if (laptop)
                    {
                        //Status text
                        LTflow = new FlowLayoutPanel();
                        LTflow.FlowDirection = FlowDirection.LeftToRight;
                        LTflow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        LTflow.AutoSize = true;
                        LTlbl1 = new Label();
                        LTlbl1.Text = "Synchronizing and Preparing Laptop...";
                        LTlbl1.AutoSize = true;
                        LTflow.Controls.Add(LTlbl1);
                        LTlbl2 = new Label();
                        LTlbl2.Text = "0%";
                        LTlbl2.AutoSize = true;
                        LTflow.Controls.Add(LTlbl2);

                        //Progressbar
                        LTpb = new ProgressBar();
                        LTpb.Maximum = 100000;
                        LTpb.Value = 0;
                        LTpb.Width = 765;
                        LTpb.Height = 38;
                        LTpb.Value = 0;

                        //Listbox
                        LTlb = new ListBox();
                        LTlb.Width = 765;
                        if (pmp && laptop)
                            LTlb.Height = 180;
                        else
                            LTlb.Height = 450;

                        //Add the objects to the layout
                        flowLayoutPanel2.Controls.Add(LTflow);
                        flowLayoutPanel2.Controls.Add(LTpb);
                        flowLayoutPanel2.Controls.Add(LTlb);
                    }
                }
                else
                {
                    tabControl.Visible = true;
                    panel1.Visible = false;

                    if (!pmp && !laptop) return; //nothing to make

                    if (pmp)
                    {
                        //Clear out the status screen
                        flow1.Controls.Remove(lbl1);
                        flow1.Controls.Remove(lbl2);
                        flowLayoutPanel2.Controls.Remove(lb1);
                        flowLayoutPanel2.Controls.Remove(pb1);
                        flowLayoutPanel2.Controls.Remove(flow1);
                        lbl1 = null;
                        lbl2 = null;
                        flow1 = null;
                        pb1 = null;
                        lb1 = null;
                    }

                    if (laptop)
                    {
                        //Status text
                        LTflow.Controls.Remove(LTlbl1);
                        LTflow.Controls.Remove(LTlbl2);
                        flowLayoutPanel2.Controls.Remove(LTlb);
                        flowLayoutPanel2.Controls.Remove(LTpb);
                        flowLayoutPanel2.Controls.Remove(LTflow);
                        LTlbl1 = null;
                        LTlbl2 = null;
                        LTflow = null;
                        LTpb = null;
                        LTlb = null;
                    }
                }
            }));
        }

        public Boolean PrepareSyncPMP()
        {
            PMPDrive = null;
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

            //debug code
            //PMPFound = true;

            if (PMPFound)
            {
                DialogResult result = MessageBox.Show("PMP found on " + PMPDrive + "\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
                    aSoundPlayer.Play();  //Plays the sound in a new thread

                    return true;
                }
            }
            else MessageBox.Show("PMP could not be found! Make sure that it is connected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        public void SetupSyncPMP()
        {
            double CopySize = 0;
            Queue<UpdateLocation> UpdateFiles = new Queue<UpdateLocation>();

            CopySize = SyncPMP(MusicLibrary, PMPDrive + "Music", ref UpdateFiles);
            //CopySize = SyncPMP(MusicLibrary, "E:\\SynchronizationTests\\PMP", ref UpdateFiles); //debug code

            Queue<UpdateLocation> CopyFiles = new Queue<UpdateLocation>();

            //populate the listbox with files that need to be copied
            while (UpdateFiles.Count > 0)
            {
                UpdateLocation update = UpdateFiles.Dequeue();
                this.Invoke(new MethodInvoker(() => lb1.Items.Add(update.DestinationFile)));
                CopyFiles.Enqueue(update);
            }

            //set up the progress bar
            int progress = 0;
            double percent = 0;
            this.Invoke(new MethodInvoker(() =>
            {
                lbl1.Text = "Copying updated files to PMP...";
                lbl2.Text = "0%";
            }));

            //copy files to PMP here
            while (CopyFiles.Count > 0)
            {
                UpdateLocation update = CopyFiles.Dequeue();
                string source = update.SourceFile;
                string destination = update.DestinationFile;
                FileInfo info = new FileInfo(source);

                File.Copy(source, destination, true); //copy the file

                //calculate progress
                progress += (int)(((info.Length / CopySize) * 100000));
                percent = Math.Round((((double)progress) / 1000), 2);
                this.Invoke(new MethodInvoker(() =>
                {
                    pb1.Value = progress; //get the file's size
                    lbl2.Text = percent.ToString() + "%";
                    lb1.Items.Remove(destination);
                }));
            }

            this.Invoke(new MethodInvoker(() =>
            {
                pb1.Value = pb1.Maximum;
                lbl2.Text = "100%";
            }));

            MessageBox.Show("Done!");
        }

        public Boolean PrepareSyncLaptop()
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

                    return true;
                }
            }
            else MessageBox.Show("Laptop is not connected! Make sure that it is mounted properly!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        public void SetupSyncLaptop()
        {
            string LaptopName = "SUPERMOBILEROB";
            string LaptopUsername = "Cord";

            double CopySize = 0;
            Queue<UpdateLocation> UpdateFiles = new Queue<UpdateLocation>();

            CopySize = Sync(MusicLibrary, "\\\\" + LaptopName + "\\Users\\" + LaptopUsername + "\\Music", ref UpdateFiles);
            //CopySize = Sync(MusicLibrary, "E:\\SynchronizationTests\\Laptop", ref UpdateFiles); //debug code

            Queue<UpdateLocation> CopyFiles = new Queue<UpdateLocation>();

            //populate the listbox with files that need to be copied
            while (UpdateFiles.Count > 0)
            {
                UpdateLocation update = UpdateFiles.Dequeue();
                this.Invoke(new MethodInvoker(() => LTlb.Items.Add(update.DestinationFile)));
                CopyFiles.Enqueue(update);
            }

            //set up the progress bar
            int progress = 0;
            double percent = 0;
            this.Invoke(new MethodInvoker(() =>
            {
                LTlbl1.Text = "Copying updated files to Laptop...";
                LTlbl2.Text = "0%";
            }));

            //copy files to PMP here
            while (CopyFiles.Count > 0)
            {
                UpdateLocation update = CopyFiles.Dequeue();
                string source = update.SourceFile;
                string destination = update.DestinationFile;
                FileInfo info = new FileInfo(source);

                File.Copy(source, destination, true); //copy the file

                //calculate progress
                progress += (int)(((info.Length / CopySize) * 100000));
                percent = Math.Round((((double)progress) / 1000), 2);
                this.Invoke(new MethodInvoker(() =>
                {
                    LTpb.Value = progress; //get the file's size
                    LTlbl2.Text = percent.ToString() + "%";
                    LTlb.Items.Remove(destination);
                }));
            }

            this.Invoke(new MethodInvoker(() =>
            {
                LTpb.Value = LTpb.Maximum;
                LTlbl2.Text = "100%";
            }));

            MessageBox.Show("Done!");
        }

        public double SyncPMP(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        {
            double num = 0; //size of all files in this directory that will need to be copied
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
                        //file is newer, so add it to the queue of files that need to be copied
                        UpdateLocation update = new UpdateLocation();
                        update.SourceFile = correctFile;
                        update.DestinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                        UpdateFiles.Enqueue(update);
                        FileInfo info = new FileInfo(correctFile);
                        num += info.Length; //get the file's size
                    }
                }
                else
                {
                    //add it to the queue of files that need to be copied
                    UpdateLocation update = new UpdateLocation();
                    update.SourceFile = correctFile;
                    update.DestinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                    UpdateFiles.Enqueue(update);
                    FileInfo info = new FileInfo(correctFile);
                    num += info.Length; //get the file's size
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
                num += SyncPMP(dir, Path.Combine(destinationPath, dirInfo.Name), ref UpdateFiles);
            }

            return num;
        }

        public double Sync(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        {
            double num = 0; //size of all files in this directory that will need to be copied
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
                        //file is newer, so add it to the queue of files that need to be copied
                        UpdateLocation update = new UpdateLocation();
                        update.SourceFile = sourceFile;
                        update.DestinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                        UpdateFiles.Enqueue(update);
                        FileInfo info = new FileInfo(sourceFile);
                        num += info.Length; //get the file's size
                    }
                }
                else
                {
                    //add it to the queue of files that need to be copied
                    UpdateLocation update = new UpdateLocation();
                    update.SourceFile = sourceFile;
                    update.DestinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                    UpdateFiles.Enqueue(update);
                    FileInfo info = new FileInfo(sourceFile);
                    num += info.Length; //get the file's size
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
                num += Sync(dir, Path.Combine(destinationPath, dirInfo.Name), ref UpdateFiles);
            }
            return num;
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

        private void lbNotDownscaled_DoubleClick(object sender, EventArgs e)
        {
            int index = lbNotDownscaled.SelectedIndex; //determine what is selected
            if (index < 0) return;
            string file = lbNotDownscaled.Items[index].ToString(); //get the selected filename
            if (file == "No files need downscaling!") return; //don't try to open windows explorer
            if (!File.Exists(file)) //check to see if the file is still in the folder
            {
                lbNotDownscaled.Items.RemoveAt(index); //remove the value if it no longer exists
                if (lbNotDownscaled.Items.Count == 0)
                    lbNotDownscaled.Items.Add("No files need downscaling!"); //no more files need downscaling
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
                tbLibraryLocation.Text = open.SelectedPath + "\\";
                if (cbAutoHandle.Checked) tbDownscaledLocation.Text = tbLibraryLocation.Text + ".downscaled\\";
            }
            else if (TextBox == 2) tbDownscaledLocation.Text = open.SelectedPath + "\\";
            else if (TextBox == 3) tbChiptunesLocation.Text = open.SelectedPath + "\\";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoHandle.Checked)
            {
                lblDownscaledLocation.Enabled = false;
                tbDownscaledLocation.Enabled = false;
                btnDownscaledLocation.Enabled = false;
                cbRemoveImproper.Enabled = false;
                cbRemoveImproper.Checked = true;
                cbShowImproper.Enabled = false;
                cbShowImproper.Checked = false;
                cbRemoveUnsupported.Enabled = false;
                cbRemoveUnsupported.Checked = true;
                cbRemoveUnnecessary.Enabled = false;
                cbRemoveUnnecessary.Checked = true;
                cbRemoveEmpty.Enabled = false;
                cbRemoveEmpty.Checked = true;
                tbDownscaledLocation.Text = tbLibraryLocation.Text + ".downscaled\\";
            }
            else
            {
                lblDownscaledLocation.Enabled = true;
                tbDownscaledLocation.Enabled = true;
                btnDownscaledLocation.Enabled = true;
                cbRemoveImproper.Enabled = true;
                cbRemoveUnsupported.Enabled = true;
                cbRemoveUnnecessary.Enabled = true;
                cbRemoveEmpty.Enabled = true;
            }
            AutoHandle = cbAutoHandle.Checked;
            Properties.Settings.Default.AutoHandle = cbAutoHandle.Checked;
            Properties.Settings.Default.Save();
        }

        private void tbLibraryLocation_TextChanged(object sender, EventArgs e)
        {
            MusicLibrary = tbLibraryLocation.Text;
            Properties.Settings.Default.MusicLibrary = tbLibraryLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void tbDownscaledLocation_TextChanged(object sender, EventArgs e)
        {
            DownscaledLibrary = tbDownscaledLocation.Text;
            Properties.Settings.Default.DownscaledLibrary = tbDownscaledLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void tbChiptunesLocation_TextChanged(object sender, EventArgs e)
        {
            ChiptunesLibrary = tbChiptunesLocation.Text;
            Properties.Settings.Default.ChiptunesLibrary = tbChiptunesLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRemoveImproper.Checked)
            {
                cbShowImproper.Enabled = false;
                cbShowImproper.Checked = false;
            }
            else
            {
                cbShowImproper.Enabled = true;
            }
            RemoveImproper = cbRemoveImproper.Checked;
            Properties.Settings.Default.RemoveImproper = cbRemoveImproper.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ShowFiles = cbShowImproper.Checked;
            Properties.Settings.Default.ShowFiles = cbShowImproper.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnsupported = cbRemoveUnsupported.Checked;
            Properties.Settings.Default.RemoveUnsupported = cbRemoveUnsupported.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            Properties.Settings.Default.RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            RemoveEmpty = cbRemoveEmpty.Checked;
            Properties.Settings.Default.RemoveEmpty = cbRemoveEmpty.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (cbChiptunesLibrary.Checked)
            {
                lblChiptunesLocation.Enabled = true;
                tbChiptunesLocation.Enabled = true;
                btnChiptunesLocation.Enabled = true;
            }
            else
            {
                lblChiptunesLocation.Enabled = false;
                tbChiptunesLocation.Enabled = false;
                btnChiptunesLocation.Enabled = false;
            }
            EnableChiptunes = cbChiptunesLibrary.Checked;
            Properties.Settings.Default.EnableChiptunes = cbChiptunesLibrary.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbPreventSynchingUpscaled_CheckedChanged(object sender, EventArgs e)
        {
            PreventSynchingUpscaled = cbPreventSynchingUpscaled.Checked;
            Properties.Settings.Default.PreventSynchingUpscaled = cbPreventSynchingUpscaled.Checked;
            Properties.Settings.Default.Save();
        }

        private void PMPSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrepareSyncPMP())
            {
                ShowSyncStatus(true, true, false);
                SetupSyncPMP();
                ShowSyncStatus(false, true, false);
            }
            this.Invoke(new MethodInvoker(() =>
            {
                btnSyncPMP.Text = "Synchronize PMP";
                if (LaptopSyncBW.IsBusy == false)
                    btnSyncBoth.Text = "Synchronize Both";
            }));
        }

        private void ScanBW_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Media.SoundPlayer aSoundPlayer = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ScannerSweep);
            aSoundPlayer.Play();  //Plays the sound in a new thread

            this.Invoke (new MethodInvoker(() => lbNotDownscaled.Items.Clear())); //clear out previous items

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
            this.Invoke(new MethodInvoker(() =>
            {
                lblLibraryBytes.Text = "Library: " + BytesToSize(s1); //display the size
                lblFLACFiles.Text = "FLAC: " + Plural(flac, "file"); //display the number of flac songs
                lblMP3Files.Text = "MP3: " + Plural(mp3, "file"); //display the number of mp3 songs
                lblWMAFiles.Text = "WMA: " + Plural(wma, "file"); //display the number of wma songs
                lblM4AFiles.Text = "M4A: " + Plural(m4a, "file"); //display the number of m4a songs
                lblOGGFiles.Text = "OGG: " + Plural(ogg, "file"); //display the number of ogg songs
                lblWAVFiles.Text = "WAV: " + Plural(wav, "file"); //display the number of wav songs
                lblXMFiles.Text = "XM: " + Plural(xm, "file"); //display the number of xm songs
                lblMODFiles.Text = "MOD: " + Plural(mod, "file"); //display the number of mod songs
                lblNSFFiles.Text = "NSF: " + Plural(nsf, "file"); //display the number of nsf songs
                lblLibraryFiles.Text = "Library: " + Plural(audioTotal, "song");
                lblChiptunesFiles.Text = "Chiptunes: " + Plural(chiptunesTotal, "song");
                lblTotalFiles.Text = "Total (without downscaled): " + Plural(total, "song");
                lblDownscaledBytes.Text = "Downscaled: " + BytesToSize(dSize);
                lblDownscaledFiles.Text = "Downscaled: " + Plural(dFlac, "file");
                lblTotalBytes.Text = "Total: " + BytesToSize(allSize);
                if (lbNotDownscaled.Items.Count == 0)
                {
                    lbNotDownscaled.Items.Add("No files need downscaling!");
                }
                btnScan.Text = "Rescan Library";
                checkedFiles.Clear();
            }));
        }

        private void LaptopSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrepareSyncLaptop())
            {
                ShowSyncStatus(true, false, true);
                SetupSyncLaptop();
                ShowSyncStatus(false, false, true);
            }
            this.Invoke (new MethodInvoker(() => 
            {
                btnSyncLaptop.Text = "Synchronize Laptop";
                if (PMPSyncBW.IsBusy == false)
                    btnSyncBoth.Text = "Synchronize Both";
            }));
        }

        private void PMPSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrepareSyncPMP())
            {
                SetupSyncPMP();
            }
            this.Invoke(new MethodInvoker(() =>
            {
                if (LaptopSyncTBW.IsBusy == false)
                {
                    ShowSyncStatus(false, true, true);
                    btnSyncBoth.Text = "Synchronize Both";
                }
            }));
        }

        private void LaptopSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrepareSyncLaptop())
            {
                SetupSyncLaptop();
            }
            this.Invoke(new MethodInvoker(() =>
            {
                if (PMPSyncTBW.IsBusy == false)
                {
                    ShowSyncStatus(false, true, true);
                    btnSyncBoth.Text = "Synchronize Both";
                }
            }));
        }

        private void PreferencesTab_Click(object sender, EventArgs e)
        {

        }
    }
}
