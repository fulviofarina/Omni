using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;

//using DB.Interfaces;
using Rsx;

namespace k0X
{
  public partial class ucSamples
  {
    protected void CheckNode(ref LINAA.SubSamplesRow s)
    {
      //so it does not run on populating...
      Populating = true;

      try
      {
        TreeNode old = MakeSampleNode(ref s);
        LINAA.MeasurementsRow[] measurements = s.GetMeasurementsRows();
        //old.Nodes.Clear();
        foreach (LINAA.MeasurementsRow m in measurements)
        {
          try
          {
            LINAA.MeasurementsRow aux = m;
            TreeNode MeasNode = MakeMeasurementNode(ref aux);
            SetAMeasurementNode(ref MeasNode);
            if (!old.Nodes.Contains(MeasNode))
            {
              MeasNode.Collapse();
              old.Nodes.Add(MeasNode);    //add childrens already
            }
          }
          catch (SystemException ex)
          {
            Interface.IReport.AddException(ex);
          }
        }
        SetASampleNode(this.sampleDescription.Checked, ref old);
      }
      catch (SystemException ex)
      {
        Interface.IReport.AddException(ex);
      }

      Populating = false;
    }

    protected void CleanNodes()
    {
      if (samples == null) return;
      foreach (LINAA.SubSamplesRow s in samples)
      {
        TreeNode n = (TreeNode)s.Tag;
        if (n != null)
        {
          try
          {
            if (n.TreeView != null)
            {
              IntPtr a = n.TreeView.Handle;
            }
            else s.Tag = null;
          }
          catch (ObjectDisposedException ex)
          {
            s.Tag = null;
          }
        }
      }
    }

    public T GetNodeDataRow<T>()
    {
      object o = null;
      if (this.TV.SelectedNode != null) o = this.TV.SelectedNode.Tag;
      return (T)o;
    }

    public bool Populating
    {
      set
      {
        if (value) this.TV.AfterCheck -= this.TV_AfterCheck;
        else this.TV.AfterCheck += this.TV_AfterCheck;
      }
    }

    protected internal void BuildTV()
    {
      if (IsEmpty()) return;

      this.progress.Value = 0;

      Populating = true;

      foreach (TreeNode n in Clon) n.Nodes.Clear();

      this.TV.BeginUpdate();
      this.TV.Nodes.Clear();
      this.TV.Nodes.AddRange(Clon); //adds template

      string filterby = (Interface.Get() as LINAA).SubSamples.SubSampleTypeColumn.ColumnName;
      bool samDesc = this.sampleDescription.Checked;
      //	monitors = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Mon, ref samples);
      //	subSamples = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Smp, ref  samples);
      //	standards = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Std, ref samples);
      //	refMaterials = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.RefMat, ref samples);
      //	blanks = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Blk, ref samples);

      Interface.IReport.Msg("Building nodes", "Building sample nodes for " + this.Name, true);
      Application.DoEvents();

      this.progress.Maximum = samples.Count();

      CleanNodes();

      //  IList<TreeNode> samplesNodes = new List<TreeNode>();
      foreach (LINAA.SubSamplesRow sample in samples)
      {
        LINAA.SubSamplesRow aux = sample;

        if (EC.IsNuDelDetch(aux)) continue;
        TreeNode SampleNode = MakeSampleNode(ref aux);

        // SetASampleNode(DescriptionAsNodeTitle, ref SampleNode);

        TreeNodeCollection col = TV.Nodes[sample.SubSampleType].Nodes;

        TreeNode toAdd = SampleNode;
        if (IsClone)
        {
          toAdd = (TreeNode)SampleNode.Clone();
          toAdd.Tag = sample;
        }

        //samplesNodes.Add( SampleNode);
        if (!col.Contains(toAdd))
        {
          if (toAdd.Parent != null) toAdd.Remove();
          col.Add(toAdd);
        }

        toAdd.Collapse();

        this.progress.PerformStep();
      }

      IEnumerable<TreeNode> basis = this.TV.Nodes.OfType<TreeNode>();
      IEnumerable<TreeNode> toDel = basis.Where(c => c.GetNodeCount(false) == 0).ToList();
      foreach (TreeNode n in toDel) n.Remove();

      this.TV.CollapseAll();

      TreeNode first = basis.FirstOrDefault();
      if (first != null) first.ExpandAll();

      this.TV.EndUpdate();
      Populating = false;
    }

    protected static TreeNode MakeMeasurementNode(ref LINAA.MeasurementsRow m)
    {
      TreeNode MeasNode = m.Tag as TreeNode;
      if (MeasNode == null)
      {
        MeasNode = new TreeNode();

        MeasNode.Name = m.MeasurementID.ToString();   //name of node
        MeasNode.Text = MeasNode.Name;
        //linking
        MeasNode.Tag = m; //link toRow measurement interface
        m.Tag = MeasNode;   //backlink toRow measurement interface
      }

      return MeasNode;
    }

    protected static TreeNode MakeSampleNode(ref LINAA.SubSamplesRow sample)
    {
      TreeNode SampleNode = sample.Tag as TreeNode;

      if (SampleNode == null)
      {
        SampleNode = new TreeNode();
        SampleNode.Name = sample.SubSamplesID.ToString();
        SampleNode.Text = SampleNode.Name;
        SampleNode.Tag = sample; //link subsample Row  or subsample interface
        sample.Tag = SampleNode;
      }

      return SampleNode;
    }

    protected static void SetAMeasurementNode(ref TreeNode MeasNode)
    {
      if (IsBadNode(ref MeasNode)) return;
      LINAA.MeasurementsRow m = MeasNode.Tag as LINAA.MeasurementsRow;

      if (m.IsSelectedNull()) m.Selected = true;

      MeasNode.Text = m.Detector + " " + m.Position + " " + m.MeasurementNr;
      MeasNode.ToolTipText = "Detector: " + m.Detector + "\nPosition: " + m.Position + "\nCounting Time: " + m.CountTime.ToString() + "s\nStarted: " + m.MeasurementStart.ToString();

      if (!m.NeedsPeaks) MeasNode.ImageKey = "green";
      else MeasNode.ImageKey = "red";
      MeasNode.Checked = m.Selected;
      if (!m.SubSamplesRow.Selected && m.Selected) m.SubSamplesRow.Selected = true;
    }

    protected static bool IsBadNode(ref TreeNode checkme)
    {
      if (checkme.Tag == null) return true;

      System.Data.DataRow row = checkme.Tag as System.Data.DataRow;

      if (EC.IsNuDelDetch(row))
      {
        checkme.Remove();
        return true;
      }
      return false;
    }

    protected static void SetASampleNode(bool DescriptionAsNodeTitle, ref TreeNode SampleNode)
    {
      if (IsBadNode(ref SampleNode)) return;

      LINAA.SubSamplesRow sample = SampleNode.Tag as LINAA.SubSamplesRow;

      if (sample.IsSubSampleNameNull()) SampleNode.Text = sample.SubSamplesID.ToString();
      else SampleNode.Text = sample.SubSampleName;

      //if cadmium put in gray

      if (!sample.IsENAANull() && sample.ENAA) SampleNode.ForeColor = System.Drawing.Color.Gray;
      else SampleNode.ForeColor = System.Drawing.Color.Blue;

      SampleNode.ImageKey = "red";

      if (!sample.HasErrors)
      {
        if (!sample.NeedsSSF && !sample.NeedsMeasurements && !sample.NeedsPeaks) SampleNode.ImageKey = "green";
      }
      SampleNode.Checked = sample.Selected;

      if (DescriptionAsNodeTitle)  //if sample text as description
      {
        if (!sample.IsSubSampleDescriptionNull()) SampleNode.Text = sample.SubSampleDescription;
        else SampleNode.Text = sample.SubSampleName;
        if (!sample.IsSubSampleNameNull()) SampleNode.ToolTipText = sample.SubSampleName;
      }
      else
      {
        if (!sample.IsSubSampleNameNull()) SampleNode.Text = sample.SubSampleName;
        if (!sample.IsSubSampleDescriptionNull()) SampleNode.ToolTipText = sample.SubSampleDescription;
      }
    }

    protected TreeNode[] Clon;
  }
}