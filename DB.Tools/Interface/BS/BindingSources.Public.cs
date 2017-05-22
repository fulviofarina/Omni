using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BindingSources
    {
        public BindingSource Channels;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedChannel;

        public BindingSource Geometry;

        public BindingSource Irradiations;

        public BindingSource Matrix;
        public BindingSource Compositions;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedIrradiation;

        public BindingSource Projects;
        public BindingSource Orders;

        public BindingSource Monitors;

        public BindingSource MonitorsFlags;

        public BindingSource Preferences;

        public BindingSource Rabbit;

        public BindingSource Samples;
        public BindingSource SelectedCompositions;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedSubSample;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedMatrix;

        public BindingSource SSF;

        public BindingSource SSFPreferences;

        public BindingSource Standards;

        //binding sources to attach;
        public BindingSource SubSamples;

        public BindingSource Units;

        public BindingSource Vial;

        /// <summary>
        /// Applies the Binding Source default filters
        /// </summary>
        public void ApplyFilters()
        {
            string col = Interface.IDB.Preferences.WindowsUserColumn.ColumnName;
            Preferences.Filter = col + " = '" + Interface.IPreferences.WindowsUser + "'";
            SSFPreferences.Filter = Preferences.Filter;

            // Dumb.BS.LinkBS(ref this.ChannelBS, Interface.IDB.Channels);
            string column = Interface.IDB.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = Interface.IDB.VialType.InnerRadiusColumn.ColumnName + " asc";
            // Dumb.BS.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Rabbit.Filter = column + " = " + "True";
            Vial.Filter = column + " = " + "False";
            Vial.Sort = innerRadCol;
            Rabbit.Sort = innerRadCol;

            Geometry.Filter = string.Empty;
            Geometry.Sort = "CreationDateTime desc";

            // Interface.IBS.SelectedSubSample.Filter =
            // Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName + " IS NULL" ;
            // Interface.IBS.SelectedMatrix.Filter = Interface.IDB.Matrix.MatrixIDColumn.ColumnName+
            // " IS NULL";

            string sort = Interface.IDB.SubSamples.SubSampleNameColumn + " asc";
            Interface.IBS.SubSamples.Sort = sort;

            sort = Interface.IDB.Unit.NameColumn.ColumnName + " asc";
            Interface.IBS.Units.Sort = sort;

            sort = Interface.IDB.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
            Irradiations.Sort = sort;

            Matrix.Filter = "SubSampleID IS NULL";
            SelectedMatrix.Filter = Matrix.Filter + " AND MatrixID = 0";
            sort = Interface.IDB.Compositions.IDColumn.ColumnName + " desc";
            SelectedCompositions.Sort = sort;

            Compositions.Sort = sort;
            Compositions.Filter = SelectedMatrix.Filter;
            SelectedCompositions.Filter = SelectedMatrix.Filter;

            Matrix.Sort = "MatrixID desc";
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

        /// <summary>
        /// EndEdit for each binding source
        /// </summary>
        public void EndEdit()
        {
            foreach (BindingSource b in bindings.Values)
            {
                try
                {
                    b.EndEdit();
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
        public void Update<T>(T r, bool doCascade = true, bool findItself = true, bool selectedBS = false)
        {
            Type tipo = typeof(T);
            //

            bool isSubSample = tipo.Equals(typeof(SubSamplesRow));
            bool isUnit = tipo.Equals(typeof(UnitRow));
            bool isMatrix = tipo.Equals(typeof(MatrixRow));
            //to check later the columns that should be ok

            // Action Checker = null; DataColumn[] columnsThatShouldBeOk = null;
            if (isSubSample)
            {
                //take columns that should be ok
                // columnsThatShouldBeOk = Interface.IDB.SubSamples.NonNullableUnit;
                updateSubSample(r, doCascade, findItself);
            }
            else if (isUnit)
            {
                // columnsThatShouldBeOk = Interface.IDB.Unit.Changeables;
                updateUnit(r, doCascade, findItself);
            }
            else if (isMatrix)
            {
                //
                updateMatrix(r, doCascade, findItself, selectedBS);
            }
            else if (tipo.Equals(typeof(VialTypeRow)))
            {
                //
                updateVialRabbit(r, doCascade, findItself);
            }
            else if (tipo.Equals(typeof(ChannelsRow)))
            {
                //
                updateChannel(r, doCascade, findItself);
            }
            else if (tipo.Equals(typeof(IrradiationRequestsRow)))
            {
                //
                updateIrradiationRequest(r, doCascade, findItself);
            }
            //now check the errors!!!
            //   if (columnsThatShouldBeOk == null) return;
            if (Rsx.EC.IsNuDelDetch(r as DataRow)) return;
            DataRow row = r as DataRow;
            if (row.HasErrors)
            {
                bool? seriousCellsWithErrors = !aChecker?.Invoke();
                // if (row.GetColumnsInError())
                if (seriousCellsWithErrors != null && (bool)seriousCellsWithErrors)
                {
                    Interface.IReport.Msg(ROW_WITH_ERROR, "Warning", false); ///cannot process because it has errors
                }
            }
        }

        public void StartBinding()
        {
            SubSamples.CurrentChanged += currentChanged_SubSamples;
            SSFPreferences.ListChanged += listChanged_Preferences;
            Preferences.ListChanged += listChanged_Preferences;

            Channels.CurrentChanged += currentChanged_Channels;
            Channels.AddingNew += addingNew;

            Irradiations.CurrentChanged += currentChanged_Irradiations;


            Matrix.CurrentChanged += currentChanged_Matrix;
            Matrix.AddingNew += addingNew;

            SelectedMatrix.CurrentChanged += currentChanged_Matrix;
            SelectedMatrix.AddingNew += addingNew;

          

            Rabbit.AddingNew += addingNew;
        //    Vial.ListChanged += listChanged_RabbitVial;
            //  ..  Channels.ListChanged += listChanged_Channels;
         
            Vial.AddingNew += addingNew;


            SubSamples.AddingNew += addingNew;
        }

     

        public BindingSources()
        {
        }

        protected Hashtable bindings;

        /// <summary>
        /// A Binding source for each table
        /// </summary>
        public BindingSources(ref Interface inter)
        {
            Interface = inter;
            LINAA set = inter.Get();

            bindings = new Hashtable();

            string name = Interface.IDB.Preferences.TableName;
            Preferences = new BindingSource(set, name);
            bindings.Add(name, Preferences);

            name = inter.IDB.Compositions.TableName;
            Compositions = new BindingSource(set, name);
            bindings.Add(name, Compositions);

            name = Interface.IDB.SSFPref.TableName;
            SSFPreferences = new BindingSource(set, name);
            bindings.Add(name, SSFPreferences);

            name = Interface.IDB.Channels.TableName;
            Channels = new BindingSource(set, name);
            bindings.Add(name, Channels);

            name = Interface.IDB.Matrix.TableName;
            Matrix = new BindingSource(set, name);
            bindings.Add(name, Matrix);

            // name = Interface.IDB.IrradiationRequests.TableName; IrradiationRequests = new
            // BindingSource(set, name); bindings.Add(name, IrradiationRequests);

            name = Interface.IDB.Projects.TableName;
            Projects = new BindingSource(set, name);
            bindings.Add(name, Projects);

            name = Interface.IDB.Orders.TableName;
            Orders = new BindingSource(set, name);
            bindings.Add(name, Orders);

            name = Interface.IDB.VialType.TableName;
            Rabbit = new BindingSource(set, name);
            bindings.Add(name + "Rabbit", Rabbit);

            name = Interface.IDB.VialType.TableName;
            Vial = new BindingSource(set, name);
            bindings.Add(name, Vial);

            name = Interface.IDB.IrradiationRequests.TableName;
            Irradiations = new BindingSource(set, name);
            bindings.Add(name, Irradiations);

            name = Interface.IDB.Geometry.TableName;
            Geometry = new BindingSource(set, name);
            bindings.Add(name, Geometry);

            name = Interface.IDB.Standards.TableName;
            Standards = new BindingSource(set, name);
            bindings.Add(name, Standards);

            name = Interface.IDB.Monitors.TableName;
            Monitors = new BindingSource(set, name);
            bindings.Add(name, Monitors);

            name = Interface.IDB.MonitorsFlags.TableName;
            MonitorsFlags = new BindingSource(set, name);
            bindings.Add(name, MonitorsFlags);

            name = Interface.IDB.Samples.TableName;
            Samples = new BindingSource(set, name);
            bindings.Add(name, Samples);

            name = Interface.IDB.SubSamples.TableName;
            SubSamples = new BindingSource(set, name);
            bindings.Add(name, SubSamples);

            name = Interface.IDB.Unit.TableName;
            Units = new BindingSource(set, name);
            bindings.Add(name, Units);

            // Units.CurrentChanged += units_CurrentChanged;
            name = Interface.IDB.MatSSF.TableName;
            SSF = new BindingSource(set, name);
            bindings.Add(name, SSF);

            name = Interface.IDB.Channels.TableName;
            SelectedChannel = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedChannel);

            name = inter.IDB.Compositions.TableName;
            SelectedCompositions = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedCompositions);
            name = Interface.IDB.Matrix.TableName;
            SelectedMatrix = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedMatrix);

            // Units.BindingComplete += Units_BindingComplete;
            name = Interface.IDB.SubSamples.TableName;
            SelectedSubSample = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedSubSample);

            name = Interface.IDB.IrradiationRequests.TableName;
            SelectedIrradiation = new BindingSource(set, name);
            bindings.Add("Selected" + name, Irradiations);

            // Units.ListChanged += units_ListChanged;
        }
    }
}