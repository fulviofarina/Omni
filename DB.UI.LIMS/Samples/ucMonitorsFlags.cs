﻿using System.Windows.Forms;
using Rsx.Dumb; using Rsx;
using DB.Tools;

namespace DB.UI
{
    public partial class ucMonitorsFlags : UserControl
    {
        public ucMonitorsFlags()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Dumb.FD(ref this.Linaa);
            Dumb.FD(ref this.BS);

            DGV.DataSource = inter.IBS.MonitorsFlags;

       //     Rsx.Dumb.BS.LinkBS(ref this.BS, this.Linaa.MonitorsFlags);
        }
    }
}