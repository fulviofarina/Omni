namespace DB.UI
{
    partial class ucMUES
    {

        private System.Windows.Forms.DataGridViewCheckBoxColumn cDataGridViewCheckBoxColumn;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DB.LINAA Linaa;

       // private System.Windows.Forms.DataGridViewTextBoxColumn matrixCompositionDataGridViewTextBoxColumn;

      //  private System.Windows.Forms.DataGridViewTextBoxColumn matrixDensityDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixIDDataGridViewTextBoxColumn;

   //     private System.Windows.Forms.DataGridViewTextBoxColumn matrixNameDataGridViewTextBoxColumn;

     //   private System.Windows.Forms.SplitContainer splitContainer4;

        private System.Windows.Forms.SplitContainer splitContainer5;

        //   private System.Windows.Forms.TableLayoutPanel TLPS;
       // private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;

        private System.Windows.Forms.TableLayoutPanel TLPMatrix;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;

   //     private System.Windows.Forms.DataGridViewCheckBoxColumn xCOMDataGridViewCheckBoxColumn;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMUES));
            this.TLPMatrix = new System.Windows.Forms.TableLayoutPanel();
            this.SC = new System.Windows.Forms.SplitContainer();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.energyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACSDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.mAISDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.pEDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.pPNFDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.pPEFDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.mATCSDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.mATNCSDataGridViewTextBoxColumn = new DB.UI.MUESColumn();
            this.mUDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edgeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.Linaa = new DB.LINAA();
            this.grapher = new VTools.Graph();
            this.matrixIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.TLPMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC)).BeginInit();
            this.SC.Panel1.SuspendLayout();
            this.SC.Panel2.SuspendLayout();
            this.SC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
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
            this.TLPMatrix.Size = new System.Drawing.Size(1257, 470);
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
            this.SC.Panel1.Controls.Add(this.DGV);
            // 
            // SC.Panel2
            // 
            this.SC.Panel2.Controls.Add(this.grapher);
            this.SC.Size = new System.Drawing.Size(1251, 464);
            this.SC.SplitterDistance = 225;
            this.SC.TabIndex = 8;
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.AutoGenerateColumns = false;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.DGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.DGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.energyDataGridViewTextBoxColumn,
            this.mACSDataGridViewTextBoxColumn,
            this.mAISDataGridViewTextBoxColumn,
            this.pEDataGridViewTextBoxColumn,
            this.pPNFDataGridViewTextBoxColumn,
            this.pPEFDataGridViewTextBoxColumn,
            this.mATCSDataGridViewTextBoxColumn,
            this.mATNCSDataGridViewTextBoxColumn,
            this.mUDataGridViewTextBoxColumn,
            this.edgeDataGridViewTextBoxColumn});
            this.DGV.DataSource = this.bs;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.EnableHeadersVisualStyles = false;
            this.DGV.GridColor = System.Drawing.Color.Black;
            this.DGV.Location = new System.Drawing.Point(0, 0);
            this.DGV.Name = "DGV";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.DGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.DGV.RowTemplate.Height = 24;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(1251, 225);
            this.DGV.TabIndex = 8;
            // 
            // energyDataGridViewTextBoxColumn
            // 
            this.energyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.energyDataGridViewTextBoxColumn.DataPropertyName = "Energy";
            dataGridViewCellStyle3.Format = "n2";
            this.energyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.energyDataGridViewTextBoxColumn.HeaderText = "Energy";
            this.energyDataGridViewTextBoxColumn.Name = "energyDataGridViewTextBoxColumn";
            this.energyDataGridViewTextBoxColumn.ToolTipText = "Photon energy in keV";
            this.energyDataGridViewTextBoxColumn.Width = 83;
            // 
            // mACSDataGridViewTextBoxColumn
            // 
            this.mACSDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.mACSDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.mACSDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.mACSDataGridViewTextBoxColumn.DataPropertyName = "MACS";
            this.mACSDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.mACSDataGridViewTextBoxColumn.HeaderText = "Coherent Scattering";
            this.mACSDataGridViewTextBoxColumn.Name = "mACSDataGridViewTextBoxColumn";
            this.mACSDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mACSDataGridViewTextBoxColumn.ToolTipText = "Mass attenuation for Coherent Scattering (g/cm2)";
            // 
            // mAISDataGridViewTextBoxColumn
            // 
            this.mAISDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.mAISDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.mAISDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.mAISDataGridViewTextBoxColumn.DataPropertyName = "MAIS";
            this.mAISDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.mAISDataGridViewTextBoxColumn.HeaderText = "Incoherent Scattering";
            this.mAISDataGridViewTextBoxColumn.Name = "mAISDataGridViewTextBoxColumn";
            this.mAISDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mAISDataGridViewTextBoxColumn.ToolTipText = "Mass attenuation for Incoherent Scattering (g/cm2)";
            // 
            // pEDataGridViewTextBoxColumn
            // 
            this.pEDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.pEDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.pEDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.pEDataGridViewTextBoxColumn.DataPropertyName = "PE";
            this.pEDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.pEDataGridViewTextBoxColumn.HeaderText = "Photo Electric";
            this.pEDataGridViewTextBoxColumn.Name = "pEDataGridViewTextBoxColumn";
            this.pEDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pEDataGridViewTextBoxColumn.ToolTipText = "Mass attenuation for Photoelectric Absorption (g/cm2)";
            // 
            // pPNFDataGridViewTextBoxColumn
            // 
            this.pPNFDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.pPNFDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.pPNFDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.pPNFDataGridViewTextBoxColumn.DataPropertyName = "PPNF";
            this.pPNFDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.pPNFDataGridViewTextBoxColumn.HeaderText = "Pair Production (NF)";
            this.pPNFDataGridViewTextBoxColumn.Name = "pPNFDataGridViewTextBoxColumn";
            this.pPNFDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pPNFDataGridViewTextBoxColumn.ToolTipText = "Mass attenuation by Pair Production under Neutron Field (g/cm2)";
            // 
            // pPEFDataGridViewTextBoxColumn
            // 
            this.pPEFDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.pPEFDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.pPEFDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.pPEFDataGridViewTextBoxColumn.DataPropertyName = "PPEF";
            this.pPEFDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.pPEFDataGridViewTextBoxColumn.HeaderText = "Pair Production (EF)";
            this.pPEFDataGridViewTextBoxColumn.Name = "pPEFDataGridViewTextBoxColumn";
            this.pPEFDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pPEFDataGridViewTextBoxColumn.ToolTipText = "Mass attenuation by Pair Production under Electron Field (g/cm2)";
            // 
            // mATCSDataGridViewTextBoxColumn
            // 
            this.mATCSDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.mATCSDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.mATCSDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.mATCSDataGridViewTextBoxColumn.DataPropertyName = "MATCS";
            this.mATCSDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.mATCSDataGridViewTextBoxColumn.HeaderText = "TOTAL";
            this.mATCSDataGridViewTextBoxColumn.Name = "mATCSDataGridViewTextBoxColumn";
            this.mATCSDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mATCSDataGridViewTextBoxColumn.ToolTipText = "Total Mass attenuation with Coherent Scattering (g/cm2)";
            // 
            // mATNCSDataGridViewTextBoxColumn
            // 
            this.mATNCSDataGridViewTextBoxColumn.BindingPreferenceField = "";
            this.mATNCSDataGridViewTextBoxColumn.BindingPreferenceRow = null;
            this.mATNCSDataGridViewTextBoxColumn.BindingRoundingField = "";
            this.mATNCSDataGridViewTextBoxColumn.DataPropertyName = "MATNCS";
            this.mATNCSDataGridViewTextBoxColumn.DefaultAction = Rsx.DGV.DefaultAction.Visibility;
            this.mATNCSDataGridViewTextBoxColumn.HeaderText = "TOTAL - CS";
            this.mATNCSDataGridViewTextBoxColumn.Name = "mATNCSDataGridViewTextBoxColumn";
            this.mATNCSDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mATNCSDataGridViewTextBoxColumn.ToolTipText = "Total Mass attenuation without Coherent Scattering (g/cm2)";
            // 
            // mUDataGridViewTextBoxColumn
            // 
            this.mUDataGridViewTextBoxColumn.DataPropertyName = "MU";
            this.mUDataGridViewTextBoxColumn.HeaderText = "MU";
            this.mUDataGridViewTextBoxColumn.Name = "mUDataGridViewTextBoxColumn";
            this.mUDataGridViewTextBoxColumn.ReadOnly = true;
            this.mUDataGridViewTextBoxColumn.Visible = false;
            // 
            // edgeDataGridViewTextBoxColumn
            // 
            this.edgeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.edgeDataGridViewTextBoxColumn.DataPropertyName = "Edge";
            this.edgeDataGridViewTextBoxColumn.HeaderText = "Edge";
            this.edgeDataGridViewTextBoxColumn.Name = "edgeDataGridViewTextBoxColumn";
            this.edgeDataGridViewTextBoxColumn.Visible = false;
            // 
            // bs
            // 
            this.bs.DataMember = "MUES";
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
            // grapher
            // 
            this.grapher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grapher.Location = new System.Drawing.Point(0, 0);
            this.grapher.Name = "grapher";
            this.grapher.Size = new System.Drawing.Size(1251, 235);
            this.grapher.TabIndex = 1;
            // 
            // matrixIDDataGridViewTextBoxColumn
            // 
            this.matrixIDDataGridViewTextBoxColumn.DataPropertyName = "MatrixID";
            this.matrixIDDataGridViewTextBoxColumn.HeaderText = "MatrixID";
            this.matrixIDDataGridViewTextBoxColumn.Name = "matrixIDDataGridViewTextBoxColumn";
            this.matrixIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.matrixIDDataGridViewTextBoxColumn.Width = 83;
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
            // ucMUES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLPMatrix);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucMUES";
            this.Size = new System.Drawing.Size(1257, 470);
            this.TLPMatrix.ResumeLayout(false);
            this.SC.Panel1.ResumeLayout(false);
            this.SC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SC)).EndInit();
            this.SC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SC;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.DataGridView DGV;
     //   private System.Windows.Forms.DataGridViewTextBoxColumn formulaDataGridViewTextBoxColumn;
    //    private System.Windows.Forms.DataGridViewTextBoxColumn weightDataGridViewTextBoxColumn;
        private VTools.Graph grapher;
        private System.Windows.Forms.DataGridViewTextBoxColumn energyDataGridViewTextBoxColumn;
        private MUESColumn mACSDataGridViewTextBoxColumn;
        private MUESColumn mAISDataGridViewTextBoxColumn;
        private MUESColumn pEDataGridViewTextBoxColumn;
        private MUESColumn pPNFDataGridViewTextBoxColumn;
        private MUESColumn pPEFDataGridViewTextBoxColumn;
        private MUESColumn mATCSDataGridViewTextBoxColumn;
        private MUESColumn mATNCSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mUDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn edgeDataGridViewTextBoxColumn;
    }
}
