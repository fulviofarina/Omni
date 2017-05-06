using System;
using System.Collections;
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
        public BindingSource SelectedChannel;
        public BindingSource Geometry;

        public BindingSource Irradiations;

        public BindingSource Matrix;
        public BindingSource Compositions;

        /// <summary>
        /// </summary>
        public BindingSource Monitors;

        /// <summary>
        /// </summary>
        public BindingSource MonitorsFlags;

        public BindingSource Preferences;

        public BindingSource Rabbit;

        public BindingSource Samples;

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
            // Dumb.BS.LinkBS(ref Rabbit, Interface.IDB.VialType, column + " = " + "True", innerRadCol);

            // Dumb.BS.LinkBS(ref Vial, Interface.IDB.VialType, column + " = " + "False", innerRadCol);

            // Dumb.BS.LinkBS(ref Geometry, Interface.IDB.Geometry, string.Empty, );
            string sortColumn;
            sortColumn = Interface.IDB.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
            Irradiations.Sort = sortColumn;

            Matrix.Filter = "SubSampleID IS NULL";
            Matrix.Sort = "MatrixName desc";
        }

      
       
        public void SuspendBindings()
        {
            foreach ( BindingSource b in bindings.Values)
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
        public void Update<T>(T r, bool doCascade = true, bool findItself = true)
        {
            Type tipo = typeof(T);
            if (r == null) return;

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
                updateMatrix(r, findItself);
            }
            else if (tipo.Equals(typeof(VialTypeRow)))
            {
                //
                updateVialRabbit(r, findItself);
            }
            else if (tipo.Equals(typeof(ChannelsRow)))
            {
                //
                updateChannel(r, findItself);
            }
            else if (tipo.Equals(typeof(IrradiationRequestsRow)))
            {
                //
                updateIrradiationRequest(r, findItself);
            }
            //now check the errors!!!
            //   if (columnsThatShouldBeOk == null) return;
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
            Preferences = new BindingSource(set,name );
            bindings.Add(name, Preferences);

            name = inter.IDB.Compositions.TableName;
            Compositions = new BindingSource(set,name );
            bindings.Add(name, Compositions);

            name = Interface.IDB.SSFPref.TableName;
            SSFPreferences = new BindingSource(set, name);
            bindings.Add(name, SSFPreferences);

            name = Interface.IDB.Channels.TableName;
            Channels = new BindingSource(set, name);
            bindings.Add(name, Channels);

            name = Interface.IDB.Matrix.TableName;
            Matrix = new BindingSource(set,name );
            bindings.Add(name, Matrix);


            name = Interface.IDB.VialType.TableName ;
            Rabbit = new BindingSource(set, name);
            bindings.Add(name+"Rabbit", Rabbit);


            name = Interface.IDB.VialType.TableName;
            Vial = new BindingSource(set,name);
            bindings.Add(name, Vial);


            name = Interface.IDB.IrradiationRequests.TableName;
            Irradiations = new BindingSource(set, name);
            bindings.Add(name, Irradiations);


            name = Interface.IDB.Geometry.TableName;
            Geometry = new BindingSource(set,name );
            bindings.Add(name, Geometry);


            name = Interface.IDB.Standards.TableName;
            Standards = new BindingSource(set, name);
            bindings.Add(name, Standards);


            name = Interface.IDB.Monitors.TableName;
            Monitors = new BindingSource(set, name);
            bindings.Add(name, Monitors);


            name = Interface.IDB.MonitorsFlags.TableName;
            MonitorsFlags = new BindingSource(set,name );
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
            name =   Interface.IDB.MatSSF.TableName;
            SSF = new BindingSource(set, name);
            bindings.Add(name, SSF);


            name =  Interface.IDB.Channels.TableName;
            SelectedChannel = new BindingSource(set,name);
            bindings.Add("Selected"+name, SelectedChannel);


            name = Interface.IDB.Matrix.TableName;
            SelectedMatrix = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedMatrix);


            // Units.BindingComplete += Units_BindingComplete;
            name =  Interface.IDB.SubSamples.TableName;
            SelectedSubSample = new BindingSource(set,name);
            bindings.Add("Selected" + name, SelectedSubSample);



            setHandlers();

            // Units.ListChanged += units_ListChanged;
        }

        private void setHandlers()
        {
            SubSamples.CurrentChanged += currentChanged_SubSamples;

            SSFPreferences.ListChanged += listChanged_Preferences;
            Preferences.ListChanged += listChanged_Preferences;
            Channels.CurrentChanged += currentChanged_Channels;

            Matrix.CurrentChanged += currentChanged_Matrix;
        }
    }
}