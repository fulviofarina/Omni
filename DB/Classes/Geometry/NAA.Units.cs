using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Math;

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
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnChDiameter, this.columnChLenght,
                            this.columnDensity,
                            this.columnDiameter,
                            this.columnLenght,
                            this.columnMass };
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    UnitRow r = e.Row as UnitRow;
                    DataColumn c = e.Column;
                    if (NonNullables.Contains(e.Column))
                    {
                        bool nu = Dumb.CheckNull(c, e.Row);
                        bool densityNull = nu && c == this.columnDensity;

                        if (c == this.columnDiameter || c == this.columnLenght || c == this.columnDensity)
                        {
                            if (!densityNull)
                            {
                                double Vol = MyMath.GetVolCylinder(r.Diameter, r.Lenght);

                                if (Vol != 0)
                                {
                                    double mass = Vol * r.Density;
                                    if (mass != r.Mass) r.Mass = mass;
                                }
                                return;
                                //    massB.Text = Decimal.Round(Convert.ToDecimal(mass), 5).ToString();
                            }
                            else densityNull = true;
                        }
                        if (e.Column == this.columnMass || densityNull)
                        {
                            double Vol = MyMath.GetVolCylinder(r.Diameter, r.Lenght);

                            double density = MyMath.GetDensity(r.Mass, Vol);
                            if (density != r.Density) r.Density = density;

                            //      densityB.Text = Decimal.Round(Convert.ToDecimal(density), 2).ToString();
                        }
                        //
                    }
                }
                catch (SystemException ex)
                {
                    e.Row.SetColumnError(e.Column, ex.Message);
                }
            }
        }

        partial class UnitRow
        {
            public void SetChannel(ref LINAA.ChannelsRow c)
            {


                this.kth = c.kth;
                this.kepi = c.kepi;
                this.ChCfg = c.FluxType;
                this.ChannelID = c.ChannelsID;

            }
            public void SetVialContainer(ref LINAA.VialTypeRow v)
            {



                string rad = "Assign a ";
                if (v.IsRabbit) rad += "source ";
                string len = rad;
                rad += "radius ";

                rad += "lenght ";
                rad += "in mm.";
                len += "in mm.";

                decimal diameter;
                decimal leng;
                v.RowError = string.Empty;
                if (v.IsInnerRadiusNull() || v.InnerRadius == 0) v.RowError = rad;
                else
                {
                    diameter = Decimal.Round(Convert.ToDecimal((v.InnerRadius * 2.0)), 4);
                    if (!v.IsRabbit)
                    {
                        this.Diameter = Convert.ToDouble(diameter);
                        //   diameterbox.Text = diameter.ToString();
                    }
                    else
                    {
                        this.ChDiameter = Convert.ToDouble(diameter);

                        //   chdiamB.Text = diameter.ToString();
                    }
                }
                if (v.IsMaxFillHeightNull() || v.MaxFillHeight == 0) v.RowError += len;
                else
                {
                    leng = Decimal.Round(Convert.ToDecimal(v.MaxFillHeight), 4);

                    if (!v.IsRabbit)
                    {
                        // lenghtbox.Text = leng;
                        this.Lenght = Convert.ToDouble(leng);
                        this.VialTypeID = v.VialTypeID;
                    }
                    else
                    {
                        this.ChLenght = Convert.ToDouble(leng);
                        this.ContainerID = v.VialTypeID;
                        //   chlenB.Text = leng.ToString();
                    }
                }


            }

            public void SetMatrix(ref LINAA.MatrixRow m)
            {

                m.RowError = string.Empty;
                if (m.IsMatrixCompositionNull())
                {
                    m.RowError = "Assign a matrix composition (compulsory).";
                }
                else
                {
                    this.Content = m.MatrixComposition;
                    this.MatrixID = m.MatrixID;
                    this.Density = m.MatrixDensity;
                    //  double density = m.MatrixDensity;
                    if (this.Density == 0)
                    {
                        m.RowError += "Optionally, assign a density to this matrix.";
                    }

                    //this.currentUnit.Density = density;
                }
            }


            public void FillWith(string Mdens, string Gt, string EXS, string MCL, string PXS)
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
                        Dumb.SetRowError(this, new SystemException("Calculated density does not match input density by " + factor.ToString()));
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


        }


    }

}