using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Rsx;

namespace k0X
{
   using DB;
   public partial class ucSamples
   {

	  internal void Weffiloader_DoWorkOld(object sender, System.ComponentModel.DoWorkEventArgs e)
	  {

		 DateTime start = DateTime.Now;

		 bool coin = (bool)(e.Argument as object[])[1];
		 IEnumerable<LINAA.SubSamplesRow> samples = (IEnumerable<LINAA.SubSamplesRow>)(e.Argument as object[])[0];

		 // if (coin) this.Linaa.PopulateCOIN(ref samples);
		 //  else this.Linaa.PopulateSolang(ref samples);

		 DateTime end = DateTime.Now;
		 double secs = (end - start).TotalSeconds;

		 string text = string.Empty;
		 if (coin) text = "Coi Factors";
		 else text = "Solid Angles";

		 e.Result = new object[] { Decimal.Round(Convert.ToDecimal(secs), 1).ToString(), text, coin, samples };


	  }
	  internal void Weffiloader_RunWorkerCompletedOld(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
	  {

		 object[] results = e.Result as object[];

		 IEnumerable<LINAA.SubSamplesRow> samples = results[3] as IEnumerable<LINAA.SubSamplesRow>;

		 this.progress.Value += samples.Count();
		 tip.ToolTipIcon = ToolTipIcon.Info;
		 tip.ToolTipTitle = results[1] + " loaded for " + this.Name;
		 tip.Show(results[1] + " loaded in " + results[0] + " seconds", this.Status, 5000);
		 Application.DoEvents();

		 if (!(bool)results[2])
		 {
			this.Linaa.CheckDirectSolcoi(ref samples);
			int needSolang = samples.Count(s => s.DirectSolcoi);
			tip.ToolTipIcon = ToolTipIcon.Info;
			tip.ToolTipTitle = needSolang + " samples need Efficiency calculations";
			string text = string.Empty;
			if (needSolang != 0)
			{
			   text = "Background calculations will start on due time";
			}
			else text = "Nothing scheduled";
			tip.Show(text, this.Status, 2000);
			Application.DoEvents();
		 }


	  }
	  internal void EffiLoadOld(bool coin, ref IEnumerable<LINAA.SubSamplesRow> samples)
	  {

		 int count = samples.Count();
		 string text = string.Empty;
		 tip.ToolTipIcon = ToolTipIcon.Info;
		 if (coin) text = "Loading Coi factors for ";
		 else text = "Loading Solid angles for ";
		 tip.ToolTipTitle = text + this.Name;
		 tip.Show(text + count + " samples", this.Status, 3000);
		 Application.DoEvents();

		 System.ComponentModel.BackgroundWorker effiloader = new System.ComponentModel.BackgroundWorker();
		 effiloader.DoWork += new System.ComponentModel.DoWorkEventHandler(Weffiloader_DoWorkOld);
		 effiloader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(Weffiloader_RunWorkerCompletedOld);

		 this.progress.Maximum += count;
		 effiloader.RunWorkerAsync(new object[] { samples, coin });

	  }
	  public void CheckDirectSolcoiOld()
	  {
		 try
		 {

			if (this.GeometryRow == null) return;
			if (this.IsGeometryNameNull()) return;

			this.SetColumnError(this.tableSubSamples.DirectSolcoiColumn, null);

			System.Collections.Generic.IEnumerable<LINAA.MeasurementsRow> measurements = this.GetMeasurementsRows().Where(o => o.GetPeaksRows().Count() != 0 && !o.HasErrors);
			foreach (LINAA.MeasurementsRow m in measurements)
			{

			   //find solangs for this meas --> geometry,det,position
			   System.Collections.Generic.IEnumerable<LINAA.SolangRow> solangs = m.GetSolangRows();
			   //force solcoi if solangs not found
			   if (solangs.Count() == 0 && !this.DirectSolcoi)
			   {
				  this.DirectSolcoi = true;	   //enable directsolcoi when solangs are not found and directsolcoi was disabled
				  break;
			   }
			   else if (solangs.Count() != 0) this.DirectSolcoi = false;  //perhaps solangs for this meas are found but the overall sample still needs solangs, this line fixes it.
			}
		 }
		 catch (SystemException ex)
		 {
			this.DirectSolcoi = true;
			Dumb.SetRowError(this, this.tableSubSamples.DirectSolcoiColumn, ex);

		 }




	  }
      
		   
		
   }



}
namespace DB
{
   partial class LINAA
   {


	
	  public partial class MeasurementsRow
	  {

	
		    
			  internal void SetCOIN(ref IEnumerable<PeaksRow> peaks, ref  IEnumerable<COINRow> coins)
		{


		   double? coi = null;

		   foreach (PeaksRow p in peaks)
		   {
			   try
			   {
				   p.COI = 1;
				   coi = null;

				   Func<LINAA.COINRow, bool> coinpeakFinder = o =>
				   {
					   if (o.Energy == p.Energy) return true;
					   else return false;
				   };

				   string coinIso = p.Iso.Replace("-", null).ToUpper().Trim();
				   IEnumerable<COINRow> coins2 = coins.Where(o => coinIso.CompareTo(o.Isotope.Trim()) == 0);

				   if (coins2.Count() != 0)
				   {
					   LINAA.COINRow c = coins2.FirstOrDefault(coinpeakFinder);
					   if (!EC.IsNuDelDetch(c)) coi = (double?)c.COI;
				   }
				   else
				   {
					   LINAATableAdapters.QTA qta = (this.tableMeasurements.DataSet as LINAA).QTA;
					   coi = (double?)qta.GetCOI(this.Detector, this.SubSamplesRow.GeometryName, coinIso, this.Position, p.Energy);
				   }

				   if (coi != null) p.COI = (double)coi;
				   else p.ID = -1 * Math.Abs(p.ID);
			   }
			   catch (SystemException ex)
			   {
				   Dumb.SetRowError(p, (p.Table as LINAA.PeaksDataTable).EfficiencyColumn, ex);
				   p.ID = -1 * Math.Abs(p.ID);
			   }
		   }

		   
		}

		    
		  internal void SetEfficiencyOld(ref IEnumerable<PeaksRow> peaks, ref IEnumerable<SolangRow> solangs, ref IEnumerable<SolangRow> solangsRefs, ref  IEnumerable<DetectorsCurvesRow> curves, ref GeometryRow reference)
		  {


			  double? SolidAngleRef = null;
			  double? SolidAngle = null;
			  double SolidFactor = 1;
			  double? Log10Effi = null;

			  foreach (PeaksRow p in peaks)
			  {
				  try
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
					  if (!EC.IsNuDelDetch(auxSol)) SolidAngleRef = (double?)auxSol.Solang;

					  //take geometry at peak
					  auxSol = solangs.FirstOrDefault<LINAA.SolangRow>(solpeakFinder);
					  if (!EC.IsNuDelDetch(auxSol)) SolidAngle = (double?)auxSol.Solang;

					  if (curves.Count() != 0)
					  {
						  DetectorsCurvesRow effiCurve = curves.FirstOrDefault(efficurveFinder);
						  if (!EC.IsNuDelDetch(effiCurve))
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
						  LINAATableAdapters.QTA qta = (this.tableMeasurements.DataSet as LINAA).QTA;
						  //LINAATableAdapters.QTA qta = (this.tableMeasurements.DataSet as LINAA).QTA;
						  if (SolidAngle == null)
						  {
							  SolidAngle = (double?)qta.GetSolidAngle(this.SubSamplesRow.GeometryName + this.SubSamplesRow.SubSampleName, this.Position, this.Detector, p.Energy);
						  }
						  if (SolidAngleRef == null)
						  {
							  SolidAngleRef = (double?)qta.GetSolidAngle(reference.GeometryName, reference.Position, this.Detector, p.Energy);
						  }
						  if (Log10Effi == null)
						  {
							  Log10Effi = (double?)qta.GetLog10Effi(p.Energy, this.Detector, reference.GeometryName, reference.Position);
						  }

					  }

					  //finally calculate
					  if (SolidAngle != null && SolidAngleRef != null && Log10Effi != null)
					  {
						  SolidFactor = (double)(SolidAngle / SolidAngleRef);
						  p.Efficiency = Math.Pow(10.0, (double)Log10Effi) * (SolidFactor);
					  }
					  else p.ID = -1 * Math.Abs(p.ID);

				  }
				  catch (SystemException ex)
				  {
					  Dumb.SetRowError(p, (p.Table as LINAA.PeaksDataTable).EfficiencyColumn, ex);
					  p.ID = -1 * Math.Abs(p.ID);
				  }
			  }



		  }
		  



	  }

	  /// <summary>
	  /// Calculates the efficiencies and COIS for the peaks inside the given array of measurements
	  /// </summary>
	  /// <param name="meas">Measurements containing peaks to set efficiencies and COIS</param>
	  public void CalculateOld(ref IEnumerable<MeasurementsRow> meas)
	  {


		 meas = EC.NotDeleted<MeasurementsRow>(meas);

		 if (meas.Count() == 0) return;

		 GeometryRow reference = this.tableGeometry.FirstOrDefault(LINAA.RefFinder("REF"));

		 if (EC.IsNuDelDetch(reference)) return;

		 foreach (MeasurementsRow m in meas)
		 {
			IEnumerable<PeaksRow> peaks = m.GetPeaksRows();

			//get efficiencies
			reference.Detector = m.Detector;
			reference.Position = 5;
			IEnumerable<DetectorsCurvesRow> curves = reference.GetDetectorsCurvesRows();
			IEnumerable<SolangRow> solangsRefs = reference.GetSolangRows();
			IEnumerable<SolangRow> solangs = m.GetSolangRows();
			m.SetEfficiencyOld(ref peaks, ref solangs, ref solangsRefs, ref curves, ref reference);

			//get COIS
			if (m.SubSamplesRow.GeometryRow == null) continue;
			m.SubSamplesRow.GeometryRow.Detector = m.Detector;
			m.SubSamplesRow.GeometryRow.Position = m.Position;
			IEnumerable<COINRow> coins = m.SubSamplesRow.GeometryRow.GetCOINRows();
			m.SetCOIN(ref peaks, ref coins);


		 }

	  }






	  public void PopulateCOIN(ref IEnumerable<LINAA.SubSamplesRow> samples)
	  {
		 //load all geometries in DB   and Reference Geometry
		 LINAA.COINDataTable table = null;
		 //  DataTableReader reader = null;
		 DB.LINAATableAdapters.COINTableAdapter cota = new DB.LINAATableAdapters.COINTableAdapter();
		 System.Collections.Generic.HashSet<string> hsgeos = new System.Collections.Generic.HashSet<string>();

		 foreach (LINAA.SubSamplesRow s in samples)
		 {

			try
			{

			   if (s.GeometryRow == null) continue;
			   if (s.IsGeometryNameNull()) continue;

			   System.Collections.Generic.IEnumerable<LINAA.MeasurementsRow> measurements = s.GetMeasurementsRows();
			   foreach (LINAA.MeasurementsRow m in measurements)
			   {
				  //find solangs for this meas --> geometry,det,position
				  //find coins for this geometry --> detector, position
				  s.GeometryRow.Detector = m.Detector;
				  s.GeometryRow.Position = m.Position;

				  System.Collections.Generic.IEnumerable<LINAA.COINRow> coins = s.GeometryRow.GetCOINRows();
				  if (coins.Count() != 0) continue;

				  //not found already, so load them
				  if (!hsgeos.Add(m.SubSamplesRow.GeometryName + m.Detector)) continue;
				  table = cota.GetDataByDetectorGeometryPosition(m.Detector, m.SubSamplesRow.GeometryName, m.Position);

				  if (table != null && table.Rows.Count != 0)
				  {
					 //  reader = table.CreateDataReader();
					 this.tableCOIN.Merge(table, false, MissingSchemaAction.AddWithKey);
					 //  reader.Dispose();
					 table.Dispose();
				  }
				  else if (table != null && table.Rows.Count == 0) table.Dispose();

			   }
			}
			catch (SystemException ex)
			{
			   Dumb.SetRowError(s, tableSubSamples.DirectSolcoiColumn, ex);
			   this.AddException(ex);

			}
		 }

		 cota.Dispose();
		 cota = null;
		 table = null;

	  }
	  public void PopulateSolang(ref IEnumerable<LINAA.SubSamplesRow> samples)
	  {
		 LINAA.SolangDataTable soldt = null;

		 DB.LINAATableAdapters.SolangTableAdapter solta = new DB.LINAATableAdapters.SolangTableAdapter();
		 System.Collections.Generic.HashSet<string> hsgeos = new System.Collections.Generic.HashSet<string>();

		 foreach (LINAA.SubSamplesRow s in samples)
		 {

			try
			{

			   if (s.GeometryRow == null) continue;
			   if (s.IsGeometryNameNull()) continue;

			   System.Collections.Generic.IEnumerable<LINAA.MeasurementsRow> measurements = s.GetMeasurementsRows();


			   foreach (LINAA.MeasurementsRow m in measurements)
			   {
				  //find solangs for this meas --> geometry,det,position
				  System.Collections.Generic.IEnumerable<LINAA.SolangRow> solangs = m.GetSolangRows();
				  //force solcoi if solangs not found
				  if (solangs.Count() == 0 && !m.IsGeometryNull())
				  {
					 if (!hsgeos.Add(m.Detector + m.Geometry + m.Position)) continue;

					 soldt = solta.GetDataByGeoDetPos(m.Geometry, m.Detector, m.Position);
					 if (soldt != null && soldt.Rows.Count == 0) soldt.Dispose();
					 else if (soldt != null)
					 {
						//merge with reference
						// reader = soldt.CreateDataReader();
						this.tableSolang.Merge(soldt, false, MissingSchemaAction.AddWithKey);
						// reader.Dispose();
						soldt.Dispose();
					 }
					 soldt = solta.GetDataByGeoDetPos("REF", m.Detector, 5);
					 if (soldt != null && soldt.Rows.Count == 0) soldt.Dispose();
					 else if (soldt != null)
					 {
						//reader = soldt.CreateDataReader();
						this.tableSolang.Merge(soldt, false, MissingSchemaAction.AddWithKey);
						//reader.Dispose();
						soldt.Dispose();
					 }
				  }
			   }
			}
			catch (SystemException ex)
			{
			   Dumb.SetRowError(s, tableSubSamples.DirectSolcoiColumn, ex);
			   this.AddException(ex);
			}
		 }

		 solta.Dispose();
		 solta = null;
		 //  reader = null;
		 soldt = null;



	  }














   }
}
