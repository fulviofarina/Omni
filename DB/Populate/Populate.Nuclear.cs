using System;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : INuclear
    {
        //

        public void PopulateElements()
        {
            try
            {
                this.tableElements.BeginLoadData();
                this.tableElements.Clear();

                this.TAM.ElementsTableAdapter.Fill(this.tableElements);
                this.tableElements.AcceptChanges();
                this.tableElements.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulatepValues()
        {
            try
            {
                this.tablepValues.BeginLoadData();
                this.tablepValues.Clear();
                this.TAM.pValuesTableAdapter.Fill(this.tablepValues);
                this.tablepValues.AcceptChanges();
                this.tablepValues.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateReactions()
        {
            try
            {
                this.tableReactions.BeginLoadData();
                this.tableReactions.Clear();
                this.TAM.ReactionsTableAdapter.Fill(this.tableReactions);
                //  HashSet<string> key = new HashSet<string>();
                //IEnumerable<ReactionsRow> r = dt.AsEnumerable().Where(o => !key.Add(o.Element + "," + o.Radioisotope));
                this.tableReactions.AcceptChanges();
                this.tableReactions.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateSigmas()
        {
            try
            {
                this.tableSigmas.BeginLoadData();
                this.tableSigmas.Clear();
                this.TAM.SigmasTableAdapter.FillByReactions(this.tableSigmas);
                this.tableSigmas.AcceptChanges();
                this.tableSigmas.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateSigmasSal()
        {
            try
            {
                this.tableSigmasSal.BeginLoadData();
                this.tableSigmasSal.Clear();
                this.TAM.SigmasSalTableAdapter.Fill(this.tableSigmasSal);
                this.tableSigmasSal.AcceptChanges();
                this.tableSigmasSal.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateYields()
        {
            try
            {
                this.tableYields.BeginLoadData();
                this.tableYields.Clear();
                this.TAM.YieldsTableAdapter.Fill(this.tableYields);
                this.tableYields.AcceptChanges();
                this.tableYields.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}