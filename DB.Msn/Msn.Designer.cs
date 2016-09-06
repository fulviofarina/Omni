namespace DB
{
    partial class Msn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Msn));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.iconic = new System.Windows.Forms.PictureBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconic)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.95547F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.04453F));
            this.tableLayoutPanel.Controls.Add(this.iconic, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.title, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.36842F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.63158F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(494, 95);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // iconic
            // 
            this.iconic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iconic.Image = ((System.Drawing.Image)(resources.GetObject("iconic.Image")));
            this.iconic.Location = new System.Drawing.Point(3, 3);
            this.iconic.Name = "iconic";
            this.tableLayoutPanel.SetRowSpan(this.iconic, 2);
            this.iconic.Size = new System.Drawing.Size(57, 89);
            this.iconic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconic.TabIndex = 12;
            this.iconic.TabStop = false;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxDescription.ForeColor = System.Drawing.Color.DarkOrange;
            this.textBoxDescription.Location = new System.Drawing.Point(69, 29);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(422, 63);
            this.textBoxDescription.TabIndex = 23;
            this.textBoxDescription.TabStop = false;
            this.textBoxDescription.Text = "Description";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Location = new System.Drawing.Point(66, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(425, 26);
            this.title.TabIndex = 24;
            this.title.Text = "Title";
            // 
            // Msn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(512, 113);
        
            this.Controls.Add(this.tableLayoutPanel);
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Location = new System.Drawing.Point(1200, 450);
         
            this.Name = "Msn";
           
            this.Padding = new System.Windows.Forms.Padding(9);
           
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        public System.Windows.Forms.TextBox textBoxDescription;
        public System.Windows.Forms.Label title;
        public System.Windows.Forms.PictureBox iconic;
    }
}
