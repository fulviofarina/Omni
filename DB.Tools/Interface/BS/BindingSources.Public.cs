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
        public BindingSource SSF;

        public BindingSource Irradiations;

        public BindingSource Matrix;
        public BindingSource MonitorsFlags;
        public BindingSource Samples;
        /// <summary>
        /// not attached yet
        /// </summary>
        public BindingSource Monitors;

        public BindingSource Preferences;

        public BindingSource Rabbit;

        public BindingSource SSFPreferences;
        public BindingSource Standards;
        //binding sources to attach;
        public BindingSource SubSamples;

        public BindingSource Units;
        public BindingSource Vial;

        /// <summary>
        /// EndEdit for each binding source
        /// </summary>
        public void EndEdit()
        {
            Matrix?.EndEdit();
            Units?.EndEdit();
            Vial?.EndEdit();
            Geometry?.EndEdit();
            Rabbit?.EndEdit();
            Channels?.EndEdit();
            Irradiations?.EndEdit();
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

            if (isSubSample)
            {
                updateSubSample(r, doCascade, findItself);
            }
            else if (isUnit)
            {
                updateUnit(r, doCascade, findItself);
            }
            else if (isMatrix)
            {
                updateMatrix(r, findItself);
            }
            else if (tipo.Equals(typeof(VialTypeRow)))
                {
                    updateVialRabbit(r, findItself);
                }
              
                else if (tipo.Equals(typeof(ChannelsRow)))
                {
                    updateChannel(r, findItself);
                }
                else if (tipo.Equals(typeof(IrradiationRequestsRow)))
                {
                    updateIrradiationRequest(r, findItself);
                    //   return;
                }

            if (r != null)
            {
                if ((r as DataRow).HasErrors)
                {
                    Interface.IReport.Msg(rowWithError, "Warning", false); ///cannot process because it has errors
                }
            }
        }

   
        /// <summary>
        /// A Binding source for each table
        /// </summary>
        public BindingSources(ref Interface inter)
        {
            Interface = inter;


            Preferences = new BindingSource(Interface.Get(), Interface.IDB.Preferences.TableName);
      Preferences.ListChanged += preferences_ListChanged;

            SSFPreferences = new BindingSource(Interface.Get(), Interface.IDB.SSFPref.TableName);
            SSFPreferences.ListChanged += preferences_ListChanged;

            Channels = new BindingSource(Interface.Get(), Interface.IDB.Channels.TableName);
            Channels.CurrentChanged += channels_CurrentChanged;
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

            SubSamples.CurrentChanged += subSamples_CurrentChanged;

            Units = new BindingSource(Interface.Get(), Interface.IDB.Unit.TableName);
            Units.CurrentChanged += units_CurrentChanged;

            SSF = new BindingSource(Interface.Get(), Interface.IDB.MatSSF.TableName);

            //  Units.BindingComplete += Units_BindingComplete;

           // Units.ListChanged += units_ListChanged;
        }

        public void ApplyFilters()
        {
            string col = Interface.IDB.Preferences.WindowsUserColumn.ColumnName;
            Preferences.Filter = col + " = '" + Interface.IPreferences.WindowsUser + "'";
            SSFPreferences.Filter = Preferences.Filter;

            //    Dumb.LinkBS(ref this.ChannelBS, Interface.IDB.Channels);
            string column = Interface.IDB.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = Interface.IDB.VialType.InnerRadiusColumn.ColumnName + " asc";
            //      Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Dumb.LinkBS(ref Rabbit, Interface.IDB.VialType, column + " = " + "True", innerRadCol);

            Dumb.LinkBS(ref Vial, Interface.IDB.VialType, column + " = " + "False", innerRadCol);


            Dumb.LinkBS(ref Geometry, Interface.IDB.Geometry, string.Empty, "CreationDateTime desc");

        }


    }

   
}