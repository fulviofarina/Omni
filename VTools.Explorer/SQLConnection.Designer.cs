namespace VTools
{
    partial class SQLConnection
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
            this.htlp = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.timeoutboxHL = new System.Windows.Forms.TextBox();
            this.securityInfoHL = new System.Windows.Forms.ComboBox();
            this.hsrv = new System.Windows.Forms.ComboBox();
            this.HyperLab = new System.Windows.Forms.Label();
            this.hdb = new System.Windows.Forms.ComboBox();
            this.hlogin = new System.Windows.Forms.TextBox();
            this.hpass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.HyperLabRTB = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.htlp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htlp
            // 
            this.htlp.ColumnCount = 2;
            this.htlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.htlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            this.htlp.Controls.Add(this.label8, 0, 4);
            this.htlp.Controls.Add(this.label7, 0, 5);
            this.htlp.Controls.Add(this.timeoutboxHL, 1, 4);
            this.htlp.Controls.Add(this.securityInfoHL, 1, 5);
            this.htlp.Controls.Add(this.hsrv, 1, 0);
            this.htlp.Controls.Add(this.HyperLab, 0, 0);
            this.htlp.Controls.Add(this.hdb, 1, 1);
            this.htlp.Controls.Add(this.hlogin, 1, 2);
            this.htlp.Controls.Add(this.hpass, 1, 3);
            this.htlp.Controls.Add(this.label1, 0, 1);
            this.htlp.Controls.Add(this.label2, 0, 2);
            this.htlp.Controls.Add(this.label5, 0, 3);
            this.htlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htlp.Location = new System.Drawing.Point(0, 0);
            this.htlp.Name = "htlp";
            this.htlp.RowCount = 6;
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.htlp.Size = new System.Drawing.Size(414, 198);
            this.htlp.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 32);
            this.label8.TabIndex = 15;
            this.label8.Text = "Timeout (s)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 32);
            this.label7.TabIndex = 14;
            this.label7.Text = "Persist Security Info";
            // 
            // timeoutboxHL
            // 
            this.timeoutboxHL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeoutboxHL.Location = new System.Drawing.Point(169, 137);
            this.timeoutboxHL.Name = "timeoutboxHL";
            this.timeoutboxHL.Size = new System.Drawing.Size(242, 29);
            this.timeoutboxHL.TabIndex = 13;
            // 
            // securityInfoHL
            // 
            this.securityInfoHL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.securityInfoHL.Items.AddRange(new object[] {
            "True",
            "False"});
            this.securityInfoHL.Location = new System.Drawing.Point(169, 169);
            this.securityInfoHL.Name = "securityInfoHL";
            this.securityInfoHL.Size = new System.Drawing.Size(242, 29);
            this.securityInfoHL.TabIndex = 12;
            // 
            // hsrv
            // 
            this.hsrv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsrv.Location = new System.Drawing.Point(169, 3);
            this.hsrv.Name = "hsrv";
            this.hsrv.Size = new System.Drawing.Size(242, 29);
            this.hsrv.TabIndex = 11;
            // 
            // HyperLab
            // 
            this.HyperLab.AutoSize = true;
            this.HyperLab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HyperLab.ForeColor = System.Drawing.Color.Firebrick;
            this.HyperLab.Location = new System.Drawing.Point(3, 0);
            this.HyperLab.Name = "HyperLab";
            this.HyperLab.Size = new System.Drawing.Size(160, 33);
            this.HyperLab.TabIndex = 4;
            this.HyperLab.Text = "Data Source";
            // 
            // hdb
            // 
            this.hdb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hdb.Location = new System.Drawing.Point(169, 36);
            this.hdb.Name = "hdb";
            this.hdb.Size = new System.Drawing.Size(242, 29);
            this.hdb.TabIndex = 5;
            // 
            // hlogin
            // 
            this.hlogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hlogin.Location = new System.Drawing.Point(169, 69);
            this.hlogin.Name = "hlogin";
            this.hlogin.Size = new System.Drawing.Size(242, 29);
            this.hlogin.TabIndex = 6;
            // 
            // hpass
            // 
            this.hpass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hpass.Location = new System.Drawing.Point(169, 100);
            this.hpass.Name = "hpass";
            this.hpass.Size = new System.Drawing.Size(242, 29);
            this.hpass.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 33);
            this.label1.TabIndex = 8;
            this.label1.Text = "Initial Catalog";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 31);
            this.label2.TabIndex = 9;
            this.label2.Text = "User";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 37);
            this.label5.TabIndex = 10;
            this.label5.Text = "Password";
            // 
            // HyperLabRTB
            // 
            this.HyperLabRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HyperLabRTB.Location = new System.Drawing.Point(0, 0);
            this.HyperLabRTB.Name = "HyperLabRTB";
            this.HyperLabRTB.Size = new System.Drawing.Size(336, 198);
            this.HyperLabRTB.TabIndex = 1;
            this.HyperLabRTB.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.htlp);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.HyperLabRTB);
            this.splitContainer1.Size = new System.Drawing.Size(754, 198);
            this.splitContainer1.SplitterDistance = 414;
            this.splitContainer1.TabIndex = 11;
            // 
            // SQLConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SQLConnection";
            this.Size = new System.Drawing.Size(754, 198);
            this.htlp.ResumeLayout(false);
            this.htlp.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel htlp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox timeoutboxHL;
        private System.Windows.Forms.ComboBox securityInfoHL;
        private System.Windows.Forms.ComboBox hsrv;
        private System.Windows.Forms.Label HyperLab;
        private System.Windows.Forms.ComboBox hdb;
        private System.Windows.Forms.TextBox hlogin;
        private System.Windows.Forms.TextBox hpass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox HyperLabRTB;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}