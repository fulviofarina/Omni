﻿using System;
using System.ComponentModel;
using System.Data;
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
        private UnitRow selectUnitOrChildRow(ref DataRow row)
        {
            bool isUnuit = row.GetType().Equals(typeof(UnitRow));
            UnitRow unit = null;

            string title = SAMPLE;// + unit.Name;
            if (isUnuit)
            {
                unit = row as UnitRow;
                title += unit.Name;
                unit.ToDo = !unit.ToDo;
                Interface.IReport.Msg(title + SELECTED_ROW, SELECTED); //report
            }
            else
            {
                //NO UNIT, maybe a matrix, a vial, a rabbit or a channel
                unit = Interface.ICurrent.Unit as UnitRow;
                title += unit.Name;
                EnabledControls = false;
                unit.SetParent(ref row);
                Interface.IReport.Msg(title + UPDATED_ROW, UPDATED); //report
                EnabledControls = true;
                //bring back to VIEW (Select)
            }

            return unit;
        }

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
            // Action Checker = null; DataColumn[] columnsThatShouldBeOk = null;
            if (isSubSample)
            {
                SubSamplesRow s = r as SubSamplesRow;
                hasErrorsMethod += s.HasBasicErrors;
                hasErrorsMethod += s.UnitRow.HasErrors;
            }
            else if (isUnit)
            {
                UnitRow u = r as UnitRow;
                hasErrorsMethod += u.HasErrors;
                hasErrorsMethod += u.SubSamplesRow.HasBasicErrors;
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
                    MatrixRow v = Interface.IPopulate.IGeometry.AddNewMatrix();
                    e.NewObject = v;
                    update(v, false, true);
                }
                else if (aVial || aRabbit)
                {
                    VialTypeRow v = Interface.IPopulate.IGeometry.AddNewVial(aRabbit);
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
        private void hasCompulsoryErrors<T>(T r)
        {
            if (Rsx.EC.IsNuDelDetch(r as DataRow)) return;
            DataRow row = r as DataRow;
            if (row.HasErrors)
            {
                bool? seriousCellsWithErrors = hasErrorsMethod?.Invoke();
                // if (row.GetColumnsInError())
                if (seriousCellsWithErrors != null && (bool)seriousCellsWithErrors)
                {
                    Interface.IReport.Msg(ROW_WITH_ERROR, WARNING, false);
                }
            }
            else Interface.IReport.Msg(ROW_OK, CHECKED, true);
            //clean
            hasErrorsMethod = null;
        }

        private void currentChanged(object sender, EventArgs e)
        {
            try
            {
                BindingSource bs = sender as BindingSource;

                bool selectedBs = false;
                if (sender.Equals(Irradiations))
                {
                    IrradiationRequestsRow r = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    update(r, true, false, selectedBs);
                }
                else if (sender.Equals(Channels))
                {
                    ChannelsRow c = Interface.ICurrent.Channel as ChannelsRow;
                    update(c, true, false, selectedBs);
                }
                else if (sender.Equals(Rabbit))
                {
                    VialTypeRow c = Interface.ICurrent.Rabbit as VialTypeRow;
                    update(c, true, false, selectedBs);
                }
                else if (sender.Equals(Vial ))
                {
                    VialTypeRow c = Interface.ICurrent.Vial as VialTypeRow;
                    update(c, true, false, selectedBs);
                }
                else if (sender.Equals(SubSamples) || sender.Equals(Units))
                {
                    SubSamplesRow r = null;
                    if (sender.Equals(SubSamples))
                    {
                        r = Interface.ICurrent.SubSample as SubSamplesRow;
                    }
                    else
                    {
                        UnitRow u = Interface.ICurrent.Unit as UnitRow;
                        if (!EC.IsNuDelDetch(u.SubSamplesRow)) r = u.SubSamplesRow;
                    }
                    update(r, true, false, selectedBs);
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
                    update(c, true, false, selectedBs);
                }

                // bs.RaiseListChangedEvents = true;
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