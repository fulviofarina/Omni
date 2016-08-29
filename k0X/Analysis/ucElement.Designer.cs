namespace k0X
{
    partial class ucElement
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
		   this.m_Sym = new System.Windows.Forms.Label();
		   this.BS = new System.Windows.Forms.BindingSource(this.components);
		   ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
		   this.SuspendLayout();
		   // 
		   // m_Sym
		   // 
		   this.m_Sym.BackColor = System.Drawing.Color.Transparent;
		   this.m_Sym.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.m_Sym.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		   this.m_Sym.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.m_Sym.Location = new System.Drawing.Point(0, 0);
		   this.m_Sym.Name = "m_Sym";
		   this.m_Sym.Size = new System.Drawing.Size(35, 28);
		   this.m_Sym.TabIndex = 2;
		   this.m_Sym.Text = "El";
		   this.m_Sym.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		   this.m_Sym.MouseHover += new System.EventHandler(this.m_Sym_MouseHover);
		   // 
		   // ucElement
		   // 
		   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
		   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		   this.AutoSize = true;
		   this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		   this.Controls.Add(this.m_Sym);
		   this.Font = new System.Drawing.Font("Segoe UI", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		   this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		   this.Name = "ucElement";
		   this.Size = new System.Drawing.Size(35, 28);
		
		   ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
		   this.ResumeLayout(false);

        }

     
        #endregion

		private System.Windows.Forms.Label m_Sym;
		private System.Windows.Forms.BindingSource BS;

    }
}
