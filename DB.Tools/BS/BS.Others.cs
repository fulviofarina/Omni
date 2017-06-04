using System.Windows.Forms;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>

    public partial class BS
    {
        private void resetPreferencesFilters()
        {
            string col = Interface.IDB.Preferences.WindowsUserColumn.ColumnName;
            Preferences.Filter = col + " = '" + Interface.IPreferences.WindowsUser + "'";
            SSFPreferences.Filter = Preferences.Filter;
        }

        private void resetIrradiationFilters()
        {
            string sort2 = Interface.IDB.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
            Irradiations.Sort = sort2;
        }

        private void resetSampleFilters()
        {
            string sort = Interface.IDB.SubSamples.SubSamplesIDColumn + " asc";
            Interface.IBS.SubSamples.Sort = sort;
            sort = Interface.IDB.Unit.SampleIDColumn.ColumnName + " asc";
            Interface.IBS.Units.Sort = sort;
        }

        private void resetGeometryFilters()
        {
            string column = Interface.IDB.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = Interface.IDB.VialType.InnerRadiusColumn.ColumnName + " asc";
            Rabbit.Filter = column + " = " + "True";
            Vial.Filter = column + " = " + "False";
            Vial.Sort = innerRadCol;
            Rabbit.Sort = innerRadCol;
            Geometry.Filter = string.Empty;
            Geometry.Sort = "CreationDateTime desc";
        }

        private void resetMatrixFilters()
        {
            string sort;
            string filter = "SubSampleID IS NULL";
            Matrix.Filter = filter;
            Matrix.Sort = "MatrixID desc";
            filter = Matrix.Filter + " AND MatrixID = 0";
            SelectedMatrix.Filter = filter;
            sort = Interface.IDB.Compositions.IDColumn.ColumnName + " desc";
            Compositions.Sort = sort;
            Compositions.Filter = filter;
            SelectedCompositions.Filter = filter;
            SelectedCompositions.Sort = sort;
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
    }
}