/*
 * -======================- License and Distribution -======================-
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

/* 
 * -==========================- About Chrononizer -==========================-
 *  
 *  Chrononizer is a simple music synchronization application that I designed 
 *  for myself to make my daily updates to my music library much easier. The
 *  goal of this project was to make it possible to quickly synchronize my
 *  music library between multiple devices in as few clicks as possible, while
 *  following unique rules for each device.
 *  
 *  The name Chrononizer is a mash-up of the words "chrono-boost" and
 *  "synchronizer." Considering that the goal of this application was to
 *  quickly synchronize multiple devices, it seemed appropriate.
 *  
 *  I was inspired to write Chrononizer after I purchased a Cowon X7 portable
 *  media player. I wanted a quick application that could synchronize my
 *  library to the device, however, I could not find anything that accomplished
 *  what I wanted up to my liking. In order for such an application to meet my
 *  needs, it had to synchronize my music library from my desktop computer to
 *  both my laptop and PMP, following unique rules for each device.
 *  
 *  For the PMP, no chiptune files could be sent to the device. Also, any FLAC
 *  audio files that are over 24-bit in bit-depth or 48khz in bit-rate must be
 *  downscaled accordingly. The laptop could be sent chiptunes and FLAC audio
 *  files with high bit-depths and bit-rates, but it has no use for downscaled
 *  files. These are the rules that Chrononizer is programmed to follow.
 *  
 *  To determine if any files in the music library need to be downscaled, the files
 *  of the library need to be analyzed. This is where the "Scan Library" function
 *  comes into play. With this feature, statistics about the user's library can be
 *  determined. FLAC files will be analyzed during this process to see if they are
 *  higher than 24-bit in bit-depth and higher than 48khz in bit-rate. If they are,
 *  they will be added to the list of files that need downscaling. Using this list,
 *  users can quickly find these files so that they can make a downscaled version
 *  of them if they desire.
 *  
 *  In the program's current state, Chrononizer meets my personal needs. However,
 *  it is likely that other users may prefer to use different synchronizing rules
 *  or may wish to add new features. This is why I intend to keep Chrononizer open
 *  source. I hope that other users out there may be able to use this code as a base
 *  to create a music synchronizer for their own personal use that meets their needs.
 * 
 *  If you wish to contact me about the application, or anything of the like,
 *  feel free to send me an email at coolcord24@gmail.com
 */

/* 
 * -================================- Credits -================================-
 *  
 *  The following files included with Chrononizer (though some modified)
 *  were not originally created by me. Credit shall be given where it is due!
 * 
 *  AaawYeah.wav:
 *      Original sound effect from My Little Pony: Friendship is Magic © Hasbro
 *      
 *  ChronoBoost.wav:
 *      Original sound effect from StarCraft II © Blizzard
 *      
 *  chrononizer.ico:
 *      Icon ripped from StarCraft II © Blizzard
 *      
 *  Luminescence.Xiph.dll:
 *      Library by Cyber_Sinh released under LGPL license
 * 
 *  OhNo.wav:
 *      Sound effect provided with IMGBurn
 *      
 *  ScannerSweep.wav:
 *      Original sound effect from StarCraft II © Blizzard
 */

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
        DirectoryInfo PMPDrive = null;

        FlowLayoutPanel PMPflow, LTflow;
        Label lbl1, lbl2, LTlbl1, LTlbl2;
        ListBox lb1, LTlb;
        ProgressBar PMPpb, LTpb;

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
                //Load saved settings. Order is important!
                cbOverridePMPPath.Checked = Properties.Settings.Default.OverridePMPPath;
                cbOverrideLaptopPath.Checked = Properties.Settings.Default.OverrideLaptopPath;
                tbChiptunesLocation.Text = Properties.Settings.Default.ChiptunesLibrary;
                tbLibraryLocation.Text = Properties.Settings.Default.MusicLibrary;
                tbDownscaledLocation.Text = Properties.Settings.Default.DownscaledLibrary;
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

                //Load default settings. Order is important!
                cbOverridePMPPath.Checked = Properties.Settings.Default.OverridePMPPath;
                cbOverrideLaptopPath.Checked = Properties.Settings.Default.OverrideLaptopPath;
                tbChiptunesLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\Chiptunes\\";
                tbLibraryLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\";
                tbDownscaledLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\.downscaled\\";
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
                cbHideMediaArtLocal.Checked = Properties.Settings.Default.HideMediaArtLocal;
                cbAutoExit.Checked = Properties.Settings.Default.AutoExit;
                cbAutoExitOne.Checked = Properties.Settings.Default.AutoExitOne;

                Properties.Settings.Default.Save();

                DialogResult result = MessageBox.Show("It is highly recommended that you check and configure your preferences before you synchronize any devices.\n\nWould you like to do so now?", "Welcome to Chrononizer!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) tabControl.SelectedTab = PreferencesTab; //show the preferences tab
            }

            //store the values
            ChiptunesLibrary = tbChiptunesLocation.Text;
            MusicLibrary = tbLibraryLocation.Text;
            DownscaledLibrary = tbDownscaledLocation.Text;
            PMPLocation = tbPMPLocation.Text;
            PMPVolumeLabel = tbPMPVolumeLabel.Text;
            LaptopLocation = tbLaptopLocation.Text;
            LaptopHostname = tbLaptopHostname.Text;
            LaptopUsername = tbLaptopUsername.Text;
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
            OverridePMPPath = cbOverridePMPPath.Checked;
            OverrideLaptopPath = cbOverrideLaptopPath.Checked;
            HideMediaArtLocal = cbHideMediaArtLocal.Checked;
            AutoExit = cbAutoExit.Checked;
            AutoExitOne = cbAutoExitOne.Checked;
        }





        //===========================================================================
        //
        // Interface Event Handlers
        //
        //===========================================================================

        private void btnChiptunesLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(3);
        }

        private void btnDownscaledLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(2);
        }

        private void btnLaptopLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(5);
        }

        private void btnLibraryLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(1);
        }

        private void btnPMPLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(4);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!AreBackgroundWorkersRunning())
            {
                if (!DoLibrariesExist()) return;
                btnScan.Text = "Scanning...";
                ScanBW.RunWorkerAsync();
            }
        }

        private void btnSyncBoth_Click(object sender, EventArgs e)
        {
            if (!AreBackgroundWorkersRunning())
            {
                if (!DoLibrariesExist()) return;
                this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences
                btnSyncBoth.Text = "Running...";
                ShowSyncStatus(true, true, true);
                PMPSyncTBW.RunWorkerAsync();
                LaptopSyncTBW.RunWorkerAsync();
            }
        }

        private void btnSyncLaptop_Click(object sender, EventArgs e)
        {
            if (!AreBackgroundWorkersRunning())
            {
                if (!DoLibrariesExist()) return;
                this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences
                LaptopSyncBW.RunWorkerAsync();
                btnSyncLaptop.Text = "Running...";
            }
        }

        private void btnSyncPMP_Click(object sender, EventArgs e)
        {
            if (!AreBackgroundWorkersRunning())
            {
                if (!DoLibrariesExist()) return;
                this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences
                PMPSyncBW.RunWorkerAsync();
                btnSyncPMP.Text = "Running...";
            }
        }

        private void cbAskSync_CheckedChanged(object sender, EventArgs e)
        {
            AskSync = cbAskSync.Checked;
            Properties.Settings.Default.AskSync = cbAskSync.Checked;
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

        private void cbCheckPMPSystem_CheckedChanged(object sender, EventArgs e)
        {
            CheckPMPSystem = cbCheckPMPSystem.Checked;
            Properties.Settings.Default.CheckPMPSystem = cbCheckPMPSystem.Checked;
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

        private void cbHideMediaArtLocal_CheckedChanged(object sender, EventArgs e)
        {
            HideMediaArtLocal = cbHideMediaArtLocal.Checked;
            Properties.Settings.Default.HideMediaArtLocal = cbHideMediaArtLocal.Checked;
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

        private void cbOverridePMPPath_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOverridePMPPath.Checked)
            {
                lblPMPLocation.Enabled = true;
                tbPMPLocation.Enabled = true;
                if (tbPMPLocation.Text == "*:\\Music\\")
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

        private void cbPreventSynchingUpscaled_CheckedChanged(object sender, EventArgs e)
        {
            PreventSynchingUpscaled = cbPreventSynchingUpscaled.Checked;
            Properties.Settings.Default.PreventSynchingUpscaled = cbPreventSynchingUpscaled.Checked;
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
                cbShowImproper.Enabled = true;
            RemoveImproper = cbRemoveImproper.Checked;
            Properties.Settings.Default.RemoveImproper = cbRemoveImproper.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRemoveEmpty_CheckedChanged(object sender, EventArgs e)
        {
            RemoveEmpty = cbRemoveEmpty.Checked;
            Properties.Settings.Default.RemoveEmpty = cbRemoveEmpty.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRemoveUnnecessary_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            Properties.Settings.Default.RemoveUnnecessary = cbRemoveUnnecessary.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRemoveUnsupported_CheckedChanged(object sender, EventArgs e)
        {
            RemoveUnsupported = cbRemoveUnsupported.Checked;
            Properties.Settings.Default.RemoveUnsupported = cbRemoveUnsupported.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbShowImproper_CheckedChanged(object sender, EventArgs e)
        {
            ShowFiles = cbShowImproper.Checked;
            Properties.Settings.Default.ShowFiles = cbShowImproper.Checked;
            Properties.Settings.Default.Save();
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

        private void tbChiptunesLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(3);
        }

        private void tbChiptunesLocation_TextChanged(object sender, EventArgs e)
        {
            ChiptunesLibrary = tbChiptunesLocation.Text;
            Properties.Settings.Default.ChiptunesLibrary = tbChiptunesLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void tbDownscaledLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(2);
        }

        private void tbDownscaledLocation_TextChanged(object sender, EventArgs e)
        {
            DownscaledLibrary = tbDownscaledLocation.Text;
            Properties.Settings.Default.DownscaledLibrary = tbDownscaledLocation.Text;
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

        private void tbLaptopLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(5);
        }
        
        private void tbLaptopLocation_TextChanged(object sender, EventArgs e)
        {
            LaptopLocation = tbLaptopLocation.Text;
            Properties.Settings.Default.LaptopLocation = tbLaptopLocation.Text;
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

        private void tbLibraryLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(1);
        }

        private void tbLibraryLocation_TextChanged(object sender, EventArgs e)
        {
            MusicLibrary = tbLibraryLocation.Text;
            if (!ChiptunesLibrary.Contains(MusicLibrary))
                tbChiptunesLocation.Text = MusicLibrary + "Chiptunes\\";
            Properties.Settings.Default.MusicLibrary = tbLibraryLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void tbPMPLocation_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(4);
        }

        private void tbPMPLocation_TextChanged(object sender, EventArgs e)
        {
            PMPLocation = tbPMPLocation.Text;
            Properties.Settings.Default.PMPLocation = tbPMPLocation.Text;
            Properties.Settings.Default.Save();
        }

        private void tbPMPVolumeLabel_TextChanged(object sender, EventArgs e)
        {
            PMPVolumeLabel = tbPMPVolumeLabel.Text;
            Properties.Settings.Default.PMPVolumeLabel = tbPMPVolumeLabel.Text;
            Properties.Settings.Default.Save();
        }





        //===========================================================================
        //
        // Background Worker Handlers
        //
        //===========================================================================

        //
        // LaptopSyncBW_DoWork(object sender, DoWorkEventArgs e)
        // Background worker for synchronizing with only the laptop
        //
        private void LaptopSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (FindLaptop())
            {
                ShowSyncStatus(true, false, true);
                this.Invoke(new MethodInvoker(() => LTlbl1.Text = "Scanning and preparing Laptop... DO NOT DISCONNECT THE DEVICE! This may take some time..."));
                LaptopSyncSuccess = PerformSyncLaptop();
                if (AutoExit && LaptopSyncSuccess)
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                    sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                    System.Environment.Exit(0); //exit the program if auto exit is enabled
                }
                else if (LaptopSyncSuccess)
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
                ShowSyncStatus(false, false, true);
            }
            this.Invoke(new MethodInvoker(() =>
            {
                btnSyncLaptop.Text = "Synchronize Laptop";
                PreferencesTab.Enabled = true; //allow changes to preferences
            }));
        }

        //
        // LaptopSyncBW_DoWork(object sender, DoWorkEventArgs e)
        // Background worker for synchronizing with the laptop while the PMP is being synchronized as well
        //
        private void LaptopSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        {
            Boolean FoundLaptop = FindLaptop();
            if (FoundLaptop)
            {
                this.Invoke(new MethodInvoker(() => LTlbl1.Text = "Scanning and preparing Laptop... DO NOT DISCONNECT THE DEVICE! This may take some time..."));
                LaptopSyncSuccess = PerformSyncLaptop();
            }
            else
                LaptopSyncSuccess = false;
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

        //
        // PMPSyncBW_DoWork(object sender, DoWorkEventArgs e)
        // Background worker for synchronizing with only the PMP
        //
        private void PMPSyncBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (FindPMP())
            {
                ShowSyncStatus(true, true, false);
                this.Invoke(new MethodInvoker(() => lbl1.Text = "Scanning and preparing PMP... DO NOT DISCONNECT THE DEVICE! This may take some time..."));
                PMPSyncSuccess = PerformSyncPMP();
                if (AutoExit && PMPSyncSuccess)
                {
                    System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.AaawYeah);
                    sound.PlaySync();  //Plays the sound in sync with the current thread so that it isn't cut off when the program exits

                    System.Environment.Exit(0); //exit the program if auto exit is enabled
                }
                else if (PMPSyncSuccess)
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
                ShowSyncStatus(false, true, false);
            }
            this.Invoke(new MethodInvoker(() =>
            {
                btnSyncPMP.Text = "Synchronize PMP";
                PreferencesTab.Enabled = true; //allow changes to preferences
            }));
        }

        //
        // PMPSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        // Background worker for synchronizing with the PMP while the laptop is being synchronized as well
        //
        private void PMPSyncTBW_DoWork(object sender, DoWorkEventArgs e)
        {
            Boolean FoundPMP = FindPMP();
            if (FoundPMP)
            {
                this.Invoke(new MethodInvoker(() => lbl1.Text = "Scanning and preparing PMP... DO NOT DISCONNECT THE DEVICE! This may take some time..."));
                PMPSyncSuccess = PerformSyncPMP();
            }
            else
                PMPSyncSuccess = false;
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

        //
        // ScanBW_DoWork(object sender, DoWorkEventArgs e)
        // Background worker for scanning the music library
        //
        private void ScanBW_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => PreferencesTab.Enabled = false)); //disallow changes to preferences

            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(Chrononizer.Properties.Resources.ScannerSweep);
            sound.Play();  //Plays the sound in a new thread

            this.Invoke(new MethodInvoker(() => lbNotDownscaled.Items.Clear())); //clear out previous items

            long flac, mp3, wma, m4a, ogg, wav, xm, mod, nsf, audioTotal, chiptunesTotal, total, dFlac;
            flac = mp3 = wma = m4a = ogg = wav = xm = mod = nsf = audioTotal = chiptunesTotal = total = dFlac = 0;
            double dSize, allSize, lSize;
            dSize = allSize = lSize = 0;

            lSize = GetDirectorySize(MusicLibrary, 0, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf);
            dSize = GetDownscaledDirectorySize(DownscaledLibrary, dSize, ref dFlac); //recurse through the downscaled files
            if (AutoHandle && Directory.Exists(MusicLibrary) && Directory.Exists(DownscaledLibrary)) //set the file attributes if auto handling is on
            {
                try
                {
                    File.SetAttributes(DownscaledLibrary, FileAttributes.Hidden | FileAttributes.System);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
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





        //===========================================================================
        //
        // Synchronize Functions
        //
        //===========================================================================

        //
        // DeleteOldDestinationDirectories(string[] sourceDirectories, string destinationPath)
        // Removes unnecessary folders from a path
        //
        private static void DeleteOldDestinationDirectories(string[] sourceDirectories, string destinationPath)
        {
            //get the destination files
            string[] destinationDirectories = null;
            try
            {
                destinationDirectories = Directory.GetDirectories(destinationPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

            foreach (string destinationDirectory in destinationDirectories)
            {
                FileInfo folder = new FileInfo(destinationDirectory);
                string[] found = Array.FindAll(sourceDirectories, path => Path.GetFileName(path).Equals(folder.Name));

                if (found.Length == 0)
                {
                    //delete file if not found in destination
                    try
                    {
                        Directory.Delete(destinationDirectory, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        //
        // DeleteOldDestinationFiles(string[] sourceFiles, string destinationPath)
        // Removes unnecessary files from a path
        //
        private static void DeleteOldDestinationFiles(string[] sourceFiles, string destinationPath)
        {
            //get the destination files
            string[] destinationFiles = null;
            try
            {
                destinationFiles = Directory.GetFiles(destinationPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

            foreach (string destinationFile in destinationFiles)
            {
                FileInfo file = new FileInfo(destinationFile);
                string[] found = Array.FindAll(sourceFiles, path => Path.GetFileName(path).Equals(file.Name));

                if (found.Length == 0)
                {
                    //delete file if not found in destination
                    try
                    {
                        File.Delete(destinationFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        //
        // FindLaptop()
        // Attempts to find the laptop
        //
        public Boolean FindLaptop()
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

        //
        // FindPMP()
        // Attempts to find the PMP
        //
        public Boolean FindPMP()
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

        //
        // PerformSyncLaptop()
        // Synchronizes the laptop with the music library
        //
        public Boolean PerformSyncLaptop()
        {
            double CopySize = 0;
            Queue<UpdateLocation> UpdateFiles = new Queue<UpdateLocation>();

            CopySize = PrepareSyncLaptop(MusicLibrary, LaptopLocation, ref UpdateFiles);

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

            //copy files to laptop here
            while (CopyFiles.Count > 0)
            {
                UpdateLocation update = CopyFiles.Dequeue();
                string source = update.SourceFile;
                string destination = update.DestinationFile;

                DialogResult result = DialogResult.Retry;
                while (result == DialogResult.Retry) //be ready in case of an error
                {
                    if (File.Exists(source))
                    {
                        //read source file info
                        FileInfo info = new FileInfo(source);

                        //copy the file
                        try
                        {
                            File.Copy(source, destination, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            if (!File.Exists(Path.GetFullPath(destination))) //check to see if the error was because of a lost connection
                                result = MessageBox.Show("Connection to the Laptop has been lost!", "Chrononizer", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            else if (!File.Exists(source)) //the source file no longer exists
                                result = MessageBox.Show("The file " + Path.GetFileName(source) + " cannot be found in the music library!", "Chrononizer", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                            else
                                return false; //unkown error, so stop the synchronization process

                            if (result == DialogResult.Abort || result == DialogResult.Cancel)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    PMPpb.Value = 0;
                                    lbl2.Text = " ";
                                    lbl1.Text = "Error: Synchronization with Laptop failed! ";
                                }));
                                return false;
                            }
                            else if (result == DialogResult.Retry)
                                continue;
                        }

                        //calculate progress
                        progress += (int)(((info.Length / CopySize) * 100000));
                        percent = Math.Round((((double)progress) / 1000), 2);
                        break;
                    }
                    else
                    {
                        result = MessageBox.Show("The file " + Path.GetFileName(source) + " cannot be found in the music library!", "Chrononizer", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                        if (result == DialogResult.Abort)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                LTpb.Value = 0;
                                LTlbl2.Text = " ";
                                LTlbl1.Text = "Error: Synchronization with Laptop failed! ";
                            }));
                            return false;
                        }
                        else if (result == DialogResult.Ignore)
                            break;
                    }
                }

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

            return true;
        }

        //
        // PerformSyncPMP()
        // Synchronizes the PMP with the music library
        //
        public Boolean PerformSyncPMP()
        {
            double CopySize = 0;
            Queue<UpdateLocation> UpdateFiles = new Queue<UpdateLocation>();

            CopySize = PrepareSyncPMP(MusicLibrary, PMPLocation, ref UpdateFiles);

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

                DialogResult result = DialogResult.Retry;
                while (result == DialogResult.Retry) //be ready in case of an error
                {
                    if (File.Exists(source))
                    {
                        //read source file info
                        FileInfo info = new FileInfo(source);

                        //copy the file
                        try
                        {
                            File.Copy(source, destination, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            if (!File.Exists(Path.GetFullPath(destination))) //check to see if the error was because of a lost connection
                                result = MessageBox.Show("The PMP has been disconnected!", "Chrononizer", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            else if (!File.Exists(source)) //the source file no longer exists
                                result = MessageBox.Show("The file " + Path.GetFileName(source) + " cannot be found in the music library!", "Chrononizer", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                            else
                                return false; //unkown error, so stop the synchronization process

                            if (result == DialogResult.Abort || result == DialogResult.Cancel)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    PMPpb.Value = 0;
                                    lbl2.Text = " ";
                                    lbl1.Text = "Error: Synchronization with PMP failed! ";
                                }));
                                return false;
                            }
                            else if (result == DialogResult.Retry)
                                continue;
                        }

                        //calculate progress
                        progress += (int)(((info.Length / CopySize) * 100000));
                        percent = Math.Round((((double)progress) / 1000), 2);
                        break;
                    }
                    else
                    {
                        result = MessageBox.Show("The file " + Path.GetFileName(source) + " cannot be found in the music library!", "Chrononizer", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                        if (result == DialogResult.Abort)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                PMPpb.Value = 0;
                                lbl2.Text = " ";
                                lbl1.Text = "Error: Synchronization with PMP failed! ";
                            }));
                            return false;
                        }
                        else if (result == DialogResult.Ignore)
                            break;
                    }
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    PMPpb.Value = progress; //get the file's size
                    lbl2.Text = percent.ToString() + "%";
                    lb1.Items.Remove(destination);
                }));
            }

            this.Invoke(new MethodInvoker(() =>
            {
                PMPpb.Value = PMPpb.Maximum;
                lbl2.Text = "100%";
                lbl1.Text = "Synchronization with PMP complete! ";
            }));

            return true;
        }

        //
        // PrepareSyncLaptop(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        // Scans the laptop, removes unnecessary files and folders, and determines what new files need to be copied
        //
        public double PrepareSyncLaptop(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        {
            double size = 0; //size of all files in this directory that will need to be copied
            bool dirExisted = DirExisted(destinationPath);
            if (!Directory.Exists(destinationPath))
                return size; //the directory could not be accessed

            //get the source files
            string[] sourceFiles = null;
            try
            {
                sourceFiles = Directory.GetFiles(sourcePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return size;
            }
            foreach (string sourceFile in sourceFiles)
            {
                FileInfo sourceInfo = new FileInfo(sourceFile);
                string destinationFile = Path.Combine(destinationPath, sourceInfo.Name);
                if (dirExisted && File.Exists(destinationFile))
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
            string[] dirs = null;
            try
            {
                dirs = Directory.GetDirectories(sourcePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return size;
            }
            DeleteOldDestinationDirectories(dirs, destinationPath);
            foreach (string dir in dirs)
            {
                if (dir == DownscaledLibrary.Substring(0, DownscaledLibrary.Length - 1))
                    continue;
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                //recursive do the directories
                size += PrepareSyncLaptop(dir, Path.Combine(destinationPath, dirInfo.Name), ref UpdateFiles);
            }
            return size;
        }

        //
        // PrepareSyncPMP(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        // Scans the PMP, removes unnecessary files and folders, and determines what new files need to be copied
        //
        public double PrepareSyncPMP(string sourcePath, string destinationPath, ref Queue<UpdateLocation> UpdateFiles)
        {
            double size = 0; //size of all files in this directory that will need to be copied
            bool dirExisted = DirExisted(destinationPath);
            if (!Directory.Exists(destinationPath))
                return size; //the directory could not be accessed

            //get the source files
            string[] sourceFiles = null;
            try
            {
                sourceFiles = Directory.GetFiles(sourcePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return size;
            }
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
                if (dirExisted && File.Exists(destinationFile))
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
            string[] dirs = null;
            try
            {
                dirs = Directory.GetDirectories(sourcePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return size;
            }
            DeleteOldDestinationDirectories(dirs, destinationPath);
            foreach (string dir in dirs)
            {
                if (dir == DownscaledLibrary.Substring(0, DownscaledLibrary.Length - 1))
                    continue; //skip downscaled folder
                if (EnableChiptunes && dir == ChiptunesLibrary.Substring(0, ChiptunesLibrary.Length - 1))
                    continue; //skip chiptunes folder

                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                //recursive do the directories
                size += PrepareSyncPMP(dir, Path.Combine(destinationPath, dirInfo.Name), ref UpdateFiles);
            }

            return size;
        }





        //===========================================================================
        //
        // Miscellaneous Functions
        //
        //===========================================================================

        //
        // AreBackgroundWorkersRunning()
        // Returns true if there are any background workers running
        //
        private Boolean AreBackgroundWorkersRunning()
        {
            if (PMPSyncBW.IsBusy || PMPSyncTBW.IsBusy || LaptopSyncBW.IsBusy || LaptopSyncTBW.IsBusy || ScanBW.IsBusy)
                return true;
            else
                return false;
        }

        //
        // BytesToSize(double size)
        // Converts a number of bytes to a readable size measured in KB, MB, GB, TB, or PB
        //
        public static string BytesToSize(double size)
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

        //
        // DirExisted(string path)
        // Return true if the directory exists. If the directory does not exist, create it and return false.
        //
        private bool DirExisted(string path)
        {
            Boolean exists = false;
            //create destination directory if not exist
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false; //directory creation failed
                }
                exists = false;
            }
            else
                exists = true;
            //hide all .mediaartlocal folders
            if (HideMediaArtLocal && Path.GetFileName(path) == ".mediaartlocal")
            {
                try
                {
                    File.SetAttributes(Path.GetFullPath(path), FileAttributes.Hidden | FileAttributes.System);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
            return exists;
        }

        //
        // DoLibrariesExist()
        // Makes sure that the library paths are valid. If they are not, an error will be thrown
        //
        private Boolean DoLibrariesExist()
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

        //
        // Plural(long value, string units)
        // Make a string plural or singular depending upon the number of units
        //
        public static string Plural(long value, string units)
        {
            string plural = " ";
            if (value == 1)
                plural = value.ToString() + " " + units; //only one object
            else
                plural = value.ToString() + " " + units + "s"; //add an s if plural
            return plural;
        }

        //
        // OpenFolderDialog(int TextBox)
        // Opens the FolderBrowserDialog() for a specific text box
        //
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
            else if (TextBox == 3)
            {
                if (!open.SelectedPath.Contains(MusicLibrary))
                    MessageBox.Show("The chiptunes library is expected to be inside the music library!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    tbChiptunesLocation.Text = open.SelectedPath + "\\";
            }
            else if (TextBox == 4)
            {
                if (MusicLibrary.Contains(open.SelectedPath) || open.SelectedPath.Contains(MusicLibrary)
                    || DownscaledLibrary.Contains(open.SelectedPath) || open.SelectedPath.Contains(DownscaledLibrary))
                {
                    MessageBox.Show("The override path cannot be related to the any of the libraries!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    tbPMPLocation.Text = open.SelectedPath + "\\";
            }
            else if (TextBox == 5)
            {
                if (MusicLibrary.Contains(open.SelectedPath) || open.SelectedPath.Contains(MusicLibrary)
                    || DownscaledLibrary.Contains(open.SelectedPath) || open.SelectedPath.Contains(DownscaledLibrary))
                {
                    MessageBox.Show("The override path cannot be related to the any of the libraries!", "Chrononizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    tbLaptopLocation.Text = open.SelectedPath + "\\";
            }
        }

        //
        // GetDirectorySize(string root, double size, ref long flac, ref long mp3, ref long wma, ref long m4a, ref long ogg, ref long wav, ref long xm, ref long mod, ref long nsf)
        // Calculates a directory size and counts the number of unique file types
        //
        private double GetDirectorySize(string root, double size, ref long flac, ref long mp3, ref long wma, ref long m4a, ref long ogg, ref long wav, ref long xm, ref long mod, ref long nsf)
        {
            if (!Directory.Exists(root)) return size; //path is invalid
            string[] files = null;
            string[] folders = null;
            try
            {
                files = Directory.GetFiles(root, "*.*"); //get array of all file names
                folders = Directory.GetDirectories(root); //get array of all folder names for this directory
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return size;
            }

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
                else if (HideMediaArtLocal && Path.GetFileName(name) == ".mediaartlocal")
                {
                    try
                    {
                        File.SetAttributes(Path.GetFullPath(name), FileAttributes.Hidden | FileAttributes.System);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return size;
                    }
                }
                size = GetDirectorySize(name, size, ref flac, ref mp3, ref wma, ref m4a, ref ogg, ref wav, ref xm, ref mod, ref nsf); //recurse through the folders
            }
            return size;
        }

        //
        // GetDownscaledDirectorySize(string root, double size, ref long flac)
        // Calculates a directory size for the downscaled library
        //
        private double GetDownscaledDirectorySize(string root, double size, ref long flac)
        {
            if (!Directory.Exists(root))
            {
                if (AutoHandle && Directory.Exists(MusicLibrary)) Directory.CreateDirectory(root); //create the directory if it doesn't exist
                return size;
            }
            string[] files = null;
            string[] folders = null;
            try
            {
                files = Directory.GetFiles(root, "*.*"); //get array of all file names
                folders = Directory.GetDirectories(root); //get array of all folder names for this directory
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return size;
            }

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
                        {
                            try
                            {
                                File.Delete(name); //downscaled flac not necessary
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            if (ShowFiles) this.Invoke(new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //show the file in the list
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
                            if (RemoveUnnecessary)
                            {
                                try
                                {
                                    File.Delete(name); //file is unnecessary
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }
                        else
                        {
                            Luminescence.Xiph.FlacTagger flacTag = new FlacTagger(name); //get the flac's tag
                            if (!File.Exists(defaultFile))
                            {
                                if (RemoveUnnecessary)
                                {
                                    try
                                    {
                                        File.Delete(name); //flac's upscaled file does not exist and is unnecessary
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                }
                                else
                                {
                                    size += info.Length; //add to the size
                                    flac++;
                                }
                            }
                            else if (flacTag.BitsPerSample > 16 || flacTag.SampleRate > 48000)
                            {
                                if (RemoveImproper)
                                {
                                    try
                                    {
                                        File.Delete(name); //downscaled flac not necessary
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                }
                                else
                                {
                                    if (ShowFiles) this.Invoke(new MethodInvoker(() => lbNotDownscaled.Items.Add(name))); //show the file in the list
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
                                    {
                                        try
                                        {
                                            File.Delete(name); //flac did not need downscaling and is unnecessary
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex);
                                        }
                                    }
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
                else if (RemoveUnsupported)
                {
                    try
                    {
                        File.Delete(name); //remove unsupported files
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            foreach (string name in folders)
            {
                size = GetDownscaledDirectorySize(name, size, ref flac); //recurse through the folders
            }

            if (RemoveEmpty && !Directory.EnumerateFileSystemEntries(root).Any() && Path.GetFullPath(root) != DownscaledLibrary)
            {
                try
                {
                    Directory.Delete(root); //delete empty folders
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return size;
            }
            return size;
        }

        //
        // RemoveEmptyDirectories(string root)
        // Removes all empty directories from a path
        //
        private void RemoveEmptyDirectories(string root)
        {
            string[] folders = null;
            try
            {
                folders = Directory.GetDirectories(root); //get array of all folder names for this directory
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            if (!Directory.EnumerateFileSystemEntries(root).Any() && Path.GetFullPath(root) != MusicLibrary)
            {
                try
                {
                    Directory.Delete(root); //delete empty folders
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return;
            }
            foreach (string name in folders)
            {
                RemoveEmptyDirectories(name); //recurse through the folders
            }
            return;
        }

        //
        // ShowSyncStatus(Boolean visible, Boolean pmp, Boolean laptop)
        // Shows or hides the synchronize status screen
        //
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
                        PMPflow = new FlowLayoutPanel();
                        PMPflow.FlowDirection = FlowDirection.LeftToRight;
                        PMPflow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        PMPflow.AutoSize = true;
                        lbl1 = new Label();
                        lbl1.Text = "Searching for PMP...";
                        lbl1.AutoSize = true;
                        PMPflow.Controls.Add(lbl1);
                        lbl2 = new Label();
                        lbl2.Text = " ";
                        lbl2.AutoSize = true;
                        PMPflow.Controls.Add(lbl2);

                        //Progressbar
                        PMPpb = new ProgressBar();
                        PMPpb.Maximum = 100000;
                        PMPpb.Value = 0;
                        PMPpb.Width = 765;
                        PMPpb.Height = 38;
                        PMPpb.Value = 0;

                        //Listbox
                        lb1 = new ListBox();
                        lb1.Width = 765;
                        if (pmp && laptop)
                            lb1.Height = 180; //when there is two
                        else
                            lb1.Height = 450; //when there is one

                        //Add the objects to the layout
                        flowLayoutPanel2.Controls.Add(PMPflow);
                        flowLayoutPanel2.Controls.Add(PMPpb);
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
                        PMPflow.Controls.Remove(lbl1);
                        PMPflow.Controls.Remove(lbl2);
                        flowLayoutPanel2.Controls.Remove(lb1);
                        flowLayoutPanel2.Controls.Remove(PMPpb);
                        flowLayoutPanel2.Controls.Remove(PMPflow);
                        lbl1 = null;
                        lbl2 = null;
                        PMPflow = null;
                        PMPpb = null;
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
    }
}
