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
using Rsx;


namespace k0X
{
    public partial class ucSamples
    {
        protected bool Waiting = false;
        protected string pathCode = string.Empty;
        protected bool isClone = false;
        public bool IsClone
        {
            get { return isClone; }
            set
            {
                isClone = value;

                pathCode = this.Name;
                if (isClone) pathCode += ".Clone";
            }
        }
        protected Interface Interface = null;
        protected IPeriodicTable pTable = null;
        public ucOrder ucOrder;
        protected ISubSamples iSubS = null;

        public ISubSamples ISubS
        {
            get { return iSubS; }
            set { iSubS = value; }
        }
        public LINAA.ProjectsRow ProjectsRow = null;
        protected IEnumerable<LINAA.SubSamplesRow> samples = null;
        protected IWC iW;
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
                        iW.SetExternalMethods(this.CheckNode, this.Finished);
                        iW.SetNodes(ref TV); //link nodes
                    }
                    iW.SetPeakSearch(minAreabox.Text, maxUncbox.Text, Awindowbox.Text, Bwindowbox.Text);
                    iW.SetOverriders(fbox.Text, alphabox.Text, Gtbox.Text, Geobox.Text, asSamplebox.Checked);
                    iW.ShowSolang = showSolang.Checked;
                    iW.ShowSSF = showMATSSF.Checked;
                }
                catch (SystemException ex)
                {
                    Interface.IReport.AddException(ex);
                }
                return iW;
            }
        }



    }
}
