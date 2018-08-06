using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class BS
    {
        private void currentChanged<T>(T r, bool doCascade = true, bool findItself = true, bool selectedBS = false)
        {
            Type tipo = typeof(T);

            bool isSubSample = tipo.Equals(typeof(SubSamplesRow));
            bool isUnit = tipo.Equals(typeof(UnitRow));
            bool isMatrix = tipo.Equals(typeof(MatrixRow));
            bool isVial = tipo.Equals(typeof(VialTypeRow));
            bool isChannel = tipo.Equals(typeof(ChannelsRow));
            bool isIrrRequest = tipo.Equals(typeof(IrradiationRequestsRow));
            bool isMeasurement = tipo.Equals(typeof(MeasurementsRow));
            //to check later the columns that should be ok
            hasErrorsMethod = null;

            // Action Checker = null; DataColumn[] columnsThatShouldBeOk = null;
            if (isSubSample)
            {
                //take columns that should be ok
                updateSubSample(r, doCascade, findItself);
                SubSamplesRow s = r as SubSamplesRow;

                if (showErrors)
                {
                    hasErrorsMethod = s.HasErrors;
                    IEnumerable<string> names = s.GetBasicColumnsInErrorNames();
                    hasCompulsoryErrors(s, names.ToArray(), false);
                    hasErrorsMethod = s.UnitRow.HasErrors;
                    hasCompulsoryErrors(s.UnitRow, null, true);
                }
            }
            else if (isUnit)
            {
                updateUnit(r, doCascade, findItself);
                UnitRow u = r as UnitRow;

                if (showErrors)
                {
                    if (!EC.IsNuDelDetch(u.SubSamplesRow))
                    {
                        hasErrorsMethod = u.SubSamplesRow.HasErrors;
                        IEnumerable<string> names = u.SubSamplesRow.GetBasicColumnsInErrorNames();
                        hasCompulsoryErrors(u.SubSamplesRow, names.ToArray(), false);
                        hasErrorsMethod = u.HasErrors;
                        hasCompulsoryErrors(u, null, true);
                    }
                }
            }
            else
            {
                if (isMatrix)
                {
                    //
                    updateMatrix(r, doCascade, findItself, selectedBS);
                }
                else if (isVial)
                {
                    //
                    updateVialRabbit(r, doCascade, findItself);
                }
                else if (isChannel)
                {
                    //
                    updateChannel(r, doCascade, findItself);
                }
                else if (isIrrRequest)
                {
                    //
                    updateIrradiationRequest(r, doCascade, findItself);
                }
                else if (isMeasurement)
                {
                    updateMeasurement(r, doCascade, findItself);
                }
                //to avoid spending too much time when loading
                if (showErrors && hasErrorsMethod != null)
                {
                    hasErrorsMethod = (r as IRow).HasErrors;
                    hasCompulsoryErrors(r);
                }
            }
        }

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
                LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;

                DataRow row = s.UnitRow;
                // doCascade = doCascade && EnabledControls;
                if (doCascade)
                {
                    currentChanged<UnitRow>(row as UnitRow);
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
                    currentChanged(row as VialTypeRow);
                    row = s.SubSamplesRow.ChCapsuleRow;
                    currentChanged(row as VialTypeRow);
                    row = s.ChannelsRow;//use the channel assigned to the unit; plan A
                    if (EC.IsNuDelDetch(row)) //plan b, use the channel from the Irr Req.
                    {
                        row = s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow;
                    }
                    currentChanged(row as ChannelsRow, true, true);

                    //aqui hacer el cambio Selected Matrix..
                    SubSamplesRow samp = s.SubSamplesRow;

                    //take template Matrix by default
                    row = samp.MatrixRow;
                    currentChanged(row as MatrixRow, true, false, false);
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

                    currentChanged(row as MatrixRow, true, true, true);
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
}