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
        // private Interface Interface;
        private string rowWithError = "The selected row has some incomplete cells or cells with errors.\nPlease fix before selecting it";


    }
    public partial class BindingSources
    {
        private Interface Interface;
        // private string rowWithError = "The selected row has some incomplete cells or cells with errors.\nPlease fix before selecting it";
    }

    public partial class BindingSources
    {

        /// <summary>
        /// A binding Current Changed event to update Binding sources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void units_CurrentChanged(object sender, System.EventArgs e)
        {
            try
            {

                Interface.IDB.MatSSF.Clear();

                LINAA.UnitRow unit = Interface.ICurrent.Unit as LINAA.UnitRow;

               // if (unit == null) return;
                //important
                Update<LINAA.SubSamplesRow>(unit?.SubSamplesRow, false,true);

                Update<LINAA.UnitRow>(unit, true, false);

                //then it will be updated
            }
            catch (Exception ex)
            {

                Interface.IMain.AddException(ex);
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
               
               //     Dumb.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);
                }
            }
            catch (Exception ex)
            {

                Interface.IMain.AddException(ex);
            }


        }


        private void preferences_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            try
            {
                if (e.ListChangedType != System.ComponentModel.ListChangedType.ItemChanged) return;

                
                
                  //  string field = Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName;

                    string main = "Main";
                    if (sender.Equals(Preferences))
                    {

                        //     Interface.IReport.Msg("A main preference was changed!", "Main Preferences updated", true);
                    }
                    else
                    {
                        main = "SSF";

                    }
                    Interface.IReport.Msg("A " + main + " preference was updated", main + " Preferences updated", true);
                
            }
            catch (Exception ex)
            {

                Interface.IMain.AddException(ex);
            }
            //throw new NotImplementedException();
        }
        private void updateIrradiationRequest<T>(T r, bool findItself)
        {
            LINAA.IrradiationRequestsRow u = r as LINAA.IrradiationRequestsRow;
            if (findItself && Irradiations != null)
            {
                string column;
                column = (u.Table as LINAA.IrradiationRequestsDataTable).IrradiationRequestsIDColumn.ColumnName;
                int id = u.IrradiationRequestsID;
                Irradiations.Position = Irradiations.Find(column, id);
            }
        }

        private void updateChannel<T>(T r, bool findItself)
        {
            LINAA.ChannelsRow u = r as LINAA.ChannelsRow;
            if (findItself && Channels != null)
            {
                string column;
                column = (u.Table as LINAA.ChannelsDataTable).ChannelsIDColumn.ColumnName;
                int id = u.ChannelsID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                Channels.Position = Channels.Find(column, id);
            }
        }

        private void updateMatrix<T>(T r, bool findItself)
        {
            LINAA.MatrixRow u = r as LINAA.MatrixRow;
            if (findItself && Matrix != null)
            {
                string column;
                column = (u.Table as LINAA.MatrixDataTable).MatrixIDColumn.ColumnName;
                int id = u.MatrixID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                Matrix.Position = Matrix.Find(column, id);
            }
        }

        private void updateVialRabbit<T>(T r, bool findItself)
        {
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

        private void updateUnit<T>(T r, bool doCascade, bool findItself)
        {

            string unitID = string.Empty;
            string filter = string.Empty;
           
              

                LINAA.UnitRow s = r as LINAA.UnitRow;
                unitID = s.UnitID.ToString();
                MatSSF.UNIT = s; //this is key

                if (findItself && Units != null)
                {
                    string unitValID = (s.Table as LINAA.UnitDataTable).UnitIDColumn.ColumnName;
                    Units.Position = Units.Find(unitValID, s.UnitID);
                }

                //do childs parents or not?
                if (doCascade && s.SubSamplesRow != null)
                {
                    Update<LINAA.VialTypeRow>(s.SubSamplesRow.VialTypeRow);
                    Update<LINAA.ChannelsRow>(s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow);
                    Update<LINAA.MatrixRow>(s.SubSamplesRow.MatrixRow);
                    Update<LINAA.VialTypeRow>(s.SubSamplesRow.VialTypeRowByChCapsule_SubSamples);
                }


            string column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
            string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;

            filter = column + " = '" + unitID + "'";
            Dumb.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);


            MatSSF.ReadXML();
      

        }

        private void updateSubSample<T>(T r, bool doCascade, bool findItself)
        {
            LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;
            // LINAA.UnitRow s = r as LINAA.UnitRow; LINAA.UnitRow u = s.UnitRow as LINAA.UnitRow;
            if (findItself && SubSamples != null)
            {
                string unitValID = (s.Table as LINAA.SubSamplesDataTable).SubSamplesIDColumn.ColumnName;
                SubSamples.Position = SubSamples.Find(unitValID, s.SubSamplesID);
            }
            //now update the childs/parents of Units
            if (doCascade && s.UnitRow != null) Update<LINAA.UnitRow>(s.UnitRow);
        }

        private void channels_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                string sortColumn;
                string filter;
                sortColumn = Interface.IDB.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
                //
                LINAA.ChannelsRow c = Dumb.Cast<LINAA.ChannelsRow>(Interface.IBS.Channels.Current);

                string chColumn = Interface.IDB.IrradiationRequests.ChannelNameColumn.ColumnName;
                filter = chColumn + " = '" + c.ChannelName + "'";// + " OR " + chColumn + " IS NULL ";

                Interface.IBS.Irradiations.Filter = filter;
                Interface.IBS.Irradiations.Sort = sortColumn;
            }
            catch (Exception ex)
            {

                Interface.IMain.AddException(ex);
            }
            //  Dumb.LinkBS(ref Interface.IBS.Irradiations, Interface.IDB.IrradiationRequests, filter, sortColumn + " desc");
        }
        private void subSamples_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                // DataRowView r = Interface.IBS.SubSamples.Current as DataRowView;
                SubSamplesRow r = Interface.ICurrent.SubSample as SubSamplesRow;
                Interface.IBS.Update(r, true, false);
                Interface.IBS.SelectedSubSample.Filter = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName + " = '" + r.SubSampleName + "'";
              //  Interface.IBS.SelectedSubSample.ResetBindings(true);
            }
            catch (Exception ex)
            {

                Interface.IMain.AddException(ex);
            }
        }

    }
}