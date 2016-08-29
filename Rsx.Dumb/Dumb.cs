using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

///FULVIO
namespace Rsx
{
    public partial class Notifier
    {
        public static System.Drawing.Icon MakeIcon(string text, System.Drawing.Font font, System.Drawing.Color col)
        {
            System.Drawing.Brush brush = new System.Drawing.SolidBrush(col);

            // Create a bitmap and draw text on it
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(16, 16);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.DrawString(text, font, brush, 0, 0);

            // Convert the bitmap with text to an Icon
            IntPtr hIcon = bitmap.GetHicon();

            System.Drawing.Icon icon = System.Drawing.Icon.FromHandle(hIcon);
            return icon;
        }

        public static System.Drawing.Bitmap MakeBitMap(string text, System.Drawing.Font font, System.Drawing.Color col)
        {
            System.Drawing.Brush brush = new System.Drawing.SolidBrush(col);

            // Create a bitmap and draw text on it
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(64, 64);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.DrawString(text, font, brush, 0, 0);

            // Convert the bitmap with text to an Icon
            return bitmap;
        }
    }

    public partial class Dumb
    {
        public static string ReadFile(string File)
        {
            System.IO.FileStream fraw = new System.IO.FileStream(File, System.IO.FileMode.Open);
            System.IO.StreamReader raw = new System.IO.StreamReader(fraw);

            string lecture = raw.ReadToEnd();
            fraw.Close();
            fraw.Dispose();
            fraw = null;
            raw.Close();
            raw.Dispose();
            raw = null;
            return lecture;
        }

        public static WebBrowser NavigateTo(string text, string uri)
        {
            WebBrowser browser = new WebBrowser();
            Form any = new Form();
            any.Text = text;
            browser.Dock = DockStyle.Fill;
            any.Show();
            any.Controls.Add(browser);
            any.WindowState = FormWindowState.Maximized;

            browser.Navigate(uri);

            return browser;
        }

        public static string GetNextName(string prefix, IList<string> items, bool putCaps)
        {
            int actualMeasCount = items.Count;

            string nextMeas = prefix + Number2String(actualMeasCount, putCaps);
            while (items.Contains(nextMeas))
            {
                actualMeasCount++;
                nextMeas = prefix + Number2String(actualMeasCount, putCaps);
            }

            return nextMeas;
        }


        public static double Parse(string Mdens, double val)
        {
            double ro = val;
            double.TryParse(Mdens, out ro);
            if (double.IsNaN(ro)) ro = val;
            return ro;
        }
        public static String Number2String(int number, bool isCaps)
        {
            int newchar = (isCaps ? 65 : 97) + (number);

            Char c = (Char)(newchar);
            return c.ToString();
        }

        public static bool MergeTable<T, T2>(ref T dt, ref T2 dataset)
        {
            object o = dt;
            DataTable table = (DataTable)o;
            object s = dataset;
            DataSet set = (DataSet)s;
            bool success = false;
            if (set == null) return success;
            if (table == null || table.Rows.Count == 0) return success;

            try
            {
                DataTable destination = set.Tables[table.TableName];
                destination.BeginLoadData();
                destination.Merge(table, false, MissingSchemaAction.AddWithKey);
                destination.EndLoadData();
                //		destination.AcceptChanges();
                success = true;
            }
            catch (SystemException ex)
            {
                throw;
            }
            return success;
        }

        public static void FD<T>(ref T objeto)
        {
            object o = objeto;
            if (o == null) return;
            Type t = typeof(T);
            if (t.Equals(typeof(DataTable)))
            {
                DataTable table = (DataTable)o;
                table.Clear();
            }
            IDisposable disposable = (IDisposable)o;
            disposable.Dispose();
            disposable = null;
        }

        public static IEnumerable<T> GetChildControls<T>(UserControl control)
        {
            IEnumerable<Control> ctrols = control.Controls.OfType<Control>();
            IEnumerable<Control> exts = null;
            while (ctrols.Count() != 0)
            {
                if (exts == null) exts = ctrols.SelectMany(o => o.Controls.OfType<Control>()).ToList();
                exts = exts.Union(ctrols.SelectMany(o => o.Controls.OfType<Control>())).ToList();
                ctrols = ctrols.SelectMany(o => o.Controls.OfType<Control>());
            }
            return exts.OfType<T>();
        }

        public static T GetTable<T>(System.Data.DataSet set)
        {
            return set.Tables.OfType<T>().FirstOrDefault();
        }

        public static bool IsUpper(string value)
        {
            value = System.Text.RegularExpressions.Regex.Replace(value, @"\d", "");
            if (String.IsNullOrEmpty(value)) return false;
            IEnumerable<char> chars = value.AsEnumerable<char>();
            chars = chars.Where(c => char.IsLower(c));
            if (chars.Count() == 0) return true;
            else return false;
        }

        public static bool IsLower(string value)
        {
            // Consider string to be lowercase if it has no uppercase letters.
            value = System.Text.RegularExpressions.Regex.Replace(value, @"\d", "");
            if (String.IsNullOrEmpty(value)) return false;
            IEnumerable<char> chars = value.AsEnumerable<char>();
            chars = chars.Where(c => char.IsUpper(c));
            if (chars.Count() == 0) return true;
            else return false;
        }

        public static string ToReadableString(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Days > 0 ? string.Format("{0:0}d", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0}h", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0}m", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0}s", span.Seconds) : string.Empty);

            return formatted;
        }

        public static TimeSpan ToReadableTimeSpan(string formattedTS)
        {
            TimeSpan span = new TimeSpan(0, 0, 0, 0);
            int days = 0;
            int h = 0;
            int m = 0;
            int s = 0;
            string[] arr = null;
            string auxiliar = formattedTS.ToUpper();

            if (auxiliar.Contains('D'))
            {
                arr = auxiliar.Split('D');
                days = Convert.ToInt32(arr[0]);
                auxiliar = arr[1];
            }
            if (auxiliar.Contains('H'))
            {
                arr = auxiliar.Split('H');
                h = Convert.ToInt32(arr[0]);
                auxiliar = arr[1];
            }
            if (auxiliar.Contains('M'))
            {
                arr = auxiliar.Split('M');
                m = Convert.ToInt32(arr[0]);
                auxiliar = arr[1];
            }

            if (auxiliar.Contains('S'))
            {
                auxiliar = auxiliar.Replace('S', ' ').Trim();
                s = Convert.ToInt32(auxiliar);
            }

            return new TimeSpan(days, h, m, s);
        }

        public static IList<string> GetDirectories(string path)
        {
            if (!System.IO.Directory.Exists(path)) return new List<string>();
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(path);
            IEnumerable<string> list = info.GetDirectories().Select(o => o.Name.ToUpper());
            HashSet<string> hs = new HashSet<string>(list);
            foreach (string l in list)
            {
                System.IO.DirectoryInfo info3 = new System.IO.DirectoryInfo(path + "\\" + l);
                IEnumerable<string> list3 = info3.GetDirectories().Select(o => o.Parent + "\\" + o.Name.ToUpper());
                hs.UnionWith(list3);
            }

            return hs.ToList();
        }

        public static IList<System.IO.FileInfo> GetFiles(string rootpath)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            IEnumerable<System.IO.FileInfo> files = dir.GetFiles();

            IEnumerable<System.IO.DirectoryInfo> dirs = dir.GetDirectories();
            foreach (System.IO.DirectoryInfo d in dirs)
            {
                IEnumerable<System.IO.FileInfo> fs = d.GetFiles();
                files = files.Union(fs);
            }
            return files.ToList();
        }

        public static IList<string> GetFileNames(string path, string Ext)
        {
            if (!System.IO.Directory.Exists(path)) return new List<string>();
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(path);
            IEnumerable<string> list = info.GetFiles().Where(o => o.Extension.ToUpper().CompareTo(Ext) == 0).Select(o => o.Name.ToUpper().Replace(Ext, null));
            return new HashSet<string>(list).ToList();
        }

        public static void AcceptChanges<T>(ref IEnumerable<T> rows)
        {
            if (rows == null || rows.Count() == 0) return;
            IEnumerable<DataRow> rows2 = rows.Cast<DataRow>().Where(o => o != null);
            rows2 = rows2.Where(o => o.RowState != DataRowState.Detached).ToList();
            foreach (DataRow r in rows2) r.AcceptChanges();
            rows2 = null;
        }

        public string GetRowAsString(ref DataRow r)
        {
            string content = string.Empty;
            IEnumerable<DataColumn> cols = r.Table.Columns.OfType<DataColumn>().ToList();
            foreach (System.Data.DataColumn c in cols)
            {
                try
                {
                    content += c.ColumnName + ":\t\t" + r[c] + "\n";
                }
                catch (SystemException ex)
                {
                    r.SetColumnError(c, ex.Message);
                }
            }
            return content;
        }

        #region Funcs<>

        public static bool HasErrors<T>(IEnumerable<T> rows)
        {
            if (GetRowsInError<T>(rows).Count() != 0) return true;
            else return false;
        }

        public static IEnumerable<T> Deleted<T>(IEnumerable<T> rows)
        {
            Func<T, bool> find = o =>
               {
                   DataRow row = (object)o as DataRow;
                   if (row.RowState == DataRowState.Deleted) return true;
                   else return false;
               };
            return rows.Where<T>(find).ToList<T>();
        }

        /// <summary>
        /// Returns true if the row is either NULL, was Deleted or is Detached, else false
        /// </summary>
        /// <param name="row">DataRow to check</param>
        /// <returns></returns>
        public static bool IsNuDelDetch(DataRow row)
        {
            if (row != null && row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
            {
                return false;
            }
            else if (row == null || row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached) return true;
            else return false;
        }

        public static IEnumerable<T> NotDeleted<T>(IEnumerable<T> rows)
        {
            Func<T, bool> find = o =>
            {
                DataRow row = (object)o as DataRow;
                if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached) return true;
                else return false;
            };
            return rows.Where<T>(find).ToList<T>();
        }

        public static IEnumerable<T> GetRowsInError<T>(IEnumerable<T> rows)
        {
            Func<T, bool> find = s =>
            {
                bool errors = false;
                if (s != null)
                {
                    DataRow r = s as DataRow;
                    if (r.RowState != DataRowState.Detached && r.RowState != DataRowState.Deleted)
                    {
                        errors = r.HasErrors;
                    }
                }
                return errors;
            };

            return rows.Where<T>(find).ToList<T>();
        }

        public static T First<T>(string fieldToCompare, object valueToCompare, IEnumerable<T> rows)
        {
            Func<T, bool> compare = (i) =>
            {
                DataRow r = i as DataRow;
                if (Dumb.IsNuDelDetch(r)) return false;
                else
                {
                    if (!r.IsNull(fieldToCompare) && r.Field<object>(fieldToCompare).Equals(valueToCompare)) return true;
                    else return false;
                }
            };

            T result = rows.AsQueryable().FirstOrDefault<T>(compare);

            return result;
        }

        public static IEnumerable<T> SelectMany<T>(string fieldToCompare, object valueToCompare, IEnumerable<T> rows)
        {
            Func<T, IEnumerable<T>> compare = i =>
            {
                List<T> list = new List<T>();
                if (i.GetType().BaseType.Equals(typeof(DataRow)))
                {
                    DataRow r = i as DataRow;
                    if (r.RowState != DataRowState.Detached && r.RowState != DataRowState.Deleted)
                    {
                        if (!r.IsNull(fieldToCompare) && r.Field<object>(fieldToCompare).Equals(valueToCompare)) list.Add(i);
                    }
                }
                return list;
            };

            IEnumerable<T> results = rows.AsQueryable().SelectMany<T, T>(compare).ToList<T>();
            return results;
        }

        public static IEnumerable<T> Where<T>(string fieldToCompare, object valueToCompare, IEnumerable<T> rows)
        {
            Func<T, bool> compare = (i) =>
            {
                if (i.GetType().BaseType.Equals(typeof(DataRow)))
                {
                    DataRow r = i as DataRow;
                    if (r.RowState == DataRowState.Detached || r.RowState == DataRowState.Deleted) return false;
                    else
                    {
                        if (!r.IsNull(fieldToCompare) && r.Field<object>(fieldToCompare).Equals(valueToCompare)) return true;
                        else return false;
                    }
                }
                else
                {
                    object o = Convert.ChangeType(i, typeof(DataRow));
                    DataRow r = o as DataRow;
                    if (r.RowState == DataRowState.Detached || r.RowState == DataRowState.Deleted) return false;
                    else
                    {
                        if (!r.IsNull(fieldToCompare) && r.Field<object>(fieldToCompare).Equals(valueToCompare)) return true;
                        else return false;
                    }
                }
            };

            IEnumerable<T> results = rows.AsQueryable().Where<T>(compare).ToList<T>();

            return results;
        }

        #endregion Funcs<>

        #region BindingSources

        public static void LinkBS(ref BindingSource BS, DataTable table)
        {
            if (table == null) throw new ArgumentException("table is null", "table");
            if (BS == null) throw new ArgumentException("BindingSource is null", "BS");

            BS.EndEdit();
            BS.SuspendBinding();
            if (table.DataSet != null)
            {
                BS.DataSource = table.DataSet;
                BS.DataMember = table.TableName;
            }
            else BS.DataSource = table;
            BS.ResumeBinding();
        }

        public static void LinkBS(ref BindingSource BS, DataTable table, String Filter, String Sort)
        {
            LinkBS(ref BS, table);
            BS.EndEdit();
            BS.SuspendBinding();
            try
            {
                BS.Filter = Filter;
                BS.Sort = Sort;
            }
            catch (SystemException ex)
            {
            }
            BS.ResumeBinding();
        }

        public static string[] DeLinkBS(ref BindingSource BS)
        {
            if (BS == null) throw new ArgumentException("BindingSource is null", "BS");

            BS.EndEdit();
            BS.SuspendBinding();
            string[] sortFilter = new string[] { BS.Sort, BS.Filter };

            BS.Sort = string.Empty;
            BS.Filter = string.Empty;
            BS.DataMember = string.Empty;
            BS.DataSource = null;
            BS.ResumeBinding();
            return sortFilter;
        }

        #endregion BindingSources

        /// <summary>
        ///  Creates a non-repeated list of items (HashSet) from values in a given DataTable's Column
        /// </summary>
        /// <typeparam name="T">the type of desired elements in the output list</typeparam>
        /// <param name="column">input Column with items to generate the list from</param>
        /// <returns>a HashSet with elements of generic type</returns>
        public static IList<T> HashFrom<T>(IEnumerable<DataRow> array, String Field)
        {
            if (array == null) throw new ArgumentException("arra is null", "array");

            IEnumerable<object> array2 = NotNulls(array, Field).Select(o => o.Field<object>(Field));

            Func<object, T> converter = o => (T)Convert.ChangeType(o, typeof(T));

            IEnumerable<T> enumcol = array2.Distinct().Select<object, T>(converter);

            return enumcol.ToList<T>();
        }

        /// <summary>
        ///  Creates a non-repeated list of items (HashSet) from values in a given DataTable's Column
        /// </summary>
        /// <typeparam name="T">the type of desired elements in the output list</typeparam>
        /// <param name="column">input Column with items to generate the list from</param>
        /// <returns>a HashSet with elements of generic type</returns>
        public static IList<T> HashFrom<T>(IEnumerable<DataRowView> array, String Field)
        {
            if (array == null) throw new ArgumentException("array is null", "array");
            IEnumerable<DataRow> array2 = array.Select(o => o.Row as DataRow);
            return HashFrom<T>(array2, Field);
        }

        public static IList<T> HashFrom<T>(IEnumerable<DataGridViewRow> array, String Field)
        {
            if (array == null) throw new ArgumentException("array is null", "array");
            IEnumerable<DataRowView> array2 = array.Select(o => o.DataBoundItem as DataRowView);
            return HashFrom<T>(array2, Field);
        }

        /// <summary>
        ///  Creates a non-repeated list of items (HashSet) from values in a given DataTable's Column
        /// </summary>
        /// <typeparam name="T">the type of desired elements in the output list</typeparam>
        /// <param name="column">input Column with items to generate the list from</param>
        /// <returns>a HashSet with elements of generic type</returns>
        public static IList<T> HashFrom<T>(DataColumn column)
        {
            if (column.Table == null) throw new ArgumentException("column.Table is null", "column.Table");
            IEnumerable<DataRow> rows = column.Table.AsEnumerable();
            return HashFrom<T>(rows, column.ColumnName);
        }

        /// <summary>
        ///  Creates a non-repeated list of items of the given Row array under the given ColumnName
        /// </summary>
        /// <typeparam name="T">type of HashSet elements</typeparam>
        /// <param name="Rows">Row array to make list from</param>
        /// <param name="ColumnName">ColumnName of the Row array with items to generate the hash set from</param>
        /// <returns>a HashSet</returns>
        public static IList<T> HashFrom<T>(IEnumerable<DataRow> array, String Field, String FieldFilter, object FilterValue)
        {
            if (array == null) throw new ArgumentException("array is null", "array");

            IEnumerable<object> array2 = NotNulls(array, Field).Where(i => i.Field<object>(FieldFilter).Equals(FilterValue)).Select(o => o.Field<object>(Field));

            Func<object, T> converter = o => (T)Convert.ChangeType(o, typeof(T));

            IEnumerable<T> enumcol = array2.Distinct().Select<object, T>(converter);

            return enumcol.ToList<T>();
        }

        public static IEnumerable<DataRow> NotNulls(IEnumerable<DataRow> array, String Field)
        {
            return array.Where(o => !IsNuDelDetch(o) && o.Field<object>(Field) != null);
        }

        public static bool HasChanges(IEnumerable<DataRow> array)
        {
            if (array == null) return false;
            if (array.Count() == 0) return false;

            DataRow first = array.FirstOrDefault(h => HasChanges(h));
            // DataTable dt = array.FirstOrDefault().Table; //get table
            // DataRow first = array.FirstOrDefault(ChangesSelector(dt)); //get selector for table and
            if (first != null) return true;
            else return false;
        }

        public static bool HasChanges(DataRow r)
        {
            if (r.RowState == DataRowState.Modified) return true;
            if (r.RowState == DataRowState.Added) return true;
            if (r.RowState == DataRowState.Deleted) return true;
            else return false;

            // DataTable dt = r.Table; //get table
            //  return ChangesSelector(dt)(r); //get selector for table and
        }

        public static IEnumerable<DataRow> GetRowsWithChanges(IEnumerable<DataRow> array)
        {
            if (array.Count() == 0) return array;
            DataTable dt = array.FirstOrDefault().Table;
            Func<DataRow, bool> changessel = ChangesSelector(dt);
            return array.Where<DataRow>(changessel).ToList();
        }

        public static Func<DataRow, bool> ChangesSelector(DataTable dt)
        {
            IEnumerable<int> ords = dt.Columns.OfType<DataColumn>().Select(o => o.Ordinal).ToList();

            Func<DataRow, bool> CurrentModif = i =>
            {
                if (i.RowState == DataRowState.Deleted) return true;
                else if (i.RowState == DataRowState.Added) return true;
                if (!i.HasVersion(DataRowVersion.Current)) return false;
                if (!i.HasVersion(DataRowVersion.Original)) return false;

                foreach (int x in ords)
                {
                    if (HasChanged(i, x)) return true;
                }
                return false;
            };
            return CurrentModif;
        }

        public static bool HasChanged(DataRow i, int x)
        {
            object curr = i.Field<object>(x, DataRowVersion.Current);
            object org = i.Field<object>(x, DataRowVersion.Original);
            if (curr == null || org == null) return false;

            if (!curr.Equals(org)) return true;
            else return false;
        }

        /// <summary>
        /// Creates a non-repeated list of items from the given DataColumn where any occurrence of string "replace" has been replaced by string "by"
        /// </summary>
        /// <typeparam name="T">type of HashSet elements</typeparam>
        /// <param name="column">Table DataColumn to make list from</param>
        /// <param name="replace">String to replace on each item in the list</param>
        /// <param name="by">String to use as replacement on each item in the list when occurrence is found</param>
        /// <returns>a HashSet</returns>
        public static IList<T> HashFrom<T>(DataColumn column, T replace, T by)
        {
            if (column.Table == null) throw new ArgumentException("column.Table is null", "column.Table");
            string replace_ = replace.ToString();
            string by_ = by.ToString();
            var list = from i in column.Table.AsEnumerable()
                       where (i.RowState != DataRowState.Deleted && i.RowState != DataRowState.Detached && i.Field<object>(column) != null)
                       select i.Field<object>(column).ToString().Replace(replace_, by_);

            Func<object, T> converter = o => (T)Convert.ChangeType(o, typeof(T));
            
            IEnumerable<T> enumcol = list.Distinct().Select<object, T>(converter);
            return enumcol.ToList<T>();
        }
        /// <summary>
        /// Creates a non-repeated list of items from the given DataColumn 
        /// </summary>
        /// <typeparam name="T">type of HashSet elements</typeparam>
        /// <param name="column">Table DataColumn to make list from</param>
    
        /// <returns>a HashSet</returns>
     

        public static IEnumerable<T> Cast<T>(DataView view)
        {
            if (view == null) throw new Exception("DataView is null, Cannot Cast<T>");

            IEnumerable<DataRowView> vs = view.OfType<DataRowView>();
            return Cast<T>(vs);
        }

        public static IEnumerable<T> Cast<T>(IEnumerable<DataRowView> views)
        {
            if (views == null) throw new Exception("DataRowViews are null, Cannot IEnum<T> Cast<T>");
            return views.Select(o => o.Row).Cast<T>();
        }

        /// <summary>
        ///  Cast to a T generic type (normally a DataRow)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <returns></returns>
        public static T Cast<T>(DataRowView view)
        {
            if (view == null) throw new Exception("DataRowView is null, Cannot T Cast<T>");
            return (T)(object)view.Row;
        }

        public static IEnumerable<T> Cast<T>(DataGridView dgv)
        {
            IEnumerable<DataGridViewRow> vs = dgv.Rows.OfType<DataGridViewRow>();
            return Cast<T>(vs);
        }

        /// <summary>
        ///   Cast to a IEnumerable of generic type (normally a IEnumerable of DataRow)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grids"></param>
        /// <returns></returns>
        public static IEnumerable<T> Cast<T>(IEnumerable<DataGridViewRow> grids)
        {
            Type tipo = typeof(T);
            IEnumerable<DataRowView> views = grids.Select(o => o.DataBoundItem as DataRowView);
            if (tipo.Equals(typeof(DataRowView))) return (IEnumerable<T>)views;
            else return Cast<T>(views);
        }

        /// <summary>
        /// Cast to a T generic type (normally a DataRow)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <returns></returns>
        public static T Cast<T>(DataGridViewRow view)
        {
            Type tipo = typeof(T);
            object aux = null;
            if (view == null) return (T)aux;
            DataRowView v = view.DataBoundItem as DataRowView;
            if (v == null) return (T)aux;
            if (tipo.Equals(typeof(DataRowView)))
            {
                aux = v;
                return (T)aux;
            }
            else if (tipo.BaseType.Equals(typeof(DataRow)))
            {
                return Cast<T>(v);
            }
            else if (tipo.Equals(typeof(DataRow)))
            {
                return Cast<T>(v);
            }
            else throw new ArgumentException("not implemented");
        }

        /// <summary>
        /// Cast any object of type DataGridViewRow or DataRowView to T
        /// </summary>
        /// <typeparam name="T">Generic output, normally a DataRow</typeparam>
        /// <param name="view">A DataGridViewRow or a DataRowView</param>
        /// <returns></returns>
        public static object Cast<T>(object view)
        {
            if (view == null) return view;
            Type tipo = view.GetType();
            if (tipo.Equals(typeof(DataGridViewRow))) return Cast<T>(view as DataGridViewRow);
            else if (tipo.Equals(typeof(DataRowView))) return Cast<T>(view as DataRowView);
            else if (tipo.Equals(typeof(DataView))) return Cast<T>(view as DataView);
            else if (tipo.Equals(typeof(DataGridView))) return Cast<T>(view as DataGridView);
            else throw new ArgumentException("not implemented");
        }
    }
}