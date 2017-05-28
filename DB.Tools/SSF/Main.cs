using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;
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

       
        public void Calculate()
        {
            //actual position
            //  Cursor.Current = Cursors.WaitCursor;
            try
            {


                //   Interface.IBS.IsCalculating = true;

                Creator.SaveInFull(true);
                IsCalculating = true;

                resetProgress?.Invoke(3);

                int position = Interface.IBS.SubSamples.Position;
                // int count = SelectUnits();
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

                int count = units.Count;
                int numberofSteps = 5;

                resetProgress?.Invoke(2 + (count * numberofSteps));

                //1
                if (count == 0)
                {
                    MessageBox.Show(SELECT_SAMPLES, "Oops, nothing was selected!", MessageBoxButtons.OK);
                    IsCalculating = false;
                }
                else
                {
                    //loop through all samples to work to
                    foreach (UnitRow item in Units)
                    {
                        try
                        {
                            UnitRow UNIT = item;
                            //update position in BS
                            UNIT.Clean();

                            Interface.IBS.Update<UnitRow>(UNIT);

                            showProgress?.Invoke(null, EventArgs.Empty);

                            bool ok = CheckInputData(UNIT);

                            if (ok)
                            {
                                GenerateAnInput(ref UNIT);
                            }
                            showProgress?.Invoke(null, EventArgs.Empty);
                        }
                        catch (SystemException ex)
                        {
                            Interface.IStore.AddException(ex);
                            Interface.IReport.Msg(ex.Message, "ERROR", false);
                        }
                    }
                }
               
                 Interface.IBS.SubSamples.Position = position;

                 RunProcess();

               

                callBack?.Invoke(null, EventArgs.Empty);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, "ERROR", false);
            }
        }

        public void Set(ref Interface inter, EventHandler callBackMethod = null, Action<int> resetProg = null, EventHandler showProg = null)
        {
            Interface = inter;
            showProgress = showProg;
            resetProgress = resetProg;
            callBack = callBackMethod;

            if (processTable == null) processTable = new System.Collections.Hashtable();
        }

     

        public MatSSF()
        {
        }
    }
}