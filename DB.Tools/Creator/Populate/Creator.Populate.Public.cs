using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{
   
    public partial class Creator
    {
        public static void Help()
        {
            string path = Interface.IMain.FolderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
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

        public static void RestartSQLServer()
        {
            string start = "start";
            bool hide = false;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
           
            Dumb.Process(process, string.Empty, sqlDBEXE, start, hide, true, 10000);
                       
            // System.Diagnostics.Process.Start(sqlDBEXE, start,);
        }

        public static void PopulateSQLDatabase()
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
        public static void SendToRestartRoutine(string texto)
        {
            string cmd = Application.StartupPath + Resources.Restarting;

            try
            {
                bool shouldReport = System.IO.File.Exists(cmd);

                if (shouldReport)
                {
                    File.AppendAllText(cmd, texto);
                    // GenerateReport("Restarting succeeded...", string.Empty, string.Empty,
                    // DataSetName, email); System.IO.File.Delete(cmd);
                }
                else File.WriteAllText(cmd, texto);
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
            }
        }
     

        public static void PopulateResources(bool overriderFound)
        {
            string path = string.Empty;
            try
            {
                path = Interface.IMain.FolderPath + Resources.Exceptions;
                Rsx.Dumb.MakeADirectory(path, overriderFound);

                path = Interface.IMain.FolderPath + Resources.Backups;
                Rsx.Dumb.MakeADirectory(path, overriderFound);
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
     
    }
 
}