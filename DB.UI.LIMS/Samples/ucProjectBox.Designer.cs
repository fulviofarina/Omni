namespace DB.UI
{
    partial class ucProjectBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucProjectBox));
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.TS = new System.Windows.Forms.ToolStrip();
            this.projectlabel = new System.Windows.Forms.ToolStripLabel();
            this.projectbox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.options = new System.Windows.Forms.ToolStripDropDownButton();
            this.AARadius = new System.Windows.Forms.ToolStripMenuItem();
            this.AAFillHeight = new System.Windows.Forms.ToolStripMenuItem();
            this.TLP.SuspendLayout();
            this.TS.SuspendLayout();
            this.SuspendLayout();
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 1;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP.Controls.Add(this.TS, 0, 0);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(0, 0);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 1;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.61589F));
            this.TLP.Size = new System.Drawing.Size(480, 53);
            this.TLP.TabIndex = 2;
            // 
            // TS
            // 
            this.TS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectlabel,
            this.projectbox,
            this.toolStripSeparator1,
            this.options});
            this.TS.Location = new System.Drawing.Point(0, 0);
            this.TS.Name = "TS";
            this.TS.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.TS.Size = new System.Drawing.Size(480, 53);
            this.TS.TabIndex = 2;
            this.TS.Text = "toolStrip1";
            // 
            // projectlabel
            // 
            this.projectlabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projectlabel.ForeColor = System.Drawing.Color.Peru;
            this.projectlabel.Name = "projectlabel";
            this.projectlabel.Size = new System.Drawing.Size(64, 50);
            this.projectlabel.Text = "Project";
            // 
            // projectbox
            // 
            this.projectbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.projectbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.projectbox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.projectbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projectbox.ForeColor = System.Drawing.Color.White;
            this.projectbox.Name = "projectbox";
            this.projectbox.Size = new System.Drawing.Size(223, 53);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 53);
            // 
            // options
            // 
            this.options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AARadius,
            this.AAFillHeight});
            this.options.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.options.Image = ((System.Drawing.Image)(resources.GetObject("options.Image")));
            this.options.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.options.Name = "options";
            this.options.Size = new System.Drawing.Size(99, 50);
            this.options.Text = "Options";
            // 
            // AARadius
            // 
            this.AARadius.CheckOnClick = true;
            this.AARadius.Name = "AARadius";
            this.AARadius.Size = new System.Drawing.Size(253, 26);
            this.AARadius.Text = "Auto-adjust Radius";
            this.AARadius.CheckedChanged += new System.EventHandler(this.AutoAdjust_CheckedChanged);
            // 
            // AAFillHeight
            // 
            this.AAFillHeight.CheckOnClick = true;
            this.AAFillHeight.Name = "AAFillHeight";
            this.AAFillHeight.Size = new System.Drawing.Size(253, 26);
            this.AAFillHeight.Text = "Auto-adjust Fill Height";
            this.AAFillHeight.CheckedChanged += new System.EventHandler(this.AutoAdjust_CheckedChanged);
            // 
            // ucProjectBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLP);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucProjectBox";
            this.Size = new System.Drawing.Size(480, 53);
            this.TLP.ResumeLayout(false);
            this.TLP.PerformLayout();
            this.TS.ResumeLayout(false);
            this.TS.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.ToolStrip TS;
        private System.Windows.Forms.ToolStripLabel projectlabel;
        public System.Windows.Forms.ToolStripComboBox projectbox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton options;
        private System.Windows.Forms.ToolStripMenuItem AARadius;
        private System.Windows.Forms.ToolStripMenuItem AAFillHeight;
    }
}
