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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSyncLaptop = new System.Windows.Forms.Button();
            this.btnSyncPMP = new System.Windows.Forms.Button();
            this.btnSyncBoth = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.PMPSyncBW = new System.ComponentModel.BackgroundWorker();
            this.ScanBW = new System.ComponentModel.BackgroundWorker();
            this.LaptopSyncBW = new System.ComponentModel.BackgroundWorker();
            this.PMPSyncTBW = new System.ComponentModel.BackgroundWorker();
            this.LaptopSyncTBW = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(785, 556);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSyncLaptop);
            this.tabPage1.Controls.Add(this.btnSyncPMP);
            this.tabPage1.Controls.Add(this.btnSyncBoth);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(777, 527);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Synchronize";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Controls.Add(this.btnScan);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(777, 527);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Library Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblNotDownscaled);
            this.splitContainer1.Panel2.Controls.Add(this.lbNotDownscaled);
            this.splitContainer1.Size = new System.Drawing.Size(757, 391);
            this.splitContainer1.SplitterDistance = 340;
            this.splitContainer1.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblLibrarySize);
            this.flowLayoutPanel1.Controls.Add(this.lblLibraryBytes);
            this.flowLayoutPanel1.Controls.Add(this.lblDownscaledBytes);
            this.flowLayoutPanel1.Controls.Add(this.lblTotalBytes);
            this.flowLayoutPanel1.Controls.Add(this.lblFileBreakdown);
            this.flowLayoutPanel1.Controls.Add(this.lblFLACFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblMP3Files);
            this.flowLayoutPanel1.Controls.Add(this.lblM4AFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblWMAFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblOGGFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblWAVFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblXMFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblMODFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblNSFFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblSongCount);
            this.flowLayoutPanel1.Controls.Add(this.lblLibraryFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblDownscaledFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblChiptunesFiles);
            this.flowLayoutPanel1.Controls.Add(this.lblTotalFiles);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(334, 385);
            this.flowLayoutPanel1.TabIndex = 4;
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
            this.lbNotDownscaled.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox7);
            this.tabPage3.Controls.Add(this.button6);
            this.tabPage3.Controls.Add(this.label23);
            this.tabPage3.Controls.Add(this.textBox3);
            this.tabPage3.Controls.Add(this.checkBox6);
            this.tabPage3.Controls.Add(this.checkBox5);
            this.tabPage3.Controls.Add(this.checkBox4);
            this.tabPage3.Controls.Add(this.checkBox3);
            this.tabPage3.Controls.Add(this.checkBox2);
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.button5);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.label22);
            this.tabPage3.Controls.Add(this.button4);
            this.tabPage3.Controls.Add(this.label21);
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(777, 527);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Preferences";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(11, 59);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(163, 21);
            this.checkBox7.TabIndex = 15;
            this.checkBox7.Text = "Use chiptunes library";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(301, 103);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(31, 23);
            this.button6.TabIndex = 14;
            this.button6.Text = "...";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Enabled = false;
            this.label23.Location = new System.Drawing.Point(8, 83);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(181, 17);
            this.label23.TabIndex = 13;
            this.label23.Text = "Chiptunes Library Location:";
            // 
            // textBox3
            // 
            this.textBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(11, 103);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(284, 22);
            this.textBox3.TabIndex = 12;
            this.textBox3.Click += new System.EventHandler(this.textBox3_Click);
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(11, 203);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(347, 21);
            this.checkBox6.TabIndex = 11;
            this.checkBox6.Text = "Remove empty directories from downscaled library";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(11, 258);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(427, 21);
            this.checkBox5.TabIndex = 10;
            this.checkBox5.Text = "Remove unnecessary downscaled files from downscaled library";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(11, 230);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(433, 21);
            this.checkBox4.TabIndex = 9;
            this.checkBox4.Text = "Remove non-audio or unsupported files from downscaled library";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(11, 312);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(430, 21);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "Show files in downscaled library that are improperly downscaled";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(11, 131);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(281, 21);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Automatically handle downscaled library";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 285);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(412, 21);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Remove improperly downscaled files from downscaled library";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(301, 174);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(31, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox2
            // 
            this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox2.Location = new System.Drawing.Point(11, 175);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(284, 22);
            this.textBox2.TabIndex = 4;
            this.textBox2.Click += new System.EventHandler(this.textBox2_Click);
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 155);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(194, 17);
            this.label22.TabIndex = 3;
            this.label22.Text = "Downscaled Library Location:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(301, 32);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(31, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 11);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(154, 17);
            this.label21.TabIndex = 1;
            this.label21.Text = "Music Library Location:";
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Location = new System.Drawing.Point(11, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(284, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(782, 555);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Chrononizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSyncLaptop;
        private System.Windows.Forms.Button btnSyncPMP;
        private System.Windows.Forms.Button btnSyncBoth;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
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
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.Panel panel1;
        private System.ComponentModel.BackgroundWorker PMPSyncBW;
        private System.ComponentModel.BackgroundWorker LaptopSyncBW;
        public System.ComponentModel.BackgroundWorker ScanBW;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.ComponentModel.BackgroundWorker PMPSyncTBW;
        private System.ComponentModel.BackgroundWorker LaptopSyncTBW;
    }
}

