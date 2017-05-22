using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

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
        protected Interface Interface;
        private CheckerDelegate aChecker = null;
        private delegate bool CheckerDelegate();

        // private string rowWithError = "The selected row has some incomplete cells or cells with
        // errors.\nPlease fix before selecting it";
    }

    public partial class BindingSources
    {
        private void updateChannel<T>(T r, bool doCascade, bool findItself)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            LINAA.ChannelsRow u = r as LINAA.ChannelsRow;

            int id = u.ChannelsID;

            if (findItself && Channels != null)
            {
                string column;
                column = (u.Table as LINAA.ChannelsDataTable).ChannelsIDColumn.ColumnName;
                //BindingSource rabbitBS = Interface.IBS.Rabbit;
                Channels.Position = Channels.Find(column, id);
            }
            string filter;

            //THIS BINDING SOURCE IS NOT EVEN USED BUT COULD BE
            string col = Interface.IDB.IrradiationRequests.ChannelsIDColumn.ColumnName;
            if (SelectedIrradiation != null)
            {
                filter = col + " = '" + id + "'";// + " OR " + chColumn + " IS NULL ";
                SelectedIrradiation.Filter = filter;
            }

            if (SelectedChannel != null)
            {
                col = Interface.IDB.Channels.ChannelsIDColumn.ColumnName;
                filter = col + " = '" + id + "'";
                Interface.IBS.SelectedChannel.Filter = filter;
            }
        }

        private void updateIrradiationRequest<T>(T r, bool doCascade = true, bool findItself = false)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;
            LINAA.IrradiationRequestsRow u = r as LINAA.IrradiationRequestsRow;
            string column;
            column = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName;
            int id = u.IrradiationRequestsID;
            if (findItself && Irradiations != null)
            {
                Irradiations.Position = Irradiations.Find(column, id);
            }

            string filter = string.Empty;
            if (SelectedIrradiation != null)
            {
                filter = column + " = '" + id + "'";// + " OR " + chColumn + " IS NULL ";
                SelectedIrradiation.Filter = filter;
            }

            if (doCascade)
            {
                //reset the selected bindings.. everything changed..
                column = Interface.IDB.Unit.IrrReqIDColumn.ColumnName;
                filter = column + " = '" + id + "'";
                Interface.IBS.Units.Filter = filter;

                Interface.IPopulate.ISamples.PopulateSubSamples(id);
                column = Interface.IDB.SubSamples.IrradiationRequestsIDColumn.ColumnName;
                filter = column + " = '" + id + "'";
                Interface.IBS.SubSamples.Filter = filter;
           //     Interface.IBS.SubSamples.ResetBindings(false);
               
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
            BindingSource bs = null;
            if (selectedBS) bs = SelectedMatrix;
            else bs = Matrix;
            LINAA.MatrixRow u = r as LINAA.MatrixRow;
            string column;
            column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;
            int id = u.MatrixID;
            if (findItself && Matrix != null)
            {
                bs.Position = bs.Find(column, id);
            }
            string filter = column + " = '" + id + "'";

            if (selectedBS) SelectedCompositions.Filter = filter;
            else Compositions.Filter = filter;
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
            }

            string filColumn = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName;

            SelectedSubSample.Filter = filColumn + " = '" + s.SubSampleName + "'";

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
                // row = s.SubSamplesRow.MatrixRow;

                filter = Interface.IDB.Matrix.SubSampleIDColumn.ColumnName + " = '" + s.SubSamplesRow.SubSamplesID + "'";
                SelectedMatrix.Filter = filter;

                // SelectedCompositions.ResetItem(SelectedCompositions.Position); SelectedMatrix.ResetItem(SelectedMatrix.Position);

                row = s.SubSamplesRow.VialTypeRowByChCapsule_SubSamples;
                Update<LINAA.VialTypeRow>(row as VialTypeRow);
            }

            string column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
            string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;

            // MatSSF.Table = dt;
            filter = column + " = '" + unitID + "'";
            // MatSSF.Table = dt;

            BS.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);
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

    public partial class BindingSources
    {
        private void addingNew(object sender, AddingNewEventArgs e)
        {


            try
            {
                DataRow row = null;
                if (sender.Equals(Channels))
                {
                    LINAA.ChannelsRow v = e.NewObject as LINAA.ChannelsRow;//Interface.IDB.Matrix.NewMatrixRow();
                    v = Interface.IDB.Channels.NewChannelsRow() as LINAA.ChannelsRow;
                    Interface.IDB.Channels.AddChannelsRow(v);
                    //      e.NewObject = v;

                    if (sender.Equals(Channels))
                    {
                        // SelectedMatrix.Position = e.NewIndex; v = Interface.ICurrent.SubSampleMatrix as LINAA.MatrixRow;
                    }
                    else
                    {
                        // Matrix.Position = e.NewIndex;
                    }
                }
                else if (sender.Equals(Matrix) || sender.Equals(SelectedMatrix))
                {
                    LINAA.MatrixRow v = e.NewObject as LINAA.MatrixRow;//Interface.IDB.Matrix.NewMatrixRow();
                    v = Interface.IDB.Matrix.NewMatrixRow() as LINAA.MatrixRow;
                    Interface.IDB.Matrix.AddMatrixRow(v);
                    //    e.NewObject = v;

                    if (sender.Equals(SelectedMatrix))
                    {
                        // SelectedMatrix.Position = e.NewIndex; v = Interface.ICurrent.SubSampleMatrix as LINAA.MatrixRow;
                    }
                    else
                    {
                        // Matrix.Position = e.NewIndex;
                    }

                    row = v;
                }
                else if (sender.Equals(Vial) || sender.Equals(Rabbit))
                {
                    LINAA.VialTypeRow v = null;
                    // IEnumerable<LINAA.VialTypeRow> enums = Interface.IDB.VialType.Where(o => o.RowState == DataRowState.Added);
                    v = Interface.IDB.VialType.NewVialTypeRow() as LINAA.VialTypeRow;
                    Interface.IDB.VialType.AddVialTypeRow(v);

                    if (sender.Equals(Rabbit))
                    {
                        // bs.Position = e.NewIndex; v = Caster.Cast<VialTypeRow>(e.NewObject);
                        v.IsRabbit = true;
                    }
                    else

                    {
                        // bs.Position = e.NewIndex; v = Caster.Cast<VialTypeRow>(bs.Current);
                        v.IsRabbit = false;
                    }

                    row = v;
                }
                else if (sender.Equals(SubSamples))
                {
             
                    IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as LINAA.IrradiationRequestsRow;
                    string project = ir.IrradiationCode;
                    LINAA.SubSamplesRow s = Interface.IPopulate.ISamples.AddSamples(project);
                   
                    row = s;


                }
                e.NewObject = row;
               Update(row, false, true);

            }
            catch   (SystemException ex)
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

        private void currentChanged_Matrix(object sender, EventArgs e)
        {
            try
            {
                //
                LINAA.MatrixRow c = null;
                bool selectedBS = false;
                if (sender.Equals(Matrix))
                {
                    c = Interface.ICurrent.Matrix as LINAA.MatrixRow;
                    // selectedBS = false;
                }
                else
                {
                    selectedBS = true;
                    c = Interface.ICurrent.SubSampleMatrix as LINAA.MatrixRow;
                }
                Update(c, true, false, selectedBS);
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

        private void listChanged_Preferences(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            try
            {
                if (e.ListChangedType != System.ComponentModel.ListChangedType.ItemChanged) return;

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