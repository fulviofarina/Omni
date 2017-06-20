using System;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFPanel : UserControl
    {
        protected bool _assigned = false;
        protected static string VIEW_COMPOSITION = "Composition View";
        protected static string VIEW_GEOMETRY = "Geometry View";
        protected internal Interface Interface = null;
        protected internal TabPage off;
        protected internal BindingNavigator bn;

        protected internal TabPage on;

        public TabPage Off
        {
            set
            {
                off = value;
            }
        }

        public TabPage On
        {
            set
            {
                on = value;
            }
        }

        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pro"></param>
        public void AttachCtrl<T>(ref T pro)
        {
            Control destiny = null;
            Type t = pro.GetType();
            if (t.Equals(typeof(Msn.Pop)))
            {
                (pro as Msn.Pop).Dock = DockStyle.Fill;
                destiny = this.splitContainer1.Panel1;
                this.splitContainer1.Panel2Collapsed = true;
            }
            else if (t.Equals(typeof(BindingNavigator)))
            {
                // BindingNavigator bn = new BindingNavigator(Interface.IBS.SubSamples); pro = (T)Convert.ChangeType(bn,pro.GetType());

                bn = pro as BindingNavigator;
                bn.Items["SaveItem"].Visible = false;
                bn.Parent.Controls.Remove(bn);

                // this.unitBN.Dispose();
                destiny = this.unitSC.Panel2;
            }

            destiny?.Controls.Add(pro as Control);
        }

        public void SetMessage(string msg)
        {
            infoLBL.Text = msg;
        }


        public EventHandler RefreshChart()
        {

            return delegate
            {
                chart1.DataSource = Interface.IBS.SSF;
                chart1.Series.Invalidate();
                chart1.Update();
                chart1.Invalidate();
            };
        }
        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {

          


                setSampleBindings();

                this._sampleCompoLbl.Click += viewChanged;
                this._imgBtn.Click += viewChanged;

                LIMS.SetSampleBox(ref this._ucSampleBox);
                LIMS.SetSampleDescriptionBox(ref this._ucSmpDescriptionBox);

                viewChanged(null, EventArgs.Empty);//.PerformClick();

                Interface.IBS.PropertyChangedHandler += delegate
                {
                    //turns off or disables the controls.
                    //necessary protection for user interface
                    bool enable = Interface.IBS.SubSamples.Count != 0;//EnabledControls;
                    _ucSubMS.Enabled = enable;
                    _ucDataContent.Enabled = enable;
                    // bool bnOk = enable &&
                    _ucSampleBox.Enabled = enable;
                    _ucSmpDescriptionBox.Enabled = enable;
                };

                //refresh?
                //   Interface.IBS.EnabledControls = true;
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        public void ViewChanged(object sender, EventArgs e)
        {
            bool.TryParse(sender.ToString(), out _assigned);
            changeView();
            setLabelView();
        }

        /// <summary>
        /// Looks good
        /// </summary>
        private void setLabelView()
        {
            // TabPage page = on;
            _sampleCompoLbl.Visible = false;
            Image img = Properties.Resources.Geometries;
            Color clr = Color.GhostWhite;
            if (_TwoSectionSC.Panel2Collapsed)
            {
                _sampleCompoLbl.Text = VIEW_GEOMETRY;
            }
            else
            {
                _sampleCompoLbl.Text = VIEW_COMPOSITION;
                img = Properties.Resources.Matrices;
                clr = Color.Lavender;
                // page = off;
            }
            _sampleCompoLbl.VisitedLinkColor = clr;
            _sampleCompoLbl.LinkColor = clr;
            _sampleCompoLbl.Visible = true;
            _imgBtn.Image = img;
            // return page;
        }

        private void changeView()
        {
            //looks good
            _TwoSectionSC.Visible = false;
            _TwoSectionSC.Panel1Collapsed = _assigned;
            _TwoSectionSC.Panel2Collapsed = !_assigned;
            _TwoSectionSC.Visible = true;
            _assigned = !_assigned;
        }

        private TabPage setTabPage()
        {
            //looks good
            TabPage page = on;
            if (!_TwoSectionSC.Panel2Collapsed)
            {
                page = off;
            }

            return page;
        }

        private void setSampleBindings()
        {
            BindingSource bsSample = Interface.IBS.SubSamples;
            SubSamplesDataTable SSamples = Interface.IDB.SubSamples;

            string column;
            column = SSamples.SubSampleNameColumn.ColumnName;
            _ucSampleBox.BindingField = column;
            _ucSampleBox.SetBindingSource(ref bsSample,true);

            column = SSamples.SubSampleDescriptionColumn.ColumnName;
            _ucSmpDescriptionBox.BindingField = column;
            _ucSmpDescriptionBox.SetBindingSource(ref bsSample,true);

            _ucDataContent.Set(ref Interface);
            _ucSubMS.Set(ref Interface, true);
        }

        private void viewChanged(object sender, EventArgs e)
        {
            this.changeView();
            this.setLabelView();
            TabPage page = setTabPage();
            TabControl ctrl = (page.Parent as TabControl);

            ctrl.Visible = false;
            ctrl.SelectedTab = page;
            if (ctrl.Parent.GetType().Equals(typeof(TabPage)))
            {
                page = (ctrl.Parent as TabPage);
                (page.Parent as TabControl).SelectedTab = page;//.Show();//Focus();
            }
            ctrl.Visible = true;
        }

        // CheckBox check = new CheckBox();
        public ucSSFPanel()
        {
            InitializeComponent();

            this._sampleCompoLbl.Text = VIEW_COMPOSITION;
        }
    }
}