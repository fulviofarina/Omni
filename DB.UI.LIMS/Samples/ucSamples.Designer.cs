namespace DB.UI
{
    partial class ucSamples
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSamples));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.SC = new System.Windows.Forms.SplitContainer();
            this.TS = new System.Windows.Forms.ToolStrip();
            this.orderlabel = new System.Windows.Forms.ToolStripLabel();
            this.orderbox = new System.Windows.Forms.ToolStripComboBox();
            this.BN = new System.Windows.Forms.BindingNavigator(this.components);
            this.BS = new System.Windows.Forms.BindingSource(this.components);
            this.Linaa = new DB.LINAA();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.samplesBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.OrdersIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SamplesIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SampleDescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SampleReceiptionDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SampleAnalyseDateBeginColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SampleAnalyseDateEndColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerSampleReference1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerSampleReference2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerSampleReference3Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TA = new DB.LINAATableAdapters.SamplesTableAdapter();
            this.TAM = new DB.LINAATableAdapters.TableAdapterManager();
            this.TLP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC)).BeginInit();
            this.SC.Panel1.SuspendLayout();
            this.SC.Panel2.SuspendLayout();
            this.SC.SuspendLayout();
            this.TS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BN)).BeginInit();
            this.BN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 1;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP.Controls.Add(this.SC, 0, 0);
            this.TLP.Controls.Add(this.DGV, 0, 1);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(0, 0);
            this.TLP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 2;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.563025F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.43697F));
            this.TLP.Size = new System.Drawing.Size(1015, 476);
            this.TLP.TabIndex = 0;
            // 
            // SC
            // 
            this.SC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC.Location = new System.Drawing.Point(3, 2);
            this.SC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SC.Name = "SC";
            // 
            // SC.Panel1
            // 
            this.SC.Panel1.Controls.Add(this.TS);
            // 
            // SC.Panel2
            // 
            this.SC.Panel2.Controls.Add(this.BN);
            this.SC.Size = new System.Drawing.Size(1009, 31);
            this.SC.SplitterDistance = 413;
            this.SC.TabIndex = 0;
            // 
            // TS
            // 
            this.TS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orderlabel,
            this.orderbox});
            this.TS.Location = new System.Drawing.Point(0, 0);
            this.TS.Name = "TS";
            this.TS.Size = new System.Drawing.Size(413, 31);
            this.TS.TabIndex = 2;
            this.TS.Text = "fillByOrderIDToolStrip";
            // 
            // orderlabel
            // 
            this.orderlabel.Name = "orderlabel";
            this.orderlabel.Size = new System.Drawing.Size(89, 28);
            this.orderlabel.Text = "ParentOrder";
            // 
            // orderbox
            // 
            this.orderbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.orderbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.orderbox.Name = "orderbox";
            this.orderbox.Size = new System.Drawing.Size(151, 31);
            this.orderbox.TextChanged += new System.EventHandler(this.orderbox_TextChanged);
            // 
            // BN
            // 
            this.BN.AccessibleName = "BN";
            this.BN.AddNewItem = null;
            this.BN.AllowItemReorder = true;
            this.BN.BindingSource = this.BS;
            this.BN.CountItem = this.bindingNavigatorCountItem;
            this.BN.DeleteItem = this.bindingNavigatorDeleteItem;
            this.BN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.samplesBindingNavigatorSaveItem});
            this.BN.Location = new System.Drawing.Point(0, 0);
            this.BN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.BN.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.BN.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.BN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.BN.Name = "BN";
            this.BN.PositionItem = this.bindingNavigatorPositionItem;
            this.BN.Size = new System.Drawing.Size(592, 31);
            this.BN.TabIndex = 1;
            this.BN.Text = "bindingNavigator1";
            // 
            // BS
            // 
            this.BS.DataMember = "Samples";
            this.BS.DataSource = this.Linaa;
            // 
            // Linaa
            // 
        //    this.Linaa.CurrentPref = null;
            this.Linaa.DataSetName = "LINAA";
            this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
            this.Linaa.EnforceConstraints = false;
            this.Linaa.FolderPath = null;
       //     this.Linaa.IStore = this.Linaa;
            this.Linaa.Locale = new System.Globalization.CultureInfo("");
          //  this.Linaa.Notify = null;
            this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.Linaa.TAM = null;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(45, 28);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(49, 27);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // samplesBindingNavigatorSaveItem
            // 
            this.samplesBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.samplesBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("samplesBindingNavigatorSaveItem.Image")));
            this.samplesBindingNavigatorSaveItem.Name = "samplesBindingNavigatorSaveItem";
            this.samplesBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 28);
            this.samplesBindingNavigatorSaveItem.Text = "Save Data";
            // 
            // DGV
            // 
            this.DGV.AccessibleName = "DGV";
            this.DGV.AllowUserToOrderColumns = true;
            this.DGV.AutoGenerateColumns = false;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DGV.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrdersIDColumn,
            this.SamplesIDColumn,
            this.SampleDescriptionColumn,
            this.SampleReceiptionDateColumn,
            this.SampleAnalyseDateBeginColumn,
            this.SampleAnalyseDateEndColumn,
            this.CustomerSampleReference1Column,
            this.CustomerSampleReference2Column,
            this.CustomerSampleReference3Column});
            this.DGV.DataSource = this.BS;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(3, 37);
            this.DGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DGV.MultiSelect = false;
            this.DGV.Name = "DGV";
            this.DGV.RowTemplate.Height = 24;
            this.DGV.Size = new System.Drawing.Size(1009, 437);
            this.DGV.TabIndex = 1;
            // 
            // OrdersIDColumn
            // 
            this.OrdersIDColumn.DataPropertyName = "OrdersID";
            this.OrdersIDColumn.HeaderText = "OrdersID";
            this.OrdersIDColumn.Name = "OrdersIDColumn";
            this.OrdersIDColumn.Width = 90;
            // 
            // SamplesIDColumn
            // 
            this.SamplesIDColumn.DataPropertyName = "SamplesID";
            this.SamplesIDColumn.HeaderText = "SamplesID";
            this.SamplesIDColumn.Name = "SamplesIDColumn";
            this.SamplesIDColumn.ReadOnly = true;
            // 
            // SampleDescriptionColumn
            // 
            this.SampleDescriptionColumn.DataPropertyName = "SampleDescription";
            this.SampleDescriptionColumn.HeaderText = "SampleDescription";
            this.SampleDescriptionColumn.Name = "SampleDescriptionColumn";
            this.SampleDescriptionColumn.Width = 151;
            // 
            // SampleReceiptionDateColumn
            // 
            this.SampleReceiptionDateColumn.DataPropertyName = "SampleReceiptionDate";
            this.SampleReceiptionDateColumn.HeaderText = "SampleReceiptionDate";
            this.SampleReceiptionDateColumn.Name = "SampleReceiptionDateColumn";
            this.SampleReceiptionDateColumn.Width = 177;
            // 
            // SampleAnalyseDateBeginColumn
            // 
            this.SampleAnalyseDateBeginColumn.DataPropertyName = "SampleAnalyseDateBegin";
            this.SampleAnalyseDateBeginColumn.HeaderText = "SampleAnalyseDateBegin";
            this.SampleAnalyseDateBeginColumn.Name = "SampleAnalyseDateBeginColumn";
            this.SampleAnalyseDateBeginColumn.Width = 196;
            // 
            // SampleAnalyseDateEndColumn
            // 
            this.SampleAnalyseDateEndColumn.DataPropertyName = "SampleAnalyseDateEnd";
            this.SampleAnalyseDateEndColumn.HeaderText = "SampleAnalyseDateEnd";
            this.SampleAnalyseDateEndColumn.Name = "SampleAnalyseDateEndColumn";
            this.SampleAnalyseDateEndColumn.Width = 185;
            // 
            // CustomerSampleReference1Column
            // 
            this.CustomerSampleReference1Column.DataPropertyName = "CustomerSampleReference1";
            this.CustomerSampleReference1Column.HeaderText = "CustomerSampleReference1";
            this.CustomerSampleReference1Column.Name = "CustomerSampleReference1Column";
            this.CustomerSampleReference1Column.Width = 214;
            // 
            // CustomerSampleReference2Column
            // 
            this.CustomerSampleReference2Column.DataPropertyName = "CustomerSampleReference2";
            this.CustomerSampleReference2Column.HeaderText = "CustomerSampleReference2";
            this.CustomerSampleReference2Column.Name = "CustomerSampleReference2Column";
            this.CustomerSampleReference2Column.Width = 214;
            // 
            // CustomerSampleReference3Column
            // 
            this.CustomerSampleReference3Column.DataPropertyName = "CustomerSampleReference3";
            this.CustomerSampleReference3Column.HeaderText = "CustomerSampleReference3";
            this.CustomerSampleReference3Column.Name = "CustomerSampleReference3Column";
            this.CustomerSampleReference3Column.Width = 214;
            // 
            // TA
            // 
            this.TA.ClearBeforeFill = true;
            // 
            // TAM
            // 
      //      this.TAM.AcquisitionsTableAdapter = null;
            this.TAM.BackupDataSetBeforeUpdate = false;
            this.TAM.BlanksTableAdapter = null;
            this.TAM.ChannelsTableAdapter = null;
            this.TAM.COINTableAdapter = null;
            this.TAM.CompositionsTableAdapter = null;
            this.TAM.ContactPersonsTableAdapter = null;
            this.TAM.CustomerTableAdapter = null;
            this.TAM.DetectorsAbsorbersTableAdapter = null;
            this.TAM.DetectorsCurvesTableAdapter = null;
            this.TAM.DetectorsDimensionsTableAdapter = null;
            this.TAM.ElementsTableAdapter = null;
            this.TAM.GeometryTableAdapter = null;
            this.TAM.HoldersTableAdapter = null;
            this.TAM.IrradiationRequestsTableAdapter = null;
            this.TAM.k0NAATableAdapter = null;
            this.TAM.MatrixTableAdapter = null;
       //     this.TAM.MatSSFTableAdapter = null;
            this.TAM.MeasurementsTableAdapter = null;
            this.TAM.MonitorsFlagsTableAdapter = null;
            this.TAM.MonitorsTableAdapter = null;
            this.TAM.MUESTableAdapter = null;
            this.TAM.NAATableAdapter = null;
            this.TAM.OrdersTableAdapter = null;
            this.TAM.PeaksTableAdapter = null;
            this.TAM.ProjectsTableAdapter = null;
            this.TAM.pValuesTableAdapter = null;
            this.TAM.ReactionsTableAdapter = null;
            this.TAM.RefMaterialsTableAdapter = null;
            this.TAM.SamplesTableAdapter = this.TA;
            this.TAM.SchAcqsTableAdapter = null;
            this.TAM.SigmasSalTableAdapter = null;
            this.TAM.SigmasTableAdapter = null;
            this.TAM.SolangTableAdapter = null;
            this.TAM.StandardsTableAdapter = null;
            this.TAM.SubSamplesTableAdapter = null;
            this.TAM.ToDoTableAdapter = null;
            this.TAM.UpdateOrder = DB.LINAATableAdapters.TableAdapterManager.UpdateOrderOption.UpdateInsertDelete;
            this.TAM.VialTypeTableAdapter = null;
            this.TAM.YieldsTableAdapter = null;
            // 
            // ucSamples
            // 
            this.AccessibleName = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLP);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucSamples";
            this.Size = new System.Drawing.Size(1015, 476);
            this.TLP.ResumeLayout(false);
            this.SC.Panel1.ResumeLayout(false);
            this.SC.Panel1.PerformLayout();
            this.SC.Panel2.ResumeLayout(false);
            this.SC.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC)).EndInit();
            this.SC.ResumeLayout(false);
            this.TS.ResumeLayout(false);
            this.TS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BN)).EndInit();
            this.BN.ResumeLayout(false);
            this.BN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.SplitContainer SC;
        private DB.LINAA Linaa;
        private System.Windows.Forms.BindingSource BS;
        private DB.LINAATableAdapters.SamplesTableAdapter TA;
        private DB.LINAATableAdapters.TableAdapterManager TAM;
        private System.Windows.Forms.BindingNavigator BN;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton samplesBindingNavigatorSaveItem;
        private System.Windows.Forms.ToolStrip TS;
        private System.Windows.Forms.ToolStripLabel orderlabel;
        public System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ToolStripComboBox orderbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrdersIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SamplesIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleDescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleReceiptionDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleAnalyseDateBeginColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleAnalyseDateEndColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerSampleReference1Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerSampleReference2Column;
		private System.Windows.Forms.DataGridViewTextBoxColumn CustomerSampleReference3Column;
    }
}
