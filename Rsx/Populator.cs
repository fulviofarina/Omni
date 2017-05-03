using System;
using System.Data.Linq;

namespace Rsx.SQL
{

    /// <summary>
    /// Provides the methods that should not be called directly but through Populator UI
    /// </summary>
    public partial class SQL
    {
        protected static string localDBFilePath = "\\Microsoft SQL Server\\120\\Tools\\Binn\\";
        // + "Click NO if you want to proceed with the default connection string.\n\nClick Cancel to
        // avoid the population of the Database";

        protected static string sqlDBEXE = "SqlLocalDB.exe";

        protected static string sqlPack32 = "localdbx32.msi";
        protected static string sqlPack64 = "localdbx64.msi";
    }

    /// <summary>
    /// Provides the methods that should not be called directly but through Populator UI
    /// </summary>
    public partial class SQL
    {
        /// <summary>
        /// Deletes completely a given database
        /// </summary>
        /// <param name="localDBPath"></param>
        /// <returns></returns>
        public static bool DeleteDatabase(string localDBPath)
        {
            DataContext destiny;

            destiny = new DataContext(localDBPath);
            bool exist = destiny.DatabaseExists();
            if (exist)
            {
                try
                {
                    // destiny.Connection.Close();
                    destiny.DeleteDatabase();
                    // destiny.Connection.Open();
                }
                catch (Exception)
                {
                }
            }
            return destiny.DatabaseExists();
        }

        /// <summary>
        /// Inserts on Submits a SQL DataTable from one place to another
        /// </summary>
        /// <param name="dt"> </param>
        /// <param name="ita"></param>
        public static void InsertDataTable(ref ITable dt, ref ITable ita)
        {
            foreach (var i in dt)
            {
                ita.InsertOnSubmit(i);
            }

            ita.Context.SubmitChanges();
        }

        /// <summary> Installs the SQL version given the prerequisite folder path where the files are
        /// located These files must be named "localdbx32.msi" and "localdbx64.msi"
        ///
        /// <param name="prerequisitePath"></param>
        public static void InstallSQL(string prerequisitePath)
        {
            bool is64 = Environment.Is64BitOperatingSystem;
            string localdbexpressPack = sqlPack32;
            if (is64) localdbexpressPack = sqlPack64;

            System.Diagnostics.Process.Start(prerequisitePath + "\\" + localdbexpressPack);
        }

        public static bool RestartSQLLocalDBServer()
        {
            // string start = "start ";
            bool hide = false;
            bool is64 = Environment.Is64BitOperatingSystem;

            string path = string.Empty;
            path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (is64) path = path.Replace(" (x86)", null);

            // else
            string workDir = path + localDBFilePath;
            path = workDir + sqlDBEXE;

            bool exist = System.IO.File.Exists(path);

            if (!exist) return exist;

            //CREATE BATE FILE
            path = sqlDBEXE;
            string tmp = "\\Temp\\";
            string content = "start /B " + path + " start";
            string batFile = "sql.bat";
            string batPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + tmp;
            System.IO.File.WriteAllText(batPath + batFile, content);

            //EXECUTE BAT FILE
            string cmd = "cmd.exe";
            string runas = "runas";

            System.Diagnostics.ProcessStartInfo i = new System.Diagnostics.ProcessStartInfo();
            if (hide) i.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            else i.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            i.Verb = runas;
            i.Arguments = "/c " + batFile;
            i.WorkingDirectory = batPath;
            i.FileName = cmd;
            i.UseShellExecute = false;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = i;
            process.Start();
            process.WaitForExit(10000);

            return exist;
        }
    }
}