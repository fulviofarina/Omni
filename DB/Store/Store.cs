using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

//using DB.Interfaces;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA : IStore
    {

        public void Read(string filepath)
        {
            LINAA dt = null;


            // file.EnforceConstraints = false;
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

                dt.CleanPreferences();

            //    this.AcceptChanges();

                this.Merge(dt);

              

          //      this.AcceptChanges();
                //    DataSet set = Interface.Get();
                //clear and repopulate

                //MergePreferences();
                // this.PopulateSSFPreferences();
            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            //return dt;
        }

        protected string folderPath = string.Empty;

        protected bool useHandlers = false;

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        public void AddException(Exception ex)
        {
            // this.PopulateColumnExpresions()
            this.tableExceptions.AddExceptionsRow(ex);
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

            // this.notify;
        }

        public IEnumerable<DataTable> GetTablesWithChanges()
        {
            IEnumerable<DataTable> tables = null;
            tables = this.Tables.OfType<DataTable>();
            Func<DataTable, bool> haschangesFunc = t =>
            {
                bool hasChanges = false;
                IEnumerable<DataRow> rows = t.AsEnumerable();
                IEnumerable<DataRow> rowsWithChanges = Changes.GetRowsWithChanges(rows);
                hasChanges = rowsWithChanges.Count() != 0;
                return hasChanges;
            };

            return tables.Where(haschangesFunc);
        }

        public void CleanPreferences()
        {
            Preferences.Clear();
            XCOMPref.Clear();
            SSFPref.Clear();
            Preferences.AcceptChanges();
            XCOMPref.AcceptChanges();
            SSFPref.AcceptChanges();
        }
    }
}