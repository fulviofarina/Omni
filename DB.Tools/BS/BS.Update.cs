using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class BS
    {
        private void updateVialOrRabbit(ref BindingSource sender, bool selectedBs)
        {
            VialTypeRow c = null;
            if (sender.Equals(Rabbit))
            {
                c = Interface.ICurrent.Rabbit as VialTypeRow;
            }
            else if (sender.Equals(Vial))
            {
                c = Interface.ICurrent.Vial as VialTypeRow;
            }

            currentChanged(c, true, false, selectedBs);
        }

        private void updateMatrixOrSelected(ref BindingSource sender, bool selectedBs)
        {
            MatrixRow c = null;
            if (sender.Equals(Matrix)) c = Interface.ICurrent.Matrix as MatrixRow;
            else
            {
                selectedBs = true;
                c = Interface.ICurrent.SubSampleMatrix as MatrixRow;
            }
            currentChanged(c, true, false, selectedBs);
            // return selectedBs;
        }

        private void updateSubSampleOrUnit(ref BindingSource sender, bool selectedBs)
        {
            SubSamplesRow r = null; //which one to send
                                    //the SubSample from the SubSample of the unit that called or
                                    //the SubSample from the Unit that called
                                    //by default doCascade = true

            if (sender.Equals(SubSamples))
            {
                //take current
                //    bool doCascade = true;
                //   bool findItself = false; //should find itself = false
                r = Interface.ICurrent.SubSample as SubSamplesRow;
                currentChanged(r, true, false, selectedBs);
            }
            else
            {
                //take current
                UnitRow u = Interface.ICurrent.Unit as UnitRow;
                //if not null
                if (!EC.IsNuDelDetch(u.SubSamplesRow))
                {
                    //   findItself = true; //when unit request yes,
                    // doCascade = false;
                    //find the subsample in the binding source list
                    //update
                    r = u.SubSamplesRow;
                }

                currentChanged(r, false, true, selectedBs);
            }
        }

        private void currentChanged<T>(T r, bool doCascade = true, bool findItself = true, bool selectedBS = false)
        {
            Type tipo = typeof(T);

            bool isSubSample = tipo.Equals(typeof(SubSamplesRow));
            bool isUnit = tipo.Equals(typeof(UnitRow));
            bool isMatrix = tipo.Equals(typeof(MatrixRow));
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
                    hasErrorsMethod = u.SubSamplesRow.HasErrors;
                    IEnumerable<string> names = u.SubSamplesRow.GetBasicColumnsInErrorNames();
                    hasCompulsoryErrors(u.SubSamplesRow, names.ToArray(), false);
                    hasErrorsMethod = u.HasErrors;
                    hasCompulsoryErrors(u, null, true);
                }
            }
            else
            {
                if (isMatrix)
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
                //to avoid spending too much time when loading
                if (showErrors && hasErrorsMethod != null)
                {
                    hasErrorsMethod = (r as IRow).HasErrors;
                    hasCompulsoryErrors(r);
                }
            }
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
                LINAA.ChannelsRow u = r as LINAA.ChannelsRow;
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
            BindingSource bs = null;
            if (selectedBS)
            {
                bs = SelectedMatrix;
            }
            else bs = Matrix;

            if (!EC.IsNuDelDetch(r as DataRow))
            {
                LINAA.MatrixRow u = r as LINAA.MatrixRow;
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
                    column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
                    id = u.MatrixID;
                    // filter += " AND " + column2 + " = '" + id + "'";
                }
               // bool nulo = u.IsXCOMTableNull();
                if (u.NeedsMUES )
                {
                    bool sql = !Interface.IPreferences.CurrentPref.Offline;
                    MUESDataTable mu =   Interface.IPopulate.IGeometry.GetMUES(ref u, sql);
                    Interface.IDB.MUES.Merge(mu);
                }


                if (findItself)
                {
                    bs.Position = bs.Find(column, id);
                    // bs.ResetBindings(false);
                }
            }
        
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