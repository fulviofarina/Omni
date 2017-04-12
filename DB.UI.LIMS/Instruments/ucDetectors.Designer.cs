namespace DB.UI
{
    partial class ucDetectors
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
                  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDetectors));
                  this.MainTab = new System.Windows.Forms.TabControl();
                  this.DimensionsTab = new System.Windows.Forms.TabPage();
                  this.TLPDimensions = new System.Windows.Forms.TableLayoutPanel();
                  this.BN = new System.Windows.Forms.BindingNavigator(this.components);
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
                  this.SaveDimensions = new System.Windows.Forms.ToolStripButton();
                  this.detdimDGV = new System.Windows.Forms.DataGridView();
                  this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
                  this.RefreshCMS = new System.Windows.Forms.ToolStripMenuItem();
                  this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
                  this.cloneCMS = new System.Windows.Forms.ToolStripMenuItem();
                  this.AbsorbersTab = new System.Windows.Forms.TabPage();
                  this.TLPAbsorbers = new System.Windows.Forms.TableLayoutPanel();
                  this.detabsDGV = new System.Windows.Forms.DataGridView();
                  this.detabsCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
                  this.RefreshAbsorbers = new System.Windows.Forms.ToolStripMenuItem();
                  this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
                  this.setHolderSupportMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setContactLayerMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setTopDeadLayerMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setCrystalMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setCrystalHolderMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setCanTopMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setCanSideMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.setOtherAbsorberMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
                  this.cloneDetAbs = new System.Windows.Forms.ToolStripMenuItem();
                  this.HoldersTab = new System.Windows.Forms.TabPage();
                  this.TLPHolders = new System.Windows.Forms.TableLayoutPanel();
                  this.holdersDGV = new System.Windows.Forms.DataGridView();
                  this.Curves = new System.Windows.Forms.TabPage();
                  this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
                  this.detCvDGV = new System.Windows.Forms.DataGridView();
                  this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
                  this.BS = new System.Windows.Forms.BindingSource(this.components);
                  this.MainTab.SuspendLayout();
                  this.DimensionsTab.SuspendLayout();
                  this.TLPDimensions.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.BN)).BeginInit();
                  this.BN.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
                  ((System.ComponentModel.ISupportInitialize)(this.detdimDGV)).BeginInit();
                  this.CMS.SuspendLayout();
                  this.AbsorbersTab.SuspendLayout();
                  this.TLPAbsorbers.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.detabsDGV)).BeginInit();
                  this.detabsCMS.SuspendLayout();
                  this.HoldersTab.SuspendLayout();
                  this.TLPHolders.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.holdersDGV)).BeginInit();
                  this.Curves.SuspendLayout();
                  this.tableLayoutPanel1.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.detCvDGV)).BeginInit();
                  this.tableLayoutPanel2.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
                  this.SuspendLayout();
                  // 
                  // MainTab
                  // 
                  this.MainTab.Controls.Add(this.DimensionsTab);
                  this.MainTab.Controls.Add(this.AbsorbersTab);
                  this.MainTab.Controls.Add(this.HoldersTab);
                  this.MainTab.Controls.Add(this.Curves);
                  this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.MainTab.Location = new System.Drawing.Point(3, 35);
                  this.MainTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.MainTab.Name = "MainTab";
                  this.MainTab.SelectedIndex = 0;
                  this.MainTab.Size = new System.Drawing.Size(1418, 498);
                  this.MainTab.TabIndex = 0;
                  this.MainTab.Selected += new System.Windows.Forms.TabControlEventHandler(this.MainTab_Selected);
                  // 
                  // DimensionsTab
                  // 
                  this.DimensionsTab.AutoScroll = true;
                  this.DimensionsTab.Controls.Add(this.TLPDimensions);
                  this.DimensionsTab.Location = new System.Drawing.Point(4, 25);
                  this.DimensionsTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.DimensionsTab.Name = "DimensionsTab";
                  this.DimensionsTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.DimensionsTab.Size = new System.Drawing.Size(1410, 469);
                  this.DimensionsTab.TabIndex = 0;
                  this.DimensionsTab.Text = "Dimensions";
                  this.DimensionsTab.UseVisualStyleBackColor = true;
                  // 
                  // TLPDimensions
                  // 
                  this.TLPDimensions.ColumnCount = 1;
                  this.TLPDimensions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                  this.TLPDimensions.Controls.Add(this.detdimDGV, 0, 0);
                  this.TLPDimensions.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.TLPDimensions.Location = new System.Drawing.Point(3, 2);
                  this.TLPDimensions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.TLPDimensions.Name = "TLPDimensions";
                  this.TLPDimensions.RowCount = 1;
                  this.TLPDimensions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.23108F));
                  this.TLPDimensions.Size = new System.Drawing.Size(1404, 465);
                  this.TLPDimensions.TabIndex = 0;
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
            this.SaveDimensions});
                  this.BN.Location = new System.Drawing.Point(0, 0);
                  this.BN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
                  this.BN.MoveLastItem = this.bindingNavigatorMoveLastItem;
                  this.BN.MoveNextItem = this.bindingNavigatorMoveNextItem;
                  this.BN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
                  this.BN.Name = "BN";
                  this.BN.PositionItem = this.bindingNavigatorPositionItem;
                  this.BN.Size = new System.Drawing.Size(1424, 33);
                  this.BN.TabIndex = 1;
                  this.BN.Text = "bindingNavigator1";
                  // 
                  // Linaa
                  // 
             //     this.Linaa.CurrentPref = null;
                  this.Linaa.DataSetName = "LINAA";
                  this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
                  this.Linaa.EnforceConstraints = false;
                  this.Linaa.FolderPath = null;
                //  this.Linaa.IStore = this.Linaa;
                  this.Linaa.Locale = new System.Globalization.CultureInfo("");
                //  this.Linaa.Notify = null;
                  this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
                  this.Linaa.TAM = null;
                  // 
                  // bindingNavigatorCountItem
                  // 
                  this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
                  this.bindingNavigatorCountItem.Size = new System.Drawing.Size(45, 30);
                  this.bindingNavigatorCountItem.Text = "of {0}";
                  this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
                  // 
                  // bindingNavigatorDeleteItem
                  // 
                  this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
                  this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
                  this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
                  this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 30);
                  this.bindingNavigatorDeleteItem.Text = "Delete";
                  // 
                  // bindingNavigatorMoveFirstItem
                  // 
                  this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
                  this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
                  this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
                  this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 30);
                  this.bindingNavigatorMoveFirstItem.Text = "Move first";
                  // 
                  // bindingNavigatorMovePreviousItem
                  // 
                  this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
                  this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
                  this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
                  this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 30);
                  this.bindingNavigatorMovePreviousItem.Text = "Move previous";
                  // 
                  // bindingNavigatorSeparator
                  // 
                  this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
                  this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 33);
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
                  this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 33);
                  // 
                  // bindingNavigatorMoveNextItem
                  // 
                  this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
                  this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
                  this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
                  this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 30);
                  this.bindingNavigatorMoveNextItem.Text = "Move next";
                  // 
                  // bindingNavigatorMoveLastItem
                  // 
                  this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
                  this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
                  this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
                  this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 30);
                  this.bindingNavigatorMoveLastItem.Text = "Move last";
                  // 
                  // bindingNavigatorSeparator2
                  // 
                  this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
                  this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 33);
                  // 
                  // bindingNavigatorAddNewItem
                  // 
                  this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
                  this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
                  this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
                  this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 30);
                  this.bindingNavigatorAddNewItem.Text = "Add new";
                  // 
                  // SaveDimensions
                  // 
                  this.SaveDimensions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                  this.SaveDimensions.Image = ((System.Drawing.Image)(resources.GetObject("SaveDimensions.Image")));
                  this.SaveDimensions.Name = "SaveDimensions";
                  this.SaveDimensions.Size = new System.Drawing.Size(23, 30);
                  this.SaveDimensions.Text = "Save Data";
                  this.SaveDimensions.Click += new System.EventHandler(this.SaveDimensions_Click);
                  // 
                  // detdimDGV
                  // 
                  this.detdimDGV.AllowUserToOrderColumns = true;
                  this.detdimDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                  this.detdimDGV.ContextMenuStrip = this.CMS;
                  this.detdimDGV.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.detdimDGV.Location = new System.Drawing.Point(3, 2);
                  this.detdimDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.detdimDGV.Name = "detdimDGV";
                  this.detdimDGV.RowTemplate.Height = 24;
                  this.detdimDGV.Size = new System.Drawing.Size(1398, 461);
                  this.detdimDGV.TabIndex = 0;
                  // 
                  // CMS
                  // 
                  this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshCMS,
            this.toolStripSeparator10,
            this.cloneCMS});
                  this.CMS.Name = "CMS";
                  this.CMS.Size = new System.Drawing.Size(128, 58);
                  // 
                  // RefreshCMS
                  // 
                  this.RefreshCMS.Name = "RefreshCMS";
                  this.RefreshCMS.Size = new System.Drawing.Size(127, 24);
                  this.RefreshCMS.Text = "Refresh";
                  this.RefreshCMS.Click += new System.EventHandler(this.RefreshTables_Click);
                  // 
                  // toolStripSeparator10
                  // 
                  this.toolStripSeparator10.Name = "toolStripSeparator10";
                  this.toolStripSeparator10.Size = new System.Drawing.Size(124, 6);
                  // 
                  // cloneCMS
                  // 
                  this.cloneCMS.Name = "cloneCMS";
                  this.cloneCMS.Size = new System.Drawing.Size(127, 24);
                  this.cloneCMS.Text = "Clone";
                  // 
                  // AbsorbersTab
                  // 
                  this.AbsorbersTab.Controls.Add(this.TLPAbsorbers);
                  this.AbsorbersTab.Location = new System.Drawing.Point(4, 25);
                  this.AbsorbersTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.AbsorbersTab.Name = "AbsorbersTab";
                  this.AbsorbersTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.AbsorbersTab.Size = new System.Drawing.Size(1410, 469);
                  this.AbsorbersTab.TabIndex = 1;
                  this.AbsorbersTab.Text = "Absorbers";
                  this.AbsorbersTab.UseVisualStyleBackColor = true;
                  // 
                  // TLPAbsorbers
                  // 
                  this.TLPAbsorbers.ColumnCount = 1;
                  this.TLPAbsorbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                  this.TLPAbsorbers.Controls.Add(this.detabsDGV, 0, 0);
                  this.TLPAbsorbers.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.TLPAbsorbers.Location = new System.Drawing.Point(3, 2);
                  this.TLPAbsorbers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.TLPAbsorbers.Name = "TLPAbsorbers";
                  this.TLPAbsorbers.RowCount = 1;
                  this.TLPAbsorbers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.03984F));
                  this.TLPAbsorbers.Size = new System.Drawing.Size(1404, 465);
                  this.TLPAbsorbers.TabIndex = 1;
                  // 
                  // detabsDGV
                  // 
                  this.detabsDGV.AllowUserToOrderColumns = true;
                  this.detabsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                  this.detabsDGV.ContextMenuStrip = this.detabsCMS;
                  this.detabsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.detabsDGV.Location = new System.Drawing.Point(3, 2);
                  this.detabsDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.detabsDGV.Name = "detabsDGV";
                  this.detabsDGV.RowTemplate.Height = 24;
                  this.detabsDGV.Size = new System.Drawing.Size(1398, 461);
                  this.detabsDGV.TabIndex = 2;
                  this.detabsDGV.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.detabsDGV_CellContentDoubleClick);
                  // 
                  // detabsCMS
                  // 
                  this.detabsCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshAbsorbers,
            this.toolStripSeparator7,
            this.setHolderSupportMatrixToolStripMenuItem,
            this.setContactLayerMatrixToolStripMenuItem,
            this.setTopDeadLayerMatrixToolStripMenuItem,
            this.setCrystalMatrixToolStripMenuItem,
            this.setCrystalHolderMatrixToolStripMenuItem,
            this.setCanTopMatrixToolStripMenuItem,
            this.setCanSideMatrixToolStripMenuItem,
            this.setOtherAbsorberMatrixToolStripMenuItem,
            this.toolStripSeparator8,
            this.cloneDetAbs});
                  this.detabsCMS.Name = "CMS";
                  this.detabsCMS.Size = new System.Drawing.Size(255, 256);
                  // 
                  // RefreshAbsorbers
                  // 
                  this.RefreshAbsorbers.Name = "RefreshAbsorbers";
                  this.RefreshAbsorbers.Size = new System.Drawing.Size(254, 24);
                  this.RefreshAbsorbers.Text = "Refresh";
                  this.RefreshAbsorbers.Click += new System.EventHandler(this.RefreshTables_Click);
                  // 
                  // toolStripSeparator7
                  // 
                  this.toolStripSeparator7.Name = "toolStripSeparator7";
                  this.toolStripSeparator7.Size = new System.Drawing.Size(251, 6);
                  // 
                  // setHolderSupportMatrixToolStripMenuItem
                  // 
                  this.setHolderSupportMatrixToolStripMenuItem.Name = "setHolderSupportMatrixToolStripMenuItem";
                  this.setHolderSupportMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setHolderSupportMatrixToolStripMenuItem.Text = "Set Holder Support Matrix";
                  // 
                  // setContactLayerMatrixToolStripMenuItem
                  // 
                  this.setContactLayerMatrixToolStripMenuItem.Name = "setContactLayerMatrixToolStripMenuItem";
                  this.setContactLayerMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setContactLayerMatrixToolStripMenuItem.Text = "Set Contact Layer Matrix";
                  // 
                  // setTopDeadLayerMatrixToolStripMenuItem
                  // 
                  this.setTopDeadLayerMatrixToolStripMenuItem.Name = "setTopDeadLayerMatrixToolStripMenuItem";
                  this.setTopDeadLayerMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setTopDeadLayerMatrixToolStripMenuItem.Text = "Set Top Dead Layer Matrix";
                  // 
                  // setCrystalMatrixToolStripMenuItem
                  // 
                  this.setCrystalMatrixToolStripMenuItem.Name = "setCrystalMatrixToolStripMenuItem";
                  this.setCrystalMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setCrystalMatrixToolStripMenuItem.Text = "Set Crystal Matrix";
                  // 
                  // setCrystalHolderMatrixToolStripMenuItem
                  // 
                  this.setCrystalHolderMatrixToolStripMenuItem.Name = "setCrystalHolderMatrixToolStripMenuItem";
                  this.setCrystalHolderMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setCrystalHolderMatrixToolStripMenuItem.Text = "Set Crystal Holder Matrix";
                  // 
                  // setCanTopMatrixToolStripMenuItem
                  // 
                  this.setCanTopMatrixToolStripMenuItem.Name = "setCanTopMatrixToolStripMenuItem";
                  this.setCanTopMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setCanTopMatrixToolStripMenuItem.Text = "Set Can Top Matrix";
                  // 
                  // setCanSideMatrixToolStripMenuItem
                  // 
                  this.setCanSideMatrixToolStripMenuItem.Name = "setCanSideMatrixToolStripMenuItem";
                  this.setCanSideMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setCanSideMatrixToolStripMenuItem.Text = "Set Can Side Matrix";
                  // 
                  // setOtherAbsorberMatrixToolStripMenuItem
                  // 
                  this.setOtherAbsorberMatrixToolStripMenuItem.Name = "setOtherAbsorberMatrixToolStripMenuItem";
                  this.setOtherAbsorberMatrixToolStripMenuItem.Size = new System.Drawing.Size(254, 24);
                  this.setOtherAbsorberMatrixToolStripMenuItem.Text = "Set Other Absorber Matrix";
                  // 
                  // toolStripSeparator8
                  // 
                  this.toolStripSeparator8.Name = "toolStripSeparator8";
                  this.toolStripSeparator8.Size = new System.Drawing.Size(251, 6);
                  // 
                  // cloneDetAbs
                  // 
                  this.cloneDetAbs.Name = "cloneDetAbs";
                  this.cloneDetAbs.Size = new System.Drawing.Size(254, 24);
                  this.cloneDetAbs.Text = "Clone";
                  // 
                  // HoldersTab
                  // 
                  this.HoldersTab.Controls.Add(this.TLPHolders);
                  this.HoldersTab.Location = new System.Drawing.Point(4, 25);
                  this.HoldersTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.HoldersTab.Name = "HoldersTab";
                  this.HoldersTab.Size = new System.Drawing.Size(1410, 469);
                  this.HoldersTab.TabIndex = 2;
                  this.HoldersTab.Text = "Holders";
                  this.HoldersTab.UseVisualStyleBackColor = true;
                  // 
                  // TLPHolders
                  // 
                  this.TLPHolders.ColumnCount = 1;
                  this.TLPHolders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                  this.TLPHolders.Controls.Add(this.holdersDGV, 0, 0);
                  this.TLPHolders.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.TLPHolders.Location = new System.Drawing.Point(0, 0);
                  this.TLPHolders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.TLPHolders.Name = "TLPHolders";
                  this.TLPHolders.RowCount = 1;
                  this.TLPHolders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.03984F));
                  this.TLPHolders.Size = new System.Drawing.Size(1410, 469);
                  this.TLPHolders.TabIndex = 2;
                  // 
                  // holdersDGV
                  // 
                  this.holdersDGV.AllowUserToOrderColumns = true;
                  this.holdersDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                  this.holdersDGV.ContextMenuStrip = this.CMS;
                  this.holdersDGV.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.holdersDGV.Location = new System.Drawing.Point(3, 2);
                  this.holdersDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.holdersDGV.Name = "holdersDGV";
                  this.holdersDGV.RowTemplate.Height = 24;
                  this.holdersDGV.Size = new System.Drawing.Size(1404, 465);
                  this.holdersDGV.TabIndex = 2;
                  // 
                  // Curves
                  // 
                  this.Curves.Controls.Add(this.tableLayoutPanel1);
                  this.Curves.Location = new System.Drawing.Point(4, 25);
                  this.Curves.Name = "Curves";
                  this.Curves.Padding = new System.Windows.Forms.Padding(3);
                  this.Curves.Size = new System.Drawing.Size(1410, 469);
                  this.Curves.TabIndex = 3;
                  this.Curves.Text = "Curves";
                  this.Curves.UseVisualStyleBackColor = true;
                  // 
                  // tableLayoutPanel1
                  // 
                  this.tableLayoutPanel1.ColumnCount = 1;
                  this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                  this.tableLayoutPanel1.Controls.Add(this.detCvDGV, 0, 0);
                  this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
                  this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.tableLayoutPanel1.Name = "tableLayoutPanel1";
                  this.tableLayoutPanel1.RowCount = 1;
                  this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.23108F));
                  this.tableLayoutPanel1.Size = new System.Drawing.Size(1404, 463);
                  this.tableLayoutPanel1.TabIndex = 1;
                  // 
                  // detCvDGV
                  // 
                  this.detCvDGV.AllowUserToOrderColumns = true;
                  this.detCvDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                  this.detCvDGV.ContextMenuStrip = this.CMS;
                  this.detCvDGV.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.detCvDGV.Location = new System.Drawing.Point(3, 2);
                  this.detCvDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.detCvDGV.Name = "detCvDGV";
                  this.detCvDGV.RowTemplate.Height = 24;
                  this.detCvDGV.Size = new System.Drawing.Size(1398, 459);
                  this.detCvDGV.TabIndex = 0;
                  // 
                  // tableLayoutPanel2
                  // 
                  this.tableLayoutPanel2.ColumnCount = 1;
                  this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                  this.tableLayoutPanel2.Controls.Add(this.BN, 0, 0);
                  this.tableLayoutPanel2.Controls.Add(this.MainTab, 0, 1);
                  this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
                  this.tableLayoutPanel2.Name = "tableLayoutPanel2";
                  this.tableLayoutPanel2.RowCount = 2;
                  this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.168224F));
                  this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.83178F));
                  this.tableLayoutPanel2.Size = new System.Drawing.Size(1424, 535);
                  this.tableLayoutPanel2.TabIndex = 2;
                  // 
                  // BS
                  // 
                  this.BS.DataMember = "DetectorsDimensions";
                  this.BS.DataSource = this.Linaa;
                  // 
                  // ucDetectors
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.Controls.Add(this.tableLayoutPanel2);
                  this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
                  this.Name = "ucDetectors";
                  this.Size = new System.Drawing.Size(1424, 535);
                  this.Load += new System.EventHandler(this.ucDetectors_Load);
                  this.MainTab.ResumeLayout(false);
                  this.DimensionsTab.ResumeLayout(false);
                  this.TLPDimensions.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.BN)).EndInit();
                  this.BN.ResumeLayout(false);
                  this.BN.PerformLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
                  ((System.ComponentModel.ISupportInitialize)(this.detdimDGV)).EndInit();
                  this.CMS.ResumeLayout(false);
                  this.AbsorbersTab.ResumeLayout(false);
                  this.TLPAbsorbers.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.detabsDGV)).EndInit();
                  this.detabsCMS.ResumeLayout(false);
                  this.HoldersTab.ResumeLayout(false);
                  this.TLPHolders.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.holdersDGV)).EndInit();
                  this.Curves.ResumeLayout(false);
                  this.tableLayoutPanel1.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.detCvDGV)).EndInit();
                  this.tableLayoutPanel2.ResumeLayout(false);
                  this.tableLayoutPanel2.PerformLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
                  this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage DimensionsTab;
        private System.Windows.Forms.TabPage AbsorbersTab;
        private System.Windows.Forms.TabPage HoldersTab;
        private System.Windows.Forms.TableLayoutPanel TLPDimensions;
        private DB.LINAA Linaa;
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
        private System.Windows.Forms.ToolStripButton SaveDimensions;
        private System.Windows.Forms.DataGridView detdimDGV;
        private System.Windows.Forms.TableLayoutPanel TLPAbsorbers;

        private System.Windows.Forms.DataGridView detabsDGV;
        private System.Windows.Forms.TableLayoutPanel TLPHolders;
        private System.Windows.Forms.DataGridView holdersDGV;
        private System.Windows.Forms.ContextMenuStrip detabsCMS;
        private System.Windows.Forms.ToolStripMenuItem RefreshAbsorbers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem setHolderSupportMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setContactLayerMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTopDeadLayerMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCrystalMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCrystalHolderMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCanTopMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCanSideMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setOtherAbsorberMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem cloneDetAbs;
        private System.Windows.Forms.ContextMenuStrip CMS;
        private System.Windows.Forms.ToolStripMenuItem RefreshCMS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem cloneCMS;
        private System.Windows.Forms.TabPage Curves;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView detCvDGV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.BindingSource BS;
    }
}
