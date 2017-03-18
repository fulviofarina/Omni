﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;

namespace DB.UI
{
    public class Picker : IPicker
    {
        private DataGridView fromdgv = null;

        public DataGridView FromDgv
        {
            get { return fromdgv; }
            set { fromdgv = value; }
        }

        private DataGridView todgv = null;

        public DataGridView ToDgv
        {
            get { return todgv; }
            set { todgv = value; }
        }

        private DataTable from = null;

        public DataTable FromDt
        {
            get { return from; }
            set { from = value; }
        }

        private DataTable to = null;

        public DataTable ToDt
        {
            get { return to; }
            set { to = value; }
        }

        private bool toAdd = false;
        private DataRelation relation = null;

        public DataRelation Relation
        {
            get { return relation; }
            set { relation = value; }
        }

        private LINAA.ExceptionsDataTable exceptions;

        public LINAA.ExceptionsDataTable Exceptions
        {
            get { return exceptions; }
            set { exceptions = value; }
        }

        private List<DataRow> ToRowList;

        public void LinkDGVs()
        {
            from.BeginLoadData();

            HashSet<int> hint = new HashSet<int>();

            HashSet<DataRow> FromRows = new HashSet<DataRow>();
            HashSet<DataRow> ToRows = new HashSet<DataRow>();
            string selected = "Selected";
            DataColumn col = null;

            if (!this.from.Columns.Contains(selected))
            {
                col = new DataColumn(selected, typeof(int), string.Empty);

                this.from.Columns.Add(col);
            }
            else col = this.from.Columns[selected];

            foreach (DataRow f in from.Rows)
            {
                try
                {
                    f.SetField<int>(selected, 100);
                }
                catch (SystemException ex)
                {
                    exceptions.AddExceptionsRow(ex);
                }
            }

            DataRow tofind = null;
            DataRow tr = null;
            foreach (DataGridViewCell cell in todgv.SelectedCells)
            {
                try
                {
                    if (hint.Add(cell.OwningRow.Index))
                    {
                        tr = Dumb.Cast<DataRow>(cell.OwningRow);

                        tofind = tr.GetParentRow(relation);
                        if (tofind != null) FromRows.Add(tofind);
                        ToRows.Add(tr);
                    }
                }
                catch (SystemException ex)
                {
                    exceptions.AddExceptionsRow(ex);
                }
            }

            hint.Clear();

            this.ToRowList = new List<DataRow>(ToRows);
            this.ToRowList.Reverse();

            List<DataRow> fromRowList = new List<DataRow>(FromRows);
            fromRowList.Reverse();

            int i = 1;
            foreach (DataRow f in fromRowList)
            {
                try
                {
                    f.SetField<int>(col, i);
                    i++;
                }
                catch (SystemException ex)
                {
                    exceptions.AddExceptionsRow(ex);
                }
            }

            BindingSource bs = this.fromdgv.DataSource as BindingSource;
            bs.Sort = selected + " asc";

            foreach (DataGridViewRow fdgv in fromdgv.Rows)
            {
                try
                {
                    DataRow fr = Dumb.Cast<DataRow>(fdgv);

                    if (fromRowList.Contains(fr))
                    {
                        fdgv.HeaderCell.Style.ForeColor = System.Drawing.Color.Black;
                        fdgv.HeaderCell.Value = fr.Field<int>(selected);
                        fdgv.Selected = true;
                    }
                    else
                    {
                        fdgv.Selected = false;
                    }
                }
                catch (SystemException ex)
                {
                    exceptions.AddExceptionsRow(ex);
                }
            }

            from.EndLoadData();
            FromRows.Clear();
            fromRowList.Clear();
        }

        public void DeLinkDGVs()
        {
            //in this order...
            BindingSource bs = this.fromdgv.DataSource as BindingSource;
            bs.Sort = string.Empty;
            if (this.from.Columns.Contains("Selected")) from.Columns.Remove("Selected");
            todgv.Refresh();
        }

        public bool Take()
        {
            to.BeginLoadData();
            from.BeginLoadData();

            bool cancel = false;
            HashSet<DataRow> FromRows = new HashSet<DataRow>();
            foreach (DataGridViewRow fdgv in fromdgv.SelectedRows)
            {
                if (fdgv.DataBoundItem != null)
                {
                    FromRows.Add(((DataRowView)fdgv.DataBoundItem).Row);
                }
            }
            List<DataRow> FromRowList = new List<DataRow>(FromRows);
            FromRowList.Reverse();

            if (FromRowList.Count == 1)
            {
                DataRow fr = FromRowList[0];

                if (fr.HasErrors)
                {
                    MessageBox.Show("The items you selected have errors:\n\nThe items are missing critical data for stablishing the proper relationships.\n\nThis software depends strongly on the completeness of the provided data due to the nature of its data-linking structure\n\n", "Sorry but I cannot take that!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cancel = true;
                }
                else
                {
                    foreach (DataRow t in ToRowList)
                    {
                        int ccnt = relation.ParentColumns.Count();

                        for (int i = 0; i < ccnt; i++)
                        {
                            string parenName = relation.ParentColumns[i].ColumnName;
                            object value = fr[parenName];
                            string cName = relation.ChildColumns[i].ColumnName;

                            t[cName] = value;
                        }
                    }
                }
            }
            else if (FromRowList.Count != 0 && (FromRowList.Count == ToRowList.Count))
            {
                if (EC.HasErrors<DataRow>(FromRowList.ToArray()))
                {
                    MessageBox.Show("The items you selected have errors:\n\nThe items are missing critical data for stablishing the proper relationships.\n\nThis software depends strongly on the completeness of the provided data due to the nature of its data-linking structure\n\n", "Sorry but I cannot take that!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cancel = true;
                }
                else
                {
                    for (int i = 0; i < ToRowList.Count; i++)
                    {
                        foreach (DataColumn col in relation.ChildColumns)
                        {
                            object value = FromRowList[i][col.ColumnName];
                            ToRowList[i][col.ColumnName] = value;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("The number of items taken (" + FromRowList.Count + ") is not equal to the number of items " +
                "to relate to (" + ToRowList.Count + ").\n\nMake sure that the number of items taken is either:\n\n - Just one.\n\n - Equal to " +
                "the number of items that you selected for setting the relationships.", "Parity mismatch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancel = true;
            }

            from.EndLoadData();
            to.EndLoadData();

            return cancel;
        }

        public Picker(ref DataGridView ToDgv, ref DataGridView[] FromDgv, bool Add)
        {
            toAdd = Add; //to add or set

            //To
            todgv = ToDgv;
            to = Rsx.DGV.Control.GetDataSource<DataTable>(ref todgv);

            if (to == null) throw new SystemException("Could not find DataTable to send data to");

            IEnumerable<DataRelation> rels = to.ParentRelations.OfType<DataRelation>();

            //From
            int i = 0;
            fromdgv = null;
            while (relation == null)
            {
                try
                {
                    fromdgv = FromDgv[i];
                    from = Rsx.DGV.Control.GetDataSource<DataTable>(ref fromdgv);
                    if (from == null) throw new SystemException("Could not find DataTable to take data from");
                    relation = rels.FirstOrDefault(r => r.ParentTable.TableName.CompareTo(from.TableName) == 0);
                    i++;
                }
                catch (SystemException ex)
                {
                    fromdgv = null;
                    break;
                }
            }
            if (fromdgv != null) fromdgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            else throw new SystemException("Could not find DataRelation to establish a link between tables");

            exceptions = new LINAA.ExceptionsDataTable();
        }
    }
}