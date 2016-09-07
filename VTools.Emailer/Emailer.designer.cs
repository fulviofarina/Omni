namespace VTools
{
    partial class Emailer
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
		   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Emailer));
		   this.groupBox1 = new System.Windows.Forms.GroupBox();
		   this.txtSubjectLine = new System.Windows.Forms.TextBox();
		   this.txtSendFrom = new System.Windows.Forms.TextBox();
		   this.lblSubjectLine = new System.Windows.Forms.Label();
		   this.lblSendFrom = new System.Windows.Forms.Label();
		   this.groupBox2 = new System.Windows.Forms.GroupBox();
		   this.txtMessage = new System.Windows.Forms.TextBox();
		   this.groupBox3 = new System.Windows.Forms.GroupBox();
		   this.btnAdd = new System.Windows.Forms.Button();
		   this.txtAttachments = new System.Windows.Forms.TextBox();
		   this.btnSend = new System.Windows.Forms.Button();
		   this.btnCancel = new System.Windows.Forms.Button();
		   this.OFD = new System.Windows.Forms.OpenFileDialog();
		   this.txtsendto = new System.Windows.Forms.TextBox();
		   this.sendtolbl = new System.Windows.Forms.Label();
		   this.groupBox1.SuspendLayout();
		   this.groupBox2.SuspendLayout();
		   this.groupBox3.SuspendLayout();
		   this.SuspendLayout();
		   // 
		   // groupBox1
		   // 
		   this.groupBox1.Controls.Add(this.txtsendto);
		   this.groupBox1.Controls.Add(this.sendtolbl);
		   this.groupBox1.Controls.Add(this.txtSubjectLine);
		   this.groupBox1.Controls.Add(this.txtSendFrom);
		   this.groupBox1.Controls.Add(this.lblSubjectLine);
		   this.groupBox1.Controls.Add(this.lblSendFrom);
		   this.groupBox1.Location = new System.Drawing.Point(13, 12);
		   this.groupBox1.Name = "groupBox1";
		   this.groupBox1.Size = new System.Drawing.Size(485, 116);
		   this.groupBox1.TabIndex = 0;
		   this.groupBox1.TabStop = false;
		   this.groupBox1.Text = "Header";
		   // 
		   // txtSubjectLine
		   // 
		   this.txtSubjectLine.Location = new System.Drawing.Point(70, 58);
		   this.txtSubjectLine.Name = "txtSubjectLine";
		   this.txtSubjectLine.Size = new System.Drawing.Size(399, 20);
		   this.txtSubjectLine.TabIndex = 5;
		   // 
		   // txtSendFrom
		   // 
		   this.txtSendFrom.Location = new System.Drawing.Point(70, 27);
		   this.txtSendFrom.Name = "txtSendFrom";
		   this.txtSendFrom.Size = new System.Drawing.Size(399, 20);
		   this.txtSendFrom.TabIndex = 4;
		   // 
		   // lblSubjectLine
		   // 
		   this.lblSubjectLine.AutoSize = true;
		   this.lblSubjectLine.Location = new System.Drawing.Point(22, 62);
		   this.lblSubjectLine.Name = "lblSubjectLine";
		   this.lblSubjectLine.Size = new System.Drawing.Size(46, 13);
		   this.lblSubjectLine.TabIndex = 2;
		   this.lblSubjectLine.Text = "Subject:";
		   // 
		   // lblSendFrom
		   // 
		   this.lblSendFrom.AutoSize = true;
		   this.lblSendFrom.Location = new System.Drawing.Point(35, 31);
		   this.lblSendFrom.Name = "lblSendFrom";
		   this.lblSendFrom.Size = new System.Drawing.Size(33, 13);
		   this.lblSendFrom.TabIndex = 1;
		   this.lblSendFrom.Text = "From:";
		   // 
		   // groupBox2
		   // 
		   this.groupBox2.Controls.Add(this.txtMessage);
		   this.groupBox2.Location = new System.Drawing.Point(13, 147);
		   this.groupBox2.Name = "groupBox2";
		   this.groupBox2.Size = new System.Drawing.Size(485, 177);
		   this.groupBox2.TabIndex = 1;
		   this.groupBox2.TabStop = false;
		   this.groupBox2.Text = "Message";
		   // 
		   // txtMessage
		   // 
		   this.txtMessage.Location = new System.Drawing.Point(25, 20);
		   this.txtMessage.Multiline = true;
		   this.txtMessage.Name = "txtMessage";
		   this.txtMessage.Size = new System.Drawing.Size(444, 167);
		   this.txtMessage.TabIndex = 0;
		   // 
		   // groupBox3
		   // 
		   this.groupBox3.Controls.Add(this.btnAdd);
		   this.groupBox3.Controls.Add(this.txtAttachments);
		   this.groupBox3.Location = new System.Drawing.Point(13, 330);
		   this.groupBox3.Name = "groupBox3";
		   this.groupBox3.Size = new System.Drawing.Size(485, 63);
		   this.groupBox3.TabIndex = 2;
		   this.groupBox3.TabStop = false;
		   this.groupBox3.Text = "Attachments";
		   // 
		   // btnAdd
		   // 
		   this.btnAdd.Location = new System.Drawing.Point(404, 25);
		   this.btnAdd.Name = "btnAdd";
		   this.btnAdd.Size = new System.Drawing.Size(75, 23);
		   this.btnAdd.TabIndex = 7;
		   this.btnAdd.Text = "Add";
		   this.btnAdd.UseVisualStyleBackColor = true;
		   this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
		   // 
		   // txtAttachments
		   // 
		   this.txtAttachments.Location = new System.Drawing.Point(25, 28);
		   this.txtAttachments.Name = "txtAttachments";
		   this.txtAttachments.Size = new System.Drawing.Size(363, 20);
		   this.txtAttachments.TabIndex = 6;
		   // 
		   // btnSend
		   // 
		   this.btnSend.Location = new System.Drawing.Point(417, 399);
		   this.btnSend.Name = "btnSend";
		   this.btnSend.Size = new System.Drawing.Size(75, 23);
		   this.btnSend.TabIndex = 3;
		   this.btnSend.Text = "Send";
		   this.btnSend.UseVisualStyleBackColor = true;
		   this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
		   // 
		   // btnCancel
		   // 
		   this.btnCancel.Location = new System.Drawing.Point(326, 399);
		   this.btnCancel.Name = "btnCancel";
		   this.btnCancel.Size = new System.Drawing.Size(75, 23);
		   this.btnCancel.TabIndex = 4;
		   this.btnCancel.Text = "Cancel";
		   this.btnCancel.UseVisualStyleBackColor = true;
		   this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
		   // 
		   // OFD
		   // 
		   this.OFD.FileName = "attachment";
		   this.OFD.Multiselect = true;
		   this.OFD.Title = "Add Attachment";
		   // 
		   // txtsendto
		   // 
		   this.txtsendto.Location = new System.Drawing.Point(70, 87);
		   this.txtsendto.Name = "txtsendto";
		   this.txtsendto.Size = new System.Drawing.Size(399, 20);
		   this.txtsendto.TabIndex = 7;
		   // 
		   // sendtolbl
		   // 
		   this.sendtolbl.AutoSize = true;
		   this.sendtolbl.Location = new System.Drawing.Point(22, 91);
		   this.sendtolbl.Name = "sendtolbl";
		   this.sendtolbl.Size = new System.Drawing.Size(23, 13);
		   this.sendtolbl.TabIndex = 6;
		   this.sendtolbl.Text = "To:";
		   // 
		   // Emailer
		   // 
		   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		   this.ClientSize = new System.Drawing.Size(509, 431);
		   this.Controls.Add(this.btnCancel);
		   this.Controls.Add(this.btnSend);
		   this.Controls.Add(this.groupBox3);
		   this.Controls.Add(this.groupBox2);
		   this.Controls.Add(this.groupBox1);
		   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
		   this.Name = "Emailer";
		   this.Text = "Send Email with Attachments";
		   this.groupBox1.ResumeLayout(false);
		   this.groupBox1.PerformLayout();
		   this.groupBox2.ResumeLayout(false);
		   this.groupBox2.PerformLayout();
		   this.groupBox3.ResumeLayout(false);
		   this.groupBox3.PerformLayout();
		   this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSubjectLine;
		private System.Windows.Forms.TextBox txtSendFrom;
        private System.Windows.Forms.Label lblSubjectLine;
		private System.Windows.Forms.Label lblSendFrom;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtAttachments;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog OFD;
		private System.Windows.Forms.TextBox txtsendto;
		private System.Windows.Forms.Label sendtolbl;
    }
}

