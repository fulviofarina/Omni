using System;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IIrradiations
    {

        public IrradiationRequestsRow AddIrradiation(string project)
        {
            string projetNoCd = project.Trim().ToUpper();
           
            if (projetNoCd.Length > 2)
            {
                if (projetNoCd.Substring(projetNoCd.Length - 2).CompareTo(DB.Properties.Misc.Cd) == 0)
                {
                    projetNoCd = projetNoCd.Replace(DB.Properties.Misc.Cd, null);
                }
            }
            IrradiationRequestsRow i = this.IrradiationRequests.NewIrradiationRequestsRow();
            this.IrradiationRequests.AddIrradiationRequestsRow(i);

            i.IrradiationStartDateTime = DateTime.Now;
            i.IrradiationCode = projetNoCd;

            return i;
        }
        public Int32? FindIrrReqID(string project)
        {
            return this.tableIrradiationRequests.FindIrrReqID(project);
        }

        public void PopulateChannels()
        {
            try
            {
                this.tableChannels.BeginLoadData();
                this.tableChannels.Clear();
                this.TAM.ChannelsTableAdapter.Fill(this.tableChannels);
                this.tableChannels.AcceptChanges();
                this.tableChannels.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateIrradiationRequests()
        {
            try
            {
                //	Dumb.CleanColumnExpressions(tableIrradiationRequests);
                this.tableIrradiationRequests.BeginLoadData();
                this.tableIrradiationRequests.Clear();
                //Handlers(this.tableIrradiationRequests, false, tableIrradiationRequests.DataColumnChanged);
                this.TAM.IrradiationRequestsTableAdapter.Fill(this.tableIrradiationRequests);
                //    IEnumerable<DataRow> rows = this.tableIrradiationRequests.AsEnumerable();
                //   LINAA.SetAdded(ref rows);
                this.tableIrradiationRequests.AcceptChanges();
                this.tableIrradiationRequests.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}