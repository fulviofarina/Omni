using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        /// <summary>
        /// Types of channels configurations
        /// </summary>
        public static string[] Types = new string[] { "0 = Isotropic", "1 = Wire flat", "2 = Foil/wire ch. axis" };

        /// <summary>
        /// This is the main row with the data
        /// </summary>

        /// <summary>
        /// The MatSSF startup path
        /// </summary>
        public static string StartupPath
        {
            get { return startupPath; }
            set { startupPath = value; }
        }

        /// <summary>
        /// The input MatSSF file
        /// </summary>

        /// <summary>
        /// The output MatSSF file
        /// </summary>

        public IList<UnitRow> Units
        {
            get
            {
                return units;
            }
        }

        public void Calculate(bool background = false)
        {

            bkgCalculation = background;

          
            //actual position
            //  Cursor.Current = Cursors.WaitCursor;
            try
            {
                // Interface.IBS.IsCalculating = true;

               if (!bkgCalculation) Creator.SaveInFull(true);
             

                resetProgress?.Invoke(3);

                int position = Interface.IBS.SubSamples.Position;
                // int count = SelectUnits();
                SelectSamples(background);
           
                //1
                if (units.Count == 0)
                {
                    if (!bkgCalculation)
                    {
                        MessageBox.Show(SELECT_SAMPLES, "Oops, nothing was selected!", MessageBoxButtons.OK);
                    }
                 //   IsCalculating = false;
                }
                else
                {
                    //loop through all samples to work to
                    int numberofSteps = 5;

                    resetProgress?.Invoke(2 + (units.Count * numberofSteps));


                    PrepareInputs(background);

                    if (!background) Interface.IBS.SubSamples.Position = position;

                    RunProcess();

                }

                callBack?.Invoke(null, EventArgs.Empty);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, "ERROR", false);
            }
        }

        /// <summary>
        /// Prepares de Input Files
        /// </summary>
     
        public void SelectSamples(bool background = false)
        {
            //should loop all?
            bool shoulLoop = Interface.IPreferences.CurrentSSFPref.Loop;

//if batch evaluation or backround bacth work
            if (shoulLoop || background)
            {
                //take ALL SAMPLES (BACKROUND RUN)
                if (background)
                {
                    units = Interface.IDB.Unit.OfType<UnitRow>()
                        .ToList(); 
                }
                //take the ones in the binding source
                else
                {
                    units = Interface.ICurrent.Units.OfType<LINAA.UnitRow>()
                        .ToList();
                }
            }
            else //no loop, take current
            {
                //take only current (binding source)
                units = new List<UnitRow>();
                UnitRow u = Interface.ICurrent.Unit as UnitRow;
                units.Add(u);
            }
            //filter now by non busy and TODO
            units = units.Where(o => o.ToDo && !o.IsBusy).ToList();

        }

        public void Set(ref Interface inter, EventHandler callBackMethod = null, Action<int> resetProg = null, EventHandler showProg = null)
        {
            Interface = inter;
            showProgress = delegate
            {
                if (!bkgCalculation)
                {
                    showProg?.Invoke(null,EventArgs.Empty);
                }
            };
            resetProgress = delegate (int i)
            {
                if (!bkgCalculation)
                {
                    resetProg?.Invoke(i);
                }
            };
            callBack = callBackMethod;

            if (processTable == null) processTable = new System.Collections.Hashtable();
        }

        public MatSSF()
        {
        }
    }
}