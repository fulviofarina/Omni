namespace DB.UI
{
    partial class ucMatrix
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DBTLP = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ucMatrixSimple1 = new DB.UI.ucMatrixSimple();
            this.ucCalculate1 = new VTools.ucCalculate();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grapher = new VTools.Graph();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ucMUES1 = new DB.UI.ucMUES();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.DBTLP.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.DBTLP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1483, 787);
            this.splitContainer1.SplitterDistance = 627;
            this.splitContainer1.TabIndex = 1;
            // 
            // DBTLP
            // 
            this.DBTLP.ColumnCount = 1;
            this.DBTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DBTLP.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.DBTLP.Controls.Add(this.ucCalculate1, 0, 0);
            this.DBTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DBTLP.Location = new System.Drawing.Point(0, 0);
            this.DBTLP.Name = "DBTLP";
            this.DBTLP.RowCount = 4;
            this.DBTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.DBTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 523F));
            this.DBTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.DBTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.DBTLP.Size = new System.Drawing.Size(627, 787);
            this.DBTLP.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.45921F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.54079F));
            this.tableLayoutPanel1.Controls.Add(this.ucMatrixSimple1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(621, 517);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // ucMatrixSimple1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucMatrixSimple1, 2);
            this.ucMatrixSimple1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMatrixSimple1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMatrixSimple1.Location = new System.Drawing.Point(3, 3);
            this.ucMatrixSimple1.Name = "ucMatrixSimple1";
            this.ucMatrixSimple1.Size = new System.Drawing.Size(615, 511);
            this.ucMatrixSimple1.TabIndex = 0;
            // 
            // ucCalculate1
            // 
            this.ucCalculate1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCalculate1.Location = new System.Drawing.Point(3, 3);
            this.ucCalculate1.Name = "ucCalculate1";
            this.ucCalculate1.Size = new System.Drawing.Size(621, 34);
            this.ucCalculate1.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(852, 787);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grapher);
            this.tabPage2.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(844, 749);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "GRAPH";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grapher
            // 
            this.grapher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grapher.Location = new System.Drawing.Point(3, 3);
            this.grapher.Name = "grapher";
            this.grapher.Size = new System.Drawing.Size(838, 743);
            this.grapher.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ucMUES1);
            this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(981, 749);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "MACs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ucMUES1
            // 
            this.ucMUES1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMUES1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMUES1.Location = new System.Drawing.Point(0, 0);
            this.ucMUES1.Name = "ucMUES1";
            this.ucMUES1.Size = new System.Drawing.Size(981, 749);
            this.ucMUES1.TabIndex = 0;
            // 
            // ucMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucMatrix";
            this.Size = new System.Drawing.Size(1483, 787);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.DBTLP.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
		private System.Windows.Forms.TableLayoutPanel DBTLP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.DataGridViewTextBoxColumn uncUnitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn weightDataGridViewTextBoxColumn;
        private ucMatrixSimple ucMatrixSimple1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private VTools.Graph grapher;
        private System.Windows.Forms.TabPage tabPage3;
        private ucMUES ucMUES1;
        private VTools.ucCalculate ucCalculate1;
    }
}
