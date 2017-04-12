namespace DB.UI
{
    partial class ucMonitors
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
		   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMonitors));
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		   System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		   this.TSTLP = new System.Windows.Forms.TableLayoutPanel();
		   this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		   this.MonCodelabel = new System.Windows.Forms.ToolStripLabel();
		   this.MonCodebox = new System.Windows.Forms.ToolStripComboBox();
		   this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		   this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		   this.minBox = new System.Windows.Forms.ToolStripTextBox();
		   this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		   this.maxBox = new System.Windows.Forms.ToolStripTextBox();
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
		   this.monitorsBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
		   this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		   this.TLP = new System.Windows.Forms.TableLayoutPanel();
		   this.DGV = new System.Windows.Forms.DataGridView();
		   this.IsLockedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		   this.MonNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.MonGrossMass1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.MonGrossMass2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.MonGrossMassAvgColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.LastMassDateColumn = new Rsx.DGV.CalendarColumn();
		   this.ReweightDaysColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.LastIrradiationDateColumn = new Rsx.DGV.CalendarColumn();
		   this.DecayDaysColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.GeometryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.MonitorsIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.LastProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
		   this.TSTLP.SuspendLayout();
		   this.toolStrip1.SuspendLayout();
		   ((System.ComponentModel.ISupportInitialize)(this.BN)).BeginInit();
		   this.BN.SuspendLayout();
		   ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
		   ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
		   this.TLP.SuspendLayout();
		   ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
		   this.SuspendLayout();
		   // 
		   // TSTLP
		   // 
		   this.TSTLP.ColumnCount = 2;
		   this.TSTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
		   this.TSTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 348F));
		   this.TSTLP.Controls.Add(this.toolStrip1, 0, 0);
		   this.TSTLP.Controls.Add(this.BN, 1, 0);
		   this.TSTLP.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.TSTLP.Location = new System.Drawing.Point(3, 3);
		   this.TSTLP.Name = "TSTLP";
		   this.TSTLP.RowCount = 1;
		   this.TSTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
		   this.TSTLP.Size = new System.Drawing.Size(1159, 28);
		   this.TSTLP.TabIndex = 3;
		   // 
		   // toolStrip1
		   // 
		   this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MonCodelabel,
            this.MonCodebox,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.minBox,
            this.toolStripSeparator3,
            this.maxBox});
		   this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		   this.toolStrip1.Name = "toolStrip1";
		   this.toolStrip1.Size = new System.Drawing.Size(811, 28);
		   this.toolStrip1.TabIndex = 6;
		   this.toolStrip1.Text = "TS";
		   // 
		   // MonCodelabel
		   // 
		   this.MonCodelabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.MonCodelabel.ForeColor = System.Drawing.Color.DarkRed;
		   this.MonCodelabel.Name = "MonCodelabel";
		   this.MonCodelabel.Size = new System.Drawing.Size(104, 25);
		   this.MonCodelabel.Text = "Filter by Code";
		   this.MonCodelabel.ToolTipText = "Filter according to the Standard Code";
		   // 
		   // MonCodebox
		   // 
		   this.MonCodebox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		   this.MonCodebox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		   this.MonCodebox.BackColor = System.Drawing.Color.Linen;
		   this.MonCodebox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.MonCodebox.Name = "MonCodebox";
		   this.MonCodebox.Size = new System.Drawing.Size(75, 28);
		   this.MonCodebox.Sorted = true;
		   this.MonCodebox.ToolTipText = "Standard Code";
		   this.MonCodebox.SelectedIndexChanged += new System.EventHandler(this.MonCodebox_SelectedIndexChanged);
		   // 
		   // toolStripSeparator2
		   // 
		   this.toolStripSeparator2.Name = "toolStripSeparator2";
		   this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
		   // 
		   // toolStripLabel1
		   // 
		   this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.toolStripLabel1.ForeColor = System.Drawing.Color.DarkRed;
		   this.toolStripLabel1.Name = "toolStripLabel1";
		   this.toolStripLabel1.Size = new System.Drawing.Size(132, 25);
		   this.toolStripLabel1.Text = "Filter by Min/Max";
		   this.toolStripLabel1.ToolTipText = "Filter according to Monitor number";
		   // 
		   // minBox
		   // 
		   this.minBox.BackColor = System.Drawing.Color.Linen;
		   this.minBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		   this.minBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.minBox.Name = "minBox";
		   this.minBox.Size = new System.Drawing.Size(70, 28);
		   this.minBox.Text = "1";
		   this.minBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		   this.minBox.ToolTipText = "Minimum Monitor number";
		   this.minBox.TextChanged += new System.EventHandler(this.MonCodebox_SelectedIndexChanged);
		   // 
		   // toolStripSeparator3
		   // 
		   this.toolStripSeparator3.Name = "toolStripSeparator3";
		   this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
		   // 
		   // maxBox
		   // 
		   this.maxBox.BackColor = System.Drawing.Color.Linen;
		   this.maxBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		   this.maxBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.maxBox.Name = "maxBox";
		   this.maxBox.Size = new System.Drawing.Size(70, 28);
		   this.maxBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		   this.maxBox.ToolTipText = "Maximum Monitor number";
		   this.maxBox.TextChanged += new System.EventHandler(this.MonCodebox_SelectedIndexChanged);
		   // 
		   // BN
		   // 
		   this.BN.AddNewItem = null;
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
            this.monitorsBindingNavigatorSaveItem,
            this.toolStripSeparator1});
		   this.BN.Location = new System.Drawing.Point(811, 0);
		   this.BN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
		   this.BN.MoveLastItem = this.bindingNavigatorMoveLastItem;
		   this.BN.MoveNextItem = this.bindingNavigatorMoveNextItem;
		   this.BN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
		   this.BN.Name = "BN";
		   this.BN.PositionItem = this.bindingNavigatorPositionItem;
		   this.BN.Size = new System.Drawing.Size(348, 28);
		   this.BN.TabIndex = 5;
		   this.BN.Text = "bindingNavigator1";
		   // 
		   // BS
		   // 
		   this.BS.DataMember = "Monitors";
		   this.BS.DataSource = this.Linaa;
		   // 
		   // Linaa
		   // 
		  // this.Linaa.CurrentPref = null;
		   this.Linaa.DataSetName = "LINAA";
		   this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
		   this.Linaa.EnforceConstraints = false;
		   this.Linaa.FolderPath = null;
		 //  this.Linaa.IStore = this.Linaa;
		   this.Linaa.Locale = new System.Globalization.CultureInfo("");
	//	   this.Linaa.Notify = null;
		   this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
		   this.Linaa.TAM = null;
		   // 
		   // bindingNavigatorCountItem
		   // 
		   this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
		   this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 25);
		   this.bindingNavigatorCountItem.Text = "of {0}";
		   this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
		   // 
		   // bindingNavigatorDeleteItem
		   // 
		   this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
		   this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
		   this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
		   this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 25);
		   this.bindingNavigatorDeleteItem.Text = "Delete";
		   // 
		   // bindingNavigatorMoveFirstItem
		   // 
		   this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
		   this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
		   this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
		   this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 25);
		   this.bindingNavigatorMoveFirstItem.Text = "Move first";
		   // 
		   // bindingNavigatorMovePreviousItem
		   // 
		   this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
		   this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
		   this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
		   this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 25);
		   this.bindingNavigatorMovePreviousItem.Text = "Move previous";
		   // 
		   // bindingNavigatorSeparator
		   // 
		   this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
		   this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 28);
		   // 
		   // bindingNavigatorPositionItem
		   // 
		   this.bindingNavigatorPositionItem.AccessibleName = "Position";
		   this.bindingNavigatorPositionItem.AutoSize = false;
		   this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
		   this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(49, 23);
		   this.bindingNavigatorPositionItem.Text = "0";
		   this.bindingNavigatorPositionItem.ToolTipText = "Current position";
		   // 
		   // bindingNavigatorSeparator1
		   // 
		   this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
		   this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 28);
		   // 
		   // bindingNavigatorMoveNextItem
		   // 
		   this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
		   this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
		   this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
		   this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 25);
		   this.bindingNavigatorMoveNextItem.Text = "Move next";
		   // 
		   // bindingNavigatorMoveLastItem
		   // 
		   this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
		   this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
		   this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
		   this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 25);
		   this.bindingNavigatorMoveLastItem.Text = "Move last";
		   // 
		   // bindingNavigatorSeparator2
		   // 
		   this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
		   this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 28);
		   // 
		   // bindingNavigatorAddNewItem
		   // 
		   this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
		   this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
		   this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
		   this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 25);
		   this.bindingNavigatorAddNewItem.Text = "Add new";
		   // 
		   // monitorsBindingNavigatorSaveItem
		   // 
		   this.monitorsBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		   this.monitorsBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("monitorsBindingNavigatorSaveItem.Image")));
		   this.monitorsBindingNavigatorSaveItem.Name = "monitorsBindingNavigatorSaveItem";
		   this.monitorsBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 25);
		   this.monitorsBindingNavigatorSaveItem.Text = "Save Data";
		   // 
		   // toolStripSeparator1
		   // 
		   this.toolStripSeparator1.Name = "toolStripSeparator1";
		   this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
		   // 
		   // TLP
		   // 
		   this.TLP.ColumnCount = 1;
		   this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
		   this.TLP.Controls.Add(this.DGV, 0, 1);
		   this.TLP.Controls.Add(this.TSTLP, 0, 0);
		   this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.TLP.Location = new System.Drawing.Point(0, 0);
		   this.TLP.Name = "TLP";
		   this.TLP.RowCount = 2;
		   this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.028369F));
		   this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.97163F));
		   this.TLP.Size = new System.Drawing.Size(1165, 564);
		   this.TLP.TabIndex = 4;
		   // 
		   // DGV
		   // 
		   this.DGV.AllowUserToAddRows = false;
		   this.DGV.AllowUserToOrderColumns = true;
		   dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
		   this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
		   this.DGV.AutoGenerateColumns = false;
		   this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
		   this.DGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
		   this.DGV.BackgroundColor = System.Drawing.Color.Ivory;
		   this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		   this.DGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
		   this.DGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
		   dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
		   dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		   dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		   dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		   dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		   this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		   this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		   this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsLockedColumn,
            this.MonNameColumn,
            this.MonGrossMass1Column,
            this.MonGrossMass2Column,
            this.MonGrossMassAvgColumn,
            this.LastMassDateColumn,
            this.ReweightDaysColumn,
            this.LastIrradiationDateColumn,
            this.DecayDaysColumn,
            this.GeometryName,
            this.MonitorsIDColumn,
            this.LastProject});
		   this.DGV.DataSource = this.BS;
		   dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
		   dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
		   dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		   dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		   dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		   this.DGV.DefaultCellStyle = dataGridViewCellStyle10;
		   this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.DGV.Location = new System.Drawing.Point(3, 37);
		   this.DGV.Name = "DGV";
		   this.DGV.RowTemplate.Height = 24;
		   this.DGV.Size = new System.Drawing.Size(1159, 524);
		   this.DGV.TabIndex = 3;
		   // 
		   // IsLockedColumn
		   // 
		   this.IsLockedColumn.DataPropertyName = "IsLocked";
		   this.IsLockedColumn.HeaderText = "Locked?";
		   this.IsLockedColumn.Name = "IsLockedColumn";
		   this.IsLockedColumn.ReadOnly = true;
		   this.IsLockedColumn.ToolTipText = "Has (or has not) decayed";
		   this.IsLockedColumn.Width = 61;
		   // 
		   // MonNameColumn
		   // 
		   this.MonNameColumn.DataPropertyName = "MonName";
		   this.MonNameColumn.HeaderText = "Monitor";
		   this.MonNameColumn.Name = "MonNameColumn";
		   this.MonNameColumn.ToolTipText = "Monitor Label";
		   this.MonNameColumn.Width = 80;
		   // 
		   // MonGrossMass1Column
		   // 
		   this.MonGrossMass1Column.DataPropertyName = "MonGrossMass1";
		   dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender;
		   dataGridViewCellStyle3.Format = "N4";
		   dataGridViewCellStyle3.NullValue = "0";
		   this.MonGrossMass1Column.DefaultCellStyle = dataGridViewCellStyle3;
		   this.MonGrossMass1Column.HeaderText = "Weight 1";
		   this.MonGrossMass1Column.Name = "MonGrossMass1Column";
		   this.MonGrossMass1Column.ToolTipText = "In miligrams";
		   this.MonGrossMass1Column.Width = 85;
		   // 
		   // MonGrossMass2Column
		   // 
		   this.MonGrossMass2Column.DataPropertyName = "MonGrossMass2";
		   dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender;
		   dataGridViewCellStyle4.Format = "N4";
		   dataGridViewCellStyle4.NullValue = "0";
		   this.MonGrossMass2Column.DefaultCellStyle = dataGridViewCellStyle4;
		   this.MonGrossMass2Column.HeaderText = "Weight 2";
		   this.MonGrossMass2Column.Name = "MonGrossMass2Column";
		   this.MonGrossMass2Column.ToolTipText = "In miligrams";
		   this.MonGrossMass2Column.Width = 85;
		   // 
		   // MonGrossMassAvgColumn
		   // 
		   this.MonGrossMassAvgColumn.DataPropertyName = "MonGrossMassAvg";
		   dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle5.BackColor = System.Drawing.Color.Lavender;
		   dataGridViewCellStyle5.Format = "N4";
		   dataGridViewCellStyle5.NullValue = null;
		   this.MonGrossMassAvgColumn.DefaultCellStyle = dataGridViewCellStyle5;
		   this.MonGrossMassAvgColumn.HeaderText = "Weight Avg";
		   this.MonGrossMassAvgColumn.Name = "MonGrossMassAvgColumn";
		   this.MonGrossMassAvgColumn.ReadOnly = true;
		   this.MonGrossMassAvgColumn.ToolTipText = "in miligrams (updated automatically after saving)";
		   // 
		   // LastMassDateColumn
		   // 
		   this.LastMassDateColumn.DataPropertyName = "LastMassDate";
		   dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle6.BackColor = System.Drawing.Color.Lavender;
		   this.LastMassDateColumn.DefaultCellStyle = dataGridViewCellStyle6;
		   this.LastMassDateColumn.HeaderText = "Last Weight Date";
		   this.LastMassDateColumn.Name = "LastMassDateColumn";
		   this.LastMassDateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		   this.LastMassDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		   this.LastMassDateColumn.ToolTipText = "double-click for today";
		   this.LastMassDateColumn.Width = 121;
		   // 
		   // ReweightDaysColumn
		   // 
		   this.ReweightDaysColumn.DataPropertyName = "Difference";
		   dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle7.BackColor = System.Drawing.Color.Lavender;
		   dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		   this.ReweightDaysColumn.DefaultCellStyle = dataGridViewCellStyle7;
		   this.ReweightDaysColumn.HeaderText = "Reweight Days";
		   this.ReweightDaysColumn.Name = "ReweightDaysColumn";
		   this.ReweightDaysColumn.ReadOnly = true;
		   this.ReweightDaysColumn.ToolTipText = "number of days elapsed since it last weighting";
		   this.ReweightDaysColumn.Width = 108;
		   // 
		   // LastIrradiationDateColumn
		   // 
		   this.LastIrradiationDateColumn.DataPropertyName = "LastIrradiationDate";
		   this.LastIrradiationDateColumn.HeaderText = "Last Irradiation Date";
		   this.LastIrradiationDateColumn.Name = "LastIrradiationDateColumn";
		   this.LastIrradiationDateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		   this.LastIrradiationDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		   this.LastIrradiationDateColumn.ToolTipText = "this is updated automatically (read-only)";
		   this.LastIrradiationDateColumn.Width = 138;
		   // 
		   // DecayDaysColumn
		   // 
		   this.DecayDaysColumn.DataPropertyName = "NumberOfDays";
		   dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   this.DecayDaysColumn.DefaultCellStyle = dataGridViewCellStyle8;
		   this.DecayDaysColumn.HeaderText = "Decay Days";
		   this.DecayDaysColumn.Name = "DecayDaysColumn";
		   this.DecayDaysColumn.ReadOnly = true;
		   this.DecayDaysColumn.ToolTipText = "number of days elapsed since it last irradiation";
		   this.DecayDaysColumn.Width = 92;
		   // 
		   // GeometryName
		   // 
		   this.GeometryName.DataPropertyName = "GeometryName";
		   dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		   dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		   this.GeometryName.DefaultCellStyle = dataGridViewCellStyle9;
		   this.GeometryName.HeaderText = "Default Geometry";
		   this.GeometryName.Name = "GeometryName";
		   this.GeometryName.ToolTipText = "The default geometry assigned to this monitor";
		   this.GeometryName.Width = 124;
		   // 
		   // MonitorsIDColumn
		   // 
		   this.MonitorsIDColumn.DataPropertyName = "MonitorsID";
		   this.MonitorsIDColumn.HeaderText = "ID";
		   this.MonitorsIDColumn.Name = "MonitorsIDColumn";
		   this.MonitorsIDColumn.ReadOnly = true;
		   this.MonitorsIDColumn.ToolTipText = "unique identification number";
		   this.MonitorsIDColumn.Visible = false;
		   this.MonitorsIDColumn.Width = 45;
		   // 
		   // LastProject
		   // 
		   this.LastProject.DataPropertyName = "LastProject";
		   this.LastProject.HeaderText = "Last Project";
		   this.LastProject.Name = "LastProject";
		   this.LastProject.ToolTipText = "The last Irradiation Project for this Monitor";
		   this.LastProject.Width = 92;
		   // 
		   // ucMonitors
		   // 
		   this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
		   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		   this.Controls.Add(this.TLP);
		   this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.Name = "ucMonitors";
		   this.Size = new System.Drawing.Size(1165, 564);
		   this.TSTLP.ResumeLayout(false);
		   this.TSTLP.PerformLayout();
		   this.toolStrip1.ResumeLayout(false);
		   this.toolStrip1.PerformLayout();
		   ((System.ComponentModel.ISupportInitialize)(this.BN)).EndInit();
		   this.BN.ResumeLayout(false);
		   this.BN.PerformLayout();
		   ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
		   ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
		   this.TLP.ResumeLayout(false);
		   ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
		   this.ResumeLayout(false);

        }

        #endregion

     
     
        private System.Windows.Forms.TableLayoutPanel TSTLP;
        private System.Windows.Forms.TableLayoutPanel TLP;
        private DB.LINAA Linaa;
        private System.Windows.Forms.BindingSource BS;
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
        private System.Windows.Forms.ToolStripButton monitorsBindingNavigatorSaveItem;
		public System.Windows.Forms.DataGridView DGV;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripComboBox MonCodebox;
		private System.Windows.Forms.ToolStripLabel MonCodelabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox minBox;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripTextBox maxBox;
		private System.Windows.Forms.DataGridViewCheckBoxColumn IsLockedColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn MonNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn MonGrossMass1Column;
		private System.Windows.Forms.DataGridViewTextBoxColumn MonGrossMass2Column;
		private System.Windows.Forms.DataGridViewTextBoxColumn MonGrossMassAvgColumn;
		private Rsx.DGV.CalendarColumn LastMassDateColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ReweightDaysColumn;
		private Rsx.DGV.CalendarColumn LastIrradiationDateColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn DecayDaysColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn GeometryName;
		private System.Windows.Forms.DataGridViewTextBoxColumn MonitorsIDColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn LastProject;
    }
}
