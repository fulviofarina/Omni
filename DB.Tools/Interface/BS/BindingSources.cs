using System;
using Rsx.Dumb;
using Rsx;
using static DB.LINAA;
using System.Windows.Forms;
using System.Data;
using System.Linq;

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
       

        public void StartBinding()
        {
            SubSamples.CurrentChanged += currentChanged_SubSamples;
            //  SubSamples.CurrentItemChanged += SubSamples_CurrentItemChanged;
         //   SubSamples.ListChanged += SubSamples_ListChanged;
            SSFPreferences.ListChanged += listChanged_Preferences;
            Preferences.ListChanged += listChanged_Preferences;
            Channels.CurrentChanged += currentChanged_Channels;
            Irradiations.CurrentChanged += currentChanged_Irradiations;
            Matrix.CurrentChanged += currentChanged_Matrix;
        }

       

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
          
        }

        private void currentChanged_SubSamples(object sender, EventArgs e)
        {
            try
            {
                // DataRowView r = Interface.IBS.SubSamples.Current as DataRowView;
                SubSamplesRow r = Interface.ICurrent.SubSample as SubSamplesRow;
              
             

                Update(r, true, false);
                  }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }
        private void currentChanged_Irradiations(object sender, EventArgs e)
        {
            try
            {
                // DataRowView r = Interface.IBS.SubSamples.Current as DataRowView;
                IrradiationRequestsRow r = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                Update(r, true, false);
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

        private void updateChannel<T>(T r, bool doCascade, bool findItself)
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

          
            if (SelectedIrradiation != null)
            {
                string chColumn = Interface.IDB.IrradiationRequests.ChannelNameColumn.ColumnName;
                filter = chColumn + " = '" + u.ChannelName + "'";// + " OR " + chColumn + " IS NULL ";
                SelectedIrradiation.Filter = filter;
            }
          
            if (SelectedChannel != null)
            {
                filter = Interface.IDB.Channels.ChannelsIDColumn.ColumnName + " = '" + u.ChannelsID + "'";
                Interface.IBS.SelectedChannel.Filter = filter;
            }
        }

        private void updateIrradiationRequest<T>(T r, bool doCascade=true, bool findItself=false)
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

            string filter = string.Empty;
            if (SelectedIrradiation != null)
            {
                string chColumn = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName;
                filter = chColumn + " = '" + u.IrradiationRequestsID + "'";// + " OR " + chColumn + " IS NULL ";
                SelectedIrradiation.Filter = filter;
            }

            if (doCascade)
            {
                int IrrReqID = u.IrradiationRequestsID;

                Interface.IPopulate.ISamples.PopulateSubSamples(IrrReqID);
         
       

                filter = Interface.IDB.SubSamples.IrradiationRequestsIDColumn.ColumnName + " = '" + IrrReqID + "'";

                Interface.IBS.SubSamples.Filter = filter;

                Interface.IBS.SubSamples.ResetBindings(false);

         //.       Interface.IBS.SelectedSubSample.Filter = filter;// Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName + " IS NULL";

          //      Interface.IBS.SelectedSubSample.ResetBindings(true);

                //   Interface.IBS.SelectedMatrix.Filter = Interface.IDB.Matrix.MatrixIDColumn.ColumnName+ " IS NULL";

                //reset the selected bindings.. everything changed..


                filter = Interface.IDB.Unit.IrrReqIDColumn.ColumnName + " = '" + IrrReqID + "'";
          
                Interface.IBS.Units.Filter = filter;
            

                Interface.IPreferences.CurrentPref.LastIrradiationProject = u.IrradiationCode;
                Interface.IPreferences.SavePreferences();
                Interface.IReport.Speak("Loaded project " + u.IrradiationCode);
            }
        }

        private void updateMatrix<T>(T r, bool doCascade=true, bool findItself=false)
        {
            if (EC.IsNuDelDetch(r as DataRow))
            {
             //   SelectedMatrix.RemoveFilter();
              //  Matrix.RemoveFilter();
              // SelectedSubSample.RemoveFilter();
                return;
            }
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

            Compositions.Filter = filter;
            Compositions.Sort = Interface.IDB.Compositions.IDColumn.ColumnName + " desc";




        }

        private void updateSubSample<T>(T r, bool doCascade, bool findItself)
        {
            //delink childs frist!!!

            BS.DeLinkBS(ref SSF);
            if (EC.IsNuDelDetch(r as DataRow))
            {
                //THIS IS SO FUCKING NECESSARY BECAUSE WHEN THE LIST IS EMPTIED
                //THE DATAGRIDVIEW GIVES ERROR
                //UNLESS THE BINDING IS SUSPENDED!
                //DONT DELETE FULVIO FROM FUTURE
            //    SelectedSubSample.SuspendBinding();
                return;
            }
            LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;
            // LINAA.UnitRow s = r as LINAA.UnitRow; LINAA.UnitRow u = s.UnitRow as LINAA.UnitRow;
         //   if (SubSamples != null)
          //  {
                if (findItself )
                {
                    string unitValID = (s.Table as LINAA.SubSamplesDataTable).SubSamplesIDColumn.ColumnName;
                    SubSamples.Position = SubSamples.Find(unitValID, s.SubSamplesID);
                }
            //        if (SelectedSubSample != null)
            // {
            string filColumn = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName;

            SelectedSubSample.Filter = filColumn + " = '" + s.SubSampleName + "'";
              //  }
         //   }
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

        

            if (EC.IsNuDelDetch(r as DataRow))
            {
                
                return;

            }
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

                filter = Interface.IDB.Matrix.MatrixIDColumn.ColumnName + " = '" + s.SubSamplesRow.MatrixID + "'";
                SelectedMatrix.Filter = filter;
                SelectedCompositions.Filter = filter;
      

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
        
            BS.LinkBS(ref SSF, dt, filter, sortCol);
        }

        private void updateVialRabbit<T>(T r, bool doCascade, bool findItself)
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