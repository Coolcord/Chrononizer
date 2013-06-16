namespace Chrononizer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.SyncTab = new System.Windows.Forms.TabPage();
            this.btnSyncLaptop = new System.Windows.Forms.Button();
            this.btnSyncPMP = new System.Windows.Forms.Button();
            this.btnSyncBoth = new System.Windows.Forms.Button();
            this.InfoTab = new System.Windows.Forms.TabPage();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flpLibraryInfo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLibrarySize = new System.Windows.Forms.Label();
            this.lblLibraryBytes = new System.Windows.Forms.Label();
            this.lblDownscaledBytes = new System.Windows.Forms.Label();
            this.lblTotalBytes = new System.Windows.Forms.Label();
            this.lblFileBreakdown = new System.Windows.Forms.Label();
            this.lblFLACFiles = new System.Windows.Forms.Label();
            this.lblMP3Files = new System.Windows.Forms.Label();
            this.lblM4AFiles = new System.Windows.Forms.Label();
            this.lblWMAFiles = new System.Windows.Forms.Label();
            this.lblOGGFiles = new System.Windows.Forms.Label();
            this.lblWAVFiles = new System.Windows.Forms.Label();
            this.lblXMFiles = new System.Windows.Forms.Label();
            this.lblMODFiles = new System.Windows.Forms.Label();
            this.lblNSFFiles = new System.Windows.Forms.Label();
            this.lblSongCount = new System.Windows.Forms.Label();
            this.lblLibraryFiles = new System.Windows.Forms.Label();
            this.lblDownscaledFiles = new System.Windows.Forms.Label();
            this.lblChiptunesFiles = new System.Windows.Forms.Label();
            this.lblTotalFiles = new System.Windows.Forms.Label();
            this.lblNotDownscaled = new System.Windows.Forms.Label();
            this.lbNotDownscaled = new System.Windows.Forms.ListBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.PreferencesTab = new System.Windows.Forms.TabPage();
            this.cbAutoExit = new System.Windows.Forms.CheckBox();
            this.cbHideMediaArtLocal = new System.Windows.Forms.CheckBox();
            this.btnLaptopLocation = new System.Windows.Forms.Button();
            this.btnPMPLocation = new System.Windows.Forms.Button();
            this.cbOverrideLaptopPath = new System.Windows.Forms.CheckBox();
            this.cbOverridePMPPath = new System.Windows.Forms.CheckBox();
            this.tbLaptopLocation = new System.Windows.Forms.TextBox();
            this.lblLaptopLocation = new System.Windows.Forms.Label();
            this.tbPMPLocation = new System.Windows.Forms.TextBox();
            this.lblPMPLocation = new System.Windows.Forms.Label();
            this.tbPMPVolumeLabel = new System.Windows.Forms.TextBox();
            this.lblPMPVolumeLabel = new System.Windows.Forms.Label();
            this.tbLaptopUsername = new System.Windows.Forms.TextBox();
            this.lblLaptopUsername = new System.Windows.Forms.Label();
            this.tbLaptopHostname = new System.Windows.Forms.TextBox();
            this.lblLaptopHostname = new System.Windows.Forms.Label();
            this.cbCheckPMPSystem = new System.Windows.Forms.CheckBox();
            this.cbAskSync = new System.Windows.Forms.CheckBox();
            this.cbPreventSynchingUpscaled = new System.Windows.Forms.CheckBox();
            this.cbChiptunesLibrary = new System.Windows.Forms.CheckBox();
            this.btnChiptunesLocation = new System.Windows.Forms.Button();
            this.lblChiptunesLocation = new System.Windows.Forms.Label();
            this.tbChiptunesLocation = new System.Windows.Forms.TextBox();
            this.cbRemoveEmpty = new System.Windows.Forms.CheckBox();
            this.cbRemoveUnnecessary = new System.Windows.Forms.CheckBox();
            this.cbRemoveUnsupported = new System.Windows.Forms.CheckBox();
            this.cbShowImproper = new System.Windows.Forms.CheckBox();
            this.cbAutoHandle = new System.Windows.Forms.CheckBox();
            this.cbRemoveImproper = new System.Windows.Forms.CheckBox();
            this.btnDownscaledLocation = new System.Windows.Forms.Button();
            this.tbDownscaledLocation = new System.Windows.Forms.TextBox();
            this.lblDownscaledLocation = new System.Windows.Forms.Label();
            this.btnLibraryLocation = new System.Windows.Forms.Button();
            this.lblLibraryLocation = new System.Windows.Forms.Label();
            this.tbLibraryLocation = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.PMPSyncBW = new System.ComponentModel.BackgroundWorker();
            this.ScanBW = new System.ComponentModel.BackgroundWorker();
            this.LaptopSyncBW = new System.ComponentModel.BackgroundWorker();
            this.PMPSyncTBW = new System.ComponentModel.BackgroundWorker();
            this.LaptopSyncTBW = new System.ComponentModel.BackgroundWorker();
            this.cbAutoExitOne = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.SyncTab.SuspendLayout();
            this.InfoTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flpLibraryInfo.SuspendLayout();
            this.PreferencesTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.SyncTab);
            this.tabControl.Controls.Add(this.InfoTab);
            this.tabControl.Controls.Add(this.PreferencesTab);
            this.tabControl.Location = new System.Drawing.Point(-1, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(785, 556);
            this.tabControl.TabIndex = 8;
            // 
            // SyncTab
            // 
            this.SyncTab.Controls.Add(this.btnSyncLaptop);
            this.SyncTab.Controls.Add(this.btnSyncPMP);
            this.SyncTab.Controls.Add(this.btnSyncBoth);
            this.SyncTab.Location = new System.Drawing.Point(4, 25);
            this.SyncTab.Name = "SyncTab";
            this.SyncTab.Padding = new System.Windows.Forms.Padding(3);
            this.SyncTab.Size = new System.Drawing.Size(777, 527);
            this.SyncTab.TabIndex = 0;
            this.SyncTab.Text = "Synchronize";
            this.SyncTab.UseVisualStyleBackColor = true;
            // 
            // btnSyncLaptop
            // 
            this.btnSyncLaptop.Location = new System.Drawing.Point(392, 276);
            this.btnSyncLaptop.Name = "btnSyncLaptop";
            this.btnSyncLaptop.Size = new System.Drawing.Size(376, 151);
            this.btnSyncLaptop.TabIndex = 2;
            this.btnSyncLaptop.Text = "Synchronize Laptop";
            this.btnSyncLaptop.UseVisualStyleBackColor = true;
            this.btnSyncLaptop.Click += new System.EventHandler(this.btnSyncLaptop_Click);
            // 
            // btnSyncPMP
            // 
            this.btnSyncPMP.Location = new System.Drawing.Point(11, 276);
            this.btnSyncPMP.Name = "btnSyncPMP";
            this.btnSyncPMP.Size = new System.Drawing.Size(376, 151);
            this.btnSyncPMP.TabIndex = 1;
            this.btnSyncPMP.Text = "Synchronize PMP";
            this.btnSyncPMP.UseVisualStyleBackColor = true;
            this.btnSyncPMP.Click += new System.EventHandler(this.btnSyncPMP_Click);
            // 
            // btnSyncBoth
            // 
            this.btnSyncBoth.Location = new System.Drawing.Point(11, 65);
            this.btnSyncBoth.Name = "btnSyncBoth";
            this.btnSyncBoth.Size = new System.Drawing.Size(758, 182);
            this.btnSyncBoth.TabIndex = 0;
            this.btnSyncBoth.Text = "Synchronize Both";
            this.btnSyncBoth.UseVisualStyleBackColor = true;
            this.btnSyncBoth.Click += new System.EventHandler(this.btnSyncBoth_Click);
            // 
            // InfoTab
            // 
            this.InfoTab.Controls.Add(this.splitContainer);
            this.InfoTab.Controls.Add(this.btnScan);
            this.InfoTab.Location = new System.Drawing.Point(4, 25);
            this.InfoTab.Name = "InfoTab";
            this.InfoTab.Size = new System.Drawing.Size(777, 527);
            this.InfoTab.TabIndex = 2;
            this.InfoTab.Text = "Library Info";
            this.InfoTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Location = new System.Drawing.Point(12, 4);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.flpLibraryInfo);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.lblNotDownscaled);
            this.splitContainer.Panel2.Controls.Add(this.lbNotDownscaled);
            this.splitContainer.Size = new System.Drawing.Size(757, 391);
            this.splitContainer.SplitterDistance = 340;
            this.splitContainer.TabIndex = 4;
            // 
            // flpLibraryInfo
            // 
            this.flpLibraryInfo.Controls.Add(this.lblLibrarySize);
            this.flpLibraryInfo.Controls.Add(this.lblLibraryBytes);
            this.flpLibraryInfo.Controls.Add(this.lblDownscaledBytes);
            this.flpLibraryInfo.Controls.Add(this.lblTotalBytes);
            this.flpLibraryInfo.Controls.Add(this.lblFileBreakdown);
            this.flpLibraryInfo.Controls.Add(this.lblFLACFiles);
            this.flpLibraryInfo.Controls.Add(this.lblMP3Files);
            this.flpLibraryInfo.Controls.Add(this.lblM4AFiles);
            this.flpLibraryInfo.Controls.Add(this.lblWMAFiles);
            this.flpLibraryInfo.Controls.Add(this.lblOGGFiles);
            this.flpLibraryInfo.Controls.Add(this.lblWAVFiles);
            this.flpLibraryInfo.Controls.Add(this.lblXMFiles);
            this.flpLibraryInfo.Controls.Add(this.lblMODFiles);
            this.flpLibraryInfo.Controls.Add(this.lblNSFFiles);
            this.flpLibraryInfo.Controls.Add(this.lblSongCount);
            this.flpLibraryInfo.Controls.Add(this.lblLibraryFiles);
            this.flpLibraryInfo.Controls.Add(this.lblDownscaledFiles);
            this.flpLibraryInfo.Controls.Add(this.lblChiptunesFiles);
            this.flpLibraryInfo.Controls.Add(this.lblTotalFiles);
            this.flpLibraryInfo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpLibraryInfo.Location = new System.Drawing.Point(3, 3);
            this.flpLibraryInfo.Name = "flpLibraryInfo";
            this.flpLibraryInfo.Size = new System.Drawing.Size(334, 385);
            this.flpLibraryInfo.TabIndex = 4;
            // 
            // lblLibrarySize
            // 
            this.lblLibrarySize.AutoSize = true;
            this.lblLibrarySize.Location = new System.Drawing.Point(3, 0);
            this.lblLibrarySize.Name = "lblLibrarySize";
            this.lblLibrarySize.Size = new System.Drawing.Size(87, 17);
            this.lblLibrarySize.TabIndex = 21;
            this.lblLibrarySize.Text = "Library Size:";
            // 
            // lblLibraryBytes
            // 
            this.lblLibraryBytes.AutoSize = true;
            this.lblLibraryBytes.Location = new System.Drawing.Point(3, 17);
            this.lblLibraryBytes.Name = "lblLibraryBytes";
            this.lblLibraryBytes.Size = new System.Drawing.Size(106, 17);
            this.lblLibraryBytes.TabIndex = 0;
            this.lblLibraryBytes.Text = "Library: 0 bytes";
            // 
            // lblDownscaledBytes
            // 
            this.lblDownscaledBytes.AutoSize = true;
            this.lblDownscaledBytes.Location = new System.Drawing.Point(3, 34);
            this.lblDownscaledBytes.Name = "lblDownscaledBytes";
            this.lblDownscaledBytes.Size = new System.Drawing.Size(138, 17);
            this.lblDownscaledBytes.TabIndex = 17;
            this.lblDownscaledBytes.Text = "Downscaled: 0 bytes";
            // 
            // lblTotalBytes
            // 
            this.lblTotalBytes.AutoSize = true;
            this.lblTotalBytes.Location = new System.Drawing.Point(3, 51);
            this.lblTotalBytes.Name = "lblTotalBytes";
            this.lblTotalBytes.Size = new System.Drawing.Size(94, 17);
            this.lblTotalBytes.TabIndex = 20;
            this.lblTotalBytes.Text = "Total: 0 bytes";
            // 
            // lblFileBreakdown
            // 
            this.lblFileBreakdown.AutoSize = true;
            this.lblFileBreakdown.Location = new System.Drawing.Point(3, 88);
            this.lblFileBreakdown.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblFileBreakdown.Name = "lblFileBreakdown";
            this.lblFileBreakdown.Size = new System.Drawing.Size(108, 17);
            this.lblFileBreakdown.TabIndex = 22;
            this.lblFileBreakdown.Text = "File Breakdown:";
            // 
            // lblFLACFiles
            // 
            this.lblFLACFiles.AutoSize = true;
            this.lblFLACFiles.Location = new System.Drawing.Point(3, 105);
            this.lblFLACFiles.Name = "lblFLACFiles";
            this.lblFLACFiles.Size = new System.Drawing.Size(87, 17);
            this.lblFLACFiles.TabIndex = 4;
            this.lblFLACFiles.Text = "FLAC: 0 files";
            // 
            // lblMP3Files
            // 
            this.lblMP3Files.AutoSize = true;
            this.lblMP3Files.Location = new System.Drawing.Point(3, 122);
            this.lblMP3Files.Name = "lblMP3Files";
            this.lblMP3Files.Size = new System.Drawing.Size(81, 17);
            this.lblMP3Files.TabIndex = 5;
            this.lblMP3Files.Text = "MP3: 0 files";
            // 
            // lblM4AFiles
            // 
            this.lblM4AFiles.AutoSize = true;
            this.lblM4AFiles.Location = new System.Drawing.Point(3, 139);
            this.lblM4AFiles.Name = "lblM4AFiles";
            this.lblM4AFiles.Size = new System.Drawing.Size(81, 17);
            this.lblM4AFiles.TabIndex = 7;
            this.lblM4AFiles.Text = "M4A: 0 files";
            // 
            // lblWMAFiles
            // 
            this.lblWMAFiles.AutoSize = true;
            this.lblWMAFiles.Location = new System.Drawing.Point(3, 156);
            this.lblWMAFiles.Name = "lblWMAFiles";
            this.lblWMAFiles.Size = new System.Drawing.Size(86, 17);
            this.lblWMAFiles.TabIndex = 6;
            this.lblWMAFiles.Text = "WMA: 0 files";
            // 
            // lblOGGFiles
            // 
            this.lblOGGFiles.AutoSize = true;
            this.lblOGGFiles.Location = new System.Drawing.Point(3, 173);
            this.lblOGGFiles.Name = "lblOGGFiles";
            this.lblOGGFiles.Size = new System.Drawing.Size(86, 17);
            this.lblOGGFiles.TabIndex = 8;
            this.lblOGGFiles.Text = "OGG: 0 files";
            // 
            // lblWAVFiles
            // 
            this.lblWAVFiles.AutoSize = true;
            this.lblWAVFiles.Location = new System.Drawing.Point(3, 190);
            this.lblWAVFiles.Name = "lblWAVFiles";
            this.lblWAVFiles.Size = new System.Drawing.Size(84, 17);
            this.lblWAVFiles.TabIndex = 9;
            this.lblWAVFiles.Text = "WAV: 0 files";
            // 
            // lblXMFiles
            // 
            this.lblXMFiles.AutoSize = true;
            this.lblXMFiles.Location = new System.Drawing.Point(3, 207);
            this.lblXMFiles.Name = "lblXMFiles";
            this.lblXMFiles.Size = new System.Drawing.Size(73, 17);
            this.lblXMFiles.TabIndex = 10;
            this.lblXMFiles.Text = "XM: 0 files";
            // 
            // lblMODFiles
            // 
            this.lblMODFiles.AutoSize = true;
            this.lblMODFiles.Location = new System.Drawing.Point(3, 224);
            this.lblMODFiles.Name = "lblMODFiles";
            this.lblMODFiles.Size = new System.Drawing.Size(85, 17);
            this.lblMODFiles.TabIndex = 11;
            this.lblMODFiles.Text = "MOD: 0 files";
            // 
            // lblNSFFiles
            // 
            this.lblNSFFiles.AutoSize = true;
            this.lblNSFFiles.Location = new System.Drawing.Point(3, 241);
            this.lblNSFFiles.Name = "lblNSFFiles";
            this.lblNSFFiles.Size = new System.Drawing.Size(80, 17);
            this.lblNSFFiles.TabIndex = 12;
            this.lblNSFFiles.Text = "NSF: 0 files";
            // 
            // lblSongCount
            // 
            this.lblSongCount.AutoSize = true;
            this.lblSongCount.Location = new System.Drawing.Point(3, 278);
            this.lblSongCount.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblSongCount.Name = "lblSongCount";
            this.lblSongCount.Size = new System.Drawing.Size(86, 17);
            this.lblSongCount.TabIndex = 23;
            this.lblSongCount.Text = "Song Count:";
            // 
            // lblLibraryFiles
            // 
            this.lblLibraryFiles.AutoSize = true;
            this.lblLibraryFiles.Location = new System.Drawing.Point(3, 295);
            this.lblLibraryFiles.Name = "lblLibraryFiles";
            this.lblLibraryFiles.Size = new System.Drawing.Size(110, 17);
            this.lblLibraryFiles.TabIndex = 13;
            this.lblLibraryFiles.Text = "Library: 0 songs";
            // 
            // lblDownscaledFiles
            // 
            this.lblDownscaledFiles.AutoSize = true;
            this.lblDownscaledFiles.Location = new System.Drawing.Point(3, 312);
            this.lblDownscaledFiles.Name = "lblDownscaledFiles";
            this.lblDownscaledFiles.Size = new System.Drawing.Size(129, 17);
            this.lblDownscaledFiles.TabIndex = 18;
            this.lblDownscaledFiles.Text = "Downscaled: 0 files";
            // 
            // lblChiptunesFiles
            // 
            this.lblChiptunesFiles.AutoSize = true;
            this.lblChiptunesFiles.Location = new System.Drawing.Point(3, 329);
            this.lblChiptunesFiles.Name = "lblChiptunesFiles";
            this.lblChiptunesFiles.Size = new System.Drawing.Size(129, 17);
            this.lblChiptunesFiles.TabIndex = 14;
            this.lblChiptunesFiles.Text = "Chiptunes: 0 songs";
            // 
            // lblTotalFiles
            // 
            this.lblTotalFiles.AutoSize = true;
            this.lblTotalFiles.Location = new System.Drawing.Point(3, 346);
            this.lblTotalFiles.Name = "lblTotalFiles";
            this.lblTotalFiles.Size = new System.Drawing.Size(234, 17);
            this.lblTotalFiles.TabIndex = 15;
            this.lblTotalFiles.Text = "Total (without downscaled): 0 songs";
            // 
            // lblNotDownscaled
            // 
            this.lblNotDownscaled.AutoSize = true;
            this.lblNotDownscaled.Location = new System.Drawing.Point(4, 4);
            this.lblNotDownscaled.Name = "lblNotDownscaled";
            this.lblNotDownscaled.Size = new System.Drawing.Size(186, 17);
            this.lblNotDownscaled.TabIndex = 1;
            this.lblNotDownscaled.Text = "Files that need downscaling:";
            // 
            // lbNotDownscaled
            // 
            this.lbNotDownscaled.FormattingEnabled = true;
            this.lbNotDownscaled.HorizontalScrollbar = true;
            this.lbNotDownscaled.ItemHeight = 16;
            this.lbNotDownscaled.Location = new System.Drawing.Point(3, 24);
            this.lbNotDownscaled.Name = "lbNotDownscaled";
            this.lbNotDownscaled.Size = new System.Drawing.Size(406, 356);
            this.lbNotDownscaled.TabIndex = 0;
            this.lbNotDownscaled.DoubleClick += new System.EventHandler(this.lbNotDownscaled_DoubleClick);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(11, 401);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(758, 118);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "Scan Library";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // PreferencesTab
            // 
            this.PreferencesTab.Controls.Add(this.cbAutoExitOne);
            this.PreferencesTab.Controls.Add(this.cbAutoExit);
            this.PreferencesTab.Controls.Add(this.cbHideMediaArtLocal);
            this.PreferencesTab.Controls.Add(this.btnLaptopLocation);
            this.PreferencesTab.Controls.Add(this.btnPMPLocation);
            this.PreferencesTab.Controls.Add(this.cbOverrideLaptopPath);
            this.PreferencesTab.Controls.Add(this.cbOverridePMPPath);
            this.PreferencesTab.Controls.Add(this.tbLaptopLocation);
            this.PreferencesTab.Controls.Add(this.lblLaptopLocation);
            this.PreferencesTab.Controls.Add(this.tbPMPLocation);
            this.PreferencesTab.Controls.Add(this.lblPMPLocation);
            this.PreferencesTab.Controls.Add(this.tbPMPVolumeLabel);
            this.PreferencesTab.Controls.Add(this.lblPMPVolumeLabel);
            this.PreferencesTab.Controls.Add(this.tbLaptopUsername);
            this.PreferencesTab.Controls.Add(this.lblLaptopUsername);
            this.PreferencesTab.Controls.Add(this.tbLaptopHostname);
            this.PreferencesTab.Controls.Add(this.lblLaptopHostname);
            this.PreferencesTab.Controls.Add(this.cbCheckPMPSystem);
            this.PreferencesTab.Controls.Add(this.cbAskSync);
            this.PreferencesTab.Controls.Add(this.cbPreventSynchingUpscaled);
            this.PreferencesTab.Controls.Add(this.cbChiptunesLibrary);
            this.PreferencesTab.Controls.Add(this.btnChiptunesLocation);
            this.PreferencesTab.Controls.Add(this.lblChiptunesLocation);
            this.PreferencesTab.Controls.Add(this.tbChiptunesLocation);
            this.PreferencesTab.Controls.Add(this.cbRemoveEmpty);
            this.PreferencesTab.Controls.Add(this.cbRemoveUnnecessary);
            this.PreferencesTab.Controls.Add(this.cbRemoveUnsupported);
            this.PreferencesTab.Controls.Add(this.cbShowImproper);
            this.PreferencesTab.Controls.Add(this.cbAutoHandle);
            this.PreferencesTab.Controls.Add(this.cbRemoveImproper);
            this.PreferencesTab.Controls.Add(this.btnDownscaledLocation);
            this.PreferencesTab.Controls.Add(this.tbDownscaledLocation);
            this.PreferencesTab.Controls.Add(this.lblDownscaledLocation);
            this.PreferencesTab.Controls.Add(this.btnLibraryLocation);
            this.PreferencesTab.Controls.Add(this.lblLibraryLocation);
            this.PreferencesTab.Controls.Add(this.tbLibraryLocation);
            this.PreferencesTab.Location = new System.Drawing.Point(4, 25);
            this.PreferencesTab.Name = "PreferencesTab";
            this.PreferencesTab.Size = new System.Drawing.Size(777, 527);
            this.PreferencesTab.TabIndex = 3;
            this.PreferencesTab.Text = "Preferences";
            this.PreferencesTab.UseVisualStyleBackColor = true;
            // 
            // cbAutoExit
            // 
            this.cbAutoExit.AutoSize = true;
            this.cbAutoExit.Location = new System.Drawing.Point(12, 440);
            this.cbAutoExit.Name = "cbAutoExit";
            this.cbAutoExit.Size = new System.Drawing.Size(428, 21);
            this.cbAutoExit.TabIndex = 36;
            this.cbAutoExit.Text = "Automatically exit Chrononizer upon successful synchronization";
            this.cbAutoExit.UseVisualStyleBackColor = true;
            this.cbAutoExit.CheckedChanged += new System.EventHandler(this.cbAutoExit_CheckedChanged);
            // 
            // cbHideMediaArtLocal
            // 
            this.cbHideMediaArtLocal.AutoSize = true;
            this.cbHideMediaArtLocal.Location = new System.Drawing.Point(12, 386);
            this.cbHideMediaArtLocal.Name = "cbHideMediaArtLocal";
            this.cbHideMediaArtLocal.Size = new System.Drawing.Size(485, 21);
            this.cbHideMediaArtLocal.TabIndex = 35;
            this.cbHideMediaArtLocal.Text = "Add hidden attributes .mediaartlocal folders when scanning music library";
            this.cbHideMediaArtLocal.UseVisualStyleBackColor = true;
            this.cbHideMediaArtLocal.CheckedChanged += new System.EventHandler(this.cbHideMediaArtLocal_CheckedChanged);
            // 
            // btnLaptopLocation
            // 
            this.btnLaptopLocation.Enabled = false;
            this.btnLaptopLocation.Location = new System.Drawing.Point(741, 313);
            this.btnLaptopLocation.Name = "btnLaptopLocation";
            this.btnLaptopLocation.Size = new System.Drawing.Size(31, 23);
            this.btnLaptopLocation.TabIndex = 34;
            this.btnLaptopLocation.Text = "...";
            this.btnLaptopLocation.UseVisualStyleBackColor = true;
            this.btnLaptopLocation.Click += new System.EventHandler(this.btnLaptopLocation_Click);
            // 
            // btnPMPLocation
            // 
            this.btnPMPLocation.Enabled = false;
            this.btnPMPLocation.Location = new System.Drawing.Point(741, 150);
            this.btnPMPLocation.Name = "btnPMPLocation";
            this.btnPMPLocation.Size = new System.Drawing.Size(31, 23);
            this.btnPMPLocation.TabIndex = 33;
            this.btnPMPLocation.Text = "...";
            this.btnPMPLocation.UseVisualStyleBackColor = true;
            this.btnPMPLocation.Click += new System.EventHandler(this.btnPMPLocation_Click);
            // 
            // cbOverrideLaptopPath
            // 
            this.cbOverrideLaptopPath.AutoSize = true;
            this.cbOverrideLaptopPath.Location = new System.Drawing.Point(451, 269);
            this.cbOverrideLaptopPath.Name = "cbOverrideLaptopPath";
            this.cbOverrideLaptopPath.Size = new System.Drawing.Size(166, 21);
            this.cbOverrideLaptopPath.TabIndex = 32;
            this.cbOverrideLaptopPath.Text = "Override Laptop Path";
            this.cbOverrideLaptopPath.UseVisualStyleBackColor = true;
            this.cbOverrideLaptopPath.CheckedChanged += new System.EventHandler(this.cbOverrideLaptopPath_CheckedChanged);
            // 
            // cbOverridePMPPath
            // 
            this.cbOverridePMPPath.AutoSize = true;
            this.cbOverridePMPPath.Location = new System.Drawing.Point(451, 106);
            this.cbOverridePMPPath.Name = "cbOverridePMPPath";
            this.cbOverridePMPPath.Size = new System.Drawing.Size(151, 21);
            this.cbOverridePMPPath.TabIndex = 31;
            this.cbOverridePMPPath.Text = "Override PMP Path";
            this.cbOverridePMPPath.UseVisualStyleBackColor = true;
            this.cbOverridePMPPath.CheckedChanged += new System.EventHandler(this.cbOverridePMPPath_CheckedChanged);
            // 
            // tbLaptopLocation
            // 
            this.tbLaptopLocation.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbLaptopLocation.Enabled = false;
            this.tbLaptopLocation.Location = new System.Drawing.Point(451, 313);
            this.tbLaptopLocation.Name = "tbLaptopLocation";
            this.tbLaptopLocation.ReadOnly = true;
            this.tbLaptopLocation.Size = new System.Drawing.Size(284, 22);
            this.tbLaptopLocation.TabIndex = 28;
            this.tbLaptopLocation.Click += new System.EventHandler(this.tbLaptopLocation_Click);
            this.tbLaptopLocation.TextChanged += new System.EventHandler(this.tbLaptopLocation_TextChanged);
            // 
            // lblLaptopLocation
            // 
            this.lblLaptopLocation.AutoSize = true;
            this.lblLaptopLocation.Enabled = false;
            this.lblLaptopLocation.Location = new System.Drawing.Point(448, 293);
            this.lblLaptopLocation.Name = "lblLaptopLocation";
            this.lblLaptopLocation.Size = new System.Drawing.Size(114, 17);
            this.lblLaptopLocation.TabIndex = 27;
            this.lblLaptopLocation.Text = "Laptop Location:";
            // 
            // tbPMPLocation
            // 
            this.tbPMPLocation.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPMPLocation.Enabled = false;
            this.tbPMPLocation.Location = new System.Drawing.Point(451, 149);
            this.tbPMPLocation.Name = "tbPMPLocation";
            this.tbPMPLocation.ReadOnly = true;
            this.tbPMPLocation.Size = new System.Drawing.Size(284, 22);
            this.tbPMPLocation.TabIndex = 26;
            this.tbPMPLocation.Click += new System.EventHandler(this.tbPMPLocation_Click);
            this.tbPMPLocation.TextChanged += new System.EventHandler(this.tbPMPLocation_TextChanged);
            // 
            // lblPMPLocation
            // 
            this.lblPMPLocation.AutoSize = true;
            this.lblPMPLocation.Enabled = false;
            this.lblPMPLocation.Location = new System.Drawing.Point(448, 130);
            this.lblPMPLocation.Name = "lblPMPLocation";
            this.lblPMPLocation.Size = new System.Drawing.Size(99, 17);
            this.lblPMPLocation.TabIndex = 25;
            this.lblPMPLocation.Text = "PMP Location:";
            // 
            // tbPMPVolumeLabel
            // 
            this.tbPMPVolumeLabel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPMPVolumeLabel.Location = new System.Drawing.Point(451, 51);
            this.tbPMPVolumeLabel.Name = "tbPMPVolumeLabel";
            this.tbPMPVolumeLabel.Size = new System.Drawing.Size(284, 22);
            this.tbPMPVolumeLabel.TabIndex = 24;
            this.tbPMPVolumeLabel.TextChanged += new System.EventHandler(this.tbPMPVolumeLabel_TextChanged);
            // 
            // lblPMPVolumeLabel
            // 
            this.lblPMPVolumeLabel.AutoSize = true;
            this.lblPMPVolumeLabel.Location = new System.Drawing.Point(448, 31);
            this.lblPMPVolumeLabel.Name = "lblPMPVolumeLabel";
            this.lblPMPVolumeLabel.Size = new System.Drawing.Size(131, 17);
            this.lblPMPVolumeLabel.TabIndex = 23;
            this.lblPMPVolumeLabel.Text = "PMP Volume Label:";
            // 
            // tbLaptopUsername
            // 
            this.tbLaptopUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbLaptopUsername.Location = new System.Drawing.Point(451, 241);
            this.tbLaptopUsername.Name = "tbLaptopUsername";
            this.tbLaptopUsername.Size = new System.Drawing.Size(284, 22);
            this.tbLaptopUsername.TabIndex = 22;
            this.tbLaptopUsername.TextChanged += new System.EventHandler(this.tbLaptopUsername_TextChanged);
            // 
            // lblLaptopUsername
            // 
            this.lblLaptopUsername.AutoSize = true;
            this.lblLaptopUsername.Location = new System.Drawing.Point(448, 221);
            this.lblLaptopUsername.Name = "lblLaptopUsername";
            this.lblLaptopUsername.Size = new System.Drawing.Size(125, 17);
            this.lblLaptopUsername.TabIndex = 21;
            this.lblLaptopUsername.Text = "Laptop Username:";
            // 
            // tbLaptopHostname
            // 
            this.tbLaptopHostname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbLaptopHostname.Location = new System.Drawing.Point(451, 199);
            this.tbLaptopHostname.Name = "tbLaptopHostname";
            this.tbLaptopHostname.Size = new System.Drawing.Size(284, 22);
            this.tbLaptopHostname.TabIndex = 20;
            this.tbLaptopHostname.TextChanged += new System.EventHandler(this.tbLaptopHostname_TextChanged);
            // 
            // lblLaptopHostname
            // 
            this.lblLaptopHostname.AutoSize = true;
            this.lblLaptopHostname.Location = new System.Drawing.Point(448, 179);
            this.lblLaptopHostname.Name = "lblLaptopHostname";
            this.lblLaptopHostname.Size = new System.Drawing.Size(124, 17);
            this.lblLaptopHostname.TabIndex = 19;
            this.lblLaptopHostname.Text = "Laptop Hostname:";
            // 
            // cbCheckPMPSystem
            // 
            this.cbCheckPMPSystem.AutoSize = true;
            this.cbCheckPMPSystem.Location = new System.Drawing.Point(451, 79);
            this.cbCheckPMPSystem.Name = "cbCheckPMPSystem";
            this.cbCheckPMPSystem.Size = new System.Drawing.Size(200, 21);
            this.cbCheckPMPSystem.TabIndex = 18;
            this.cbCheckPMPSystem.Text = "Check PMP for system files";
            this.cbCheckPMPSystem.UseVisualStyleBackColor = true;
            this.cbCheckPMPSystem.CheckedChanged += new System.EventHandler(this.cbCheckPMPSystem_CheckedChanged);
            // 
            // cbAskSync
            // 
            this.cbAskSync.AutoSize = true;
            this.cbAskSync.Location = new System.Drawing.Point(12, 413);
            this.cbAskSync.Name = "cbAskSync";
            this.cbAskSync.Size = new System.Drawing.Size(235, 21);
            this.cbAskSync.TabIndex = 17;
            this.cbAskSync.Text = "Always ask before synchronizing";
            this.cbAskSync.UseVisualStyleBackColor = true;
            this.cbAskSync.CheckedChanged += new System.EventHandler(this.cbAskSync_CheckedChanged);
            // 
            // cbPreventSynchingUpscaled
            // 
            this.cbPreventSynchingUpscaled.AutoSize = true;
            this.cbPreventSynchingUpscaled.Location = new System.Drawing.Point(12, 359);
            this.cbPreventSynchingUpscaled.Name = "cbPreventSynchingUpscaled";
            this.cbPreventSynchingUpscaled.Size = new System.Drawing.Size(464, 21);
            this.cbPreventSynchingUpscaled.TabIndex = 16;
            this.cbPreventSynchingUpscaled.Text = "Prevent synching upscaled audio files when no downscaled file exists";
            this.cbPreventSynchingUpscaled.UseVisualStyleBackColor = true;
            this.cbPreventSynchingUpscaled.CheckedChanged += new System.EventHandler(this.cbPreventSynchingUpscaled_CheckedChanged);
            // 
            // cbChiptunesLibrary
            // 
            this.cbChiptunesLibrary.AutoSize = true;
            this.cbChiptunesLibrary.Location = new System.Drawing.Point(12, 79);
            this.cbChiptunesLibrary.Name = "cbChiptunesLibrary";
            this.cbChiptunesLibrary.Size = new System.Drawing.Size(163, 21);
            this.cbChiptunesLibrary.TabIndex = 15;
            this.cbChiptunesLibrary.Text = "Use chiptunes library";
            this.cbChiptunesLibrary.UseVisualStyleBackColor = true;
            this.cbChiptunesLibrary.CheckedChanged += new System.EventHandler(this.cbChiptunesLibrary_CheckedChanged);
            // 
            // btnChiptunesLocation
            // 
            this.btnChiptunesLocation.Enabled = false;
            this.btnChiptunesLocation.Location = new System.Drawing.Point(302, 123);
            this.btnChiptunesLocation.Name = "btnChiptunesLocation";
            this.btnChiptunesLocation.Size = new System.Drawing.Size(31, 23);
            this.btnChiptunesLocation.TabIndex = 14;
            this.btnChiptunesLocation.Text = "...";
            this.btnChiptunesLocation.UseVisualStyleBackColor = true;
            this.btnChiptunesLocation.Click += new System.EventHandler(this.btnChiptunesLocation_Click);
            // 
            // lblChiptunesLocation
            // 
            this.lblChiptunesLocation.AutoSize = true;
            this.lblChiptunesLocation.Enabled = false;
            this.lblChiptunesLocation.Location = new System.Drawing.Point(9, 103);
            this.lblChiptunesLocation.Name = "lblChiptunesLocation";
            this.lblChiptunesLocation.Size = new System.Drawing.Size(181, 17);
            this.lblChiptunesLocation.TabIndex = 13;
            this.lblChiptunesLocation.Text = "Chiptunes Library Location:";
            // 
            // tbChiptunesLocation
            // 
            this.tbChiptunesLocation.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbChiptunesLocation.Enabled = false;
            this.tbChiptunesLocation.Location = new System.Drawing.Point(12, 123);
            this.tbChiptunesLocation.Name = "tbChiptunesLocation";
            this.tbChiptunesLocation.ReadOnly = true;
            this.tbChiptunesLocation.Size = new System.Drawing.Size(284, 22);
            this.tbChiptunesLocation.TabIndex = 12;
            this.tbChiptunesLocation.Click += new System.EventHandler(this.tbChiptunesLocation_Click);
            this.tbChiptunesLocation.TextChanged += new System.EventHandler(this.tbChiptunesLocation_TextChanged);
            // 
            // cbRemoveEmpty
            // 
            this.cbRemoveEmpty.AutoSize = true;
            this.cbRemoveEmpty.Location = new System.Drawing.Point(12, 223);
            this.cbRemoveEmpty.Name = "cbRemoveEmpty";
            this.cbRemoveEmpty.Size = new System.Drawing.Size(347, 21);
            this.cbRemoveEmpty.TabIndex = 11;
            this.cbRemoveEmpty.Text = "Remove empty directories from downscaled library";
            this.cbRemoveEmpty.UseVisualStyleBackColor = true;
            this.cbRemoveEmpty.CheckedChanged += new System.EventHandler(this.cbRemoveEmpty_CheckedChanged);
            // 
            // cbRemoveUnnecessary
            // 
            this.cbRemoveUnnecessary.AutoSize = true;
            this.cbRemoveUnnecessary.Location = new System.Drawing.Point(12, 278);
            this.cbRemoveUnnecessary.Name = "cbRemoveUnnecessary";
            this.cbRemoveUnnecessary.Size = new System.Drawing.Size(427, 21);
            this.cbRemoveUnnecessary.TabIndex = 10;
            this.cbRemoveUnnecessary.Text = "Remove unnecessary downscaled files from downscaled library";
            this.cbRemoveUnnecessary.UseVisualStyleBackColor = true;
            this.cbRemoveUnnecessary.CheckedChanged += new System.EventHandler(this.cbRemoveUnnecessary_CheckedChanged);
            // 
            // cbRemoveUnsupported
            // 
            this.cbRemoveUnsupported.AutoSize = true;
            this.cbRemoveUnsupported.Location = new System.Drawing.Point(12, 250);
            this.cbRemoveUnsupported.Name = "cbRemoveUnsupported";
            this.cbRemoveUnsupported.Size = new System.Drawing.Size(433, 21);
            this.cbRemoveUnsupported.TabIndex = 9;
            this.cbRemoveUnsupported.Text = "Remove non-audio or unsupported files from downscaled library";
            this.cbRemoveUnsupported.UseVisualStyleBackColor = true;
            this.cbRemoveUnsupported.CheckedChanged += new System.EventHandler(this.cbRemoveUnsupported_CheckedChanged);
            // 
            // cbShowImproper
            // 
            this.cbShowImproper.AutoSize = true;
            this.cbShowImproper.Location = new System.Drawing.Point(12, 332);
            this.cbShowImproper.Name = "cbShowImproper";
            this.cbShowImproper.Size = new System.Drawing.Size(430, 21);
            this.cbShowImproper.TabIndex = 8;
            this.cbShowImproper.Text = "Show files in downscaled library that are improperly downscaled";
            this.cbShowImproper.UseVisualStyleBackColor = true;
            this.cbShowImproper.CheckedChanged += new System.EventHandler(this.cbShowImproper_CheckedChanged);
            // 
            // cbAutoHandle
            // 
            this.cbAutoHandle.AutoSize = true;
            this.cbAutoHandle.Location = new System.Drawing.Point(12, 151);
            this.cbAutoHandle.Name = "cbAutoHandle";
            this.cbAutoHandle.Size = new System.Drawing.Size(281, 21);
            this.cbAutoHandle.TabIndex = 7;
            this.cbAutoHandle.Text = "Automatically handle downscaled library";
            this.cbAutoHandle.UseVisualStyleBackColor = true;
            this.cbAutoHandle.CheckedChanged += new System.EventHandler(this.cbAutoHandle_CheckedChanged);
            // 
            // cbRemoveImproper
            // 
            this.cbRemoveImproper.AutoSize = true;
            this.cbRemoveImproper.Location = new System.Drawing.Point(12, 305);
            this.cbRemoveImproper.Name = "cbRemoveImproper";
            this.cbRemoveImproper.Size = new System.Drawing.Size(412, 21);
            this.cbRemoveImproper.TabIndex = 6;
            this.cbRemoveImproper.Text = "Remove improperly downscaled files from downscaled library";
            this.cbRemoveImproper.UseVisualStyleBackColor = true;
            this.cbRemoveImproper.CheckedChanged += new System.EventHandler(this.cbRemoveImproper_CheckedChanged);
            // 
            // btnDownscaledLocation
            // 
            this.btnDownscaledLocation.Location = new System.Drawing.Point(302, 194);
            this.btnDownscaledLocation.Name = "btnDownscaledLocation";
            this.btnDownscaledLocation.Size = new System.Drawing.Size(31, 23);
            this.btnDownscaledLocation.TabIndex = 5;
            this.btnDownscaledLocation.Text = "...";
            this.btnDownscaledLocation.UseVisualStyleBackColor = true;
            this.btnDownscaledLocation.Click += new System.EventHandler(this.btnDownscaledLocation_Click);
            // 
            // tbDownscaledLocation
            // 
            this.tbDownscaledLocation.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbDownscaledLocation.Location = new System.Drawing.Point(12, 195);
            this.tbDownscaledLocation.Name = "tbDownscaledLocation";
            this.tbDownscaledLocation.ReadOnly = true;
            this.tbDownscaledLocation.Size = new System.Drawing.Size(284, 22);
            this.tbDownscaledLocation.TabIndex = 4;
            this.tbDownscaledLocation.Click += new System.EventHandler(this.tbDownscaledLocation_Click);
            this.tbDownscaledLocation.TextChanged += new System.EventHandler(this.tbDownscaledLocation_TextChanged);
            // 
            // lblDownscaledLocation
            // 
            this.lblDownscaledLocation.AutoSize = true;
            this.lblDownscaledLocation.Location = new System.Drawing.Point(9, 175);
            this.lblDownscaledLocation.Name = "lblDownscaledLocation";
            this.lblDownscaledLocation.Size = new System.Drawing.Size(194, 17);
            this.lblDownscaledLocation.TabIndex = 3;
            this.lblDownscaledLocation.Text = "Downscaled Library Location:";
            // 
            // btnLibraryLocation
            // 
            this.btnLibraryLocation.Location = new System.Drawing.Point(302, 50);
            this.btnLibraryLocation.Name = "btnLibraryLocation";
            this.btnLibraryLocation.Size = new System.Drawing.Size(31, 23);
            this.btnLibraryLocation.TabIndex = 2;
            this.btnLibraryLocation.Text = "...";
            this.btnLibraryLocation.UseVisualStyleBackColor = true;
            this.btnLibraryLocation.Click += new System.EventHandler(this.btnLibraryLocation_Click);
            // 
            // lblLibraryLocation
            // 
            this.lblLibraryLocation.AutoSize = true;
            this.lblLibraryLocation.Location = new System.Drawing.Point(9, 31);
            this.lblLibraryLocation.Name = "lblLibraryLocation";
            this.lblLibraryLocation.Size = new System.Drawing.Size(154, 17);
            this.lblLibraryLocation.TabIndex = 1;
            this.lblLibraryLocation.Text = "Music Library Location:";
            // 
            // tbLibraryLocation
            // 
            this.tbLibraryLocation.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbLibraryLocation.Location = new System.Drawing.Point(12, 51);
            this.tbLibraryLocation.Name = "tbLibraryLocation";
            this.tbLibraryLocation.ReadOnly = true;
            this.tbLibraryLocation.Size = new System.Drawing.Size(284, 22);
            this.tbLibraryLocation.TabIndex = 0;
            this.tbLibraryLocation.Click += new System.EventHandler(this.tbLibraryLocation_Click);
            this.tbLibraryLocation.TextChanged += new System.EventHandler(this.tbLibraryLocation_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 556);
            this.panel1.TabIndex = 9;
            this.panel1.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(7, 12);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(771, 537);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // PMPSyncBW
            // 
            this.PMPSyncBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PMPSyncBW_DoWork);
            // 
            // ScanBW
            // 
            this.ScanBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScanBW_DoWork);
            // 
            // LaptopSyncBW
            // 
            this.LaptopSyncBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LaptopSyncBW_DoWork);
            // 
            // PMPSyncTBW
            // 
            this.PMPSyncTBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PMPSyncTBW_DoWork);
            // 
            // LaptopSyncTBW
            // 
            this.LaptopSyncTBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LaptopSyncTBW_DoWork);
            // 
            // cbAutoExitOne
            // 
            this.cbAutoExitOne.AutoSize = true;
            this.cbAutoExitOne.Enabled = false;
            this.cbAutoExitOne.Location = new System.Drawing.Point(12, 467);
            this.cbAutoExitOne.Name = "cbAutoExitOne";
            this.cbAutoExitOne.Size = new System.Drawing.Size(518, 21);
            this.cbAutoExitOne.TabIndex = 37;
            this.cbAutoExitOne.Text = "Always automatically exit as long as at least one synchronization is successful";
            this.cbAutoExitOne.UseVisualStyleBackColor = true;
            this.cbAutoExitOne.CheckedChanged += new System.EventHandler(this.cbAutoExitOne_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(782, 555);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Chrononizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl.ResumeLayout(false);
            this.SyncTab.ResumeLayout(false);
            this.InfoTab.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.flpLibraryInfo.ResumeLayout(false);
            this.flpLibraryInfo.PerformLayout();
            this.PreferencesTab.ResumeLayout(false);
            this.PreferencesTab.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage SyncTab;
        private System.Windows.Forms.TabPage InfoTab;
        private System.Windows.Forms.Button btnSyncLaptop;
        private System.Windows.Forms.Button btnSyncPMP;
        private System.Windows.Forms.Button btnSyncBoth;
        private System.Windows.Forms.TabPage PreferencesTab;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.FlowLayoutPanel flpLibraryInfo;
        private System.Windows.Forms.Label lblLibrarySize;
        private System.Windows.Forms.Label lblLibraryBytes;
        private System.Windows.Forms.Label lblDownscaledBytes;
        private System.Windows.Forms.Label lblTotalBytes;
        private System.Windows.Forms.Label lblFileBreakdown;
        private System.Windows.Forms.Label lblFLACFiles;
        private System.Windows.Forms.Label lblMP3Files;
        private System.Windows.Forms.Label lblWMAFiles;
        private System.Windows.Forms.Label lblM4AFiles;
        private System.Windows.Forms.Label lblOGGFiles;
        private System.Windows.Forms.Label lblWAVFiles;
        private System.Windows.Forms.Label lblXMFiles;
        private System.Windows.Forms.Label lblMODFiles;
        private System.Windows.Forms.Label lblNSFFiles;
        private System.Windows.Forms.Label lblSongCount;
        private System.Windows.Forms.Label lblLibraryFiles;
        private System.Windows.Forms.Label lblDownscaledFiles;
        private System.Windows.Forms.Label lblChiptunesFiles;
        private System.Windows.Forms.Label lblTotalFiles;
        private System.Windows.Forms.ListBox lbNotDownscaled;
        private System.Windows.Forms.Label lblNotDownscaled;
        private System.Windows.Forms.Button btnLibraryLocation;
        private System.Windows.Forms.Label lblLibraryLocation;
        private System.Windows.Forms.TextBox tbLibraryLocation;
        private System.Windows.Forms.Button btnDownscaledLocation;
        private System.Windows.Forms.TextBox tbDownscaledLocation;
        private System.Windows.Forms.Label lblDownscaledLocation;
        private System.Windows.Forms.CheckBox cbRemoveImproper;
        private System.Windows.Forms.CheckBox cbAutoHandle;
        private System.Windows.Forms.CheckBox cbShowImproper;
        private System.Windows.Forms.CheckBox cbRemoveUnnecessary;
        private System.Windows.Forms.CheckBox cbRemoveUnsupported;
        private System.Windows.Forms.CheckBox cbRemoveEmpty;
        private System.Windows.Forms.Button btnChiptunesLocation;
        private System.Windows.Forms.Label lblChiptunesLocation;
        private System.Windows.Forms.TextBox tbChiptunesLocation;
        private System.Windows.Forms.CheckBox cbChiptunesLibrary;
        private System.Windows.Forms.Panel panel1;
        private System.ComponentModel.BackgroundWorker PMPSyncBW;
        private System.ComponentModel.BackgroundWorker LaptopSyncBW;
        public System.ComponentModel.BackgroundWorker ScanBW;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.ComponentModel.BackgroundWorker PMPSyncTBW;
        private System.ComponentModel.BackgroundWorker LaptopSyncTBW;
        private System.Windows.Forms.CheckBox cbPreventSynchingUpscaled;
        private System.Windows.Forms.TextBox tbLaptopLocation;
        private System.Windows.Forms.Label lblLaptopLocation;
        private System.Windows.Forms.TextBox tbPMPLocation;
        private System.Windows.Forms.Label lblPMPLocation;
        private System.Windows.Forms.TextBox tbPMPVolumeLabel;
        private System.Windows.Forms.Label lblPMPVolumeLabel;
        private System.Windows.Forms.TextBox tbLaptopUsername;
        private System.Windows.Forms.Label lblLaptopUsername;
        private System.Windows.Forms.TextBox tbLaptopHostname;
        private System.Windows.Forms.Label lblLaptopHostname;
        private System.Windows.Forms.CheckBox cbCheckPMPSystem;
        private System.Windows.Forms.CheckBox cbAskSync;
        private System.Windows.Forms.CheckBox cbOverrideLaptopPath;
        private System.Windows.Forms.CheckBox cbOverridePMPPath;
        private System.Windows.Forms.Button btnLaptopLocation;
        private System.Windows.Forms.Button btnPMPLocation;
        private System.Windows.Forms.CheckBox cbHideMediaArtLocal;
        private System.Windows.Forms.CheckBox cbAutoExit;
        private System.Windows.Forms.CheckBox cbAutoExitOne;
    }
}

