using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;

//this works too good YEAH!!!

namespace DB.UI
{
    public partial class Picker : IPicker
    {
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
            //if they measure the same row size
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

        public Picker(ref DataGridView ToDgv, ref DataGridView[] FromDgv, bool Add, ref LINAA.ExceptionsDataTable Exceptions, int relNr = 0)
        {
            this.exceptions = Exceptions;
            this.fromDgvs = FromDgv;

            this.toAdd = Add; //to add or set

            //To
            this.todgv = ToDgv;
            this.to = Rsx.DGV.Control.GetDataSource<DataTable>(ref todgv);

            if (to == null) throw new SystemException("Could not find DataTable to send data to");

            IEnumerable<DataRelation> rels = to.ParentRelations.OfType<DataRelation>();

            //From
            int i = 0;
            fromdgv = null;
            while (relation == null)
            {
                try
                {
                    //find dgv from list of fromDgVs
                    fromdgv = fromDgvs[i];
                    //get table
                    from = Rsx.DGV.Control.GetDataSource<DataTable>(ref fromdgv);
                    //if null
                    if (from == null) throw new SystemException("Could not find DataTable to take data from");

                    //find relations for the "from" data table...
                    rels = rels.Where(r => r.ParentTable.TableName.CompareTo(from.TableName) == 0);

                    //if there is more than one relation for the same table
                    //as with Vials and Rabbits... and SubSamples (Child)
                    if (rels.Count() > 1)
                    {
                        //take the one with the number provided as input argument
                        relation = rels.ElementAt(relNr);
                    }
                    //else take the firts relation found...
                    else relation = rels.FirstOrDefault(r => r.ParentTable.TableName.CompareTo(from.TableName) == 0);
                    i++; //loop until some relation is found
                }
                catch (SystemException ex)
                {
                    fromdgv = null;
                    exceptions.AddExceptionsRow(ex);
                    break;
                }
            }
            if (fromdgv != null) fromdgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            else
            {
                SystemException ex;
                ex = new SystemException("Could not find DataRelation to establish a link between tables");
                exceptions.AddExceptionsRow(ex);
                throw ex;
            }
        }
    }
}