using DB.Tools;
using Rsx.Dumb;
using System;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucSamples : UserControl
    {
        public ucSamples()
        {
            InitializeComponent();
        }

        private Interface Interface;

        public void Set(ref Interface inter)
        {
            Interface = inter;
            Dumb.FD(ref this.Linaa);
            Dumb.FD(ref BS);

            DGV.DataSource = Interface.IBS.Samples;

            // Dumb.BS.LinkBS(ref this.BS, this.Linaa.Samples);

            UIControl.FillABox(orderbox, Interface.IPopulate.IOrders.OrdersList, true, false);
        }

        private void orderbox_TextChanged(object sender, EventArgs e)
        {
            if (orderbox.AutoCompleteCustomSource.Contains(orderbox.Text.ToUpper()))
            {
                try
                {
                    Int32? ordId = Interface.IPopulate.IOrders.FindOrderID(orderbox.Text.ToUpper());
                    if (ordId != null)
                    {
                        this.TA.FillByOrderID(Interface.IDB.Samples, ordId);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    // MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
                }
            }
        }
    }
}