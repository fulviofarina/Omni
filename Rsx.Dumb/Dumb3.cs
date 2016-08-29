using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Rsx
{
    public partial class Dumb
    {
        /*
         public static bool HasChanges(DataTable table)
         {
            IEnumerable<DataRow> rows = table.AsEnumerable().OfType<DataRow>();
            return Dumb.HasChanges(rows);
         }
          */

        public static System.Data.DataTable DGVToTable(System.Windows.Forms.DataGridView dgv)
        {
            System.Data.DataTable table = new System.Data.DataTable();

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                table.Columns.Add(col.HeaderText, typeof(object), String.Empty);
            }

            System.Collections.ArrayList list = new System.Collections.ArrayList();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    list.Add(cell.Value);
                }
                table.LoadDataRow(list.ToArray(), true);
                list.Clear();
            }

            return table;
        }

        public static void CloneRows(ref DataGridView Origin, ref DataTable Destiny)
        {
            HashSet<int> hint = new HashSet<int>();
            if (Origin != null && Destiny != null)
            {
                foreach (DataGridViewCell cell in Origin.SelectedCells)
                {
                    if (hint.Add(cell.OwningRow.Index))
                    {
                        DataRowView rv = (DataRowView)cell.OwningRow.DataBoundItem;
                        DataRow ToClone = (DataRow)rv.Row;
                        CloneARow(Destiny, ToClone);
                    }
                }
            }
            hint.Clear();
        }

        public static void CloneARow(DataTable Destiny, DataRow ToClone)
        {
            DataRow newr = Destiny.NewRow();

            for (int i = 0; i < Destiny.Columns.Count; i++)
            {
                if (!Destiny.PrimaryKey[0].Equals(Destiny.Columns[i]))
                {
                    newr[i] = ToClone[i];
                }
            }
            Destiny.Rows.Add(newr);
        }

        public static Comparison<string> stringsorter = delegate(string a, string b)
            {
                return a.Remove(0, a.Length - 1).CompareTo(b.Remove(0, b.Length - 1));
            };

        public static void FillABox(ComboBox combo, ICollection<string> hs, bool clear, bool AddAsterisk)
        {
            combo.AutoCompleteMode = AutoCompleteMode.Suggest;
            combo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            if (clear)
            {
                combo.AutoCompleteCustomSource.Clear();
                combo.Items.Clear();
            }
            if (hs != null && hs.Count() != 0)
            {
                combo.Items.AddRange(hs.ToArray<string>());
                combo.AutoCompleteCustomSource.AddRange(hs.ToArray<string>());
                if (AddAsterisk)
                {
                    combo.Items.Add("*");
                    combo.AutoCompleteCustomSource.Add("*");
                }
            }
        }

        public static void FillABox(ToolStripComboBox combo, ICollection<string> hs, bool clear, bool AddAsterisk)
        {
            FillABox(combo.ComboBox, hs, clear, AddAsterisk);
        }

        public static double Process(System.Diagnostics.Process process, string WorkingDir, string EXE, string Arguments, bool hide, bool wait, int wait_time)
        {
            double span = 0;
            ProcessStartInfo info = new ProcessStartInfo(EXE);
            info.WorkingDirectory = WorkingDir;
            info.Arguments = Arguments;
            info.ErrorDialog = true;
            process.StartInfo = info;

            //   process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            if (hide)
            {
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            process.Start();
            if (wait)
            {
                process.WaitForExit();
            }

            if (process.HasExited) span = ((TimeSpan)(process.ExitTime - process.StartTime)).TotalSeconds;

            return span;
        }

        public static void SetError(Control control, ErrorProvider error, string message)
        {
            error.SetError(control, error.GetError(control) + message);
        }
    }
}