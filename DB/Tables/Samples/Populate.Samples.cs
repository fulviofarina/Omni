using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

//using DB.Interfaces;

namespace DB
{
  public partial class LINAA : ISamples
  {
    public void LoadSampleData(bool load)
    {
      if (load)
      {
        this.Measurements.BeginLoadData();
        this.Peaks.BeginLoadData();
        this.Samples.BeginLoadData();
        this.IRequestsAverages.BeginLoadData();
        this.IPeakAverages.BeginLoadData();
      }
      else
      {
        this.Measurements.EndLoadData();
        this.Peaks.EndLoadData();
        this.Samples.EndLoadData();
        this.IRequestsAverages.EndLoadData();
        this.IPeakAverages.EndLoadData();
      }

      PopulateSelectedExpression(!load);
    }

    //
    private Action[] PMStd()
    {
      Action[] populatorArray = null;

      populatorArray = new Action[]   {
        PopulateStandards,
       PopulateMonitors,
         PopulateMonitorFlags,
         };

      return populatorArray;
    }

    public void PopulateStandards()
    {
      try
      {
        this.tableStandards.BeginLoadData();
        this.tableStandards.Clear();
        this.TAM.StandardsTableAdapter.Fill(this.tableStandards);

        this.tableStandards.AcceptChanges();
        this.tableStandards.EndLoadData();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    public void PopulateMonitorFlags()
    {
      try
      {
        this.tableMonitorsFlags.BeginLoadData();
        this.tableMonitorsFlags.Clear();
        this.TAM.MonitorsFlagsTableAdapter.Fill(this.tableMonitorsFlags);
        this.tableMonitorsFlags.AcceptChanges();
        this.tableMonitorsFlags.EndLoadData();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    public void PopulateMonitors()
    {
      try
      {
        this.tableMonitors.BeginLoadData();
        tableMonitors.Clear();
        TAM.MonitorsTableAdapter.DeleteNulls();
        TAM.MonitorsTableAdapter.Fill(tableMonitors);
        this.tableMonitors.EndLoadData();
        tableMonitors.AcceptChanges();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }

    public int AddSamples(string project, ref ICollection<string> hsamples)
    {
      int added = 0;
      if (hsamples.Count == 0) return added;

      // bool cd = false;
      project = project.ToUpper().Trim();
      //  if (project.Contains(DB.Properties.Misc.Cd)) cd = true;

      string Sname = this.SubSamples.SubSampleNameColumn.ColumnName;
      string Mname = this.Monitors.MonNameColumn.ColumnName;
      int? id = this.IrradiationRequests.FindIrrReqID(project);

      foreach (string s in hsamples)
      {
        LINAA.SubSamplesRow sample = this.tableSubSamples.FindBySample(s, true, id);

        if (Rsx.EC.IsNuDelDetch(sample)) continue;
        if (sample.RowState == DataRowState.Added) added++;

        if (sample.MonitorsRow == null)
        {
          string sampleNrOrMon = s.ToString().Replace(project.Substring(1), null);
          LINAA.MonitorsRow mon = this.Monitors.FindByMonName(sampleNrOrMon);
          if (mon != null) sample.MonitorsRow = mon;
        }
        if (sample.VialTypeRow == null && sample.IrradiationRequestsRow != null)
        {
          string channel = sample.IrradiationRequestsRow.ChannelName;
          IEnumerable<VialTypeRow> capsules = this.VialType.Where(o => !o.IsVialTypeRefNull() && o.Comments.ToUpper().Contains(channel));  //the capsule for the channel
          if (capsules.Count() != 0)
          {
            LINAA.VialTypeRow c = capsules.FirstOrDefault();
            if (c != null) sample.VialTypeRow = c;
          }
        }
      }

      return added;
    }

    public void LoadMonitorsFile(string file)
    {
      LINAA.MonitorsDataTable importing = new LINAA.MonitorsDataTable(false);

      importing.ReadXml(file);

      foreach (LINAA.MonitorsRow i in importing)
      {
        LINAA.MonitorsRow l = this.tableMonitors.FindByMonName(i.MonName); //local monitor
        if (l == null)
        {
          l = this.tableMonitors.NewMonitorsRow();
          this.tableMonitors.AddMonitorsRow(l);

          l.MonName = i.MonName;
        }
        if (l != null)
        {
          if (l.IsLastMassDateNull())
          {
            l.LastMassDate = DateTime.Now.Subtract(new TimeSpan(3650, 0, 0, 0));
          }
          if (l.IsLastIrradiationDateNull())
          {
            l.LastIrradiationDate = DateTime.Now.Subtract(new TimeSpan(3650, 0, 0, 0));
          }
          if (!i.IsLastMassDateNull() && i.LastMassDate > l.LastMassDate)
          {
            if (!i.IsMonGrossMass1Null())
            {
              l.MonGrossMass1 = i.MonGrossMass1;
            }
            if (!i.IsMonGrossMass2Null())
            {
              l.MonGrossMass2 = i.MonGrossMass2;
            }

            l.LastMassDate = i.LastMassDate;
          }
          if (!i.IsLastMassDateNull() && i.LastIrradiationDate > l.LastIrradiationDate)
          {
            double daysdiff = (i.LastIrradiationDate.Subtract(l.LastIrradiationDate)).TotalDays;
            l.LastIrradiationDate = i.LastIrradiationDate;
          }
        }
      }
    }

    public void PopulateSubSamples(Int32 IrReqID)
    {
      try
      {
        //   this.tableSubSamples.TableNewRow += new DataTableNewRowEventHandler(this.tableSubSamples.SubSamplesDataTable_TableNewRow);

        TAM.SubSamplesTableAdapter.DeleteNulls();

        LINAA.SubSamplesDataTable newsamples = new SubSamplesDataTable(false);
        TAM.SubSamplesTableAdapter.FillBy(newsamples, IrReqID);

        string uniquefield = newsamples.SubSampleNameColumn.ColumnName;
        string Indexfield = newsamples.SubSamplesIDColumn.ColumnName;
        TAMDeleteMethod remov = this.tAM.SubSamplesTableAdapter.Delete;
        bool duplicates = RemoveDuplicates(newsamples, uniquefield, Indexfield, ref remov);

        if (duplicates)
        {
          newsamples.Clear();
          newsamples.Dispose();
          newsamples = null;
          PopulateSubSamples(IrReqID);
          return;
        }

        this.tableSubSamples.BeginLoadData();
        this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);
        this.tableSubSamples.EndLoadData();
        this.tableSubSamples.AcceptChanges();

        //	this.tableSubSamples.TableNewRow -= new DataTableNewRowEventHandler(this.tableSubSamples.SubSamplesDataTable_TableNewRow);
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }
  }
}