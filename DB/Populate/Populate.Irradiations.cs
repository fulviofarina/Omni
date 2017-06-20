﻿using System;
using System.Linq;
using Rsx;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IIrradiations
    {
        public int? FindIrradiationID(String project)
        {
            int? IrrReqID = null;

            LINAA.IrradiationRequestsRow irr = this.FindIrradiationByCode(project.Trim().ToUpper());
            if (!EC.IsNuDelDetch(irr)) IrrReqID = irr.IrradiationRequestsID;
            return IrrReqID;
        }

        public IrradiationRequestsRow FindIrradiationByCode(string project)
        {
            return this.tableIrradiationRequests
                .FirstOrDefault(i => i.IrradiationCode.CompareTo(project.Trim().ToUpper()) == 0);
        }

        /// <summary>
        /// Adds a new row and returns it with basic dat aon it
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public IrradiationRequestsRow AddNewIrradiation(string project)
        {
            IrradiationRequestsRow i = addIrradiation(project);

            return i;
        }

      

        public ChannelsRow AddNewChannel()
        {
            // ChannelsRow v = e.NewObject as ChannelsRow;//Interface.IDB.Matrix.NewMatrixRow();
            ChannelsRow v = addChannel();
            return v;
        }

     
    }

    public partial class LINAA : IIrradiations
    {
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
                // Dumb.CleanColumnExpressions(tableIrradiationRequests);
                this.tableIrradiationRequests.BeginLoadData();
                this.tableIrradiationRequests.Clear();
                //Handlers(this.tableIrradiationRequests, false, tableIrradiationRequests.DataColumnChanged);
                this.TAM.IrradiationRequestsTableAdapter.Fill(this.tableIrradiationRequests);
                // IEnumerable<DataRow> rows = this.tableIrradiationRequests.AsEnumerable();
                // LINAA.SetAdded(ref rows);
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