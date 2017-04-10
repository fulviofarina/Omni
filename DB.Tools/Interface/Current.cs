using System.Collections.Generic;
using System.Data;
using System.Linq;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>
    public partial class Current : IPreferences
    {
        private PreferencesRow currentPref;

        private SSFPrefRow currentSSFPref;

        public PreferencesRow CurrentPref
        {
            get
            {
                currentPref = inter.IDB.Preferences.FirstOrDefault();
                return currentPref;
            }
        }

        public SSFPrefRow CurrentSSFPref
        {
            get
            {
                currentSSFPref = inter.IDB.SSFPref.FirstOrDefault();
                return currentSSFPref;
            }
        }

        private BindingSources bs;
        private Interface inter;

        public Current(ref BindingSources bss, ref Interface interfaces)
        {
            bs = bss;
            inter = interfaces;
        }

        public DataRow SubSample
        {
            get
            {
                return (bs.SubSamples.Current as DataRowView).Row;
            }
        }

        public DataRow Unit
        {
            get
            {
                return (bs.Units.Current as DataRowView).Row;
            }
        }

        public IEnumerable<DataRow> Units
        {
            get
            {
                return (bs.Units.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }

        public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return (bs.SubSamples.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }
    }
}