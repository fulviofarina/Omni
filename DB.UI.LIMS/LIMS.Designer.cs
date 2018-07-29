namespace DB.UI
{
    partial class LIMS
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Geometries", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Samples", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Irradiations", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Vials", 3);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Sub Samples", 2);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Standards", 6);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Samples", 9);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Reference Materials", 7);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Orders", 34);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Monitors",
            "Standards",
            "Monitors",
            "Reference Materials",
            "Blanks"}, 5);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Matrices", 1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Irradiations", 32);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "Geometries"}, 10, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Detectors", 21);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("Channels", 35);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("Blanks");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LIMS));
            this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshT = new System.Windows.Forms.ToolStripMenuItem();
            this.undoT = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.setVials = new System.Windows.Forms.ToolStripMenuItem();
            this.setMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.setGeometry = new System.Windows.Forms.ToolStripMenuItem();
            this.setRabbit = new System.Windows.Forms.ToolStripMenuItem();
            this.setIrradCh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.setMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.setStandards = new System.Windows.Forms.ToolStripMenuItem();
            this.setSubSamples = new System.Windows.Forms.ToolStripMenuItem();
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.predictToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.halflife = new System.Windows.Forms.ToolStripMenuItem();
            this.setTodaysDate = new System.Windows.Forms.ToolStripMenuItem();
            this.shareTirr = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIrradiationDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.unlockProtectedCellsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.Modules = new System.Windows.Forms.ListView();
            this.images = new System.Windows.Forms.ImageList(this.components);
            this.GAlgo = new System.Windows.Forms.ToolStripMenuItem();
            this.CMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // CMS
            // 
            this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshT,
            this.undoT,
            this.findToolStripMenuItem,
            this.toolStripSeparator3,
            this.setVials,
            this.setMatrix,
            this.setGeometry,
            this.setRabbit,
            this.setIrradCh,
            this.toolStripSeparator2,
            this.setMonitor,
            this.setStandards,
            this.setSubSamples,
            this.aToolStripMenuItem,
            this.GAlgo,
            this.predictToolStripMenuItem,
            this.toolStripSeparator1,
            this.halflife,
            this.setTodaysDate,
            this.shareTirr,
            this.updateIrradiationDateToolStripMenuItem,
            this.toolStripSeparator4,
            this.unlockProtectedCellsToolStripMenuItem,
            this.toolStripSeparator5});
            this.CMS.Name = "CMS";
            this.CMS.Size = new System.Drawing.Size(333, 530);
            // 
            // refreshT
            // 
            this.refreshT.Name = "refreshT";
            this.refreshT.Size = new System.Drawing.Size(332, 26);
            this.refreshT.Text = "Refresh";
       
            // 
            // undoT
            // 
            this.undoT.Name = "undoT";
            this.undoT.Size = new System.Drawing.Size(332, 26);
            this.undoT.Text = "Undo";
         
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.ForeColor = System.Drawing.Color.Maroon;
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(332, 26);
            this.findToolStripMenuItem.Text = "Find...";
        
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(329, 6);
            // 
            // setVials
            // 
            this.setVials.Name = "setVials";
            this.setVials.Size = new System.Drawing.Size(332, 26);
            this.setVials.Text = "Set Vial";
            // 
            // setMatrix
            // 
            this.setMatrix.Name = "setMatrix";
            this.setMatrix.Size = new System.Drawing.Size(332, 26);
            this.setMatrix.Text = "Set Matrix";
            // 
            // setGeometry
            // 
            this.setGeometry.Name = "setGeometry";
            this.setGeometry.Size = new System.Drawing.Size(332, 26);
            this.setGeometry.Text = "Set Geometry";
            // 
            // setRabbit
            // 
            this.setRabbit.Name = "setRabbit";
            this.setRabbit.Size = new System.Drawing.Size(332, 26);
            this.setRabbit.Text = "Set Rabbit";
            // 
            // setIrradCh
            // 
            this.setIrradCh.Name = "setIrradCh";
            this.setIrradCh.Size = new System.Drawing.Size(332, 26);
            this.setIrradCh.Text = "Set Irradiation Channel";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(329, 6);
            // 
            // setMonitor
            // 
            this.setMonitor.Name = "setMonitor";
            this.setMonitor.Size = new System.Drawing.Size(332, 26);
            this.setMonitor.Text = "Set Monitor";
            // 
            // setStandards
            // 
            this.setStandards.Name = "setStandards";
            this.setStandards.Size = new System.Drawing.Size(332, 26);
            this.setStandards.Text = "Set Standard";
            // 
            // setSubSamples
            // 
            this.setSubSamples.Enabled = false;
            this.setSubSamples.Name = "setSubSamples";
            this.setSubSamples.Size = new System.Drawing.Size(332, 26);
            this.setSubSamples.Text = "Set SubSamples";
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(329, 6);
            // 
            // predictToolStripMenuItem
            // 
            this.predictToolStripMenuItem.Name = "predictToolStripMenuItem";
            this.predictToolStripMenuItem.Size = new System.Drawing.Size(332, 26);
            this.predictToolStripMenuItem.Text = "Predict (beta)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(329, 6);
            // 
            // halflife
            // 
            this.halflife.Name = "halflife";
            this.halflife.Size = new System.Drawing.Size(332, 26);
            this.halflife.Text = "Set Half-lives";
            // 
            // setTodaysDate
            // 
            this.setTodaysDate.ForeColor = System.Drawing.Color.Teal;
            this.setTodaysDate.Name = "setTodaysDate";
            this.setTodaysDate.Size = new System.Drawing.Size(332, 26);
            this.setTodaysDate.Text = "Set Today\'s date!";
       
            // 
            // shareTirr
            // 
            this.shareTirr.ForeColor = System.Drawing.Color.Brown;
            this.shareTirr.Name = "shareTirr";
            this.shareTirr.Size = new System.Drawing.Size(332, 26);
            this.shareTirr.Text = "Propagate this Irradiation Date/Time";
            // 
            // updateIrradiationDateToolStripMenuItem
            // 
            this.updateIrradiationDateToolStripMenuItem.Name = "updateIrradiationDateToolStripMenuItem";
            this.updateIrradiationDateToolStripMenuItem.Size = new System.Drawing.Size(332, 26);
            this.updateIrradiationDateToolStripMenuItem.Text = "Update Irradiation Dates";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(329, 6);
            // 
            // unlockProtectedCellsToolStripMenuItem
            // 
            this.unlockProtectedCellsToolStripMenuItem.Name = "unlockProtectedCellsToolStripMenuItem";
            this.unlockProtectedCellsToolStripMenuItem.Size = new System.Drawing.Size(332, 26);
            this.unlockProtectedCellsToolStripMenuItem.Text = "Unlock/Lock protected cells";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(329, 6);
            // 
            // Modules
            // 
            this.Modules.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.Modules.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.Modules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Modules.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            listViewGroup1.Header = "Geometries";
            listViewGroup1.Name = "Geometries";
            listViewGroup2.Header = "Samples";
            listViewGroup2.Name = "Samples";
            listViewGroup3.Header = "Irradiations";
            listViewGroup3.Name = "Irradiations";
            this.Modules.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.Group = listViewGroup2;
            listViewItem3.Group = listViewGroup2;
            listViewItem4.Group = listViewGroup2;
            listViewItem5.Group = listViewGroup2;
            listViewItem6.Group = listViewGroup3;
            listViewItem7.Group = listViewGroup2;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.Group = listViewGroup1;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.Group = listViewGroup3;
            listViewItem10.Group = listViewGroup1;
            listViewItem10.StateImageIndex = 0;
            listViewItem11.Group = listViewGroup1;
            listViewItem12.Group = listViewGroup3;
            listViewItem12.StateImageIndex = 0;
            listViewItem13.Group = listViewGroup2;
            this.Modules.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13});
            this.Modules.LabelEdit = true;
            this.Modules.LargeImageList = this.images;
            this.Modules.Location = new System.Drawing.Point(0, 0);
            this.Modules.Name = "Modules";
            this.Modules.Size = new System.Drawing.Size(898, 386);
            this.Modules.TabIndex = 0;
            this.Modules.TileSize = new System.Drawing.Size(200, 60);
            this.Modules.UseCompatibleStateImageBehavior = false;
            this.Modules.View = System.Windows.Forms.View.Tile;
             // 
            // images
            // 
            this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
            this.images.TransparentColor = System.Drawing.Color.Transparent;
            this.images.Images.SetKeyName(0, "RadBlue.png");
            this.images.Images.SetKeyName(1, "Matrices.png");
            this.images.Images.SetKeyName(2, "Vials.png");
            this.images.Images.SetKeyName(3, "Geometries.png");
            this.images.Images.SetKeyName(4, "Atom.png");
            this.images.Images.SetKeyName(5, "Monitors.png");
            this.images.Images.SetKeyName(6, "Standards.png");
            this.images.Images.SetKeyName(7, "RefMat.png");
            this.images.Images.SetKeyName(8, "Worker.ico");
            this.images.Images.SetKeyName(9, "SubSamples.png");
            this.images.Images.SetKeyName(10, "trouble.png");
            this.images.Images.SetKeyName(11, "trouble.ico");
            this.images.Images.SetKeyName(12, "Nuclear (2).ico");
            this.images.Images.SetKeyName(13, "nuclear_waste_canister.png");
            this.images.Images.SetKeyName(14, "Radioactive2.ico");
            this.images.Images.SetKeyName(15, "Radioactive2.png");
            this.images.Images.SetKeyName(16, "Radioactive3.ico");
            this.images.Images.SetKeyName(17, "Radioactive3.png");
            this.images.Images.SetKeyName(18, "Clean.png");
            this.images.Images.SetKeyName(19, "Recycle.png");
            this.images.Images.SetKeyName(20, "Bomb.png");
            this.images.Images.SetKeyName(21, "Alarm.png");
            this.images.Images.SetKeyName(22, "Logo.png");
            this.images.Images.SetKeyName(23, "Nuclear.ico");
            this.images.Images.SetKeyName(24, "Browse.png");
            this.images.Images.SetKeyName(25, "Green.png");
            this.images.Images.SetKeyName(26, "Handpoint.png");
            this.images.Images.SetKeyName(27, "Radioactive.png");
            this.images.Images.SetKeyName(28, "Red.png");
            this.images.Images.SetKeyName(29, "SCK-CEN.png");
            this.images.Images.SetKeyName(30, "Labo.jpg");
            this.images.Images.SetKeyName(31, "Reactor.jpg");
            this.images.Images.SetKeyName(32, "Reaction.gif");
            this.images.Images.SetKeyName(33, "Worker2.jpg");
            this.images.Images.SetKeyName(34, "Project.jpg");
            this.images.Images.SetKeyName(35, "Reactors.jpg");
            this.images.Images.SetKeyName(36, "Worker3.jpg");
            this.images.Images.SetKeyName(37, "Springfield.jpg");
            // 
            // GAlgo
            // 
            this.GAlgo.Name = "GAlgo";
            this.GAlgo.Size = new System.Drawing.Size(332, 26);
            this.GAlgo.Text = "Time Optimization (beta)";
            // 
            // LIMS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(898, 386);
            this.Controls.Add(this.Modules);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LIMS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LIMS";
       
            this.CMS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Modules;
        private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.ToolStripMenuItem shareTirr;
		private System.Windows.Forms.ToolStripMenuItem predictToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setVials;
		private System.Windows.Forms.ToolStripMenuItem setStandards;
		private System.Windows.Forms.ToolStripMenuItem setMonitor;
		private System.Windows.Forms.ToolStripMenuItem setSubSamples;
		private System.Windows.Forms.ToolStripMenuItem setGeometry;
		private System.Windows.Forms.ToolStripSeparator aToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem halflife;
		private System.Windows.Forms.ToolStripMenuItem setMatrix;
		private System.Windows.Forms.ToolStripMenuItem unlockProtectedCellsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updateIrradiationDateToolStripMenuItem;
		public System.Windows.Forms.ContextMenuStrip CMS;
		private System.Windows.Forms.ToolStripMenuItem refreshT;
		private System.Windows.Forms.ToolStripMenuItem undoT;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setTodaysDate;
        private System.Windows.Forms.ToolStripMenuItem setIrradCh;
        private System.Windows.Forms.ToolStripMenuItem setRabbit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem GAlgo;
    }
}