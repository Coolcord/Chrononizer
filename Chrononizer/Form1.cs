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
        private string ChiptunesLibrary = " ";
        private string DownscaledLibrary = " ";
        private string LaptopHostname = " ";
        private string LaptopLocation = " ";
        private string LaptopUsername = " ";
        private string MusicLibrary = " ";
        private string PMPLocation = " ";
        private string PMPVolumeLabel = " ";
        
        Boolean AskSync = true;
        Boolean AutoExit = false;
        Boolean AutoExitOne = false;
        Boolean AutoHandle = true;
        Boolean CheckPMPSystem = true;
        Boolean EnableChiptunes = true;
        Boolean HideMediaArtLocal = true;
        Boolean LaptopSyncSuccess = true;
        Boolean OverrideLaptopPath = false;
        Boolean OverridePMPPath = false;
        Boolean PMPSyncSuccess = true;
        Boolean PreventSynchingUpscaled = true;
        Boolean RemoveEmpty = true;
        Boolean RemoveImproper = true;
        Boolean RemoveUnnecessary = true;
        Boolean RemoveUnsupported = true;
        Boolean ShowFiles = false;
        
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
                tbLaptopHostname.Text = Properties.Settings.Default.LaptopHostname;
                tbLaptopUsername.Text = Properties.Settings.Default.LaptopUsername;
                tbPMPVolumeLabel.Text = Properties.Settings.Default.PMPVolumeLabel;
                tbPMPLocation.Text = Properties.Settings.Default.PMPLocation;
                tbLaptopLocation.Text = Properties.Settings.Default.LaptopLocation;
                cbRemoveImproper.Checked = Properties.Settings.Default.RemoveImproper;
                cbAutoHandle.Checked = Properties.Settings.Default.AutoHandle;
                cbShowImproper.Checked = Properties.Settings.Default.ShowFiles;
                cbRemoveUnsupported.Checked = Properties.Settings.Default.RemoveUnsupported;
                cbRemoveUnnecessary.Checked = Properties.Settings.Default.RemoveUnnecessary;
                cbRemoveEmpty.Checked = Properties.Settings.Default.RemoveEmpty;
                cbPreventSynchingUpscaled.Checked = Properties.Settings.Default.PreventSynchingUpscaled;
                cbChiptunesLibrary.Checked = Properties.Settings.Default.EnableChiptunes;
                cbPreventSynchingUpscaled.Checked = Properties.Settings.Default.PreventSynchingUpscaled;
                cbAskSync.Checked = Properties.Settings.Default.AskSync;
                cbCheckPMPSystem.Checked = Properties.Settings.Default.CheckPMPSystem;
                cbOverridePMPPath.Checked = Properties.Settings.Default.OverridePMPPath;
                cbOverrideLaptopPath.Checked = Properties.Settings.Default.OverrideLaptopPath;
                cbHideMediaArtLocal.Checked = Properties.Settings.Default.HideMediaArtLocal;
                cbAutoExit.Checked = Properties.Settings.Default.AutoExit;
                cbAutoExitOne.Checked = Properties.Settings.Default.AutoExitOne;
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
                tbLibraryLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\";
                tbDownscaledLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\.downscaled\\";
                tbChiptunesLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\Chiptunes\\";
                tbLaptopHostname.Text = "Laptop";
                tbLaptopUsername.Text = Environment.UserName; //assume that the laptop's username is the same as the user running the app
                tbPMPVolumeLabel.Text = "X7 HDD";
                tbPMPLocation.Text = "*:\\Music\\";
                tbLaptopLocation.Text = "\\\\" + LaptopHostname + "\\Users\\" + LaptopUsername + "\\Music\\";
                cbRemoveImproper.Checked = Properties.Settings.Default.RemoveImproper;
                cbAutoHandle.Checked = Properties.Settings.Default.AutoHandle;
                cbShowImproper.Checked = Properties.Settings.Default.ShowFiles;
                cbRemoveUnsupported.Checked = Properties.Settings.Default.RemoveUnsupported;
                cbRemoveUnnecessary.Checked = Properties.Settings.Default.RemoveUnnecessary;
                cbRemoveEmpty.Checked = Properties.Settings.Default.RemoveEmpty;
                cbChiptunesLibrary.Checked = Properties.Settings.Default.EnableChiptunes;
                cbPreventSynchingUpscaled.Checked = Properties.Settings.Default.PreventSynchingUpscaled;
                cbAskSync.Checked = Properties.Settings.Default.AskSync;
                cbCheckPMPSystem.Checked = Properties.Settings.Default.CheckPMPSystem;
                cbOverridePMPPath.Checked = Properties.Settings.Default.OverridePMPPath;
                cbOverrideLaptopPath.Checked = Properties.Settings.Default.OverrideLaptopPath;
                cbHideMediaArtLocal.Checked = Properties.Settings.Default.HideMediaArtLocal;
                cbAutoExit.Checked = Properties.Settings.Default.AutoExit;
                cbAutoExitOne.Checked = Properties.Settings.Default.AutoExitOne;

                Properties.Settings.Default.Save();

                DialogResult result = MessageBox.Show("It is highly recommended that you check and configure your preferences before you synchronize any devices.\n\nWould you like to do so now?", "Welcome to Chrononizer!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) tabControl.SelectedTab = PreferencesTab; //show the preferences tab
            }

            //store the values
            MusicLibrary = tbLibraryLocation.Text;
            DownscaledLibrary = tbDownscaledLocation.Text;
            ChiptunesLibrary = tbChiptunesLocation.Text;
            PMPLocation = tbPMPLocation.Text;
            LaptopLocation = tbLaptopLocation.Text;
            AutoHandle = cbAutoHandle.Checked;
            RemoveImproper = cbRemoveImproper.Checked;
            ShowFiles = cbShowImproper.Checked;
            RemoveUnsupported = cbRemoveUnsupported.Checked;
            RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            RemoveEmpty = cbRemoveEmpty.Checked;
            PreventSynchingUpscaled = cbPreventSynchingUpscaled.Checked;
            EnableChiptunes = cbChiptunesLibrary.Checked;
            AskSync = cbAskSync.Checked;
            CheckPMPSystem = cbCheckPMPSystem.Checked;
            OverridePMPPath = cbOverrideLaptopPath.Checked;
            OverrideLaptopPath = cbOverrideLaptopPath.Checked;
            HideMediaArtLocal = cbHideMediaArtLocal.Checked;
            AutoExit = cbAutoExit.Checked;
            AutoExitOne = cbAutoExitOne.Checked;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!are_background_workers_running())
            {
                if (!do_libraries_exist()) return;
                btnScan.Text = "Scanning...";
                ScanBW.RunWorkerAsync();
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

        static string BytesToSize(double size)
        {
            int type = 0;
            string s = " ";
            while (size > 1024)
            {
                size /= 1024;
                type++;
                if (type >= 5)
                {
                    break; //don't calculate beyond PB
                }
            }
            size = Math.Round(size, 2); //round to two decimal places
            s = size.ToString(); //put the sizeber in a string
            switch (type)
            {
                case 0:
                    if (size == 1) s += " byte";
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

        private double GetDirectorySize(string root, double size, ref long flac, ref long mp3, ref long wma, ref long m4a, ref long ogg, ref long wav, ref long xm, ref long mod, ref long nsf)
        {
            if (!Directory.Exists(root)) return size; //path is invalid
            string[] files = Directory.GetFiles(root, "*.*"); //get array of all file names
            string[] folders = Directory.GetDirectories(root); //get array of all folder names for this directory

            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name); //read in the file
                String ext = Path.GetExtension(name); //get the file's extension
                size += info.Length; //get the length
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
                //hide all .mediaartlocal folders
                else if (HideMediaArtLocal && Path.GetFileName(name) == ".mediaartlocal") File.SetAttributes(Path.GetFullPath(name), FileAttributes.Hidden | FileAttributes.System);
                size = GetDirectorySize(name, size, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf); //recurse through the folders
            }
            return size;
        }

        private double GetDownscaledSize(string root, double size, ref long flac)
        {
            if (!Directory.Exists(root))
            {
                if (AutoHandle && Directory.Exists(MusicLibrary)) Directory.CreateDirectory(root); //create the directory if it doesn't exist
                return size;
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
                        size += info.Length; //add to the size
                        flac++;
                    }
                    else if (scanned && !valid)
                    {
                        if (RemoveImproper)
                            File.Delete(name); //downscaled flac not necessary
                        else
                        {
                            if (ShowFiles) this.Invoke (new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //show the file in the list
                            size += info.Length; //add to the size
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
                            size += info.Length; //add to the size
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
                                    size += info.Length; //add to the size
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
                                    size += info.Length; //add to the size
                                    flac++;
                                }
                            }
                            else
                            {
                                flacTag = new FlacTagger(defaultFile);
                                if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                                {
                                    size += info.Length; //add to the size
                                    flac++; //flac is ok to use
                                }
                                else
                                {
                                    if (RemoveUnnecessary)
                                        File.Delete(name); //flac did not need downscaling and is unnecessary
                                    else
                                    {
                                        size += info.Length; //add to the size
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
                size = GetDownscaledSize(name, size, ref flac); //recurse through the folders
            }

            if (RemoveEmpty && !Directory.EnumerateFileSystemEntries(root).Any() && Path.GetFullPath(root) != DownscaledLibrary)
            {
                Directory.Delete(root); //delete empty folders
                return size;
            }
            return size;
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
            if (!are_background_workers_running())
            {
                if (!do_libraries_exist()) return;
                this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences
                btnSyncBoth.Text = "Running...";
                ShowSyncStatus(true, true, true);
                PMPSyncTBW.RunWorkerAsync();
                LaptopSyncTBW.RunWorkerAsync();
            }
        }

        private void btnSyncPMP_Click(object sender, EventArgs e)
        {
            if (!are_background_workers_running())
            {
                if (!do_libraries_exist()) return;
                this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences
                PMPSyncBW.RunWorkerAsync();
                btnSyncPMP.Text = "Running...";
            }
        }

        private void btnSyncLaptop_Click(object sender, EventArgs e)
        {
            if (!are_background_workers_running())
            {
                if (!do_libraries_exist()) return;
                this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences
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
                        lbl1.Text = "Searching for PMP...";
                        lbl1.AutoSize = true;
                        flow1.Controls.Add(lbl1);
                        lbl2 = new Label();
                        lbl2.Text = " ";
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
                        LTlbl1.Text = "Searching for Laptop...";
                        LTlbl1.AutoSize = true;
                        LTflow.Controls.Add(LTlbl1);
                        LTlbl2 = new Label();
                        LTlbl2.Text = " ";
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

            if (!OverridePMPPath)
            {
                foreach (var drive in drives)
                {
                    try
                    {
                        if (drive.VolumeLabel == PMPVolumeLabel)
                        {
                            PMPDrive = drive.RootDirectory;
                            if (CheckPMPSystem)
                            {
                                if (!Directory.Exists(PMPDrive + "System\\") || !Directory.Exists(PMPDrive + "Music\\") || !File.Exists(PMPDrive + "DID.bin") || !File.Exists(PMPDrive + "nonce.bin"))
                                    PMPFound = false; //PMP is missing some core system files
                                else
                                {
                                    PMPFound = true; //PMP found
                                    PMPLocation = PMPDrive + "Music\\";
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(PMPDrive + "Music\\"))
                                    PMPFound = false; //PMP not found
                                else
                                {
                                    PMPFound = true; //PMP found
                                    PMPLocation = PMPDrive + "Music\\";
                                }
                            }
                            break;
                        }
                    }
                    catch (Exception) { }
                }
            }

            if (PMPFound || (OverridePMPPath && Directory.Exists(PMPLocation)))
            {
                DialogResult result = DialogResult.Yes;
                if (!OverridePMPPath && AskSync)
                    result = MessageBox.Show("PMP found on " + PMPDrive + "\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                else if (OverridePMPPath && AskSync)
                    result = MessageBox.Show("PMP found at the following location:\n" + PMPLocation + "\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
                    sound.Play();  //Plays the sound in a new thread

                    return true;
                }
                else //the user said no
                {
                    if (lbl1 != null)
                        this.Invoke(new MethodInvoker(() => lbl1.Text = "Error: Synchronization with PMP canceled! "));
                }
            }
            else //could not find device
            {
                if (lbl1 != null)
                    this.Invoke(new MethodInvoker(() => lbl1.Text = "Error: Synchronization with PMP failed! "));
                
                MessageBox.Show("PMP could not be found! Make sure that it is connected!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public void SetupSyncPMP()
        {
            double CopySize = 0;
            Queue<UpdateLocation> UpdateFiles = new Queue<UpdateLocation>();

            CopySize = SyncPMP(MusicLibrary, PMPLocation, ref UpdateFiles);

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
                lbl1.Text = "Synchronization with PMP complete! ";
            }));
        }

        public Boolean PrepareSyncLaptop()
        {
            if (Directory.Exists(LaptopLocation))
            {
                DialogResult result = DialogResult.Yes;
                if (!OverrideLaptopPath && AskSync)
                    result = MessageBox.Show(LaptopHostname + " logged in as " + LaptopUsername + " is mounted.\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                else if (OverrideLaptopPath && AskSync)
                    result = MessageBox.Show("Laptop found at the following location:\n" + LaptopLocation + "\nWould you like to sync to this device?", "Device Found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ChronoBoost);
                    sound.Play();  //Plays the sound in a new thread

                    return true;
                }
                else //the user said no
                {
                    if (LTlbl1 != null)
                        this.Invoke(new MethodInvoker(() => LTlbl1.Text = "Error: Synchronization with Laptop canceled! "));
                }
            }
            else //could not find device
            {
                if (LTlbl1 != null)
                    this.Invoke(new MethodInvoker(() => LTlbl1.Text = "Error: Synchronization with Laptop failed! "));
                MessageBox.Show("Laptop is not connected! Make sure that it is mounted properly!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public void SetupSyncLaptop()
        {
            double CopySize = 0;
            Queue<UpdateLocation> UpdateFiles = new Queue<UpdateLocation>();

            CopySize = Sync(MusicLibrary, LaptopLocation, ref UpdateFiles);

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
                LTlbl1.Text = "Synchronization with Laptop complete! ";
            }));
        }

        public double SyncPMP(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        {
            double size = 0; //size of all files in this directory that will need to be copied
            bool dirExisted = DirExists(destinationPath);

            //get the source files
            string[] sourceFiles = Directory.GetFiles(sourcePath);
            foreach (string sourceFile in sourceFiles)
            {
                string correctFile = sourceFile;

                if (Path.GetExtension(sourceFile) == ".flac")
                {
                    Luminescence.Xiph.FlacTagger flacTag = new FlacTagger(correctFile); //get the flac's tag
                    if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                    {
                        if (File.Exists(DownscaledLibrary + sourceFile.Substring(MusicLibrary.Length)))
                            correctFile = DownscaledLibrary + sourceFile.Substring(MusicLibrary.Length); //redirect to downscaled file
                        else if (PreventSynchingUpscaled)
                            continue;
                    }
                }

                FileInfo sourceInfo = new FileInfo(correctFile);
                string destinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                if (!dirExisted && File.Exists(destinationFile))
                {
                    FileInfo destinationInfo = new FileInfo(destinationFile);
                    if (sourceInfo.LastWriteTime > destinationInfo.LastWriteTime)
                    {
                        //file is newer, so add it to the queue of files that need to be copied
                        UpdateLocation update = new UpdateLocation();
                        update.SourceFile = correctFile;
                        update.DestinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                        UpdateFiles.Enqueue(update);
                        FileInfo info = new FileInfo(correctFile);
                        size += info.Length; //get the file's size
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
                    size += info.Length; //get the file's size
                }
            }

            DeleteOldDestinationFiles(sourceFiles, destinationPath);

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
                size += SyncPMP(dir, Path.Combine(destinationPath, dirInfo.Name), ref UpdateFiles);
            }

            return size;
        }

        public double Sync(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        {
            double size = 0; //size of all files in this directory that will need to be copied
            bool dirExisted = DirExists(destinationPath);

            //get the source files
            string[] sourceFiles = Directory.GetFiles(sourcePath);
            foreach (string sourceFile in sourceFiles)
            {
                FileInfo sourceInfo = new FileInfo(sourceFile);
                string destinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                if (!dirExisted && File.Exists(destinationFile))
                {
                    FileInfo destinationInfo = new FileInfo(destinationFile);
                    if (sourceInfo.LastWriteTime > destinationInfo.LastWriteTime)
                    {
                        //file is newer, so add it to the queue of files that need to be copied
                        UpdateLocation update = new UpdateLocation();
                        update.SourceFile = sourceFile;
                        update.DestinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                        UpdateFiles.Enqueue(update);
                        FileInfo info = new FileInfo(sourceFile);
                        size += info.Length; //get the file's size
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
                    size += info.Length; //get the file's size
                }
            }

            DeleteOldDestinationFiles(sourceFiles, destinationPath);

            //now process the directories if exist
            string[] dirs = Directory.GetDirectories(sourcePath);
            DeleteOldDestinationDirectories(dirs, destinationPath);
            foreach (string dir in dirs)
            {
                if (dir == DownscaledLibrary.Substring(0, DownscaledLibrary.Length-1))
                    continue;
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                //recursive do the directories
                size += Sync(dir, Path.Combine(destinationPath, dirInfo.Name), ref UpdateFiles);
            }
            return size;
        }

        private bool DirExists(string path)
        {
            Boolean exists = false;
            //create destination directory if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                exists = true;
            }
            else
            {
                exists = false;
            }
            //hide all .mediaartlocal folders
            if (HideMediaArtLocal && Path.GetFileName(path) == ".mediaartlocal") File.SetAttributes(Path.GetFullPath(path), FileAttributes.Hidden | FileAttributes.System);
            return exists;
        }

        private static void DeleteOldDestinationFiles(string[] sourceFiles, string destinationPath)
        {
            //get the destination files
            string[] destinationFiles = Directory.GetFiles(destinationPath);

            foreach (string destinationFile in destinationFiles)
            {
                FileInfo f = new FileInfo(destinationFile);
                string[] found = Array.FindAll(sourceFiles, str => GetFileName(str).Equals(f.Name));

                if (found.Length == 0)
                {
                    //delete file if not found in destination
                    File.Delete(destinationFile);
                }
            }
        }

        private static void DeleteOldDestinationDirectories(string[] sourceDirectories, string destinationPath)
        {
            //get the destination files
            string[] destinationDirectories = Directory.GetDirectories(destinationPath);

            foreach (string destinationDirectory in destinationDirectories)
            {
                FileInfo f = new FileInfo(destinationDirectory);
                string[] found = Array.FindAll(sourceDirectories, str => GetDirectoryName(str).Equals(f.Name));

                if (found.Length == 0)
                {
                    //delete file if not found in destination
                    Directory.Delete(destinationDirectory, true);
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

        private void btnLibraryLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(1);
        }

        private void btnDownscaledLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(2);
        }

        private void btnChiptunesLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(3);
        }

        private void tbLibraryLocation_Click(object sender, EventArgs e)
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

        private void tbPMPLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(4);
        }

        private void btnPMPLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(4);
        }

        private void tbLaptopLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(5);
        }

        private void btnLaptopLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(5);
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
            else if (TextBox == 4) tbPMPLocation.Text = open.SelectedPath + "\\";
            else if (TextBox == 5) tbLaptopLocation.Text = open.SelectedPath + "\\";
        }

        private void cbAutoHandle_CheckedChanged(object sender, EventArgs e)
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

        private void cbRemoveImproper_CheckedChanged(object sender, EventArgs e)
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

        private void cbShowImproper_CheckedChanged(object sender, EventArgs e)
        {
            ShowFiles = cbShowImproper.Checked;
            Properties.Settings.Default.ShowFiles = cbShowImproper.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRemoveUnsupported_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnsupported = cbRemoveUnsupported.Checked;
            Properties.Settings.Default.RemoveUnsupported = cbRemoveUnsupported.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRemoveUnnecessary_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            Properties.Settings.Default.RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRemoveEmpty_CheckedChanged(object sender, EventArgs e)
        {
            RemoveEmpty = cbRemoveEmpty.Checked;
            Properties.Settings.Default.RemoveEmpty = cbRemoveEmpty.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbChiptunesLibrary_CheckedChanged(object sender, EventArgs e)
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

        private void cbAskSync_CheckedChanged(object sender, EventArgs e)
        {
            AskSync = cbAskSync.Checked;
            Properties.Settings.Default.AskSync = cbAskSync.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbCheckPMPSystem_CheckedChanged(object sender, EventArgs e)
        {
            CheckPMPSystem = cbCheckPMPSystem.Checked;
            Properties.Settings.Default.CheckPMPSystem = cbCheckPMPSystem.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbHideMediaArtLocal_CheckedChanged(object sender, EventArgs e)
        {
            HideMediaArtLocal = cbHideMediaArtLocal.Checked;
            Properties.Settings.Default.HideMediaArtLocal = cbHideMediaArtLocal.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbAutoExit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoExit.Checked)
                cbAutoExitOne.Enabled = true;
            else
            {
                cbAutoExitOne.Checked = false;
                cbAutoExitOne.Enabled = false;
            }
            AutoExit = cbAutoExit.Checked;
            Properties.Settings.Default.AutoExit = cbAutoExit.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbAutoExitOne_CheckedChanged(object sender, EventArgs e)
        {
            AutoExitOne = cbAutoExitOne.Checked;
            Properties.Settings.Default.AutoExitOne = cbAutoExitOne.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbOverridePMPPath_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOverridePMPPath.Checked)
            {
                lblPMPLocation.Enabled = true;
                tbPMPLocation.Enabled = true;
                tbPMPLocation.Text = " ";
                btnPMPLocation.Enabled = true;
                cbCheckPMPSystem.Enabled = false;
                lblPMPVolumeLabel.Enabled = false;
                tbPMPVolumeLabel.Enabled = false;
            }
            else
            {
                lblPMPLocation.Enabled = false;
                tbPMPLocation.Enabled = false;
                btnPMPLocation.Enabled = false;
                tbPMPLocation.Text = "*:\\Music\\";
                cbCheckPMPSystem.Enabled = true;
                lblPMPVolumeLabel.Enabled = true;
                tbPMPVolumeLabel.Enabled = true;
            }
            OverridePMPPath = cbOverridePMPPath.Checked;
            Properties.Settings.Default.OverridePMPPath = cbOverridePMPPath.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbOverrideLaptopPath_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOverrideLaptopPath.Checked)
            {
                lblLaptopLocation.Enabled = true;
                tbLaptopLocation.Enabled = true;
                btnLaptopLocation.Enabled = true;
                lblLaptopHostname.Enabled = false;
                tbLaptopHostname.Enabled = false;
                lblLaptopUsername.Enabled = false;
                tbLaptopUsername.Enabled = false;
            }
            else
            {
                lblLaptopLocation.Enabled = false;
                tbLaptopLocation.Enabled = false;
                btnLaptopLocation.Enabled = false;
                tbLaptopLocation.Text = "\\\\" + LaptopHostname + "\\Users\\" + LaptopUsername + "\\Music\\";
                lblLaptopHostname.Enabled = true;
                tbLaptopHostname.Enabled = true;
                lblLaptopUsername.Enabled = true;
                tbLaptopUsername.Enabled = true;
            }
            OverrideLaptopPath = cbOverrideLaptopPath.Checked;
            Properties.Settings.Default.OverrideLaptopPath = cbOverrideLaptopPath.Checked;
            Properties.Settings.Default.Save();
        }

        private void tbLaptopHostname_TextChanged(object sender, EventArgs e)
        {
            LaptopHostname = tbLaptopHostname.Text;
            if (!OverrideLaptopPath)
                tbLaptopLocation.Text = "\\\\" + LaptopHostname + "\\Users\\" + LaptopUsername + "\\Music\\";
            Properties.Settings.Default.LaptopHostname = tbLaptopHostname.Text;
            Properties.Settings.Default.Save();
        }

        private void tbLaptopUsername_TextChanged(object sender, EventArgs e)
        {
            LaptopUsername = tbLaptopUsername.Text;
            if (!OverrideLaptopPath)
                tbLaptopLocation.Text = "\\\\" + LaptopHostname + "\\Users\\" + LaptopUsername + "\\Music\\";
            Properties.Settings.Default.LaptopUsername = tbLaptopUsername.Text;
            Properties.Settings.Default.Save();
        }

        private void tbPMPVolumeLabel_TextChanged(object sender, EventArgs e)
        {
            PMPVolumeLabel = tbPMPVolumeLabel.Text;
            Properties.Settings.Default.PMPVolumeLabel = tbPMPVolumeLabel.Text;
            Properties.Settings.Default.Save();
        }

        private void tbPMPLocation_TextChanged(object sender, EventArgs e)
        {
            PMPLocation = tbPMPLocation.Text;
            Properties.Settings.Default.PMPLocation = tbPMPLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void tbLaptopLocation_TextChanged(object sender, EventArgs e)
        {
            LaptopLocation = tbLaptopLocation.Text;
            Properties.Settings.Default.LaptopLocation = tbLaptopLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void PMPSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrepareSyncPMP())
            {
                ShowSyncStatus(true, true, false);
                this.Invoke(new MethodInvoker(() => lbl1.Text = "Scanning and preparing PMP... this may take some time..."));
                SetupSyncPMP();
                if (AutoExit)
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                    sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                    System.Environment.Exit(0); //exit the program if auto exit is enabled
                }
                else
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                    sound.Play();  //Plays the sound in a new thread

                    MessageBox.Show("Synchronization Complete!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Information); //show the success window if at least one operation completed
                }
                ShowSyncStatus(false, true, false);
            }
            this.Invoke(new MethodInvoker(() =>
            {
                btnSyncPMP.Text = "Synchronize PMP";
                PreferencesTab.Enabled = true; //allow changes to preferences
            }));
        }

        private void ScanBW_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences

            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ScannerSweep);
            sound.Play();  //Plays the sound in a new thread

            this.Invoke (new MethodInvoker(() => lbNotDownscaled.Items.Clear())); //clear out previous items

            long flac, mp3, wma, m4a, ogg, wav, xm, mod, nsf, audioTotal, chiptunesTotal, total, dFlac;
            flac = mp3 = wma = m4a = ogg = wav = xm = mod = nsf = audioTotal = chiptunesTotal = total = dFlac = 0;
            double dSize, allSize, lSize;
            dSize = allSize = lSize = 0;

            lSize = GetDirectorySize(MusicLibrary, 0, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf);
            dSize = GetDownscaledSize(DownscaledLibrary, dSize, ref dFlac); //recurse through the downscaled files
            if (AutoHandle && Directory.Exists(MusicLibrary) && Directory.Exists(DownscaledLibrary)) //set the file attributes if auto handling is on
                File.SetAttributes(DownscaledLibrary, FileAttributes.Hidden | FileAttributes.System);
            audioTotal = flac + mp3 + wma + m4a + ogg + wav;
            chiptunesTotal = xm + mod + nsf;
            total = audioTotal + chiptunesTotal;
            allSize = lSize + dSize;
            this.Invoke(new MethodInvoker(() =>
            {
                lblLibraryBytes.Text = "Library: " + BytesToSize(lSize); //display the size
                lblFLACFiles.Text = "FLAC: " + Plural(flac, "file"); //display the sizeber of flac songs
                lblMP3Files.Text = "MP3: " + Plural(mp3, "file"); //display the sizeber of mp3 songs
                lblWMAFiles.Text = "WMA: " + Plural(wma, "file"); //display the sizeber of wma songs
                lblM4AFiles.Text = "M4A: " + Plural(m4a, "file"); //display the sizeber of m4a songs
                lblOGGFiles.Text = "OGG: " + Plural(ogg, "file"); //display the sizeber of ogg songs
                lblWAVFiles.Text = "WAV: " + Plural(wav, "file"); //display the sizeber of wav songs
                lblXMFiles.Text = "XM: " + Plural(xm, "file"); //display the sizeber of xm songs
                lblMODFiles.Text = "MOD: " + Plural(mod, "file"); //display the sizeber of mod songs
                lblNSFFiles.Text = "NSF: " + Plural(nsf, "file"); //display the sizeber of nsf songs
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

            this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = true)); //allow changes to preferences
        }

        private void LaptopSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PrepareSyncLaptop())
            {
                ShowSyncStatus(true, false, true);
                this.Invoke(new MethodInvoker(() => LTlbl1.Text = "Scanning and preparing Laptop... this may take some time..."));
                SetupSyncLaptop();
                if (AutoExit)
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                    sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                    System.Environment.Exit(0); //exit the program if auto exit is enabled
                }
                else
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                    sound.Play();  //Plays the sound in a new thread

                    MessageBox.Show("Synchronization Complete!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Information); //show the success window if at least one operation completed
                }
                ShowSyncStatus(false, false, true);
            }
            this.Invoke (new MethodInvoker(() => 
            {
                btnSyncLaptop.Text = "Synchronize Laptop";
                PreferencesTab.Enabled = true; //allow changes to preferences
            }));
        }

        private void PMPSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        {
            PMPSyncSuccess = PrepareSyncPMP();
            if (PMPSyncSuccess)
            {
                this.Invoke(new MethodInvoker(() => lbl1.Text = "Scanning and preparing PMP... this may take some time..."));
                SetupSyncPMP();
            }
            this.Invoke(new MethodInvoker(() =>
            {
                if (LaptopSyncTBW.IsBusy == false)
                {
                    if (AutoExit && PMPSyncSuccess && LaptopSyncSuccess)
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                        sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                        System.Environment.Exit(0); //exit the program if auto exit is enabled
                    }
                    if (AutoExitOne && (PMPSyncSuccess || LaptopSyncSuccess)) //auto exit if at least one succeeds
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                        sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                        System.Environment.Exit(0); //exit the program if auto exit is enabled
                    }
                    if (PMPSyncSuccess || LaptopSyncSuccess)
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                        sound.Play();  //Plays the sound in a new thread

                        MessageBox.Show("Synchronization Complete!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Information); //show the success window if at least one operation completed
                    }
                    else
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.OhNo);
                        sound.Play();  //Plays the sound in a new thread

                        MessageBox.Show("Synchronization Failed!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    ShowSyncStatus(false, true, true);
                    btnSyncBoth.Text = "Synchronize Both";
                    PreferencesTab.Enabled = true; //allow changes to preferences
                }
            }));
        }

        private void LaptopSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        {
            LaptopSyncSuccess = PrepareSyncLaptop();
            if (LaptopSyncSuccess)
            {
                this.Invoke(new MethodInvoker(() => LTlbl1.Text = "Scanning and preparing Laptop... this may take some time..."));
                SetupSyncLaptop();
            }
            this.Invoke(new MethodInvoker(() =>
            {
                if (PMPSyncTBW.IsBusy == false)
                {
                    if (AutoExit && PMPSyncSuccess && LaptopSyncSuccess)
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                        sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                        System.Environment.Exit(0); //exit the program if auto exit is enabled
                    }
                    if (AutoExitOne && (PMPSyncSuccess || LaptopSyncSuccess)) //auto exit if at least one succeeds
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                        sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                        System.Environment.Exit(0); //exit the program if auto exit is enabled
                    }
                    if (PMPSyncSuccess || LaptopSyncSuccess)
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                        sound.Play();  //Plays the sound in a new thread

                        MessageBox.Show("Synchronization Complete!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Information); //show the success window if at least one operation completed
                    }
                    else
                    {
                        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.OhNo);
                        sound.Play();  //Plays the sound in a new thread

                        MessageBox.Show("Synchronization Failed!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    ShowSyncStatus(false, true, true);
                    btnSyncBoth.Text = "Synchronize Both";
                    PreferencesTab.Enabled = true; //allow changes to preferences
                }
            }));
        }

        private Boolean are_background_workers_running()
        {
            if (PMPSyncBW.IsBusy || PMPSyncTBW.IsBusy || LaptopSyncBW.IsBusy || LaptopSyncTBW.IsBusy || ScanBW.IsBusy)
                return true;
            else
                return false;
        }

        private Boolean do_libraries_exist()
        {
            if (!Directory.Exists(MusicLibrary))
            {
                System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.OhNo);
                sound.Play();  //Plays the sound in a new thread

                MessageBox.Show("The music library could not be found at the following specified directory: " + MusicLibrary, "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!Directory.Exists(DownscaledLibrary))
            {
                System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.OhNo);
                sound.Play();  //Plays the sound in a new thread

                MessageBox.Show("The downscaled library could not be found at the following specified directory: " + DownscaledLibrary, "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!Directory.Exists(ChiptunesLibrary))
            {
                System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.OhNo);
                sound.Play();  //Plays the sound in a new thread

                MessageBox.Show("The chiptunes library could not be found at the following specified directory: " + ChiptunesLibrary, "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
