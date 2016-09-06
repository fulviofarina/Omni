using System;
//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IIrradiations
    {
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

        public Int32? FindIrrReqID(string project)
        {
            return this.tableIrradiationRequests.FindIrrReqID(project);
        }
    }
}