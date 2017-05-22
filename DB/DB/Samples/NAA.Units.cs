using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class MatSSFRow
        {
        }

        partial class UnitDataTable : IColumn
        {
            // private DataColumn[] changeables;
            private DataColumn[] nonNullables;

            /// <summary>
            /// fix this to use windows user
            /// </summary>
            public bool defaultValue
            {
                //TODO: windows user instead
                get
                {
                    // LINAA set = this.DataSet as LINAA;
                    return !(this.DataSet as LINAA).SSFPref.FirstOrDefault().Overrides;
                }
            }

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnChRadius, this.columnChLength,
                            this.columnkth,this.columnkepi,
                            this.columnChCfg,
                            this.columnBellFactor,
                            this.pEpiColumn,
                            this.pThColumn,
                            this.WGtColumn,
                            this.nFactorColumn
                           
                        };
                    }
                    return nonNullables;
                }
            }

            /*
            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnChRadius, this.columnChLength,
                            this.columnkepi,this.columnkth,
                            this.columnChCfg,
                            this.columnLastCalc,
                           this.columnLastChanged,
                           this.columnWGt,
                           this.nFactorColumn,
                                  this.columnBellFactor,
                                  this.pEpiColumn,
                                  this.pThColumn
                               // this.SSFTableColumn
                        };
                    }
                    return nonNullables;
                }
            }
            */

            /// <summary>
            /// I think it is perfect like this, don't mess it up
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e">     </param>
            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                DataColumn c = e.Column;
                if (!NonNullables.Contains(c) && c!= this.SSFTableColumn) return;

                DataRow row = e.Row;
                UnitRow r = row as UnitRow;

                try
                {
                    r.Check(c);
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }

            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                if (!NonNullables.Contains(e.Column)) return;

                DataRow row = e.Row;
                UnitRow r = row as UnitRow;

                try
                {
                    bool change = (e.ProposedValue.ToString().CompareTo(e.Row[e.Column].ToString()) != 0);

                    if (change)
                    {
                        r.ValueChanged();
                    }
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }

        partial class UnitRow : IRow
        {
            public void Check(DataColumn c)
            {
                bool nullo = EC.CheckNull(c, this);// string.IsNullOrEmpty(e.Row.GetColumnError(e.Column));

                if (this.tableUnit.BellFactorColumn == c)
                {
                    if (nullo)
                    {
                        BellFactor = 1;
                    }
                }
                else if (this.tableUnit.kepiColumn == c)
                {
                    if (nullo)
                    {
                        kepi = 1;
                    }
                }
                else if (this.tableUnit.kthColumn == c)
                {
                    if (nullo)
                    {
                        kth = 0.6;
                    }
                }
                else if (this.tableUnit.ChRadiusColumn == c)
                {
                    // if (Convert.ToDouble(e.ProposedValue) == 0) e.ProposedValue = 1 ;
                    if (nullo)
                    {
                        ChRadius = 24;
                    }
                }
                else if (this.tableUnit.ChLengthColumn == c)
                {
                    if (nullo)
                    {
                        ChLength = 300;
                    }
                }
                else if (c == this.tableUnit.SSFTableColumn)
                {
                    checkSSFTable(c, nullo);
                }
                else if (c == this.tableUnit.ChCfgColumn)
                {
                    if (nullo) ChCfg = "0";
                    else
                    {
                        // bool nullFluxType = (EC.CheckNull(this.tableChannels.FluxTypeColumn, this));
                        checkChCfg();
                    }
                }
                else if (c == this.tableUnit.WGtColumn)
                {
                    if (nullo) WGt = 1;
                }
                else if (c == this.tableUnit.BellFactorColumn)
                {
                    if (nullo) BellFactor = 1.16;
                }
                else if (c == this.tableUnit.nFactorColumn)
                {
                    if (nullo) nFactor = 0.5;
                }
                else if (c == this.tableUnit.pThColumn)
                {
                    if (nullo) pTh = 0.964;
                }
                else if (c == this.tableUnit.pEpiColumn)
                {
                    if (nullo) pEpi = 0.82;
                }
                
            }

            public bool CheckErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                colsInE = colsInE.Intersect(this.tableUnit.NonNullables).ToArray();
                int count = colsInE.Count();
                return count == 0;
            }

            public void Clean()
            {
                SetGtChNull();
                SetGtNull();
                SetGtMCNull();
                SetMCLNull();
                SetPXSNull();
                SetEXSNull();

                SetSSFTableNull();
                AcceptChanges();
            }

            public MatSSFDataTable GetMatSSFTable(string path)
            {
                LINAA.MatSSFDataTable dt = new MatSSFDataTable();

                if (!IsSSFTableNull())
                {
                    byte[] arr = SSFTable;
                    Rsx.Dumb.Tables.ReadDTBytes(path, ref arr, ref dt);
                }

                return dt;
            }

            public void SetParent<T>(ref T row)
            {
                if (EC.IsNuDelDetch(row as DataRow)) return;
                bool isChannel = row.GetType().Equals(typeof(ChannelsRow));
                bool isMatrix = row.GetType().Equals(typeof(MatrixRow));
                if (isChannel)
                {
                    ChannelsRow c = row as ChannelsRow;
                    setChannel(ref c);
                }
                else if (!isMatrix)
                {
                    LINAA.VialTypeRow v = row as VialTypeRow;
                    setVial(ref v);
                }
                else
                {
                    MatrixRow m = row as MatrixRow;
                    setMatrix(ref m);
                }
            }

            /// <summary>
            /// RESET THE UNIT TODO SO IT CAN BE RECALCULATED
            /// </summary>
            public void ValueChanged()
            {
                this.LastChanged = DateTime.Now;
                if (!ToDo) ToDo = true;
            }

            private void checkChCfg()
            {
                if (EC.CheckNull(this.tableUnit.WGtColumn, this) || this.tableUnit.defaultValue)
                {
                    if (ChCfg.Contains("2"))
                    {
                        WGt = 0.67;
                        // BellFactor = 1.16;
                    }
                    else if (ChCfg.Contains("1"))
                    {
                        WGt = 0.93;
                        // BellFactor = 1.30;
                    }
                    else
                    {
                        WGt = 1;
                        // BellFactor = 1.16;
                    }
                }
                if (EC.CheckNull(this.tableUnit.BellFactorColumn, this) || this.tableUnit.defaultValue)
                {
                    if (ChCfg.Contains("2"))
                    {
                        // WGt = 0.67;
                        BellFactor = 1.16;
                    }
                    else if (ChCfg.Contains("1"))
                    {
                        // WGt = 0.93;
                        BellFactor = 1.30;
                    }
                    else
                    {
                        // WGt = 1;
                        BellFactor = 1.16;
                    }
                }
            }

            private void checkSSFTable(DataColumn c, bool nullo)
            {
                LINAA linaa = (this.tableUnit.DataSet as LINAA);
                //if there are rows bytes but no rows in the table, LOAD
                if (GetMatSSFRows().Count() == 0 && !nullo)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";
                    MatSSFDataTable dt = GetMatSSFTable(path);
                    linaa.MatSSF.Merge(dt, false, MissingSchemaAction.AddWithKey);

                    //refreshh nullo
               

                }
                //if set null, delete the child rows (if any)
                else if (nullo)
                {
                    IEnumerable<MatSSFRow> ssfs = GetMatSSFRows();
                    if (ssfs.Count() != 0)
                    {
                        linaa.Delete(ref ssfs);
                        linaa.MatSSF.AcceptChanges();
                    }
                }

                nullo = EC.CheckNull(c, this);// string.IsNullOrEmpty(e.Row.GetColumnError(e.Column));

                //finally... set todo to true
                if (nullo || GetMatSSFRows().Count() == 0)
                {
                    if (!ToDo) ToDo = true;
                }
                else if (ToDo) ToDo = false;
            }

            /// <summary>
            /// Sets the channel data
            /// </summary>
            /// <param name="c"></param>
            private void setChannel(ref LINAA.ChannelsRow c)
            {
                this.kth = c.kth;
                this.kepi = c.kepi;
                this.ChCfg = c.FluxType;
                this.BellFactor = c.BellFactor;
                this.SubSamplesRow.FC = c.FC;
                this.WGt = c.WGt;
                this.nFactor = c.nFactor;
                this.pTh = c.pTh;
                this.pEpi = c.pEpi;
            }

            private void setMatrix(ref MatrixRow m)
            {
                if (EC.IsNuDelDetch(m)) return;
                if (EC.IsNuDelDetch(SubSamplesRow)) return;
                SubSamplesRow.MatrixID = m.MatrixID;
            }

            private void setVial(ref VialTypeRow v)
            {
                if (EC.IsNuDelDetch(v)) return;
                if (EC.IsNuDelDetch(SubSamplesRow)) return;
                if (!v.IsIsRabbitNull() && !v.IsRabbit) SubSamplesRow.VialTypeRow = v;
                else
                {
                    SubSamplesRow.VialTypeRowByChCapsule_SubSamples = v;

                    this.ChRadius = v.InnerRadius;
                    this.ChLength = v.MaxFillHeight;
                }
            }

            /// <summary>
            /// sets the vial container data
            /// </summary>
            /// <param name="v"></param>

            /*
            /// <summary>
            /// Sets the matrix data
            /// </summary>
            /// <param name="m"></param>
            public void SetMatrix(ref LINAA.MatrixRow m)
            {
                // LINAA.MatrixRow m = this.MatrixRow;

                this.SubSamplesRow.MatrixID = m.MatrixID;
                this.Density = m.MatrixDensity;
                this.Content = m.MatrixComposition;
            }
            */
        }
    }
}