using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DB.Tools
{
    public partial class BS
    {
        protected static string CHECKED = "CHECKED";
        protected static string ERROR = "ERROR";
        protected static string NO_ROWS = "No Rows found in the DataGridView.";
        protected static string ROW_OK = "Data for this item seems OK.";
        protected static string ROW_WITH_ERROR = "Data for this item is missing.\nPlease check:\t";
        protected static string SAMPLE = "Sample ";
        protected static string SELECTED = "SELECTED";
        protected static string SELECTED_ROW = " selected for calculations.";
        protected static string UPDATED = "UPDATED";
        protected static string UPDATED_ROW = " values updated with the template item.";
        protected static string WARNING = "WARNING";
    }

    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BS
    {
        public BindingSource MUES;

        public BindingSource Channels;
        public BindingSource Compositions;
        public BindingSource Geometry;
        public BindingSource Irradiations;
        public BindingSource Matrix;
        public BindingSource Monitors;
        public BindingSource MonitorsFlags;
        public BindingSource Orders;
        public BindingSource Preferences;
        public BindingSource Projects;
        public BindingSource XCOMPref;

        ///This is an event handler that fires the function the user GUI provides
        ///the execution is called above on NotyPropertyChanged
        public PropertyChangedEventHandler PropertyChangedHandler = null;

        public BindingSource Rabbit;
        public BindingSource Samples;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedChannel;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedCompositions;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedIrradiation; //dont know if being used really

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedMatrix;

        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedSubSample;

        public BindingSource SSF;
        public BindingSource SSFPreferences;
        public BindingSource Standards;

        //binding sources to attach;
        public BindingSource SubSamples;

        public BindingSource Units;
        public BindingSource Vial;

        protected internal BindingList<BS> bindingList;
        protected internal Hashtable bindings;
        protected internal bool enabledControls = false;
        protected internal Interface Interface;
        protected internal const string ENABLE_CONTROLS_FIELD = "EnabledControls";

        /// <summary>
        /// Notifies the controls the BS is busy calculating
        /// </summary>
        protected internal bool isCalculating = false;

        /// <summary>
        /// checks for errors according to the IRow delegate
        /// </summary>
        protected internal CheckerDelegate hasErrorsMethod = null;

        protected internal delegate bool CheckerDelegate();

        public BindingList<BS> BindingList
        {
            get
            {
                return bindingList;
            }

            set
            {
                bindingList = value;
            }
        }

        protected internal bool showErrors = true;

        public bool ShowErrors
        {
            get
            {
                return showErrors;
            }

            set
            {
                showErrors = value;
            }
        }
    }
}