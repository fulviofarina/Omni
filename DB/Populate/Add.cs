using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        private ChannelsRow addChannel()
        {
            ChannelsRow v = Channels.NewChannelsRow() as ChannelsRow;
            Channels.AddChannelsRow(v);
            return v;
        }

        private void addCompositions(ref MatrixRow m, IList<string[]> listOfFormulasAndCompositions, bool reCode, ref IList<CompositionsRow> compos)
        {
            bool nulo = EC.CheckNull(this.tableMatrix.MatrixCompositionColumn, m);
            if (nulo)
            {
                compos = new List<CompositionsRow>();
                return;
            }
            if (!reCode || listOfFormulasAndCompositions == null)
            {
                listOfFormulasAndCompositions = RegEx.StripComposition(m.MatrixComposition);
                //to store matrix composition
            }

            // .. IList<CompositionsRow> compos = new List<CompositionsRow>();
            string fullComposition = string.Empty;
            //ilst of element and Quantity
            foreach (string[] formCompo in listOfFormulasAndCompositions)
            {
                try
                {
                    //decompose
                    string element = formCompo[0].Trim();
                    // double formulaweight = 0;
                    double quantity = Convert.ToDouble(formCompo[1].Trim());
                    // elementQuantity(formCompo, out element, out quantity, out formulaweight);
                    //CODE COMPOSITION
                    if (reCode)
                    {
                        fullComposition += "#" + element.Trim() + "   (" + quantity.ToString() + ")   ";
                        continue;
                    }

                    CompositionsRow c = addCompositions(element, quantity, ref m);
                    compos.Add(c);
                }
                catch (SystemException ex)
                {
                    AddException(ex);
                }
            }
            if (reCode) m.MatrixComposition = fullComposition;
        }

        private CompositionsRow addCompositions(string element, double quantity, ref MatrixRow m)
        {
            CompositionsRow c = m.FindComposition(element);
            if (c == null)
            {
                c = this.Compositions.NewCompositionsRow();
                Compositions.AddCompositionsRow(c);
            }
            else quantity += c.Quantity; //add new quantity

            c.SetValues(m.MatrixID, quantity, element);

            if (!EC.IsNuDelDetch(m.SubSamplesRow))
            {
                c.SubSampleID = m.SubSamplesRow.SubSamplesID;
            }

            return c;
        }

        private IEnumerable<MatrixRow> addCompositions(ref IEnumerable<MatrixRow> matrices)
        {
            if (matrices == null) matrices = this.Matrix.AsEnumerable();

            foreach (MatrixRow item in matrices)
            {
                IList<string[]> stripped = RegEx.StripComposition(item.MatrixComposition);
                LINAA.MatrixRow m = item;
                AddCompositions(ref m, stripped, true);
            }

            return matrices;
        }

        private IRequestsAveragesRow addIRequestsAverage(int NAAID)
        {
            IRequestsAveragesRow irs = this.tableIRequestsAverages.NewIRequestsAveragesRow();
            this.tableIRequestsAverages.AddIRequestsAveragesRow(irs);
            irs.NAAID = NAAID;
            return irs;
        }

        private IrradiationRequestsRow addIrradiation(string project)
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

        private MatrixRow addMatrix()
        {
            MatrixRow v = Matrix.NewMatrixRow() as MatrixRow;
            Matrix.AddMatrixRow(v);
            return v;
        }

        private LINAA.MeasurementsRow addMeasurement(string measName)
        {
            LINAA.MeasurementsRow meas = this.tableMeasurements.NewMeasurementsRow();
            this.tableMeasurements.AddMeasurementsRow(meas);

            try
            {
                meas.SetName(measName);
            }
            catch (SystemException ex)
            {
                EC.SetRowError(meas, ex);
            }
            return meas;
        }

        private SubSamplesRow addSamples(int? IrrReqID)
        {
            SubSamplesRow sample = this.tableSubSamples.NewSubSamplesRow();
            if (IrrReqID != null) sample.IrradiationRequestsID = (int)IrrReqID;
            // sample.SetIrradiationRequestID(IrrReqID);
            sample.SetCreationDate();
            this.tableSubSamples.AddSubSamplesRow(sample);
            return sample;
        }

        /// <summary>
        /// The actual function
        /// </summary>
        /// <param name="sampleName"></param>
        /// <returns></returns>
        private SubSamplesRow addSamples(string sampleName = "")
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

            if (saveSamp) Save(ref samplesToImport);

            addUnits(ref samplesToImport);
        }

        private SchAcqsRow addSchedule()
        {
            SchAcqsRow sch = this.SchAcqs.NewSchAcqsRow();
            this.SchAcqs.AddSchAcqsRow(sch);
            return sch;
        }

        private void addScheduleMeasurement(string project, string sample, short pos, string det, short repeats, double preset, DateTime startOn, string useremail, bool cummu, bool Force)
        {
            DB.LINAA.SchAcqsRow sch = this.FindASpecificSchedule(det, project, sample);
            DialogResult result;
            string Content = string.Empty;
            if (sch == null) sch = addSchedule();
            else
            {
                Content = sch.GetReportString();
                if (Force) result = DialogResult.No;
                else
                {
                    string msg = "Sample " + sample + " was found in the schedule:\n\n" + Content + CREATE_NEW;
                    result = MessageBox.Show(msg, MEASUREMENT_FOUND, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                }

                if (result == DialogResult.No) sch = addSchedule();
                else if (result == DialogResult.Cancel) return;
            }

            sch.SetSchedule(project, sample, pos, det, repeats, preset, startOn, useremail, cummu);
            sch.Reset();

            Content = sch.GetReportString();

            if (Force) result = DialogResult.OK;
            else result = MessageBox.Show(MEASUREMENT_ADDED + Content, CONFIRM, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel) sch.Delete();

            this.Save<LINAA.SchAcqsDataTable>();
        }

        private void addUnits(ref IEnumerable<SubSamplesRow> samplesToImport, bool saveUnit = true)
        {
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                SubSamplesRow sample = s;
                addUnits(ref sample);
            }
            IEnumerable<UnitRow> units = samplesToImport.Select(o => o.UnitRow);
            if (saveUnit) Save(ref units);
        }

        private void addUnits(ref SubSamplesRow s)
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

        private VialTypeRow addVial(bool aRabbit)
        {
            VialTypeRow v = VialType.NewVialTypeRow() as VialTypeRow;
            VialType.AddVialTypeRow(v);
            if (aRabbit) v.IsRabbit = true;
            else v.IsRabbit = false;
            return v;
        }

        private void cleanCompositions(ref IEnumerable<CompositionsRow> compos)
        {
            Delete(ref compos);
            this.Compositions.AcceptChanges();
            // return Save(ref compos);
        }

        private void deleteSigmaColumns()
        {
            DataColumn col = Sigmas.Columns["Element1"];
            Sigmas.Columns.Remove(col);
            col = Sigmas.Columns["Target1"];
            Sigmas.Columns.Remove(col);
            // col = Interface.IDB.Sigmas.Columns["Radioisotope1"]; Interface.IDB.Sigmas.Columns.Remove(col);
            col = Sigmas.Columns["ID1"];
            Sigmas.Columns.Remove(col);
        }
    }
}