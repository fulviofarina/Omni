using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;

namespace DB.Tools
{
  public partial class WC
  {
    /// <summary>
    /// Finds the efficiency for the given sample according to Plan A or using the Contingency Plan if null
    /// </summary>
    /// <param name="all">true to find efficiency for all peaks</param>
    /// <param name="s">sample</param>
    public static IEnumerable<LINAA.MeasurementsRow> SetEfficiency(bool all, LINAA.SubSamplesRow s, ref LINAA.GeometryRow reference)
    {
      IList<LINAA.MeasurementsRow> ls = null;

      IEnumerable<LINAA.MeasurementsRow> meas = s.GetMeasurementsRows();
      meas = EC.NotDeleted<LINAA.MeasurementsRow>(meas);
      if (meas.Count() == 0) return ls;

      LINAA.PeaksDataTable sol = new LINAA.PeaksDataTable(false);
      LINAATableAdapters.PeaksTableAdapter ta = new LINAATableAdapters.PeaksTableAdapter();
      ta.ClearBeforeFill = true;

      foreach (LINAA.MeasurementsRow m in meas)
      {
        IEnumerable<LINAA.PeaksRow> oldpeaks = LINAA.GetPeaksInNeedOf(all, false, m);
        if (oldpeaks.Count() == 0) continue;

        ta.FillSolidFactors(sol, m.MeasurementID);
        if (sol.Rows.Count == 0)
        {
          //find alternative!! by adding failed-to-load measurements to a separate list
          if (ls == null) ls = new List<LINAA.MeasurementsRow>();
          ls.Add(m);
        }
        else if (sol.Rows.Count != 0)
        {
          oldpeaks = oldpeaks.ToList(); //keep fucking static
          foreach (LINAA.PeaksRow o in oldpeaks)
          {
            LINAA.PeaksRow p = sol.FirstOrDefault(j => (j.PeaksID == o.PeaksID && Math.Abs(j.ID) == Math.Abs(o.ID)));
            if (p != null) o.Efficiency = p.Efficiency;
          }
          oldpeaks = oldpeaks.Where(o => o.RowState == System.Data.DataRowState.Modified || o.RowState == System.Data.DataRowState.Added).ToList();
          if (oldpeaks.Count() == 0) continue;
          //get efficiencies
          reference.Detector = m.Detector;
          reference.Position = 5;
          IEnumerable<LINAA.DetectorsCurvesRow> curves = reference.GetDetectorsCurvesRows();
          SetEfficiency(ref oldpeaks, ref curves);
        }
      }

      if (sol != null)
      {
        sol.Clear();
        sol.Dispose();
        sol = null;
      }
      if (ta != null)
      {
        ta.Dispose();
        ta = null;
      }

      return ls;
    }

    protected static void SetEfficiency(ref IEnumerable<LINAA.PeaksRow> peaks, ref IEnumerable<LINAA.DetectorsCurvesRow> curves)
    {
      double? Log10Effi = null;
      double energy = 0;
      Func<LINAA.DetectorsCurvesRow, bool> efficurveFinder = o =>
      {
        if (o.Curve.CompareTo("EFF") == 0)
        {
          if (o.LowEnergy <= energy && o.HighEnergy > energy) return true;
          else return false;
        }
        else return false;
      };

      foreach (LINAA.PeaksRow p in peaks)
      {
        try
        {
          Log10Effi = null;

          energy = p.Energy;

          if (curves.Count() != 0)
          {
            LINAA.DetectorsCurvesRow effiCurve = curves.FirstOrDefault(efficurveFinder);
            if (!Rsx.EC.IsNuDelDetch(effiCurve))
            {
              Log10Effi = effiCurve.a0 * Math.Pow(Math.Log10(p.Energy), 0);
              Log10Effi += effiCurve.a1 * Math.Pow(Math.Log10(p.Energy), 1);
              Log10Effi += effiCurve.a2 * Math.Pow(Math.Log10(p.Energy), 2);
              Log10Effi += effiCurve.a3 * Math.Pow(Math.Log10(p.Energy), 3);
              Log10Effi += effiCurve.a4 * Math.Pow(Math.Log10(p.Energy), 4);
            }
          }

          /*
        //try trhough DB if not found locally...
        if (Log10Effi == null)
        {
           LINAATableAdapters.QTA qta = (this.tableMeasurements.DataSet as LINAA).QTA;
           Log10Effi = (double?)qta.GetLog10Effi(p.Energy, this.Detector, reference.GeometryName, reference.Position);
        }
           */
          //finally calculate
          if (Log10Effi != null)
          {
            p.Efficiency = Math.Pow(10.0, (double)Log10Effi) * (p.Efficiency);
          }
          else p.RowError = "Reference Efficiency not found";

          LINAA.PeaksRow aux = p;
          CheckEffi(ref aux);
        }
        catch (SystemException ex)
        {
          EC.SetRowError(p, ex);
          p.ID = -1 * Math.Abs(p.ID);
        }
      }
    }

    /// <summary>
    /// Finds COIS factors for the given sample according to Plan A or using the Contingency Plan if null
    /// </summary>
    /// <param name="all">true to find cois for all isotopes</param>
    /// <param name="s">sample</param>
    public static IEnumerable<LINAA.MeasurementsRow> SetCOI(bool all, LINAA.SubSamplesRow s)
    {
      IEnumerable<LINAA.PeaksRow> oldpeaks = LINAA.GetPeaksInNeedOf(all, true, s);  //not deferenced
      List<LINAA.MeasurementsRow> ls = null;

      if (oldpeaks.Count() == 0) return ls;
      //load all geometries in DB   and Reference Geometry

      LINAA.PeaksDataTable coi = new LINAA.PeaksDataTable(false);
      LINAATableAdapters.PeaksTableAdapter ta = new LINAATableAdapters.PeaksTableAdapter();
      ta.ClearBeforeFill = true;
      ta.FillCOINSpecific(coi, s.SubSampleName);   // first try to get cois from specific geometry (Sample +geometry)
      if (coi.Rows.Count == 0) ta.FillCOIN(coi, s.SubSampleName); //if not, try gettting coi for general geometry
      if (coi.Rows.Count == 0)     //finally, if none worked, try populating COIN table instead
      {
        //find alternativ (by population)
        if (ls == null) ls = new List<LINAA.MeasurementsRow>();
        IEnumerable<LINAA.MeasurementsRow> meas = s.GetMeasurementsRows();
        ls.AddRange(meas);
      }
      else
      {
        oldpeaks = oldpeaks.ToList(); //important
        foreach (LINAA.PeaksRow p in oldpeaks)
        {
          LINAA.PeaksRow o = coi.FirstOrDefault(j => (p.MeasurementID == j.MeasurementID && j.PeaksID == p.PeaksID && Math.Abs(j.ID) == Math.Abs(p.ID)));
          if (o != null) p.COI = o.COI;
        }
      }

      if (coi != null)
      {
        coi.Clear();
        coi.Dispose();
        coi = null;
      }
      if (ta != null)
      {
        ta.Dispose();
        ta = null;
      }

      return ls;
    }

    /// <summary>
    /// Finds COIS factors or efficiency for the given sample according to Plan A or using the Contingency Plan if null
    /// </summary>
    /// <param name="samples">samples to find stuff for</param>
    /// <param name="coin">true for cois only</param>
    /// <param name="all">true to find stuff for all peaks</param>
    public static IEnumerable<LINAA.MeasurementsRow> SetCOINSolid(ref IEnumerable<LINAA.SubSamplesRow> samples, bool coin, bool all, ref LINAA.GeometryRow reference)
    {
      List<LINAA.MeasurementsRow> measForContingengy = null;
      foreach (LINAA.SubSamplesRow s in samples)
      {
        IEnumerable<LINAA.MeasurementsRow> aux = SetCOINSolid(s, coin, all, ref reference);
        if (aux != null && aux.Count() != 0)
        {
          if (measForContingengy == null) measForContingengy = new List<LINAA.MeasurementsRow>();
          measForContingengy.AddRange(aux);
        }
      }
      return measForContingengy;
    }

    /// <summary>
    /// Calculates the efficiency or COIS for the given sample according to Plan A or a Contingency Plan if null
    /// </summary>
    /// <param name="s">sample</param>
    /// <param name="coin">calculate cois</param>
    /// <param name="all">true to find stuff for all peaks</param>
    public static IEnumerable<LINAA.MeasurementsRow> SetCOINSolid(LINAA.SubSamplesRow s, bool coin, bool all, ref LINAA.GeometryRow reference)
    {
      List<LINAA.MeasurementsRow> measForContingency = null;

      try
      {
        IEnumerable<LINAA.MeasurementsRow> aux = null;
        if (coin)
        {
          aux = SetCOI(all, s);
        }
        else
        {
          aux = SetEfficiency(all, s, ref reference);
        }

        if (aux != null && aux.Count() != 0)
        {
          if (measForContingency == null) measForContingency = new List<LINAA.MeasurementsRow>();
          measForContingency.AddRange(aux);
        }
      }
      catch (SystemException ex)
      {
        s.RowError += "SetCOINSolid Module Error: " + ex.Message;
      }
      return measForContingency;
    }

    /// <summary>
    /// Calculates the efficiency or COIS for the given measurements according to a Contingency Plan
    /// </summary>
    /// <param name="meas"></param>
    /// <param name="coin"></param>
    public static void ContingencySetCOINSolid(ref IEnumerable<LINAA.MeasurementsRow> meas, bool coin, ref LINAA.GeometryRow reference)
    {
      meas = EC.NotDeleted<LINAA.MeasurementsRow>(meas);
      if (meas.Count() == 0) return;

      foreach (LINAA.MeasurementsRow m in meas)
      {
        //get COIS or Solangs
        if (coin) ContingencySetCOI(m);
        else ContingencySetEfficiency(m, ref reference, 5);
      }
    }

    /// <summary>
    /// Uses data relations and COIN and SOLANG tables
    /// </summary>
    /// <param name="meas"></param>
    /// <param name="reference"></param>
    /// <param name="refPosition"></param>
    public static void ContingencySetEfficiency(LINAA.MeasurementsRow meas, ref LINAA.GeometryRow reference, Int16 refPosition)
    {
      IEnumerable<LINAA.PeaksRow> peaks = meas.GetPeaksRows();
      if (peaks == null || peaks.Count() == 0)
      {
        meas.RowError += "Cannot set peak efficiency when no peaks are found";
        return;
      }
      if (Rsx.EC.IsNuDelDetch(reference))
      {
        meas.RowError += "Cannot set peak efficiency when a null reference geometry is linked to this measurement";
        return;
      }
      //get efficiencies
      reference.Detector = meas.Detector;
      reference.Position = refPosition;

      IEnumerable<LINAA.DetectorsCurvesRow> curves = reference.GetDetectorsCurvesRows();
      if (curves == null || curves.Count() == 0)
      {
        meas.RowError += "Cannot set peak efficiency when no efficiencies curves are linked to the reference geometry";
        return;
      }
      IEnumerable<LINAA.SolangRow> solangsRefs = reference.GetSolangRows();
      if (solangsRefs == null || solangsRefs.Count() == 0)
      {
        meas.RowError += "Cannot set peak efficiency when no reference solid angles are linked to the reference geometry";
        return;
      }
      IEnumerable<LINAA.SolangRow> solangs = meas.GetSolangRows();
      if (solangs == null || solangs.Count() == 0)
      {
        meas.RowError += "Cannot set peak efficiency when no solid angles are linked to this measurement";
        return;
      }

      double? SolidAngleRef = null;
      double? SolidAngle = null;
      double SolidFactor = 1;
      double? Log10Effi = null;

      foreach (LINAA.PeaksRow p in peaks)
      {
        SolidAngleRef = null;
        SolidAngle = null;
        SolidFactor = 1;
        Log10Effi = null;

        p.Efficiency = 1.0;

        //try locally

        Func<LINAA.SolangRow, bool> solpeakFinder = o =>
        {
          if (o.Energy == p.Energy) return true;
          else return false;
        };

        Func<LINAA.DetectorsCurvesRow, bool> efficurveFinder = o =>
        {
          if (o.Curve.CompareTo("EFF") == 0)
          {
            if (o.LowEnergy <= p.Energy && o.HighEnergy > p.Energy) return true;
            else return false;
          }
          else return false;
        };

        LINAA.SolangRow auxSol = solangsRefs.FirstOrDefault<LINAA.SolangRow>(solpeakFinder);//filtered by peak
        if (!Rsx.EC.IsNuDelDetch(auxSol)) SolidAngleRef = (double?)auxSol.Solang;

        //take geometry at peak
        auxSol = solangs.FirstOrDefault<LINAA.SolangRow>(solpeakFinder);
        if (!Rsx.EC.IsNuDelDetch(auxSol)) SolidAngle = (double?)auxSol.Solang;

        if (curves.Count() != 0)
        {
          LINAA.DetectorsCurvesRow effiCurve = curves.FirstOrDefault(efficurveFinder);
          if (!Rsx.EC.IsNuDelDetch(effiCurve))
          {
            Log10Effi = effiCurve.a0 * Math.Pow(Math.Log10(p.Energy), 0);
            Log10Effi += effiCurve.a1 * Math.Pow(Math.Log10(p.Energy), 1);
            Log10Effi += effiCurve.a2 * Math.Pow(Math.Log10(p.Energy), 2);
            Log10Effi += effiCurve.a3 * Math.Pow(Math.Log10(p.Energy), 3);
            Log10Effi += effiCurve.a4 * Math.Pow(Math.Log10(p.Energy), 4);
          }
        }

        //try trhough DB if not found locally...
        if (SolidAngle == null || SolidAngleRef == null || Log10Effi == null)
        {
          LINAATableAdapters.QTA qta = new LINAATableAdapters.QTA();
          //LINAATableAdapters.QTA qta = (this.tableMeasurements.DataSet as LINAA).QTA;
          if (SolidAngle == null)
          {
            SolidAngle = (double?)qta.GetSolidAngle(meas.SubSamplesRow.GeometryName + meas.SubSamplesRow.SubSampleName, meas.Position, meas.Detector, p.Energy);
          }
          if (SolidAngleRef == null)
          {
            SolidAngleRef = (double?)qta.GetSolidAngle(reference.GeometryName, reference.Position, meas.Detector, p.Energy);
          }
          if (Log10Effi == null)
          {
            Log10Effi = (double?)qta.GetLog10Effi(p.Energy, meas.Detector, reference.GeometryName, reference.Position);
          }
          qta.Dispose();
          qta = null;
        }

        //finally calculate
        if (SolidAngle != null && SolidAngleRef != null && Log10Effi != null)
        {
          SolidFactor = (double)(SolidAngle / SolidAngleRef);
          p.Efficiency = Math.Pow(10.0, (double)Log10Effi) * (SolidFactor);
        }
        LINAA.PeaksRow aux = p;
        CheckEffi(ref aux);
      }
    }

    public static void ContingencySetCOI(LINAA.MeasurementsRow meas)
    {
      LINAA.GeometryRow geo = meas.SubSamplesRow.GeometryRow;
      if (Rsx.EC.IsNuDelDetch(geo))
      {
        meas.RowError += "Cannot set peak COIS when a null geometry is linked to this measurement";
        return;
      }
      geo.Detector = meas.Detector;
      geo.Position = meas.Position;
      IEnumerable<LINAA.COINRow> coins = geo.GetCOINRows();

      if (coins == null || coins.Count() == 0)
      {
        meas.RowError += "Cannot set peak COIS when no source COIS are linked to this measurement";
        return;
      }
      IEnumerable<LINAA.PeaksRow> peaks = meas.GetPeaksRows();
      if (peaks == null || peaks.Count() == 0)
      {
        meas.RowError += "Cannot set peak COIS when no peaks are found";
        return;
      }

      double? coi = null;
      double energy = 0;

      LINAATableAdapters.QTA qta = new LINAATableAdapters.QTA();

      Func<LINAA.COINRow, bool> coinpeakFinder = o =>
      {
        if (o.Energy == energy) return true;
        else return false;
      };

      foreach (LINAA.PeaksRow p in peaks)
      {
        p.COI = 1;
        coi = null;
        energy = p.Energy;

        string coinIso = p.Iso.Replace("-", null).ToUpper().Trim();
        IEnumerable<LINAA.COINRow> coins2 = coins.Where(o => coinIso.CompareTo(o.Isotope.Trim()) == 0);

        if (coins2.Count() != 0)
        {
          LINAA.COINRow c = coins2.FirstOrDefault(coinpeakFinder);
          if (c != null) coi = (double?)c.COI;
        }

        if (coi == null) coi = (double?)qta.GetCOI(meas.Detector, meas.SubSamplesRow.GeometryName, coinIso, meas.Position, p.Energy);

        if (coi != null) p.COI = (double)coi;
        else p.ID = -1 * Math.Abs(p.ID);
      }

      qta.Dispose();
      qta = null;
    }

    /// <summary>
    /// Populates the SolidAngles table with solid angles for the given measurements. A HashSet of geometries loaded is needed to avoid unnecessary re-loading
    /// </summary>
    /// <param name="hsgeos">Geometries already loaded for the given measurement</param>
    /// <param name="measurements">list of measurements to load</param>
    public static LINAA.COINDataTable PopulateCOIN(ref System.Collections.Generic.HashSet<string> hsgeos, ref System.Collections.Generic.IEnumerable<LINAA.MeasurementsRow> measurements)
    {
      DB.LINAATableAdapters.COINTableAdapter cota = new DB.LINAATableAdapters.COINTableAdapter();

      LINAA.COINDataTable coin = new LINAA.COINDataTable();
      coin.BeginLoadData();
      foreach (LINAA.MeasurementsRow m in measurements)
      {
        //find solangs for this meas --> geometry,det,position
        //find coins for this geometry --> detector, position

        LINAA.SubSamplesRow s = m.SubSamplesRow;

        if (Rsx.EC.IsNuDelDetch(s))
        {
          m.RowError += "No Sample is linked to this measurement. Cannot determine COIs";
          continue;
        }

        LINAA.GeometryRow geo = m.SubSamplesRow.GeometryRow;
        if (Rsx.EC.IsNuDelDetch(geo))
        {
          m.RowError += "No Geometry is linked to this measurement. Cannot determine COIs";
          continue;
        }

        geo.Detector = m.Detector;
        geo.Position = m.Position;

        IEnumerable<LINAA.COINRow> coins = geo.GetCOINRows();
        if (coins.Count() != 0) continue;

        //not found already, so load them
        if (!hsgeos.Add(s.GeometryName + m.Detector)) continue;

        LINAA.COINDataTable table = null;
        table = cota.GetDataByDetectorGeometryPosition(m.Detector, s.GeometryName + s.SubSampleName, m.Position);
        if (table == null || table.Rows.Count == 0)
        {
          m.RowError = "Could not find COI factors for this measurement";
          continue;
        }
        coin.Merge(table, false, System.Data.MissingSchemaAction.AddWithKey);
        table.Clear();
        table.Dispose();
        table = null;
      }
      coin.EndLoadData();
      cota.Dispose();
      cota = null;
      return coin;
    }

    /// <summary>
    /// Populates the SolidAngles table with solid angles for the given measurements. A HashSet of geometries loaded is needed to avoid unnecessary re-loading
    /// </summary>
    /// <param name="hsgeos">Geometries already loaded for the given measurement</param>
    /// <param name="measurements">list of measurements to load</param>
    public static LINAA.SolangDataTable PopulateSolang(ref System.Collections.Generic.HashSet<string> hsgeos, ref System.Collections.Generic.IEnumerable<LINAA.MeasurementsRow> measurements)
    {
      DB.LINAATableAdapters.SolangTableAdapter solta = new DB.LINAATableAdapters.SolangTableAdapter();

      LINAA.SolangDataTable dt = new LINAA.SolangDataTable();
      dt.BeginLoadData();

      foreach (LINAA.MeasurementsRow m in measurements)
      {
        //find solangs for this meas --> geometry,det,position

        if (m.IsGeometryNull())
        {
          m.RowError = "Geometry cannot be null";
          continue;
        }

        if (!hsgeos.Add(m.Detector + m.Geometry + m.Position)) continue;

        System.Collections.Generic.IEnumerable<LINAA.SolangRow> solangs = m.GetSolangRows();
        if (solangs.Count() != 0) continue;

        //force solcoi if solangs not found

        LINAA.SolangDataTable soldt = null;
        soldt = solta.GetDataByGeoDetPos(m.Geometry, m.Detector, m.Position);
        if (soldt.Rows.Count == 0)
        {
          soldt.Dispose();
          soldt = null;
        }
        if (soldt == null)
        {
          m.RowError = "Could not find solid angles for this measurement";
          continue;
        }
        dt.Merge(soldt, false, System.Data.MissingSchemaAction.AddWithKey);
        soldt = solta.GetDataByGeoDetPos("REF", m.Detector, 5);
        if (soldt.Rows.Count == 0)
        {
          soldt.Dispose();
          soldt = null;
        }
        if (soldt == null)
        {
          m.RowError = "Could not find Reference solid angles for this measurement";
          continue;
        }

        dt.Merge(soldt, false, System.Data.MissingSchemaAction.AddWithKey);

        soldt.Dispose();
        soldt = null;
      }
      dt.EndLoadData();
      solta.Dispose();
      solta = null;
      return dt;
    }

    public static void PopulateCOINSolang(bool coin, ref IEnumerable<LINAA.MeasurementsRow> measforContingency, ref object To)
    {
      HashSet<string> geos = new HashSet<string>();

      if (coin)
      {
        LINAA.COINDataTable to2 = (LINAA.COINDataTable)To;
        LINAA.COINDataTable dt = PopulateCOIN(ref geos, ref measforContingency);
        to2.BeginLoadData();
        to2.Merge(dt, false, System.Data.MissingSchemaAction.AddWithKey);
        to2.EndLoadData();
      }
      else
      {
        LINAA.SolangDataTable to = (LINAA.SolangDataTable)To;
        LINAA.SolangDataTable dt2 = PopulateSolang(ref geos, ref measforContingency);
        to.BeginLoadData();
        to.Merge(dt2, false, System.Data.MissingSchemaAction.AddWithKey);
        to.EndLoadData();
      }
      geos.Clear();
      geos = null;
    }
  }
}