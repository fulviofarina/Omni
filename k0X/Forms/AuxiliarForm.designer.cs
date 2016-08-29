namespace k0X
{
    partial class AuxiliarForm
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
		   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuxiliarForm));
		   this.TLP = new System.Windows.Forms.TableLayoutPanel();
		   this.SuspendLayout();
		   // 
		   // TLP
		   // 
		   this.TLP.AutoScroll = true;
		   this.TLP.AutoSize = true;
		   this.TLP.ColumnCount = 1;
		   this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		   this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.TLP.Location = new System.Drawing.Point(0, 0);
		   this.TLP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		   this.TLP.Name = "TLP";
		   this.TLP.RowCount = 1;
		   this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
		   this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 655F));
		   this.TLP.Size = new System.Drawing.Size(1041, 655);
		   this.TLP.TabIndex = 7;
		   this.TLP.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.TLP_ControlRemoved);
		   // 
		   // AuxiliarForm
		   // 
		   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
		   this.AutoSize = true;
		   this.ClientSize = new System.Drawing.Size(1041, 655);
		   this.Controls.Add(this.TLP);
		   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
		   this.Margin = new System.Windows.Forms.Padding(4);
		   this.MaximizeBox = false;
		   this.Name = "AuxiliarForm";
		   this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
		   this.Text = "AuxiliarForm";
		   this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuxiliarForm_FormClosing);
		   this.LocationChanged += new System.EventHandler(this.AuxiliarForm_LocationChanged);
		   this.MouseCaptureChanged += new System.EventHandler(this.AuxiliarForm_MouseCaptureChanged);
		   this.ResumeLayout(false);
		   this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP;




    }
}