namespace DB.UI
{
    partial class ucPreferences
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label minAreaLabel;
            System.Windows.Forms.Label maxUncLabel;
            System.Windows.Forms.Label windowALabel;
            System.Windows.Forms.Label windowBLabel;
            System.Windows.Forms.Label roundingLabel;
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.calcMassCheckBox = new System.Windows.Forms.CheckBox();
            this.roundingTextBox = new System.Windows.Forms.TextBox();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.showMatSSFCheckBox = new System.Windows.Forms.CheckBox();
            this.doCKCheckBox = new System.Windows.Forms.CheckBox();
            this.doMatSSFCheckBox = new System.Windows.Forms.CheckBox();
            this.calcDensityCheckBox = new System.Windows.Forms.CheckBox();
            this.aAFillHeightCheckBox = new System.Windows.Forms.CheckBox();
            this.aARadiusCheckBox = new System.Windows.Forms.CheckBox();
            this.showOtherCheckBox = new System.Windows.Forms.CheckBox();
            this.overridesCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PrefBS = new System.Windows.Forms.BindingSource(this.components);
            this.SSFPref = new System.Windows.Forms.BindingSource(this.components);
            minAreaLabel = new System.Windows.Forms.Label();
            maxUncLabel = new System.Windows.Forms.Label();
            windowALabel = new System.Windows.Forms.Label();
            windowBLabel = new System.Windows.Forms.Label();
            roundingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrefBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSFPref)).BeginInit();
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
            // roundingLabel
            // 
            roundingLabel.AutoSize = true;
            roundingLabel.ForeColor = System.Drawing.Color.DarkOrange;
            roundingLabel.Location = new System.Drawing.Point(3, 510);
            roundingLabel.Name = "roundingLabel";
            roundingLabel.Size = new System.Drawing.Size(81, 21);
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
            this.splitContainer1.Size = new System.Drawing.Size(607, 619);
            this.splitContainer1.SplitterDistance = 308;
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
            this.tableLayoutPanel1.RowCount = 19;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(304, 615);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // advancedEditorCheckBox
            // 
            this.advancedEditorCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedEditorCheckBox.Location = new System.Drawing.Point(3, 530);
            this.advancedEditorCheckBox.Name = "advancedEditorCheckBox";
            this.advancedEditorCheckBox.Size = new System.Drawing.Size(298, 24);
            this.advancedEditorCheckBox.TabIndex = 18;
            this.advancedEditorCheckBox.Text = "Advanced Editor";
            this.advancedEditorCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSampleDescriptionCheckBox
            // 
            this.showSampleDescriptionCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showSampleDescriptionCheckBox.Location = new System.Drawing.Point(3, 500);
            this.showSampleDescriptionCheckBox.Name = "showSampleDescriptionCheckBox";
            this.showSampleDescriptionCheckBox.Size = new System.Drawing.Size(298, 24);
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
            this.label2.Size = new System.Drawing.Size(298, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "Main Preferences";
            // 
            // fillBySpectraCheckBox
            // 
            this.fillBySpectraCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillBySpectraCheckBox.Location = new System.Drawing.Point(3, 449);
            this.fillBySpectraCheckBox.Name = "fillBySpectraCheckBox";
            this.fillBySpectraCheckBox.Size = new System.Drawing.Size(298, 45);
            this.fillBySpectraCheckBox.TabIndex = 14;
            this.fillBySpectraCheckBox.Text = "Fill by Spectra";
            this.fillBySpectraCheckBox.UseVisualStyleBackColor = true;
            // 
            // fillByHLCheckBox
            // 
            this.fillByHLCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillByHLCheckBox.Location = new System.Drawing.Point(3, 419);
            this.fillByHLCheckBox.Name = "fillByHLCheckBox";
            this.fillByHLCheckBox.Size = new System.Drawing.Size(298, 24);
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
            this.offlineCheckBox.Location = new System.Drawing.Point(3, 296);
            this.offlineCheckBox.Name = "offlineCheckBox";
            this.offlineCheckBox.Size = new System.Drawing.Size(298, 35);
            this.offlineCheckBox.TabIndex = 4;
            this.offlineCheckBox.Text = "Offline";
            this.offlineCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoLoadCheckBox
            // 
            this.autoLoadCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoLoadCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoLoadCheckBox.Location = new System.Drawing.Point(3, 248);
            this.autoLoadCheckBox.Name = "autoLoadCheckBox";
            this.autoLoadCheckBox.Size = new System.Drawing.Size(298, 42);
            this.autoLoadCheckBox.TabIndex = 3;
            this.autoLoadCheckBox.Text = "Auto-Load";
            this.autoLoadCheckBox.UseVisualStyleBackColor = true;
            // 
            // doSolangCheckBox
            // 
            this.doSolangCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doSolangCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doSolangCheckBox.Location = new System.Drawing.Point(3, 337);
            this.doSolangCheckBox.Name = "doSolangCheckBox";
            this.doSolangCheckBox.Size = new System.Drawing.Size(298, 35);
            this.doSolangCheckBox.TabIndex = 1;
            this.doSolangCheckBox.Text = "Do SolCoi";
            this.doSolangCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSolangCheckBox
            // 
            this.showSolangCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showSolangCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showSolangCheckBox.Location = new System.Drawing.Point(3, 378);
            this.showSolangCheckBox.Name = "showSolangCheckBox";
            this.showSolangCheckBox.Size = new System.Drawing.Size(298, 35);
            this.showSolangCheckBox.TabIndex = 2;
            this.showSolangCheckBox.Text = "Show SolCoi";
            this.showSolangCheckBox.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.calcMassCheckBox, 0, 4);
            this.tableLayoutPanel2.Controls.Add(roundingLabel, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.roundingTextBox, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.loopCheckBox, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.showMatSSFCheckBox, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.doCKCheckBox, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.doMatSSFCheckBox, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.calcDensityCheckBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.aAFillHeightCheckBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.aARadiusCheckBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.showOtherCheckBox, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.overridesCheckBox, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 13;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(285, 615);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // calcMassCheckBox
            // 
            this.calcMassCheckBox.AutoCheck = false;
            this.calcMassCheckBox.Location = new System.Drawing.Point(3, 170);
            this.calcMassCheckBox.Name = "calcMassCheckBox";
            this.calcMassCheckBox.Size = new System.Drawing.Size(104, 24);
            this.calcMassCheckBox.TabIndex = 12;
            this.calcMassCheckBox.Text = "Find Mass";
            this.calcMassCheckBox.UseVisualStyleBackColor = true;
            // 
            // roundingTextBox
            // 
            this.roundingTextBox.Location = new System.Drawing.Point(3, 562);
            this.roundingTextBox.Name = "roundingTextBox";
            this.roundingTextBox.Size = new System.Drawing.Size(100, 29);
            this.roundingTextBox.TabIndex = 11;
            this.roundingTextBox.Text = "N4";
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loopCheckBox.Location = new System.Drawing.Point(3, 317);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(279, 43);
            this.loopCheckBox.TabIndex = 10;
            this.loopCheckBox.Text = "Loop Calculation";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // showMatSSFCheckBox
            // 
            this.showMatSSFCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showMatSSFCheckBox.Location = new System.Drawing.Point(3, 464);
            this.showMatSSFCheckBox.Name = "showMatSSFCheckBox";
            this.showMatSSFCheckBox.Size = new System.Drawing.Size(279, 43);
            this.showMatSSFCheckBox.TabIndex = 9;
            this.showMatSSFCheckBox.Text = "Show MatSSF process";
            this.showMatSSFCheckBox.UseVisualStyleBackColor = true;
            // 
            // doCKCheckBox
            // 
            this.doCKCheckBox.Checked = true;
            this.doCKCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doCKCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doCKCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doCKCheckBox.Location = new System.Drawing.Point(3, 268);
            this.doCKCheckBox.Name = "doCKCheckBox";
            this.doCKCheckBox.Size = new System.Drawing.Size(279, 43);
            this.doCKCheckBox.TabIndex = 8;
            this.doCKCheckBox.Text = "Do CK";
            this.doCKCheckBox.UseVisualStyleBackColor = true;
            // 
            // doMatSSFCheckBox
            // 
            this.doMatSSFCheckBox.Checked = true;
            this.doMatSSFCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doMatSSFCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doMatSSFCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doMatSSFCheckBox.Location = new System.Drawing.Point(3, 219);
            this.doMatSSFCheckBox.Name = "doMatSSFCheckBox";
            this.doMatSSFCheckBox.Size = new System.Drawing.Size(279, 43);
            this.doMatSSFCheckBox.TabIndex = 6;
            this.doMatSSFCheckBox.Text = "Do MatSSF";
            this.doMatSSFCheckBox.UseVisualStyleBackColor = true;
            // 
            // calcDensityCheckBox
            // 
            this.calcDensityCheckBox.Checked = true;
            this.calcDensityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.calcDensityCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calcDensityCheckBox.Location = new System.Drawing.Point(3, 23);
            this.calcDensityCheckBox.Name = "calcDensityCheckBox";
            this.calcDensityCheckBox.Size = new System.Drawing.Size(279, 43);
            this.calcDensityCheckBox.TabIndex = 5;
            this.calcDensityCheckBox.Text = "Find Density";
            this.calcDensityCheckBox.UseVisualStyleBackColor = true;
            // 
            // aAFillHeightCheckBox
            // 
            this.aAFillHeightCheckBox.Checked = true;
            this.aAFillHeightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.aAFillHeightCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aAFillHeightCheckBox.Location = new System.Drawing.Point(3, 121);
            this.aAFillHeightCheckBox.Name = "aAFillHeightCheckBox";
            this.aAFillHeightCheckBox.Size = new System.Drawing.Size(279, 43);
            this.aAFillHeightCheckBox.TabIndex = 4;
            this.aAFillHeightCheckBox.Text = "Find Length";
            this.aAFillHeightCheckBox.UseVisualStyleBackColor = true;
            // 
            // aARadiusCheckBox
            // 
            this.aARadiusCheckBox.Checked = true;
            this.aARadiusCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.aARadiusCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aARadiusCheckBox.Location = new System.Drawing.Point(3, 72);
            this.aARadiusCheckBox.Name = "aARadiusCheckBox";
            this.aARadiusCheckBox.Size = new System.Drawing.Size(279, 43);
            this.aARadiusCheckBox.TabIndex = 3;
            this.aARadiusCheckBox.Text = "Find Radius";
            this.aARadiusCheckBox.UseVisualStyleBackColor = true;
            // 
            // showOtherCheckBox
            // 
            this.showOtherCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showOtherCheckBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showOtherCheckBox.Location = new System.Drawing.Point(3, 415);
            this.showOtherCheckBox.Name = "showOtherCheckBox";
            this.showOtherCheckBox.Size = new System.Drawing.Size(279, 43);
            this.showOtherCheckBox.TabIndex = 2;
            this.showOtherCheckBox.Text = "Show Other Content";
            this.showOtherCheckBox.UseVisualStyleBackColor = true;
            // 
            // overridesCheckBox
            // 
            this.overridesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overridesCheckBox.Location = new System.Drawing.Point(3, 366);
            this.overridesCheckBox.Name = "overridesCheckBox";
            this.overridesCheckBox.Size = new System.Drawing.Size(279, 43);
            this.overridesCheckBox.TabIndex = 13;
            this.overridesCheckBox.Text = "Override Default Values";
            this.overridesCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.Aqua;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "SSF Preferences";
            // 
            // PrefBS
            // 
            this.PrefBS.DataSource = typeof(DB.LINAA.PreferencesDataTable);
            // 
            // SSFPref
            // 
            this.SSFPref.DataSource = typeof(DB.LINAA.SSFPrefDataTable);
            // 
            // ucPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucPreferences";
            this.Size = new System.Drawing.Size(607, 619);
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
            ((System.ComponentModel.ISupportInitialize)(this.PrefBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSFPref)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox autoLoadCheckBox;
        private System.Windows.Forms.BindingSource PrefBS;
        private System.Windows.Forms.CheckBox doSolangCheckBox;
        private System.Windows.Forms.CheckBox showSolangCheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox offlineCheckBox;
        private System.Windows.Forms.CheckBox doCKCheckBox;
        private System.Windows.Forms.BindingSource SSFPref;
        private System.Windows.Forms.CheckBox doMatSSFCheckBox;
        private System.Windows.Forms.CheckBox calcDensityCheckBox;
        private System.Windows.Forms.CheckBox aAFillHeightCheckBox;
        private System.Windows.Forms.CheckBox aARadiusCheckBox;
        private System.Windows.Forms.CheckBox showOtherCheckBox;
        private System.Windows.Forms.TextBox windowBTextBox;
        private System.Windows.Forms.TextBox windowATextBox;
        private System.Windows.Forms.TextBox maxUncTextBox;
        private System.Windows.Forms.TextBox minAreaTextBox;
        private System.Windows.Forms.CheckBox fillByHLCheckBox;
        private System.Windows.Forms.CheckBox showMatSSFCheckBox;
        private System.Windows.Forms.CheckBox loopCheckBox;
        private System.Windows.Forms.TextBox roundingTextBox;
        private System.Windows.Forms.CheckBox calcMassCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox fillBySpectraCheckBox;
        private System.Windows.Forms.CheckBox overridesCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox showSampleDescriptionCheckBox;
        private System.Windows.Forms.CheckBox advancedEditorCheckBox;
    }
}
