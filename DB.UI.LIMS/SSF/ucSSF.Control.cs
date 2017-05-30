using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFData : UserControl
    {
      
        private Interface Interface = null;
        BindingNavigator bn = null;
        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pro"></param>
        public void AttachCtrl<T>(ref T pro)
        {
            Control destiny = null;
            if (pro.GetType().Equals(typeof(Msn.Pop)))
            {
                (pro as Msn.Pop).Dock = DockStyle.Fill;
                destiny = this.UnitSSFSC.Panel2;
            }
            else if (pro.GetType().Equals(typeof(BindingNavigator)))
            {

            //    BindingNavigator bn = new BindingNavigator(Interface.IBS.SubSamples);
              //  pro = (T)Convert.ChangeType(bn,pro.GetType());
                
                bn = pro as BindingNavigator;
                bn.Items["SaveItem"].Visible = false;
                bn.Parent.Controls.Remove(bn);
             
                // this.unitBN.Dispose();
                destiny = this.unitSC.Panel2;
            }
            else if (pro.GetType().Equals(typeof(ucPreferences)))
            {
                ucNS.AttachCtrl(ref pro);
            }
            destiny?.Controls.Add(pro as Control);
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
                ucNS.Set(ref inter);
                ucSubMS.Set(ref inter,true);


                this.sampleCompoLbl.Click += viewChanged;
                this.imgBtn.Click += viewChanged;
              


                this.SampleLBL.Click += dropDownClickedLabel;
                this.descriplbl.Click += dropDownClickedLabel;


                Interface.IBS.PropertyChangedHandler += enableControls;
                Interface.IBS.EnabledControls = true;
            //    enableControls(null, new PropertyChangedEventArgs(string.Empty));

                sampleCompoLbl.PerformClick();

                //    Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void enableControls(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //turns off or disables the controls.
            //necessary protection for user interface
            bool enable = Interface.IBS.SubSamples.Count != 0;//EnabledControls;
            ucSubMS.Enabled = enable;
            ucNS.Enabled = enable;
        //    bool bnOk = enable && 
            nameToolStrip.Enabled = enable;
            descripTS.Enabled = enable;
          //  changeViewTS.Enabled = enable;

        }

        private void setSampleBindings()
        {

            BindingSource bsSample = Interface.IBS.SubSamples;
            SubSamplesDataTable SSamples = Interface.IDB.SubSamples;

            ToolStripComboBox namebox = this.nameB;
            ToolStripComboBox descriptionbox = this.descripBox;

            string column;
            column = SSamples.SubSampleNameColumn.ColumnName;
            Binding b1 = Rsx.Dumb.BS.ABinding(ref bsSample, column);
            nameB.ComboBox.DataBindings.Add(b1);

         //   BS.BindAComboBox(ref bsSample, ref namebox, column);

            column = SSamples.SubSampleDescriptionColumn.ColumnName;
            Binding b0 = Rsx.Dumb.BS.ABinding(ref bsSample, column);
            descripBox.ComboBox.DataBindings.Add(b0);

        //    BS.BindAComboBox(ref bsSample, ref descripBox, column);

        }

       



        // CheckBox check = new CheckBox();
        public ucSSFData()
        {
            InitializeComponent();

            this.sampleCompoLbl.Text = "SWITCH VIEW";
            
        }

        TabPage on;
        public TabPage On
        {

            set
            {
                on = value;
            }
        }
        TabPage off;
        public TabPage Off
        {

            set
            {
                off = value;
            }
        }


        private void dropDownClickedLabel(object sender, EventArgs e)
        {

            ToolStripLabel lbl = sender as ToolStripLabel;
            ToolStripComboBox cbox = null;
            //    if (lbl.Equals())
            string[] items = Interface.ICurrent.SubSamples
               .OfType<SubSamplesRow>()
               .Where(o => !o.IsSubSampleNameNull())
               .Select(o=> o.SubSampleName)
               .ToArray();

            if (lbl.Equals(this.SampleLBL))
            {
                cbox = this.nameB;
            }
            else
            {
                cbox = this.descripBox;
                items = Interface.ICurrent.SubSamples
               .OfType<SubSamplesRow>()
                      .Where(o => !o.IsSubSampleDescriptionNull())
               .Select(o => o.SubSampleDescription)
               .ToArray();
            }

            cbox.Items.Clear();

            if (!cbox.DroppedDown && items.Count()!=0 )
            {
                cbox.AutoCompleteMode = AutoCompleteMode.Suggest;
                cbox.AutoCompleteSource = AutoCompleteSource.ListItems;
                cbox.Items.AddRange(items);
            }

            cbox.DroppedDown = !cbox.IsOnDropDown;
      
        }

        protected bool assigned = false;
         protected string compo = "SWITCH";
        protected string geom = "CHANGE";


        public void ViewChanged(object sender, EventArgs e)
        {
            
            bool.TryParse(sender.ToString(), out assigned);

            TabPage page = setLabelView(compo, geom);

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
            Color clr = Color.OrangeRed;
            if (sampleCompoLbl.Text.Contains(compo))
            {
                sampleCompoLbl.Text = sampleCompoLbl.Text.Replace(compo, geom);
            }

            else
            {
                sampleCompoLbl.Text = sampleCompoLbl.Text.Replace(geom, compo);
                img = Properties.Resources.Matrices;
                clr = Color.Fuchsia;
                page = off;

            }
            sampleCompoLbl.VisitedLinkColor = clr;
            sampleCompoLbl.LinkColor = clr;

            this.sampleCompoLbl.Visible = true;
            this.imgBtn.Image = img;
            return page;
        }
    }
}