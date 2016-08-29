namespace k0X
{
   partial class ucWatchDog
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
		 System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
		 System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucWatchDog));
		 System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		 System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		 System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		 this.SC = new System.Windows.Forms.SplitContainer();
		 this.TLP = new System.Windows.Forms.TableLayoutPanel();
		 this.sampleDGV = new System.Windows.Forms.DataGridView();
		 this.Order = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.DetectorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.PositionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.GeometryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		 this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		 this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		 this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.sampleCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
		 this.startAcquisition = new System.Windows.Forms.ToolStripMenuItem();
		 this.sampleBS = new System.Windows.Forms.BindingSource(this.components);
		 this.Linaa = new DB.LINAA();
		 this.sampleBN = new System.Windows.Forms.BindingNavigator(this.components);
		 this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
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
		 this.subSamplesBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
		 this.FilterMode = new System.Windows.Forms.ToolStripButton();
		 this.TAB = new System.Windows.Forms.TabControl();
		 this.ListTab = new System.Windows.Forms.TabPage();
		 this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
		 this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.startedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.CTSpan = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		 this.KW = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.measBS = new System.Windows.Forms.BindingSource(this.components);
		 this.xTableTab = new System.Windows.Forms.TabPage();
		 this.XtableDGV = new System.Windows.Forms.DataGridView();
		 this.MainTLP = new System.Windows.Forms.TableLayoutPanel();
		 this.WDTS = new System.Windows.Forms.ToolStrip();
		 this.browse = new System.Windows.Forms.ToolStripButton();
		 this.Dirbox = new System.Windows.Forms.ToolStripTextBox();
		 this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		 this.List = new System.Windows.Forms.ToolStripButton();
		 this.progress = new System.Windows.Forms.ToolStripProgressBar();
		 this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		 this.Export = new System.Windows.Forms.ToolStripButton();
		 this.FBD = new System.Windows.Forms.FolderBrowserDialog();
		 this.watcher = new System.IO.FileSystemWatcher();
		 ((System.ComponentModel.ISupportInitialize)(this.SC)).BeginInit();
		 this.SC.Panel1.SuspendLayout();
		 this.SC.Panel2.SuspendLayout();
		 this.SC.SuspendLayout();
		 this.TLP.SuspendLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.sampleDGV)).BeginInit();
		 this.sampleCMS.SuspendLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.sampleBS)).BeginInit();
		 ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
		 ((System.ComponentModel.ISupportInitialize)(this.sampleBN)).BeginInit();
		 this.sampleBN.SuspendLayout();
		 this.TAB.SuspendLayout();
		 this.ListTab.SuspendLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
		 ((System.ComponentModel.ISupportInitialize)(this.measBS)).BeginInit();
		 this.xTableTab.SuspendLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.XtableDGV)).BeginInit();
		 this.MainTLP.SuspendLayout();
		 this.WDTS.SuspendLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.watcher)).BeginInit();
		 this.SuspendLayout();
		 // 
		 // SC
		 // 
		 this.SC.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.SC.Location = new System.Drawing.Point(2, 2);
		 this.SC.Margin = new System.Windows.Forms.Padding(2);
		 this.SC.Name = "SC";
		 // 
		 // SC.Panel1
		 // 
		 this.SC.Panel1.Controls.Add(this.TLP);
		 // 
		 // SC.Panel2
		 // 
		 this.SC.Panel2.AutoScroll = true;
		 this.SC.Panel2.Controls.Add(this.TAB);
		 this.SC.Size = new System.Drawing.Size(1178, 506);
		 this.SC.SplitterDistance = 396;
		 this.SC.SplitterWidth = 3;
		 this.SC.TabIndex = 0;
		 // 
		 // TLP
		 // 
		 this.TLP.ColumnCount = 1;
		 this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
		 this.TLP.Controls.Add(this.sampleDGV, 0, 1);
		 this.TLP.Controls.Add(this.sampleBN, 0, 0);
		 this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.TLP.Location = new System.Drawing.Point(0, 0);
		 this.TLP.Margin = new System.Windows.Forms.Padding(2);
		 this.TLP.Name = "TLP";
		 this.TLP.RowCount = 2;
		 this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.737705F));
		 this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.2623F));
		 this.TLP.Size = new System.Drawing.Size(396, 506);
		 this.TLP.TabIndex = 0;
		 // 
		 // sampleDGV
		 // 
		 this.sampleDGV.AllowUserToAddRows = false;
		 this.sampleDGV.AllowUserToDeleteRows = false;
		 this.sampleDGV.AllowUserToOrderColumns = true;
		 this.sampleDGV.AutoGenerateColumns = false;
		 this.sampleDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
		 this.sampleDGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
		 this.sampleDGV.BackgroundColor = System.Drawing.Color.LavenderBlush;
		 this.sampleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		 this.sampleDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Order,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.DetectorColumn,
            this.PositionColumn,
            this.GeometryColumn,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewTextBoxColumn27,
            this.dataGridViewCheckBoxColumn3,
            this.dataGridViewTextBoxColumn29,
            this.dataGridViewTextBoxColumn30});
		 this.sampleDGV.ContextMenuStrip = this.sampleCMS;
		 this.sampleDGV.DataSource = this.sampleBS;
		 dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		 dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
		 dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		 dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
		 dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		 dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		 dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		 this.sampleDGV.DefaultCellStyle = dataGridViewCellStyle1;
		 this.sampleDGV.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.sampleDGV.Location = new System.Drawing.Point(2, 31);
		 this.sampleDGV.Margin = new System.Windows.Forms.Padding(2);
		 this.sampleDGV.Name = "sampleDGV";
		 this.sampleDGV.RowTemplate.Height = 24;
		 this.sampleDGV.Size = new System.Drawing.Size(392, 473);
		 this.sampleDGV.TabIndex = 0;
		 this.sampleDGV.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.sampleDGV_RowPrePaint);
		 this.sampleDGV.SelectionChanged += new System.EventHandler(this.sampleDGV_SelectionChanged);
		 // 
		 // Order
		 // 
		 this.Order.DataPropertyName = "Order";
		 this.Order.HeaderText = "Order";
		 this.Order.Name = "Order";
		 this.Order.Width = 58;
		 // 
		 // dataGridViewTextBoxColumn2
		 // 
		 this.dataGridViewTextBoxColumn2.DataPropertyName = "SubSampleName";
		 this.dataGridViewTextBoxColumn2.HeaderText = "Sample";
		 this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
		 this.dataGridViewTextBoxColumn2.Width = 67;
		 // 
		 // dataGridViewTextBoxColumn3
		 // 
		 this.dataGridViewTextBoxColumn3.DataPropertyName = "SubSampleDescription";
		 this.dataGridViewTextBoxColumn3.HeaderText = "Description";
		 this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		 this.dataGridViewTextBoxColumn3.Width = 85;
		 // 
		 // DetectorColumn
		 // 
		 this.DetectorColumn.DataPropertyName = "Detector";
		 this.DetectorColumn.HeaderText = "Detector";
		 this.DetectorColumn.Name = "DetectorColumn";
		 this.DetectorColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		 this.DetectorColumn.Width = 73;
		 // 
		 // PositionColumn
		 // 
		 this.PositionColumn.DataPropertyName = "Position";
		 this.PositionColumn.HeaderText = "Position";
		 this.PositionColumn.Name = "PositionColumn";
		 this.PositionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		 this.PositionColumn.Width = 69;
		 // 
		 // GeometryColumn
		 // 
		 this.GeometryColumn.DataPropertyName = "GeometryName";
		 this.GeometryColumn.HeaderText = "Geometry";
		 this.GeometryColumn.Name = "GeometryColumn";
		 this.GeometryColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		 this.GeometryColumn.Width = 77;
		 // 
		 // dataGridViewCheckBoxColumn1
		 // 
		 this.dataGridViewCheckBoxColumn1.DataPropertyName = "DirectSolcoi";
		 this.dataGridViewCheckBoxColumn1.HeaderText = "DirectSolcoi";
		 this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
		 this.dataGridViewCheckBoxColumn1.Width = 70;
		 // 
		 // dataGridViewCheckBoxColumn2
		 // 
		 this.dataGridViewCheckBoxColumn2.DataPropertyName = "ENAA";
		 this.dataGridViewCheckBoxColumn2.HeaderText = "ENAA";
		 this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
		 this.dataGridViewCheckBoxColumn2.ReadOnly = true;
		 this.dataGridViewCheckBoxColumn2.Width = 42;
		 // 
		 // dataGridViewTextBoxColumn27
		 // 
		 this.dataGridViewTextBoxColumn27.DataPropertyName = "MatrixName";
		 this.dataGridViewTextBoxColumn27.HeaderText = "Matrix";
		 this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
		 this.dataGridViewTextBoxColumn27.ReadOnly = true;
		 this.dataGridViewTextBoxColumn27.Width = 60;
		 // 
		 // dataGridViewCheckBoxColumn3
		 // 
		 this.dataGridViewCheckBoxColumn3.DataPropertyName = "Comparator";
		 this.dataGridViewCheckBoxColumn3.HeaderText = "Comparator";
		 this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
		 this.dataGridViewCheckBoxColumn3.Width = 67;
		 // 
		 // dataGridViewTextBoxColumn29
		 // 
		 this.dataGridViewTextBoxColumn29.DataPropertyName = "Concentration";
		 this.dataGridViewTextBoxColumn29.HeaderText = "Concentration";
		 this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
		 this.dataGridViewTextBoxColumn29.Width = 98;
		 // 
		 // dataGridViewTextBoxColumn30
		 // 
		 this.dataGridViewTextBoxColumn30.DataPropertyName = "Elements";
		 this.dataGridViewTextBoxColumn30.HeaderText = "Elements";
		 this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
		 this.dataGridViewTextBoxColumn30.Width = 75;
		 // 
		 // sampleCMS
		 // 
		 this.sampleCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startAcquisition});
		 this.sampleCMS.Name = "sampleCMS";
		 this.sampleCMS.Size = new System.Drawing.Size(162, 26);
		 // 
		 // startAcquisition
		 // 
		 this.startAcquisition.Name = "startAcquisition";
		 this.startAcquisition.Size = new System.Drawing.Size(161, 22);
		 this.startAcquisition.Text = "Start Acquisition";
		 // 
		 // sampleBS
		 // 
		 this.sampleBS.DataMember = "SubSamples";
		 this.sampleBS.DataSource = this.Linaa;
		 // 
		 // Linaa
		 // 
		 this.Linaa.CurrentPref = null;
		 this.Linaa.DataSetName = "LINAA";
		 this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
		 this.Linaa.EnforceConstraints = false;
		 this.Linaa.FolderPath = null;
	//	 this.Linaa.IStore = this.Linaa;
		 this.Linaa.Locale = new System.Globalization.CultureInfo("");
		 this.Linaa.Notify = null;
		 this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
		 this.Linaa.TAM = null;
		 // 
		 // sampleBN
		 // 
		 this.sampleBN.AddNewItem = this.bindingNavigatorAddNewItem;
		 this.sampleBN.BindingSource = this.sampleBS;
		 this.sampleBN.CountItem = this.bindingNavigatorCountItem;
		 this.sampleBN.DeleteItem = this.bindingNavigatorDeleteItem;
		 this.sampleBN.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.sampleBN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.subSamplesBindingNavigatorSaveItem,
            this.FilterMode});
		 this.sampleBN.Location = new System.Drawing.Point(0, 0);
		 this.sampleBN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
		 this.sampleBN.MoveLastItem = this.bindingNavigatorMoveLastItem;
		 this.sampleBN.MoveNextItem = this.bindingNavigatorMoveNextItem;
		 this.sampleBN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
		 this.sampleBN.Name = "sampleBN";
		 this.sampleBN.PositionItem = this.bindingNavigatorPositionItem;
		 this.sampleBN.Size = new System.Drawing.Size(396, 29);
		 this.sampleBN.TabIndex = 1;
		 this.sampleBN.Text = "bindingNavigator1";
		 // 
		 // bindingNavigatorAddNewItem
		 // 
		 this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
		 this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
		 this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
		 this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 26);
		 this.bindingNavigatorAddNewItem.Text = "Add new";
		 // 
		 // bindingNavigatorCountItem
		 // 
		 this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
		 this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 26);
		 this.bindingNavigatorCountItem.Text = "of {0}";
		 this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
		 // 
		 // bindingNavigatorDeleteItem
		 // 
		 this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
		 this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
		 this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
		 this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 26);
		 this.bindingNavigatorDeleteItem.Text = "Delete";
		 // 
		 // bindingNavigatorMoveFirstItem
		 // 
		 this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
		 this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
		 this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
		 this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 26);
		 this.bindingNavigatorMoveFirstItem.Text = "Move first";
		 // 
		 // bindingNavigatorMovePreviousItem
		 // 
		 this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
		 this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
		 this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
		 this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 26);
		 this.bindingNavigatorMovePreviousItem.Text = "Move previous";
		 // 
		 // bindingNavigatorSeparator
		 // 
		 this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
		 this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 29);
		 // 
		 // bindingNavigatorPositionItem
		 // 
		 this.bindingNavigatorPositionItem.AccessibleName = "Position";
		 this.bindingNavigatorPositionItem.AutoSize = false;
		 this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
		 this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(38, 23);
		 this.bindingNavigatorPositionItem.Text = "0";
		 this.bindingNavigatorPositionItem.ToolTipText = "Current position";
		 // 
		 // bindingNavigatorSeparator1
		 // 
		 this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
		 this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 29);
		 // 
		 // bindingNavigatorMoveNextItem
		 // 
		 this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
		 this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
		 this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
		 this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 26);
		 this.bindingNavigatorMoveNextItem.Text = "Move next";
		 // 
		 // bindingNavigatorMoveLastItem
		 // 
		 this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
		 this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
		 this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
		 this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 26);
		 this.bindingNavigatorMoveLastItem.Text = "Move last";
		 // 
		 // bindingNavigatorSeparator2
		 // 
		 this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
		 this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 29);
		 // 
		 // subSamplesBindingNavigatorSaveItem
		 // 
		 this.subSamplesBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		 this.subSamplesBindingNavigatorSaveItem.Enabled = false;
		 this.subSamplesBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("subSamplesBindingNavigatorSaveItem.Image")));
		 this.subSamplesBindingNavigatorSaveItem.Name = "subSamplesBindingNavigatorSaveItem";
		 this.subSamplesBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 26);
		 this.subSamplesBindingNavigatorSaveItem.Text = "Save Data";
		 // 
		 // FilterMode
		 // 
		 this.FilterMode.CheckOnClick = true;
		 this.FilterMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		 this.FilterMode.Image = ((System.Drawing.Image)(resources.GetObject("FilterMode.Image")));
		 this.FilterMode.ImageTransparentColor = System.Drawing.Color.Magenta;
		 this.FilterMode.Name = "FilterMode";
		 this.FilterMode.Size = new System.Drawing.Size(62, 26);
		 this.FilterMode.Text = "All Visible";
		 this.FilterMode.Click += new System.EventHandler(this.FilterMode_Click);
		 // 
		 // TAB
		 // 
		 this.TAB.Controls.Add(this.ListTab);
		 this.TAB.Controls.Add(this.xTableTab);
		 this.TAB.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.TAB.Location = new System.Drawing.Point(0, 0);
		 this.TAB.Name = "TAB";
		 this.TAB.SelectedIndex = 0;
		 this.TAB.Size = new System.Drawing.Size(779, 506);
		 this.TAB.TabIndex = 1;
		 this.TAB.Selected += new System.Windows.Forms.TabControlEventHandler(this.TAB_Selected);
		 // 
		 // ListTab
		 // 
		 this.ListTab.Controls.Add(this.measurementsDataGridView);
		 this.ListTab.Location = new System.Drawing.Point(4, 22);
		 this.ListTab.Name = "ListTab";
		 this.ListTab.Padding = new System.Windows.Forms.Padding(3);
		 this.ListTab.Size = new System.Drawing.Size(771, 480);
		 this.ListTab.TabIndex = 0;
		 this.ListTab.Text = "Full List";
		 this.ListTab.UseVisualStyleBackColor = true;
		 // 
		 // measurementsDataGridView
		 // 
		 this.measurementsDataGridView.AllowUserToAddRows = false;
		 this.measurementsDataGridView.AllowUserToDeleteRows = false;
		 this.measurementsDataGridView.AllowUserToOrderColumns = true;
		 this.measurementsDataGridView.AutoGenerateColumns = false;
		 this.measurementsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
		 this.measurementsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
		 this.measurementsDataGridView.BackgroundColor = System.Drawing.Color.Thistle;
		 this.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		 this.measurementsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sample,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn14,
            this.startedCol,
            this.CTSpan,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewCheckBoxColumn4,
            this.KW,
            this.dataGridViewTextBoxColumn17});
		 this.measurementsDataGridView.DataSource = this.measBS;
		 dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		 dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
		 dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		 dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
		 dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		 dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		 dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		 this.measurementsDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
		 this.measurementsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.measurementsDataGridView.Location = new System.Drawing.Point(3, 3);
		 this.measurementsDataGridView.Margin = new System.Windows.Forms.Padding(2);
		 this.measurementsDataGridView.Name = "measurementsDataGridView";
		 this.measurementsDataGridView.RowTemplate.Height = 24;
		 this.measurementsDataGridView.Size = new System.Drawing.Size(765, 474);
		 this.measurementsDataGridView.TabIndex = 0;
		 this.measurementsDataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.measurementsDataGridView_RowPrePaint);
		 // 
		 // Sample
		 // 
		 this.Sample.DataPropertyName = "Sample";
		 this.Sample.HeaderText = "Sample";
		 this.Sample.Name = "Sample";
		 this.Sample.Width = 67;
		 // 
		 // dataGridViewTextBoxColumn8
		 // 
		 this.dataGridViewTextBoxColumn8.DataPropertyName = "Measurement";
		 this.dataGridViewTextBoxColumn8.HeaderText = "Measurement";
		 this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
		 this.dataGridViewTextBoxColumn8.Width = 96;
		 // 
		 // dataGridViewTextBoxColumn5
		 // 
		 this.dataGridViewTextBoxColumn5.DataPropertyName = "MeasurementNr";
		 this.dataGridViewTextBoxColumn5.HeaderText = "Nr";
		 this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		 this.dataGridViewTextBoxColumn5.Width = 43;
		 // 
		 // dataGridViewTextBoxColumn12
		 // 
		 this.dataGridViewTextBoxColumn12.DataPropertyName = "Detector";
		 this.dataGridViewTextBoxColumn12.HeaderText = "Detector";
		 this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
		 this.dataGridViewTextBoxColumn12.Width = 73;
		 // 
		 // dataGridViewTextBoxColumn14
		 // 
		 this.dataGridViewTextBoxColumn14.DataPropertyName = "Position";
		 this.dataGridViewTextBoxColumn14.HeaderText = "Position";
		 this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
		 this.dataGridViewTextBoxColumn14.Width = 69;
		 // 
		 // startedCol
		 // 
		 this.startedCol.DataPropertyName = "MeasurementStart";
		 this.startedCol.HeaderText = "Started";
		 this.startedCol.Name = "startedCol";
		 this.startedCol.Width = 66;
		 // 
		 // CTSpan
		 // 
		 this.CTSpan.DataPropertyName = "CTSpan";
		 this.CTSpan.HeaderText = "Count Time";
		 this.CTSpan.Name = "CTSpan";
		 this.CTSpan.Width = 86;
		 // 
		 // dataGridViewTextBoxColumn4
		 // 
		 this.dataGridViewTextBoxColumn4.DataPropertyName = "DT";
		 dataGridViewCellStyle2.Format = "N1";
		 this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle2;
		 this.dataGridViewTextBoxColumn4.HeaderText = "DT";
		 this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		 this.dataGridViewTextBoxColumn4.ReadOnly = true;
		 this.dataGridViewTextBoxColumn4.Width = 47;
		 // 
		 // dataGridViewCheckBoxColumn4
		 // 
		 this.dataGridViewCheckBoxColumn4.DataPropertyName = "HL";
		 this.dataGridViewCheckBoxColumn4.HeaderText = "HL";
		 this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
		 this.dataGridViewCheckBoxColumn4.Width = 27;
		 // 
		 // KW
		 // 
		 this.KW.DataPropertyName = "KW";
		 this.KW.HeaderText = "KW";
		 this.KW.Name = "KW";
		 this.KW.Width = 50;
		 // 
		 // dataGridViewTextBoxColumn17
		 // 
		 this.dataGridViewTextBoxColumn17.DataPropertyName = "DecayTime";
		 dataGridViewCellStyle3.Format = "N1";
		 this.dataGridViewTextBoxColumn17.DefaultCellStyle = dataGridViewCellStyle3;
		 this.dataGridViewTextBoxColumn17.HeaderText = "DecayTime";
		 this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
		 this.dataGridViewTextBoxColumn17.Width = 86;
		 // 
		 // measBS
		 // 
		 this.measBS.DataMember = "Measurements";
		 this.measBS.DataSource = this.Linaa;
		 // 
		 // xTableTab
		 // 
		 this.xTableTab.Controls.Add(this.XtableDGV);
		 this.xTableTab.ForeColor = System.Drawing.Color.Maroon;
		 this.xTableTab.Location = new System.Drawing.Point(4, 22);
		 this.xTableTab.Name = "xTableTab";
		 this.xTableTab.Padding = new System.Windows.Forms.Padding(3);
		 this.xTableTab.Size = new System.Drawing.Size(771, 480);
		 this.xTableTab.TabIndex = 1;
		 this.xTableTab.Text = "Cross Table";
		 this.xTableTab.UseVisualStyleBackColor = true;
		 // 
		 // XtableDGV
		 // 
		 this.XtableDGV.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
		 this.XtableDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		 this.XtableDGV.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.XtableDGV.Location = new System.Drawing.Point(3, 3);
		 this.XtableDGV.Name = "XtableDGV";
		 this.XtableDGV.Size = new System.Drawing.Size(765, 474);
		 this.XtableDGV.TabIndex = 0;
		 // 
		 // MainTLP
		 // 
		 this.MainTLP.ColumnCount = 1;
		 this.MainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
		 this.MainTLP.Controls.Add(this.SC, 0, 0);
		 this.MainTLP.Controls.Add(this.WDTS, 0, 1);
		 this.MainTLP.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.MainTLP.Location = new System.Drawing.Point(0, 0);
		 this.MainTLP.Margin = new System.Windows.Forms.Padding(2);
		 this.MainTLP.Name = "MainTLP";
		 this.MainTLP.RowCount = 2;
		 this.MainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.61967F));
		 this.MainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.380334F));
		 this.MainTLP.Size = new System.Drawing.Size(1182, 539);
		 this.MainTLP.TabIndex = 1;
		 // 
		 // WDTS
		 // 
		 this.WDTS.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.WDTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browse,
            this.Dirbox,
            this.toolStripSeparator2,
            this.List,
            this.progress,
            this.toolStripSeparator5,
            this.Export});
		 this.WDTS.Location = new System.Drawing.Point(0, 510);
		 this.WDTS.Name = "WDTS";
		 this.WDTS.Size = new System.Drawing.Size(1182, 29);
		 this.WDTS.TabIndex = 3;
		 // 
		 // browse
		 // 
		 this.browse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		 this.browse.Image = ((System.Drawing.Image)(resources.GetObject("browse.Image")));
		 this.browse.ImageTransparentColor = System.Drawing.Color.Magenta;
		 this.browse.Name = "browse";
		 this.browse.Size = new System.Drawing.Size(69, 26);
		 this.browse.Text = "Browse";
		 this.browse.Click += new System.EventHandler(this.browse_Click);
		 // 
		 // Dirbox
		 // 
		 this.Dirbox.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		 this.Dirbox.Name = "Dirbox";
		 this.Dirbox.Size = new System.Drawing.Size(700, 29);
		 this.Dirbox.Text = "X1035";
		 // 
		 // toolStripSeparator2
		 // 
		 this.toolStripSeparator2.Name = "toolStripSeparator2";
		 this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
		 // 
		 // List
		 // 
		 this.List.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		 this.List.ForeColor = System.Drawing.Color.Crimson;
		 this.List.Image = global::k0X.Properties.Resources.Atom;
		 this.List.ImageTransparentColor = System.Drawing.Color.Magenta;
		 this.List.Name = "List";
		 this.List.Size = new System.Drawing.Size(63, 26);
		 this.List.Text = "Watch";
		 this.List.Click += new System.EventHandler(this.FindMeasurements);
		 // 
		 // progress
		 // 
		 this.progress.Name = "progress";
		 this.progress.Size = new System.Drawing.Size(80, 26);
		 // 
		 // toolStripSeparator5
		 // 
		 this.toolStripSeparator5.Name = "toolStripSeparator5";
		 this.toolStripSeparator5.Size = new System.Drawing.Size(6, 29);
		 // 
		 // Export
		 // 
		 this.Export.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		 this.Export.ForeColor = System.Drawing.SystemColors.MenuHighlight;
		 this.Export.ImageTransparentColor = System.Drawing.Color.Magenta;
		 this.Export.Name = "Export";
		 this.Export.Size = new System.Drawing.Size(56, 26);
		 this.Export.Text = "REPORT";
		 this.Export.Click += new System.EventHandler(this.ReportMeasurements);
		 // 
		 // FBD
		 // 
		 this.FBD.Description = "Folder to watch...";
		 this.FBD.RootFolder = System.Environment.SpecialFolder.MyComputer;
		 // 
		 // watcher
		 // 
		 this.watcher.EnableRaisingEvents = true;
		 this.watcher.Filter = "*.CNF";
		 this.watcher.IncludeSubdirectories = true;
		 this.watcher.NotifyFilter = System.IO.NotifyFilters.FileName;
		 this.watcher.SynchronizingObject = this;
		 this.watcher.Changed += new System.IO.FileSystemEventHandler(this.watcher_Changed);
		 this.watcher.Created += new System.IO.FileSystemEventHandler(this.watcher_Created);
		 this.watcher.Deleted += new System.IO.FileSystemEventHandler(this.watcher_Deleted);
		 this.watcher.Renamed += new System.IO.RenamedEventHandler(this.watcher_Renamed);
		 // 
		 // ucWatchDog
		 // 
		 this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		 this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		 this.Controls.Add(this.MainTLP);
		 this.Margin = new System.Windows.Forms.Padding(2);
		 this.Name = "ucWatchDog";
		 this.Size = new System.Drawing.Size(1182, 539);
		 this.SC.Panel1.ResumeLayout(false);
		 this.SC.Panel2.ResumeLayout(false);
		 ((System.ComponentModel.ISupportInitialize)(this.SC)).EndInit();
		 this.SC.ResumeLayout(false);
		 this.TLP.ResumeLayout(false);
		 this.TLP.PerformLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.sampleDGV)).EndInit();
		 this.sampleCMS.ResumeLayout(false);
		 ((System.ComponentModel.ISupportInitialize)(this.sampleBS)).EndInit();
		 ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
		 ((System.ComponentModel.ISupportInitialize)(this.sampleBN)).EndInit();
		 this.sampleBN.ResumeLayout(false);
		 this.sampleBN.PerformLayout();
		 this.TAB.ResumeLayout(false);
		 this.ListTab.ResumeLayout(false);
		 ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
		 ((System.ComponentModel.ISupportInitialize)(this.measBS)).EndInit();
		 this.xTableTab.ResumeLayout(false);
		 ((System.ComponentModel.ISupportInitialize)(this.XtableDGV)).EndInit();
		 this.MainTLP.ResumeLayout(false);
		 this.MainTLP.PerformLayout();
		 this.WDTS.ResumeLayout(false);
		 this.WDTS.PerformLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.watcher)).EndInit();
		 this.ResumeLayout(false);

	  }

	  #endregion

	  private System.Windows.Forms.SplitContainer SC;
	  private System.Windows.Forms.TableLayoutPanel TLP;
	  private System.Windows.Forms.DataGridView sampleDGV;
	  private System.Windows.Forms.BindingSource sampleBS;
	  private DB.LINAA Linaa;
	  private System.Windows.Forms.BindingNavigator sampleBN;
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
	  private System.Windows.Forms.ToolStripButton subSamplesBindingNavigatorSaveItem;
	  private System.Windows.Forms.TableLayoutPanel MainTLP;
	  private System.Windows.Forms.DataGridView measurementsDataGridView;
	  private System.Windows.Forms.BindingSource measBS;
	  private System.Windows.Forms.FolderBrowserDialog FBD;
	  private System.IO.FileSystemWatcher watcher;
	  private System.Windows.Forms.ToolStrip WDTS;
	  private System.Windows.Forms.ToolStripButton browse;
	  private System.Windows.Forms.ToolStripTextBox Dirbox;
	  private System.Windows.Forms.ToolStripButton List;
	  private System.Windows.Forms.ToolStripProgressBar progress;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
	  private System.Windows.Forms.ContextMenuStrip sampleCMS;
	  private System.Windows.Forms.ToolStripMenuItem startAcquisition;
	  private System.Windows.Forms.ToolStripButton Export;
	  private System.Windows.Forms.ToolStripButton FilterMode;
	  private System.Windows.Forms.DataGridViewTextBoxColumn Order;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
	  private System.Windows.Forms.DataGridViewTextBoxColumn DetectorColumn;
	  private System.Windows.Forms.DataGridViewTextBoxColumn PositionColumn;
	  private System.Windows.Forms.DataGridViewTextBoxColumn GeometryColumn;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	  private System.Windows.Forms.TabControl TAB;
	  private System.Windows.Forms.TabPage ListTab;
	  private System.Windows.Forms.TabPage xTableTab;
	  private System.Windows.Forms.DataGridView XtableDGV;
	  private System.Windows.Forms.DataGridViewTextBoxColumn Sample;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
	  private System.Windows.Forms.DataGridViewTextBoxColumn startedCol;
	  private System.Windows.Forms.DataGridViewTextBoxColumn CTSpan;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
	  private System.Windows.Forms.DataGridViewTextBoxColumn KW;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
   }
}
