using System;
using System.Data.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Rsx;

namespace DB.Tools
{
   
    public partial class Populate
    {

        public void Help()
        {
            string path = Interface.IMain.FolderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
        }

        public void PopulateDirectory(string path)
        {
            string result = string.Empty;
            try
            {
                DirectorySecurity secutiry = new DirectorySecurity(path, AccessControlSections.All);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path, secutiry);
                }
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
            }
        }
        public static void CloneSQL(ref LinqDataContext original, ref LinqDataContext destiny)
        {
                      
            ITable dt = original.GetTable<Unit>();
            ITable ita = destiny.GetTable(typeof(Unit));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Matrix>();
            ita = destiny.GetTable(typeof(Matrix));
            insertSQL(ref dt, ref ita);
            dt = original.GetTable<Composition>();
            ita = destiny.GetTable(typeof(Composition));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Reaction>();
            ita = destiny.GetTable(typeof(Reaction));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Sigma>();
            ita = destiny.GetTable(typeof(Sigma));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<SigmasSal>();
            ita = destiny.GetTable(typeof(SigmasSal));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Element>();
            ita = destiny.GetTable(typeof(Element));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<MonitorsFlag>();
            ita = destiny.GetTable(typeof(MonitorsFlag));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Yield>();
            ita = destiny.GetTable(typeof(Yield));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<tStudent>();
            ita = destiny.GetTable(typeof(tStudent));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<LINE>();
            ita = destiny.GetTable(typeof(LINE));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<LINES_FI>();
            ita = destiny.GetTable(typeof(LINES_FI));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<DetectorsAbsorber>();
            ita = destiny.GetTable(typeof(DetectorsAbsorber));
            insertSQL(ref dt, ref ita);
            dt = original.GetTable<DetectorsCurve>();
            ita = destiny.GetTable(typeof(DetectorsCurve));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<DetectorsDimension>();
            ita = destiny.GetTable(typeof(DetectorsDimension));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Holder>();
            ita = destiny.GetTable(typeof(Holder));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<VialType>();
            ita = destiny.GetTable(typeof(VialType));
            insertSQL(ref dt, ref ita);
            dt = original.GetTable<Standard>();
            ita = destiny.GetTable(typeof(Standard));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Monitor>();
            ita = destiny.GetTable(typeof(Monitor));
            insertSQL(ref dt, ref ita);
            dt = original.GetTable<Channel>();
            ita = destiny.GetTable(typeof(Channel));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<Geometry>();
            ita = destiny.GetTable(typeof(Geometry));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<NAA>();
            ita = destiny.GetTable(typeof(NAA));
            insertSQL(ref dt, ref ita);

            dt = original.GetTable<k0NAA>();
            ita = destiny.GetTable(typeof(k0NAA));
            insertSQL(ref dt, ref ita);
        }

        public void RestartSQLServer()
        {
            string start = "start";
             System.Diagnostics.Process.Start(sqlDBEXE, start);
        }

        public void PopulateSQLDatabase()
        {
            //problems withe the connection

            MessageBoxIcon i = MessageBoxIcon.Information;
            //should install SQL?
            DialogResult yesnot = MessageBox.Show(shouldInstallSQL, sqlLocalDB, MessageBoxButtons.YesNo, i);

            if (yesnot == DialogResult.Yes)
            {
                bool is64 = Environment.Is64BitOperatingSystem;
                string localdbexpressPack = sqlPack32;
                if (is64) localdbexpressPack = sqlPack64;

                System.Diagnostics.Process.Start(Application.StartupPath + "\\" + localdbexpressPack);

                RestartSQLServer();
                
                MessageBox.Show(sqlStarted, triedToInstall, MessageBoxButtons.OK, i);

                //create the database if it does not exist
                LinqDataContext destiny = new LinqDataContext(DB.Properties.Settings.Default.localDB);
                if (!destiny.DatabaseExists())
                {
                    destiny.CreateDatabase();
                    // destiny.DeleteDatabase();
                }
                //QUITAR DE AQUI
                //    LinqDataContext original = new LinqDataContext(DB.Properties.Settings.Default.NAAConnectionString);
                // CloneSQL(ref original, ref destiny);


            }
            else
            {
                MessageBox.Show(nocontinueWOSQL, deniedTheInstall, MessageBoxButtons.OK, i);
            }
        
            //try to load the server now
       

        

        }
        public void SendToRestartRoutine(string texto)
        {
            string cmd = Application.StartupPath + Resources.Restarting;

            try
            {
                bool shouldReport = System.IO.File.Exists(cmd);

                if (shouldReport)
                {
                    System.IO.File.AppendAllText(cmd, texto);
                    // GenerateReport("Restarting succeeded...", string.Empty, string.Empty,
                    // DataSetName, email); System.IO.File.Delete(cmd);
                }
                else System.IO.File.WriteAllText(cmd, texto);
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
            }
        }
        public bool RestartingRoutine()
        {
            string cmd = Application.StartupPath + Resources.Restarting;
            bool shouldReport = System.IO.File.Exists(cmd);

            if (shouldReport)
            {
                string email = System.IO.File.ReadAllText(cmd);
                Interface.IReport.GenerateReport("Restarting succeeded...", string.Empty, string.Empty, Interface.IDB.DataSetName, email);
                System.IO.File.Delete(cmd);
            }
            shouldReport = shouldReport || Interface.IDB.Exceptions.Count != 0;

            //should send bug report?
            if (!shouldReport) return false;

            //yes...
            Interface.IReport.GenerateBugReport();

            return true;
        }

        public void PopulateResources(bool overriderFound)
        {
            string path = string.Empty;
            try
            {
                path = Interface.IMain.FolderPath + Resources.Exceptions;
                populateDirectory(path, overriderFound);

                path = Interface.IMain.FolderPath + Resources.Backups;
                populateDirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            try
            {
                populateSolCoiResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            try
            {
                populateMatSSFResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }
        }

        public bool PopulateOverriders()
        {
            string path;
            //override preferences
            path = Interface.IMain.FolderPath + Resources.Preferences + ".xml";
            string developerPath = Application.StartupPath + "\\" + Resources.Preferences + "Dev.xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.SSFPreferences + ".xml";
            developerPath = Application.StartupPath + "\\" + Resources.SSFPreferences + "Dev.xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.WCalc;
            developerPath = Application.StartupPath + "\\" + Resources.WCalc;
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.XCOMEnergies;
            developerPath = Application.StartupPath + "\\" + Resources.XCOMEnergies;
            populateReplaceFile(path, developerPath);

            // path = folderPath + Resources.SolCoiFolder;

            bool overriderFound = false;
            try
            {
                //does nothing
                path = Application.StartupPath + "\\" + Resources.ResourcesOverrider;
                overriderFound = File.Exists(path);
                //TODO:
                if (overriderFound) File.Delete(path);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            path = Application.StartupPath + "\\" + Resources.Features;
            string currentpath = Interface.IMain.FolderPath + Resources.Features;
            populateFeaturesDirectory(path, currentpath);

            return overriderFound;
        }

        public IDetSol IDetSol;
        public IGeometry IGeometry;
        public IIrradiations IIrradiations;

        // public IExpressions IExpressions;
        public INuclear INuclear;

        public IOrders IOrders;
        public IProjects IProjects;
        public ISamples ISamples;
        public ISchedAcqs ISchedAcqs;
        public IToDoes IToDoes;
       

        public Populate(ref Interface inter)
        {
            LINAA aux = inter.Get();
            Interface = inter;

            // IExpressions = (IExpressions)aux;
            INuclear = (INuclear)aux;
            IProjects = (IProjects)aux;
            IIrradiations = (IIrradiations)aux;
            IGeometry = (IGeometry)aux;
            IDetSol = (IDetSol)aux;
            IOrders = (IOrders)aux;
            ISamples = (ISamples)aux;
            ISchedAcqs = (ISchedAcqs)aux;
            IToDoes = (IToDoes)aux;

    
        }


    }

  
}