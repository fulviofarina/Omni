﻿namespace DB.UI
{
    partial class ucXCOMPreferences
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
            System.Windows.Forms.Label minAreaLabel;
            System.Windows.Forms.Label maxUncLabel;
            System.Windows.Forms.Label windowALabel;
            System.Windows.Forms.Label roundingLabel;
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.stepsBox = new System.Windows.Forms.TextBox();
            this.maxEneBox = new System.Windows.Forms.TextBox();
            this.minEneBox = new System.Windows.Forms.TextBox();
            this.roundingTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.logscaleBox = new System.Windows.Forms.CheckBox();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.forceBox = new System.Windows.Forms.CheckBox();
            this.useListbox = new System.Windows.Forms.CheckBox();
            this.asciibox = new System.Windows.Forms.CheckBox();
            this.overridesCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.isbox = new System.Windows.Forms.CheckBox();
            this.csbox = new System.Windows.Forms.CheckBox();
            this.pebox = new System.Windows.Forms.CheckBox();
            this.ppefbox = new System.Windows.Forms.CheckBox();
            this.ppnfbox = new System.Windows.Forms.CheckBox();
            this.totncs = new System.Windows.Forms.CheckBox();
            this.totcs = new System.Windows.Forms.CheckBox();
            minAreaLabel = new System.Windows.Forms.Label();
            maxUncLabel = new System.Windows.Forms.Label();
            windowALabel = new System.Windows.Forms.Label();
            roundingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // minAreaLabel
            // 
            minAreaLabel.AutoSize = true;
            minAreaLabel.ForeColor = System.Drawing.Color.DarkOrange;
            minAreaLabel.Location = new System.Drawing.Point(3, 21);
            minAreaLabel.Name = "minAreaLabel";
            minAreaLabel.Size = new System.Drawing.Size(137, 21);
            minAreaLabel.TabIndex = 4;
            minAreaLabel.Text = "Min. Energy (keV)";
            // 
            // maxUncLabel
            // 
            maxUncLabel.AutoSize = true;
            maxUncLabel.ForeColor = System.Drawing.Color.DarkOrange;
            maxUncLabel.Location = new System.Drawing.Point(3, 77);
            maxUncLabel.Name = "maxUncLabel";
            maxUncLabel.Size = new System.Drawing.Size(140, 21);
            maxUncLabel.TabIndex = 6;
            maxUncLabel.Text = "Max. Energy (keV)";
            // 
            // windowALabel
            // 
            windowALabel.AutoSize = true;
            windowALabel.ForeColor = System.Drawing.Color.DarkOrange;
            windowALabel.Location = new System.Drawing.Point(3, 133);
            windowALabel.Name = "windowALabel";
            windowALabel.Size = new System.Drawing.Size(85, 21);
            windowALabel.TabIndex = 8;
            windowALabel.Text = "Step (keV)";
            // 
            // roundingLabel
            // 
            roundingLabel.AutoSize = true;
            roundingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            roundingLabel.ForeColor = System.Drawing.Color.DarkOrange;
            roundingLabel.Location = new System.Drawing.Point(3, 189);
            roundingLabel.Name = "roundingLabel";
            roundingLabel.Size = new System.Drawing.Size(175, 21);
            roundingLabel.TabIndex = 10;
            roundingLabel.Text = "Rounding";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.ForeColor = System.Drawing.Color.White;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(479, 474);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(windowALabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.stepsBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(maxUncLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.maxEneBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(minAreaLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.minEneBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(roundingLabel, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.roundingTextBox, 0, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(181, 470);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "Energy Preferences";
            // 
            // stepsBox
            // 
            this.stepsBox.Location = new System.Drawing.Point(3, 157);
            this.stepsBox.Name = "stepsBox";
            this.stepsBox.Size = new System.Drawing.Size(100, 29);
            this.stepsBox.TabIndex = 9;
            // 
            // maxEneBox
            // 
            this.maxEneBox.Location = new System.Drawing.Point(3, 101);
            this.maxEneBox.Name = "maxEneBox";
            this.maxEneBox.Size = new System.Drawing.Size(100, 29);
            this.maxEneBox.TabIndex = 7;
            // 
            // minEneBox
            // 
            this.minEneBox.Location = new System.Drawing.Point(3, 45);
            this.minEneBox.Name = "minEneBox";
            this.minEneBox.Size = new System.Drawing.Size(100, 29);
            this.minEneBox.TabIndex = 5;
            // 
            // roundingTextBox
            // 
            this.roundingTextBox.Location = new System.Drawing.Point(3, 213);
            this.roundingTextBox.Name = "roundingTextBox";
            this.roundingTextBox.Size = new System.Drawing.Size(100, 29);
            this.roundingTextBox.TabIndex = 11;
            this.roundingTextBox.Text = "N4";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.totcs, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.totncs, 0, 14);
            this.tableLayoutPanel2.Controls.Add(this.ppnfbox, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.ppefbox, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.pebox, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.csbox, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.isbox, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.logscaleBox, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.loopCheckBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.forceBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.useListbox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.asciibox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.overridesCheckBox, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 15;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.14301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.140868F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(280, 470);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // logscaleBox
            // 
            this.logscaleBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logscaleBox.Enabled = false;
            this.logscaleBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logscaleBox.Location = new System.Drawing.Point(3, 183);
            this.logscaleBox.Name = "logscaleBox";
            this.logscaleBox.Size = new System.Drawing.Size(274, 26);
            this.logscaleBox.TabIndex = 15;
            this.logscaleBox.Text = "Log Scale (plot)";
            this.logscaleBox.UseVisualStyleBackColor = true;
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loopCheckBox.ForeColor = System.Drawing.Color.Khaki;
            this.loopCheckBox.Location = new System.Drawing.Point(3, 87);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(274, 26);
            this.loopCheckBox.TabIndex = 10;
            this.loopCheckBox.Text = "Process all units";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // forceBox
            // 
            this.forceBox.Checked = true;
            this.forceBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forceBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forceBox.ForeColor = System.Drawing.Color.Gold;
            this.forceBox.Location = new System.Drawing.Point(3, 55);
            this.forceBox.Name = "forceBox";
            this.forceBox.Size = new System.Drawing.Size(274, 26);
            this.forceBox.TabIndex = 8;
            this.forceBox.Text = "Force calculations";
            this.forceBox.UseVisualStyleBackColor = true;
            // 
            // useListbox
            // 
            this.useListbox.Checked = true;
            this.useListbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useListbox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useListbox.ForeColor = System.Drawing.Color.DarkKhaki;
            this.useListbox.Location = new System.Drawing.Point(3, 23);
            this.useListbox.Name = "useListbox";
            this.useListbox.Size = new System.Drawing.Size(274, 26);
            this.useListbox.TabIndex = 6;
            this.useListbox.Text = "Use my Energies List";
            this.useListbox.UseVisualStyleBackColor = true;
            // 
            // asciibox
            // 
            this.asciibox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.asciibox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asciibox.Location = new System.Drawing.Point(3, 119);
            this.asciibox.Name = "asciibox";
            this.asciibox.Size = new System.Drawing.Size(274, 26);
            this.asciibox.TabIndex = 2;
            this.asciibox.Text = "Output as CSV file";
            this.asciibox.UseVisualStyleBackColor = true;
            // 
            // overridesCheckBox
            // 
            this.overridesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overridesCheckBox.Enabled = false;
            this.overridesCheckBox.ForeColor = System.Drawing.Color.Red;
            this.overridesCheckBox.Location = new System.Drawing.Point(3, 151);
            this.overridesCheckBox.Name = "overridesCheckBox";
            this.overridesCheckBox.Size = new System.Drawing.Size(274, 26);
            this.overridesCheckBox.TabIndex = 13;
            this.overridesCheckBox.Text = "Expert Mode";
            this.overridesCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.Aqua;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Other Preferences";
            // 
            // isbox
            // 
            this.isbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.isbox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isbox.Location = new System.Drawing.Point(3, 247);
            this.isbox.Name = "isbox";
            this.isbox.Size = new System.Drawing.Size(274, 26);
            this.isbox.TabIndex = 16;
            this.isbox.Text = "Incoherent Scattering";
            this.isbox.UseVisualStyleBackColor = true;
            // 
            // csbox
            // 
            this.csbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csbox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.csbox.ForeColor = System.Drawing.Color.LightBlue;
            this.csbox.Location = new System.Drawing.Point(3, 279);
            this.csbox.Name = "csbox";
            this.csbox.Size = new System.Drawing.Size(274, 26);
            this.csbox.TabIndex = 17;
            this.csbox.Text = "Coherent Scattering";
            this.csbox.UseVisualStyleBackColor = true;
            // 
            // pebox
            // 
            this.pebox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pebox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pebox.ForeColor = System.Drawing.Color.MistyRose;
            this.pebox.Location = new System.Drawing.Point(3, 311);
            this.pebox.Name = "pebox";
            this.pebox.Size = new System.Drawing.Size(274, 26);
            this.pebox.TabIndex = 18;
            this.pebox.Text = "Photoelectric Effect";
            this.pebox.UseVisualStyleBackColor = true;
            // 
            // ppefbox
            // 
            this.ppefbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppefbox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ppefbox.ForeColor = System.Drawing.Color.LemonChiffon;
            this.ppefbox.Location = new System.Drawing.Point(3, 375);
            this.ppefbox.Name = "ppefbox";
            this.ppefbox.Size = new System.Drawing.Size(274, 26);
            this.ppefbox.TabIndex = 19;
            this.ppefbox.Text = "Pair Production (Electron Field)";
            this.ppefbox.UseVisualStyleBackColor = true;
            // 
            // ppnfbox
            // 
            this.ppnfbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppnfbox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ppnfbox.ForeColor = System.Drawing.Color.Khaki;
            this.ppnfbox.Location = new System.Drawing.Point(3, 343);
            this.ppnfbox.Name = "ppnfbox";
            this.ppnfbox.Size = new System.Drawing.Size(274, 26);
            this.ppnfbox.TabIndex = 20;
            this.ppnfbox.Text = "Pair Production (Neutron Field)";
            this.ppnfbox.UseVisualStyleBackColor = true;
            // 
            // totncs
            // 
            this.totncs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totncs.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totncs.Location = new System.Drawing.Point(3, 439);
            this.totncs.Name = "totncs";
            this.totncs.Size = new System.Drawing.Size(274, 28);
            this.totncs.TabIndex = 21;
            this.totncs.Text = "Total (w/o Coherent Scattering)";
            this.totncs.UseVisualStyleBackColor = true;
            // 
            // totcs
            // 
            this.totcs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totcs.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totcs.ForeColor = System.Drawing.Color.LightBlue;
            this.totcs.Location = new System.Drawing.Point(3, 407);
            this.totcs.Name = "totcs";
            this.totcs.Size = new System.Drawing.Size(274, 26);
            this.totcs.TabIndex = 22;
            this.totcs.Text = "Total (with Coherent Scattering)";
            this.totcs.UseVisualStyleBackColor = true;
            // 
            // ucXCOMPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucXCOMPreferences";
            this.Size = new System.Drawing.Size(479, 474);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox forceBox;
        private System.Windows.Forms.CheckBox useListbox;
        private System.Windows.Forms.CheckBox asciibox;
        private System.Windows.Forms.TextBox stepsBox;
        private System.Windows.Forms.TextBox maxEneBox;
        private System.Windows.Forms.TextBox minEneBox;
        private System.Windows.Forms.CheckBox loopCheckBox;
        private System.Windows.Forms.TextBox roundingTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox overridesCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox logscaleBox;
        private System.Windows.Forms.CheckBox totcs;
        private System.Windows.Forms.CheckBox totncs;
        private System.Windows.Forms.CheckBox ppnfbox;
        private System.Windows.Forms.CheckBox ppefbox;
        private System.Windows.Forms.CheckBox pebox;
        private System.Windows.Forms.CheckBox csbox;
        private System.Windows.Forms.CheckBox isbox;
    }
}
