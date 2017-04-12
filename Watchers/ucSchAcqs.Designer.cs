namespace k0X
{
   partial class ucSchAcqs
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSchAcqs));
          this.TLP = new System.Windows.Forms.TableLayoutPanel();
          this.BN = new System.Windows.Forms.BindingNavigator(this.components);
          this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
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
          this.schAcqsBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.Refreshbto = new System.Windows.Forms.ToolStripLabel();
          this.schAcqsDataGridView = new System.Windows.Forms.DataGridView();
          this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.Detector = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn8 = new Rsx.DGV.CalendarColumn();
          this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.Counter = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
          this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.Interrupted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
          this.IsAwake = new System.Windows.Forms.DataGridViewCheckBoxColumn();
          this.Delay = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.Saved = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.Informed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
          this.CT = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.NotCrashed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
          this.NextStartOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.CheckedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.RefreshRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.MeasStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
          this.TLP.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.BN)).BeginInit();
          this.BN.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.Linaa)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.schAcqsDataGridView)).BeginInit();
          this.SuspendLayout();
          // 
          // TLP
          // 
          this.TLP.ColumnCount = 1;
          this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.TLP.Controls.Add(this.BN, 0, 0);
          this.TLP.Controls.Add(this.schAcqsDataGridView, 0, 1);
          this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
          this.TLP.Location = new System.Drawing.Point(0, 0);
          this.TLP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.TLP.Name = "TLP";
          this.TLP.RowCount = 2;
          this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.64526F));
          this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.35474F));
          this.TLP.Size = new System.Drawing.Size(1408, 402);
          this.TLP.TabIndex = 0;
          // 
          // BN
          // 
          this.BN.AddNewItem = this.bindingNavigatorAddNewItem;
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
            this.schAcqsBindingNavigatorSaveItem,
            this.toolStripSeparator1,
            this.Refreshbto});
          this.BN.Location = new System.Drawing.Point(0, 0);
          this.BN.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
          this.BN.MoveLastItem = this.bindingNavigatorMoveLastItem;
          this.BN.MoveNextItem = this.bindingNavigatorMoveNextItem;
          this.BN.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
          this.BN.Name = "BN";
          this.BN.PositionItem = this.bindingNavigatorPositionItem;
          this.BN.Size = new System.Drawing.Size(1408, 30);
          this.BN.TabIndex = 1;
          this.BN.Text = "bindingNavigator1";
          // 
          // bindingNavigatorAddNewItem
          // 
          this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
          this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
          this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
          this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 27);
          this.bindingNavigatorAddNewItem.Text = "Add new";
          // 
          // BS
          // 
          this.BS.DataMember = "SchAcqs";
          this.BS.DataSource = this.Linaa;
          // 
          // Linaa
          // 
         // this.Linaa.CurrentPref = null;
          this.Linaa.DataSetName = "LINAA";
          this.Linaa.DetectorsList = ((System.Collections.Generic.ICollection<string>)(resources.GetObject("Linaa.DetectorsList")));
          this.Linaa.EnforceConstraints = false;
          this.Linaa.FolderPath = null;
          
     //     this.Linaa.IStore = this.Linaa;
          this.Linaa.Locale = new System.Globalization.CultureInfo("en");
      //    this.Interface.IReport.Notify = null;
          this.Linaa.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
          this.Linaa.TAM = null;
        //  this.Linaa.WindowsUser = null;
          // 
          // bindingNavigatorCountItem
          // 
          this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
          this.bindingNavigatorCountItem.Size = new System.Drawing.Size(45, 27);
          this.bindingNavigatorCountItem.Text = "of {0}";
          this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
          // 
          // bindingNavigatorDeleteItem
          // 
          this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
          this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
          this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
          this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 27);
          this.bindingNavigatorDeleteItem.Text = "Delete";
          // 
          // bindingNavigatorMoveFirstItem
          // 
          this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
          this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
          this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
          this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 27);
          this.bindingNavigatorMoveFirstItem.Text = "Move first";
          // 
          // bindingNavigatorMovePreviousItem
          // 
          this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
          this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
          this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
          this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 27);
          this.bindingNavigatorMovePreviousItem.Text = "Move previous";
          // 
          // bindingNavigatorSeparator
          // 
          this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
          this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 30);
          // 
          // bindingNavigatorPositionItem
          // 
          this.bindingNavigatorPositionItem.AccessibleName = "Position";
          this.bindingNavigatorPositionItem.AutoSize = false;
          this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
          this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(65, 27);
          this.bindingNavigatorPositionItem.Text = "0";
          this.bindingNavigatorPositionItem.ToolTipText = "Current position";
          // 
          // bindingNavigatorSeparator1
          // 
          this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
          this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 30);
          // 
          // bindingNavigatorMoveNextItem
          // 
          this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
          this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
          this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
          this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 27);
          this.bindingNavigatorMoveNextItem.Text = "Move next";
          // 
          // bindingNavigatorMoveLastItem
          // 
          this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
          this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
          this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
          this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 27);
          this.bindingNavigatorMoveLastItem.Text = "Move last";
          // 
          // bindingNavigatorSeparator2
          // 
          this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
          this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 30);
          // 
          // schAcqsBindingNavigatorSaveItem
          // 
          this.schAcqsBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.schAcqsBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("schAcqsBindingNavigatorSaveItem.Image")));
          this.schAcqsBindingNavigatorSaveItem.Name = "schAcqsBindingNavigatorSaveItem";
          this.schAcqsBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 27);
          this.schAcqsBindingNavigatorSaveItem.Text = "Save Data";
          this.schAcqsBindingNavigatorSaveItem.Click += new System.EventHandler(this.schAcqsBindingNavigatorSaveItem_Click);
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
          // 
          // Refreshbto
          // 
          this.Refreshbto.Name = "Refreshbto";
          this.Refreshbto.Size = new System.Drawing.Size(58, 27);
          this.Refreshbto.Text = "Refresh";
          this.Refreshbto.Click += new System.EventHandler(this.Refresh_Click);
          // 
          // schAcqsDataGridView
          // 
          this.schAcqsDataGridView.AllowUserToAddRows = false;
          this.schAcqsDataGridView.AllowUserToOrderColumns = true;
          this.schAcqsDataGridView.AutoGenerateColumns = false;
          this.schAcqsDataGridView.BackgroundColor = System.Drawing.Color.BlanchedAlmond;
          this.schAcqsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.schAcqsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Detector,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.Counter,
            this.dataGridViewCheckBoxColumn1,
            this.User,
            this.Interrupted,
            this.IsAwake,
            this.Delay,
            this.Saved,
            this.Informed,
            this.CT,
            this.NotCrashed,
            this.NextStartOn,
            this.CheckedOn,
            this.RefreshRate,
            this.MeasStart});
          this.schAcqsDataGridView.DataSource = this.BS;
          this.schAcqsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
          this.schAcqsDataGridView.Location = new System.Drawing.Point(4, 34);
          this.schAcqsDataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.schAcqsDataGridView.Name = "schAcqsDataGridView";
          this.schAcqsDataGridView.Size = new System.Drawing.Size(1400, 364);
          this.schAcqsDataGridView.TabIndex = 0;
          // 
          // dataGridViewTextBoxColumn1
          // 
          this.dataGridViewTextBoxColumn1.DataPropertyName = "SID";
          this.dataGridViewTextBoxColumn1.HeaderText = "SID";
          this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
          this.dataGridViewTextBoxColumn1.Visible = false;
          // 
          // Detector
          // 
          this.Detector.DataPropertyName = "Detector";
          this.Detector.HeaderText = "Detector";
          this.Detector.Name = "Detector";
          // 
          // dataGridViewTextBoxColumn2
          // 
          this.dataGridViewTextBoxColumn2.DataPropertyName = "Project";
          this.dataGridViewTextBoxColumn2.HeaderText = "Project";
          this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
          // 
          // dataGridViewTextBoxColumn3
          // 
          this.dataGridViewTextBoxColumn3.DataPropertyName = "Sample";
          this.dataGridViewTextBoxColumn3.HeaderText = "Sample";
          this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
          // 
          // dataGridViewTextBoxColumn4
          // 
          this.dataGridViewTextBoxColumn4.DataPropertyName = "Position";
          this.dataGridViewTextBoxColumn4.HeaderText = "Position";
          this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
          // 
          // dataGridViewTextBoxColumn8
          // 
          this.dataGridViewTextBoxColumn8.DataPropertyName = "StartOn";
          this.dataGridViewTextBoxColumn8.HeaderText = "StartOn";
          this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
          this.dataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
          this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
          // 
          // dataGridViewTextBoxColumn5
          // 
          this.dataGridViewTextBoxColumn5.DataPropertyName = "LastMeas";
          this.dataGridViewTextBoxColumn5.HeaderText = "LastMeas";
          this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
          // 
          // dataGridViewTextBoxColumn6
          // 
          this.dataGridViewTextBoxColumn6.DataPropertyName = "PresetTime";
          this.dataGridViewTextBoxColumn6.HeaderText = "PresetTime";
          this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
          this.dataGridViewTextBoxColumn6.ToolTipText = "in seconds";
          // 
          // dataGridViewTextBoxColumn7
          // 
          this.dataGridViewTextBoxColumn7.DataPropertyName = "Repeats";
          this.dataGridViewTextBoxColumn7.HeaderText = "Repeats";
          this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
          // 
          // Counter
          // 
          this.Counter.DataPropertyName = "Counter";
          this.Counter.HeaderText = "Counter";
          this.Counter.Name = "Counter";
          // 
          // dataGridViewCheckBoxColumn1
          // 
          this.dataGridViewCheckBoxColumn1.DataPropertyName = "Done";
          this.dataGridViewCheckBoxColumn1.HeaderText = "Done";
          this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
          // 
          // User
          // 
          this.User.DataPropertyName = "User";
          this.User.HeaderText = "User";
          this.User.Name = "User";
          // 
          // Interrupted
          // 
          this.Interrupted.DataPropertyName = "Interrupted";
          this.Interrupted.HeaderText = "Interrupted";
          this.Interrupted.Name = "Interrupted";
          // 
          // IsAwake
          // 
          this.IsAwake.DataPropertyName = "IsAwake";
          this.IsAwake.HeaderText = "IsAwake";
          this.IsAwake.Name = "IsAwake";
          // 
          // Delay
          // 
          this.Delay.DataPropertyName = "Delay";
          this.Delay.HeaderText = "Delay";
          this.Delay.Name = "Delay";
          // 
          // Saved
          // 
          this.Saved.DataPropertyName = "Saved";
          this.Saved.HeaderText = "Saved";
          this.Saved.Name = "Saved";
          // 
          // Informed
          // 
          this.Informed.DataPropertyName = "Informed";
          this.Informed.HeaderText = "Informed";
          this.Informed.Name = "Informed";
          // 
          // CT
          // 
          this.CT.DataPropertyName = "CT";
          this.CT.HeaderText = "CT";
          this.CT.Name = "CT";
          // 
          // NotCrashed
          // 
          this.NotCrashed.DataPropertyName = "NotCrashed";
          this.NotCrashed.HeaderText = "NotCrashed";
          this.NotCrashed.Name = "NotCrashed";
          // 
          // NextStartOn
          // 
          this.NextStartOn.DataPropertyName = "NextStartOn";
          this.NextStartOn.HeaderText = "NextStartOn";
          this.NextStartOn.Name = "NextStartOn";
          // 
          // CheckedOn
          // 
          this.CheckedOn.DataPropertyName = "CheckedOn";
          this.CheckedOn.HeaderText = "CheckedOn";
          this.CheckedOn.Name = "CheckedOn";
          // 
          // RefreshRate
          // 
          this.RefreshRate.DataPropertyName = "RefreshRate";
          this.RefreshRate.HeaderText = "RefreshRate";
          this.RefreshRate.Name = "RefreshRate";
          // 
          // MeasStart
          // 
          this.MeasStart.DataPropertyName = "MeasStart";
          this.MeasStart.HeaderText = "MeasStart";
          this.MeasStart.Name = "MeasStart";
          // 
          // ucSchAcqs
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.Controls.Add(this.TLP);
          this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.Name = "ucSchAcqs";
          this.Size = new System.Drawing.Size(1408, 402);
          this.TLP.ResumeLayout(false);
          this.TLP.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this.BN)).EndInit();
          this.BN.ResumeLayout(false);
          this.BN.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.Linaa)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.schAcqsDataGridView)).EndInit();
          this.ResumeLayout(false);

	  }

	  #endregion

	  private System.Windows.Forms.TableLayoutPanel TLP;
	  private System.Windows.Forms.BindingNavigator BN;
	  private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
	  private System.Windows.Forms.BindingSource BS;
	  private DB.LINAA Linaa;
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
	  private System.Windows.Forms.DataGridView schAcqsDataGridView;
	  private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	  public System.Windows.Forms.ToolStripButton schAcqsBindingNavigatorSaveItem;
	  public System.Windows.Forms.ToolStripLabel Refreshbto;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
	  private System.Windows.Forms.DataGridViewTextBoxColumn Detector;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
	  private Rsx.DGV.CalendarColumn dataGridViewTextBoxColumn8;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
	  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
	  private System.Windows.Forms.DataGridViewTextBoxColumn Counter;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
	  private System.Windows.Forms.DataGridViewTextBoxColumn User;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn Interrupted;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn IsAwake;
	  private System.Windows.Forms.DataGridViewTextBoxColumn Delay;
	  private System.Windows.Forms.DataGridViewTextBoxColumn Saved;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn Informed;
	  private System.Windows.Forms.DataGridViewTextBoxColumn CT;
	  private System.Windows.Forms.DataGridViewCheckBoxColumn NotCrashed;
	  private System.Windows.Forms.DataGridViewTextBoxColumn NextStartOn;
	  private System.Windows.Forms.DataGridViewTextBoxColumn CheckedOn;
	  private System.Windows.Forms.DataGridViewTextBoxColumn RefreshRate;
	  private System.Windows.Forms.DataGridViewTextBoxColumn MeasStart;
   }
}
