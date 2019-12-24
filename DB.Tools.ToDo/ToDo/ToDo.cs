using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx.DGV;
using Rsx.Dumb;
using Rsx.Math;

namespace DB.Tools
{
    public partial class ToDo : DB.Tools.IToDo
    {
        public delegate void ToDoCalculator<T>(ref T o, ref ToDoArgs args);

        public IFitted Fit
        {
            get { return fit; }
        }

        public IEnumerable<LINAA.ToDoAvgRow> IAvgs
        {
            get { return iAvgs; }
            set { iAvgs = value; }
        }

        public IEnumerable<LINAA.ToDoRow> IList
        {
            get { return ToDoes; }
            set
            {
                ToDoes = value;
                ToDoes = ToDoes.ToList(); //esta funcion deberían legalizarla!!! es muy sencilla
            }
        }

        public short MinPosition
        {
            get
            {
                return minPosition;
            }
            set { minPosition = value; }
        }

        /// <summary>
        /// Ultimate Function, so it is better to call this one
        /// </summary>
        /// <param name="meas1">     </param>
        /// <param name="meas2">     </param>
        /// <param name="todoNr">    </param>
        /// <param name="SameDetPos"></param>
        /// <param name="SameNAA">   </param>
        public IEnumerable<LINAA.ToDoResRow> AddToDoResRow(int todoNr, bool Alike, bool SameDetPos, short minpos, ref IEnumerable<LINAA.IPeakAveragesRow> monS, ref IEnumerable<LINAA.IPeakAveragesRow> refeS)
        {
            IEnumerable<LINAA.PeaksRow> Peaks1 = monS.SelectMany(o => o.GetPeaksRows()).ToList();
            IEnumerable<LINAA.PeaksRow> Peaks2 = refeS.SelectMany(o => o.GetPeaksRows()).ToList();

            IList<LINAA.PeaksRow[]> list = null;
            LINAA.Comparer<LINAA.PeaksRow> compa2 = null;
            if (Alike) compa2 = LINAA.Comparers.Alike;
            else compa2 = LINAA.Comparers.NotLike;
            list = LINAA.Intersect(ref Peaks1, ref Peaks2, compa2);

            LINAA.Comparer<LINAA.MeasurementsRow> meascompa = null;
            if (SameDetPos) meascompa = LINAA.Comparers.Alike;
            else meascompa = LINAA.Comparers.NotLike;
            //resources...
            HashSet<string> hashy = new HashSet<string>();
            //release

            HashSet<string> hs = new HashSet<string>();

            Func<LINAA.PeaksRow[], bool> filter = arr2 =>
            {
                LINAA.PeaksRow p = arr2[0];
                LINAA.PeaksRow p2 = arr2[1];
                if (p.MeasurementsRow == null) return false;
                if (p2.MeasurementsRow == null) return false;
                if (!meascompa(p.MeasurementsRow, p2.MeasurementsRow)) return false;
                if (hs.Add(p.MeasurementID + "," + p.PeaksID + "," + p2.MeasurementID + "," + p2.PeaksID)) return true;
                else return false;
            };

            list = list.Where(filter).ToList();

            hs.Clear();
            hs = null;

            Func<LINAA.ToDoResRow, bool> filter2 = res =>
            {
                string key = res.sample1 + "," + res.sample2 + "," + Math.Abs(res.NAA1ID) + "," + Math.Abs(res.NAA2ID) + ",";
                if (res.ToDoNr != 0)
                {
                    key += res.ToDoNr + ",";
                }

                key += res.DP;
                key = key.Trim();
                return hashy.Add(key);
            };

            IList<LINAA.ToDoResRow> added = null;
            added = new List<LINAA.ToDoResRow>();

            //selector for Res...
            Func<LINAA.PeaksRow[], bool> selector = arr2 =>
            {
                LINAA.PeaksRow p = arr2[0];
                LINAA.PeaksRow p2 = arr2[1];
                LINAA.ToDoResRow res = null;
                if (!Alike)
                {
                    if (p.MeasurementsRow.Position <= minpos) return false;
                    if (p2.MeasurementsRow.Position <= minpos) return false;
                }
                res = this.Linaa.ToDoRes.AddToDoResRow(ref p, ref p2, todoNr);

                bool toAdd = filter2(res);
                if (toAdd) added.Add(res);
                return toAdd;
            };

            //adds the data rows and
            //returns an array ready to make the AVG table (non-repeated)
            list = list.Where(selector).ToList();

            hashy.Clear();
            hashy = null;

            list.Clear();
            list = null;

            filter2 = null;
            selector = null;
            filter = null;

            return added;
        }

        public decimal[] Average(string field, string fieldSD, string filterCol, ref IEnumerable<DataRow> rows)
        {
            decimal[] averages = new decimal[4];

            for (int i = 0; i < averages.Length; i++) averages[i] = 0;
            try
            {
                string filter = filterCol;

                IList<double> doubles = MyMath.ListFrom(rows.ToList(), field, 1, filter, true);
                if (doubles.Count != 0)
                {
                    double average = doubles.Average();
                    if (average != 0.0)
                    {
                        double sd = (MyMath.StDev(doubles, average) * 100.0) / average;
                        averages[0] = decimal.Round(Convert.ToDecimal(average), 3);
                        averages[1] = decimal.Round(Convert.ToDecimal(sd), 1);
                        //how toRow weight accordingly??? modify this
                        double[] weightedAvgAndObservedSD = MyMath.WAverageStDeV(rows, field, fieldSD, filter);
                        averages[2] = decimal.Round(Convert.ToDecimal(weightedAvgAndObservedSD[0]), 3);
                        averages[3] = decimal.Round(Convert.ToDecimal(weightedAvgAndObservedSD[1]), 1);
                    }
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            return averages;
        }

        public string CalculateAi()
        {
            DateTime start = DateTime.Now;

            ToDoArgs X = new ToDoArgs(tocalculate, minPosition, optimize);

            X.SetDelegates();

            //IMPORTANT NOTE
            //
            //useRefe and Optimize are only used in the functions where it was included!!!
            // that is, those functions where true or false matters!!!

            IList<LINAA.ToDoAvgRow> rows = this.iAvgs.OfType<LINAA.ToDoAvgRow>().ToList();
            string result = "Calculate Ai Error: Source Collection is empty";
            if (rows.Count() == 0) return result;

            IList<LINAA.ToDoAvgRow> refes = rows.Where(r => r.ToDoRow._ref).ToList();

            try
            {
                //calculate refes first!
                //for overriding f and alpha with reference when Qo determination
                if (useRef) //always enters when in k0!!! so profit from that to not calculated todoAvgs for these...
                {
                    int i = 0;
                    for (i = refes.Count() - 1; i >= 0; i--)
                    {
                        LINAA.ToDoAvgRow r = refes.ElementAt(i);
                        CalculateAi(ref r, ref X, f0, alpha0);
                        rows.Remove(r);  //rows, not refes!!! take care
                    }
                    //clean the ones with errors first
                    refes = refes.Where(t => !t.HasErrors).ToList();
                    string fcol = this.Linaa.ToDoAvg.fColumn.ColumnName;
                    string sdCol = this.Linaa.ToDoAvg.SDColumn.ColumnName;
                    double[] WfSD = MyMath.WAverageStDeV(refes, fcol, sdCol);
                    f0 = WfSD[0]; //find f from refes!

                    // refes = null;
                }

                X.References = refes.ToList();  //for k0

                //calculate te others..
                foreach (LINAA.ToDoAvgRow avg in rows)
                {
                    LINAA.ToDoAvgRow aux = avg;
                    CalculateAi(ref aux, ref X, f0, alpha0);
                }

                result = "Calculate Ai was OK";
            }
            catch (SystemException ex)
            {
                result = ex.Message;
            }

            int secs = Convert.ToInt32((DateTime.Now - start).TotalSeconds);

            Interface.IReport.Msg(tocalculate.ToString(), "Calculated Ai in " + secs + " seconds");

            return result;
        }

        public string CalculateFC()
        {
            DateTime start = DateTime.Now;

            bool k0 = tocalculate.Equals(LINAA.ToDoType.k0determination);
            bool Q0 = tocalculate.Equals(LINAA.ToDoType.Q0determination);

            string result = string.Empty;

            IEnumerable<LINAA.ToDoDataRow> raws = this.Linaa.ToDoData;
            IEnumerable<LINAA.ToDoAvgRow> avgsR = this.Linaa.ToDoAvg;
            CalculateFCs(tocalculate, ref raws);
            CalculateFCs(tocalculate, ref avgsR);

            if (!Q0 && !k0)
            {
                double[] avgs = GetAvgsfAlpha(tocalculate, this.Linaa.ToDoAvg);
                GetUncfAlpha(tocalculate, avgs, this.Linaa.ToDoAvgUnc);
            }

            PerformFitOthers();
            if (!Q0 && !k0) PerformFitfAlpha();

            GetResiduals(ref avgsR);

            // Push(ref rows); //not for now...

            int secs = Convert.ToInt32((DateTime.Now - start).TotalSeconds);

            Interface.IReport.Msg(tocalculate.ToString(), "Calculated FC in " + secs + " seconds");

            return result;
        }

        public string CreateSelectReject(ref DataGridView dgv, ref BindingSource bs)
        {
            string result = string.Empty;

            //in this order...
            sRBS = bs;
            sRDGV = dgv;

            //this after setting the dgv
            Handlers(false);

            try
            {
                DataView view2 = this.sRBS.List as DataView;
                //take DataView
                LINAA.ToDoResAvgDataTable dt = this.Linaa.ToDoResAvg;

                int arrCount = dt.SRColumnNames.Count();
                //make select/reject
                xTable.New(ref view2, dt.DPColumn.ColumnName, dt.DPColumn.ColumnName, dt.SRColumnNames, ref sRDGV, 100);
                //fill with Xij
                xTable.Fill_Xij(ref sRDGV, arrCount, dt.XColumn.ColumnName, dt.SDColumn.ColumnName, 2);
                IList<DataGridViewColumn> cols = sRDGV.Columns.OfType<DataGridViewColumn>().Where(c => c.Index >= arrCount).ToList();
                foreach (DataGridViewColumn col in cols) col.DefaultCellStyle.Format = "N2";
                cols = null;

                int falseVal = Convert.ToInt16(false);
                int trueVal = Convert.ToInt16(true);
                //paint selected/rejected
                xTable.Paint.BySwitch(ref sRDGV, dt.useColumn.ColumnName, falseVal, System.Drawing.Color.Red, trueVal, System.Drawing.Color.Black, arrCount);
            }
            catch (SystemException ex)
            {
                result = ex.Message + "\n\n" + ex.StackTrace;
                this.Linaa.AddException(ex);
            }

            Handlers(true);

            return result;
        }

        public string Prepare(short minposition)
        {
            try
            {
                if (ToDoes.Count() == 0) return "Nothing found in the ToDo List";
                ToDoes = ToDoes.OrderBy(o => !o._ref).ToList(); //make static list

                minPosition = minposition;

                bool k0 = (tocalculate == LINAA.ToDoType.k0determination);
                bool Bare = (tocalculate == LINAA.ToDoType.fAlphaBare);
                bool CdCover = tocalculate == LINAA.ToDoType.fAlphaCdCovered;

                IEnumerable<LINAA.IPeakAveragesRow> refeS = null;

                LINAA.ToDoRow Reference = null;
                if (Bare || k0)
                {
                    IEnumerable<LINAA.ToDoRow> possibleRefes = ToDoes.Where(o => o._ref).ToList();
                    int cnt = possibleRefes.Count();
                    if (cnt == 0) return "Please select a Reference Isotope!";
                    else if (Bare && cnt > 1) return "Please select only 1 Reference Isotope!";

                    if (k0)
                    {
                        foreach (LINAA.ToDoRow r in possibleRefes)
                        {
                            IEnumerable<LINAA.IPeakAveragesRow> a = null;
                            r.Checkk0(ref a);
                            if (refeS != null) refeS = refeS.Union(a).ToList();
                            else refeS = a;
                        }
                    }
                    else
                    {
                        Reference = possibleRefes.First();
                        Reference.CheckBare(ref refeS);
                    }
                    possibleRefes = null;
                }

                //start cleaning now
                this.Linaa.ToDoData.Clear();
                this.Linaa.ToDoData.AcceptChanges();

                if (!k0)
                {
                    this.Linaa.ToDoAvg.Clear();
                    this.Linaa.ToDoAvg.AcceptChanges();
                    this.Linaa.ToDoAvgUnc.Clear();
                    this.Linaa.ToDoAvgUnc.AcceptChanges();

                    //because Im supposed to do Q0 determination first
                    //so this table has already the new Qo values! And I need to reuse it for k0-determination
                }

                this.Linaa.ToDoRes.Clear();
                this.Linaa.ToDoRes.AcceptChanges(); // SOOO IMPORTANT!
                this.Linaa.ToDoResAvg.Clear();
                this.Linaa.ToDoResAvg.AcceptChanges();

                HashSet<string> hsToDoesFork0 = new HashSet<string>();

                //start iteration
                foreach (LINAA.ToDoRow todo in ToDoes)
                {
                    try
                    {
                        LINAA.ToDoRow to = todo;

                        if (to.Is_refNull()) to._ref = false; //put false as minimum value
                                                              //clean errors
                        to.RowError = string.Empty; //clean error

                        //avoid repetiition of TODOES Isotope 1 to find k0s...
                        if (k0 && hsToDoesFork0.Contains(to.sample)) continue;

                        //start checking
                        IEnumerable<LINAA.IPeakAveragesRow> monS = null;
                        if (Bare || k0)
                        {
                            if (to._ref) continue;
                            else
                            {
                                if (!k0) todo.CheckBare(ref monS);
                                else todo.Checkk0(ref monS);
                            }
                        }
                        else if (tocalculate == LINAA.ToDoType.fAlphaCdCovered) todo.CheckCdCovered(ref refeS, ref monS);
                        // any other method different than fAlpha Bare needs to take the reFes from Isotope 2
                        //cdRatio or Qo determination
                        else todo.CheckCdRatioOrQ0(tocalculate, ref refeS, ref monS);

                        if (!todo.RowError.Equals(string.Empty)) continue;  //didn't pass the check!

                        ToDoArgs nu = ToDoArgs.Empty;

                        LINAA.Comparer<LINAA.IPeakAveragesRow> ipcompa = null;
                        if (Bare || k0) ipcompa = LINAA.Comparers.NotLike;
                        else ipcompa = LINAA.Comparers.Alike;

                        IList<LINAA.IPeakAveragesRow[]> list = LINAA.Intersect(ref monS, ref refeS, ipcompa);
                        foreach (LINAA.IPeakAveragesRow[] o in list)
                        {
                            LINAA.IPeakAveragesRow refe = o[1];
                            LINAA.IPeakAveragesRow mon = o[0];
                            LINAA.ToDoDataRow p = this.Linaa.ToDoData.AddToDoDataRow(ref to, ref mon, ref refe);
                            ToDo.Reset(ref p, ref nu);
                        }

                        if (!CdCover)
                        {
                            IEnumerable<LINAA.ToDoResRow> resesAux = null;
                            resesAux = AddToDoResRow(todo.ToDoNr, !(k0 || Bare), true, minPosition, ref monS, ref refeS);

                            //add them
                            foreach (LINAA.ToDoResRow r in resesAux)
                            {
                                LINAA.ToDoResAvgRow res = this.Linaa.ToDoResAvg.AddToDoResAvgRow(r);
                                if (!k0) StDev(ref res);
                            }
                        }

                        if (k0) continue;

                        LINAA.ToDoAvgRow t = this.Linaa.ToDoAvg.AddToDoAvgRow(ref to);
                        Reset(ref t, ref nu);

                        //Reference overrides the automatic second (reference) Isotope
                        if (Bare) t.IR2 = Reference.IRAvgRow; //Reference is not null when k0 or f-alpha bare

                        LINAA.ToDoAvgUncRow tu = this.Linaa.ToDoAvgUnc.AddToDoAvgUncRow(to.ToDoNr);
                        ToDo.Reset(ref tu, ref nu);
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(todo, ex);
                        this.Linaa.AddException(ex);
                    }
                }

                hsToDoesFork0.Clear();
                hsToDoesFork0 = null;

                AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
                return "Prepare ERROR: " + ex.Message;
            }

            return string.Empty;
        }

        public void PropagateSR()
        {
            IEnumerable<LINAA.ToDoResAvgRow> ravgs = this.Linaa.ToDoResAvg.AsEnumerable();
            IEnumerable<LINAA.PeaksRow> enume = PropagateSR(ref ravgs);
            this.Linaa.SaveRows(ref enume);
        }

        public string Reset()
        {
            ToDoArgs nu = ToDoArgs.Empty;

            IEnumerable<object> avgs = this.Linaa.ToDoAvg.AsEnumerable().ToList();
            foreach (object s in avgs)
            {
                object o = s;
                ToDo.Reset(ref o, ref nu);
            }
            avgs = this.Linaa.ToDoData.AsEnumerable().ToList();
            foreach (object s in avgs)
            {
                object o = s;
                ToDo.Reset(ref o, ref nu);
            }
            avgs = this.Linaa.ToDoAvgUnc.AsEnumerable().ToList();
            foreach (object s in avgs)
            {
                object o = s;
                ToDo.Reset(ref o, ref nu);
            }

            this.Linaa.ToDoData.AcceptChanges();
            this.Linaa.ToDoAvg.AcceptChanges();
            this.Linaa.ToDoAvgUnc.AcceptChanges();

            return string.Empty;
        }

        //on devellopment
        public void SelectToDoesGroup()
        {
            int toCalc = (int)tocalculate;
            foreach (LINAA.ToDoRow t in ToDoes)
            {
                if (!t.IsgroupNrNull() && t.groupNr == toCalc) t.use = true;
                else t.use = false;
            }
        }

        public void SetParameters(string alpha0box, string f0box, bool opTimize, bool useRefe)
        {
            alpha0 = Convert.ToDouble(alpha0box);
            f0 = Convert.ToDouble(f0box);
            optimize = opTimize;
            useRef = false;
            if (tocalculate == LINAA.ToDoType.Q0determination || tocalculate == LINAA.ToDoType.k0determination)
            {
                useRef = useRefe;
            }
        }

        //on devellopment
        public void SetToDoesGroup()
        {
            int toCalc = (int)tocalculate;
            foreach (LINAA.ToDoRow t in ToDoes)
            {
                if (t.use) t.groupNr = toCalc;
                else if (t.groupNr == toCalc) t.SetgroupNrNull();
            }
        }

        public LINAA.ToDoType SetToDoType(String ToDoTypeText)
        {
            ToDoTypeText = ToDoTypeText.ToUpper();
            tocalculate = 0;

            if (ToDoTypeText.CompareTo("F-ALPHA BARE") == 0) tocalculate = LINAA.ToDoType.fAlphaBare;
            else if (ToDoTypeText.CompareTo("F-ALPHA CD-RATIO") == 0) tocalculate = LINAA.ToDoType.fAlphaCdRatio;
            else if (ToDoTypeText.CompareTo("F-ALPHA CD-COVERED") == 0) tocalculate = LINAA.ToDoType.fAlphaCdCovered;
            else if (ToDoTypeText.CompareTo("GENERAL RATIO") == 0) tocalculate = LINAA.ToDoType.GeneralRatio;
            else if (ToDoTypeText.CompareTo("U-RATIO") == 0) tocalculate = LINAA.ToDoType.URatio;
            else if (ToDoTypeText.CompareTo("CD-RATIO") == 0) tocalculate = LINAA.ToDoType.CdRatio;
            else if (ToDoTypeText.CompareTo("Q0-DETERMINATION") == 0) tocalculate = LINAA.ToDoType.Q0determination;
            else if (ToDoTypeText.CompareTo("K0-DETERMINATION") == 0) tocalculate = LINAA.ToDoType.k0determination;

            return tocalculate;
        }

        private void AcceptChanges()
        {
            this.Linaa.ToDoRes.AcceptChanges(); // SOOO IMPORTANT!
            this.Linaa.ToDoResAvg.AcceptChanges();
            this.Linaa.ToDoData.AcceptChanges();
            this.Linaa.ToDoAvg.AcceptChanges();
            this.Linaa.ToDoAvgUnc.AcceptChanges();
        }

        private void Handlers(bool activate)
        {
            if (activate)
            {
                sRDGV.CellMouseClick += this.SRDGV_CellMouseClick;
                sRDGV.CellMouseDoubleClick += (this.SRDGV_CellMouseDoubleClick);
                sRDGV.CellMouseEnter += (this.SRDGV_CellMouseEnter);
                sRDGV.KeyDown += (this.SRDGV_KeyDown);
            }
            else
            {
                sRDGV.CellMouseClick -= this.SRDGV_CellMouseClick;
                sRDGV.CellMouseDoubleClick -= (this.SRDGV_CellMouseDoubleClick);
                sRDGV.CellMouseEnter -= (this.SRDGV_CellMouseEnter);
                sRDGV.KeyDown -= (this.SRDGV_KeyDown);
            }
        }

        private void PerformFitfAlpha()
        {
            string filter = this.Linaa.ToDoAvg.useColumn.ColumnName;

            double[] numArray = new double[5];
            numArray = MyMath.CurveFit.Linear.LeastSquaresFit(this.Linaa.ToDoAvg.XColumn, this.Linaa.ToDoAvg.YColumn, this.Linaa.ToDoAvg.ResColumn, this.Linaa.ToDoAvg.YCalcColumn, this.Linaa.ToDoAvg.useColumn);
            this.fit.f = decimal.Round(Convert.ToDecimal(Math.Exp(-1.0 * numArray[0])), 3);
            this.fit.Alpha = decimal.Round(Convert.ToDecimal(-1.0 * numArray[1]), 5);
            this.fit.R2 = decimal.Round(Convert.ToDecimal(numArray[2]), 4);
            this.fit.SEf = decimal.Round(Convert.ToDecimal(numArray[3]), 3);
            this.fit.SEAlpha = decimal.Round(Convert.ToDecimal(numArray[4]), 4);

            fit.YCalc = MyMath.ListFrom(this.Linaa.ToDoAvg.YCalcColumn, 1, filter, true);
            fit.YErrorHigh = MyMath.ListFrom(this.Linaa.ToDoAvg.YErrorHighColumn, 1, filter, true);
            fit.YErrorLow = MyMath.ListFrom(this.Linaa.ToDoAvg.YErrorLowColumn, 1, filter, true);
            fit.Y = MyMath.ListFrom(this.Linaa.ToDoAvg.YColumn, 1, filter, false);
            fit.X = MyMath.ListFrom(this.Linaa.ToDoAvg.XColumn, 1, filter, false);
            this.fit.AlphaUncsSqr = MyMath.ListFrom(this.Linaa.ToDoAvg.alphaUncColumn, 2, filter, true);
            int count = this.fit.AlphaUncsSqr.Count;
            if (count != 0)
            {
                double sum = this.fit.AlphaUncsSqr.Sum();
                double alfaunc = Math.Sqrt(sum) / Math.Sqrt(count);
                this.fit.AlphaSD = Decimal.Round(Convert.ToDecimal(alfaunc), 4);
            }

            if (tocalculate == LINAA.ToDoType.fAlphaBare || tocalculate == LINAA.ToDoType.fAlphaCdRatio)
            {
                fit.Quantity = MyMath.ListFrom(this.Linaa.ToDoAvg.fColumn, 1, filter, true);
            }
            else fit.Quantity = MyMath.ListFrom(this.Linaa.ToDoAvg.FcColumn, 1, filter, true);
        }

        private void PerformFitOthers()
        {
            string filter = this.Linaa.ToDoAvg.useColumn.ColumnName;

            fit.YLog = MyMath.ListFrom(this.Linaa.ToDoAvg.YColumn, 1, filter, true);
            fit.XLog = MyMath.ListFrom(this.Linaa.ToDoAvg.XColumn, 1, filter, true);
            fit.Alphas = MyMath.ListFrom(this.Linaa.ToDoAvg.alphaColumn, 1, filter, true);
            fit.Qo = MyMath.ListFrom(this.Linaa.ToDoAvg.Qo1Column, 1, filter, true);
            fit.Isotopes = Rsx.Dumb.Hash.HashFrom<string>(this.Linaa.ToDoAvg, this.Linaa.ToDoAvg.IsoColumn.ColumnName, filter, true);
        }

        private void Push(ref IEnumerable<LINAA.ToDoAvgRow> rows)
        {
            clone.Merge(this.Linaa.ToDoAvg, false, MissingSchemaAction.AddWithKey);
            foreach (DataColumn col in arrOfColToPush)
            {
                string colPushed = col + "." + alpha0.ToString();
                if (!clone.Columns.Contains(colPushed)) clone.Columns.Add(colPushed, typeof(double));
            }
            Push(ref clone, ref rows, alpha0.ToString(), ref arrOfColToPush);
            clone.TableName = "Clone";
            if (!this.Linaa.Tables.Contains(clone.TableName)) this.Linaa.Tables.Add(clone);
            rows = null;
        }

        private void SRDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!xTable.GoodDGV(dgv)) return;
            if (!xTable.GoodColRowCell(dgv, e.ColumnIndex, e.RowIndex)) return;
            locked = true;
        }

        private void SRDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            locked = false;
        }

        private void SRDGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!xTable.GoodDGV(sRDGV) || locked) return;
            if (!xTable.GoodColRowCell(sRDGV, e.ColumnIndex, e.RowIndex)) return;

            sRDGV.CurrentCell = sRDGV[e.ColumnIndex, e.RowIndex];

            LINAA.ToDoResAvgRow tag = (LINAA.ToDoResAvgRow)sRDGV.CurrentCell.Tag;

            IEnumerable<DataRowView> view = sRBS.List.Cast<DataRowView>();

            DataRowView vi = view.FirstOrDefault(v => v.Row.Equals(tag));

            sRBS.Position = sRBS.List.IndexOf(vi);
        }

        private void SRDGV_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == Keys.P || key == Keys.E || key == Keys.Space || key == Keys.Delete)
            {
                if (!xTable.GoodDGV(sRDGV)) return;
            }
            else return;

            IEnumerable<DataGridViewCell> cells = sRDGV.SelectedCells.OfType<DataGridViewCell>();
            cells = cells.Where(c => c.ColumnIndex >= 6);
            cells = cells.Where(c => xTable.GoodColRowCell(sRDGV, c.ColumnIndex, c.RowIndex));

            int count = cells.Count();

            if (count == 0) return;

            foreach (DataGridViewCell c in cells)
            {
                LINAA.ToDoResAvgRow t = (LINAA.ToDoResAvgRow)c.Tag;
                if (!t.use)
                {
                    t.use = true;
                    c.Style.ForeColor = System.Drawing.Color.Black;
                    c.Style.SelectionForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    t.use = false;
                    c.Style.ForeColor = System.Drawing.Color.Red;
                    c.Style.SelectionForeColor = System.Drawing.Color.Red;
                }
            }
        }

        public ToDo(ref Interface set)
        {
            Linaa = set.Get();
            Interface = set;
            clone = new LINAA.ToDoAvgDataTable(false);
            fit = new FitParameters();
            LINAA.ToDoAvgDataTable dt = this.Linaa.ToDoAvg;
            arrOfColToPush = new DataColumn[] { dt.fColumn, dt.AiColumn, dt.Qo1Column, dt.dQoColumn, dt.SDColumn, dt.ObsSDColumn };
        }
    }
}