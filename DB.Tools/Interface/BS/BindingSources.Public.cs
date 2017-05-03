using System;
using System.Data;
using System.Windows.Forms;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BindingSources
    {
        public BindingSource Channels;

        public BindingSource Geometry;

        public BindingSource Irradiations;

        public BindingSource Matrix;
        public BindingSource Compositions;
        /// <summary>
        /// not attached yet
        /// </summary>
        public BindingSource Monitors;

        public BindingSource MonitorsFlags;

        public BindingSource Preferences;

        public BindingSource Rabbit;

        public BindingSource Samples;

        public BindingSource SelectedSubSample;
        public BindingSource SelectedMatrix;

        public BindingSource SSF;

        public BindingSource SSFPreferences;

        public BindingSource Standards;

        //binding sources to attach;
        public BindingSource SubSamples;

        public BindingSource Units;

        public BindingSource Vial;

        public void ApplyFilters()
        {
            string col = Interface.IDB.Preferences.WindowsUserColumn.ColumnName;
            Preferences.Filter = col + " = '" + Interface.IPreferences.WindowsUser + "'";
            SSFPreferences.Filter = Preferences.Filter;

            // Dumb.LinkBS(ref this.ChannelBS, Interface.IDB.Channels);
            string column = Interface.IDB.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = Interface.IDB.VialType.InnerRadiusColumn.ColumnName + " asc";
            // Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Rabbit.Filter = column + " = " + "True";
            Vial.Filter = column + " = " + "False";
            Vial.Sort = innerRadCol;
            Rabbit.Sort = innerRadCol;

            Geometry.Filter = string.Empty;
            Geometry.Sort = "CreationDateTime desc";
            // Dumb.LinkBS(ref Rabbit, Interface.IDB.VialType, column + " = " + "True", innerRadCol);

            //  Dumb.LinkBS(ref Vial, Interface.IDB.VialType, column + " = " + "False", innerRadCol);

            //  Dumb.LinkBS(ref Geometry, Interface.IDB.Geometry, string.Empty, );
            string sortColumn;
            sortColumn = Interface.IDB.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
            Irradiations.Sort = sortColumn;


            Matrix.Filter = "SubSampleID IS NULL";
            Matrix.Sort = "MatrixName desc";
            
        }

        /// <summary>
        /// EndEdit for each binding source
        /// </summary>
        public void EndEdit()
        {
            Matrix?.EndEdit();
            Compositions?.EndEdit();
            Units?.EndEdit();
            Vial?.EndEdit();
            Geometry?.EndEdit();
            Rabbit?.EndEdit();
            Channels?.EndEdit();
            Irradiations?.EndEdit();
            SubSamples?.EndEdit();
            SelectedSubSample?.EndEdit();
            SelectedMatrix?.EndEdit();
            SSF?.EndEdit();
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
                bool? seriousCellsWithErrors = !AChecker?.Invoke();
                // if (row.GetColumnsInError())
                if (seriousCellsWithErrors != null && (bool)seriousCellsWithErrors)
                {
                    Interface.IReport.Msg(_ROWWITHERROR, "Warning", false); ///cannot process because it has errors
                }
            }
        }

        public BindingSources()
        {
        }
        /// <summary>
        /// A Binding source for each table
        /// </summary>
        public BindingSources(ref Interface inter)
        {
            Interface = inter;

            Preferences = new BindingSource(Interface.Get(), Interface.IDB.Preferences.TableName);
            Compositions = new BindingSource(Interface.Get(), inter.IDB.Compositions.TableName);

            SSFPreferences = new BindingSource(Interface.Get(), Interface.IDB.SSFPref.TableName);

            Channels = new BindingSource(Interface.Get(), Interface.IDB.Channels.TableName);

            Matrix = new BindingSource(Interface.Get(), Interface.IDB.Matrix.TableName);
            Rabbit = new BindingSource(Interface.Get(), Interface.IDB.VialType.TableName);

            Vial = new BindingSource(Interface.Get(), Interface.IDB.VialType.TableName);

            Irradiations = new BindingSource(Interface.Get(), Interface.IDB.IrradiationRequests.TableName);

            Geometry = new BindingSource(Interface.Get(), Interface.IDB.Geometry.TableName);

            Standards = new BindingSource(Interface.Get(), Interface.IDB.Standards.TableName);

            Monitors = new BindingSource(Interface.Get(), Interface.IDB.Monitors.TableName);

            MonitorsFlags = new BindingSource(Interface.Get(), Interface.IDB.MonitorsFlags.TableName);

            Samples = new BindingSource(Interface.Get(), Interface.IDB.Samples.TableName);

            SubSamples = new BindingSource(Interface.Get(), Interface.IDB.SubSamples.TableName);


            Units = new BindingSource(Interface.Get(), Interface.IDB.Unit.TableName);
            // Units.CurrentChanged += units_CurrentChanged;

            SSF = new BindingSource(Interface.Get(), Interface.IDB.MatSSF.TableName);
            SelectedMatrix = new BindingSource(Interface.Get(), Interface.IDB.Matrix.TableName);
            // Units.BindingComplete += Units_BindingComplete;
            SelectedSubSample = new BindingSource(Interface.Get(), Interface.IDB.SubSamples.TableName);

            setHandlers();


            // Units.ListChanged += units_ListChanged;
        }

        private void setHandlers()
        {
            SubSamples.CurrentChanged += subSamples_CurrentChanged;

            SSFPreferences.ListChanged += preferences_ListChanged;
            Preferences.ListChanged += preferences_ListChanged;
            Channels.CurrentChanged += channels_CurrentChanged;


            Matrix.CurrentChanged += matrix_CurrentChanged;
        }

      
    }
}