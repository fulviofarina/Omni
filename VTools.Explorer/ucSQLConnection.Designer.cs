namespace VTools
{
    partial class ucSQLConnection
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
            this.label4 = new System.Windows.Forms.Label();
            this.enlistBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.poolingBox = new System.Windows.Forms.ComboBox();
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
            this.testButton = new System.Windows.Forms.Button();
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
            this.htlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.htlp.Controls.Add(this.label4, 0, 7);
            this.htlp.Controls.Add(this.enlistBox, 1, 7);
            this.htlp.Controls.Add(this.label3, 0, 6);
            this.htlp.Controls.Add(this.poolingBox, 1, 6);
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
            this.htlp.Controls.Add(this.testButton, 0, 8);
            this.htlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htlp.Location = new System.Drawing.Point(0, 0);
            this.htlp.Name = "htlp";
            this.htlp.RowCount = 9;
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.htlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.htlp.Size = new System.Drawing.Size(513, 339);
            this.htlp.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(250, 38);
            this.label4.TabIndex = 19;
            this.label4.Text = "Enlist";
            // 
            // enlistBox
            // 
            this.enlistBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enlistBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.enlistBox.Location = new System.Drawing.Point(259, 269);
            this.enlistBox.Name = "enlistBox";
            this.enlistBox.Size = new System.Drawing.Size(251, 29);
            this.enlistBox.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 38);
            this.label3.TabIndex = 17;
            this.label3.Text = "Pooling";
            // 
            // poolingBox
            // 
            this.poolingBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poolingBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.poolingBox.Location = new System.Drawing.Point(259, 231);
            this.poolingBox.Name = "poolingBox";
            this.poolingBox.Size = new System.Drawing.Size(251, 29);
            this.poolingBox.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(250, 38);
            this.label8.TabIndex = 15;
            this.label8.Text = "Connection Timeout (s)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 38);
            this.label7.TabIndex = 14;
            this.label7.Text = "Persist Security Info";
            // 
            // timeoutboxHL
            // 
            this.timeoutboxHL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeoutboxHL.Location = new System.Drawing.Point(259, 155);
            this.timeoutboxHL.Name = "timeoutboxHL";
            this.timeoutboxHL.Size = new System.Drawing.Size(251, 29);
            this.timeoutboxHL.TabIndex = 13;
            // 
            // securityInfoHL
            // 
            this.securityInfoHL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.securityInfoHL.Items.AddRange(new object[] {
            "True",
            "False"});
            this.securityInfoHL.Location = new System.Drawing.Point(259, 193);
            this.securityInfoHL.Name = "securityInfoHL";
            this.securityInfoHL.Size = new System.Drawing.Size(251, 29);
            this.securityInfoHL.TabIndex = 12;
            // 
            // hsrv
            // 
            this.hsrv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsrv.Location = new System.Drawing.Point(259, 3);
            this.hsrv.Name = "hsrv";
            this.hsrv.Size = new System.Drawing.Size(251, 29);
            this.hsrv.TabIndex = 11;
            // 
            // HyperLab
            // 
            this.HyperLab.AutoSize = true;
            this.HyperLab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HyperLab.ForeColor = System.Drawing.Color.Firebrick;
            this.HyperLab.Location = new System.Drawing.Point(3, 0);
            this.HyperLab.Name = "HyperLab";
            this.HyperLab.Size = new System.Drawing.Size(250, 38);
            this.HyperLab.TabIndex = 4;
            this.HyperLab.Text = "Data Source";
            // 
            // hdb
            // 
            this.hdb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hdb.Location = new System.Drawing.Point(259, 41);
            this.hdb.Name = "hdb";
            this.hdb.Size = new System.Drawing.Size(251, 29);
            this.hdb.TabIndex = 5;
            // 
            // hlogin
            // 
            this.hlogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hlogin.Location = new System.Drawing.Point(259, 79);
            this.hlogin.Name = "hlogin";
            this.hlogin.Size = new System.Drawing.Size(251, 29);
            this.hlogin.TabIndex = 6;
            // 
            // hpass
            // 
            this.hpass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hpass.Location = new System.Drawing.Point(259, 117);
            this.hpass.Name = "hpass";
            this.hpass.Size = new System.Drawing.Size(251, 29);
            this.hpass.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 38);
            this.label1.TabIndex = 8;
            this.label1.Text = "Database (or Initial Catalog)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 38);
            this.label2.TabIndex = 9;
            this.label2.Text = "User ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(250, 38);
            this.label5.TabIndex = 10;
            this.label5.Text = "Password";
            // 
            // HyperLabRTB
            // 
            this.HyperLabRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HyperLabRTB.Location = new System.Drawing.Point(0, 0);
            this.HyperLabRTB.Name = "HyperLabRTB";
            this.HyperLabRTB.Size = new System.Drawing.Size(308, 339);
            this.HyperLabRTB.TabIndex = 1;
            this.HyperLabRTB.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
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
            this.splitContainer1.Size = new System.Drawing.Size(825, 339);
            this.splitContainer1.SplitterDistance = 513;
            this.splitContainer1.TabIndex = 11;
            // 
            // testButton
            // 
            this.testButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testButton.Location = new System.Drawing.Point(3, 307);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(250, 29);
            this.testButton.TabIndex = 20;
            this.testButton.Text = "TEST Connection";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // ucSQLConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucSQLConnection";
            this.Size = new System.Drawing.Size(825, 339);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox enlistBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox poolingBox;
        private System.Windows.Forms.Button testButton;
    }
}