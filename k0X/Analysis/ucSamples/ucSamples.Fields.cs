using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Drawing;

using System.Windows.Forms;
using DB;

//using DB.Interfaces;
using DB.Tools;
using DB.UI;
using Rsx.Dumb; using Rsx;

namespace k0X
{
    public partial class ucSamples
    {
        public LINAA.ProjectsRow ProjectsRow = null;

        public ucOrder ucOrder;
        public Interface Interface = null;
        private bool isClone = false;
        public ucSubSamples ISubS = null;
        protected IWC iW;
        protected string pathCode = string.Empty;
        public IPeriodicTable pTable = null;
        public IEnumerable<LINAA.SubSamplesRow> samples = null;
        protected bool Waiting = false;

        public bool IsClone
        {
            get
            {
                return isClone;
            }
            set
            {
                isClone = value;
                pathCode = this.Name;
                if (isClone) pathCode += ".Clone";
            }
        }

        public IWC W
        {
            get
            {
                try
                {
                    if (iW == null)
                    {
                        LINAA Linaa = Interface.Get() as LINAA;
                        iW = new WC(this.Name, ref this.progress, ref this.Cancel, ref Linaa);
                        iW.SetExternalMethods(TV.CheckNode, this.Finished);
                        TreeView tv = TV;
                        iW.SetNodes(ref tv); //link nodes
                    }
                    iW.SetPeakSearch(minAreabox.Text, maxUncbox.Text, Awindowbox.Text, Bwindowbox.Text);
                    iW.SetOverriders(fbox.Text, alphabox.Text, Gtbox.Text, Geobox.Text, asSamplebox.Checked);
                    iW.ShowSolang = showSolang.Checked;
                    iW.ShowSSF = showMATSSF.Checked;
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                }
                return iW;
            }
        }
    }
}