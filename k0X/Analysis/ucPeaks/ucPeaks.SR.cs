using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;
using Rsx.DGV;

namespace k0X
{
    public partial class ucPeaks
    {
        protected void Field_Click(object sender, EventArgs e)
        {
            // if (!sender.GetType().Equals(typeof(ToolStripComboBox))) return;
            if (Yi.Text.Equals(string.Empty) || Xj.Text.Equals(string.Empty)) return;

            ToolStripComboBox box = sender as ToolStripComboBox;

            string boxtext = box.Text;
            bool empty = string.IsNullOrWhiteSpace(boxtext);

            if (!empty && box.AutoCompleteCustomSource.Contains(boxtext))
            {
                if (box.Equals(weighterbox))
                {
                    string weighert = weighterbox.Text;
                    this.Linaa.Peaks.FlagColumn.Expression = "IIF(ID<0 OR Selected=FALSE OR " + weighert + "<=0 ,'0','1')";
                    // this.Linaa.Peaks.wColumn.Expression = "IIF(Flag=1,(1/(" + weighert + "*" + weighert + ")),'0')";
                }
                else if (!box.Equals(Xij) && !box.Equals(XijTip))
                {
                    DataView view = BS.List as DataView;
                    MakeSelectReject(ref view, ref SRDGV);
                }
                else Refresh_Click(sender, e);
            }
            else
            {
                if (box.Equals(Xij))
                {
                    if (this.Sample != null)
                    {
                        if (this.Sample.Comparator) this.Xij.Text = "Fc";
                        else this.Xij.Text = "ppm";
                    }
                    else if (this.Samples != null) this.Xij.Text = "ppm";
                    else this.Xij.Text = "ppm";
                }
                else if (box.Equals(Yi) && empty) this.Yi.Text = "Energy";
            }
        }

        protected void SRDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!xTable.GoodDGV(dgv)) return;
            if (e.ColumnIndex < 0) return;
            bool energyCol = dgv.Columns[e.ColumnIndex].HeaderText.CompareTo(this.Linaa.Peaks.EnergyColumn.ColumnName) == 0;
            if (energyCol)
            {
                LINAA.IPeakAveragesRow r = Dumb.Cast<LINAA.IPeakAveragesRow>((DataRowView)this.AvgPeakBS.Current);
                this.FindGammaCorrections(r.Radioisotope, r.Energy, r.Sample);
                this.MainTab.SelectTab(this.InterferencesTab);
            }
            else Lock.Checked = true;
        }

        protected void SRDGV_MouseEnter(object sender, EventArgs e)
        {
            this.CurrentDGV = sender as DataGridView;

            if (this.CurrentDGV.CurrentCell != null) return;

            this.CurrentDGV.CurrentCell = this.CurrentDGV.FirstDisplayedCell;
        }

        protected void SRDGV_MouseHover(object sender, EventArgs e)
        {
            this.SaveItem.Enabled = Rsx.Dumb.HasChanges(this.Linaa.Peaks);
        }

        protected void AnyDGV_KeyDown(object sender, KeyEventArgs e)
        {
            this.CurrentDGV = (DataGridView)sender;
            Keys key = e.KeyCode;
            if (key == Keys.P || key == Keys.E || key == Keys.Space || key == Keys.Delete)
            {
                if (!xTable.GoodDGV(this.CurrentDGV)) return;
            }
            else return;

            IEnumerable<DataGridViewCell> cells = this.CurrentDGV.SelectedCells.OfType<DataGridViewCell>();
            cells = cells.Where(c => c.ColumnIndex >= 2);
            cells = cells.Where(c => xTable.GoodColRowCell(this.CurrentDGV, c.ColumnIndex, c.RowIndex));
            cells = cells.ToList();

            int count = cells.Count();

            if (count == 0) return;
            //  if (count > 1)
            {
                this.DeLinkOtherBS(true);
                this.DeLinkBS(true);
            }

            System.Collections.Generic.HashSet<LINAA.IRequestsAveragesRow> irs = new System.Collections.Generic.HashSet<LINAA.IRequestsAveragesRow>();

            foreach (DataGridViewCell cell in cells)
            {
                LINAA.PeaksRow r = (LINAA.PeaksRow)cell.Tag;
                LINAA.PeaksRow newRow = null;
                try
                {
                    switch (key)
                    {
                        #region Space

                        case Keys.Space:
                            {
                                int aux = r.ID;
                                if (aux < 0)
                                {
                                    aux = Math.Abs(aux);
                                    r.ID = aux;
                                    cell.Style.ForeColor = Color.Black;
                                    cell.Style.SelectionForeColor = Color.Black;
                                }
                                else
                                {
                                    aux = -1 * Math.Abs(aux);
                                    r.ID = aux;
                                    cell.Style.ForeColor = Color.Red;
                                    cell.Style.SelectionForeColor = Color.Red;
                                }
                                newRow = r;
                            }
                            break;

                        #endregion Space

                        #region P (Peak)

                        case Keys.P:
                            {
                            }
                            break;

                        #endregion P (Peak)

                        #region E (Error)

                        case Keys.E:
                            {
                                if (r.RowError.Contains("Duplicated by Row: "))
                                {
                                    int rowduplicatingIndex = Convert.ToInt32(r.RowError.Split(':')[1].Split('\n')[0].ToString());
                                    r.RowError = "Duplicating Row: " + rowduplicatingIndex + "\n";
                                    newRow = r.Table.Rows[rowduplicatingIndex] as LINAA.PeaksRow;
                                    newRow.RowError = "Duplicated by Row: " + r.Table.Rows.IndexOf(r) + "\n";
                                    cell.ErrorText = "Duplicated by Row: " + r.Table.Rows.IndexOf(r) + "\n";
                                    cell.Tag = newRow;
                                    cell.Value = newRow[Xij.Text];
                                    cell.ToolTipText = decimal.Round(Convert.ToDecimal(newRow[XijTip.Text]), Convert.ToInt16(TipDigits.Text)).ToString();
                                }
                            }
                            break;

                        #endregion E (Error)

                        #region Del (Delete)

                        case Keys.Delete:
                            {
                                r.ID = -1 * Math.Abs(r.ID);
                                if (r.RowError.Contains("Duplicated by Row: "))
                                {
                                    int rowduplicatingIndex = Convert.ToInt32(r.RowError.Split(':')[1].Split('\n')[0].ToString());
                                    newRow = r.Table.Rows[rowduplicatingIndex] as LINAA.PeaksRow;
                                    newRow.RowError = null;
                                    newRow.ID = -1 * Math.Abs(newRow.ID);
                                    cell.ErrorText = null;
                                    cell.Tag = newRow;
                                    cell.Value = newRow[Xij.Text];
                                    cell.Style.ForeColor = Color.Red;
                                    cell.ToolTipText = decimal.Round(Convert.ToDecimal(newRow[XijTip.Text]), Convert.ToInt16(TipDigits.Text)).ToString();
                                }
                                else
                                {
                                    cell.Style.BackColor = Color.Beige;
                                    cell.Tag = null;
                                }
                                r.Delete();
                            }
                            break;

                        #endregion Del (Delete)
                    }

                    if (!Dumb.IsNuDelDetch(newRow))
                    {
                        if (Dumb.IsNuDelDetch(newRow.IPeakAveragesRowParent)) newRow.Delete();
                        if (Dumb.IsNuDelDetch(newRow.IRequestsAveragesRowParent)) newRow.Delete();
                        else irs.Add(newRow.IRequestsAveragesRowParent);
                    }
                }
                catch (SystemException ex)
                {
                    this.Linaa.AddException(ex);
                }
            }

            IEnumerable<LINAA.IRequestsAveragesRow> irss = irs as IEnumerable<LINAA.IRequestsAveragesRow>;

            DB.Tools.WC.FindSDs(ref irss);

            this.LinkBS(this.CurrentFilter, true);
            this.LinkOtherBS(this.CurrentFilter, true);

            this.CurrentDGV.ClearSelection();
        }

        protected void SelectCurrentCell(object sender, DataGridViewCellEventArgs e)
        {
            this.CurrentDGV = (DataGridView)sender;
            if (!xTable.GoodDGV(this.CurrentDGV) || this.Lock.Checked) return;
            if (!xTable.GoodColRowCell(CurrentDGV, e.ColumnIndex, e.RowIndex)) return;

            CurrentDGV.CurrentCell = CurrentDGV[e.ColumnIndex, e.RowIndex];

            DB.LINAA.PeaksRow tag = (DB.LINAA.PeaksRow)this.CurrentDGV.CurrentCell.Tag;

            IEnumerable<DataRowView> view = this.BS.List.Cast<DataRowView>();

            DataRowView vi = view.FirstOrDefault(v => v.Row.Equals(tag));

            this.BS.Position = this.BS.List.IndexOf(vi);

            this.fbox.Text = tag.SubSamplesRow.f.ToString();
            this.alphabox.Text = tag.SubSamplesRow.Alpha.ToString();
            this.Geobox.Text = tag.SubSamplesRow.GeometryName;
            this.massbox.Text = tag.SubSamplesRow.DryNet.ToString();
            this.AvgPeakBS.Position = this.AvgPeakBS.Find("Energy", tag.Energy);
            this.AvgIsotopesBS.Position = this.AvgIsotopesBS.Find("Radioisotope", tag.Iso);
            //this.AvgElementBS.Position = this.AvgElementBS.Find("Sample", tag.Sample);
        }

        protected void AvgDGV_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            string header = e.Column.HeaderText;
            e.Column.DefaultCellStyle = new DataGridViewCellStyle();
            e.Column.DefaultCellStyle.Format = "N" + DigitsBox.Text;
            if (header.CompareTo(Xij.Text) == 0)
            {
                e.Column.DefaultCellStyle.BackColor = Color.Honeydew;
                e.Column.DisplayIndex = 4;
            }
            else
            {
                e.Column.DefaultCellStyle.BackColor = Color.White;
            }
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            if (this.CurrentDGV != null) this.CurrentDGV.Focus();
            Timer.Enabled = false;
        }
    }
}