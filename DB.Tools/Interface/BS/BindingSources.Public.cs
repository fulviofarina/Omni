﻿using System;
using System.Data;
using System.Windows.Forms;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public partial class BindingSources
    {
        public BindingSource Channels;

        public BindingSource Geometry;
        public BindingSource SSF;

        public BindingSource Irradiations;

        public BindingSource Matrix;
        public BindingSource MonitorsFlags;
        public BindingSource Samples;
        /// <summary>
        /// not attached yet
        /// </summary>
        public BindingSource Monitors;

        public BindingSource Preferences;

        public BindingSource Rabbit;

        public BindingSource SSFPreferences;
        public BindingSource Standards;
        //binding sources to attach;
        public BindingSource SubSamples;

        public BindingSource Units;
        public BindingSource Vial;

        /// <summary>
        /// EndEdit for each binding source
        /// </summary>
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
        /// <param name="r">        </param>
        /// <param name="doCascade"></param>
        public void Update<T>(T r, bool doCascade = true, bool findItself = true)
        {
            Type tipo = typeof(T);
            if (r == null) return;

            if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
            {
                // DataRow r = (SubSamples.Current as DataRowView).Row;

                LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;
                // LINAA.UnitRow s = r as LINAA.UnitRow; LINAA.UnitRow u = s.UnitRow as LINAA.UnitRow;
                if (findItself && SubSamples != null)
                {
                    string unitValID = (s.Table as LINAA.SubSamplesDataTable).SubSamplesIDColumn.ColumnName;
                    SubSamples.Position = SubSamples.Find(unitValID, s.SubSamplesID);
                }
                //now update the childs/parents of Units
                if (doCascade && s.UnitRow != null) Update<LINAA.UnitRow>(s.UnitRow);
            }
            else if (tipo.Equals(typeof(LINAA.UnitRow)))
            {
                LINAA.UnitRow s = r as LINAA.UnitRow;
            
                MatSSF.UNIT = s; //this is key


                if (findItself && Units != null)
                {
                    string unitValID = (s.Table as LINAA.UnitDataTable).UnitIDColumn.ColumnName;
                    Units.Position = Units.Find(unitValID, s.UnitID);
                }
                //do childs parents or not?
                if (doCascade && s.SubSamplesRow != null)
                {
                    Update<LINAA.VialTypeRow>(s.SubSamplesRow.VialTypeRow);
                    Update<LINAA.ChannelsRow>(s.SubSamplesRow.IrradiationRequestsRow.ChannelsRow);
                    Update<LINAA.MatrixRow>(s.SubSamplesRow.MatrixRow);
                    Update<LINAA.VialTypeRow>(s.SubSamplesRow.VialTypeRowByChCapsule_SubSamples);
                }

                MatSSF.ReadXML();


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

                    if (u.IsRabbit && Rabbit != null)
                    {
                        Rabbit.Position = Rabbit.Find(column, id);
                    }
                    else if (Vial != null) Vial.Position = Vial.Find(column, id);
                }
            }
            else if (tipo.Equals(typeof(LINAA.MatrixRow)))
            {
                LINAA.MatrixRow u = r as LINAA.MatrixRow;
                if (findItself && Matrix != null)
                {
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
                if (findItself && Channels != null)
                {
                    string column;
                    column = (u.Table as LINAA.ChannelsDataTable).ChannelsIDColumn.ColumnName;
                    int id = u.ChannelsID;
                    //BindingSource rabbitBS = Interface.IBS.Rabbit;
                    Channels.Position = Channels.Find(column, id);
                }
            }
            else if (tipo.Equals(typeof(LINAA.IrradiationRequestsRow)))
            {
                LINAA.IrradiationRequestsRow u = r as LINAA.IrradiationRequestsRow;
                if (findItself && Irradiations != null)
                {
                    string column;
                    column = (u.Table as LINAA.IrradiationRequestsDataTable).IrradiationRequestsIDColumn.ColumnName;
                    int id = u.IrradiationRequestsID;
                    Irradiations.Position = Irradiations.Find(column, id);

                    

                }



            }
            if ((r as DataRow).HasErrors)
            {
                Interface.IReport.Msg(rowWithError, "Warning", false); ///cannot process because it has errors
            }
        }
        private void Channels_CurrentChanged(object sender, EventArgs e)
        {
                string sortColumn;
           string filter;
            sortColumn = Interface.IDB.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
            //
            LINAA.ChannelsRow c = Dumb.Cast<LINAA.ChannelsRow>(Interface.IBS.Channels.Current);
           
                string chColumn = Interface.IDB.IrradiationRequests.ChannelNameColumn.ColumnName;
                filter = chColumn + " = '" + c.ChannelName + "'";// + " OR " + chColumn + " IS NULL ";

            Interface.IBS.Irradiations.Filter = filter;
            Interface.IBS.Irradiations.Sort = sortColumn;
          //  Dumb.LinkBS(ref Interface.IBS.Irradiations, Interface.IDB.IrradiationRequests, filter, sortColumn + " desc");
        }
        /// <summary>
        /// A Binding source for each table
        /// </summary>
        public BindingSources(ref Interface inter)
        {
            Interface = inter;

            Channels = new BindingSource(Interface.Get(), Interface.IDB.Channels.TableName);
            Channels.CurrentChanged += Channels_CurrentChanged;
            Matrix = new BindingSource(Interface.Get(), Interface.IDB.Matrix.TableName);
            Rabbit = new BindingSource(Interface.Get(), Interface.IDB.VialType.TableName);
       
            //    Dumb.LinkBS(ref this.ChannelBS, Interface.IDB.Channels);
            string column = Interface.IDB.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = Interface.IDB.VialType.InnerRadiusColumn.ColumnName + " asc";
            //      Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Dumb.LinkBS(ref this.Rabbit, Interface.IDB.VialType, column + " = " + "True", innerRadCol);

            Vial = new BindingSource(Interface.Get(), Interface.IDB.VialType.TableName);
            Dumb.LinkBS(ref this.Rabbit, Interface.IDB.VialType, column + " = " + "False", innerRadCol);


            Irradiations = new BindingSource(Interface.Get(), Interface.IDB.IrradiationRequests.TableName);

            Geometry = new BindingSource(Interface.Get(), Interface.IDB.Geometry.TableName);


            Dumb.LinkBS(ref Geometry, Interface.IDB.Geometry, string.Empty, "CreationDateTime desc");

            Standards = new BindingSource(Interface.Get(), Interface.IDB.Standards.TableName);

            Monitors = new BindingSource(Interface.Get(), Interface.IDB.Monitors.TableName);

            MonitorsFlags = new BindingSource(Interface.Get(), Interface.IDB.MonitorsFlags.TableName);

            Samples = new BindingSource(Interface.Get(), Interface.IDB.Samples.TableName);

            SubSamples = new BindingSource(Interface.Get(), Interface.IDB.SubSamples.TableName);

            SubSamples.CurrentChanged += SubSamples_CurrentChanged;

            Units = new BindingSource(Interface.Get(), Interface.IDB.Unit.TableName);
            SSF = new BindingSource(Interface.Get(), Interface.IDB.MatSSF.TableName);

            Units.CurrentChanged += Units_CurrentChanged;

        }

     
                 /// <summary>
                 /// A binding Current Changed event to update Binding sources
                 /// </summary>
                 /// <param name="sender"></param>
                 /// <param name="e"></param>
        private void Units_CurrentChanged(object sender, System.EventArgs e)
        {

            LINAA.UnitRow unit = Interface.ICurrent.Unit as LINAA.UnitRow;

            //important
            Interface.IBS.Update<LINAA.SubSamplesRow>(unit.SubSamplesRow, false);
            Interface.IBS.Update<LINAA.UnitRow>(unit, true, false);



            //then it will be updated
            string column = Interface.IDB.MatSSF.UnitIDColumn.ColumnName;
            string sortCol = Interface.IDB.MatSSF.TargetIsotopeColumn.ColumnName;
            string unitID = unit.UnitID.ToString();
            string filter = column + " is " + unitID;

            Dumb.LinkBS(ref SSF, Interface.IDB.MatSSF, filter, sortCol);



      
    }

        private void SubSamples_CurrentChanged(object sender, EventArgs e)
        {

            // DataRowView r = Interface.IBS.SubSamples.Current as DataRowView;
            SubSamplesRow r = Interface.ICurrent.SubSample as SubSamplesRow;
            Interface.IBS.Update(r, true, false);

        }
    }

   
}