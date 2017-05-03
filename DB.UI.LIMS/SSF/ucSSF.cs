using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using VTools;
using Rsx;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSF : UserControl
    {

        private static Size currentSize;
    
        private bool cancelCalculations = false;
        private Interface Interface = null;
        private Action<int> resetProgress;
        private Action showProgress;


        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pro"></param>
        public void AttachCtrl<T>(ref T pro)
        {
            try
            {

                Control destiny = null;
                if (pro.GetType().Equals(typeof(ucProjectBox)))
                {
                    ucProjectBox projBox = pro as ucProjectBox;

                    projBox.HideChildControl = Hide;
                    destiny = this.splitContainer1.Panel1;
                    projBox.CallBack += delegate
                     {
                         bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
                         this.CalcBtn.Enabled = ThereIsData;
                         ucSSFData.Disabler(ThereIsData);
                       //  this.ucSSFData.Enabled = Interface.IBS.SubSamples.Count!=0;

                     };
                   
                }
                else if (pro.GetType().Equals(typeof(ucOptions)))
                {
                    IucOptions options = pro as IucOptions;
                    showProgress = options.ShowProgress;
                    resetProgress = options.ResetProgress;
                    //   projBox.HideChildControl = Hide;
                    destiny = this.splitContainer1.Panel2;
                }


                else if (pro.GetType().Equals(typeof(ucSubSamples)))
                {
                    Control p = pro as Control;
                    p.Dock = DockStyle.Fill;
                    destiny = this.SamplesTab;


                    //ucSubSamples c = p as ucSubSamples;
                    

                }
                else
                {
                    ucSSFData.AttachCtrl(ref pro);
                }

                destiny?.Controls.Add(pro as Control);

            }
            catch (Exception ex)
            {
                               Interface.IMain.AddException(ex);
            }

        }

        public void Calculate(object sender, EventArgs e)
        {

            Interface.IBS.EndEdit();
          //  this.ValidateChildren();

            Cursor.Current = Cursors.WaitCursor;

            cancelCalculations = false;

            resetProgress?.Invoke(3);

            this.CalcBtn.Enabled = false;

            this.cancelBtn.Enabled = true;

            this.Visible = true;
            this.ParentForm.Visible = true;

            this.Tab.SelectedTab = this.CalcTab;


            //actual position
                 int position = Interface.IBS.SubSamples.Position;

            MatSSF.StartupPath = Interface.IMain.FolderPath + Resources.SSFFolder;


            IList<UnitRow> units = null;
            bool shoulLoop = Interface.IPreferences.CurrentSSFPref.Loop;
            if (shoulLoop)
            {
                //take currents
                units = Interface.ICurrent.Units.OfType<LINAA.UnitRow>()
                    .Where(o => o.ToDo).ToList();
            }
            else
            {
                //take only current
                units = new List<UnitRow>();
                units.Add(Interface.ICurrent.Unit as UnitRow);
            }

            resetProgress?.Invoke(2 + (units.Count * 5));

            //1
            if (units.Count==0)
            {
                Interface.IReport.Msg("Select the Units to calculate with the 'Do?' checkbox", "Oops, nothing was selected");

            }
            //loop through all samples to work to
            foreach (UnitRow item in units)
            {

           
                //update position in BS
                Interface.IBS.Update<LINAA.UnitRow>(item);

                //   if (cancelCalculations) continue;
                ucSSFData.CalculateUnit(showProgress, ref cancelCalculations);


                string file = MatSSF.StartupPath + MatSSF.InputFile;
                Dumb.LoadFilesIntoBoxes(showProgress, ref inputbox, file);


                file = MatSSF.StartupPath + MatSSF.OutputFile;
                Dumb.LoadFilesIntoBoxes(showProgress, ref outputBox, file);


            }

            Interface.IBS.SubSamples.Position = position;


            this.CalcBtn.Enabled = true;

            cancelCalculations = false;

          //  Creator.SaveInFull(true);
      

            this.cancelBtn.Enabled = false;

            //    HideShowAll(true);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// A function to show hide this control and mimetize
        /// </summary>
        public new void Hide()
        {
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            DaCONTAINER.FixedPanel = FixedPanel.Panel1;
            bool hidden = DaCONTAINER.Panel2Collapsed;
            ParentForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            //show
            if (hidden)
            {
                SSFPlitter.Visible = true;
                this.Size = currentSize;
            
            }
            else  
            {
                this.Size = new Size((int)(DaCONTAINER.Panel1.Width * 0.66), DaCONTAINER.Panel1.Height);
                SSFPlitter.Visible = false;
            }

            DaCONTAINER.Panel2Collapsed = !hidden;
       

        }

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {

                MatSSF.Table = Interface.IDB.MatSSF;


                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;

                // OTHER CONTROLS
                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;

                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
           
                //link to bindings

                ucUnit.Set(ref Interface);
             

                ucSSFData.Set(ref Interface);

               

                //set calculation options
             //   Interface.IBS.EndEdit();

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IMain.AddException(ex);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            cancelCalculations = true;
         //   this.cancelBtn.Enabled = false;
        }

        public ucSSF()
        {
            InitializeComponent();

            currentSize = this.Size;
            this.cancelBtn.Enabled = false;

        }

       
    }
}