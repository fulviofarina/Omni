namespace DB.UI
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
            minAreaLabel.Size = new System.Drawing.Size(96, 21);
            minAreaLabel.TabIndex = 4;
            minAreaLabel.Text = "Min. Energy";
            // 
            // maxUncLabel
            // 
            maxUncLabel.AutoSize = true;
            maxUncLabel.ForeColor = System.Drawing.Color.DarkOrange;
            maxUncLabel.Location = new System.Drawing.Point(3, 77);
            maxUncLabel.Name = "maxUncLabel";
            maxUncLabel.Size = new System.Drawing.Size(99, 21);
            maxUncLabel.TabIndex = 6;
            maxUncLabel.Text = "Max. Energy";
            // 
            // windowALabel
            // 
            windowALabel.AutoSize = true;
            windowALabel.ForeColor = System.Drawing.Color.DarkOrange;
            windowALabel.Location = new System.Drawing.Point(3, 133);
            windowALabel.Name = "windowALabel";
            windowALabel.Size = new System.Drawing.Size(51, 21);
            windowALabel.TabIndex = 8;
            windowALabel.Text = "Steps";
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
            this.splitContainer1.Size = new System.Drawing.Size(400, 273);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(181, 269);
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
            this.tableLayoutPanel2.Controls.Add(this.logscaleBox, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.loopCheckBox, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.forceBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.useListbox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.asciibox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.overridesCheckBox, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(201, 269);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // logscaleBox
            // 
            this.logscaleBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logscaleBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logscaleBox.Location = new System.Drawing.Point(3, 198);
            this.logscaleBox.Name = "logscaleBox";
            this.logscaleBox.Size = new System.Drawing.Size(195, 29);
            this.logscaleBox.TabIndex = 15;
            this.logscaleBox.Text = "Log Scale (plot)";
            this.logscaleBox.UseVisualStyleBackColor = true;
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loopCheckBox.ForeColor = System.Drawing.Color.Aqua;
            this.loopCheckBox.Location = new System.Drawing.Point(3, 128);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(195, 29);
            this.loopCheckBox.TabIndex = 10;
            this.loopCheckBox.Text = "Calculate All Samples";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // forceBox
            // 
            this.forceBox.Checked = true;
            this.forceBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forceBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forceBox.ForeColor = System.Drawing.Color.Gold;
            this.forceBox.Location = new System.Drawing.Point(3, 58);
            this.forceBox.Name = "forceBox";
            this.forceBox.Size = new System.Drawing.Size(195, 29);
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
            this.useListbox.ForeColor = System.Drawing.Color.Chartreuse;
            this.useListbox.Location = new System.Drawing.Point(3, 23);
            this.useListbox.Name = "useListbox";
            this.useListbox.Size = new System.Drawing.Size(195, 29);
            this.useListbox.TabIndex = 6;
            this.useListbox.Text = "Use Energies List";
            this.useListbox.UseVisualStyleBackColor = true;
            // 
            // asciibox
            // 
            this.asciibox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.asciibox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asciibox.Location = new System.Drawing.Point(3, 93);
            this.asciibox.Name = "asciibox";
            this.asciibox.Size = new System.Drawing.Size(195, 29);
            this.asciibox.TabIndex = 2;
            this.asciibox.Text = "Output as CSV file";
            this.asciibox.UseVisualStyleBackColor = true;
            // 
            // overridesCheckBox
            // 
            this.overridesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overridesCheckBox.ForeColor = System.Drawing.Color.Red;
            this.overridesCheckBox.Location = new System.Drawing.Point(3, 163);
            this.overridesCheckBox.Name = "overridesCheckBox";
            this.overridesCheckBox.Size = new System.Drawing.Size(195, 29);
            this.overridesCheckBox.TabIndex = 13;
            this.overridesCheckBox.Text = "Expert Mode";
            this.overridesCheckBox.UseVisualStyleBackColor = true;
            this.overridesCheckBox.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.Aqua;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Other Preferences";
            // 
            // ucXCOMPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucXCOMPreferences";
            this.Size = new System.Drawing.Size(400, 273);
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
    }
}
