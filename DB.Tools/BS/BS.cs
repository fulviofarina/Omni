using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>

    public partial class BS
    {
 

        /// <summary>
        /// Checks a row for serious errors according to the NonNullable columns or specialized array
        /// </summary>
        private void hasErrors<T>(T r)
        {
            if (EC.IsNuDelDetch(r as DataRow)) return;

            Type tipo = typeof(T);

            bool isSubSample = tipo.Equals(typeof(SubSamplesRow));
            bool isUnit = tipo.Equals(typeof(UnitRow));
            bool isMatrix = tipo.Equals(typeof(MatrixRow));
            //to check later the columns that should be ok

            hasErrorsMethod = null;

            IRow irow = r as IRow;

            hasErrorsMethod += irow.HasErrors;

            if (isSubSample)
            {
                SubSamplesRow s = r as SubSamplesRow;

              //  hasErrorsMethod += s.HasErrors;
                 hasErrorsMethod += s.UnitRow.HasErrors;
            }
            else if (isUnit)
            {
                UnitRow u = r as UnitRow;
              //  hasErrorsMethod += u.HasErrors;
                hasErrorsMethod += u.SubSamplesRow.HasErrors;
            }


            /*
            // Action Checker = null; DataColumn[] columnsThatShouldBeOk = null;
            if (isSubSample)
            {
                SubSamplesRow s = r as SubSamplesRow;

                hasErrorsMethod += s.HasErrors;
          //      hasErrorsMethod += s.UnitRow.HasErrors;
            }
            else if (isUnit)
            {
                UnitRow u = r as UnitRow;
                hasErrorsMethod += u.HasErrors;
           //     hasErrorsMethod += u.SubSamplesRow.HasErrors;
            }
            else if (isMatrix)
            {
                MatrixRow m = r as MatrixRow;
                hasErrorsMethod += m.HasErrors;
            }
            else if (tipo.Equals(typeof(VialTypeRow)))
            {
                //
                VialTypeRow v = r as VialTypeRow;
                hasErrorsMethod += v.HasErrors;
            }
            else if (tipo.Equals(typeof(ChannelsRow)))
            {
                ChannelsRow c = r as ChannelsRow;
                hasErrorsMethod += c.HasErrors;

                // updateChannel(r, doCascade, findItself);
            }
            else if (tipo.Equals(typeof(IrradiationRequestsRow)))
            {
                //
                IrradiationRequestsRow i = r as IrradiationRequestsRow;
                hasErrorsMethod += i.HasErrors;
                // updateIrradiationRequest(r, doCascade, findItself);
            }
            */
            //now check the errors!!!
            hasCompulsoryErrors(r);
        }

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
                    update(c, false, true);
                }
                else if (sender.Equals(Matrix))
                {
                    MatrixRow v = Interface.IPopulate.IGeometry.AddMatrix();
                    e.NewObject = v;
                    update(v, false, true);
                }
                else if (aVial || aRabbit)
                {
                    VialTypeRow v = Interface.IPopulate.IGeometry.AddVial(aRabbit);
                    e.NewObject = v;
                    update(v, false, true);
                }
                else if (sender.Equals(SubSamples))
                {
                    IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    SubSamplesRow s = Interface.IPopulate.ISamples.AddSamples(ref ir);
                    e.NewObject = s;
                    update(s, false, true);

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
        protected void hasCompulsoryErrors<T>(T r)
        {
            if (Rsx.EC.IsNuDelDetch(r as DataRow)) return;
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
                    string result = GetDisplayColumName(ref row, tableName);
                    ok = false;
                    msg = ROW_WITH_ERROR + result;
                    title = WARNING;
                   
                }
            }
         
            GetDisplayTableName(ref tableName);
            //display name
            msg = msg.Replace("item", tableName);
            Interface.IReport.Msg(msg, title, ok);
            //clean
            hasErrorsMethod = null;
        }
        private  void GetDisplayTableName(ref string tableName)
        {
            if (tableName.Contains(Interface.IDB.VialType.TableName))
            {
                tableName = "Container";
            }
            else if (tableName.Contains(Interface.IDB.Channels.TableName))
            {
                tableName = "Neutron Source";
            }
            else if (tableName.Contains(Interface.IDB.Unit.TableName))
            {
                tableName = "Sample";
            }
            else if (tableName.Contains(Interface.IDB.SubSamples.TableName))
            {
                tableName = "Sample";
            }


        }
        private static string GetDisplayColumName(ref DataRow row, string tableName)
        {
            string[] colsInError = row.GetColumnsInError().Select(o => o.ColumnName).ToArray();

            for (int i = 0; i < colsInError.Count(); i++)
            {
                string col = colsInError[i];
                if (col.Contains(tableName))
                {
                    colsInError[i] = colsInError[i].Replace(tableName, null);
                }
                if (col.Contains("FillHeight"))
                {
                    colsInError[i] = "Length";

                }
                else if (col.Contains("Radius"))
                {
                    colsInError[i] = "Radius";
                }
                else if (col.Contains("Name") || col.Contains("Ref"))
                {
                    colsInError[i] = "Label";
                }
                else if (col.Contains("Gross"))
                {
                    colsInError[i] = "Mass";
                }
                
               
            }
            string sep = ",\n\t\t\t\t";
            string result = colsInError.Aggregate((o, next) => o=o+ sep + next);
            result = "\t" + result;
            return result;
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
              //  if (e.ListChangedType != System.ComponentModel.ListChangedType.ItemChanged) return;

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