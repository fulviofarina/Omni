using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {

        partial class MatSSFRow
        {

            public  void SetContent(string[] content, string[] ZEl)
            {

                string radioisotope = ZEl[1] + "-" + (Convert.ToInt32(ZEl[2]) + 1).ToString();
                string targetIsotope = ZEl[1] + "-" + ZEl[2];

                RadioIsotope = radioisotope;
                TargetIsotope = targetIsotope;
                //    m.SSF = -1;

                //    m.GFast = -1;
                SigB = Convert.ToDouble(content[3]);
                Weight = Convert.ToDouble(content[1]);
                ND = Convert.ToDouble(content[2]);
                if (content.Length == 6)
                {
                    SSF = Convert.ToDouble(content[4]);
                    GFast = Convert.ToDouble(content[5]);
                }
                else if (content.Length == 5) GFast = Convert.ToDouble(content[4]);

            }
        }
        partial class UnitDataTable : IColumn
        {
            private DataColumn[] changeables;
            private DataColumn[] nonNullables;

            public DataColumn[] Changeables
            {
                get
                {
                    if (changeables == null)
                    {
                        changeables = new DataColumn[] {
                            this.columnChRadius, this.columnChLength,
                            this.columnkth,this.columnkepi,
                            this.columnChCfg,
                            this.columnBellFactor,
                            this.WGtColumn,
                            this.nFactorColumn  };
                          }
                    return changeables;
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
                            this.columnkepi,this.columnkth,
                            this.columnChCfg,
                            this.columnLastCalc,
                           this.columnLastChanged,
                           this.columnWGt,
                           this.nFactorColumn,
                                  this.columnBellFactor,
                              this.columnSSFTable
                        };
                    }
                    return nonNullables;
                }
            }

            /// <summary>
            /// fix this to use windows user
            /// </summary>
            public bool overriders
            {
                //TODO: windows user instead
                get
                {
                    // LINAA set = this.DataSet as LINAA;
                    return (this.DataSet as LINAA).SSFPref.FirstOrDefault().Overrides;
                }
            }

            /// <summary>
            /// I think it is perfect like this, don't mess it up
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e">     </param>
            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                DataColumn c = e.Column;
                if (!NonNullables.Contains(c)) return;

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
                if (!Changeables.Contains(e.Column)) return;

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

                if (nullo)
                {
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
                    else if (this.tableUnit.ChCfgColumn == c)
                    {
                        if (nullo)
                        {
                            ChCfg = "0";
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
                        if (IsSSFTableNull())
                        {
                            ToDo = true;
                            // r.LastChanged = DateTime.Now; //update the time
                        }
                        // else r.ToDo = false;
                    }
                    else if (c == this.tableUnit.ChCfgColumn)
                    {
                        if (nullo) ChCfg = "0";
                        else
                        {
                            // bool nullFluxType = (EC.CheckNull(this.tableChannels.FluxTypeColumn, this));
                            if (EC.CheckNull(this.tableUnit.WGtColumn, this) || this.tableUnit.overriders)
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
                            if (EC.CheckNull(this.tableUnit.BellFactorColumn, this) || this.tableUnit.overriders)
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
                }
            }

            public bool CheckErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableUnit.Changeables).Count();

                return count == 0;
            }

            public LINAA.MatSSFDataTable FillSSFTable(IList<string> fileContent)
            {
                string[] content = null;

                string separator = "------------------------------------------------------------------------";
                int sep = fileContent.IndexOf(separator);
                fileContent = fileContent.Skip(sep + 1).ToList();

                LINAA.MatSSFDataTable table = new LINAA.MatSSFDataTable(false);


                foreach (string item in fileContent)
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;

                    try
                    {
                        content = item.Substring(10).Trim().Split(' ');
                        content = content.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
                        // Z, the element and A data
                        string[] ZEl = item.Substring(0, 10).Split('-');
                        ZEl[2] = ZEl[2].Trim();
                        Int16 A = Convert.ToInt16(ZEl[2]);
                        //interested only in the isotopes
                        if (A > 0)
                        {
                            ZEl[0] = ZEl[0].Trim();// Z
                            ZEl[1] = ZEl[1].Trim(); // Element

                            LINAA.MatSSFRow m = table.NewMatSSFRow();
                            m.UnitID = this.UnitID;
                            table.AddMatSSFRow(m);
                            m.SubSamplesID = this.SampleID;
                            m.SetContent(content, ZEl);
                        }
                    }
                    catch (SystemException ex)
                    {
                        (this.tableUnit.DataSet as LINAA).AddException(ex);
                        EC.SetRowError(this, ex);
                    }
                  
                
                }



                return table;
            }

            /// <summary>
            /// Fills the UnitRow with the given physical parameters
            /// </summary>
            /// <param name="Mdens">  density matrix</param>
            /// <param name="Gtermal">thermal SSF</param>
            /// <param name="EXS">    Escape X section</param>
            /// <param name="MCL">    Mean Chord Lenght</param>
            /// <param name="PXS">    Potential X section</param>
            public void FillWith(string Mdens, string Gtermal, string EXS, string MCL, string PXS)
            {
                try
                {
                    double aux2 = 0;

                    if (!Mdens.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(Mdens, 0);
                        double dens1 = Math.Abs(aux2);
                        if (!this.SubSamplesRow.IsCalcDensityNull())
                        {
                            double dens2 = this.SubSamplesRow.CalcDensity;
                            int factor = 10;
                            if (Math.Abs((dens1 / dens2) - 1) * 100 > factor)
                            {
                                EC.SetRowError(this, new SystemException("Calculated density does not match input density by " + factor.ToString()));
                            }
                        }
                        this.SubSamplesRow.CalcDensity = dens1;
                    }
                    if (!Gtermal.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(Gtermal, 1);

                        this.Gt = Math.Abs(aux2);
                        if (SubSamplesRow != null) SubSamplesRow.Gthermal = this.Gt;
                    }
                    if (!EXS.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(EXS, 0);

                        this.EXS = aux2 / 10;
                    }
                    if (!MCL.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(MCL, 0);

                        this.MCL = aux2 * 10;
                    }
                    if (!PXS.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(MCL, 0);

                        this.PXS = aux2 / 10;
                    }

                    //this goes FIRST!!!

                    this.LastCalc = DateTime.Now;
                    // ValueChanged();
                    this.ToDo = false;
                }
                catch (SystemException ex)
                {
                    // LINAA linaa = this.Table.DataSet as LINAA;
                    (this.Table.DataSet as LINAA).AddException(ex);
                    // this.RowError = ex.Message;
                }
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

            public void ValueChanged()
            {
                this.LastChanged = DateTime.Now;
                this.ToDo = true;
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