using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{


    public partial class BindingSources
    {
        private void updateChannel<T>(T r, bool doCascade, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.ChannelsRow u = r as LINAA.ChannelsRow;

            int id = u.ChannelsID;

            if (findItself )
            {
                string column;
                column = (u.Table as LINAA.ChannelsDataTable).ChannelsIDColumn.ColumnName;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                Channels.Position = Channels.Find(column, id);
             //   Channels.ResetBindings(false);

            }
            string filter;

            //THIS BINDING SOURCE IS NOT EVEN USED BUT COULD BE
            string col = Interface.IDB.IrradiationRequests.ChannelsIDColumn.ColumnName;

            filter = col + " = '" + id + "'";// + " OR " + chColumn + " IS NULL ";
            SelectedIrradiation.Filter = filter;
          //  SelectedIrradiation.ResetBindings(false);

            col = Interface.IDB.Channels.ChannelsIDColumn.ColumnName;
            filter = col + " = '" + id + "'";
            SelectedChannel.Filter = filter;
         
          //  SelectedChannel.ResetBindings(false);
        }

        private void updateIrradiationRequest<T>(T r, bool doCascade = true, bool findItself = false)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;
            LINAA.IrradiationRequestsRow u = r as LINAA.IrradiationRequestsRow;
            string column;
            column = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName;
            int id = u.IrradiationRequestsID;
            if (findItself )
            {
                Irradiations.Position = Irradiations.Find(column, id);
            //    Irradiations.ResetBindings(false);
            }
            string filter = string.Empty;
            filter = column + " = '" + id + "'";// + " OR " + chColumn + " IS NULL ";
            SelectedIrradiation.Filter = filter;

          //  SelectedIrradiation.ResetBindings(false);

            if (doCascade)
            {
                //reset the selected bindings.. everything changed..
                column = Interface.IDB.Unit.IrrReqIDColumn.ColumnName;
                filter = column + " = '" + id + "'";
                Units.Filter = filter;

            //    Units.ResetBindings(false);

                Interface.IPopulate.ISamples.PopulateSubSamples(id);
                column = Interface.IDB.SubSamples.IrradiationRequestsIDColumn.ColumnName;
                filter = column + " = '" + id + "'";
                Interface.IBS.SubSamples.Filter = filter;

              //  SubSamples.ResetBindings(false);

                Interface.IPreferences.CurrentPref.LastIrradiationProject = u.IrradiationCode;
                Interface.IPreferences.SavePreferences();
                Interface.IReport.Speak("Loaded project " + u.IrradiationCode);
            }


          
        }

        private void updateMatrix<T>(T r, bool doCascade = true, bool findItself = false, bool selectedBS = false)
        {
            if (EC.IsNuDelDetch(r as DataRow))
            {
                // SelectedMatrix.RemoveFilter(); Matrix.RemoveFilter(); SelectedSubSample.RemoveFilter();
                return;
            }

            LINAA.MatrixRow u = r as LINAA.MatrixRow;
            int id = u.MatrixID;

            string column;
            column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
            string filter = column + " = '" + id + "'";

            BindingSource bs = null;
            if (selectedBS)
            {
                bs = SelectedMatrix;
            //    filter = Interface.IDB.Matrix.MatrixIDColumn.ColumnName + " = '" + id + "'";
               // bs.Filter = filter;
            }
            else bs = Matrix;
          
            if (findItself )
            {
                bs.Position = bs.Find(column, id);
               // bs.ResetBindings(false);
            }

                    
            BindingSource bs2 = Compositions;
            if (selectedBS)
            {
                bs2 = SelectedCompositions;
            }

            bs2.Filter = filter;
          //  bs2.ResetBindings(false);

        }

        private void updateSubSample<T>(T r, bool doCascade, bool findItself)
        {
            //delink childs frist!!!
            //  BS.DeLinkBS(ref SSF);

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

            if (findItself)
            {
                string unitValID = (s.Table as LINAA.SubSamplesDataTable).SubSamplesIDColumn.ColumnName;
                SubSamples.Position = SubSamples.Find(unitValID, s.SubSamplesID);
            //    SubSamples.ResetBindings(false);

            }

            if (!s.IsSubSampleNameNull())
            {
                string filColumn = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName;
                SelectedSubSample.Filter = filColumn + " = '" + s.SubSampleName + "'";
           //     SelectedSubSample.ResetBindings(false);
            }

        //    aChecker = s.CheckUnit;
            DataRow row = s.UnitRow;
            //now update the childs/parents of Units
            if (doCascade)
            {
                Update<UnitRow>(row as UnitRow);
            }
        }

        private void updateUnit<T>(T r, bool doCascade, bool findItself)
        {
            BS.DeLinkBS(ref SSF);

            if (EC.IsNuDelDetch(r as DataRow))
            {
                return;
            }
            LINAA.UnitRow s = r as LINAA.UnitRow;

            string unitID = string.Empty;
            string filter = string.Empty;

            unitID = s.UnitID.ToString();
            // MatSSF.UNIT = s; //this is key

          

            if (findItself )
            {
                string unitValID = (s.Table as LINAA.UnitDataTable).UnitIDColumn.ColumnName;
                Units.Position = Units.Find(unitValID, s.UnitID);

             //   Units.ResetBindings(false);

            }

            DataRow row = s.SubSamplesRow;
            //do childs parents or not?
            if (doCascade && !EC.IsNuDelDetch(row))
            {
                row = s.SubSamplesRow.VialTypeRow;
                Update<LINAA.VialTypeRow>(row as VialTypeRow);
                row = s.SubSamplesRow.ChCapsuleRow;
                Update<LINAA.VialTypeRow>(row as VialTypeRow);
                row = s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow;
                Update<LINAA.ChannelsRow>(row as ChannelsRow,true,true);
                row = s.SubSamplesRow.MatrixRow;
            

                filter = Interface.IDB.Matrix.SubSampleIDColumn.ColumnName + " = '" + s.SubSamplesRow.SubSamplesID + "'";
                    SelectedMatrix.Filter = filter;

                Update<MatrixRow>(row as MatrixRow, true, false, false);
                row = s.SubSamplesRow.GetMatrixRows().FirstOrDefault(o=> o.MatrixID == s.SubSamplesRow.MatrixID);
                Update<MatrixRow>(row as MatrixRow, true, true, true);

                //    SelectedMatrix.ResetBindings(false);
                // SelectedCompositions.ResetItem(SelectedCompositions.Position); SelectedMatrix.ResetItem(SelectedMatrix.Position);



            }

            string column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
            string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;

            // MatSSF.Table = dt;
            filter = column + " = '" + unitID + "'";
            BS.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);
        //    SSF.ResetBindings(false);
        }

        private void updateVialRabbit<T>(T r, bool doCascade, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.VialTypeRow u = r as LINAA.VialTypeRow;

            BindingSource bs = Rabbit;

            if (!u.IsRabbit)
            {
                bs = Vial;
            }

            if (findItself)
            {
                string column;
                column = Interface.IDB.VialType.VialTypeIDColumn.ColumnName;
                int id = u.VialTypeID;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                bs.Position = bs.Find(column, id);
            }

       //     bs.ResetBindings(false);
        }
    }

    
}