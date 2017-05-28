using System;
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
   
        private void addingNew(object sender, AddingNewEventArgs e)
        {
            try
            {


             //   EnabledControls = false;

                bool isChannel = sender.Equals(Channels);
                bool aRabbit = sender.Equals(Rabbit);
                bool aVial = sender.Equals(Vial);
                if (isChannel)
                {
                    ChannelsRow c = Interface.IPopulate.IIrradiations.AddNewChannel();
                    e.NewObject = c;
                    Update(c, false, true);
                }
                else if (sender.Equals(Matrix))
                {
                    MatrixRow v = Interface.IPopulate.IGeometry.AddNewMatrix();
                    e.NewObject = v;
                    Update(v, false, true);
                }
                else if (aVial || aRabbit)
                {
                    VialTypeRow v = Interface.IPopulate.IGeometry.AddNewVial(aRabbit);
                    e.NewObject = v;
                    Update(v, false, true);
                }
                else if (sender.Equals(SubSamples))
                {
                    IrradiationRequestsRow ir = Interface.ICurrent.Irradiation as IrradiationRequestsRow;
                    SubSamplesRow s = Interface.IPopulate.ISamples.AddSamples(ref ir);
                    e.NewObject = s;
                    Update(s, false, true);

           //         SelectedSubSample.ResetBindings(false);
               //     Units.ResetBindings(false);
                 //   SelectedMatrix.ResetBindings(false);
                  //  SelectedCompositions.ResetBindings(false);
                }

                notifyPropertyChanged("added");
                IRow row = e.NewObject as IRow;
                row.Check();
               // (sender as BindingSource).ResetBindings(false);

             //   EnabledControls = true;
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
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
                bool? seriousCellsWithErrors = hasErrors?.Invoke();
                // if (row.GetColumnsInError())
                if (seriousCellsWithErrors != null && (bool)seriousCellsWithErrors)
                {
                    Interface.IReport.Msg(ROW_WITH_ERROR, WARNING, false);
                }
            }
            else Interface.IReport.Msg(ROW_OK, CHECKED, true);
            //clean
            hasErrors = null;
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
                    Update(r, true, false, selectedBs);
                }
                else if (sender.Equals(Channels))
                {
                    
                    ChannelsRow c = Interface.ICurrent.Channel as ChannelsRow;
                    Update(c, true, false, selectedBs);
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

             //   bs.RaiseListChangedEvents = true;
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void initializeGeometryBindingSources()
        {
            string rabbit = "Rabbit";
            LINAA set = Interface.Get();
            string name = Interface.IDB.Matrix.TableName;
            Matrix = new BindingSource(set, name);
            bindings.Add(name, Matrix);
            name = Interface.IDB.Compositions.TableName;
            Compositions = new BindingSource(set, name);
            bindings.Add(name, Compositions);

            name = Interface.IDB.VialType.TableName;
            Rabbit = new BindingSource(set, name);
            bindings.Add(name + rabbit, Rabbit);

            name = Interface.IDB.VialType.TableName;
            Vial = new BindingSource(set, name);
            bindings.Add(name, Vial);

            name = Interface.IDB.Geometry.TableName;
            Geometry = new BindingSource(set, name);
            bindings.Add(name, Geometry);
        }

        private void initializePreferencesBindingSources()
        {
            LINAA set = Interface.Get();
            string name = Interface.IDB.Preferences.TableName;
            Preferences = new BindingSource(set, name);
            bindings.Add(name, Preferences);

            name = Interface.IDB.SSFPref.TableName;
            SSFPreferences = new BindingSource(set, name);
            bindings.Add(name, SSFPreferences);
        }

        private void initializeProjectBindingSources()
        {
            LINAA set = Interface.Get();

            string name = Interface.IDB.Channels.TableName;
            Channels = new BindingSource(set, name);
            bindings.Add(name, Channels);

            name = Interface.IDB.IrradiationRequests.TableName;
            Irradiations = new BindingSource(set, name);
            bindings.Add(name, Irradiations);

            name = Interface.IDB.Projects.TableName;
            Projects = new BindingSource(set, name);
            bindings.Add(name, Projects);

            name = Interface.IDB.Orders.TableName;
            Orders = new BindingSource(set, name);
            bindings.Add(name, Orders);
        }
        private void initializeSampleBindingSources()
        {
            LINAA set = Interface.Get();
            string name = Interface.IDB.Standards.TableName;
            Standards = new BindingSource(set, name);
            bindings.Add(name, Standards);

            name = Interface.IDB.Monitors.TableName;
            Monitors = new BindingSource(set, name);
            bindings.Add(name, Monitors);

            name = Interface.IDB.MonitorsFlags.TableName;
            MonitorsFlags = new BindingSource(set, name);
            bindings.Add(name, MonitorsFlags);

            name = Interface.IDB.Samples.TableName;
            Samples = new BindingSource(set, name);
            bindings.Add(name, Samples);

            name = Interface.IDB.SubSamples.TableName;
            SubSamples = new BindingSource(set, name);
            bindings.Add(name, SubSamples);

            name = Interface.IDB.Unit.TableName;
            Units = new BindingSource(set, name);
            bindings.Add(name, Units);

            // Units.CurrentChanged += units_CurrentChanged;
            name = Interface.IDB.MatSSF.TableName;
            SSF = new BindingSource(set, name);
            bindings.Add(name, SSF);
        }

        private void initializeSelectedBindingSources()
        {
            LINAA set = Interface.Get();

            string name = Interface.IDB.Channels.TableName;
            SelectedChannel = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedChannel);

            name = Interface.IDB.Compositions.TableName;
            SelectedCompositions = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedCompositions);
            name = Interface.IDB.Matrix.TableName;
            SelectedMatrix = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedMatrix);

            // Units.BindingComplete += Units_BindingComplete;
            name = Interface.IDB.SubSamples.TableName;
            SelectedSubSample = new BindingSource(set, name);
            bindings.Add("Selected" + name, SelectedSubSample);

            name = Interface.IDB.IrradiationRequests.TableName;
            SelectedIrradiation = new BindingSource(set, name);
            bindings.Add("Selected" + name, Irradiations);
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