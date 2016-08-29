namespace k0X
{
    partial class ucOrder
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
            this.Box = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Box
            // 
            this.Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Box.FormattingEnabled = true;
            this.Box.Location = new System.Drawing.Point(0, 0);
            this.Box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(789, 24);
            this.Box.TabIndex = 0;
            this.Box.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Box_KeyUp);
            // 
            // ucOrder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.Box);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucOrder";
            this.Size = new System.Drawing.Size(789, 35);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ComboBox Box;

    }
}
