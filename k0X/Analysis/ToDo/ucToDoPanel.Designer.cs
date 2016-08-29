





namespace k0X
{
    partial class ucToDoPanel
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucToDoPanel));
            this.MTLP = new System.Windows.Forms.TableLayoutPanel();
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.panel2TS = new System.Windows.Forms.ToolStrip();
            this.panel2box = new System.Windows.Forms.ToolStripComboBox();
            this.Undock2 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1TS = new System.Windows.Forms.ToolStrip();
            this.panel1box = new System.Windows.Forms.ToolStripComboBox();
            this.Undock1 = new System.Windows.Forms.ToolStripButton();
            this.ToDoMenu = new System.Windows.Forms.MenuStrip();
            this.Load = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToDo = new System.Windows.Forms.ToolStripMenuItem();
            this.View = new System.Windows.Forms.ToolStripMenuItem();
            this.Auto = new System.Windows.Forms.ToolStripMenuItem();
            this.MTLP.SuspendLayout();
            this.TLP.SuspendLayout();
            this.panel2TS.SuspendLayout();
            this.panel1TS.SuspendLayout();
            this.ToDoMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MTLP
            // 
            this.MTLP.ColumnCount = 1;
            this.MTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MTLP.Controls.Add(this.TLP, 0, 1);
            this.MTLP.Controls.Add(this.ToDoMenu, 0, 0);
            this.MTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MTLP.Location = new System.Drawing.Point(0, 0);
            this.MTLP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MTLP.Name = "MTLP";
            this.MTLP.RowCount = 2;
            this.MTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.580153F));
            this.MTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.41985F));
            this.MTLP.Size = new System.Drawing.Size(579, 789);
            this.MTLP.TabIndex = 10;
            // 
            // TLP
            // 
            this.TLP.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.TLP.ColumnCount = 2;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP.Controls.Add(this.panel2TS, 1, 1);
            this.TLP.Controls.Add(this.panel1, 0, 0);
            this.TLP.Controls.Add(this.panel2, 1, 0);
            this.TLP.Controls.Add(this.panel1TS, 0, 1);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(3, 38);
            this.TLP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 2;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.33012F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.669887F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP.Size = new System.Drawing.Size(573, 749);
            this.TLP.TabIndex = 6;
            // 
            // panel2TS
            // 
            this.panel2TS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.panel2box,
            this.Undock2});
            this.panel2TS.Location = new System.Drawing.Point(287, 713);
            this.panel2TS.Name = "panel2TS";
            this.panel2TS.Size = new System.Drawing.Size(285, 35);
            this.panel2TS.TabIndex = 7;
            // 
            // panel2box
            // 
            this.panel2box.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.panel2box.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.panel2box.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.panel2box.Name = "panel2box";
            this.panel2box.Size = new System.Drawing.Size(100, 35);
            this.panel2box.Text = "Z945";
            this.panel2box.KeyUp += new System.Windows.Forms.KeyEventHandler(this.panelbox_KeyUp);
            // 
            // Undock2
            // 
            this.Undock2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Undock2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Undock2.Name = "Undock2";
            this.Undock2.Size = new System.Drawing.Size(63, 32);
            this.Undock2.Text = "Undock";
            this.Undock2.Click += new System.EventHandler(this.Undock_Click);
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 707);
            this.panel1.TabIndex = 4;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // panel2
            // 
            this.panel2.AllowDrop = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(290, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 707);
            this.panel2.TabIndex = 5;
            this.panel2.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.panel2.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // panel1TS
            // 
            this.panel1TS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1TS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.panel1box,
            this.Undock1});
            this.panel1TS.Location = new System.Drawing.Point(1, 713);
            this.panel1TS.Name = "panel1TS";
            this.panel1TS.Size = new System.Drawing.Size(285, 35);
            this.panel1TS.TabIndex = 6;
            // 
            // panel1box
            // 
            this.panel1box.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.panel1box.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.panel1box.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.panel1box.Name = "panel1box";
            this.panel1box.Size = new System.Drawing.Size(100, 35);
            this.panel1box.Text = "Z945";
            this.panel1box.KeyUp += new System.Windows.Forms.KeyEventHandler(this.panelbox_KeyUp);
            // 
            // Undock1
            // 
            this.Undock1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Undock1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Undock1.Name = "Undock1";
            this.Undock1.Size = new System.Drawing.Size(63, 32);
            this.Undock1.Text = "Undock";
            this.Undock1.Click += new System.EventHandler(this.Undock_Click);
            // 
            // ToDoMenu
            // 
            this.ToDoMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToDoMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Load,
            this.Auto,
            this.AddToDo,
            this.View});
            this.ToDoMenu.Location = new System.Drawing.Point(0, 0);
            this.ToDoMenu.Name = "ToDoMenu";
            this.ToDoMenu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.ToDoMenu.Size = new System.Drawing.Size(579, 36);
            this.ToDoMenu.TabIndex = 9;
            this.ToDoMenu.Text = "ToDo Menu";
            // 
            // Load
            // 
            this.Load.Image = ((System.Drawing.Image)(resources.GetObject("Load.Image")));
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(70, 32);
            this.Load.Text = "Load";
            this.Load.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // AddToDo
            // 
            this.AddToDo.Image = ((System.Drawing.Image)(resources.GetObject("AddToDo.Image")));
            this.AddToDo.Name = "AddToDo";
            this.AddToDo.Size = new System.Drawing.Size(65, 32);
            this.AddToDo.Text = "Add";
            this.AddToDo.Click += new System.EventHandler(this.AddToDo_Click);
            // 
            // View
            // 
            this.View.Image = ((System.Drawing.Image)(resources.GetObject("View.Image")));
            this.View.Name = "View";
            this.View.Size = new System.Drawing.Size(69, 32);
            this.View.Text = "View";
            this.View.Click += new System.EventHandler(this.View_Click);
            // 
            // Auto
            // 
            this.Auto.Name = "Auto";
            this.Auto.Size = new System.Drawing.Size(53, 32);
            this.Auto.Text = "Auto";
            this.Auto.Click += new System.EventHandler(this.AddToDo_Click);
            // 
            // ucToDoPanel
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MTLP);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucToDoPanel";
            this.Size = new System.Drawing.Size(579, 789);
            this.MTLP.ResumeLayout(false);
            this.MTLP.PerformLayout();
            this.TLP.ResumeLayout(false);
            this.TLP.PerformLayout();
            this.panel2TS.ResumeLayout(false);
            this.panel2TS.PerformLayout();
            this.panel1TS.ResumeLayout(false);
            this.panel1TS.PerformLayout();
            this.ToDoMenu.ResumeLayout(false);
            this.ToDoMenu.PerformLayout();
            this.ResumeLayout(false);

        }

     

        #endregion

        private System.Windows.Forms.ToolStripMenuItem AddToDo;
        private System.Windows.Forms.TableLayoutPanel MTLP;
        public System.Windows.Forms.ToolStripComboBox panel1box;
        private System.Windows.Forms.ToolStrip panel1TS;
        public System.Windows.Forms.ToolStripComboBox panel2box;
        private System.Windows.Forms.ToolStrip panel2TS;
      
        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.ToolStripButton Undock1;
        private System.Windows.Forms.ToolStripButton Undock2;
        private System.Windows.Forms.ToolStripMenuItem View;

        public System.Windows.Forms.MenuStrip ToDoMenu;
        public System.Windows.Forms.ToolStripMenuItem Load;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem Auto;
    }
}
