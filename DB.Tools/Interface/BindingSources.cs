using System;
using System.Data;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public class BindingSources
    {
        //binding sources to attach;
        public dynamic SubSamples;

        /// <summary>
        /// not attached yet
        /// </summary>
        public dynamic Monitors;

        public dynamic Preferences;
        public dynamic SSFPreferences;
        public dynamic Units;
        public dynamic Matrix;
        public dynamic Vial;
        public dynamic Geometry;
        public dynamic Rabbit;
        public dynamic Channels;
        public dynamic Irradiations;

        public BindingSources()
        {
        }

        public void EndEdit()
        {
            Matrix?.EndEdit();
            Units?.EndEdit();
            Vial?.EndEdit();
            Geometry?.EndEdit();
            Rabbit?.EndEdit();
            Channels?.EndEdit();
            Irradiations?.EndEdit();
        }

        public void Update<T>(DataRowView r)
        {
            Update<T>(r.Row);
        }

        public void Update<T>(DataRow r)
        {
            Type tipo = typeof(T);
            if (r == null) return;

            if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
            {
                //  DataRow r = (SubSamples.Current as DataRowView).Row;

                LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;

                Update<LINAA.UnitRow>(s.UnitRow);
            }
            else if (tipo.Equals(typeof(LINAA.UnitRow)))
            {
                if (Units != null)
                {
                    LINAA.UnitRow s = r as LINAA.UnitRow;
                    string unitValID = (s.Table as LINAA.UnitDataTable).UnitIDColumn.ColumnName;
                    Units.Position = Units.Find(unitValID, s.UnitID);
                }
            }
        }
    }
}