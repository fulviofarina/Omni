using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;



using static DB.LINAA;
using System.ComponentModel;

namespace DB.Tools
{
    //basic update

    public partial class BS
    {
      
     

        private void updateMeasurement<T>(T r, bool doCascade, bool findItself)
        {
            string column = Interface.IDB.PeaksHL.MeasurementIDColumn.ColumnName;
            string filter = column + " IS NULL";

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                MeasurementsRow picked = r as MeasurementsRow;
                int? id = picked.MeasurementID;
                if (findItself)
                {
                    Measurements.Position = Measurements.Find(column, id);
                }
                Interface.IPopulate.IMeasurements.PopulatePeaksHL(id);

                filter = column + " = '" + picked.MeasurementID + "'";

                SpecPrefRow pref = Interface.IPreferences.CurrentSpecPref;

                if (pref != null)
                {
                    column = Interface.IDB.PeaksHL.AreaColumn.ColumnName;
                    filter += " AND " + column + " >= '" + pref.minArea + "'";
                    column = Interface.IDB.PeaksHL.AreaUncertaintyColumn.ColumnName;
                    filter += " AND " + column + " <= '" + pref.maxUnc + "'";
                }
            }

            PeaksHL.Filter = filter;
            Peaks.Filter = filter;
            // throw new NotImplementedException();
        }

        private void updateChannel<T>(T r, bool doCascade, bool findItself)
        {
            string col = Interface.IDB.Channels.ChannelsIDColumn.ColumnName;
            string col2 = Interface.IDB.IrradiationRequests.ChannelsIDColumn.ColumnName;
            // string column = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName;
            string filter = col + " IS NULL";
            string filter2 = col2 + " IS NULL";

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                ChannelsRow u = r as ChannelsRow;
                int id = u.ChannelsID;
                if (findItself)
                {
                    Channels.Position = Channels.Find(col, id);
                }
                filter = col + " = '" + id + "'";
                filter2 = col2 + " = '" + id + "'";// + " OR " + chColumn + " IS NULL ";
            }
            SelectedIrradiation.Filter = filter2;
            SelectedChannel.Filter = filter;
        }

        private void updateIrradiationRequest<T>(T r, bool doCascade = true, bool findItself = false)
        {
            string column = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName;
            string filter = column + " IS NULL";

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                LINAA.IrradiationRequestsRow u = r as LINAA.IrradiationRequestsRow;
                int id = u.IrradiationRequestsID;
                if (findItself)
                {
                    Irradiations.Position = Irradiations.Find(column, id);
                    // Irradiations.ResetBindings(false);
                }
                if (doCascade)
                {
                    string filter1 = string.Empty;
                    string filter2 = string.Empty;
                    //reset the selected bindings.. everything changed..
                    column = Interface.IDB.Unit.IrrReqIDColumn.ColumnName;
                    filter1 = column + " = '" + id + "'";

                    Units.Filter = filter1;
                    column = Interface.IDB.SubSamples.IrradiationRequestsIDColumn.ColumnName;
                    filter2 = column + " = '" + id + "'";
                    Interface.IBS.SubSamples.Filter = filter2;

                    Interface.IPopulate.ISamples.PopulateSubSamples(id);
                    Interface.IPreferences.CurrentPref.LastIrradiationProject = u.IrradiationCode;
                    Interface.IPreferences.SavePreferences();
                    Interface.IReport.Speak("Loaded project " + u.IrradiationCode);
                }

                column = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName;
                filter = column + " = '" + id + "'";// + " OR " + chColumn + " IS NULL ";
            }

            SelectedIrradiation.Filter = filter;
        }

        private void updateMatrix<T>(T r, bool doCascade = true, bool findItself = false, bool selectedBS = false)
        {
            string column = Interface.IDB.Compositions.MatrixIDColumn.ColumnName;

            string filter = column + " IS NULL";
            // string filterMatrixSelected = string.Empty;
            column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
            string filterMatrixSelected = column + " IS NULL";

            BindingSource bs2 = Compositions;
            if (selectedBS)
            {
                bs2 = SelectedCompositions;
            }
            BindingSource bs = Matrix;
            if (selectedBS)
            {
                bs = SelectedMatrix;
            }

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                MatrixRow u = r as MatrixRow;
                int id = u.MatrixID; //default matrix id to filter with
                                     // column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
                filter = column + " = '" + id + "'";
                if (selectedBS)
                {
                    // column2 = Interface.IDB.Matrix.TemplateIDColumn.ColumnName;
                    column = Interface.IDB.Matrix.SubSampleIDColumn.ColumnName;
                    //last matrix id
                    id = u.SubSampleID;
                    filterMatrixSelected = column + " = '" + id + "'";

                    // filter += " AND " + column2 + " = '" + id + "'";
                }
                // bool nulo = u.IsXCOMTableNull();
                if (u.NeedsMUES)
                {
                    bool sql = !Interface.IPreferences.CurrentPref.Offline;
                    MUESDataTable mu = Interface.IPopulate.IGeometry.GetMUES(ref u, sql);
                    Interface.IDB.MUES.Merge(mu);
                }

                if (findItself)
                {
                    column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
                    id = u.MatrixID;
                    bs.Position = bs.Find(column, id);
                    // bs.ResetBindings(false);
                }
            }

            //MEJORAR ESTE FILTROOOOOOO, DEBERIA RESETEAR A LOS NULL COMO ARRIBA PERO CON LAS COLUMNAS QUE SON
            //   MUES.Filter = string.Empty;
            MUES.Filter = filter;

            bs2.Filter = filter; //sea cual sea este es el filtrro
            if (selectedBS) bs.Filter = filterMatrixSelected;
            // bs2.ResetBindings(false);
        }

        private void updateSubSample<T>(T r, bool doCascade, bool findItself)
        {
            // Rsx.Dumb.BS.DeLinkBS(ref SelectedSubSample);
            string column = Interface.IDB.SubSamples.SubSamplesIDColumn.ColumnName;
            string filter = column + " IS NULL";

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                SubSamplesRow s = r as SubSamplesRow;

                DataRow row = s.UnitRow;
                // doCascade = doCascade && EnabledControls;
                if (doCascade)
                {
                    CurrentChanged<UnitRow>(row as UnitRow);
                }

                if (findItself)
                {
                    // string unitValID = Interface.IDB.SubSamples.SubSamplesIDColumn.ColumnName;
                    SubSamples.Position = SubSamples.Find(column, s.SubSamplesID);
                }

                filter = column + " = '" + s.SubSamplesID + "'";
            }

            SelectedSubSample.Filter = filter;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r">         </param>
        /// <param name="doCascade"> </param>
        /// <param name="findItself"></param>
        private void updateUnit<T>(T r, bool doCascade, bool findItself)
        {
            Rsx.Dumb.BS.DeLinkBS(ref SSF);
            string column = Interface.IDB.MatSSF.MatSSFIDColumn.ColumnName;
            string filter = column + " IS NULL";
            string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                LINAA.UnitRow s = r as LINAA.UnitRow;
                if (findItself)
                {
                    string unitValID = Interface.IDB.Unit.UnitIDColumn.ColumnName;
                    Units.Position = Units.Find(unitValID, s.UnitID);
                }

                DataRow row = s.SubSamplesRow;
                //do childs parents or not?
                doCascade = doCascade && !EC.IsNuDelDetch(row);
                //TOD:  doCascade = doCascade && EnabledControls;
                if (doCascade)
                {
                    row = s.SubSamplesRow.VialTypeRow;
                    CurrentChanged(row as VialTypeRow);
                    row = s.SubSamplesRow.ChCapsuleRow;
                    CurrentChanged(row as VialTypeRow);
                    row = s.ChannelsRow;//use the channel assigned to the unit; plan A
                    if (EC.IsNuDelDetch(row)) //plan b, use the channel from the Irr Req.
                    {
                        row = s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow;
                    }
                    CurrentChanged(row as ChannelsRow, true, true);

                    //aqui hacer el cambio Selected Matrix..
                    SubSamplesRow samp = s.SubSamplesRow;

                    //take template Matrix by default
                    row = samp.MatrixRow;
                    CurrentChanged(row as MatrixRow, true, false, false);
                    IEnumerable<MatrixRow> ROWS = samp.GetMatrixRows();
                    if (!EC.IsNuDelDetch(samp.MatrixRow))
                    {
                        //take Latest! child matrix then...
                        row = ROWS
                                  // .Where(o => !o.IsTemplateIDNull())
                                  .FirstOrDefault(o => o.MatrixID == samp.MatrixID);
                    }
                    //take any child matrix then
                    else row = ROWS.FirstOrDefault();

                    // row = ROWS

                    CurrentChanged(row as MatrixRow, true, true, true);
                }

                column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
                filter = column + " = '" + s.UnitID.ToString() + "'";
            }
            //por que cuando estan enabled?
            //por que se pone a autochequear?
            if (EnabledControls)
            {
                // SelectedSubSample.Filter = filter;
                Rsx.Dumb.BS.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);
            }
        }

        private void updateVialRabbit<T>(T r, bool doCascade, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.VialTypeRow u = r as LINAA.VialTypeRow;
            BindingSource bs = Rabbit;

            if (!u.IsIsRabbitNull() && !u.IsRabbit) bs = Vial;

            if (findItself)
            {
                string column = Interface.IDB.VialType.VialTypeIDColumn.ColumnName;
                int id = u.VialTypeID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                bs.Position = bs.Find(column, id);
            }
        }
    }

    // other update

    public partial class BS
    {
        private void addingNew(object sender, AddingNewEventArgs e)
        {
            try
            {
                // EnabledControls = false;

                bool isChannel = sender.Equals(Channels);
                bool aRabbit = sender.Equals(Rabbit);
                bool aVial = sender.Equals(Vial);
                if (isChannel)
                {
                    ChannelsRow c = Interface.IPopulate.IIrradiations.AddNewChannel();
                    e.NewObject = c;
                    CurrentChanged(c, false, true);
                }
                else if (sender.Equals(Matrix))
                {
                    MatrixRow v = Interface.IPopulate.IGeometry.AddMatrix();
                    e.NewObject = v;
                    CurrentChanged(v, false, true);
                }
                else if (aVial || aRabbit)
                {
                    VialTypeRow v = Interface.IPopulate.IGeometry.AddVial(aRabbit);
                    e.NewObject = v;
                    CurrentChanged(v, false, true);
                }
                else if (sender.Equals(SubSamples))
                {
                    IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    SubSamplesRow s = Interface.IPopulate.ISamples.AddSample(ref ir);
                    e.NewObject = s;
                    CurrentChanged(s, false, true);

                    // SelectedSubSample.ResetBindings(false); Units.ResetBindings(false);
                    // SelectedMatrix.ResetBindings(false); SelectedCompositions.ResetBindings(false);
                }
                //reanimate controls
                Interface.IBS.EnabledControls = true;
                IRow row = e.NewObject as IRow;
                row.Check();
                // (sender as BindingSource).ResetBindings(false);

                // EnabledControls = true;
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void notifyPropertyChanged(/*[CallerMemberName]*/ String propertyName = "")
        {
            if (propertyName.CompareTo(RSX.ENABLE_CONTROLS_FIELD) == 0)
            {
                EnableControlsChanged?.Invoke(this, EventArgs.Empty);
            }
            //put some other handlers to execute (invoke) when other properties are changed
        }

        /// <summary>
        /// Checks a row for serious errors according to the NonNullable columns or specialized array
        /// of forbidden columns
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        private bool hasCompulsoryErrors<T>(T r, string[] colsinError = null, bool accumulate = false)
        {
            if (EC.IsNuDelDetch(r as DataRow))
            {
                return true;
            }
            DataRow row = r as DataRow;
            string tableName = row.Table.TableName;
            string msg = RSX.ROW_OK;
            string title = RSX.CHECKED;
            bool ok = true;
            if (row.HasErrors)
            {
                IRow irow = (IRow)row;
                
                bool seriousCellsWithErrors = irow.HasErrors();
                // if (row.GetColumnsInError())
                if ( seriousCellsWithErrors)
                {
                    //in case I send my own set of cols with errores
                    if (colsinError == null)
                    {
                        colsinError = row.GetColumnsInError().Select(o => o.ColumnName).ToArray();
                    }
                    string result = Interface.GetDisplayColumName(tableName, colsinError);
                    ok = false;
                    msg = RSX.ROW_WITH_ERROR + result;
                    title = RSX.WARNING;
                }
            }
            //get display name
            Interface.GetDisplayTableName(ref tableName);
            //display name
            msg = msg.Replace("item", tableName);
            Interface.IReport.Msg(msg, title, ok, accumulate);
            //clean
            //hasErrorsMethod = null;
            return ok;
        }

        private void currentChangedHandler(object sender, EventArgs e)
        {

            if (!EnabledControls) return;

            //BindingSource bs = sender as BindingSource;
            bool selectedBs = false;
            bool doCascade = true;
            bool findItself = false;


            dynamic r = null;
            if (sender.Equals(Units))
            {

                findItself = true;
                doCascade = false;

            }
            else if (sender.Equals(SelectedMatrix))
            {
                selectedBs = true;

            }

           
                /*
                if (sender.Equals(Units))
                {
                    //take current
                    UnitRow u = Interface.ICurrent.Unit as UnitRow;
                    //if not null
                    if (!EC.IsNuDelDetch(u.SubSamplesRow))
                    {
                        r = u.SubSamplesRow;
                    }

                }
                else if (sender.Equals(SubSamples))
                {

                    r = Interface.ICurrent.SubSample as SubSamplesRow;


                }
                else if (sender.Equals(Irradiations))
                {
                    r = Interface.ICurrent.Irradiation as IrradiationRequestsRow;

                }
                else if (sender.Equals(Channels))
                {
                    r = Interface.ICurrent.Channel as ChannelsRow;

                }
                else if (sender.Equals(Measurements))
                {
                    r = Interface.ICurrent.Measurement as MeasurementsRow;

                }

                else if (sender.Equals(Matrix))
                {

                    r = Interface.ICurrent.Matrix as MatrixRow;


                }
                else if (sender.Equals(SelectedMatrix))
                {
                    r = Interface.ICurrent.SubSampleMatrix as MatrixRow;

                }


                else if (sender.Equals(Rabbit))
                {
                    r = Interface.ICurrent.Rabbit as VialTypeRow;
                }
                else if (sender.Equals(Vial))
                {
                    r = Interface.ICurrent.Vial as VialTypeRow;
                }

                */
                //     CurrentChanged(s, doCascade, findItself, selectedBs);
                BindingSource BS = sender as BindingSource;

                r = (BS.Current as DataRowView)?.Row;

                if (sender.Equals(Units))
                {
                    //take current
                    UnitRow u = Interface.ICurrent.Unit as UnitRow;
                    //if not null
                    if (u!=null && !EC.IsNuDelDetch(u.SubSamplesRow))
                    {
                        r = u.SubSamplesRow;
                    }

                }

           

            CurrentChanged(r, doCascade, findItself, selectedBs);


        }

    
       

        private void listChanged_Preferences(object sender, ListChangedEventArgs e)
        {
            try
            {
                // if (e.ListChangedType != System.ComponentModel.ListChangedType.ItemChanged) return;

                string main = "Main";
                if (sender.Equals(Preferences))
                {
                }
                else
                {
                    main = "SSF";
                }
                Interface.IReport.Msg("A " + main + " preference was updated", main + " Preferences updated", true);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }
    }


}