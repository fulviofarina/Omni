using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{


    public partial class BS
    {
        protected static string CHECKED = "CHECKED";
        protected static string ROW_OK = "Data for this item seems OK";
        protected static string ROW_WITH_ERROR = "Crucial data for this item is missing. Please check it";
        protected static string WARNING = "WARNING";
    }


    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BS
    {
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
        public BindingSource Rabbit;
        public BindingSource Samples;
        /// <summary>
        /// Selected only
        /// </summary>
        public BindingSource SelectedChannel;
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
        protected Hashtable bindings;
        protected Interface Interface;
        private CheckerDelegate hasErrors = null;
        private delegate bool CheckerDelegate();
        protected bool enabledControls = false;
        protected BindingList<BS> bindingList;
        protected bool isCalculating = false;

        
    }
}