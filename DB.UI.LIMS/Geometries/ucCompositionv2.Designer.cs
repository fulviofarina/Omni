namespace DB.UI
{
    partial class ucComposition2
    {

        private System.Windows.Forms.DataGridViewCheckBoxColumn cDataGridViewCheckBoxColumn;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DB.LINAA Linaa;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixCompositionDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixDensityDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixIDDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixNameDataGridViewTextBoxColumn;

        private System.Windows.Forms.RichTextBox matrixRTB;

     //   private System.Windows.Forms.SplitContainer splitContainer4;

        private System.Windows.Forms.SplitContainer splitContainer5;

        //   private System.Windows.Forms.TableLayoutPanel TLPS;
       // private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;

        private System.Windows.Forms.TableLayoutPanel TLPMatrix;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;

        private System.Windows.Forms.DataGridViewCheckBoxColumn xCOMDataGridViewCheckBoxColumn;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucComposition2));
            this.TLPMatrix = new System.Windows.Forms.TableLayoutPanel();
            this.SC = new System.Windows.Forms.SplitContainer();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.Linaa = new DB.LINAA();
            this.matrixRTB = new System.Windows.Forms.RichTextBox();
            this.matrixIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixCompositionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixDensityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xCOMDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.listView1 = new System.Windows.Forms.ListView();
            this.TLPMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC)).BeginInit();
            this.SC.Panel1.SuspendLayout();
            this.SC.Panel2.SuspendLayout();
            this.SC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // TLPMatrix
            // 
            this.TLPMatrix.ColumnCount = 1;
            this.TLPMatrix.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPMatrix.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLPMatrix.Controls.Add(this.SC, 0, 0);
            this.TLPMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLPMatrix.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TLPMatrix.Location = new System.Drawing.Point(0, 0);
            this.TLPMatrix.Name = "TLPMatrix";
            this.TLPMatrix.RowCount = 1;
            this.TLPMatrix.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPMatrix.Size = new System.Drawing.Size(706, 493);
            this.TLPMatrix.TabIndex = 6;
            // 
            // SC
            // 
            this.SC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC.Location = new System.Drawing.Point(3, 3);
            this.SC.Name = "SC";
            this.SC.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SC.Panel1
            // 
            this.SC.Panel1.Controls.Add(this.listView1);
            // 
            // SC.Panel2
            // 
            this.SC.Panel2.Controls.Add(this.matrixRTB);
            this.SC.Size = new System.Drawing.Size(700, 487);
            this.SC.SplitterDistance = 276;
            this.SC.TabIndex = 8;
            // 
            // bs
            // 
            this.bs.DataMember = "Compositions";
            this.bs.DataSource = this.Linaa;
            // 
            // Linaa
            // 
            this.Linaa.DataSetName = "LINAA";
            this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
            this.Linaa.EnforceConstraints = false;
            this.Linaa.FolderPath = null;
            this.Linaa.Locale = new System.Globalization.CultureInfo("");
            this.Linaa.QTA = null;
            this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.Linaa.TAM = null;
            // 
            // matrixRTB
            // 
            this.matrixRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matrixRTB.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matrixRTB.Location = new System.Drawing.Point(0, 0);
            this.matrixRTB.Margin = new System.Windows.Forms.Padding(5);
            this.matrixRTB.Name = "matrixRTB";
            this.matrixRTB.Size = new System.Drawing.Size(700, 207);
            this.matrixRTB.TabIndex = 4;
            this.matrixRTB.Text = "";
            // 
            // matrixIDDataGridViewTextBoxColumn
            // 
            this.matrixIDDataGridViewTextBoxColumn.DataPropertyName = "MatrixID";
            this.matrixIDDataGridViewTextBoxColumn.HeaderText = "MatrixID";
            this.matrixIDDataGridViewTextBoxColumn.Name = "matrixIDDataGridViewTextBoxColumn";
            this.matrixIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.matrixIDDataGridViewTextBoxColumn.Width = 83;
            // 
            // matrixNameDataGridViewTextBoxColumn
            // 
            this.matrixNameDataGridViewTextBoxColumn.DataPropertyName = "MatrixName";
            this.matrixNameDataGridViewTextBoxColumn.HeaderText = "MatrixName";
            this.matrixNameDataGridViewTextBoxColumn.Name = "matrixNameDataGridViewTextBoxColumn";
            this.matrixNameDataGridViewTextBoxColumn.Width = 107;
            // 
            // matrixCompositionDataGridViewTextBoxColumn
            // 
            this.matrixCompositionDataGridViewTextBoxColumn.DataPropertyName = "MatrixComposition";
            this.matrixCompositionDataGridViewTextBoxColumn.HeaderText = "MatrixComposition";
            this.matrixCompositionDataGridViewTextBoxColumn.Name = "matrixCompositionDataGridViewTextBoxColumn";
            this.matrixCompositionDataGridViewTextBoxColumn.Width = 147;
            // 
            // matrixDensityDataGridViewTextBoxColumn
            // 
            this.matrixDensityDataGridViewTextBoxColumn.DataPropertyName = "MatrixDensity";
            this.matrixDensityDataGridViewTextBoxColumn.HeaderText = "MatrixDensity";
            this.matrixDensityDataGridViewTextBoxColumn.Name = "matrixDensityDataGridViewTextBoxColumn";
            this.matrixDensityDataGridViewTextBoxColumn.Width = 117;
            // 
            // xCOMDataGridViewCheckBoxColumn
            // 
            this.xCOMDataGridViewCheckBoxColumn.DataPropertyName = "XCOM";
            this.xCOMDataGridViewCheckBoxColumn.HeaderText = "XCOM";
            this.xCOMDataGridViewCheckBoxColumn.Name = "xCOMDataGridViewCheckBoxColumn";
            this.xCOMDataGridViewCheckBoxColumn.ReadOnly = true;
            this.xCOMDataGridViewCheckBoxColumn.Width = 54;
            // 
            // cDataGridViewCheckBoxColumn
            // 
            this.cDataGridViewCheckBoxColumn.DataPropertyName = "C";
            this.cDataGridViewCheckBoxColumn.HeaderText = "C";
            this.cDataGridViewCheckBoxColumn.Name = "cDataGridViewCheckBoxColumn";
            this.cDataGridViewCheckBoxColumn.ReadOnly = true;
            this.cDataGridViewCheckBoxColumn.Width = 23;
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
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(700, 276);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // ucComposition2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLPMatrix);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucComposition2";
            this.Size = new System.Drawing.Size(706, 493);
            this.TLPMatrix.ResumeLayout(false);
            this.SC.Panel1.ResumeLayout(false);
            this.SC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SC)).EndInit();
            this.SC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SC;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.ListView listView1;
    }
}
