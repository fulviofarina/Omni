using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>

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
                    currentChanged(c, false, true);
                }
                else if (sender.Equals(Matrix))
                {
                    MatrixRow v = Interface.IPopulate.IGeometry.AddMatrix();
                    e.NewObject = v;
                    currentChanged(v, false, true);
                }
                else if (aVial || aRabbit)
                {
                    VialTypeRow v = Interface.IPopulate.IGeometry.AddVial(aRabbit);
                    e.NewObject = v;
                    currentChanged(v, false, true);
                }
                else if (sender.Equals(SubSamples))
                {
                    IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    SubSamplesRow s = Interface.IPopulate.ISamples.AddSamples(ref ir);
                    e.NewObject = s;
                    currentChanged(s, false, true);

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
            PropertyChangedHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Checks a row for serious errors according to the NonNullable columns or specialized array
        /// of forbidden columns
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        protected bool hasCompulsoryErrors<T>(T r, string[] colsinError = null, bool accumulate = false)
        {
            if (EC.IsNuDelDetch(r as DataRow))
            {
                return true;
            }
            DataRow row = r as DataRow;
            string tableName = row.Table.TableName;
            string msg = ROW_OK;
            string title = CHECKED;
            bool ok = true;
            if (row.HasErrors)
            {
                bool? seriousCellsWithErrors = hasErrorsMethod?.Invoke();
                // if (row.GetColumnsInError())
                if (seriousCellsWithErrors != null && (bool)seriousCellsWithErrors)
                {
                    //in case I send my own set of cols with errores
                    if (colsinError == null)
                    {
                        colsinError = row.GetColumnsInError().Select(o => o.ColumnName).ToArray();
                    }
                    string result = Interface.GetDisplayColumName(tableName, colsinError);
                    ok = false;
                    msg = ROW_WITH_ERROR + result;
                    title = WARNING;
                }
            }
            //get display name
            Interface.GetDisplayTableName(ref tableName);
            //display name
            msg = msg.Replace("item", tableName);
            Interface.IReport.Msg(msg, title, ok, accumulate);
            //clean
            hasErrorsMethod = null;
            return ok;
        }

        private void currentChanged(object sender, EventArgs e)
        {
            BindingSource bs = sender as BindingSource;
            CurrentChanged(ref bs);
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