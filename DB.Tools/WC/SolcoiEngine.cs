using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Rsx;

namespace DB.Tools
{
    /// <summary>
    /// PRIVATES
    /// </summary>
    public partial class WC
    {
        /// <summary>
        /// Calculates Solid Angles
        /// </summary>
        private void CalculateSolidAngles(ref System.Collections.Hashtable solcoinTable, ref System.Collections.Hashtable solcoinRefTable)
        {
            IterateSolang(ref solcoinRefTable);

            while (progress.Maximum != progress.Value)
            {
                if (cancel.Checked) return;
                Application.DoEvents();
            }

            IterateSolang(ref solcoinTable);
        }

        private void IterateSolang(ref System.Collections.Hashtable solcoinTable)
        {
            int goSlow = 0;

            foreach (System.Collections.DictionaryEntry sol in solcoinTable)
            {
                String key = (String)sol.Key;
                string[] PosGeoDetFillRad = key.Split(',');
                double[] energies = ((System.Collections.Generic.ICollection<double>)sol.Value).ToArray();
                progress.Maximum++;
                UnitSolang(PosGeoDetFillRad, energies, true, false);

                goSlow++;

                if (goSlow == 3)
                {
                    while (progress.Maximum != progress.Value)
                    {
                        if (cancel.Checked) return;
                        Application.DoEvents();
                    }
                    goSlow = 0;
                }
            }
        }

        /// <summary>
        /// Calculates COI factos
        /// </summary>
        private void CalculateCOIS(ref System.Collections.Hashtable solcoinTable, ref System.Collections.Hashtable solcoinRefTable)
        {
            int goSlow = 0;

            foreach (System.Collections.DictionaryEntry sol in solcoinRefTable)
            {
                if (cancel.Checked) return;

                String key = (String)sol.Key;
                string[] PosGeoDetFillRad = key.Split(',');
                double[] energies = SolCoin.ClassicEnergies;
                progress.Maximum++;
                UnitSolang(PosGeoDetFillRad, energies, true, false);
                goSlow++;

                if (goSlow == 3)
                {
                    while (progress.Maximum != progress.Value) Application.DoEvents();
                    goSlow = 0;
                }
            }

            while (progress.Maximum != progress.Value) Application.DoEvents();
            goSlow = 0;

            foreach (System.Collections.DictionaryEntry sol in solcoinTable)
            {
                if (cancel.Checked) return;

                String key = (String)sol.Key;
                string[] PosGeoDetFillRad = key.Split(',');
                double[] energies = SolCoin.ClassicEnergies;
                progress.Maximum++;
                UnitSolang(PosGeoDetFillRad, energies, true, true);

                goSlow++;

                if (goSlow == 3)
                {
                    while (progress.Maximum != progress.Value) Application.DoEvents();
                    goSlow = 0;
                }
            }

            while (progress.Maximum != progress.Value) Application.DoEvents();
            goSlow = 0;
        }

        /// <summary>
        /// Prepares hashtables with geometries to calculate
        /// </summary>
        private System.Collections.Hashtable[] PrepareSolang(bool DoSolang, ref IEnumerable<LINAA.SubSamplesRow> samples)
        {
            System.Collections.Hashtable[] array = null;
            System.Collections.Hashtable solcoin_table = new System.Collections.Hashtable();
            System.Collections.Hashtable solcoin_tableRef = new System.Collections.Hashtable();
            array = new System.Collections.Hashtable[] { solcoin_tableRef, solcoin_table };

            string energyCol = Linaa.Peaks.EnergyColumn.ColumnName;

            foreach (LINAA.SubSamplesRow s in samples)
            {
                IEnumerable<LINAA.MeasurementsRow> measurements = s.GetMeasurementsRows();

                foreach (LINAA.MeasurementsRow iMeas in measurements)
                {
                    if (cancel.Checked) return array;

                    if (!iMeas.Selected) continue;
                    else if (iMeas.NeedsPeaks) continue;
                    else if (!iMeas.NeedsSolang && !DoSolang) continue;

                    IEnumerable<LINAA.PeaksRow> peaks = LINAA.GetPeaksInNeedOf(DoSolang, false, iMeas);
                    if (peaks.Count() == 0) continue;

                    System.Collections.Generic.ICollection<double> hs = Dumb.HashFrom<double>(peaks, energyCol);
                    String keyRef = 5 + "," + "REF" + "," + iMeas.Detector;
                    //add reference
                    if (solcoin_tableRef.ContainsKey(keyRef))	//reference already added, then update energies to include new ones
                    {
                        System.Collections.Generic.ICollection<double> hsref = (System.Collections.Generic.ICollection<double>)solcoin_tableRef[keyRef];
                        hsref = hsref.Union(hs).ToList();
                        solcoin_tableRef[keyRef] = hsref;
                    }
                    else solcoin_tableRef.Add(keyRef, hs);

                    String key = iMeas.Position + "," + s.GeometryName + "," + iMeas.Detector + "," + s.FillHeight.ToString() + "," + s.Radius.ToString() + "," + s.SubSampleName;
                    //ad geometry
                    //add new position, geometry, detector key value with its set of energies
                    if (solcoin_table.ContainsKey(key))	//reference already added, then update energies to include new ones
                    {
                        System.Collections.Generic.ICollection<double> hs2 = (System.Collections.Generic.ICollection<double>)solcoin_table[key];
                        hs2 = hs2.Union(hs).ToList();
                        solcoin_table[key] = hs2;
                    }
                    else solcoin_table.Add(key, hs);
                }
            }

            return array;
        }

        private void UnitSolang(string[] PosGeoDetFillRad, double[] energies, bool calcSolid, bool calcCois)
        {
            SolCoin Solcoin = new SolCoin(ref Linaa);
            object[] arg = new object[6];
            arg[0] = Solcoin;
            arg[2] = PosGeoDetFillRad;
            arg[3] = energies;
            arg[1] = calcSolid;
            arg[4] = calcCois;
            arg[5] = !showSolang;

            Solcoin.IntegrationMode = mode;

            //CREATE A WORKER FOR THIS TASK
            System.ComponentModel.BackgroundWorker solcoinWorker = new System.ComponentModel.BackgroundWorker();
            solcoinWorker.WorkerReportsProgress = true;
            solcoinWorker.WorkerSupportsCancellation = true;
            solcoinWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(ProgressChanged);
            solcoinWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(solcoinWorker_DoWork);
            Solcoin.Tag = solcoinWorker;

            AddWorker(ref solcoinWorker);

            solcoinWorker.RunWorkerAsync(arg);
        }
    }
}