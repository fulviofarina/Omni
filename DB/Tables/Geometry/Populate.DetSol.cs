using System;
using System.Collections.Generic;

//using DB.Interfaces;
using Rsx;

namespace DB
{
  public partial class LINAA : IDetSol
  {
    //   #region Populate Detectors/Solang
    private Action[] PMDetect()
    {
      Action[] populatorArray = null;

      populatorArray = new Action[]   {
            PopulateDetectorCurves,
            PopulateDetectorAbsorbers,
            PopulateDetectorDimensions,
            PopulateDetectorHolders
     };

      return populatorArray;
    }

    public void PopulateCOIList()
    {
      COINDataTable coi = this.TAM.COINTableAdapter.GetGeometryList();
      COIN.Clear();
      COIN.Merge(coi);
      COIN.AcceptChanges();
    }

    public void PopulateDetectorCurves()
    {
      try
      {
        this.tableDetectorsCurves.BeginLoadData();
        this.tableDetectorsCurves.Clear();
        this.TAM.DetectorsCurvesTableAdapter.Fill(this.tableDetectorsCurves);
        this.tableDetectorsCurves.EndLoadData();
        this.tableDetectorsCurves.AcceptChanges();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    public void PopulateDetectorAbsorbers()
    {
      try
      {
        this.tableDetectorsAbsorbers.BeginLoadData();
        this.tableDetectorsAbsorbers.Clear();
        this.TAM.DetectorsAbsorbersTableAdapter.Fill(this.tableDetectorsAbsorbers);
        this.tableDetectorsAbsorbers.EndLoadData();
        this.tableDetectorsAbsorbers.AcceptChanges();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    public void PopulateDetectorDimensions()
    {
      try
      {
        this.tableDetectorsDimensions.BeginLoadData();
        this.tableDetectorsDimensions.Clear();
        this.TAM.DetectorsDimensionsTableAdapter.Fill(this.tableDetectorsDimensions);
        this.tableDetectorsDimensions.EndLoadData();

        this.tableDetectorsDimensions.AcceptChanges();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    public void PopulateDetectorHolders()
    {
      try
      {
        this.tableHolders.BeginLoadData();
        this.tableHolders.Clear();
        this.TAM.HoldersTableAdapter.Fill(this.tableHolders);
        this.tableHolders.EndLoadData();
        this.tableHolders.AcceptChanges();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    /// <summary>
    /// Gets a non-repeated list of Detectors in the database
    /// </summary>
    protected internal ICollection<string> detectorsList;

    public ICollection<string> DetectorsList
    {
      get
      {
        if (detectorsList != null) detectorsList.Clear();
        detectorsList = Dumb.HashFrom<string>(this.DetectorsDimensions.DetectorColumn);  //list of detectors
        return detectorsList;
      }
      set { detectorsList = value; }
    }

    //  #endregion Populate Detectors/Solang
  }
}