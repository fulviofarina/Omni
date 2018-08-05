using Rsx.Dumb;
using System;
using System.Collections;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BS
    {
        /// <summary>
        /// Selects a Unit Child Row or the Unit Row itself and assigns the respective Parent
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public void AssignUnitChild<T>(ref T row)
        {
            try
            {
                if (row == null) return;

                //NO UNIT, maybe a matrix, a vial, a rabbit or a channel
                UnitRow unit = Interface.ICurrent.Unit as UnitRow;

                string title = SAMPLE;// + unit.Name;
                title += unit.Name;

                //set parent
                unit.SetParent(ref row);
                //report
                Interface.IReport.Msg(title + UPDATED_ROW, UPDATED); //report
                //stuff update and chek?
                CurrentChanged<UnitRow>(unit, true, false, true);

                IRow ir = unit as IRow;
                ir.Check();

                Interface.IStore.Save(ref unit);
                SubSamplesRow s = unit.SubSamplesRow;
                Interface.IStore.Save(ref s);

                //do I need this? yes!
                CurrentChanged<SubSamplesRow>(s, false, false, true);
            }
            catch (System.Exception ex)
            {
                // Interface.IReport.Msg(ex.StackTrace, ex.Message);
                Interface.IStore.AddException(ex);
            }
        }

        /*
        public UnitRow SelectUnit<T>(ref T row)
        {
            UnitRow unit;
            string title = SAMPLE;// + unit.Name;
            unit = row as UnitRow;
            title += unit.Name;
            unit.ToDo = !unit.ToDo;
            Interface.IReport.Msg(title + SELECTED_ROW, SELECTED); //report
            return unit;
        }
        */

        public void ResetBidings(bool v)
        {
            foreach (BindingSource bs in bindings.Values)
            {
                bs.ResetBindings(v);
            }
        }

        public bool EnabledControls
        {
            get
            {
                return enabledControls;
            }
            set
            {
                enabledControls = value;

                notifyPropertyChanged(ENABLE_CONTROLS_FIELD);
            }
        }

        public bool IsCalculating
        {
            get
            {
                return isCalculating;
            }

            set
            {
                isCalculating = value;
                if (isCalculating)
                {
                    EndEdit();
                    // EnabledControls = false;
                }
                //DO I NEED TO PUT ENABLED TRUE??
                //    EnabledControls = !value;// else { EnabledControls = true; }
            }
        }

        public void ApplyFilters()
        {
            try
            {
                resetPreferencesFilters();

                resetGeometryFilters();

                resetSampleFilters();

                resetMeasurementFilters();
                resetPeaksFilters();

                resetIrradiationFilters();

                resetMatrixFilters();
            }
            catch (Exception ex)
            {
                // Interface.IReport.Msg(ex.StackTrace, ex.Message);
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Checks a row for serious errors according to the NonNullable columns or specialized array
        /// of forbidden columns
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>

        /// <summary>
        /// EndEdit for each binding source
        /// </summary>
        public void EndEdit()
        {
            foreach (BindingSource b in bindings?.Values)
            {
                try
                {
                    b?.EndEdit();
                }
                catch (Exception ex)
                {
                    Interface.IStore.AddException(ex);
                }
            }
        }

        public void EnableRaisingList(bool enable)
        {
            foreach (BindingSource bs in bindings.Values)
            {
                bs.RaiseListChangedEvents = enable;
            }
        }

        public void ResumeBindings()
        {
            foreach (BindingSource b in bindings.Values)
            {
                try
                {
                    b.ResumeBinding();
                }
                catch (Exception ex)
                {
                    Interface.IStore.AddException(ex);
                }
            }
        }

        public void SelectProject(string projectOrOrder)
        {
            string field = Interface.IDB.IrradiationRequests.IrradiationCodeColumn.ColumnName;
            int position = Interface.IBS.Irradiations.Find(field, projectOrOrder);
            Interface.IBS.Irradiations.Position = position;
            IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
            currentChanged(ir, true, false, true);
        }

        public void SelectProjectHL(string projectOrOrder)
        {
            string field = Interface.IDB.Measurements.ProjectColumn.ColumnName;
            Interface.IBS.Measurements.Filter = field + " = '" + projectOrOrder + "'";

            // int position = Interface.IBS.Irradiations.Find(field, projectOrOrder);
            // Interface.IBS.Irradiations.Position = position; IrradiationRequestsRow ir =
            // Interface.ICurrent.Irradiation as IrradiationRequestsRow; currentChanged(ir, true,
            // false, true);
        }

        public void StartBinding()
        {
            Channels.CurrentChanged += currentChangedHandler;
            Irradiations.CurrentChanged += currentChangedHandler;

            SubSamples.CurrentChanged += currentChangedHandler;
            Vial.CurrentChanged += currentChangedHandler;
            Rabbit.CurrentChanged += currentChangedHandler;
            Units.CurrentChanged += currentChangedHandler;

            Matrix.CurrentChanged += currentChangedHandler;
            SelectedMatrix.CurrentChanged += currentChangedHandler;

            Measurements.CurrentChanged += currentChangedHandler;
            PeaksHL.CurrentChanged += currentChangedHandler;
            Peaks.CurrentChanged += currentChangedHandler;
            Gammas.CurrentChanged += currentChangedHandler;

            Matrix.AddingNew += addingNew;
            Channels.AddingNew += addingNew;
            Rabbit.AddingNew += addingNew;
            Vial.AddingNew += addingNew;
            Irradiations.AddingNew += addingNew;
            SubSamples.AddingNew += addingNew;

            // SelectedMatrix.AddingNew += addingNew;

            // Vial.ListChanged += listChanged_RabbitVial; .. Channels.ListChanged += listChanged_Channels;
        }

        public void SuspendBindings()
        {
            foreach (BindingSource b in bindings.Values)
            {
                try
                {
                    b.SuspendBinding();
                }
                catch (Exception ex)
                {
                    Interface.IStore.AddException(ex);
                }
            }
        }

        /// <summary>
        /// Updates the binding sources positions!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r">        </param>
        /// <param name="doCascade"></param>
        public void CurrentChanged<T>(T r, bool doCascade = true, bool findItself = true, bool selectedBS = false)
        {
            try
            {
                currentChanged(r, doCascade, findItself, selectedBS);
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /*
             public void EndEditGeometries()
                {
                    Matrix.EndEdit();
                    Geometry.EndEdit();
                    Compositions.EndEdit();
                    SelectedMatrix.EndEdit();
                    SelectedCompositions.EndEdit();
                    MUES.EndEdit();
                }
                */

        public BS()
        {
        }

        /// <summary>
        /// A Binding source for each table
        /// </summary>
        public BS(ref Interface inter)
        {
            Interface = inter;
            bindings = new Hashtable();

            initializePreferencesBindingSources();

            initializeProjectBindingSources();

            initializeGeometryBindingSources();

            initializeSampleBindingSources();

            initializeSelectedBindingSources();

            initializeMeasurementsBindingSources();
            // bindingList = new BindingList<BS>(); bindingList.Add(this);

            // Units.ListChanged += units_ListChanged;
        }
    }
}