using System;
using System.Collections;
using System.Windows.Forms;

namespace VTools
{
    /// <summary>
    /// Test Application Form: This application is used to test sending email and email with attachments.
    /// </summary>
    public partial class Emailer : Form
    {
        /// <summary>
        /// An arraylist containing all of the attachments
        /// </summary>
        private ArrayList alAttachments;

        /// <summary>
        /// Default constructor
        /// </summary>

        public Emailer(string From)
        {
            InitializeComponent();
            this.txtSendFrom.Text = From;
            this.txtSendFrom.ReadOnly = true;
        }

        /// <summary>
        /// Add files to be attached to the email message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] arr = OFD.FileNames;
                    alAttachments = new ArrayList();
                    txtAttachments.Text = string.Empty;
                    alAttachments.AddRange(arr);

                    foreach (string s in alAttachments)
                    {
                        txtAttachments.Text += s + ";";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Send an email message with or without attachments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSendFrom.Text))
            {
                MessageBox.Show("Missing sender address.", "Email Error");
                return;
            }

            if (String.IsNullOrEmpty(txtSubjectLine.Text))
            {
                MessageBox.Show("Missing subject line.", "Email Error");
                return;
            }

            if (String.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Missing message.", "Email Error");
                return;
            }

            string[] arr = txtAttachments.Text.Split(';');
            alAttachments = new ArrayList();
            for (int i = 0; i < arr.Length; i++)
            {
                if (!String.IsNullOrEmpty(arr[i].ToString().Trim()))
                {
                    alAttachments.Add(arr[i].ToString().Trim());
                }
            }

            // if there are attachments, send message with SendMessageWithAttachment call, else use
            // the standard SendMessage call
            if (alAttachments.Count > 0)
            {
                string result = Rsx.Emailer.SendMessageWithAttachment(txtSendFrom.Text, txtSubjectLine.Text, txtMessage.Text, alAttachments, txtsendto.Text);

                MessageBox.Show(result, "Email Sent!");
            }
            else
            {
                string result = Rsx.Emailer.SendMessage(txtSendFrom.Text, txtSubjectLine.Text, txtMessage.Text, txtsendto.Text);

                MessageBox.Show(result, "Email Sent!");
            }
        }
    }
}