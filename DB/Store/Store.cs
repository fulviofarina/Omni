//using DB.Interfaces;
using DB.Properties;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace DB
{
    public partial class LINAA : IStore
    {
        public void ReadLIMS(string filepath)
        {
            LINAA dt = null;
            dt = new LINAA();

            // file.EnforceConstraints = false;
            XmlReader reader = null;

            try
            {
                reader = IO.ReadDataSet(filepath, dt);

                dt.CleanPreferences();

                this.Merge(dt);

            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            reader?.Dispose();
         
            dt.Dispose();
          

            //return dt;
        }

     

        /// <summary>
        /// reads from the backup LIMS.xml file or the Developers version
        /// </summary>
        public  void ReadDefaultLIMS()
        {
            string filePath = folderPath + Resources.Backups + Resources.Linaa;
            //if it does not exist, then read the developer file
            if (!System.IO.File.Exists(filePath))
            {
                //esto cambio en uFinder asi que arreglar en MSTFF, usar lims.xml como un resource y empotrarlo
                filePath = DevPath + Resources.Linaa;
            }
            // Interface.Get().Clear();
            this.AcceptChanges();

            ReadLIMS(filePath);
        }

        public string DevPath
        {
            get
            {
                return folderPath + Resources.DevFiles;
            }
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

            return Changes.GetTablesWithChanges(ref tables);
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