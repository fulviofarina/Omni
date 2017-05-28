using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BS 
    {
        public DataRow GetDataRowFromDGV(object dgvSender, int rowInder)
        {
            DataGridView dgv = dgvSender as DataGridView;
            DataRow row = null;
            if (dgv.RowCount == 0)
            {
                Interface.IReport.Msg("No Rows found in the DataGridView", "Error", false); //report
            }
            else row = Caster.Cast<DataRow>(dgv.Rows[rowInder]);
            return row;
        }
        public void SelectUnitChildRow(int rowInder, ref DataRow row, ref int lastIndex)
        {
            try
            {
                  if (row == null) return;
                   bool isUnuit = row.GetType().Equals(typeof(UnitRow));
                    UnitRow unit = null;
                if (isUnuit)
                {
                    unit = row as UnitRow;
                    unit.ToDo = !unit.ToDo;
                    Interface.IReport.Msg("Sample "+unit.Name + " selected for calculations", "Selected!"); //report
                }
                else
                {
                    //NO UNIT, maybe a matrix, a vial, a rabbit or a channel
                    unit = Interface.ICurrent.Unit as UnitRow;
                    EnabledControls = false;
                    unit.SetParent(ref row);
                    Interface.IReport.Msg("Sample " + unit.Name + " values updated with the template item", "Updated!"); //report
                    EnabledControls = true;
                    //bring back to VIEW (Select)
                }

                Update<UnitRow>(unit, true, false, true);
                IRow ir = unit as IRow;
                ir.Check();
           //     Interface.IStore.Save(ref row);
                Interface.IStore.Save(ref unit);
                SubSamplesRow s = unit.SubSamplesRow;
                Interface.IStore.Save(ref s);
                HasErrors(row);
            }
            catch (System.Exception ex)
            {
                // Interface.IReport.Msg(ex.StackTrace, ex.Message);
                Interface.IStore.AddException(ex);
            }
        }
    }
    public partial class BS //: INotifyPropertyChanged
    {
        public bool EnabledControls
        {
            get
            {
                return enabledControls;
            }
            set
            {
                enabledControls = value;
                notifyPropertyChanged("EnabledControls");
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
                    EnabledControls = false;
                }
                else { EnabledControls = true; }
            }
        }
        public BindingList<BS> BindingList
        {
            get
            {
                return bindingList;
            }

            set
            {
                bindingList = value;
            }
        }

        //This is an event handler that fires the function the user GUI provides
        //the execution is called above on NotyPropertyChanged
        public  PropertyChangedEventHandler PropertyChanged=null;

     

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

        /// <summary>
        /// Checks a row for serious errors according to the NonNullable columns or specialized array
        /// of forbidden columns
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        public void HasErrors<T>(T r)
        {

            if (EC.IsNuDelDetch(r as DataRow)) return;

            Type tipo = typeof(T);

            bool isSubSample = tipo.Equals(typeof(SubSamplesRow));
            bool isUnit = tipo.Equals(typeof(UnitRow));
            bool isMatrix = tipo.Equals(typeof(MatrixRow));
            //to check later the columns that should be ok

            hasErrors = null;
            // Action Checker = null; DataColumn[] columnsThatShouldBeOk = null;
            if (isSubSample)
            {
                SubSamplesRow s = r as SubSamplesRow;
                hasErrors += s.HasBasicErrors;
                hasErrors += s.UnitRow.HasErrors;
            }
            else if (isUnit)
            {
                UnitRow u = r as UnitRow;
                hasErrors += u.HasErrors;
                hasErrors += u.SubSamplesRow.HasBasicErrors;
            }
            else if (isMatrix)
            {
                MatrixRow m = r as MatrixRow;
                hasErrors += m.HasErrors;
            }
            else if (tipo.Equals(typeof(VialTypeRow)))
            {
                //
                VialTypeRow v = r as VialTypeRow;
                hasErrors += v.HasErrors;
            }
            else if (tipo.Equals(typeof(ChannelsRow)))
            {
                ChannelsRow c = r as ChannelsRow;
                hasErrors += c.HasErrors;

                // updateChannel(r, doCascade, findItself);
            }
            else if (tipo.Equals(typeof(IrradiationRequestsRow)))
            {
                //
                IrradiationRequestsRow i = r as IrradiationRequestsRow;
                hasErrors += i.HasErrors;
                // updateIrradiationRequest(r, doCascade, findItself);
            }
            //now check the errors!!!
            hasCompulsoryErrors(r);
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


        public void EnableRaisingList(bool enable)
        {

            foreach(BindingSource bs in bindings.Values)
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
            Update(ir, true, false, true);
        }

        public void StartBinding()
        {
            SSFPreferences.ListChanged += listChanged_Preferences;
            Preferences.ListChanged += listChanged_Preferences;

            SubSamples.CurrentChanged += currentChanged;
            Channels.CurrentChanged += currentChanged;
            Irradiations.CurrentChanged += currentChanged;
            Matrix.CurrentChanged += currentChanged;
            Vial.CurrentChanged += currentChanged;
            Rabbit.CurrentChanged += currentChanged;
            Units.CurrentChanged += currentChanged;

            SelectedMatrix.CurrentChanged += currentChanged;


           

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
        public void Update<T>(T r, bool doCascade = true, bool findItself = true, bool selectedBS = false)
        {

            //    if (!EnabledControls) return;

            Type tipo = typeof(T);

            bool isSubSample = tipo.Equals(typeof(SubSamplesRow));
            bool isUnit = tipo.Equals(typeof(UnitRow));
            bool isMatrix = tipo.Equals(typeof(MatrixRow));
            //to check later the columns that should be ok

            // Action Checker = null; DataColumn[] columnsThatShouldBeOk = null;
            if (isSubSample)
            {
                //take columns that should be ok
                updateSubSample(r, doCascade, findItself);
            }
            else if (isUnit)
            {
                // columnsThatShouldBeOk = Interface.IDB.Unit.Changeables;
                updateUnit(r, doCascade, findItself);

                //the checker Method
                //       aChecker += s.CheckErrors;
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

           if (EnabledControls) HasErrors(r);
            //now check the errors!!!
            //   CheckCompulsoryErrors(r);
        }

        private void notifyPropertyChanged(/*[CallerMemberName]*/ String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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

            bindingList = new BindingList<BS>();
            bindingList.Add(this);

            // Units.ListChanged += units_ListChanged;
        }
    }
}