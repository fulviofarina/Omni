namespace DB.UI
{
    partial class PickerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
		   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickerForm));
		   this.TLP = new System.Windows.Forms.TableLayoutPanel();
		   this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		   this.Take = new System.Windows.Forms.Button();
		   this.Cancel = new System.Windows.Forms.Button();
		   this.TLP.SuspendLayout();
		   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
		   this.splitContainer1.Panel1.SuspendLayout();
		   this.splitContainer1.Panel2.SuspendLayout();
		   this.splitContainer1.SuspendLayout();
		   this.SuspendLayout();
		   // 
		   // TLP
		   // 
		   this.TLP.AutoSize = true;
		   this.TLP.ColumnCount = 1;
		   this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
		   this.TLP.Controls.Add(this.splitContainer1, 0, 0);
		   this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.TLP.Location = new System.Drawing.Point(0, 0);
		   this.TLP.Name = "TLP";
		   this.TLP.RowCount = 2;
		   this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
		   this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
		   this.TLP.Size = new System.Drawing.Size(1291, 615);
		   this.TLP.TabIndex = 0;
		   // 
		   // splitContainer1
		   // 
		   this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.splitContainer1.Location = new System.Drawing.Point(3, 3);
		   this.splitContainer1.Name = "splitContainer1";
		   // 
		   // splitContainer1.Panel1
		   // 
		   this.splitContainer1.Panel1.Controls.Add(this.Take);
		   // 
		   // splitContainer1.Panel2
		   // 
		   this.splitContainer1.Panel2.Controls.Add(this.Cancel);
		   this.splitContainer1.Size = new System.Drawing.Size(1285, 44);
		   this.splitContainer1.SplitterDistance = 601;
		   this.splitContainer1.TabIndex = 0;
		   // 
		   // Take
		   // 
		   this.Take.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.Take.Location = new System.Drawing.Point(0, 0);
		   this.Take.Name = "Take";
		   this.Take.Size = new System.Drawing.Size(601, 44);
		   this.Take.TabIndex = 0;
		   this.Take.Text = "Take";
		   this.Take.UseVisualStyleBackColor = true;
		   this.Take.Click += new System.EventHandler(this.Take_Click);
		   // 
		   // Cancel
		   // 
		   this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		   this.Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.Cancel.Location = new System.Drawing.Point(0, 0);
		   this.Cancel.Name = "Cancel";
		   this.Cancel.Size = new System.Drawing.Size(680, 44);
		   this.Cancel.TabIndex = 0;
		   this.Cancel.Text = "Cancel";
		   this.Cancel.UseVisualStyleBackColor = true;
		   this.Cancel.Click += new System.EventHandler(this.Take_Click);
		   // 
		   // PickerForm
		   // 
		   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
		   this.AutoSize = true;
		   this.CancelButton = this.Cancel;
		   this.ClientSize = new System.Drawing.Size(1291, 615);
		   this.ControlBox = false;
		   this.Controls.Add(this.TLP);
		   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
		   this.Margin = new System.Windows.Forms.Padding(4);
		   this.MaximizeBox = false;
		   this.MinimizeBox = false;
		   this.Name = "PickerForm";
		   this.Text = "Picker";
		   this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PickerForm_FormClosing);
		   this.TLP.ResumeLayout(false);
		   this.splitContainer1.Panel1.ResumeLayout(false);
		   this.splitContainer1.Panel2.ResumeLayout(false);
		   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
		   this.splitContainer1.ResumeLayout(false);
		   this.ResumeLayout(false);
		   this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button Take;
        private System.Windows.Forms.Button Cancel;





    }
}