using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class UnitDataTable
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
                            this.columnChDiameter, this.columnChLength,
                            this.columnkth,this.columnkepi,
                            this.columnChCfg,
                            this.columnBellFactor//,
                         // this.columnLastCalc, this.columnLastChanged, this.columnToDo,
                         //   this.columnContent
                        };
                        // this.columnSSFTable};
                    }
                    return changeables;
                }
            }

            public DataColumn[] NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnChDiameter, this.columnChLength,
                            this.columnkepi,this.columnkth,
                            this.columnChCfg,
                            this.columnLastCalc,
                           this.columnLastChanged,
                      // this.columnToDo,
                          //  this.columnContent,
                                  this.columnBellFactor,
                              this.columnSSFTable
                        };
                    }
                    return nonNullables;
                }
            }


            /// <summary>
            /// I think it is perfect like this, don't mess it up
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e">     </param>
            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                //this.ColumnChanging += UnitDataTable_ColumnChanging;
                DataColumn c = e.Column;
                if (!NonNullables.Contains(c)) return;

                DataRow row = e.Row;
                UnitRow r = row as UnitRow;

                try
                {
                    // bool isZero = false;

                    // bool nullo = EC.CheckNull(c, row);

                    bool nullo = EC.CheckNull(e.Column, e.Row);// string.IsNullOrEmpty(e.Row.GetColumnError(e.Column));

                    // if (NonNullables.Contains(e.Column)) EC.CheckNull(e.Column, e.Row);
                    /*      if (c == this.columnLastChanged)
                          {
                                  if (r.IsLastCalcNull()) return;
                                  if (r.IsLastChangedNull()) return;
                                  double tot = r.LastChanged.Subtract(r.LastCalc).TotalSeconds;
                                  //positive means needs to be calculated
                                  if (tot > 2)
                                  {
                                      r.ToDo = true;
                                  }
                                  else if (tot != 0) //negative has been calculated
                                  {
                                      r.ToDo = false;
                                  }

                          // }

                              //negative if calculated after it has changed (which is good)
                          }
                          else
                          {
                              */
                    if (nullo)
                    {
                        if (this.BellFactorColumn == c)
                        {
                            if (nullo)
                            {
                                r.BellFactor = 1;
                            }
                        }
                        else if (this.kepiColumn == c)
                        {
                            if (nullo)
                            {
                                r.kepi = 1;
                            }
                        }
                        else if (this.kthColumn == c)
                        {
                            if (nullo)
                            {
                                r.kth = 0.6;
                            }
                        }
                        else if (this.ChCfgColumn == c)
                        {
                            if (nullo)
                            {
                                r.ChCfg = "0";
                            }
                        }
                        else if (this.ChDiameterColumn == c)
                        {
                            // if (Convert.ToDouble(e.ProposedValue) == 0) e.ProposedValue = 1 ;
                            if (nullo)
                            {
                                r.ChDiameter = 100;
                            }
                        }
                        else if (this.ChLengthColumn == c)
                        {
                            if (nullo)
                            {
                                r[c] = 100;
                            }
                        }
                        else if (c == this.SSFTableColumn)
                        {
                            if (r.IsSSFTableNull())
                            {
                                r.ToDo = true;
                                // r.LastChanged = DateTime.Now; //update the time
                            }
                            // else r.ToDo = false;
                        }
                        /*
                        else if (c == this.ContentColumn)
                        {
                            if (nullo)
                            {
                                r.Content = "#Al (50%), #Gd (50%) "; // "Please assign a Matrix/Content to this Unit";
                                                                     // r.LastChanged = DateTime.Now;
                                                                     // //update the time
                            }
                            // else r.ToDo = false;
                        }
                        */
                    }
                    //}
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

        partial class UnitRow
        {
            public bool Check()
            {
                //      bool ok = true;
                //columns in error

                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableUnit.Changeables).Count();

                // List<DataColumn> cols = colsInE.ToList(); string co = count.ToString();

                return count == 0;
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

            /// <summary>
            /// Sets the channel data
            /// </summary>
            /// <param name="c"></param>
            public void SetChannel(ref LINAA.ChannelsRow c)
            {
                this.kth = c.kth;
                this.kepi = c.kepi;
                this.ChCfg = c.FluxType;
                this.BellFactor = c.BellFactor;
                this.SubSamplesRow.FC =c.FC;
            }

            /// <summary>
            /// sets the vial container data
            /// </summary>
            /// <param name="v"></param>
            public void SetRabbitContainer(ref LINAA.VialTypeRow v)
            {
                this.ChDiameter = v.InnerRadius * 2.0;
                this.ChLength = v.MaxFillHeight;
            }

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

            public void ValueChanged()
            {
                this.LastChanged = DateTime.Now;
                this.ToDo = true;
            }
        }
    }
}