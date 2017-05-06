using System;
using Rsx.Dumb;
using Rsx;
using static DB.LINAA;
using System.Windows.Forms;
using System.Data;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>

    public partial class BindingSources
    {
        // private Interface Interface;
        protected string ROW_WITH_ERROR = "The selected Item has incomplete information, i.e. cells with errors or null values";

      
    }

    public partial class BindingSources
    {
        private CheckerDelegate aChecker = null;

        protected Interface Interface;

        private delegate bool CheckerDelegate();

        // private string rowWithError = "The selected row has some incomplete cells or cells with
        // errors.\nPlease fix before selecting it";
    }

    public partial class BindingSources
    {
        private void currentChanged_Matrix(object sender, EventArgs e)
        {
            try
            {
                //
                LINAA.MatrixRow c = Interface.ICurrent.Matrix as LINAA.MatrixRow;
                Update(c, true, false);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void currentChanged_Channels(object sender, EventArgs e)
        {
            try
            {
                //
                LINAA.ChannelsRow c = Interface.ICurrent.Channel as LINAA.ChannelsRow;
                Update(c, true, false);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
            // Dumb.BS.LinkBS(ref Interface.IBS.Irradiations, Interface.IDB.IrradiationRequests, filter,
            // sortColumn + " desc");
        }

        private void listChanged_Preferences(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            try
            {
                if (e.ListChangedType != System.ComponentModel.ListChangedType.ItemChanged) return;

                // string field = Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName;

                string main = "Main";
                if (sender.Equals(Preferences))
                {
                    // Interface.IReport.Msg("A main preference was changed!", "Main Preferences
                    // updated", true);
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
            //throw new NotImplementedException();
        }

        private void currentChanged_SubSamples(object sender, EventArgs e)
        {
            try
            {
                // DataRowView r = Interface.IBS.SubSamples.Current as DataRowView;
                SubSamplesRow r = Interface.ICurrent.SubSample as SubSamplesRow;
                Update(r, true, false);
                // Interface.IBS.SelectedSubSample.Filter =
                // Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName + " = '" + r.SubSampleName
                // + "'"; Interface.IBS.SelectedMatrix.Filter =
                // Interface.IDB.Matrix.SubSampleIDColumn.ColumnName + " = '" + r.SubSamplesID + "'"; Interface.IBS.SelectedSubSample.ResetBindings(true);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /*
        /// <summary>
        /// A binding Current Changed event to update Binding sources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void units_CurrentChanged(object sender, System.EventArgs e)
        {
            try
            {
                Interface.IDB.MatSSF.Clear();

                LINAA.UnitRow unit = Interface.ICurrent.Unit as LINAA.UnitRow;

                // if (unit == null) return;
                //important
                Update<LINAA.SubSamplesRow>(unit?.SubSamplesRow, false, true);

                Update<LINAA.UnitRow>(unit, true, false);

                //then it will be updated
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void units_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            try
            {
                if (e.ListChangedType == System.ComponentModel.ListChangedType.Reset)
                {
                    string column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
                    string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;

                    string filter = column + " = '" + 0.ToString() + "'";

                    // Dumb.BS.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);
                }
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }
        */

        private void updateChannel<T>(T r, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.ChannelsRow u = r as LINAA.ChannelsRow;
            if (findItself && Channels != null)
            {
                string column;
                column = (u.Table as LINAA.ChannelsDataTable).ChannelsIDColumn.ColumnName;
                int id = u.ChannelsID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                Channels.Position = Channels.Find(column, id);
            }
            string filter;

            if (Irradiations != null)
            {
                string chColumn = Interface.IDB.IrradiationRequests.ChannelNameColumn.ColumnName;
                filter = chColumn + " = '" + u.ChannelName + "'";// + " OR " + chColumn + " IS NULL ";
                Irradiations.Filter = filter;
            }
          
            if (SelectedMatrix != null)
            {
                filter = Interface.IDB.Channels.ChannelsIDColumn.ColumnName + " = '" + u.ChannelsID + "'";
                Interface.IBS.SelectedChannel.Filter = filter;
            }
        }

        private void updateIrradiationRequest<T>(T r, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;
            LINAA.IrradiationRequestsRow u = r as LINAA.IrradiationRequestsRow;
            if (findItself && Irradiations != null)
            {
                string column;
                column = (u.Table as LINAA.IrradiationRequestsDataTable).IrradiationRequestsIDColumn.ColumnName;
                int id = u.IrradiationRequestsID;
                Irradiations.Position = Irradiations.Find(column, id);
            }
        }

        private void updateMatrix<T>(T r, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;
            LINAA.MatrixRow u = r as LINAA.MatrixRow;
            if (findItself && Matrix != null)
            {
                string column;
                column = (u.Table as LINAA.MatrixDataTable).MatrixIDColumn.ColumnName;
                int id = u.MatrixID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                Matrix.Position = Matrix.Find(column, id);
            }
            string filter = Interface.IDB.Matrix.MatrixIDColumn.ColumnName + " = '" + u.MatrixID + "'";

            if (SelectedMatrix != null)
            {
                Interface.IBS.SelectedMatrix.Filter = filter;
            }
            if (Compositions != null)
            {
                Interface.IBS.Compositions.Filter = filter;
            }
        }

        private void updateSubSample<T>(T r, bool doCascade, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;
            // LINAA.UnitRow s = r as LINAA.UnitRow; LINAA.UnitRow u = s.UnitRow as LINAA.UnitRow;
            if (SubSamples != null)
            {
                if (findItself && SubSamples != null)
                {
                    string unitValID = (s.Table as LINAA.SubSamplesDataTable).SubSamplesIDColumn.ColumnName;
                    SubSamples.Position = SubSamples.Find(unitValID, s.SubSamplesID);
                }
                if (SelectedSubSample != null)
                {
                    Interface.IBS.SelectedSubSample.Filter = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName + " = '" + s.SubSampleName + "'";
                }
            }
            aChecker = s.CheckUnit;
            DataRow row = s.UnitRow;
            //now update the childs/parents of Units
            if (doCascade)
            {
                Update<UnitRow>(row as UnitRow);
            }
          
        }

        private void updateUnit<T>(T r, bool doCascade, bool findItself)
        {

            if (EC.IsNuDelDetch(r as DataRow) ) return;

            LINAA.UnitRow s = r as LINAA.UnitRow;

            string unitID = string.Empty;
            string filter = string.Empty;

            unitID = s.UnitID.ToString();
            MatSSF.UNIT = s; //this is key

            //the checker Method
            aChecker = s.CheckErrors;

            if (findItself && Units != null)
            {
                string unitValID = (s.Table as LINAA.UnitDataTable).UnitIDColumn.ColumnName;
                Units.Position = Units.Find(unitValID, s.UnitID);
            }
            DataRow row = s.SubSamplesRow;
            //do childs parents or not?
            if (doCascade && !EC.IsNuDelDetch(row))
            {
                row = s.SubSamplesRow.VialTypeRow;
                Update<LINAA.VialTypeRow>(row as VialTypeRow);
                row = s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow;
                Update<LINAA.ChannelsRow>(row as ChannelsRow);
                row = s.SubSamplesRow.MatrixRow;
                Update<LINAA.MatrixRow>(row as MatrixRow);
                row = s.SubSamplesRow.VialTypeRowByChCapsule_SubSamples;
                Update<LINAA.VialTypeRow>(row as VialTypeRow);
       
            }

           LINAA.MatSSFDataTable dt = new MatSSFDataTable(false);
            byte[] arr = s.SSFTable;
            Tables.ReadDTBytes(MatSSF.StartupPath, ref arr, ref dt);
           // MatSSF.ReadXML();

            string column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
            string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;

           // MatSSF.Table = dt;
            filter = column + " = '" + unitID + "'";
            //   MatSSF.Table = dt;
            BS.DeLinkBS(ref SSF);
            BS.LinkBS(ref SSF, dt, filter, sortCol);
        }

        private void updateVialRabbit<T>(T r, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.VialTypeRow u = r as LINAA.VialTypeRow;
            if (findItself)
            {
                string column;
                column = (u.Table as LINAA.VialTypeDataTable).VialTypeIDColumn.ColumnName;
                int id = u.VialTypeID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;

                if (u.IsRabbit && Rabbit != null)
                {
                    Rabbit.Position = Rabbit.Find(column, id);
                }
                else if (Vial != null) Vial.Position = Vial.Find(column, id);
            }
        }
    }
}