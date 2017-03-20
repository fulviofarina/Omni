using System;
using System.Windows.Forms;
using Rsx;

namespace DB.UI
{
    public partial class ucSamples : UserControl
    {
        public ucSamples()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();

            Dumb.LinkBS(ref this.BS, this.Linaa.Samples);

            Dumb.FillABox(orderbox, this.Linaa.OrdersList, true, false);
        }

        private void orderbox_TextChanged(object sender, EventArgs e)
        {
            if (orderbox.AutoCompleteCustomSource.Contains(orderbox.Text.ToUpper()))
            {
                try
                {
                    Int32? ordId = this.Linaa.FindOrderID(orderbox.Text.ToUpper());
                    if (ordId != null)
                    {
                        this.TA.FillByOrderID(this.Linaa.Samples, ordId);
                    }
                }
                catch (SystemException ex)
                {
                    this.Linaa.AddException(ex);
                    MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
                }
            }
        }
    }
}