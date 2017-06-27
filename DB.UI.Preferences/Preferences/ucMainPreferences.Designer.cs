namespace DB.UI
{
    partial class ucMainPreferences
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
            System.Windows.Forms.Label windowBLabel;
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.runInBackgroundCheckBox = new System.Windows.Forms.CheckBox();
            this.advancedEditorCheckBox = new System.Windows.Forms.CheckBox();
            this.showSampleDescriptionCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fillBySpectraCheckBox = new System.Windows.Forms.CheckBox();
            this.fillByHLCheckBox = new System.Windows.Forms.CheckBox();
            this.windowBTextBox = new System.Windows.Forms.TextBox();
            this.windowATextBox = new System.Windows.Forms.TextBox();
            this.maxUncTextBox = new System.Windows.Forms.TextBox();
            this.minAreaTextBox = new System.Windows.Forms.TextBox();
            this.offlineCheckBox = new System.Windows.Forms.CheckBox();
            this.autoLoadCheckBox = new System.Windows.Forms.CheckBox();
            this.doSolangCheckBox = new System.Windows.Forms.CheckBox();
            this.showSolangCheckBox = new System.Windows.Forms.CheckBox();
            minAreaLabel = new System.Windows.Forms.Label();
            maxUncLabel = new System.Windows.Forms.Label();
            windowALabel = new System.Windows.Forms.Label();
            windowBLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // minAreaLabel
            // 
            minAreaLabel.AutoSize = true;
            minAreaLabel.ForeColor = System.Drawing.Color.DarkOrange;
            minAreaLabel.Location = new System.Drawing.Point(3, 21);
            minAreaLabel.Name = "minAreaLabel";
            minAreaLabel.Size = new System.Drawing.Size(76, 21);
            minAreaLabel.TabIndex = 4;
            minAreaLabel.Text = "Min Area";
            // 
            // maxUncLabel
            // 
            maxUncLabel.AutoSize = true;
            maxUncLabel.ForeColor = System.Drawing.Color.DarkOrange;
            maxUncLabel.Location = new System.Drawing.Point(3, 77);
            maxUncLabel.Name = "maxUncLabel";
            maxUncLabel.Size = new System.Drawing.Size(73, 21);
            maxUncLabel.TabIndex = 6;
            maxUncLabel.Text = "Max Unc";
            // 
            // windowALabel
            // 
            windowALabel.AutoSize = true;
            windowALabel.ForeColor = System.Drawing.Color.DarkOrange;
            windowALabel.Location = new System.Drawing.Point(3, 133);
            windowALabel.Name = "windowALabel";
            windowALabel.Size = new System.Drawing.Size(85, 21);
            windowALabel.TabIndex = 8;
            windowALabel.Text = "Window A";
            // 
            // windowBLabel
            // 
            windowBLabel.AutoSize = true;
            windowBLabel.ForeColor = System.Drawing.Color.DarkOrange;
            windowBLabel.Location = new System.Drawing.Point(3, 189);
            windowBLabel.Name = "windowBLabel";
            windowBLabel.Size = new System.Drawing.Size(84, 21);
            windowBLabel.TabIndex = 10;
            windowBLabel.Text = "Window B";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.runInBackgroundCheckBox, 0, 18);
            this.tableLayoutPanel1.Controls.Add(this.advancedEditorCheckBox, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.showSampleDescriptionCheckBox, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fillBySpectraCheckBox, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.fillByHLCheckBox, 0, 13);
            this.tableLayoutPanel1.Controls.Add(windowBLabel, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.windowBTextBox, 0, 8);
            this.tableLayoutPanel1.Controls.Add(windowALabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.windowATextBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(maxUncLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.maxUncTextBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(minAreaLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.minAreaTextBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.offlineCheckBox, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.autoLoadCheckBox, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.doSolangCheckBox, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.showSolangCheckBox, 0, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 20;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(270, 619);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // runInBackgroundCheckBox
            // 
            this.runInBackgroundCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runInBackgroundCheckBox.ForeColor = System.Drawing.Color.Red;
            this.runInBackgroundCheckBox.Location = new System.Drawing.Point(3, 560);
            this.runInBackgroundCheckBox.Name = "runInBackgroundCheckBox";
            this.runInBackgroundCheckBox.Size = new System.Drawing.Size(264, 24);
            this.runInBackgroundCheckBox.TabIndex = 19;
            this.runInBackgroundCheckBox.Text = "Timer ON/OFF (Experimental)";
            this.runInBackgroundCheckBox.UseVisualStyleBackColor = true;
            // 
            // advancedEditorCheckBox
            // 
            this.advancedEditorCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedEditorCheckBox.ForeColor = System.Drawing.Color.White;
            this.advancedEditorCheckBox.Location = new System.Drawing.Point(3, 530);
            this.advancedEditorCheckBox.Name = "advancedEditorCheckBox";
            this.advancedEditorCheckBox.Size = new System.Drawing.Size(264, 24);
            this.advancedEditorCheckBox.TabIndex = 18;
            this.advancedEditorCheckBox.Text = "Advanced Editor";
            this.advancedEditorCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSampleDescriptionCheckBox
            // 
            this.showSampleDescriptionCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showSampleDescriptionCheckBox.ForeColor = System.Drawing.Color.White;
            this.showSampleDescriptionCheckBox.Location = new System.Drawing.Point(3, 500);
            this.showSampleDescriptionCheckBox.Name = "showSampleDescriptionCheckBox";
            this.showSampleDescriptionCheckBox.Size = new System.Drawing.Size(264, 24);
            this.showSampleDescriptionCheckBox.TabIndex = 17;
            this.showSampleDescriptionCheckBox.Text = "Show Sample Description";
            this.showSampleDescriptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(264, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "Main Preferences";
            // 
            // fillBySpectraCheckBox
            // 
            this.fillBySpectraCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillBySpectraCheckBox.ForeColor = System.Drawing.Color.White;
            this.fillBySpectraCheckBox.Location = new System.Drawing.Point(3, 449);
            this.fillBySpectraCheckBox.Name = "fillBySpectraCheckBox";
            this.fillBySpectraCheckBox.Size = new System.Drawing.Size(264, 45);
            this.fillBySpectraCheckBox.TabIndex = 14;
            this.fillBySpectraCheckBox.Text = "Fill by Spectra";
            this.fillBySpectraCheckBox.UseVisualStyleBackColor = true;
            // 
            // fillByHLCheckBox
            // 
            this.fillByHLCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillByHLCheckBox.ForeColor = System.Drawing.Color.White;
            this.fillByHLCheckBox.Location = new System.Drawing.Point(3, 419);
            this.fillByHLCheckBox.Name = "fillByHLCheckBox";
            this.fillByHLCheckBox.Size = new System.Drawing.Size(264, 24);
            this.fillByHLCheckBox.TabIndex = 13;
            this.fillByHLCheckBox.Text = "Fill by HL";
            this.fillByHLCheckBox.UseVisualStyleBackColor = true;
            // 
            // windowBTextBox
            // 
            this.windowBTextBox.Location = new System.Drawing.Point(3, 213);
            this.windowBTextBox.Name = "windowBTextBox";
            this.windowBTextBox.Size = new System.Drawing.Size(100, 29);
            this.windowBTextBox.TabIndex = 11;
            // 
            // windowATextBox
            // 
            this.windowATextBox.Location = new System.Drawing.Point(3, 157);
            this.windowATextBox.Name = "windowATextBox";
            this.windowATextBox.Size = new System.Drawing.Size(100, 29);
            this.windowATextBox.TabIndex = 9;
            // 
            // maxUncTextBox
            // 
            this.maxUncTextBox.Location = new System.Drawing.Point(3, 101);
            this.maxUncTextBox.Name = "maxUncTextBox";
            this.maxUncTextBox.Size = new System.Drawing.Size(100, 29);
            this.maxUncTextBox.TabIndex = 7;
            // 
            // minAreaTextBox
            // 
            this.minAreaTextBox.Location = new System.Drawing.Point(3, 45);
            this.minAreaTextBox.Name = "minAreaTextBox";
            this.minAreaTextBox.Size = new System.Drawing.Size(100, 29);
            this.minAreaTextBox.TabIndex = 5;
            // 
            // offlineCheckBox
            // 
            this.offlineCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offlineCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offlineCheckBox.ForeColor = System.Drawing.Color.White;
            this.offlineCheckBox.Location = new System.Drawing.Point(3, 296);
            this.offlineCheckBox.Name = "offlineCheckBox";
            this.offlineCheckBox.Size = new System.Drawing.Size(264, 35);
            this.offlineCheckBox.TabIndex = 4;
            this.offlineCheckBox.Text = "Offline";
            this.offlineCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoLoadCheckBox
            // 
            this.autoLoadCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoLoadCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoLoadCheckBox.ForeColor = System.Drawing.Color.White;
            this.autoLoadCheckBox.Location = new System.Drawing.Point(3, 248);
            this.autoLoadCheckBox.Name = "autoLoadCheckBox";
            this.autoLoadCheckBox.Size = new System.Drawing.Size(264, 42);
            this.autoLoadCheckBox.TabIndex = 3;
            this.autoLoadCheckBox.Text = "Auto-Load";
            this.autoLoadCheckBox.UseVisualStyleBackColor = true;
            // 
            // doSolangCheckBox
            // 
            this.doSolangCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doSolangCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doSolangCheckBox.ForeColor = System.Drawing.Color.White;
            this.doSolangCheckBox.Location = new System.Drawing.Point(3, 337);
            this.doSolangCheckBox.Name = "doSolangCheckBox";
            this.doSolangCheckBox.Size = new System.Drawing.Size(264, 35);
            this.doSolangCheckBox.TabIndex = 1;
            this.doSolangCheckBox.Text = "Do SolCoi";
            this.doSolangCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSolangCheckBox
            // 
            this.showSolangCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showSolangCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showSolangCheckBox.ForeColor = System.Drawing.Color.White;
            this.showSolangCheckBox.Location = new System.Drawing.Point(3, 378);
            this.showSolangCheckBox.Name = "showSolangCheckBox";
            this.showSolangCheckBox.Size = new System.Drawing.Size(264, 35);
            this.showSolangCheckBox.TabIndex = 2;
            this.showSolangCheckBox.Text = "Show SolCoi";
            this.showSolangCheckBox.UseVisualStyleBackColor = true;
            // 
            // ucMainPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ucMainPreferences";
            this.Size = new System.Drawing.Size(270, 619);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox autoLoadCheckBox;
        private System.Windows.Forms.CheckBox doSolangCheckBox;
        private System.Windows.Forms.CheckBox showSolangCheckBox;
        private System.Windows.Forms.CheckBox offlineCheckBox;
        private System.Windows.Forms.TextBox windowBTextBox;
        private System.Windows.Forms.TextBox windowATextBox;
        private System.Windows.Forms.TextBox maxUncTextBox;
        private System.Windows.Forms.TextBox minAreaTextBox;
        private System.Windows.Forms.CheckBox fillByHLCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox fillBySpectraCheckBox;
        private System.Windows.Forms.CheckBox showSampleDescriptionCheckBox;
        private System.Windows.Forms.CheckBox advancedEditorCheckBox;
        private System.Windows.Forms.CheckBox runInBackgroundCheckBox;
    }
}
