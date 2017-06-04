namespace DB.UI
{
    partial class ucSSFPanel
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.UnitSSFSC = new System.Windows.Forms.SplitContainer();
            this.inputTLP = new System.Windows.Forms.TableLayoutPanel();
            this.descripTS = new DB.UI.ucGenericCBox();
            this.infoTS = new System.Windows.Forms.ToolStrip();
            this.infoLBL = new System.Windows.Forms.ToolStripLabel();
            this.unitSC = new System.Windows.Forms.SplitContainer();
            this.ucGenericCBox1 = new DB.UI.ucGenericCBox();
            this.barTLP = new System.Windows.Forms.TableLayoutPanel();
            this.changeViewTS = new System.Windows.Forms.ToolStrip();
            this.sampleCompoLbl = new System.Windows.Forms.ToolStripLabel();
            this.imgBtn = new System.Windows.Forms.ToolStripButton();
            this.TwoSectionSC = new System.Windows.Forms.SplitContainer();
            this.ucDataContent = new DB.UI.ucSSFData();
            this.ucSubMS = new DB.UI.ucMatrixSimple();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitSSFSC)).BeginInit();
            this.UnitSSFSC.Panel1.SuspendLayout();
            this.UnitSSFSC.SuspendLayout();
            this.inputTLP.SuspendLayout();
            this.infoTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unitSC)).BeginInit();
            this.unitSC.Panel1.SuspendLayout();
            this.unitSC.SuspendLayout();
            this.barTLP.SuspendLayout();
            this.changeViewTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TwoSectionSC)).BeginInit();
            this.TwoSectionSC.Panel1.SuspendLayout();
            this.TwoSectionSC.Panel2.SuspendLayout();
            this.TwoSectionSC.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(355, 3);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Size = new System.Drawing.Size(374, 23);
            this.splitContainer5.SplitterDistance = 123;
            this.splitContainer5.TabIndex = 9;
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 30);
            // 
            // UnitSSFSC
            // 
            this.UnitSSFSC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.UnitSSFSC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnitSSFSC.Location = new System.Drawing.Point(0, 0);
            this.UnitSSFSC.Margin = new System.Windows.Forms.Padding(6);
            this.UnitSSFSC.Name = "UnitSSFSC";
            this.UnitSSFSC.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // UnitSSFSC.Panel1
            // 
            this.UnitSSFSC.Panel1.Controls.Add(this.inputTLP);
            // 
            // UnitSSFSC.Panel2
            // 
            this.UnitSSFSC.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UnitSSFSC.Size = new System.Drawing.Size(702, 854);
            this.UnitSSFSC.SplitterDistance = 654;
            this.UnitSSFSC.SplitterWidth = 8;
            this.UnitSSFSC.TabIndex = 11;
            // 
            // inputTLP
            // 
            this.inputTLP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.inputTLP.ColumnCount = 1;
            this.inputTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.inputTLP.Controls.Add(this.descripTS, 0, 1);
            this.inputTLP.Controls.Add(this.infoTS, 0, 3);
            this.inputTLP.Controls.Add(this.unitSC, 0, 0);
            this.inputTLP.Controls.Add(this.barTLP, 0, 2);
            this.inputTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTLP.Location = new System.Drawing.Point(0, 0);
            this.inputTLP.Margin = new System.Windows.Forms.Padding(4);
            this.inputTLP.Name = "inputTLP";
            this.inputTLP.RowCount = 4;
            this.inputTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.910892F));
            this.inputTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.920792F));
            this.inputTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.20792F));
            this.inputTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.960396F));
            this.inputTLP.Size = new System.Drawing.Size(702, 654);
            this.inputTLP.TabIndex = 4;
            // 
            // descripTS
            // 
            this.descripTS.AutoSize = true;
            this.descripTS.BindingField = "";
            this.descripTS.CallBack = null;
            this.descripTS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descripTS.EnterPressed = false;
            this.descripTS.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descripTS.InputProjects = new string[0];
            this.descripTS.KeyValue = 0;
            this.descripTS.Label = "Description";
            this.descripTS.LabelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.descripTS.LabelForeColor = System.Drawing.Color.White;
            this.descripTS.Location = new System.Drawing.Point(3, 61);
            this.descripTS.Name = "descripTS";
            this.descripTS.Offline = false;
            this.descripTS.Size = new System.Drawing.Size(696, 45);
            this.descripTS.TabIndex = 15;
            this.descripTS.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.descripTS.TextContent = "NOTHING";
            this.descripTS.TextForeColor = System.Drawing.Color.YellowGreen;
            // 
            // infoTS
            // 
            this.infoTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoTS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.infoTS.Dock = System.Windows.Forms.DockStyle.None;
            this.infoTS.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoTS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.infoTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoLBL});
            this.infoTS.Location = new System.Drawing.Point(0, 627);
            this.infoTS.Name = "infoTS";
            this.infoTS.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.infoTS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.infoTS.Size = new System.Drawing.Size(702, 27);
            this.infoTS.TabIndex = 14;
            this.infoTS.Text = "toolStrip2";
            // 
            // infoLBL
            // 
            this.infoLBL.AutoToolTip = true;
            this.infoLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.infoLBL.ForeColor = System.Drawing.Color.Violet;
            this.infoLBL.Name = "infoLBL";
            this.infoLBL.Size = new System.Drawing.Size(40, 24);
            this.infoLBL.Text = "Info";
            // 
            // unitSC
            // 
            this.unitSC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unitSC.Location = new System.Drawing.Point(6, 6);
            this.unitSC.Margin = new System.Windows.Forms.Padding(6);
            this.unitSC.Name = "unitSC";
            // 
            // unitSC.Panel1
            // 
            this.unitSC.Panel1.Controls.Add(this.ucGenericCBox1);
            this.unitSC.Size = new System.Drawing.Size(690, 46);
            this.unitSC.SplitterDistance = 378;
            this.unitSC.SplitterWidth = 7;
            this.unitSC.TabIndex = 8;
            // 
            // ucGenericCBox1
            // 
            this.ucGenericCBox1.AutoSize = true;
            this.ucGenericCBox1.BindingField = "";
            this.ucGenericCBox1.CallBack = null;
            this.ucGenericCBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGenericCBox1.EnterPressed = false;
            this.ucGenericCBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucGenericCBox1.InputProjects = new string[0];
            this.ucGenericCBox1.KeyValue = 0;
            this.ucGenericCBox1.Label = "Sample";
            this.ucGenericCBox1.LabelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.ucGenericCBox1.LabelForeColor = System.Drawing.Color.Gold;
            this.ucGenericCBox1.Location = new System.Drawing.Point(0, 0);
            this.ucGenericCBox1.Name = "ucGenericCBox1";
            this.ucGenericCBox1.Offline = false;
            this.ucGenericCBox1.Size = new System.Drawing.Size(378, 46);
            this.ucGenericCBox1.TabIndex = 0;
            this.ucGenericCBox1.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ucGenericCBox1.TextContent = "NOTHING";
            this.ucGenericCBox1.TextForeColor = System.Drawing.Color.LemonChiffon;
            // 
            // barTLP
            // 
            this.barTLP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barTLP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.barTLP.ColumnCount = 1;
            this.barTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.barTLP.Controls.Add(this.changeViewTS, 0, 0);
            this.barTLP.Controls.Add(this.TwoSectionSC, 0, 1);
            this.barTLP.Location = new System.Drawing.Point(3, 112);
            this.barTLP.Name = "barTLP";
            this.barTLP.RowCount = 2;
            this.barTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.barTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.barTLP.Size = new System.Drawing.Size(696, 512);
            this.barTLP.TabIndex = 20;
            // 
            // changeViewTS
            // 
            this.changeViewTS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.changeViewTS.CanOverflow = false;
            this.changeViewTS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.changeViewTS.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeViewTS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.changeViewTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sampleCompoLbl,
            this.imgBtn});
            this.changeViewTS.Location = new System.Drawing.Point(0, 0);
            this.changeViewTS.Name = "changeViewTS";
            this.changeViewTS.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.changeViewTS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.changeViewTS.Size = new System.Drawing.Size(696, 28);
            this.changeViewTS.TabIndex = 13;
            this.changeViewTS.Text = "toolStrip2";
            // 
            // sampleCompoLbl
            // 
            this.sampleCompoLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sampleCompoLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sampleCompoLbl.ForeColor = System.Drawing.Color.LightYellow;
            this.sampleCompoLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.sampleCompoLbl.LinkColor = System.Drawing.Color.Lime;
            this.sampleCompoLbl.Margin = new System.Windows.Forms.Padding(1, 1, 2, 2);
            this.sampleCompoLbl.Name = "sampleCompoLbl";
            this.sampleCompoLbl.Size = new System.Drawing.Size(193, 25);
            this.sampleCompoLbl.Text = "COMPOSITION VIEW";
            this.sampleCompoLbl.ToolTipText = "Click here to change the sample View";
            this.sampleCompoLbl.VisitedLinkColor = System.Drawing.Color.Fuchsia;
            // 
            // imgBtn
            // 
            this.imgBtn.AutoToolTip = false;
            this.imgBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imgBtn.Image = global::DB.UI.Properties.Resources.Matrices;
            this.imgBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imgBtn.Name = "imgBtn";
            this.imgBtn.Size = new System.Drawing.Size(23, 25);
            this.imgBtn.Text = "toolStripButton1";
            this.imgBtn.ToolTipText = "Click here to change the sample View";
            // 
            // TwoSectionSC
            // 
            this.TwoSectionSC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TwoSectionSC.Location = new System.Drawing.Point(3, 31);
            this.TwoSectionSC.Name = "TwoSectionSC";
            // 
            // TwoSectionSC.Panel1
            // 
            this.TwoSectionSC.Panel1.Controls.Add(this.ucDataContent);
            // 
            // TwoSectionSC.Panel2
            // 
            this.TwoSectionSC.Panel2.Controls.Add(this.ucSubMS);
            this.TwoSectionSC.Size = new System.Drawing.Size(690, 550);
            this.TwoSectionSC.SplitterDistance = 336;
            this.TwoSectionSC.TabIndex = 14;
            // 
            // ucDataContent
            // 
            this.ucDataContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ucDataContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDataContent.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.ucDataContent.Location = new System.Drawing.Point(0, 0);
            this.ucDataContent.Margin = new System.Windows.Forms.Padding(4);
            this.ucDataContent.Name = "ucDataContent";
            this.ucDataContent.Size = new System.Drawing.Size(336, 550);
            this.ucDataContent.TabIndex = 0;
            // 
            // ucSubMS
            // 
            this.ucSubMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSubMS.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSubMS.Location = new System.Drawing.Point(0, 0);
            this.ucSubMS.Name = "ucSubMS";
            this.ucSubMS.Size = new System.Drawing.Size(350, 550);
            this.ucSubMS.TabIndex = 0;
            // 
            // ucSSFPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UnitSSFSC);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucSSFPanel";
            this.Size = new System.Drawing.Size(702, 854);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.UnitSSFSC.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnitSSFSC)).EndInit();
            this.UnitSSFSC.ResumeLayout(false);
            this.inputTLP.ResumeLayout(false);
            this.inputTLP.PerformLayout();
            this.infoTS.ResumeLayout(false);
            this.infoTS.PerformLayout();
            this.unitSC.Panel1.ResumeLayout(false);
            this.unitSC.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unitSC)).EndInit();
            this.unitSC.ResumeLayout(false);
            this.barTLP.ResumeLayout(false);
            this.barTLP.PerformLayout();
            this.changeViewTS.ResumeLayout(false);
            this.changeViewTS.PerformLayout();
            this.TwoSectionSC.Panel1.ResumeLayout(false);
            this.TwoSectionSC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TwoSectionSC)).EndInit();
            this.TwoSectionSC.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel inputTLP;
        private System.Windows.Forms.SplitContainer UnitSSFSC;
        //   private System.Windows.Forms.TableLayoutPanel TLPS;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer unitSC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStrip changeViewTS;
        private System.Windows.Forms.ToolStripLabel sampleCompoLbl;
        private System.Windows.Forms.TableLayoutPanel barTLP;
        private System.Windows.Forms.ToolStrip infoTS;
        private System.Windows.Forms.SplitContainer TwoSectionSC;
        private ucSSFData ucDataContent;
        private ucMatrixSimple ucSubMS;
        private System.Windows.Forms.ToolStripButton imgBtn;
        private System.Windows.Forms.ToolStripLabel infoLBL;
        private ucGenericCBox descripTS;
        private ucGenericCBox ucGenericCBox1;
    }
}
