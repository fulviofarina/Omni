using System;
using System.Windows.Forms;

namespace DB
{
    public partial class Msn : Form
    {
        public Msn()
        {
            InitializeComponent();

            this.Text = string.Empty;

            this.title.Text = string.Empty;
            this.textBoxDescription.Text = string.Empty;

            n = new Timer();

            n.Tick += n_Tick;
            n.Interval = 4000;

            this.textBoxDescription.TextChanged += textBoxDescription_TextChanged;
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                n.Stop();
                n.Start();
            }
            else this.Visible = true;
        }

        private Timer n;

        public void Msg(string msg, string title)
        {
            this.textBoxDescription.Text = msg;
            this.title.Text = title;
        }

        private void n_Tick(object sender, EventArgs e)
        {
            n.Stop();
            if (this.Visible) this.Visible = false;
        }
    }
}