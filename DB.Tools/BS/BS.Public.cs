using Rsx.Dumb;
using System;
using System.Collections;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{

    //binding sources to attach//

    public partial class BS
    {

        public BindingSource MUES;
        public BindingSource Measurements;
        public BindingSource Channels;
        public BindingSource Compositions;
        public BindingSource Geometry;
        public BindingSource Irradiations;
        public BindingSource Matrix;
        public BindingSource PeaksHL;
        public BindingSource Peaks;
        public BindingSource Gammas;
        public BindingSource Monitors;
        public BindingSource MonitorsFlags;
        public BindingSource Orders;
        public BindingSource Preferences;
        public BindingSource SpecPref;
        public BindingSource Projects;
        public BindingSource XCOMPref;

      
        public BindingSource Rabbit;
        public BindingSource Samples;
        public BindingSource SSF;
        public BindingSource SSFPreferences;
        public BindingSource Standards;

      
        public BindingSource SubSamples;

        public BindingSource Units;
        public BindingSource Vial;



        ///This is an event handler that fires the function the user GUI provides
        ///the execution is called above on NotyPropertyChanged
        public EventHandler EnableControlsChanged = null;


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

      
    }
}