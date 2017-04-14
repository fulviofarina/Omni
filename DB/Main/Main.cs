using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using DB.Properties;

//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IMain
    {
        /*
     public bool Read(string file)

     {
         bool read = false;

         try
         {
             this.Clear();

             if (System.IO.File.Exists(file))
             {
                 this.ReadXml(file, XmlReadMode.InferTypedSchema);
             }
             read = true;
         }
         catch (SystemException ex)
         {
             this.AddException(ex);
         }

         return read;
     }
     */

        private string folderPath = string.Empty;

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        private List<int> dTWithHandlers;

        private List<DataColumnChangeEventHandler> handlers;

        protected internal void Handlers(bool activate, ref DataTable dt)
        {
            int dtindex = Tables.IndexOf(dt);
            int index = dTWithHandlers.IndexOf(dtindex);
            if (index < 0) return; //not in the list of handlers

            DataColumnChangeEventHandler han = handlers.ElementAt(index);
            if (activate)
            {
                if (dt.Equals(Matrix)) Matrix.ColumnChanging += Matrix.DataColumnChanging;
                dt.ColumnChanged += han;
            }
            else
            {
                if (dt.Equals(Matrix)) Matrix.ColumnChanging -= Matrix.DataColumnChanging;
                dt.ColumnChanged -= han;
            }
        }

        protected internal void RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add && e.Action != DataRowAction.Commit) return;
            //	if( e.Action != DataRowAction.Commit) return;

            e.Row.ClearErrors();
            dynamic table;
            table = e.Row.Table;
            IEnumerable<DataColumn> cols = e.Row.Table.Columns.OfType<DataColumn>();
            foreach (DataColumn column in cols)
            {
                table.DataColumnChanged(sender, new DataColumnChangeEventArgs(e.Row, column, e.Row[column]));
            }
        }

        protected internal void RowHandlers(ref DataTable table, bool activate)
        {
            int dtindex = Tables.IndexOf(table);
            int index = dTWithHandlers.IndexOf(dtindex);
            if (index < 0) return; //not in the list of handlers

            if (activate) table.RowChanged += RowChanged;
            else table.RowChanged -= RowChanged;
        }

        private void cleanReadOnly(ref DataTable table)
        {
            //    DataTable table = dt as DataTable;
            foreach (DataColumn column in table.Columns)
            {
                bool ok = column.ReadOnly;
                ok = ok && column.Expression.Equals(string.Empty);
                ok = ok && !table.PrimaryKey.Contains(column);
                if (ok) column.ReadOnly = false;
            }
        }

        public void AddException(Exception ex)
        {
            // this.PopulateColumnExpresions()
            this.tableExceptions.AddExceptionsRow(ex);
        }

        private void Handlers(bool activate)
        {
            for (int i = 0; i < dTWithHandlers.Count; i++)
            {
                int index = dTWithHandlers[i];
                DataTable dt = Tables[index];
                Handlers(activate, ref dt);
                RowHandlers(ref dt, activate);
            }
        }

        public void PopulateColumnExpresions()
        {
            handlers = new List<DataColumnChangeEventHandler>();
            dTWithHandlers = new List<int>();

            handlers.Add(Channels.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Channels));

            handlers.Add(IrradiationRequests.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(IrradiationRequests));

            handlers.Add(Matrix.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Matrix));
            handlers.Add(VialType.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(VialType));
            handlers.Add(Geometry.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(Geometry));
            handlers.Add(SubSamples.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SubSamples));

            handlers.Add(Standards.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(Standards));
            handlers.Add(Monitors.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(Monitors));
            handlers.Add(DetectorsAbsorbers.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(DetectorsAbsorbers));
            handlers.Add(Unit.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(Unit));
            handlers.Add(Preferences.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(Preferences));
            handlers.Add(SSFPref.DataColumnChanged);


            dTWithHandlers.Add(Tables.IndexOf(SSFPref));

            Handlers(true);

            PopulateSelectedExpression(true);

            //  tableIRequestsAverages.ChThColumn.Expression = " ISNULL(1000 * Parent(SigmasSal_IRequestsAverages).sigmaSal / Parent(SigmasSal_IRequestsAverages).Mat ,'0')";
            //  tableIRequestsAverages.ChEpiColumn.Expression = " ISNULL(1000 * Parent(Sigmas_IRequestsAverages).sigmaEp / Parent(Sigmas_IRequestsAverages).Mat,'0') ";
            //   tableIRequestsAverages.SDensityColumn.Expression = "  6.0221415 * 10 * Parent(SubSamples_IRequestsAverages).DryNet / ( Parent(SubSamples_IRequestsAverages).Radius * ( Parent(SubSamples_IRequestsAverages).Radius + Parent(SubSamples_IRequestsAverages).FillHeight))";
        }

        public void PopulateSelectedExpression(bool setexpression)
        {
            string expression = string.Empty;
            if (setexpression)
            {
                expression = "Parent(Measurements_Peaks).Selected";
            }
            //   PopulatePreferences();
            Peaks.SelectedColumn.Expression = expression;
        }

        public void CloneDataSet(ref LINAA set)
        {
            this.InitializeComponent();
            this.Merge(set, false, MissingSchemaAction.Ignore);
            this.PopulateColumnExpresions();
            this.IRequestsAverages.Clear();
            this.IPeakAverages.Clear();

            DataTable table = IRequestsAverages;
            cleanReadOnly(ref table);
            table = IPeakAverages;
            cleanReadOnly(ref table);

            //   this.notify;
        }

        public void Read(string filepath)
        {
            LINAA dt = null;

            //  file.EnforceConstraints = false;
            XmlReader reader = null;
            try
            {
                XmlReaderSettings set = new XmlReaderSettings();
                set.CheckCharacters = false;
                set.ConformanceLevel = ConformanceLevel.Auto;
                set.DtdProcessing = DtdProcessing.Ignore;
                set.IgnoreWhitespace = true;
                set.ValidationFlags = XmlSchemaValidationFlags.None;
                set.ValidationType = ValidationType.None;
                reader = XmlReader.Create(filepath, set);

                dt = new LINAA();

                dt.ReadXml(reader, XmlReadMode.IgnoreSchema);

                //MergePreferences();
                // this.PopulateSSFPreferences();
            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            Dumb.FD<LINAA>(ref dt);
        }



        private static bool removeDuplicates(DataTable table, string UniqueField, string IndexField, ref TAMDeleteMethod remover)
        {
            bool duplicates = false;

            IList<object> hs = Dumb.HashFrom<object>(table.Columns[UniqueField]);

            if (hs.Count != table.Rows.Count) //there are duplicates!!
            {
                IEnumerable<DataRow> rows = null;
                foreach (object s in hs)
                {
                    rows = table.AsEnumerable();
                    rows = rows.Where(d => d.Field<object>(UniqueField).Equals(s));
                    if (rows.Count() > 1)// there are sample duplicates
                    {
                        rows = rows.OrderByDescending(d => d.Field<object>(IndexField)); //most recent is the first, older the last
                        rows = rows.Take(rows.Count() - 1);
                        foreach (DataRow d in rows)
                        {
                            remover.Invoke(d.Field<int>(IndexField));
                        }
                    }
                }

                hs.Clear();
                hs = null;

                duplicates = true;
            }

            return duplicates;
        }

        /*
    public void PopulateSubSamples()
    {
    try
    {
       this.tableSubSamples.Clear();
       this.tableSubSamples.BeginLoadData();
       TAM.SubSamplesTableAdapter.DeleteNulls();
       LINAA.SubSamplesDataTable newsamples = new SubSamplesDataTable(false);
       TAM.SubSamplesTableAdapter.Fill(newsamples);
       this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);

       foreach (LINAA.SubSamplesRow s in this.tableSubSamples)
       {
          if (s.IsCapsulesIDNull()) continue;
          if (s.CapsulesRow == null) continue;
          int id = s.CapsulesRow.VialTypeID;
          bool enaa = s.CapsulesRow.ENAA;
         // s.CapsulesID = id;
     //	 s.ENAA = enaa;

          this.tAM.SubSamplesTableAdapter.UpdateCaps(enaa, id, s.SubSamplesID);
       }

       // LINAA.SetAdded(ref old);
       this.tableSubSamples.EndLoadData();
       this.tableSubSamples.AcceptChanges();
    }
    catch (SystemException ex)
    {
       this.AddException(ex);
    }
    }
    */
    }
}