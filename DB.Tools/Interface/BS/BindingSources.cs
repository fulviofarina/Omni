using System;
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

    public partial class BindingSources
    {
        protected static string ROW_OK = "Data for this item seems OK";
        protected static string ROW_WITH_ERROR = "Crucial data for this item is missing. Please check it";
    }

    public partial class BindingSources
    {
        protected Interface Interface;
        private CheckerDelegate aChecker = null;

        private delegate bool CheckerDelegate();

      
    }

    public partial class BindingSources
    {
        private void addingNew(object sender, AddingNewEventArgs e)
        {
            try
            {
                // DataRow row = null;
                if (sender.Equals(Channels))
                {
                   // ChannelsRow v = e.NewObject as ChannelsRow;//Interface.IDB.Matrix.NewMatrixRow();
                    ChannelsRow v =  Interface.IDB.Channels.NewChannelsRow() as ChannelsRow;
                    Interface.IDB.Channels.AddChannelsRow(v);
                    if (sender.Equals(Channels))
                    {
                    }
                    else
                    {
                        // Matrix.Position = e.NewIndex;
                    }
                    e.NewObject = v;
                    Update(v, false, true);
                }
                else if (sender.Equals(Matrix))
                {
                    MatrixRow v =null;//Interface.IDB.Matrix.NewMatrixRow();
                    v = Interface.IDB.Matrix.NewMatrixRow() as MatrixRow;
                    Interface.IDB.Matrix.AddMatrixRow(v);
                   

                   e.NewObject = v;
                    Update(v, false, true);
                }
                else if (sender.Equals(Vial) || sender.Equals(Rabbit))
                {
                    VialTypeRow v = null;
                 
                    v = Interface.IDB.VialType.NewVialTypeRow() as VialTypeRow;
                    Interface.IDB.VialType.AddVialTypeRow(v);

                    if (sender.Equals(Rabbit)) v.IsRabbit = true;
                    else    v.IsRabbit = false;

                    e.NewObject = v;
                    Update(v, false, true);
                }
                else if (sender.Equals(SubSamples))
                {
                  
                    IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    SubSamplesRow s = Interface.IPopulate.ISamples.AddSamples(ref ir);
                    e.NewObject = s;
                    Update(s, false, true);

                    SelectedSubSample.ResetBindings(false);
                    Units.ResetBindings(false);
                    SelectedMatrix.ResetBindings(false);
                    SelectedCompositions.ResetBindings(false);
                }

                (sender as BindingSource).ResetBindings(false);

               // Samples.

            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void currentChanged(object sender, EventArgs e)
        {
            try
            {
                bool selectedBs = false;
                if (sender.Equals(Irradiations))
                {
                    IrradiationRequestsRow r = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    Update(r, true, false, selectedBs);
                }
                else if (sender.Equals(Channels))
                {
                    ChannelsRow c = Interface.ICurrent.Channel as ChannelsRow;
                    Update(c, true, false, selectedBs);
                }
                else if (sender.Equals(SubSamples))
                {
                    SubSamplesRow r = Interface.ICurrent.SubSample as SubSamplesRow;
                    Update(r, true, false, selectedBs);
                }
                else if (sender.Equals(Matrix) || sender.Equals(SelectedMatrix))
                {
                    MatrixRow c = null;
                    if (sender.Equals(Matrix)) c = Interface.ICurrent.Matrix as MatrixRow;
                    else
                    {
                        selectedBs = true;
                        c = Interface.ICurrent.SubSampleMatrix as MatrixRow;
                    }
                    Update(c, true, false, selectedBs);
                }
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void listChanged_Preferences(object sender, ListChangedEventArgs e)
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