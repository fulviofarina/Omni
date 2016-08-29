using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;

namespace k0X
{
    public interface IPeriodicTable
    {
        void NewForm();

        void Query(ref LINAA.SubSamplesRow toquery);

        bool IsDisposed { get; }
        k0X.IPeaks IPeaks { get; set; }

        void Link();

        void DeLink();

        IEnumerable<string> Projects { set; }
    }

    public partial class ucPeriodicTable : UserControl, IPeriodicTable
    {
        private IEnumerable<LINAA.ElementsRow> Elements;

        private IPeaks ucpeaks = null;

        public IPeaks IPeaks
        {
            get { return ucpeaks; }
            set { ucpeaks = value; }
        }

        public void Link()
        {
            if (ucpeaks == null) return;
            this.ucpeaks.Link();
        }

        public void DeLink()
        {
            if (ucpeaks == null) return;
            this.ucpeaks.DeLink();
        }

        private IList<string> elements = null;

        public IEnumerable<string> Projects
        {
            set
            {
                IEnumerable<string> projects = null;
                projects = value;
                Dumb.FillABox(projectbox, projects.ToList(), true, true);
            }
        }

        public ucPeriodicTable(ref IEnumerable<LINAA.ElementsRow> elements)
        {
            InitializeComponent();

            this.SuspendLayout();

            try
            {
                Elements = elements.ToList();
                foreach (LINAA.ElementsRow e in Elements)
                {
                    ucElement element = new ucElement(e, this);
                    element.MouseClick += this.Element_MouseClick;
                    TLP.Controls.Add(element, e.Col - 1, e.Row - 1);
                }
            }
            catch (SystemException ex)
            {
            }
            this.ResumeLayout(false);  //important
        }

        private void projectbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Error.SetError(projectbox.Control, null);
                string _project = this.projectbox.Text.ToUpper().Trim();

                if (this.projectbox.AutoCompleteCustomSource.Contains(_project))
                {
                    this.samplebox.Enabled = false;
                    this.Symbox.Enabled = false;

                    ICollection<string> hssamples = this.ucpeaks.SetProject(_project);
                    //keep here, not after

                    this.samplebox.Enabled = true;
                    this.Symbox.Enabled = true;

                    if (hssamples.Count != 0)
                    {
                        Dumb.FillABox(this.samplebox, hssamples, true, true);
                        if (hssamples.Count == 1) samplebox.Text = hssamples.First();
                    }
                }
            }
            catch (SystemException ex)
            {
                this.Error.SetError(projectbox.Control, ex.Message);
            }
        }

        private void samplebox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.samplebox.Text.Equals(string.Empty) || !samplebox.Enabled) return;
                this.Error.SetError(samplebox.Control, null);
                string sample = samplebox.Text.ToUpper().Trim();
                if (this.samplebox.AutoCompleteCustomSource.Contains(sample))
                {
                    this.Symbox.Enabled = false;
                    this.ucpeaks.SetSample(sample);
                    IList<string> elements = this.ucpeaks.FindElements();

                    this.Symbox.Enabled = true;

                    if (elements.Count != 0)
                    {
                        Dumb.FillABox(this.Symbox, elements, true, true);
                        if (!SC.Panel1Collapsed) this.PopulateFor(ref elements, true);
                        string ele = string.Empty;
                        //pick first element or ALL
                        if (elements.Count == 1) ele = elements.First();
                        else ele = "*";
                        //force reloading of symbox
                        if (ele.CompareTo(Symbox.Text) == 0) this.Symbox.Text = string.Empty;
                        this.Symbox.Text = ele;
                    }
                }
                else ucpeaks.CleanSR();
            }
            catch (SystemException ex)
            {
                this.Error.SetError(projectbox.Control, ex.Message);
            }
        }

        private void Symbox_TextChanged(object sender, EventArgs e)
        {
            bool fill = false;

            try
            {
                if (Symbox.Text.Equals(string.Empty) || !Symbox.Enabled) return;

                this.Error.SetError(Symbox.Control, null);
                string sym = Symbox.Text.Trim();

                if (Symbox.AutoCompleteCustomSource.Contains(sym))
                {
                    this.ucpeaks.SetElement(sym);
                    fill = true;
                }
                else ucpeaks.CleanSR();
            }
            catch (SystemException ex)
            {
                this.Error.SetError(projectbox.Control, ex.Message);
            }

            if (fill) Fill_Click(sender, e);
        }

        /// <summary>
        /// Populates the Periodic Table for the given list of elements (HashSet) under the respective module
        /// </summary>
        /// <param name="hs">list of elements toRow populate in the table</param>
        private void PopulateFor(ref IList<string> hs, bool autoclick)
        {
            //autoclick is to select the element automatically
            //when loading this is ok, but when just hiding/viewing the table is not necessary
            if (hs == null) return;
            IList<string> aux = hs.ToList();

            IEnumerable<LINAA.ElementsRow> toEnable = Elements.Where(o => aux.Contains(o.Element));
            IEnumerable<LINAA.ElementsRow> toDisable = Elements.Except(toEnable);

            this.SuspendLayout();

            foreach (LINAA.ElementsRow e in toDisable)
            {
                ucElement element = (ucElement)e.ucControl;
                element.Enabled = false;
                element.PaintElement();
            }
            foreach (LINAA.ElementsRow e in toEnable)
            {
                ucElement element = (ucElement)e.ucControl;
                element.Enabled = true;
                element.PaintElement();
            }

            this.ResumeLayout(false);
        }

        public void NewForm()
        {
            AuxiliarForm form = new AuxiliarForm();
            UserControl control = this;
            form.Populate(ref control);

            form.AutoSize = true;
            form.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            form.Text = "Selector";
            form.Show();

            Screen screen = Screen.PrimaryScreen;
            if (Screen.AllScreens.Length == 2)
            {
                if (Screen.AllScreens[0].Primary) screen = Screen.AllScreens[0];
                else screen = Screen.AllScreens[1];
            }
            System.Drawing.Rectangle area = screen.WorkingArea;
            form.Location = new System.Drawing.Point(area.X + 100, area.Y + 10);
        }

        public void Query(ref LINAA.SubSamplesRow toquery)
        {
            if (ucpeaks == null) return;

            //when the parent_node (SubSample) is the selected node then just refresh...
            if (ucpeaks.Sample != null && ucpeaks.Sample.Equals(toquery))
            {
                this.Fill_Click(null, EventArgs.Empty);
            }
            else
            {
                projectbox.Text = toquery.IrradiationCode.ToUpper().Trim();
                samplebox.Text = toquery.SubSampleName.ToUpper().Trim();
            }
        }

        private void ViewTable_Click(object sender, EventArgs e)
        {
            this.TLP.SuspendLayout();
            if (this.ViewTable.Text.Contains("View"))
            {
                this.ViewTable.Text = "Hide";
                this.ParentForm.MinimizeBox = true;
                this.AutoSize = false;
                SC.Panel1Collapsed = false;
                if (elements.Count() != 0) PopulateFor(ref elements, false);
            }
            else
            {
                this.ViewTable.Text = "View";
                this.ParentForm.MinimizeBox = false;
                this.AutoSize = true;
                SC.Panel1Collapsed = true;
            }
            this.TLP.ResumeLayout();
        }

        private void Fill_Click(object sender, EventArgs e)
        {
            this.ucpeaks.LoadData();
        }

        private void Element_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.Enabled) return;
            ucElement uc = sender as ucElement;

            if (e.Button == MouseButtons.Left)
            {
                Symbox.Text = string.Empty;
                Symbox.Text = uc.ElementRow.Element;
                Symbox.Tag = uc.ElementRow;
                ParentForm.Icon = (System.Drawing.Icon)Properties.PT.ResourceManager.GetObject(uc.ElementRow.ElementNameEn);

                uc.Refresh();
            }
            else Dumb.NavigateTo("Element Information for " + uc.ElementRow.ElementNameEn, DB.Tools.NuDat.GetElementUrl(uc.ElementRow.ElementNameEn));
        }
    }
}