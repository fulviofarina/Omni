using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFPanel : UserControl
    {
        protected  bool assigned = false;
        protected  static string compo = "SWITCH VIEW";
        protected static string geom = "CHANGE VIEW";
        protected internal   Interface Interface= null;
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
                destiny = this.UnitSSFSC.Panel2;
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
        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {


                setSampleBindings();

           
                this.sampleCompoLbl.Click += viewChanged;
                this.imgBtn.Click += viewChanged;

                LIMS.SetSampleBox(ref this.ucGenericCBox1);
                LIMS.SetSampleDescriptionBox(ref this.descripTS);

                viewChanged(null, EventArgs.Empty);//.PerformClick();

                Interface.IBS.PropertyChangedHandler += delegate
                {
                    //turns off or disables the controls.
                    //necessary protection for user interface
                    bool enable = Interface.IBS.SubSamples.Count != 0;//EnabledControls;
                    ucSubMS.Enabled = enable;
                    ucDataContent.Enabled = enable;
                    // bool bnOk = enable &&
                    ucGenericCBox1.Enabled = enable;
                    descripTS.Enabled = enable;
                };

                //refresh?
                Interface.IBS.EnabledControls = true;
          
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

     

     


        public void ViewChanged(object sender, EventArgs e)
        {
            bool.TryParse(sender.ToString(), out assigned);

            TabPage page = setLabelView(compo, geom);
        }

       

        private TabPage setLabelView(string compo, string geom)
        {
            TwoSectionSC.Visible = false;

            TwoSectionSC.Panel1Collapsed = assigned;
            TwoSectionSC.Panel2Collapsed = !assigned;

            TwoSectionSC.Visible = true;
            assigned = !assigned;

            TabPage page = on;

            this.sampleCompoLbl.Visible = false;
            Image img = Properties.Resources.Geometries;
            Color clr = Color.GhostWhite;
            if (sampleCompoLbl.Text.Contains(compo))
            {
                sampleCompoLbl.Text = geom;
            }
            else
            {
                sampleCompoLbl.Text = compo;
                img = Properties.Resources.Matrices;
                clr = Color.WhiteSmoke;
                page = off;
            }
            sampleCompoLbl.VisitedLinkColor = clr;
            sampleCompoLbl.LinkColor = clr;

            this.sampleCompoLbl.Visible = true;
            this.imgBtn.Image = img;
            return page;
        }

        private void setSampleBindings()
        {
            BindingSource bsSample = Interface.IBS.SubSamples;
            SubSamplesDataTable SSamples = Interface.IDB.SubSamples;


            string column;
            column = SSamples.SubSampleNameColumn.ColumnName;
            ucGenericCBox1.BindingField = column;
            ucGenericCBox1.SetBindingSource(ref bsSample);

            column = SSamples.SubSampleDescriptionColumn.ColumnName;
            descripTS.BindingField = column;
            descripTS.SetBindingSource(ref bsSample);


            ucDataContent.Set(ref Interface);
            ucSubMS.Set(ref Interface, true);



        }


        private void viewChanged(object sender, EventArgs e)
        {
            TabPage page = setLabelView(compo, geom);
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

            this.sampleCompoLbl.Text = "SWITCH VIEW";
        }
    }
}