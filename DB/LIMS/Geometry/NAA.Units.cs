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
                            this.columnDiameter,
                            this.columnLength,
                            this.columnLastCalc,
                            this.columnLastChanged,
                            this.columnToDo,
                            this.columnContent,
                            this.columnMass };
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
          bool densityNull = nullo;
          bool diamLeng = (c == this.columnDiameter || c == this.columnLength);

          bool Dens = (c == this.columnDensity);

          // diameter or lenght changed
          if (diamLeng)
          {
            if (CalcDensity) r.FindMD(true);
            else if (!densityNull) r.FindMD(false);
          }
          else if (c == this.columnMass) // || densityNull
          {
            r.FindMD(true);
          }
          else if (Dens)
          {
            if (densityNull) r.FindMD(true);
            else if (!CalcDensity)
            {
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

          if (r.IsNameNull() || string.IsNullOrEmpty(r.Name))
          {
            r.Name = "Unit @ ";
            EC.CheckNull(this.columnLastChanged, r);
            r.Name += r.LastChanged;
          }
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
      /// Finds the mass or density
      /// </summary>
      /// <param name="density">forces density calculation</param>
      public void FindMD(bool density)
      {
        if (Vol == 0) return;
        //   {
        int round = 4;

        Decimal current = 0;
        double aux = 0;
        if (density)
        {
          aux = this.Mass / this.Vol;

          current = Convert.ToDecimal(this.Density);
        }
        else
        {
          aux = Vol * Density;

          current = Convert.ToDecimal(this.Mass);
        }

        current = Decimal.Round(current, round);

        Decimal valor = 0;
        valor = Convert.ToDecimal(aux);
        valor = Decimal.Round(valor, round);

        if (valor != current)

        {
          if (!density) Mass = (double)valor;
          else Density = (double)valor;
          LastChanged = DateTime.Now; //update the time
        }

        //  }
      }

      /// <summary>
      /// Sets the channel data
      /// </summary>
      /// <param name="c"></param>
      public void SetChannel()
      {
        LINAA.ChannelsRow c = this.ChannelsRow;
        this.kth = c.kth;
        this.kepi = c.kepi;
        this.ChCfg = c.FluxType;
        this.ChannelID = c.ChannelsID;
      }

      /// <summary>
      /// sets the vial container data
      /// </summary>
      /// <param name="v"></param>
      public void SetVialContainer(ref LINAA.VialTypeRow v)
      {
        // LINAA.VialTypeRow v = null;
        if (!v.IsRabbit) this.VialTypeRow = v;
        else this.ContainerRow = v;

        decimal diameter;
        decimal leng;

        diameter = Convert.ToDecimal((v.InnerRadius * 2.0));
        diameter = Decimal.Round(diameter, 4);
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

        leng = Convert.ToDecimal(v.MaxFillHeight);
        leng = Decimal.Round(leng, 4);

        if (!v.IsRabbit)
        {
          // lenghtbox.Text = leng;
          this.Length = (double)leng;
          // this.VialTypeID = v.VialTypeID;
        }
        else
        {
          this.ChLength = (double)leng;
          //  this.ContainerID = v.VialTypeID;
          //   chlenB.Text = leng.ToString();
        }
      }

      /// <summary>
      /// Sets the matrix data
      /// </summary>
      /// <param name="m"></param>
      public void SetMatrix()
      {
        LINAA.MatrixRow m = this.MatrixRow;

        this.Content = m.MatrixComposition;
        this.MatrixID = m.MatrixID;
        this.Density = m.MatrixDensity;
      }

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