namespace VTools
{
    partial class Explorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Explorer));
            this.BN = new System.Windows.Forms.BindingNavigator(this.components);
            this.BNAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.BS = new System.Windows.Forms.BindingSource(this.components);
            this.BNCountItem = new System.Windows.Forms.ToolStripLabel();
            this.BNDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.TableLabel = new System.Windows.Forms.ToolStripLabel();
            this.Box = new System.Windows.Forms.ToolStripComboBox();
            this.ColumnLabel = new System.Windows.Forms.ToolStripLabel();
            this.columnBox = new System.Windows.Forms.ToolStripTextBox();
            this.TypeLabel = new System.Windows.Forms.ToolStripLabel();
            this.ColumnTypeBox = new System.Windows.Forms.ToolStripComboBox();
            this.AddCol = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.BNMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.BNMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.BNSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.BNPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.BNSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.BNMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.BNMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.BNSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveBtton = new System.Windows.Forms.ToolStripButton();
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.TS2 = new System.Windows.Forms.ToolStrip();
            this.ExpressionLabel = new System.Windows.Forms.ToolStripLabel();
            this.expBox = new System.Windows.Forms.ToolStripTextBox();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.TS = new System.Windows.Forms.ToolStrip();
            this.FilterLabel = new System.Windows.Forms.ToolStripLabel();
            this.filterbox = new System.Windows.Forms.ToolStripTextBox();
            this.sortLabel = new System.Windows.Forms.ToolStripLabel();
            this.sortBox = new System.Windows.Forms.ToolStripTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BN)).BeginInit();
            this.BN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
            this.TLP.SuspendLayout();
            this.TS2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.TS.SuspendLayout();
            this.SuspendLayout();
            // 
            // BN
            // 
            this.BN.AddNewItem = this.BNAddNewItem;
            this.BN.BindingSource = this.BS;
            this.BN.CountItem = this.BNCountItem;
            this.BN.DeleteItem = this.BNDeleteItem;
            this.BN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TableLabel,
            this.Box,
            this.ColumnLabel,
            this.columnBox,
            this.TypeLabel,
            this.ColumnTypeBox,
            this.AddCol,
            this.toolStripSeparator4,
            this.BNMoveFirstItem,
            this.BNMovePreviousItem,
            this.BNSeparator,
            this.BNPositionItem,
            this.BNCountItem,
            this.BNSeparator1,
            this.BNMoveNextItem,
            this.BNMoveLastItem,
            this.BNSeparator2,
            this.BNAddNewItem,
            this.BNDeleteItem,
            this.toolStripSeparator1,
            this.saveBtton});
            this.BN.Location = new System.Drawing.Point(0, 0);
            this.BN.MoveFirstItem = this.BNMoveFirstItem;
            this.BN.MoveLastItem = this.BNMoveLastItem;
            this.BN.MoveNextItem = this.BNMoveNextItem;
            this.BN.MovePreviousItem = this.BNMovePreviousItem;
            this.BN.Name = "BN";
            this.BN.PositionItem = this.BNPositionItem;
            this.BN.Size = new System.Drawing.Size(1127, 24);
            this.BN.TabIndex = 0;
            this.BN.Text = "bindingNavigator1";
            // 
            // BNAddNewItem
            // 
            this.BNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BNAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("BNAddNewItem.Image")));
            this.BNAddNewItem.Name = "BNAddNewItem";
            this.BNAddNewItem.RightToLeftAutoMirrorImage = true;
            this.BNAddNewItem.Size = new System.Drawing.Size(23, 21);
            this.BNAddNewItem.Text = "Add new";
            // 
            // BNCountItem
            // 
            this.BNCountItem.Name = "BNCountItem";
            this.BNCountItem.Size = new System.Drawing.Size(35, 21);
            this.BNCountItem.Text = "of {0}";
            this.BNCountItem.ToolTipText = "Total number of items";
            // 
            // BNDeleteItem
            // 
            this.BNDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BNDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("BNDeleteItem.Image")));
            this.BNDeleteItem.Name = "BNDeleteItem";
            this.BNDeleteItem.RightToLeftAutoMirrorImage = true;
            this.BNDeleteItem.Size = new System.Drawing.Size(23, 21);
            this.BNDeleteItem.Text = "Delete";
            // 
            // TableLabel
            // 
            this.TableLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TableLabel.IsLink = true;
            this.TableLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.TableLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.TableLabel.Name = "TableLabel";
            this.TableLabel.Size = new System.Drawing.Size(35, 21);
            this.TableLabel.Text = "Table";
            this.TableLabel.Click += new System.EventHandler(this.Box_TextChanged);
            // 
            // Box
            // 
            this.Box.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(280, 24);
            this.Box.TextChanged += new System.EventHandler(this.Box_TextChanged);
            // 
            // ColumnLabel
            // 
            this.ColumnLabel.Name = "ColumnLabel";
            this.ColumnLabel.Size = new System.Drawing.Size(50, 21);
            this.ColumnLabel.Text = "Column";
            // 
            // columnBox
            // 
            this.columnBox.Name = "columnBox";
            this.columnBox.Size = new System.Drawing.Size(120, 24);
            this.columnBox.TextChanged += new System.EventHandler(this.columnBox_TextChanged);
            // 
            // TypeLabel
            // 
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(33, 21);
            this.TypeLabel.Text = "Type";
            // 
            // ColumnTypeBox
            // 
            this.ColumnTypeBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ColumnTypeBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ColumnTypeBox.Items.AddRange(new object[] {
            "double",
            "int",
            "string",
            "datetime",
            "bool",
            "decimal"});
            this.ColumnTypeBox.Name = "ColumnTypeBox";
            this.ColumnTypeBox.Size = new System.Drawing.Size(80, 24);
            this.ColumnTypeBox.Text = "double";
            // 
            // AddCol
            // 
            this.AddCol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddCol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddCol.Name = "AddCol";
            this.AddCol.Size = new System.Drawing.Size(33, 21);
            this.AddCol.Text = "Add";
            this.AddCol.Click += new System.EventHandler(this.AddCol_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 24);
            // 
            // BNMoveFirstItem
            // 
            this.BNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BNMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("BNMoveFirstItem.Image")));
            this.BNMoveFirstItem.Name = "BNMoveFirstItem";
            this.BNMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.BNMoveFirstItem.Size = new System.Drawing.Size(23, 21);
            this.BNMoveFirstItem.Text = "Move first";
            // 
            // BNMovePreviousItem
            // 
            this.BNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BNMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("BNMovePreviousItem.Image")));
            this.BNMovePreviousItem.Name = "BNMovePreviousItem";
            this.BNMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.BNMovePreviousItem.Size = new System.Drawing.Size(23, 21);
            this.BNMovePreviousItem.Text = "Move previous";
            // 
            // BNSeparator
            // 
            this.BNSeparator.Name = "BNSeparator";
            this.BNSeparator.Size = new System.Drawing.Size(6, 24);
            // 
            // BNPositionItem
            // 
            this.BNPositionItem.AccessibleName = "Position";
            this.BNPositionItem.AutoSize = false;
            this.BNPositionItem.Name = "BNPositionItem";
            this.BNPositionItem.Size = new System.Drawing.Size(38, 23);
            this.BNPositionItem.Text = "0";
            this.BNPositionItem.ToolTipText = "Current position";
            // 
            // BNSeparator1
            // 
            this.BNSeparator1.Name = "BNSeparator1";
            this.BNSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // BNMoveNextItem
            // 
            this.BNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BNMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("BNMoveNextItem.Image")));
            this.BNMoveNextItem.Name = "BNMoveNextItem";
            this.BNMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.BNMoveNextItem.Size = new System.Drawing.Size(23, 21);
            this.BNMoveNextItem.Text = "Move next";
            // 
            // BNMoveLastItem
            // 
            this.BNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BNMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("BNMoveLastItem.Image")));
            this.BNMoveLastItem.Name = "BNMoveLastItem";
            this.BNMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.BNMoveLastItem.Size = new System.Drawing.Size(23, 21);
            this.BNMoveLastItem.Text = "Move last";
            // 
            // BNSeparator2
            // 
            this.BNSeparator2.Name = "BNSeparator2";
            this.BNSeparator2.Size = new System.Drawing.Size(6, 24);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // saveBtton
            // 
            this.saveBtton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveBtton.ForeColor = System.Drawing.Color.MidnightBlue;
            this.saveBtton.Image = ((System.Drawing.Image)(resources.GetObject("saveBtton.Image")));
            this.saveBtton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveBtton.Name = "saveBtton";
            this.saveBtton.Size = new System.Drawing.Size(38, 21);
            this.saveBtton.Text = "SAVE";
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 1;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP.Controls.Add(this.TS2, 0, 2);
            this.TLP.Controls.Add(this.BN, 0, 0);
            this.TLP.Controls.Add(this.DGV, 0, 1);
            this.TLP.Controls.Add(this.TS, 0, 3);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(0, 0);
            this.TLP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 4;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.536913F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.46309F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP.Size = new System.Drawing.Size(1127, 484);
            this.TLP.TabIndex = 1;
            // 
            // TS2
            // 
            this.TS2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExpressionLabel,
            this.expBox});
            this.TS2.Location = new System.Drawing.Point(0, 438);
            this.TS2.Name = "TS2";
            this.TS2.Size = new System.Drawing.Size(1127, 25);
            this.TS2.TabIndex = 3;
            // 
            // ExpressionLabel
            // 
            this.ExpressionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExpressionLabel.IsLink = true;
            this.ExpressionLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.ExpressionLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ExpressionLabel.Name = "ExpressionLabel";
            this.ExpressionLabel.Size = new System.Drawing.Size(63, 22);
            this.ExpressionLabel.Text = "Expression";
            this.ExpressionLabel.Click += new System.EventHandler(this.ExpressionLabel_Click);
            // 
            // expBox
            // 
            this.expBox.Name = "expBox";
            this.expBox.Size = new System.Drawing.Size(850, 25);
            this.expBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.expBox_KeyDown);
            // 
            // DGV
            // 
            this.DGV.AutoGenerateColumns = false;
            this.DGV.BackgroundColor = System.Drawing.Color.RosyBrown;
            this.DGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.DataSource = this.BS;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(2, 26);
            this.DGV.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DGV.Name = "DGV";
            this.DGV.RowTemplate.Height = 24;
            this.DGV.Size = new System.Drawing.Size(1123, 410);
            this.DGV.TabIndex = 1;
            this.DGV.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
            // 
            // TS
            // 
            this.TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FilterLabel,
            this.filterbox,
            this.sortLabel,
            this.sortBox});
            this.TS.Location = new System.Drawing.Point(0, 463);
            this.TS.Name = "TS";
            this.TS.Size = new System.Drawing.Size(1127, 21);
            this.TS.TabIndex = 2;
            this.TS.Text = "toolStrip1";
            // 
            // FilterLabel
            // 
            this.FilterLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilterLabel.IsLink = true;
            this.FilterLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.FilterLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.FilterLabel.Name = "FilterLabel";
            this.FilterLabel.Size = new System.Drawing.Size(33, 18);
            this.FilterLabel.Text = "Filter";
            this.FilterLabel.Click += new System.EventHandler(this.Box_TextChanged);
            // 
            // filterbox
            // 
            this.filterbox.Name = "filterbox";
            this.filterbox.Size = new System.Drawing.Size(550, 21);
            this.filterbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterbox_KeyUp);
            // 
            // sortLabel
            // 
            this.sortLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortLabel.IsLink = true;
            this.sortLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.sortLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.sortLabel.Name = "sortLabel";
            this.sortLabel.Size = new System.Drawing.Size(29, 18);
            this.sortLabel.Text = "Sort";
            this.sortLabel.Click += new System.EventHandler(this.Box_TextChanged);
            // 
            // sortBox
            // 
            this.sortBox.Name = "sortBox";
            this.sortBox.Size = new System.Drawing.Size(300, 21);
            this.sortBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sortBox_KeyDown);
            // 
            // Explorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLP);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Explorer";
            this.Size = new System.Drawing.Size(1127, 484);
            ((System.ComponentModel.ISupportInitialize)(this.BN)).EndInit();
            this.BN.ResumeLayout(false);
            this.BN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
            this.TLP.ResumeLayout(false);
            this.TLP.PerformLayout();
            this.TS2.ResumeLayout(false);
            this.TS2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.TS.ResumeLayout(false);
            this.TS.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingNavigator BN;
        private System.Windows.Forms.ToolStripButton BNAddNewItem;
        private System.Windows.Forms.ToolStripLabel BNCountItem;
		private System.Windows.Forms.ToolStripButton BNDeleteItem;
        private System.Windows.Forms.ToolStripButton BNMoveFirstItem;
        private System.Windows.Forms.ToolStripButton BNMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator BNSeparator;
        private System.Windows.Forms.ToolStripTextBox BNPositionItem;
        private System.Windows.Forms.ToolStripSeparator BNSeparator1;
        private System.Windows.Forms.ToolStripButton BNMoveNextItem;
        private System.Windows.Forms.ToolStripButton BNMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator BNSeparator2;
        private System.Windows.Forms.BindingSource BS;
        private System.Windows.Forms.TableLayoutPanel TLP;
		private System.Windows.Forms.ToolStrip TS;
		private System.Windows.Forms.ToolStripTextBox filterbox;
		private System.Windows.Forms.ToolStrip TS2;
		private System.Windows.Forms.ToolStripLabel ExpressionLabel;
		private System.Windows.Forms.ToolStripTextBox expBox;
		private System.Windows.Forms.ToolStripLabel FilterLabel;
		private System.Windows.Forms.ToolStripLabel sortLabel;
		private System.Windows.Forms.ToolStripTextBox sortBox;
		private System.Windows.Forms.ToolStripButton AddCol;
		private System.Windows.Forms.ToolStripComboBox ColumnTypeBox;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripLabel ColumnLabel;
		private System.Windows.Forms.ToolStripTextBox columnBox;
		private System.Windows.Forms.ToolStripLabel TypeLabel;
		private System.Windows.Forms.ToolStripLabel TableLabel;
		public System.Windows.Forms.ToolStripComboBox Box;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripButton saveBtton;
        public System.Windows.Forms.DataGridView DGV;
    }
}
