namespace k0X
{
   partial class WatchersForm
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatchersForm));
          this.SchTimer = new System.Windows.Forms.Timer(this.components);
          this.TV = new System.Windows.Forms.TreeView();
          this.splitContainer1 = new System.Windows.Forms.SplitContainer();
          this.TLP = new System.Windows.Forms.TableLayoutPanel();
          this.TS2 = new System.Windows.Forms.ToolStrip();
          this.sched = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
          this.hide = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
          this.closelbl = new System.Windows.Forms.ToolStripButton();
          this.TS = new System.Windows.Forms.ToolStrip();
          this.start = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
          this.stop = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
          this.save = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.clear = new System.Windows.Forms.ToolStripButton();
          ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
          this.splitContainer1.Panel1.SuspendLayout();
          this.splitContainer1.Panel2.SuspendLayout();
          this.splitContainer1.SuspendLayout();
          this.TLP.SuspendLayout();
          this.TS2.SuspendLayout();
          this.TS.SuspendLayout();
          this.SuspendLayout();
          // 
          // SchTimer
          // 
          this.SchTimer.Enabled = true;
          this.SchTimer.Interval = 100000;
          this.SchTimer.Tick += new System.EventHandler(this.ScheduledAcqTimer_Tick);
          // 
          // TV
          // 
          this.TV.BackColor = System.Drawing.Color.MintCream;
          this.TV.Dock = System.Windows.Forms.DockStyle.Fill;
          this.TV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.TV.Location = new System.Drawing.Point(0, 0);
          this.TV.Margin = new System.Windows.Forms.Padding(2);
          this.TV.Name = "TV";
          this.TV.Size = new System.Drawing.Size(218, 169);
          this.TV.TabIndex = 0;
          this.TV.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TV_NodeMouseDoubleClick);
          // 
          // splitContainer1
          // 
          this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
          this.splitContainer1.Location = new System.Drawing.Point(0, 0);
          this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
          this.splitContainer1.Name = "splitContainer1";
          this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
          // 
          // splitContainer1.Panel1
          // 
          this.splitContainer1.Panel1.Controls.Add(this.TV);
          // 
          // splitContainer1.Panel2
          // 
          this.splitContainer1.Panel2.Controls.Add(this.TLP);
          this.splitContainer1.Size = new System.Drawing.Size(218, 227);
          this.splitContainer1.SplitterDistance = 169;
          this.splitContainer1.SplitterWidth = 3;
          this.splitContainer1.TabIndex = 1;
          // 
          // TLP
          // 
          this.TLP.ColumnCount = 1;
          this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.TLP.Controls.Add(this.TS2, 0, 1);
          this.TLP.Controls.Add(this.TS, 0, 0);
          this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
          this.TLP.Location = new System.Drawing.Point(0, 0);
          this.TLP.Margin = new System.Windows.Forms.Padding(2);
          this.TLP.Name = "TLP";
          this.TLP.RowCount = 2;
          this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.93221F));
          this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.06779F));
          this.TLP.Size = new System.Drawing.Size(218, 55);
          this.TLP.TabIndex = 1;
          // 
          // TS2
          // 
          this.TS2.BackColor = System.Drawing.Color.LightGoldenrodYellow;
          this.TS2.Dock = System.Windows.Forms.DockStyle.Fill;
          this.TS2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sched,
            this.toolStripSeparator2,
            this.hide,
            this.toolStripSeparator3,
            this.closelbl});
          this.TS2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
          this.TS2.Location = new System.Drawing.Point(0, 30);
          this.TS2.Name = "TS2";
          this.TS2.Size = new System.Drawing.Size(218, 25);
          this.TS2.TabIndex = 1;
          this.TS2.Text = "toolStrip1";
          // 
          // sched
          // 
          this.sched.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.sched.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
          this.sched.ForeColor = System.Drawing.Color.Indigo;
          this.sched.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.sched.Name = "sched";
          this.sched.Size = new System.Drawing.Size(66, 22);
          this.sched.Text = "Schedule";
          this.sched.Click += new System.EventHandler(this.Command_Click);
          // 
          // toolStripSeparator2
          // 
          this.toolStripSeparator2.Name = "toolStripSeparator2";
          this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
          // 
          // hide
          // 
          this.hide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.hide.ForeColor = System.Drawing.Color.DarkGoldenrod;
          this.hide.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.hide.Name = "hide";
          this.hide.Size = new System.Drawing.Size(36, 22);
          this.hide.Text = "Hide";
          this.hide.ToolTipText = "Hides the selected Detectors Watchers. Closing this window does the same for all " +
              "opened detectors";
          this.hide.Click += new System.EventHandler(this.Command_Click);
          // 
          // toolStripSeparator3
          // 
          this.toolStripSeparator3.Name = "toolStripSeparator3";
          this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
          // 
          // closelbl
          // 
          this.closelbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.closelbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
          this.closelbl.ForeColor = System.Drawing.Color.Fuchsia;
          this.closelbl.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.closelbl.Name = "closelbl";
          this.closelbl.Size = new System.Drawing.Size(44, 22);
          this.closelbl.Text = "Close";
          this.closelbl.Click += new System.EventHandler(this.Command_Click);
          // 
          // TS
          // 
          this.TS.Dock = System.Windows.Forms.DockStyle.Fill;
          this.TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.start,
            this.toolStripSeparator6,
            this.stop,
            this.toolStripSeparator7,
            this.save,
            this.toolStripSeparator4,
            this.toolStripSeparator5,
            this.toolStripSeparator1,
            this.clear});
          this.TS.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
          this.TS.Location = new System.Drawing.Point(0, 0);
          this.TS.Name = "TS";
          this.TS.Size = new System.Drawing.Size(218, 30);
          this.TS.TabIndex = 0;
          this.TS.Text = "TS";
          // 
          // start
          // 
          this.start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.start.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.start.ForeColor = System.Drawing.Color.Green;
          this.start.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.start.Name = "start";
          this.start.Size = new System.Drawing.Size(41, 27);
          this.start.Text = "Start";
          this.start.ToolTipText = "Start selected detectors";
          this.start.Click += new System.EventHandler(this.Command_Click);
          // 
          // toolStripSeparator6
          // 
          this.toolStripSeparator6.Name = "toolStripSeparator6";
          this.toolStripSeparator6.Size = new System.Drawing.Size(6, 30);
          // 
          // stop
          // 
          this.stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.stop.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.stop.ForeColor = System.Drawing.Color.Red;
          this.stop.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.stop.Name = "stop";
          this.stop.Size = new System.Drawing.Size(40, 27);
          this.stop.Text = "Stop";
          this.stop.ToolTipText = "Stop selected detectors";
          this.stop.Click += new System.EventHandler(this.Command_Click);
          // 
          // toolStripSeparator7
          // 
          this.toolStripSeparator7.Name = "toolStripSeparator7";
          this.toolStripSeparator7.Size = new System.Drawing.Size(6, 30);
          // 
          // save
          // 
          this.save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.save.ForeColor = System.Drawing.Color.Blue;
          this.save.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.save.Name = "save";
          this.save.Size = new System.Drawing.Size(40, 27);
          this.save.Text = "Save";
          this.save.ToolTipText = "Save selected detectors";
          this.save.Click += new System.EventHandler(this.Command_Click);
          // 
          // toolStripSeparator4
          // 
          this.toolStripSeparator4.Name = "toolStripSeparator4";
          this.toolStripSeparator4.Size = new System.Drawing.Size(6, 30);
          // 
          // toolStripSeparator5
          // 
          this.toolStripSeparator5.Name = "toolStripSeparator5";
          this.toolStripSeparator5.Size = new System.Drawing.Size(6, 30);
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
          // 
          // clear
          // 
          this.clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.clear.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.clear.ForeColor = System.Drawing.Color.Black;
          this.clear.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.clear.Name = "clear";
          this.clear.Size = new System.Drawing.Size(43, 27);
          this.clear.Text = "Clear";
          this.clear.ToolTipText = "Clear selected detectors";
          this.clear.Click += new System.EventHandler(this.Command_Click);
          // 
          // WatchersForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
          this.ClientSize = new System.Drawing.Size(218, 227);
          this.Controls.Add(this.splitContainer1);
          this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "WatchersForm";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.Text = "Detectors";
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WatchersForm_FormClosing);
          this.splitContainer1.Panel1.ResumeLayout(false);
          this.splitContainer1.Panel2.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
          this.splitContainer1.ResumeLayout(false);
          this.TLP.ResumeLayout(false);
          this.TLP.PerformLayout();
          this.TS2.ResumeLayout(false);
          this.TS2.PerformLayout();
          this.TS.ResumeLayout(false);
          this.TS.PerformLayout();
          this.ResumeLayout(false);

	  }

	  #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.TableLayoutPanel TLP;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
      public System.Windows.Forms.TreeView TV;
      protected internal System.Windows.Forms.Timer SchTimer;
      protected internal System.Windows.Forms.ToolStrip TS;
    
      public System.Windows.Forms.ToolStripButton closelbl;
      protected internal System.Windows.Forms.ToolStrip TS2;
      public System.Windows.Forms.ToolStripButton sched;
      public System.Windows.Forms.ToolStripButton hide;
      protected internal System.Windows.Forms.ToolStripButton start;
      protected internal System.Windows.Forms.ToolStripButton stop;
      protected internal System.Windows.Forms.ToolStripButton clear;
      protected internal System.Windows.Forms.ToolStripButton save;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
   }
}

