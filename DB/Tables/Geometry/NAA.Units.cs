using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class MatSSFDataTable
        {
            private String[] gtdensity = new String[2] { String.Empty, String.Empty };

            public String[] GtDensity
            {
                get { return gtdensity; }
                set { gtdensity = value; }
            }
        }

        partial class UnitDataTable
        {
            public LINAA.UnitRow NewUnitRow(double kepi, double kth, string chfg)
            {
                LINAA.UnitRow u = this.NewUnitRow();

                u.kepi = kepi;
                u.kth = kth;
                //  u.RowError = string.Empty;
                u.ChCfg = chfg;
                this.AddUnitRow(u);

                return u;
            }

            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnChDiameter, this.columnChLength,
                            this.columnDensity,
                         //   this.columnDiameter,
                         //   this.columnLength,
                            this.columnLastCalc,
                            this.columnLastChanged,
                            this.columnToDo,
                            this.columnContent };
                    }
                    return nonNullables;
                }
            }

            /// <summary>
            /// NO ME GUSTA PERO QUE CONO
            /// </summary>
            public bool CalcDensity
            {
                get
                {
                    return (this.DataSet as LINAA).CurrentSSFPref.CalcDensity;
                }
            }

            /// <summary>
            /// I think it is perfect like this, don't mess it up
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                DataColumn c = e.Column;

                if (!NonNullables.Contains(c)) return;

                DataRow row = e.Row;

                // if (e.ProposedValue == current) return;

                UnitRow r = row as UnitRow;

                try
                {
                    bool nullo = EC.CheckNull(c, row);
                    bool Dens = (c == this.columnDensity);
                    if (EC.IsNuDelDetch(r.SubSamplesRow)) return;
                    //the density column called
                    //but it was not the SubSampleRow Parent who called
                    //then it was the user updating the value with a combobox
                    //otherwise FindMD is goind to try to call back the parent
                    if (Dens)
                    {
                        if (nullo) r.FindMD(true);
                        double mass = r.SubSamplesRow.Net;
                        if (!CalcDensity || mass == 0) // || r.SubSamplesRow.Net == 0)
                        {
                            // if (!r.SubSamplesRow.IsSender)
                            r.FindMD(false);
                        }
                    }
                    else if (c == this.columnLastCalc || c == this.columnLastChanged)
                    {
                        //negative if calculated after it has changed (which is good)
                        double tot = r.LastChanged.Subtract(r.LastCalc).TotalSeconds;
                        //positive means needs to be calculated
                        if (tot > 1)
                        {
                            r.ToDo = true;
                        }
                        else if (tot != 0) //negative has been calculated
                        {
                            r.ToDo = false;
                        }
                    }
                    else if (c == this.ToDoColumn)
                    {
                        if (!r.ToDo) r.LastChanged = r.LastCalc;
                    }
                    else if (c == this.columnContent)
                    {
                        if (r.IsToDoNull() || !r.ToDo)
                        {
                            r.LastChanged = DateTime.Now; //update the time
                        }
                    }

                    /*
                    if (r.IsNameNull() || string.IsNullOrEmpty(r.Name))
                    {
                        r.Name = "Unit @ ";
                        EC.CheckNull(this.columnLastChanged, r);
                        r.Name += r.LastChanged;
                    }
                    */
                }
                catch (SystemException ex)
                {
                    LINAA linaa = this.DataSet as LINAA;
                    e.Row.SetColumnError(c, ex.Message);
                    linaa.AddException(ex);
                }
            }
        }

        partial class UnitRow
        {
            /// <summary>
            /// Finds the mass or density 03-2017
            /// </summary>
            /// <param name="density">forces density or mass calculation</param>
            public void FindMD(bool density)
            {
                if (EC.IsNuDelDetch(this.SubSamplesRow)) return;

                //no volume? exit
                if (Vol == 0)
                {
                    //  this.Density = this.SubSamplesRow.MatrixDensity;
                    return;
                }
                //chek if null

                double auxMass = this.SubSamplesRow.Net; //take mass
                                                         //   double Vol = SubSamplesRow.FindVolumen();
                double aux = 0;
                if (density)
                {
                    aux = auxMass * 0.001 / Vol; //in g/cm3 as Vol is in cm3
                    auxMass = this.Density;
                }
                else
                {
                    aux = Vol * Density * 1000; //= cm3 * g/ cm3 * 1000  = in mg
                }

                if (aux != auxMass)
                {
                    if (!density)
                    {
                        this.SubSamplesRow.IsSender = true;
                        this.SubSamplesRow.Gross1 = aux;
                        this.SubSamplesRow.Gross2 = aux;
                        this.SubSamplesRow.IsSender = false;
                    }
                    else Density = aux;
                    LastChanged = DateTime.Now; //update the time
                }
            }

            /// <summary>
            /// Sets the channel data
            /// </summary>
            /// <param name="c"></param>
            public void SetChannel(ref LINAA.ChannelsRow c)
            {
                //       this.ChannelsRow = c;
                this.kth = c.kth;
                this.kepi = c.kepi;
                this.ChCfg = c.FluxType;

                //  this.ChannelID = c.ChannelsID;
            }

            /// <summary>
            /// sets the vial container data
            /// </summary>
            /// <param name="v"></param>
            public void SetVialContainer(ref LINAA.VialTypeRow v)
            {
                // LINAA.VialTypeRow v = null;

                decimal diameter;
                decimal leng;

                diameter = Convert.ToDecimal((v.InnerRadius * 2.0));
                diameter = Decimal.Round(diameter, 4);

                this.ChDiameter = Convert.ToDouble(diameter);

                leng = Convert.ToDecimal(v.MaxFillHeight);
                leng = Decimal.Round(leng, 4);

                this.ChLength = (double)leng;
            }

            /*
            /// <summary>
            /// Sets the matrix data
            /// </summary>
            /// <param name="m"></param>
            public void SetMatrix(ref LINAA.MatrixRow m)
            {
                //   LINAA.MatrixRow m = this.MatrixRow;

                this.SubSamplesRow.MatrixID = m.MatrixID;
                this.Density = m.MatrixDensity;
                this.Content = m.MatrixComposition;
            }
            */

            /// <summary>
            /// Fills the UnitRow with data from an array extracted from the OUTPUT MatSSF File
            /// </summary>
            /// <param name="array">Output file extracted array</param>
            public void FillWith(ref IEnumerable<string> array)
            {
                string aux = string.Empty;
                string Gt = string.Empty;
                string Mdens = string.Empty;
                string MCL = string.Empty;
                string EXS = string.Empty;
                string PXS = string.Empty;

                try
                {
                    string densityUnit = "[g/cm3]";
                    string cmUnit = "[cm]";
                    string invcmUnit = "[1/cm]";

                    aux = "Material density";
                    Dumb.SetField(ref Mdens, ref array, aux, densityUnit);
                    aux = "G-thermal";
                    Dumb.SetField(ref Gt, ref array, aux, string.Empty);
                    aux = "Mean chord length";
                    Dumb.SetField(ref MCL, ref array, aux, cmUnit);
                    aux = "Escape x.sect.";
                    Dumb.SetField(ref EXS, ref array, aux, invcmUnit);
                    aux = "Potential x.sect.";
                    Dumb.SetField(ref PXS, ref array, aux, invcmUnit);
                }
                catch (SystemException ex)
                {
                    LINAA linaa = this.Table.DataSet as LINAA;
                    linaa.AddException(ex);
                    this.RowError = ex.Message;
                }

                this.FillWith(Mdens, Gt, EXS, MCL, PXS);
            }

            /// <summary>
            /// Fills the UnitRow with the given physical parameters
            /// </summary>
            /// <param name="Mdens">density matrix</param>
            /// <param name="Gt">thermal SSF</param>
            /// <param name="EXS">Escape X section</param>
            /// <param name="MCL">Mean Chord Lenght</param>
            /// <param name="PXS">Potential X section</param>
            public void FillWith(string Mdens, string Gt, string EXS, string MCL, string PXS)
            {
                try
                {
                    double aux2 = 0;

                    if (!Mdens.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(Mdens, 0);

                        double dens2 = this.Density;
                        double dens1 = Math.Abs(aux2);
                        int factor = 10;
                        if (Math.Abs((dens1 / dens2) - 1) * 100 > factor)
                        {
                            EC.SetRowError(this, new SystemException("Calculated density does not match input density by " + factor.ToString()));
                        }

                        this.Density = dens1;
                    }
                    if (!Gt.Equals(string.Empty))
                    {
                        aux2 = Dumb.Parse(Gt, 1);

                        this.Gt = Math.Abs(aux2);
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

                    this.LastCalc = DateTime.Now;
                }
                catch (SystemException ex)
                {
                    LINAA linaa = this.Table.DataSet as LINAA;
                    linaa.AddException(ex);
                    this.RowError = ex.Message;
                }
            }
        }
    }
}