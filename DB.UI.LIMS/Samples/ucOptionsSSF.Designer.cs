namespace DB.UI.Samples
{
    partial class ucOptionsSSF
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucOptionsSSF));
            this.TS = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.OptionsBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.calcDensity = new System.Windows.Forms.ToolStripMenuItem();
            this.findRadius = new System.Windows.Forms.ToolStripMenuItem();
            this.findlength = new System.Windows.Forms.ToolStripMenuItem();
            this.loop = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.N4 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.doCK = new System.Windows.Forms.ToolStripMenuItem();
            this.doMatSSF = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.showMatSSF = new System.Windows.Forms.ToolStripMenuItem();
            this.showOther = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.connectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workOffline = new System.Windows.Forms.ToolStripMenuItem();
            this.SQL = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderPath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.TS.SuspendLayout();
            this.SuspendLayout();
            // 
            // TS
            // 
            this.TS.BackColor = System.Drawing.SystemColors.Menu;
            this.TS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator8,
            this.OptionsBtn,
            this.toolStripSeparator13,
            this.Save});
            this.TS.Location = new System.Drawing.Point(0, 0);
            this.TS.Name = "TS";
            this.TS.Size = new System.Drawing.Size(670, 115);
            this.TS.TabIndex = 6;
            this.TS.Text = "toolStrip4";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 115);
            // 
            // OptionsBtn
            // 
            this.OptionsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OptionsBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.calcDensity,
            this.findRadius,
            this.findlength,
            this.loop,
            this.AutoLoad,
            this.toolStripSeparator7,
            this.toolStripMenuItem4,
            this.N4,
            this.toolStripSeparator6,
            this.toolStripMenuItem1,
            this.doCK,
            this.doMatSSF,
            this.toolStripSeparator3,
            this.showMatSSF,
            this.showOther,
            this.toolStripSeparator9,
            this.connectionsToolStripMenuItem,
            this.workOffline,
            this.SQL,
            this.FolderPath});
            this.OptionsBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionsBtn.ForeColor = System.Drawing.Color.DarkOrange;
            this.OptionsBtn.Image = ((System.Drawing.Image)(resources.GetObject("OptionsBtn.Image")));
            this.OptionsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsBtn.Name = "OptionsBtn";
            this.OptionsBtn.Size = new System.Drawing.Size(108, 112);
            this.OptionsBtn.Text = "OPTIONS";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripMenuItem5.Size = new System.Drawing.Size(293, 30);
            this.toolStripMenuItem5.Text = "Hacks";
            // 
            // calcDensity
            // 
            this.calcDensity.Checked = true;
            this.calcDensity.CheckOnClick = true;
            this.calcDensity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.calcDensity.Name = "calcDensity";
            this.calcDensity.Size = new System.Drawing.Size(293, 30);
            this.calcDensity.Text = "Find Density";
            // 
            // findRadius
            // 
            this.findRadius.Checked = true;
            this.findRadius.CheckOnClick = true;
            this.findRadius.CheckState = System.Windows.Forms.CheckState.Checked;
            this.findRadius.Name = "findRadius";
            this.findRadius.Size = new System.Drawing.Size(293, 30);
            this.findRadius.Text = "Find Radius";
            // 
            // findlength
            // 
            this.findlength.Checked = true;
            this.findlength.CheckOnClick = true;
            this.findlength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.findlength.Name = "findlength";
            this.findlength.Size = new System.Drawing.Size(293, 30);
            this.findlength.Text = "Find Length";
            // 
            // loop
            // 
            this.loop.Checked = true;
            this.loop.CheckOnClick = true;
            this.loop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loop.Name = "loop";
            this.loop.Size = new System.Drawing.Size(293, 30);
            this.loop.Text = "Loop All Units";
            // 
            // AutoLoad
            // 
            this.AutoLoad.Checked = true;
            this.AutoLoad.CheckOnClick = true;
            this.AutoLoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoLoad.Name = "AutoLoad";
            this.AutoLoad.Size = new System.Drawing.Size(293, 30);
            this.AutoLoad.Text = "Load Last Project";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(290, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripMenuItem4.Size = new System.Drawing.Size(293, 30);
            this.toolStripMenuItem4.Text = "Rounding";
            // 
            // N4
            // 
            this.N4.BackColor = System.Drawing.Color.SandyBrown;
            this.N4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.N4.Name = "N4";
            this.N4.Size = new System.Drawing.Size(233, 33);
            this.N4.Text = "N4";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(290, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripMenuItem1.Size = new System.Drawing.Size(293, 30);
            this.toolStripMenuItem1.Text = "Engine";
            // 
            // doCK
            // 
            this.doCK.Checked = true;
            this.doCK.CheckOnClick = true;
            this.doCK.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doCK.Name = "doCK";
            this.doCK.Size = new System.Drawing.Size(293, 30);
            this.doCK.Text = "Do CK";
            // 
            // doMatSSF
            // 
            this.doMatSSF.Checked = true;
            this.doMatSSF.CheckOnClick = true;
            this.doMatSSF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doMatSSF.Name = "doMatSSF";
            this.doMatSSF.Size = new System.Drawing.Size(293, 30);
            this.doMatSSF.Text = "Do MatSSF";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(290, 6);
            // 
            // showMatSSF
            // 
            this.showMatSSF.Checked = true;
            this.showMatSSF.CheckOnClick = true;
            this.showMatSSF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMatSSF.Name = "showMatSSF";
            this.showMatSSF.Size = new System.Drawing.Size(293, 30);
            this.showMatSSF.Text = "Show MatSSF";
            // 
            // showOther
            // 
            this.showOther.Checked = true;
            this.showOther.CheckOnClick = true;
            this.showOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOther.Name = "showOther";
            this.showOther.Size = new System.Drawing.Size(293, 30);
            this.showOther.Text = "Show Other";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(290, 6);
            // 
            // connectionsToolStripMenuItem
            // 
            this.connectionsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionsToolStripMenuItem.Name = "connectionsToolStripMenuItem";
            this.connectionsToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.connectionsToolStripMenuItem.Size = new System.Drawing.Size(293, 30);
            this.connectionsToolStripMenuItem.Text = "Connections";
            // 
            // workOffline
            // 
            this.workOffline.Checked = true;
            this.workOffline.CheckOnClick = true;
            this.workOffline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.workOffline.Name = "workOffline";
            this.workOffline.Size = new System.Drawing.Size(293, 30);
            this.workOffline.Text = "Work Offline";
            // 
            // SQL
            // 
            this.SQL.Name = "SQL";
            this.SQL.Size = new System.Drawing.Size(293, 30);
            this.SQL.Text = "SQL";
            // 
            // FolderPath
            // 
            this.FolderPath.BackColor = System.Drawing.Color.SandyBrown;
            this.FolderPath.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Size = new System.Drawing.Size(233, 33);
            this.FolderPath.Text = "MatSSF Folder";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 115);
            // 
            // Save
            // 
            this.Save.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save.ForeColor = System.Drawing.Color.DarkOrange;
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(70, 115);
            this.Save.Text = "SAVE";
            // 
            // ucOptionsSSF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TS);
            this.Name = "ucOptionsSSF";
            this.Size = new System.Drawing.Size(670, 115);
            this.TS.ResumeLayout(false);
            this.TS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip TS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripDropDownButton OptionsBtn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem calcDensity;
        private System.Windows.Forms.ToolStripMenuItem loop;
        private System.Windows.Forms.ToolStripMenuItem AutoLoad;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripTextBox N4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem doCK;
        private System.Windows.Forms.ToolStripMenuItem doMatSSF;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem showMatSSF;
        private System.Windows.Forms.ToolStripMenuItem showOther;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem connectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workOffline;
        private System.Windows.Forms.ToolStripMenuItem SQL;
        private System.Windows.Forms.ToolStripTextBox FolderPath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem findRadius;
        private System.Windows.Forms.ToolStripMenuItem findlength;
    }
}
