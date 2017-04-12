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
            this.preferencesTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.N4 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionsTSMI = new System.Windows.Forms.ToolStripMenuItem();
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
            this.preferencesTSMI,
            this.toolStripSeparator7,
            this.toolStripMenuItem4,
            this.N4,
            this.toolStripSeparator6,
            this.connectionsTSMI});
            this.OptionsBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionsBtn.ForeColor = System.Drawing.Color.DarkOrange;
            this.OptionsBtn.Image = ((System.Drawing.Image)(resources.GetObject("OptionsBtn.Image")));
            this.OptionsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsBtn.Name = "OptionsBtn";
            this.OptionsBtn.Size = new System.Drawing.Size(108, 112);
            this.OptionsBtn.Text = "OPTIONS";
            // 
            // preferencesTSMI
            // 
            this.preferencesTSMI.Name = "preferencesTSMI";
            this.preferencesTSMI.Size = new System.Drawing.Size(293, 30);
            this.preferencesTSMI.Text = "Preferences";
            this.preferencesTSMI.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
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
            // connectionsTSMI
            // 
            this.connectionsTSMI.Name = "connectionsTSMI";
            this.connectionsTSMI.Size = new System.Drawing.Size(293, 30);
            this.connectionsTSMI.Text = "Connections";
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripTextBox N4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem preferencesTSMI;
        private System.Windows.Forms.ToolStripMenuItem connectionsTSMI;
    }
}
