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


        Interface Interface;
        string rowWithError = "The selected row has some incomplete cells or cells with errors.\nPlease fix before selecting it";

        public BindingSources(ref Interface inter)
        {
            Interface = inter;
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

     

        /// <summary>
        /// Updates the binding sources positions!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <param name="doCascade"></param>
        public void Update<T>(T r, bool doCascade = true, bool findItself = true)
        {
            Type tipo = typeof(T);
            if (r == null) return;

            if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
            {
                //  DataRow r = (SubSamples.Current as DataRowView).Row;

                LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;
                //  LINAA.UnitRow s = r as LINAA.UnitRow;
                //   LINAA.UnitRow u = s.UnitRow as LINAA.UnitRow;
                if (findItself)
                {
                    string unitValID = (s.Table as LINAA.SubSamplesDataTable).SubSamplesIDColumn.ColumnName;
                    SubSamples.Position = SubSamples.Find(unitValID, s.SubSamplesID);
                }
                //now update the childs/parents of Units
                if (doCascade)  Update<LINAA.UnitRow>(s.UnitRow);
            }
            else if (tipo.Equals(typeof(LINAA.UnitRow)))
            {

                LINAA.UnitRow s = r as LINAA.UnitRow;
                //   LINAA.UnitRow u = s.UnitRow as LINAA.UnitRow;

                if (findItself)
                {
                    string unitValID = (s.Table as LINAA.UnitDataTable).UnitIDColumn.ColumnName;
                    Units.Position = Units.Find(unitValID, s.UnitID);
                }
                //do childs parents or not?
                if (!doCascade) return;

                    Update<LINAA.VialTypeRow>(s.SubSamplesRow.VialTypeRow);
                    Update<LINAA.ChannelsRow>(s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow);
                    Update<LINAA.MatrixRow>(s.SubSamplesRow.MatrixRow);
                    Update<LINAA.VialTypeRow>(s.SubSamplesRow.VialTypeRowByChCapsule_SubSamples);

            


            }
            else if (tipo.Equals(typeof(LINAA.VialTypeRow)))
            {
                LINAA.VialTypeRow u = r as LINAA.VialTypeRow;
                if (findItself)
                {
                    string column;
                    column = (u.Table as LINAA.VialTypeDataTable).VialTypeIDColumn.ColumnName;
                    int id = u.VialTypeID;
                    //BindingSource rabbitBS = Interface.IBS.Rabbit;
               
                if (u.IsRabbit && Rabbit!=null)
                {
                    Rabbit.Position = Rabbit.Find(column, id);
                }
                else if (Vial!=null) Vial.Position = Vial.Find(column, id);
                }

            }
            else if (tipo.Equals(typeof(LINAA.MatrixRow)))
            {
                LINAA.MatrixRow u = r as LINAA.MatrixRow;
                if (findItself)
                {
                    if (Matrix == null) return;

                    string column;
                    column = (u.Table as LINAA.MatrixDataTable).MatrixIDColumn.ColumnName;
                    int id = u.MatrixID;
                    //BindingSource rabbitBS = Interface.IBS.Rabbit;
                    Matrix.Position = Matrix.Find(column, id);
                }
            }

            else if (tipo.Equals(typeof(LINAA.ChannelsRow)))
            {
                LINAA.ChannelsRow u = r as LINAA.ChannelsRow;
                if (findItself)
                {
                    if (Channels == null) return;
                    string column;
                    column = (u.Table as LINAA.ChannelsDataTable).ChannelsIDColumn.ColumnName;
                    int id = u.ChannelsID;
                    //BindingSource rabbitBS = Interface.IBS.Rabbit;
                    Channels.Position = Channels.Find(column, id);
                }
            }
            if ((r as DataRow).HasErrors)
            {
                Interface.IReport.Msg(rowWithError, "Warning", false); ///cannot process because it has errors
            }
        }
    }
}