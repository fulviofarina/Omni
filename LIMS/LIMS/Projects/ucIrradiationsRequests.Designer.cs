namespace DB.UI
{
    partial class ucIrradiationsRequests
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucIrradiationsRequests));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BN = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.BS = new System.Windows.Forms.BindingSource(this.components);
            this.Linaa = new DB.LINAA();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.channellabel = new System.Windows.Forms.ToolStripLabel();
            this.channelBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.irradiationSave = new System.Windows.Forms.ToolStripButton();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.IrradiationCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Channel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelsIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IrradiationStartDateTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IrradiationEndDateTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IrradiationRequestDocPathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IrradiationRequestDocFileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BudgetNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IrradiationRequestsIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SetIrradiationChannel = new System.Windows.Forms.ToolStripMenuItem();
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.BN)).BeginInit();
            this.BN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.CMS.SuspendLayout();
            this.TLP.SuspendLayout();
            this.SuspendLayout();
            // 
            // BN
            // 
            this.BN.AddNewItem = null;
            this.BN.BindingSource = this.BS;
            this.BN.CountItem = this.bindingNavigatorCountItem;
            this.BN.DeleteItem = this.bindingNavigatorDeleteItem;
            this.BN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.channellabel,
            this.channelBox,
            this.toolStripSeparator2,
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
            this.irradiationSave});
            this.BN.Location = new System.Drawing.Point(0, 0);
            this.BN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.BN.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.BN.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.BN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.BN.Name = "BN";
            this.BN.PositionItem = this.bindingNavigatorPositionItem;
            this.BN.Size = new System.Drawing.Size(694, 26);
            this.BN.TabIndex = 0;
            this.BN.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 23);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // BS
            // 
            this.BS.DataMember = "IrradiationRequests";
            this.BS.DataSource = this.Linaa;
            // 
            // Linaa
            // 
            this.Linaa.CurrentPref = null;
            this.Linaa.DataSetName = "LINAA";
            this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
            this.Linaa.EnforceConstraints = false;
            this.Linaa.FolderPath = null;
          //  this.Linaa.IStore = this.Linaa;
            this.Linaa.Locale = new System.Globalization.CultureInfo("");
            this.Linaa.Notify = null;
            this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.Linaa.TAM = null;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(45, 23);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 23);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // channellabel
            // 
            this.channellabel.Name = "channellabel";
            this.channellabel.Size = new System.Drawing.Size(62, 23);
            this.channellabel.Text = "Channel";
            // 
            // channelBox
            // 
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(100, 26);
            this.channelBox.SelectedIndexChanged += new System.EventHandler(this.channelBox_SelectedIndexChanged);
            this.channelBox.TextChanged += new System.EventHandler(this.channelBox_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 23);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 23);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 26);
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
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 23);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 23);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // irradiationSave
            // 
            this.irradiationSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.irradiationSave.Image = ((System.Drawing.Image)(resources.GetObject("irradiationSave.Image")));
            this.irradiationSave.Name = "irradiationSave";
            this.irradiationSave.Size = new System.Drawing.Size(23, 23);
            this.irradiationSave.Text = "Save Data";
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToOrderColumns = true;
            this.DGV.AutoGenerateColumns = false;
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DGV.BackgroundColor = System.Drawing.Color.RosyBrown;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IrradiationCodeColumn,
            this.Channel,
            this.Reactor,
            this.NumberColumn,
            this.ChannelsIDColumn,
            this.IrradiationStartDateTimeColumn,
            this.IrradiationEndDateTimeColumn,
            this.IrradiationRequestDocPathColumn,
            this.IrradiationRequestDocFileNameColumn,
            this.BudgetNumberColumn,
            this.IrradiationRequestsIDColumn});
            this.DGV.ContextMenuStrip = this.CMS;
            this.DGV.DataSource = this.BS;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle3;
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(3, 28);
            this.DGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DGV.Name = "DGV";
            this.DGV.RowTemplate.Height = 24;
            this.DGV.Size = new System.Drawing.Size(688, 680);
            this.DGV.TabIndex = 1;
            this.DGV.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DGV_CellPainting);
            // 
            // IrradiationCodeColumn
            // 
            this.IrradiationCodeColumn.DataPropertyName = "IrradiationCode";
            this.IrradiationCodeColumn.HeaderText = "Irradiation Code";
            this.IrradiationCodeColumn.Name = "IrradiationCodeColumn";
            this.IrradiationCodeColumn.Width = 130;
            // 
            // Channel
            // 
            this.Channel.DataPropertyName = "ChannelName";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.OldLace;
            this.Channel.DefaultCellStyle = dataGridViewCellStyle1;
            this.Channel.HeaderText = "Channel";
            this.Channel.Name = "Channel";
            this.Channel.ReadOnly = true;
            this.Channel.Width = 87;
            // 
            // Reactor
            // 
            this.Reactor.DataPropertyName = "Reactor";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.OldLace;
            this.Reactor.DefaultCellStyle = dataGridViewCellStyle2;
            this.Reactor.HeaderText = "Reactor";
            this.Reactor.Name = "Reactor";
            this.Reactor.ReadOnly = true;
            this.Reactor.Width = 85;
            // 
            // NumberColumn
            // 
            this.NumberColumn.DataPropertyName = "Number";
            this.NumberColumn.HeaderText = "Irradiation Number";
            this.NumberColumn.Name = "NumberColumn";
            this.NumberColumn.Width = 147;
            // 
            // ChannelsIDColumn
            // 
            this.ChannelsIDColumn.DataPropertyName = "ChannelsID";
            this.ChannelsIDColumn.HeaderText = "ChannelsID";
            this.ChannelsIDColumn.Name = "ChannelsIDColumn";
            this.ChannelsIDColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ChannelsIDColumn.Visible = false;
            this.ChannelsIDColumn.Width = 92;
            // 
            // IrradiationStartDateTimeColumn
            // 
            this.IrradiationStartDateTimeColumn.DataPropertyName = "IrradiationStartDateTime";
            this.IrradiationStartDateTimeColumn.HeaderText = "Created On";
            this.IrradiationStartDateTimeColumn.Name = "IrradiationStartDateTimeColumn";
            this.IrradiationStartDateTimeColumn.ReadOnly = true;
            // 
            // IrradiationEndDateTimeColumn
            // 
            this.IrradiationEndDateTimeColumn.DataPropertyName = "IrradiationEndDateTime";
            this.IrradiationEndDateTimeColumn.HeaderText = "IrradiationEndDateTime";
            this.IrradiationEndDateTimeColumn.Name = "IrradiationEndDateTimeColumn";
            this.IrradiationEndDateTimeColumn.ToolTipText = "not used";
            this.IrradiationEndDateTimeColumn.Visible = false;
            this.IrradiationEndDateTimeColumn.Width = 157;
            // 
            // IrradiationRequestDocPathColumn
            // 
            this.IrradiationRequestDocPathColumn.DataPropertyName = "IrradiationRequestDocPath";
            this.IrradiationRequestDocPathColumn.HeaderText = "IrradiationRequestDocPath";
            this.IrradiationRequestDocPathColumn.Name = "IrradiationRequestDocPathColumn";
            this.IrradiationRequestDocPathColumn.ToolTipText = "not used";
            this.IrradiationRequestDocPathColumn.Visible = false;
            this.IrradiationRequestDocPathColumn.Width = 173;
            // 
            // IrradiationRequestDocFileNameColumn
            // 
            this.IrradiationRequestDocFileNameColumn.DataPropertyName = "IrradiationRequestDocFileName";
            this.IrradiationRequestDocFileNameColumn.HeaderText = "IrradiationRequestDocFileName";
            this.IrradiationRequestDocFileNameColumn.Name = "IrradiationRequestDocFileNameColumn";
            this.IrradiationRequestDocFileNameColumn.ToolTipText = "not used";
            this.IrradiationRequestDocFileNameColumn.Visible = false;
            this.IrradiationRequestDocFileNameColumn.Width = 199;
            // 
            // BudgetNumberColumn
            // 
            this.BudgetNumberColumn.DataPropertyName = "BudgetNumber";
            this.BudgetNumberColumn.HeaderText = "BudgetNumber";
            this.BudgetNumberColumn.Name = "BudgetNumberColumn";
            this.BudgetNumberColumn.ToolTipText = "not used";
            this.BudgetNumberColumn.Visible = false;
            this.BudgetNumberColumn.Width = 114;
            // 
            // IrradiationRequestsIDColumn
            // 
            this.IrradiationRequestsIDColumn.DataPropertyName = "IrradiationRequestsID";
            this.IrradiationRequestsIDColumn.HeaderText = "IrradiationRequestsID";
            this.IrradiationRequestsIDColumn.Name = "IrradiationRequestsIDColumn";
            this.IrradiationRequestsIDColumn.ReadOnly = true;
            this.IrradiationRequestsIDColumn.Visible = false;
            this.IrradiationRequestsIDColumn.Width = 144;
            // 
            // CMS
            // 
            this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.toolStripSeparator1,
            this.SetIrradiationChannel});
            this.CMS.Name = "CMS";
            this.CMS.Size = new System.Drawing.Size(230, 80);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(226, 6);
            // 
            // SetIrradiationChannel
            // 
            this.SetIrradiationChannel.Name = "SetIrradiationChannel";
            this.SetIrradiationChannel.Size = new System.Drawing.Size(229, 24);
            this.SetIrradiationChannel.Text = "Set Irradiation Channel";
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 1;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP.Controls.Add(this.DGV, 0, 1);
            this.TLP.Controls.Add(this.BN, 0, 0);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(0, 0);
            this.TLP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 2;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.661972F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.33803F));
            this.TLP.Size = new System.Drawing.Size(694, 710);
            this.TLP.TabIndex = 3;
            // 
            // ucIrradiationsRequests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TLP);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucIrradiationsRequests";
            this.Size = new System.Drawing.Size(694, 710);
            ((System.ComponentModel.ISupportInitialize)(this.BN)).EndInit();
            this.BN.ResumeLayout(false);
            this.BN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.CMS.ResumeLayout(false);
            this.TLP.ResumeLayout(false);
            this.TLP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

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
        private System.Windows.Forms.ToolStripButton irradiationSave;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ContextMenuStrip CMS;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem SetIrradiationChannel;
		//private System.Windows.Forms.DataGridViewTextBoxColumn ReactorNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn IrradiationCodeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn Channel;
		private System.Windows.Forms.DataGridViewTextBoxColumn Reactor;
		private System.Windows.Forms.DataGridViewTextBoxColumn NumberColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ChannelsIDColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn IrradiationStartDateTimeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn IrradiationEndDateTimeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn IrradiationRequestDocPathColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn IrradiationRequestDocFileNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn BudgetNumberColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn IrradiationRequestsIDColumn;
		private System.Windows.Forms.ToolStripLabel channellabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripComboBox channelBox;
    }
}
