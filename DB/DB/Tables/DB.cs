using Rsx.Dumb;
using System.Data;

namespace DB
{
    public partial class LINAA : IDB
    {
        protected static string[] matssftypes = new string[] { "0 = Isotropic", "1 = Wire flat", "2 = Foil/wire ch. axis" };

        public string[] MatSSFTYPES
        {
            get
            {
                return matssftypes;
            }
        }

        /// <summary>
        /// IS THIS USED???
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        protected internal static bool IsTableOk<T>(ref T table)
        {
            DataTable table2 = table as DataTable;
            if (table2 == null) return false;
            if (table2.Rows.Count == 0) return false;
            return true;
        }

        public void CheckMatrixToDoes()
        {
            EventData ebento = new EventData();
            this.Matrix.CalcParametersHandler?.Invoke(this, ebento);

            PreferencesRow prefe = (PreferencesRow)ebento.Args[0];
            bool offline = prefe.Offline;

            foreach (MatrixRow item in this.tableMatrix)
            {
                if (offline)
                {
                    MatrixRow m = item;
                    string tempfile = GetMUESFile(ref m);
                    m.ToDo = !System.IO.File.Exists(tempfile);
                }
                else
                {
                    int? o = QTA.CheckMatrixInMUES(item.MatrixID);
                    if (o != null) item.ToDo = false;
                    else item.ToDo = true;
                }
            }
        }
    }
}