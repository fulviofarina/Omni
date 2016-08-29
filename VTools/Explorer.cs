using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace VTools
{
    public partial class Explorer : UserControl
    {
        private DataSet set;
        private DataTable CurrentTable;

        public Explorer(ref DataSet dataset)
        {
            InitializeComponent();

            set = dataset;
            HashSet<string> hs = new HashSet<string>();
            foreach (DataTable table in set.Tables)
            {
                hs.Add(table.TableName);
            }
            Rsx.Dumb.FillABox(Box, hs, true, false);
            Box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Box.Text = set.Tables.OfType<DataTable>().First().TableName;

            DGV.AutoGenerateColumns = true;
            DGV.DataSource = this.BS;
        }

        private void Box_TextChanged(object sender, EventArgs e)
        {
            if (Box.AutoCompleteCustomSource.Contains(Box.Text))
            {
                RefreshTable();
            }
        }

        public void RefreshTable()
        {
            try
            {
                this.CurrentTable = set.Tables[Box.Text];
                Rsx.Dumb.LinkBS(ref this.BS, CurrentTable, filterbox.Text, sortBox.Text);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void filterbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Box_TextChanged(sender, EventArgs.Empty);
            }
        }

        private void expBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && expBox.Tag != null)
            {
                DataColumn colTable = expBox.Tag as DataColumn;

                colTable.Expression = expBox.Text;
                Box_TextChanged(sender, EventArgs.Empty);
            }
        }

        private void DGV_CurrentCellChanged(object sender, EventArgs e)
        {
            if (DGV.CurrentCell == null) return;
            if (DGV.CurrentCell.ColumnIndex < 0) return;
            if (this.BS.Current == null) return;

            string columnName = DGV.Columns[DGV.CurrentCell.ColumnIndex].Name;
            DataRow row = (this.BS.Current as DataRowView).Row;
            if (row == null) return;
            DataColumn colTable = row.Table.Columns[columnName];
            if (colTable == null) return;

            expBox.Text = colTable.Expression;
            expBox.Tag = colTable;
            columnBox.Text = columnName;
        }

        private void AddCol_Click(object sender, EventArgs e)
        {
            if (CurrentTable == null) return;
            if (ColumnTypeBox.Text.Equals(string.Empty)) return;
            if (columnBox.Text.Equals(string.Empty)) return;

            string type = ColumnTypeBox.Text.ToUpper();

            try
            {
                if (type.CompareTo("DOUBLE") == 0) CurrentTable.Columns.Add(columnBox.Text, typeof(double), expBox.Text);
                else if (type.CompareTo("INT") == 0) CurrentTable.Columns.Add(columnBox.Text, typeof(int), expBox.Text);
                else if (type.CompareTo("STRING") == 0) CurrentTable.Columns.Add(columnBox.Text, typeof(string), expBox.Text);
                else if (type.CompareTo("BOOL") == 0) CurrentTable.Columns.Add(columnBox.Text, typeof(bool), expBox.Text);
                else if (type.CompareTo("DATETIME") == 0) CurrentTable.Columns.Add(columnBox.Text, typeof(DateTime), expBox.Text);
                else if (type.CompareTo("DECIMAL") == 0) CurrentTable.Columns.Add(columnBox.Text, typeof(decimal), expBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }

            Box_TextChanged(sender, EventArgs.Empty);
        }

        private void columnBox_TextChanged(object sender, EventArgs e)
        {
            if (CurrentTable == null) return;

            if (CurrentTable.Columns.Contains(columnBox.Text)) AddCol.Enabled = false;
            else AddCol.Enabled = true;
        }

        private void sortBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Box_TextChanged(sender, EventArgs.Empty);
            }
        }

        private void ExpressionLabel_Click(object sender, EventArgs e)
        {
            expBox_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }
    }
}