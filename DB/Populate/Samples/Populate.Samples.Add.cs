using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rsx;

namespace DB
{



    /// <summary>
    /// THIS IS QUITE CLEAN
    /// </summary>
    public partial class LINAA : ISamples
    {
    

        public IRequestsAveragesRow AddIRequestsAverage(Int32 NAAID, ref SubSamplesRow s)
        {
            IRequestsAveragesRow irs = null;

            irs = addIRequestsAverage(NAAID);
            //irs.Radioisotope = iso;
            //	irs.Element = sym;
            if (!Rsx.EC.IsNuDelDetch(s))
            {
                irs.Sample = s.SubSampleName;
                // irs.Project = s.IrradiationCode;
            }
            return irs;
        }

        public SubSamplesRow AddSamples(string sampleName, int? IrrReqID = null)
        {
            SubSamplesRow sample = FindSample(sampleName);
            if (sample == null) sample = addSamples(IrrReqID);
            sample.SetName(sampleName);
            return sample;
        }

        //THIS CLASS IS QUITE CLEAN SOME THINGS TO DO
        public SubSamplesRow AddSamples(ref IrradiationRequestsRow ir, string sampleName = "")
        {
            string project = ir.IrradiationCode.Trim().ToUpper();
            return AddSamples(project, sampleName);
        }

        public SubSamplesRow AddSamples(string project, string sampleName = "")
        {
            IList<SubSamplesRow> list = new List<SubSamplesRow>();
            SubSamplesRow s = addSamples(sampleName);
            list.Add(s);
            IEnumerable<SubSamplesRow> samples = list;
            AddSamples(project, ref samples);
            return s;
        }

        public IEnumerable<LINAA.SubSamplesRow> AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> samplesToImport, bool save = true)
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

        public IEnumerable<LINAA.SubSamplesRow> AddSamples(ref IEnumerable<string> hsamples, string project, bool salve = true)
        {
            IList<LINAA.SubSamplesRow> ls = new List<SubSamplesRow>();
            foreach (string sname in hsamples)
            {
                SubSamplesRow s = addSamples(sname);
                ls.Add(s);
            }
            IEnumerable<SubSamplesRow> samps = ls;
            return AddSamples(project, ref samps, salve);
        }
    }
}
