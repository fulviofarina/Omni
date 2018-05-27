namespace DB.UI
{
    partial class ucMatrixSimple
    {

        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;

        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;

        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;

        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;

        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;

        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;

        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;

        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;

        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;

        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;

        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;

        private System.Windows.Forms.DataGridViewCheckBoxColumn cDataGridViewCheckBoxColumn;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStripLabel editContentLBL;
        private System.Windows.Forms.ToolStripTextBox contentNameBox;

        private System.Windows.Forms.ToolStrip contentTS;

        private DB.LINAA lINAA;

        private System.Windows.Forms.BindingNavigator MatrixBN;

        private System.Windows.Forms.BindingSource MatrixBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn matrixCompositionDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixDensityDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridView matrixDGV;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixIDDataGridViewTextBoxColumn;

        private System.Windows.Forms.DataGridViewTextBoxColumn matrixNameDataGridViewTextBoxColumn;

     //   private System.Windows.Forms.SplitContainer splitContainer4;

        private System.Windows.Forms.SplitContainer splitContainer5;

        //   private System.Windows.Forms.TableLayoutPanel TLPS;
       // private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;

        private System.Windows.Forms.TableLayoutPanel TLPMatrix;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;

        private ucComposition ucComposition1;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMatrixSimple));
            this.TLPMatrix = new System.Windows.Forms.TableLayoutPanel();
            this.matrixDGV = new System.Windows.Forms.DataGridView();
            this.MatrixBS = new System.Windows.Forms.BindingSource(this.components);
            this.lINAA = new DB.LINAA();
            this.contentTS = new System.Windows.Forms.ToolStrip();
            this.editContentLBL = new System.Windows.Forms.ToolStripLabel();
            this.contentNameBox = new System.Windows.Forms.ToolStripTextBox();
            this.MatrixBN = new System.Windows.Forms.BindingNavigator(this.components);
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
            this.ucComposition1 = new DB.UI.ucComposition();
            this.matrixIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixCompositionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixDensityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xCOMDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.matrixNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matrixDensityDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToDo = new Rsx.DGV.CalculableColumn();
            this.TLPMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lINAA)).BeginInit();
            this.contentTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBN)).BeginInit();
            this.MatrixBN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // TLPMatrix
            // 
            this.TLPMatrix.ColumnCount = 2;
            this.TLPMatrix.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.983F));
            this.TLPMatrix.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.017F));
            this.TLPMatrix.Controls.Add(this.matrixDGV, 0, 1);
            this.TLPMatrix.Controls.Add(this.contentTS, 1, 0);
            this.TLPMatrix.Controls.Add(this.MatrixBN, 0, 0);
            this.TLPMatrix.Controls.Add(this.ucComposition1, 1, 1);
            this.TLPMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLPMatrix.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TLPMatrix.Location = new System.Drawing.Point(0, 0);
            this.TLPMatrix.Name = "TLPMatrix";
            this.TLPMatrix.RowCount = 2;
            this.TLPMatrix.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLPMatrix.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLPMatrix.Size = new System.Drawing.Size(706, 578);
            this.TLPMatrix.TabIndex = 6;
            // 
            // matrixDGV
            // 
            this.matrixDGV.AllowUserToAddRows = false;
            this.matrixDGV.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.matrixDGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.matrixDGV.AutoGenerateColumns = false;
            this.matrixDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.matrixDGV.BackgroundColor = System.Drawing.Color.Plum;
            this.matrixDGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.matrixDGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.matrixDGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.matrixDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.matrixDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matrixDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matrixNameDataGridViewTextBoxColumn1,
            this.matrixDensityDataGridViewTextBoxColumn1,
            this.ToDo});
            this.matrixDGV.DataSource = this.MatrixBS;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.matrixDGV.DefaultCellStyle = dataGridViewCellStyle3;
            this.matrixDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matrixDGV.EnableHeadersVisualStyles = false;
            this.matrixDGV.GridColor = System.Drawing.Color.Black;
            this.matrixDGV.Location = new System.Drawing.Point(3, 32);
            this.matrixDGV.MultiSelect = false;
            this.matrixDGV.Name = "matrixDGV";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.matrixDGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.matrixDGV.RowTemplate.Height = 24;
            this.matrixDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.matrixDGV.Size = new System.Drawing.Size(360, 543);
            this.matrixDGV.TabIndex = 7;
            // 
            // MatrixBS
            // 
            this.MatrixBS.DataMember = "Matrix";
            this.MatrixBS.DataSource = this.lINAA;
            // 
            // lINAA
            // 
            this.lINAA.DataSetName = "LINAA";
            this.lINAA.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("lINAA.DetectorsList")));
            this.lINAA.EnforceConstraints = false;
            this.lINAA.FolderPath = null;
            this.lINAA.Locale = new System.Globalization.CultureInfo("");
            this.lINAA.QTA = null;
            this.lINAA.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.lINAA.TAM = null;
            // 
            // contentTS
            // 
            this.contentTS.BackColor = System.Drawing.SystemColors.Menu;
            this.contentTS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.contentTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editContentLBL,
            this.contentNameBox});
            this.contentTS.Location = new System.Drawing.Point(366, 0);
            this.contentTS.Name = "contentTS";
            this.contentTS.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.contentTS.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contentTS.Size = new System.Drawing.Size(340, 29);
            this.contentTS.TabIndex = 5;
            this.contentTS.Text = "toolStrip8";
            // 
            // editContentLBL
            // 
            this.editContentLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editContentLBL.ForeColor = System.Drawing.Color.Blue;
            this.editContentLBL.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.editContentLBL.Name = "editContentLBL";
            this.editContentLBL.Size = new System.Drawing.Size(48, 26);
            this.editContentLBL.Text = "VIEW";
            // 
            // contentNameBox
            // 
            this.contentNameBox.BackColor = System.Drawing.SystemColors.Control;
            this.contentNameBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contentNameBox.Name = "contentNameBox";
            this.contentNameBox.Size = new System.Drawing.Size(140, 29);
            this.contentNameBox.Text = "Nothing";
            this.contentNameBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MatrixBN
            // 
            this.MatrixBN.AddNewItem = this.bindingNavigatorAddNewItem;
            this.MatrixBN.BackColor = System.Drawing.SystemColors.Control;
            this.MatrixBN.BindingSource = this.MatrixBS;
            this.MatrixBN.CountItem = this.bindingNavigatorCountItem;
            this.MatrixBN.DeleteItem = this.bindingNavigatorDeleteItem;
            this.MatrixBN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MatrixBN.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MatrixBN.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MatrixBN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.bindingNavigatorDeleteItem});
            this.MatrixBN.Location = new System.Drawing.Point(0, 0);
            this.MatrixBN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.MatrixBN.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.MatrixBN.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.MatrixBN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.MatrixBN.Name = "MatrixBN";
            this.MatrixBN.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.MatrixBN.PositionItem = this.bindingNavigatorPositionItem;
            this.MatrixBN.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MatrixBN.Size = new System.Drawing.Size(366, 29);
            this.MatrixBN.TabIndex = 1;
            this.MatrixBN.Text = "bindingNavigator1";
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
            this.bindingNavigatorCountItem.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(49, 26);
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
            this.bindingNavigatorPositionItem.ForeColor = System.Drawing.Color.Black;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(55, 29);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // ucComposition1
            // 
            this.ucComposition1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucComposition1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucComposition1.Location = new System.Drawing.Point(369, 32);
            this.ucComposition1.Name = "ucComposition1";
            this.ucComposition1.Size = new System.Drawing.Size(334, 543);
            this.ucComposition1.TabIndex = 8;
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
            // matrixNameDataGridViewTextBoxColumn1
            // 
            this.matrixNameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.matrixNameDataGridViewTextBoxColumn1.DataPropertyName = "MatrixName";
            this.matrixNameDataGridViewTextBoxColumn1.HeaderText = "Label";
            this.matrixNameDataGridViewTextBoxColumn1.Name = "matrixNameDataGridViewTextBoxColumn1";
            this.matrixNameDataGridViewTextBoxColumn1.ToolTipText = "A Label for the Matrix";
            this.matrixNameDataGridViewTextBoxColumn1.Width = 72;
            // 
            // matrixDensityDataGridViewTextBoxColumn1
            // 
            this.matrixDensityDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.matrixDensityDataGridViewTextBoxColumn1.DataPropertyName = "MatrixDensity";
            this.matrixDensityDataGridViewTextBoxColumn1.HeaderText = "Density";
            this.matrixDensityDataGridViewTextBoxColumn1.Name = "matrixDensityDataGridViewTextBoxColumn1";
            this.matrixDensityDataGridViewTextBoxColumn1.ToolTipText = "Estimated or Known Density for this Matrix (in g/cm3)";
            // 
            // ToDo
            // 
            this.ToDo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ToDo.DataPropertyName = "ToDo";
            this.ToDo.HeaderText = "ToDo";
            this.ToDo.Name = "ToDo";
            this.ToDo.ReadOnly = true;
            this.ToDo.Width = 71;
            // 
            // ucMatrixSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLPMatrix);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucMatrixSimple";
            this.Size = new System.Drawing.Size(706, 578);
            this.TLPMatrix.ResumeLayout(false);
            this.TLPMatrix.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lINAA)).EndInit();
            this.contentTS.ResumeLayout(false);
            this.contentTS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBN)).EndInit();
            this.MatrixBN.ResumeLayout(false);
            this.MatrixBN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewCheckBoxColumn xCOMDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn matrixNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn matrixDensityDataGridViewTextBoxColumn1;
        private Rsx.DGV.CalculableColumn ToDo;
    }
}
