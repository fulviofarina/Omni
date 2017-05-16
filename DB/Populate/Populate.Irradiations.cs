using System;
using System.Linq;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IIrradiations
    {

        public int? FindIrrReqID(String project)
        {
            int? IrrReqID = null;

            LINAA.IrradiationRequestsRow irr = this.FindByIrradiationCode(project);
            if (irr != null) IrrReqID = irr.IrradiationRequestsID;

            return IrrReqID;
        }

        public IrradiationRequestsRow FindByIrradiationCode(string project)
        {
            project = project.ToUpper();
            return this.tableIrradiationRequests.FirstOrDefault(i => i.IrradiationCode.CompareTo(project) == 0);
        }

        /// <summary>
        /// Adds a new row and returns it with basic dat aon it
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
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