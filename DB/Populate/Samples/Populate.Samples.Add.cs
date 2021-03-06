﻿using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    /// <summary>
    /// THIS IS QUITE CLEAN
    /// </summary>
    public partial class LINAA : ISamples
    {
        public bool CleanSSFs(ref IEnumerable<MatSSFRow> ssfs)
        {
            if (ssfs.Count() != 0)
            {
                Delete(ref ssfs);
                MatSSF.AcceptChanges();
            }
            return true;
        }

        public IRequestsAveragesRow AddIRequestsAverage(Int32 NAAID, ref SubSamplesRow s)
        {
            IRequestsAveragesRow irs = this.tableIRequestsAverages.NewIRequestsAveragesRow();
            this.tableIRequestsAverages.AddIRequestsAveragesRow(irs);
            irs.NAAID = NAAID;

            //irs.Radioisotope = iso;
            //	irs.Element = sym;
            if (!EC.IsNuDelDetch(s))
            {
                irs.Sample = s.SubSampleName;
                // irs.Project = s.IrradiationCode;
            }
            return irs;
        }

        public SubSamplesRow AddSample(string sampleName, int? IrrReqID = null)
        {
            SubSamplesRow sample = FindSample(sampleName);
            if (sample == null)
            {
                sample = this.tableSubSamples.NewSubSamplesRow();
                if (IrrReqID != null) sample.IrradiationRequestsID = (int)IrrReqID;
                // sample.SetIrradiationRequestID(IrrReqID);
                sample.SetCreationDate();
                this.tableSubSamples.AddSubSamplesRow(sample);
                //return sample;
                //   sample = addSamples(IrrReqID);
            }
            sample.SetName(sampleName);
            return sample;
        }

        //THIS CLASS IS QUITE CLEAN SOME THINGS TO DO
        public SubSamplesRow AddSample(ref IrradiationRequestsRow ir, string sampleName = "")
        {
            string project = ir.IrradiationCode.Trim().ToUpper();
            return AddSample(project, sampleName);
        }

        public SubSamplesRow AddSample(string project, string sampleName = "")
        {
            IList<SubSamplesRow> list = new List<SubSamplesRow>();
            SubSamplesRow s = addSample(sampleName);
            list.Add(s);
            IEnumerable<SubSamplesRow> samples = list;
            AddSamples(project, ref samples);
            return s;
        }

        public IEnumerable<SubSamplesRow> AddSamples(string project, ref IEnumerable<SubSamplesRow> samplesToImport, bool save = true)
        {
            IrradiationRequestsRow ir = this.FindIrradiationByCode(project);
            samplesToImport = AddSamples(ref ir, ref samplesToImport, save);
            return samplesToImport;
        }

        public IEnumerable<SubSamplesRow> AddSamples(ref IrradiationRequestsRow ir, ref IEnumerable<SubSamplesRow> samplesToImport, bool save = true)
        {
            addSamples(ref ir, ref samplesToImport);
            return samplesToImport;
        }

        public IEnumerable<SubSamplesRow> AddSamples(ref IEnumerable<string> hsamples, string project, bool salve = true)
        {
            IList<LINAA.SubSamplesRow> ls = new List<SubSamplesRow>();
            foreach (string sname in hsamples)
            {
                SubSamplesRow s = addSample(sname);
                ls.Add(s);
            }
            IEnumerable<SubSamplesRow> samps = ls;
            return AddSamples(project, ref samps, salve);
        }

        /// <summary>
        /// The actual function
        /// </summary>
        /// <param name="sampleName"></param>
        /// <returns></returns>
        private SubSamplesRow addSample(string sampleName = "")
        {
            LINAA.SubSamplesRow s = null;
            s = this.SubSamples.NewSubSamplesRow();
            s.SetName(sampleName);
            s.SetCreationDate();
            this.SubSamples.AddSubSamplesRow(s);
            return s;
        }

        /// <summary>
        /// The actual function
        /// </summary>
        /// <param name="ir">             </param>
        /// <param name="samplesToImport"></param>
        private void addSamples(ref IrradiationRequestsRow ir, ref IEnumerable<SubSamplesRow> samplesToImport, bool saveSamp = true)
        {
            IEnumerable<SubSamplesRow> samplesInTable = this.FindSamplesByIrrReqID(ir.IrradiationRequestsID);
            //join them if any
            if (samplesToImport != null && samplesToImport.Count() != 0)
            {
                samplesToImport = samplesToImport.Union(samplesInTable);
            }
            else samplesToImport = samplesInTable;

            string project = ir.GetIrradiationCode();
            int _lastSampleNr = GetLastSampleNr(ref samplesToImport, project);

            //set irr request BASIC
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                s.SetParent(ref ir);
            }
            //set monitors
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                bool attachMon = EC.IsNuDelDetch(s.MonitorsRow);
                //attach monitor
                if (attachMon)
                {
                    string monName = s.GetMonitorNameFromSampleName();
                    //find monitor if any
                    LINAA.MonitorsRow mon = this.FindMonitorByName(monName);
                    s.SetParent(ref mon);
                }
                s.SetName(ref _lastSampleNr);
                //update the date
                s.SetCreationDate();
            }

            /*
            //set vials
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                //attach vial
                bool attachRabbit = (EC.IsNuDelDetch(s.ChCapsuleRow));
                attachRabbit = attachRabbit && !EC.IsNuDelDetch(s.IrradiationRequestsRow);
                if (attachRabbit)
                {
                    string channel = s.IrradiationRequestsRow.ChannelsRow.ChannelName;
                    IEnumerable<VialTypeRow> capsules = FindCapsules(channel);
                    LINAA.VialTypeRow c = capsules.FirstOrDefault();
                    s.SetParent(ref c);

                // s.ChCapsuleRow;
                }
            }
            */

            if (saveSamp) SaveRows(ref samplesToImport);

            addUnits(ref samplesToImport);
        }

        private void addUnits(ref IEnumerable<SubSamplesRow> samplesToImport, bool saveUnit = true)
        {
            foreach (SubSamplesRow s in samplesToImport)
            {
                SubSamplesRow sample = s;
                addUnit(ref sample);
            }
            IEnumerable<UnitRow> units = samplesToImport.Select(o => o.UnitRow);
            if (saveUnit) SaveRows(ref units);
        }

        private void addUnit(ref SubSamplesRow s)
        {
            //attach unit
            bool attachUnit = EC.IsNuDelDetch(s.UnitRow);
            if (attachUnit)
            {
                UnitRow u = this.Unit.NewUnitRow();
                this.Unit.AddUnitRow(u);
                // LINAA.SubSamplesRow sample = s;
                u.SetParent(ref s);
            }
        }
    }
}