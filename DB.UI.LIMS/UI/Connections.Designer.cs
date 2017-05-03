namespace DB.UI
{
    partial class Connections
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connections));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SpectraSvr = new System.Windows.Forms.Label();
            this.Spectra = new System.Windows.Forms.Label();
            this.SpectraRBT = new System.Windows.Forms.RichTextBox();
            this.SpectraSvrRBT = new System.Windows.Forms.RichTextBox();
            this.ucSQLHLCom = new VTools.ucSQLConnection();
            this.ucSQLLIMSCom = new VTools.ucSQLConnection();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.23792F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.76208F));
            this.tableLayoutPanel1.Controls.Add(this.ucSQLLIMSCom, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SpectraSvr, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Spectra, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.SpectraRBT, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.SpectraSvrRBT, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ucSQLHLCom, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.6F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(883, 610);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // SpectraSvr
            // 
            this.SpectraSvr.AutoSize = true;
            this.SpectraSvr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpectraSvr.ForeColor = System.Drawing.Color.Firebrick;
            this.SpectraSvr.Location = new System.Drawing.Point(3, 546);
            this.SpectraSvr.Name = "SpectraSvr";
            this.SpectraSvr.Size = new System.Drawing.Size(384, 64);
            this.SpectraSvr.TabIndex = 7;
            this.SpectraSvr.Text = "Genie Server PC";
            // 
            // Spectra
            // 
            this.Spectra.AutoSize = true;
            this.Spectra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Spectra.ForeColor = System.Drawing.Color.Firebrick;
            this.Spectra.Location = new System.Drawing.Point(3, 475);
            this.Spectra.Name = "Spectra";
            this.Spectra.Size = new System.Drawing.Size(384, 71);
            this.Spectra.TabIndex = 6;
            this.Spectra.Text = "Spectra Folder Path";
            // 
            // SpectraRBT
            // 
            this.SpectraRBT.Location = new System.Drawing.Point(393, 478);
            this.SpectraRBT.Name = "SpectraRBT";
            this.SpectraRBT.Size = new System.Drawing.Size(487, 62);
            this.SpectraRBT.TabIndex = 8;
            this.SpectraRBT.Text = "";
            // 
            // SpectraSvrRBT
            // 
            this.SpectraSvrRBT.Location = new System.Drawing.Point(393, 549);
            this.SpectraSvrRBT.Name = "SpectraSvrRBT";
            this.SpectraSvrRBT.Size = new System.Drawing.Size(487, 56);
            this.SpectraSvrRBT.TabIndex = 9;
            this.SpectraSvrRBT.Text = "";
            // 
            // ucSQLHLCom
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucSQLHLCom, 2);
            this.ucSQLHLCom.ConnectionString = "";
            this.ucSQLHLCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSQLHLCom.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSQLHLCom.Location = new System.Drawing.Point(3, 3);
            this.ucSQLHLCom.Name = "ucSQLHLCom";
            this.ucSQLHLCom.Size = new System.Drawing.Size(877, 249);
            this.ucSQLHLCom.TabIndex = 10;
            // 
            // ucSQLLIMSCom
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucSQLLIMSCom, 2);
            this.ucSQLLIMSCom.ConnectionString = "";
            this.ucSQLLIMSCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSQLLIMSCom.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSQLLIMSCom.Location = new System.Drawing.Point(3, 258);
            this.ucSQLLIMSCom.Name = "ucSQLLIMSCom";
            this.ucSQLLIMSCom.Size = new System.Drawing.Size(877, 214);
            this.ucSQLLIMSCom.TabIndex = 11;
            // 
            // Connections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 610);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Connections";
            this.Text = "Connections";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Connections_FormClosing);
            this.Load += new System.EventHandler(this.Connections_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label Spectra;
        private System.Windows.Forms.Label SpectraSvr;
        private System.Windows.Forms.RichTextBox SpectraRBT;
        private System.Windows.Forms.RichTextBox SpectraSvrRBT;
        private VTools.ucSQLConnection ucSQLLIMSCom;
        private VTools.ucSQLConnection ucSQLHLCom;
    }
}