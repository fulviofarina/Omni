using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class BS
    {
        private void update<T>(T r, bool doCascade = true, bool findItself = true, bool selectedBS = false)
        {
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

            if (EnabledControls) hasErrors(r);
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
            string filterMatrixSelected = string.Empty;
            column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
            filterMatrixSelected = column + " IS NULL";

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
                    column = Interface.IDB.Matrix.SubSampleIDColumn.ColumnName;
                    //last matrix id
                    id = u.SubSamplesRow.SubSamplesID;
                    filterMatrixSelected = column + " = '" + id + "'";
                    filter += " AND " + column + " = '" + id + "'";
                }

                if (findItself)
                {
                    bs.Position = bs.Find(column, id);
                    // bs.ResetBindings(false);
                }
            }

            bs2.Filter = filter;
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

                if (findItself)
                {
                    // string unitValID = Interface.IDB.SubSamples.SubSamplesIDColumn.ColumnName;
                    SubSamples.Position = SubSamples.Find(column, s.SubSamplesID);
                }

                DataRow row = s.UnitRow;
                if (doCascade && EnabledControls)
                {
                    update<UnitRow>(row as UnitRow);
                }

                // if (!s.IsSubSampleNameNull()) {// string filColumn = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName;
                filter = column + " = '" + s.SubSamplesID + "'";
                // }
            }

            // if (EnabledControls)
            {
                // Rsx.Dumb.BS.LinkBS(ref SelectedSubSample, Interface.IDB.SubSamples,filter,string.Empty);
                SelectedSubSample.Filter = filter;
            }
        }

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
                if (doCascade && !EC.IsNuDelDetch(row) && EnabledControls)
                {
                    row = s.SubSamplesRow.VialTypeRow;
                    update<LINAA.VialTypeRow>(row as VialTypeRow);
                    row = s.SubSamplesRow.ChCapsuleRow;
                    update<LINAA.VialTypeRow>(row as VialTypeRow);
                    row = s.ChannelsRow;//use the channel assigned to the unit; plan A
                    if (EC.IsNuDelDetch(row)) //plan b, use the channel from the Irr Req.
                    {
                        row = s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow;
                    }
                    update<LINAA.ChannelsRow>(row as ChannelsRow, true, true);
                    row = s.SubSamplesRow.MatrixRow;
                    update<MatrixRow>(row as MatrixRow, true, false, false);
                    if (s.SubSamplesRow.MatrixRow != null)
                    {
                        row = s.SubSamplesRow.GetMatrixRows().FirstOrDefault(o => o.MatrixID == s.SubSamplesRow.MatrixID);
                    }
                    else row = s.SubSamplesRow.GetMatrixRows().FirstOrDefault();
                    update<MatrixRow>(row as MatrixRow, true, true, true);
                }

                column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
                filter = column + " = '" + s.UnitID.ToString() + "'";
            }
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