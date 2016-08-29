namespace k0X
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LoadWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Release = new System.Windows.Forms.ToolStripMenuItem();
            this.BugReportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearLinaa = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.OtherMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.buffered = new System.Windows.Forms.ToolStripMenuItem();
            this.mimetic = new System.Windows.Forms.ToolStripMenuItem();
            this.Clone = new System.Windows.Forms.ToolStripMenuItem();
            this.EmailerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.autoload = new System.Windows.Forms.ToolStripMenuItem();
            this.fillbyHL = new System.Windows.Forms.ToolStripMenuItem();
            this.fillBySpectra = new System.Windows.Forms.ToolStripMenuItem();
            this.talkTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.Connections = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Help = new System.Windows.Forms.ToolStripMenuItem();
            this.About = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MS = new System.Windows.Forms.MenuStrip();
            this.LIMSData = new System.Windows.Forms.ToolStripMenuItem();
            this.Detectors = new System.Windows.Forms.ToolStripMenuItem();
            this.ExplorerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Analysis = new System.Windows.Forms.ToolStripMenuItem();
            this.HyperLabData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToDoPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.SolCoiPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.MatSSFPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.SC = new System.Windows.Forms.SplitContainer();
            this.Box = new System.Windows.Forms.ComboBox();
            this.CMS.SuspendLayout();
            this.MS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC)).BeginInit();
            this.SC.Panel1.SuspendLayout();
            this.SC.Panel2.SuspendLayout();
            this.SC.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox2,
            this.toolStripTextBox3});
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(13, 22);
            // 
            // notify
            // 
            this.notify.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notify.ContextMenuStrip = this.CMS;
            this.notify.Icon = ((System.Drawing.Icon)(resources.GetObject("notify.Icon")));
            this.notify.Visible = true;
            this.notify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notify_MouseClick);
            // 
            // CMS
            // 
            this.CMS.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CMS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CMS.BackgroundImage")));
            this.CMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadWorkspace,
            this.SaveWorkspace,
            this.toolStripSeparator4,
            this.Release,
            this.BugReportMenu,
            this.ClearLinaa,
            this.toolStripSeparator3,
            this.OtherMenu,
            this.talkTSMI,
            this.Connections,
            this.toolStripSeparator1,
            this.Help,
            this.About,
            this.quitToolStripMenuItem});
            this.CMS.Name = "CMS";
            this.CMS.Size = new System.Drawing.Size(222, 264);
            // 
            // LoadWorkspace
            // 
            this.LoadWorkspace.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadWorkspace.ForeColor = System.Drawing.Color.Blue;
            this.LoadWorkspace.Name = "LoadWorkspace";
            this.LoadWorkspace.Size = new System.Drawing.Size(221, 22);
            this.LoadWorkspace.Text = "&Load Workspace";
            this.LoadWorkspace.Click += new System.EventHandler(this.LoadWorkspace_Click);
            // 
            // SaveWorkspace
            // 
            this.SaveWorkspace.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveWorkspace.ForeColor = System.Drawing.Color.Fuchsia;
            this.SaveWorkspace.Name = "SaveWorkspace";
            this.SaveWorkspace.Size = new System.Drawing.Size(221, 22);
            this.SaveWorkspace.Text = "&Save Workspace";
            this.SaveWorkspace.Click += new System.EventHandler(this.SaveWorkspace_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(218, 6);
            // 
            // Release
            // 
            this.Release.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Release.Name = "Release";
            this.Release.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.Release.Size = new System.Drawing.Size(221, 22);
            this.Release.Text = "&Release Memory";
            this.Release.Click += new System.EventHandler(this.releaseMemory_Click);
            // 
            // BugReportMenu
            // 
            this.BugReportMenu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BugReportMenu.Name = "BugReportMenu";
            this.BugReportMenu.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.BugReportMenu.Size = new System.Drawing.Size(221, 22);
            this.BugReportMenu.Text = "Send &Bug Report";
            this.BugReportMenu.Click += new System.EventHandler(this.BugReportMenu_Click);
            // 
            // ClearLinaa
            // 
            this.ClearLinaa.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearLinaa.Name = "ClearLinaa";
            this.ClearLinaa.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.ClearLinaa.Size = new System.Drawing.Size(221, 22);
            this.ClearLinaa.Text = "&Clear";
            this.ClearLinaa.Click += new System.EventHandler(this.ClearLinaa_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(218, 6);
            // 
            // OtherMenu
            // 
            this.OtherMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buffered,
            this.mimetic,
            this.Clone,
            this.EmailerMenu,
            this.autoload,
            this.fillbyHL,
            this.fillBySpectra});
            this.OtherMenu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OtherMenu.ForeColor = System.Drawing.Color.DodgerBlue;
            this.OtherMenu.Name = "OtherMenu";
            this.OtherMenu.Size = new System.Drawing.Size(221, 22);
            this.OtherMenu.Text = "&Other";
            this.OtherMenu.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // buffered
            // 
            this.buffered.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buffered.Name = "buffered";
            this.buffered.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.buffered.Size = new System.Drawing.Size(272, 22);
            this.buffered.Text = "Check Buffered";
            this.buffered.Click += new System.EventHandler(this.buffered_Click);
            // 
            // mimetic
            // 
            this.mimetic.CheckOnClick = true;
            this.mimetic.Name = "mimetic";
            this.mimetic.Size = new System.Drawing.Size(272, 22);
            this.mimetic.Text = "Mimetic Style";
            this.mimetic.CheckedChanged += new System.EventHandler(this.mimetic_CheckedChanged);
            // 
            // Clone
            // 
            this.Clone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clone.Name = "Clone";
            this.Clone.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.Clone.Size = new System.Drawing.Size(272, 22);
            this.Clone.Text = "Clone";
            this.Clone.Click += new System.EventHandler(this.Clone_Click);
            // 
            // EmailerMenu
            // 
            this.EmailerMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailerMenu.Name = "EmailerMenu";
            this.EmailerMenu.Size = new System.Drawing.Size(272, 22);
            this.EmailerMenu.Text = "Send Email";
            this.EmailerMenu.Click += new System.EventHandler(this.EmailerMenu_Click);
            // 
            // autoload
            // 
            this.autoload.CheckOnClick = true;
            this.autoload.Name = "autoload";
            this.autoload.Size = new System.Drawing.Size(272, 22);
            this.autoload.Text = "Auto-load last project";
            this.autoload.CheckedChanged += new System.EventHandler(this.autoload_CheckedChanged);
            // 
            // fillbyHL
            // 
            this.fillbyHL.CheckOnClick = true;
            this.fillbyHL.Name = "fillbyHL";
            this.fillbyHL.Size = new System.Drawing.Size(272, 22);
            this.fillbyHL.Text = "Load Samples from HyperLab";
            this.fillbyHL.CheckedChanged += new System.EventHandler(this.autoload_CheckedChanged);
            // 
            // fillBySpectra
            // 
            this.fillBySpectra.CheckOnClick = true;
            this.fillBySpectra.Name = "fillBySpectra";
            this.fillBySpectra.Size = new System.Drawing.Size(272, 22);
            this.fillBySpectra.Text = "Load Samples from Spectra Directory";
            this.fillBySpectra.CheckedChanged += new System.EventHandler(this.autoload_CheckedChanged);
            // 
            // talkTSMI
            // 
            this.talkTSMI.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.talkTSMI.ForeColor = System.Drawing.Color.Navy;
            this.talkTSMI.Name = "talkTSMI";
            this.talkTSMI.Size = new System.Drawing.Size(221, 22);
            this.talkTSMI.Text = "Talk...";
            // 
            // Connections
            // 
            this.Connections.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connections.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Connections.Name = "Connections";
            this.Connections.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.Connections.Size = new System.Drawing.Size(221, 22);
            this.Connections.Text = "&Database Connections...";
            this.Connections.Click += new System.EventHandler(this.Connections_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(218, 6);
            // 
            // Help
            // 
            this.Help.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Help.Name = "Help";
            this.Help.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.Help.Size = new System.Drawing.Size(221, 22);
            this.Help.Text = "&Help";
            this.Help.Click += new System.EventHandler(this.Help_Click);
            // 
            // About
            // 
            this.About.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(221, 22);
            this.About.Text = "&About";
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.Quit_Click);
            // 
            // MS
            // 
            this.MS.AllowItemReorder = true;
            this.MS.AutoSize = false;
            this.MS.BackColor = System.Drawing.Color.Transparent;
            this.MS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MS.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LIMSData,
            this.Detectors,
            this.ExplorerMenu,
            this.Analysis,
            this.HyperLabData,
            this.ToDoPanel,
            this.SolCoiPanel,
            this.MatSSFPanel});
            this.MS.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.MS.Location = new System.Drawing.Point(0, 0);
            this.MS.MdiWindowListItem = this.OtherMenu;
            this.MS.Name = "MS";
            this.MS.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MS.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MS.Size = new System.Drawing.Size(309, 51);
            this.MS.TabIndex = 23;
            this.MS.Text = "Menu";
            // 
            // LIMSData
            // 
            this.LIMSData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LIMSData.ForeColor = System.Drawing.Color.Maroon;
            this.LIMSData.Name = "LIMSData";
            this.LIMSData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.LIMSData.Size = new System.Drawing.Size(47, 19);
            this.LIMSData.Text = "&LIMS";
            this.LIMSData.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.LIMSData.Click += new System.EventHandler(this.LIMSData_Click);
            // 
            // Detectors
            // 
            this.Detectors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detectors.ForeColor = System.Drawing.Color.DarkOrange;
            this.Detectors.Name = "Detectors";
            this.Detectors.Size = new System.Drawing.Size(75, 19);
            this.Detectors.Text = "&Detectors";
            // 
            // ExplorerMenu
            // 
            this.ExplorerMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExplorerMenu.ForeColor = System.Drawing.Color.Magenta;
            this.ExplorerMenu.Name = "ExplorerMenu";
            this.ExplorerMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.ExplorerMenu.ShowShortcutKeys = false;
            this.ExplorerMenu.Size = new System.Drawing.Size(66, 19);
            this.ExplorerMenu.Text = "Explorer";
            this.ExplorerMenu.Click += new System.EventHandler(this.ExplorerMenu_Click);
            // 
            // Analysis
            // 
            this.Analysis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analysis.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Analysis.Name = "Analysis";
            this.Analysis.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.Analysis.Size = new System.Drawing.Size(62, 19);
            this.Analysis.Text = "&Analysis";
            this.Analysis.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Analysis.Click += new System.EventHandler(this.Analysis_Click);
            // 
            // HyperLabData
            // 
            this.HyperLabData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HyperLabData.ForeColor = System.Drawing.Color.SeaGreen;
            this.HyperLabData.Name = "HyperLabData";
            this.HyperLabData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.HyperLabData.Size = new System.Drawing.Size(62, 19);
            this.HyperLabData.Text = "&Spectra";
            this.HyperLabData.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.HyperLabData.Click += new System.EventHandler(this.HyperLabData_Click);
            // 
            // ToDoPanel
            // 
            this.ToDoPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToDoPanel.ForeColor = System.Drawing.Color.Firebrick;
            this.ToDoPanel.Name = "ToDoPanel";
            this.ToDoPanel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.ToDoPanel.Size = new System.Drawing.Size(48, 19);
            this.ToDoPanel.Text = "To&Do";
            this.ToDoPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ToDoPanel.Click += new System.EventHandler(this.ToDoPanel_Click);
            // 
            // SolCoiPanel
            // 
            this.SolCoiPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SolCoiPanel.ForeColor = System.Drawing.Color.BlueViolet;
            this.SolCoiPanel.Name = "SolCoiPanel";
            this.SolCoiPanel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.SolCoiPanel.Size = new System.Drawing.Size(53, 19);
            this.SolCoiPanel.Text = "Sol&Coi";
            this.SolCoiPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.SolCoiPanel.Click += new System.EventHandler(this.SolCoiPanel_Click);
            // 
            // MatSSFPanel
            // 
            this.MatSSFPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MatSSFPanel.ForeColor = System.Drawing.Color.SkyBlue;
            this.MatSSFPanel.Name = "MatSSFPanel";
            this.MatSSFPanel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
            this.MatSSFPanel.Size = new System.Drawing.Size(61, 19);
            this.MatSSFPanel.Text = "&MatSSF";
            this.MatSSFPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.MatSSFPanel.Click += new System.EventHandler(this.MatSSFPanel_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 3600000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // SFD
            // 
            this.SFD.DefaultExt = "xml";
            this.SFD.FileName = "Workspace.xml";
            this.SFD.Filter = "Workspace (*.xml) | *.xml";
            this.SFD.RestoreDirectory = true;
            this.SFD.SupportMultiDottedExtensions = true;
            this.SFD.Title = "Save Workspace As...";
            this.SFD.FileOk += new System.ComponentModel.CancelEventHandler(this.SFD_FileOk);
            // 
            // OFD
            // 
            this.OFD.FileName = "Workspace.xml";
            this.OFD.Filter = "Workspace (*.xml) | *.xml";
            this.OFD.RestoreDirectory = true;
            this.OFD.SupportMultiDottedExtensions = true;
            this.OFD.Title = "Open Workspace...";
            this.OFD.FileOk += new System.ComponentModel.CancelEventHandler(this.OFD_FileOk);
            // 
            // SC
            // 
            this.SC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SC.Location = new System.Drawing.Point(0, 0);
            this.SC.Margin = new System.Windows.Forms.Padding(4);
            this.SC.Name = "SC";
            this.SC.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SC.Panel1
            // 
            this.SC.Panel1.Controls.Add(this.Box);
            // 
            // SC.Panel2
            // 
            this.SC.Panel2.Controls.Add(this.MS);
            this.SC.Size = new System.Drawing.Size(309, 98);
            this.SC.SplitterDistance = 44;
            this.SC.SplitterWidth = 3;
            this.SC.TabIndex = 24;
            // 
            // Box
            // 
            this.Box.BackColor = System.Drawing.Color.RosyBrown;
            this.Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Box.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box.ForeColor = System.Drawing.SystemColors.Window;
            this.Box.FormattingEnabled = true;
            this.Box.Location = new System.Drawing.Point(0, 0);
            this.Box.Margin = new System.Windows.Forms.Padding(4);
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(309, 33);
            this.Box.Sorted = true;
            this.Box.TabIndex = 0;
            this.Box.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Box_KeyUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(309, 98);
            this.ControlBox = false;
            this.Controls.Add(this.SC);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(1200, 450);
            this.MainMenuStrip = this.MS;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Opacity = 0.8D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.CMS.ResumeLayout(false);
            this.MS.ResumeLayout(false);
            this.MS.PerformLayout();
            this.SC.Panel1.ResumeLayout(false);
            this.SC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SC)).EndInit();
            this.SC.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
	
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripTextBox1;
		public System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.ToolStripMenuItem HyperLabData;
		private System.Windows.Forms.ToolStripMenuItem Analysis;
        private System.Windows.Forms.ToolStripMenuItem OtherMenu;
        private System.Windows.Forms.ToolStripMenuItem Clone;
        private System.Windows.Forms.ToolStripMenuItem About;
        private System.Windows.Forms.ToolStripMenuItem Help;
        private System.Windows.Forms.MenuStrip MS;
		private System.Windows.Forms.ToolStripMenuItem ToDoPanel;
        private System.Windows.Forms.ToolStripMenuItem LIMSData;
		private System.Windows.Forms.ToolStripMenuItem MatSSFPanel;
        private System.Windows.Forms.ToolStripMenuItem SolCoiPanel;
        private System.Windows.Forms.ToolStripMenuItem Release;
		private System.Windows.Forms.ToolStripMenuItem Connections;
		private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem ClearLinaa;
		private System.Windows.Forms.ToolStripMenuItem ExplorerMenu;
		private System.Windows.Forms.ToolStripMenuItem EmailerMenu;
		public System.Windows.Forms.ToolStripMenuItem BugReportMenu;
		private System.Windows.Forms.ContextMenuStrip CMS;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mimetic;

		private System.Windows.Forms.ToolStripMenuItem buffered;
		private System.Windows.Forms.ToolStripMenuItem Detectors;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.SaveFileDialog SFD;
		private System.Windows.Forms.OpenFileDialog OFD;
		private System.Windows.Forms.ToolStripMenuItem LoadWorkspace;
		private System.Windows.Forms.ToolStripMenuItem SaveWorkspace;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem talkTSMI;
		private System.Windows.Forms.SplitContainer SC;
		public System.Windows.Forms.ComboBox Box;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem autoload;
        private System.Windows.Forms.ToolStripMenuItem fillbyHL;
        private System.Windows.Forms.ToolStripMenuItem fillBySpectra;
     
      
        }
}

