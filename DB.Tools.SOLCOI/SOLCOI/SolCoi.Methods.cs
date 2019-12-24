using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Rsx.Dumb;

namespace DB.Tools
{
    public partial class SolCoin : IDisposable
    {
        public bool DoAll(bool Hide)
        {
            finished = false;
            exception = null;

            SentAt = DateTime.Now;

            GenerateDetectorCurves();

            GenerateSolidAngles(Hide);

            GenerateCOIS(Hide);

            try
            {
                if (storeSolidAngles) this.ToDBSolidAngles();
                if (storeCOIS) this.ToDBCOIS();
            }
            catch (SystemException ex)
            {
                exception = ex;
                this.linaa.AddException(ex);
            }

            // Merge();

            finished = true;

            return finished;
        }

        public void Merge()
        {
            try
            {
                this.Set.Solang.Merge(this.linaa.Solang, false, MissingSchemaAction.Error);
                this.Set.COIN.Merge(this.linaa.COIN, false, MissingSchemaAction.Error);

                this.Set.DetectorsCurves.Merge(this.linaa.DetectorsCurves, false, MissingSchemaAction.Error);
                this.Set.Exceptions.Merge(this.linaa.Exceptions, false, MissingSchemaAction.Error);
            }
            catch (SystemException ex)
            {
                exception = ex;
                this.Set.AddException(ex);
            }
        }

        internal void GenerateCOIS(bool Hide)
        {
            try
            {
                if (CalculateCOIS) this.COINProcessTime += this.RunCOIN(Hide);
                this.linaa.COIN.Clear();
                this.ReadCOIs();
            }
            catch (SystemException ex)
            {
                exception = ex;
                this.linaa.AddException(ex);
            }
        }

        internal void GenerateDetectorCurves()
        {
            try
            {
                if (Geometry.GeometryName.ToUpper().CompareTo(Reference.GeometryName.ToUpper()) == 0)
                {
                    this.ReadDetectorCurve("EFF");
                    this.ToDBDetectorCurves();
                }
            }
            catch (SystemException ex)
            {
                exception = ex;
                this.linaa.AddException(ex);
            }
        }

        internal bool GenerateInput(double[] Energy)
        {
            bool success = false;

            TextWriter writer = null;
            try
            {
                File.Delete(Files.DirectoryPath + Files.Input);
                writer = new System.IO.StreamWriter(Files.DirectoryPath + Files.Input, false);
            }
            catch (Exception ex)
            {
                this.exception = new SystemException(ex.Message, ex.InnerException);
            }

            if (writer != null)
            {
                #region detector, geometry, files

                int ne = Energy.Length; //number of energies
                int np = 1;
                if (Geometry.Position == 0) np = detectorHolder.Length; //number of positions
                String auxiliar = null;
                String comments = String.Empty;
                if (!geometry.IsCommentsNull()) comments = geometry.Comments;
                writer.WriteLine(comments);
                writer.WriteLine(Files.SolidAngles);
                writer.WriteLine(Files.COI);
                writer.WriteLine(Files.ReferenceEfficiency);
                writer.WriteLine(Files.ReferencePTT);
                writer.WriteLine(Files.ReferenceSolidAngles);
                writer.WriteLine("COI.V5");
                writer.WriteLine(DetectorName);
                writer.WriteLine(Geometry.GeometryName);
                writer.WriteLine(" " + String.Format("{0:0.00000000000000E+0}", geometry.MatrixRow.MatrixDensity));
                writer.WriteLine(" " + String.Format("{0:0.00000000000000E+0}", geometry.Radius * geometry.Radius * Math.PI * geometry.FillHeight * 1e-3));
                writer.WriteLine(" " + String.Format("{0:0.00000000000000E+0}", detectorDimension.PulseShaping));
                writer.WriteLine(geometry.MatrixRow.MatrixName); //replace if possible for matrix composition
                writer.WriteLine(detectorDimension.DetectorType);

                #endregion detector, geometry, files

                #region Dimentions

                auxiliar = " " + String.Format("{0:0.00000}", (geometry.Radius * 0.1));
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (geometry.FillHeight * 0.1));
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (geometry.VialTypeRow.SideThickness * 0.1));
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", geometry.VialTypeRow.BottomThickness * 0.1) + ne + " " + np;

                writer.WriteLine(auxiliar);

                auxiliar = " " + String.Format("{0:0.00000}", (detectorDimension.HolderSupportThickness * 0.1));
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.CanTopThickness * 0.1));
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.TopDeadLayerThickness * 0.1));   //
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.OtherAbsorberThickness) * 0.1);
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.CrystalRadius - detectorDimension.TopDeadLayerThickness) * 0.1);
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.CrystalHeight - detectorDimension.TopDeadLayerThickness) * 0.1);
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.CoreCavityRadius * 0.1));
                auxiliar = auxiliar + " " + String.Format("{0:0.00000}", (detectorDimension.CoreCavityHeight * 0.1));
                writer.WriteLine(auxiliar);
                writer.WriteLine(" " + String.Format("{0:0.00000}", (detectorDimension.ContactLayerThickness * 0.1)));

                if (this.integrationMode == IntegrationModes.AsPointSource) writer.WriteLine(" " + ((int)this.integrationMode).ToString());
                else writer.WriteLine(((int)this.integrationMode).ToString());

                #endregion Dimentions

                ///second part

                #region Mues

                double mu1 = 0, mu2 = 0, mu3 = 0, mu4 = 0, mu5 = 0, mu6 = 0, mu7 = 0, mu8 = 0, mu9 = 0;
                double energy = 0;
                for (int j = 0; j < Energy.Length; j++)
                {
                    try
                    {
                        mu1 = mu2 = mu3 = mu4 = mu5 = mu6 = mu7 = mu8 = mu9 = 0;
                        energy = 0;
                        //get the Mues for rounded energies, otherwise it will not find them!

                        energy = Energy[j];
                        if (energy - Math.Floor(energy) > 0.5) energy = Math.Ceiling(Energy[j]);
                        else energy = Math.Floor(Energy[j]);

                        object a = null;

                        mu5 = (double)(this.linaa.QTA.GetMU(energy, 3));      //air!!!
                        if (!detectorAbsorber.IsCrystalMatrixIDNull() && detectorAbsorber.CrystalMatrixID != 0)
                        {
                            a = (this.linaa.QTA.GetMU(energy, detectorAbsorber.CrystalMatrixID));

                            mu1 = (double)a;
                        }
                        if (!geometry.IsMatrixIDNull() && geometry.MatrixID != 0)
                        {
                            a = (this.linaa.QTA.GetMU(energy, geometry.MatrixID));
                            mu2 = (double)a;
                        }
                        if (!geometry.VialTypeRow.IsMatrixIDNull() && geometry.VialTypeRow.MatrixID != 0) mu3 = (double)(this.linaa.QTA.GetMU(energy, geometry.VialTypeRow.MatrixID));
                        if (!detectorAbsorber.IsContactLayerMatrixIDNull() && detectorAbsorber.ContactLayerMatrixID != 0) mu4 = (double)(this.linaa.QTA.GetMU(energy, detectorAbsorber.ContactLayerMatrixID));
                        if (!detectorAbsorber.IsHolderSupportMatrixIDNull() && detectorAbsorber.HolderSupportMatrixID != 0) mu6 = (double)(this.linaa.QTA.GetMU(energy, detectorAbsorber.HolderSupportMatrixID));
                        if (!detectorAbsorber.IsCanTopMatrixIDNull() && detectorAbsorber.CanTopMatrixID != 0) mu7 = (double)(this.linaa.QTA.GetMU(energy, detectorAbsorber.CanTopMatrixID));
                        if (!detectorAbsorber.IsTopDeadLayerMatrixIDNull() && detectorAbsorber.TopDeadLayerMatrixID != 0) mu8 = (double)(this.linaa.QTA.GetMU(energy, detectorAbsorber.TopDeadLayerMatrixID));
                        if (!detectorAbsorber.IsOtherAbsorberMatrixIDNull() && detectorAbsorber.OtherAbsorberMatrixID != 0) mu9 = (double)(this.linaa.QTA.GetMU(energy, detectorAbsorber.OtherAbsorberMatrixID));

                        String towrite = null;
                        String any = null;

                        //print the original energy fromRow the LIST!!!!!
                        any = String.Format("{0:0.00}", Energy[j]);
                        if (any.Length == 5) towrite = "   " + any;
                        if (any.Length == 6) towrite = "  " + any;
                        if (any.Length == 7) towrite = " " + any;
                        towrite = towrite + String.Format("{0:0.000E+0}", mu1);
                        towrite = towrite + String.Format("{0:0.000E+0}", mu2);
                        towrite = towrite + String.Format("{0:0.000E+0}", mu3);   //vial!!
                        towrite = towrite + String.Format("{0:0.000E+0}", mu4);

                        writer.WriteLine(towrite);

                        towrite = " " + String.Format("{0:0.000000E+0}", mu5);        //   air
                        towrite = towrite + " " + String.Format("{0:0.000000E+0}", mu6);
                        towrite = towrite + " " + String.Format("{0:0.000000E+0}", mu7);
                        towrite = towrite + " " + String.Format("{0:0.000000E+0}", mu8);
                        towrite = towrite + " " + String.Format("{0:0.000000E+0}", mu9);

                        writer.WriteLine(towrite);
                    }
                    catch (SystemException ex)
                    {
                        this.exception = ex;
                        this.linaa.AddException(ex);
                    }
                }

                #endregion Mues

                //Holder information

                #region holder

                double[] Y = new double[detectorHolder.Length];
                double[] X = new double[detectorHolder.Length];
                String auxX = null;
                String auxY = null;

                for (int i = 0; i < detectorHolder.Length; i++)
                {
                    LINAA.HoldersRow h = (LINAA.HoldersRow)detectorHolder[i];

                    if (Geometry.Position == h.Position || Geometry.Position == 0)
                    {
                        Y[i] = h.DistanceToBaseSupport + geometry.VialTypeRow.Distance + detectorDimension.AirLayerCanToHolderSupport;
                        X[i] = Y[i] + detectorDimension.VacuumGap + detectorDimension.HolderSupportThickness + detectorDimension.CanTopThickness
                             + detectorDimension.TopDeadLayerThickness + detectorDimension.OtherAbsorberThickness + geometry.VialTypeRow.BottomThickness; //mejoro??

                        auxX = String.Format("{0:0.00000}", X[i] * 0.1);
                        auxY = String.Format("{0:0.00000}", Y[i] * 0.1);
                        if (auxX.Length == 7) auxX = " " + auxX;
                        if (auxY.Length == 7) auxY = " " + auxY;

                        writer.WriteLine(auxX + auxY);
                    }
                }

                writer.WriteLine(DateTime.Now);
                writer.WriteLine(String.Format("{0:0.00000000}", geometry.VialTypeRow.Distance * 0.001)); //distance fromRow vial toRow support in meters

                #endregion holder

                writer.Close();
                success = true;
            }
            else this.exception = new SystemException("Cannot create file writer! - Generate Input Module");

            return success;
        }

        internal void GenerateSolidAngles(bool Hide)
        {
            try
            {
                //if null put classic energies and clean SolidAnglesDataBase
                if (Energies == null) Energies = SolCoin.ClassicEnergies;
                this.linaa.Solang.Clear();
                //reduced list of 17 energies because SolCoi cannot handle more than that each time it runs
                HashSet<double> reduced = new HashSet<double>();
                foreach (double e in Energies)
                {
                    reduced.Add(e);
                    if (reduced.Count == 17)
                    {
                        this.GenerateInput(reduced.ToArray());
                        if (CalculateSolidAngles) this.SolangProcessTime += RunSolang(Hide);
                        this.ReadSolidAngles();
                        reduced.Clear();
                    }
                }
                if (reduced.Count != 0)    //if energy.count is not exactly divisble by 17
                {
                    this.GenerateInput(reduced.ToArray());
                    if (CalculateSolidAngles) this.SolangProcessTime += RunSolang(Hide);
                    this.ReadSolidAngles();
                    reduced.Clear();
                }
                reduced = null;
            }
            catch (SystemException ex)
            {
                exception = ex;
                this.linaa.AddException(ex);
            }
        }

        /// <summary>
        /// reads and puts the data in the COIN data table
        /// </summary>
        /// <returns></returns>
        internal bool ReadCOIs()
        {
            bool success = false;

            if (System.IO.File.Exists(Files.DirectoryPath + Files.COI))
            {
                string[] strArray = System.IO.File.ReadAllLines(Files.DirectoryPath + Files.COI);
                List<string> list = new List<string>();
                List<string> energy_cois = new List<string>();
                for (int i = 7; i < strArray.Length; i++)
                {
                    if (strArray[i].Length == 13)
                    {
                        list.Clear();
                        foreach (string str in strArray[i].Split(new char[] { ' ' }))
                        {
                            if (!str.Equals(string.Empty))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    else
                    {
                        if (list.Count != 0)
                        {
                            foreach (string str2 in strArray[i].Split(new char[] { ' ' }))
                            {
                                if (!str2.Equals(string.Empty) && !str2.Equals("keV"))
                                {
                                    energy_cois.Add(str2);
                                }
                            }

                            if (Geometry.Position == 0)
                            {
                                for (Int16 x = 1; x < energy_cois.Count; x++)
                                {
                                    this.linaa.COIN.AddCOINRow(DetectorName, Geometry.GeometryName, list[0], Convert.ToDouble(energy_cois[0]), x, Convert.ToDouble(energy_cois[x]), this.sentAt);
                                }
                            }
                            else this.linaa.COIN.AddCOINRow(DetectorName, Geometry.GeometryName, list[0], Convert.ToDouble(energy_cois[0]), Geometry.Position, Convert.ToDouble(energy_cois[1]), this.sentAt);

                            energy_cois.Clear();
                        }
                    }
                }
            }

            if (this.linaa.COIN.Rows.Count != 0) success = true;

            return success;
        }

        internal void ReadDetectorCurve(string CurveTypeCode)
        {
            if (File.Exists(Files.DirectoryPath + Files.ReferenceEfficiency))
            {
                this.linaa.DetectorsCurves.Clear();

                string[] strArray = System.IO.File.ReadAllLines(Files.DirectoryPath + Files.ReferenceEfficiency);
                List<double> list = new List<double>();
                foreach (string str in strArray[1].Split(new char[] { ' ' }))
                {
                    if (!str.Equals(string.Empty))
                    {
                        list.Add(Convert.ToDouble(str));
                    }
                }
                int num = Convert.ToInt16(strArray[0].Trim()) + 1;
                List<int> list2 = new List<int>();
                foreach (string str2 in strArray[2].Split(new char[] { ' ' }))
                {
                    if (!str2.Equals(string.Empty))
                    {
                        list2.Add(Convert.ToInt16(str2));
                    }
                }
                int num2 = 0;
                int index = 0;
                List<double>[] listArray = new List<double>[num - 1];
                for (index = 0; index < (num - 1); index++)
                {
                    listArray[index] = new List<double>();
                }
                index = 0;
                for (int i = 3; i < strArray.Length; i++)
                {
                    foreach (string str3 in strArray[i].Split(new char[] { ' ' }))
                    {
                        if (!str3.Equals(string.Empty))
                        {
                            listArray[index].Add(Convert.ToDouble(str3));
                            if (num2 == list2[index])
                            {
                                double[] array = new double[5];
                                listArray[index].CopyTo(array, 0);
                                LINAA.DetectorsCurvesRow c = this.linaa.DetectorsCurves.NewDetectorsCurvesRow();

                                c.Detector = DetectorName;
                                c.Curve = CurveTypeCode;
                                c.LowEnergy = list[index];
                                c.HighEnergy = list[index + 1];
                                c.Degree = list2[index];
                                c.a0 = array[0];
                                c.a1 = array[1];
                                c.a2 = array[2];
                                c.a3 = array[3];
                                c.a4 = array[4];
                                c.RefGeometry = Reference.GeometryName;
                                c.RefPosition = Reference.Position;
                                this.linaa.DetectorsCurves.AddDetectorsCurvesRow(c);
                                num2 = 0;
                                index++;
                            }
                            else
                            {
                                num2++;
                            }
                        }
                    }
                }
            }
        }

        internal bool ReadSolidAngles()
        {
            bool success = false;

            if (System.IO.File.Exists(Files.DirectoryPath + Files.SolidAngles))
            {
                string[] strArray = System.IO.File.ReadAllLines(Files.DirectoryPath + Files.SolidAngles);
                string[] strArray2 = strArray[0].Split(new char[] { ' ' });
                int index = 0;
                int[] numArray = new int[2];
                foreach (string str in strArray2)
                {
                    if (!str.Equals(string.Empty))
                    {
                        numArray[index] = Convert.ToInt16(str);
                        index++;
                        if (index == 2)
                        {
                            break;
                        }
                    }
                }
                List<double>[] listArray = new List<double>[numArray[0] + 1];
                for (index = 0; index < (numArray[0] + 1); index++)
                {
                    listArray[index] = new List<double>();
                }
                index = 0;
                int num2 = 0;
                for (int i = 1; i < strArray.Length; i++)
                {
                    foreach (string str2 in strArray[i].Split(new char[] { ' ' }))
                    {
                        if (!str2.Equals(String.Empty))
                        {
                            listArray[index].Add(Convert.ToDouble(str2));
                            num2++;
                        }
                        if (num2 == numArray[1])
                        {
                            index++;
                            num2 = 0;
                        }
                    }
                }

                int toprintPosition = 0;
                for (num2 = 0; num2 < numArray[0]; num2++)
                {
                    for (index = 0; index < numArray[1]; index++)
                    {
                        if (Geometry.Position == 0) toprintPosition = num2 + 1; // value 0 performs automatic positioning, else inserts AsPosition as the position
                        else toprintPosition = Geometry.Position;
                        this.linaa.Solang.AddSolangRow(DetectorName, Geometry.GeometryName, Convert.ToDouble(listArray[0][index]), Convert.ToDouble(listArray[num2 + 1][index]), (Int16)toprintPosition, this.SentAt);
                    }
                }
            }

            if (this.linaa.Solang.Rows.Count != 0) success = true;

            return success;
        }

        internal double RunCOIN(bool Hide)
        {
            double time = 0.0;
            string arguments = " " + Files.Input;

            File.Delete(Files.DirectoryPath + Files.COI);

            if (cOINProcess != null)
            {
                if (!cOINProcess.HasExited)
                {
                    cOINProcess.Kill();
                    cOINProcess.Close();
                }
                cOINProcess.Dispose();
                cOINProcess = null;
            }

            cOINProcess = new Process();
            time += IO.Process(cOINProcess, Files.DirectoryPath, "solcoina.exe", arguments, Hide, true, 1000000);
            time += IO.Process(cOINProcess, Files.DirectoryPath, "solcoinb.exe", arguments, Hide, true, 1000000);
            time += IO.Process(cOINProcess, Files.DirectoryPath, "solcoinc.exe", arguments, Hide, true, 1000000);
            time += IO.Process(cOINProcess, Files.DirectoryPath, "solcoind.exe", arguments, Hide, true, 1000000);
            time += IO.Process(cOINProcess, Files.DirectoryPath, "solcoine.exe", arguments, Hide, true, 1000000);

            return time;
        }

        internal double RunSolang(bool Hide)
        {
            double time = 0.0;
            if (solangProcess != null)
            {
                if (!solangProcess.HasExited)
                {
                    solangProcess.Kill();
                    solangProcess.Close();
                }
                solangProcess.Dispose();
                solangProcess = null;
            }
            solangProcess = new Process();

            string file = "solcois.exe";

            // IList<Process> pros = System.Diagnostics.Process.GetProcesses().Where(p => p.ProcessName.Contains(file)).ToList();

            string arguments = " " + Files.Input;
            File.Delete(Files.DirectoryPath + Files.SolidAngles);
            time = IO.Process(solangProcess, Files.DirectoryPath, file, arguments, Hide, true, 1000000);
            return time;
        }

        internal bool ToDBCOIS()
        {
            bool success = false;

            if (this.linaa.COIN.Rows.Count != 0)
            {
                DB.LINAATableAdapters.COINTableAdapter cointa = this.linaa.TAM.COINTableAdapter;

                try
                {
                    if (cleanEffiCOIS)
                    {
                        if (Geometry.Position == 0) cointa.DeleteByDetectorGeometry(Geometry.GeometryName, DetectorName);
                        else cointa.DeleteByDetectorGeometryPosition(Geometry.GeometryName, DetectorName, Geometry.Position);
                    }

                    foreach (LINAA.COINRow c in this.linaa.COIN)
                    {
                        if (!cleanEffiCOIS)
                        {
                            int t = cointa.UpdateCOI(c.COI, this.sentAt, c.Detector, c.Geometry, c.Isotope, c.Energy, c.Position);
                            if (t == 0) cointa.Insert(c.Detector, c.Geometry, c.Isotope, c.Energy, c.Position, c.COI, this.sentAt);
                        }
                        else cointa.Insert(c.Detector, c.Geometry, c.Isotope, c.Energy, c.Position, c.COI, this.sentAt);
                    }

                    // if (Geometry.Position == 0) cointa.FillByDetectorGeometry(this.linaa.COIN,
                    // DetectorName, Geometry.GeometryName); else
                    // cointa.FillByDetectorGeometryPosition(this.linaa.COIN, DetectorName,
                    // Geometry.GeometryName, Geometry.Position);
                }
                catch (SystemException ex)
                {
                    this.linaa.AddException(ex);
                }
            }

            if (this.linaa.COIN.Rows.Count != 0)
            {
                this.linaa.COIN.WriteXml(this.files.DirectoryPath + files.COI + ".xml");
                success = true;
            }

            return success;
        }

        internal bool ToDBDetectorCurves()
        {
            bool success = false;

            if (this.linaa.DetectorsCurves.Rows.Count != 0)
            {
                DB.LINAATableAdapters.DetectorsCurvesTableAdapter detcurta = this.linaa.TAM.DetectorsCurvesTableAdapter;

                try
                {
                    detcurta.DeleteByDetectorGeometryPosition(DetectorName, Reference.GeometryName, Reference.Position);
                    foreach (LINAA.DetectorsCurvesRow row in this.linaa.DetectorsCurves)
                    {
                        detcurta.Insert(row.Detector, row.Curve, row.LowEnergy, row.HighEnergy, row.Degree, row.a0, row.a1, row.a2, row.a3, row.a4, row.RefGeometry, row.RefPosition);
                    }
                    success = true;
                }
                catch (SystemException ex)
                {
                    this.linaa.AddException(ex);
                }
            }

            return success;
        }

        internal bool ToDBSolidAngles()
        {
            bool success = false;

            if (this.linaa.Solang.Rows.Count != 0)
            {
                DB.LINAATableAdapters.SolangTableAdapter solta = this.linaa.TAM.SolangTableAdapter;

                int records = 0;
                try
                {
                    if (CleanSolidAngles)
                    {
                        if (Geometry.Position == 0)
                        {
                            records += solta.DeleteByDetectorGeometry(DetectorName, Geometry.GeometryName);
                        }
                        else
                        {
                            records += solta.DeleteByDetectorGeometryPosition(DetectorName, Geometry.GeometryName, Geometry.Position);
                        }
                    }
                    int t = 0;
                    foreach (LINAA.SolangRow row in this.linaa.Solang)
                    {
                        t = 0;
                        if (!CleanSolidAngles)
                        {
                            t = solta.UpdateSolang(row.Detector, row.Geometry, row.Energy, row.Solang, row.Position, this.SentAt);
                        }
                        if (t == 0) solta.Insert(row.Detector, row.Geometry, row.Energy, row.Solang, row.Position, this.SentAt);
                    }
                    // solta.FillByGeometryDetector(this.linaa.Solang, Geometry.GeometryName, Geometry.Detector);
                }
                catch (SystemException ex)
                {
                    this.linaa.AddException(ex);
                }
            }
            if (this.linaa.Solang.Rows.Count != 0)
            {
                this.linaa.Solang.WriteXml(this.files.DirectoryPath + files.SolidAngles + ".xml");
                success = true;
            }

            return success;
        }
    }
}