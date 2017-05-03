using System;
using System.Data.Linq;

namespace DB.Linq
{
    partial class LinqDataContext
    {
        private static string sqlDBEXE = "SqlLocalDB.exe";
   
        private static string sqlPack32 = "localdbx32.msi";
        private static string sqlPack64 = "localdbx64.msi";
        public static void InstallSQLLocalDB(string path)
        {
            bool is64 = Environment.Is64BitOperatingSystem;
            string localdbexpressPack = sqlPack32;
            if (is64) localdbexpressPack = sqlPack64;

            System.Diagnostics.Process.Start(path+ "\\" + localdbexpressPack);
        }


        public static bool PopulateSQL(string localDBPath)
        {
            //create the database if it does not exist
            LinqDataContext destiny = new LinqDataContext(localDBPath);
            if (!destiny.DatabaseExists())
            {
                destiny.CreateDatabase();
                // destiny.DeleteDatabase();
            }
            //QUITAR DE AQUI
            //    LinqDataContext original = new LinqDataContext(DB.Properties.Settings.Default.NAAConnectionString);
            // CloneSQL(ref original, ref destiny);
            return true;
        }
        public static bool RestartSQLServer()
        {
            string start = "start ";
            bool hide = false;
            bool is64 = Environment.Is64BitOperatingSystem;

            string path = string.Empty;
            path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (is64) path = path.Replace(" (x86)", null);
            // else 
            string workDir = path + "\\Microsoft SQL Server\\120\\Tools\\Binn\\";
            path = workDir + sqlDBEXE;

          
            bool exist = System.IO.File.Exists(path);

            if (!exist) return exist;

            path = sqlDBEXE;
            string batPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+ "\\Temp\\";
            System.IO.File.WriteAllText(batPath + "sql.bat", "start /B " + path + " start" );

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo i = new System.Diagnostics.ProcessStartInfo();
            if (hide) i.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            else i.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            i.Verb = "runas";
            i.Arguments = "/c sql.bat";
            i.WorkingDirectory = batPath;
            i.FileName = "cmd.exe";
            i.UseShellExecute = false;

            process.StartInfo = i;
            process.Start();
            process.WaitForExit(10000);
            
            return exist;
        
        }

        public static void CloneSQLDatabase(ref LinqDataContext original, ref LinqDataContext destiny)
        {
            ITable dt = original.GetTable<Unit>();
            ITable ita = destiny.GetTable(typeof(Unit));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Matrix>();
            ita = destiny.GetTable(typeof(Matrix));
            InsertSQLDataTable(ref dt, ref ita);
            dt = original.GetTable<Composition>();
            ita = destiny.GetTable(typeof(Composition));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Reaction>();
            ita = destiny.GetTable(typeof(Reaction));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Sigma>();
            ita = destiny.GetTable(typeof(Sigma));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<SigmasSal>();
            ita = destiny.GetTable(typeof(SigmasSal));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Element>();
            ita = destiny.GetTable(typeof(Element));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<MonitorsFlag>();
            ita = destiny.GetTable(typeof(MonitorsFlag));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Yield>();
            ita = destiny.GetTable(typeof(Yield));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<tStudent>();
            ita = destiny.GetTable(typeof(tStudent));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<LINE>();
            ita = destiny.GetTable(typeof(LINE));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<LINES_FI>();
            ita = destiny.GetTable(typeof(LINES_FI));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<DetectorsAbsorber>();
            ita = destiny.GetTable(typeof(DetectorsAbsorber));
            InsertSQLDataTable(ref dt, ref ita);
            dt = original.GetTable<DetectorsCurve>();
            ita = destiny.GetTable(typeof(DetectorsCurve));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<DetectorsDimension>();
            ita = destiny.GetTable(typeof(DetectorsDimension));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Holder>();
            ita = destiny.GetTable(typeof(Holder));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<VialType>();
            ita = destiny.GetTable(typeof(VialType));
            InsertSQLDataTable(ref dt, ref ita);
            dt = original.GetTable<Standard>();
            ita = destiny.GetTable(typeof(Standard));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Monitor>();
            ita = destiny.GetTable(typeof(Monitor));
            InsertSQLDataTable(ref dt, ref ita);
            dt = original.GetTable<Channel>();
            ita = destiny.GetTable(typeof(Channel));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<Geometry>();
            ita = destiny.GetTable(typeof(Geometry));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<NAA>();
            ita = destiny.GetTable(typeof(NAA));
            InsertSQLDataTable(ref dt, ref ita);

            dt = original.GetTable<k0NAA>();
            ita = destiny.GetTable(typeof(k0NAA));
            InsertSQLDataTable(ref dt, ref ita);
        }

        public static void InsertSQLDataTable(ref ITable dt, ref ITable ita)
        {
            foreach (var i in dt)
            {
                ita.InsertOnSubmit(i);
            }

            ita.Context.SubmitChanges();
        }


    }
}