using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb; using Rsx;
using Rsx.Math;

namespace DB.Tools
{
    public partial class ToDo
    {
        public static IEnumerable<LINAA.PeaksRow> PropagateSR(ref IEnumerable<LINAA.ToDoResAvgRow> ravgs)
        {
            HashSet<LINAA.PeaksRow> tosave = new HashSet<LINAA.PeaksRow>();
            foreach (LINAA.ToDoResAvgRow ravg in ravgs)
            {
                IEnumerable<LINAA.ToDoResRow> reses = ravg.GetToDoResRows();
                bool use = ravg.use;
                foreach (LINAA.ToDoResRow res in reses)
                {
                    try
                    {
                        LINAA.PeaksRow p1 = res.PeaksRowParentByPeaks_TDRes;
                        LINAA.PeaksRow p2 = res.PeaksRowParentByPeaks_TDRes2;
                        if (use)
                        {
                            p1.ID = Math.Abs(p1.ID);
                            p2.ID = Math.Abs(p2.ID);
                        }
                        else
                        {
                            p1.ID = -1 * Math.Abs(p1.ID);
                            p2.ID = -1 * Math.Abs(p2.ID);
                        }

                        tosave.Add(p1);
                        tosave.Add(p2);
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(res, ex);
                    }
                }
            }

            return tosave.ToList();
        }

        public static string CalculateFCs(LINAA.ToDoType todoType, ref IEnumerable<LINAA.ToDoDataRow> rows)
        {
            if (rows.Count() == 0) return "CalculateFCs Error (ToDoData): Collection is empty";

            rows = rows.ToList();
            foreach (LINAA.ToDoDataRow p in rows)
            {
                try
                {
                    p.Fc = p.IPAvg.Fc;
                    if (todoType != LINAA.ToDoType.fAlphaCdCovered) // Alpha Cd-Covered
                    {
                        try
                        {
                            p.Fc2 = p.IPAvg2.Fc;
                        }
                        catch (SystemException ex)
                        {
                            p.use = false;
                            p.SetColumnError("Fc2", ex.Message);
                        }
                    }
                }
                catch (SystemException ex)
                {
                    p.use = false;
                    p.SetColumnError("Fc", ex.Message);
                }
            }

            return "CalculateFC (ToDoData) was OK";
        }

        public static string CalculateFCs(LINAA.ToDoType todoType, ref IEnumerable<LINAA.ToDoAvgRow> rows)
        {
            if (rows.Count() == 0) return "CalculateFCs Error (ToDoAvg): Collection is empty";
            rows = rows.ToList();
            foreach (LINAA.ToDoAvgRow t in rows)
            {
                try
                {
                    IEnumerable<LINAA.ToDoDataRow> Allchilds = t.GetToDoDataRows();
                    IEnumerable<LINAA.ToDoDataRow> childs = Allchilds.Where(o => !o.HasErrors);
                    if (childs.Count() == 0) t.use = false;
                    else
                    {
                        t.use = childs.First().use;
                        //   if (t.use)
                        //  {
                        //i'm using the column names because they are common to bot parent and child

                        double[] WAvgs = MyMath.WAverageStDeV(childs, "Fc", "unc", "use", t.use);
                        t.Fc = WAvgs[0];
                        WAvgs = MyMath.WAverageStDeV(childs, "Fc2", "unc2", "use", t.use);
                        t.Fc2 = WAvgs[0];

                        // }
                    }
                }
                catch (SystemException ex)
                {
                    t.use = false;
                    EC.SetRowError(t, ex);
                }
            }

            return "CalculateFCs (ToDoAvg) was OK";
        }

        public static double[] GetAvgsfAlpha(LINAA.ToDoType todoType, LINAA.ToDoAvgDataTable table)
        {
            double avgLogEr = 0;
            double avgqo_qo2a_1 = 0;
            double avgQoa_1 = 0;

            if (todoType != LINAA.ToDoType.Q0determination && todoType != LINAA.ToDoType.k0determination)
            {
                //SECOND PART, DETERMINING ERRORS
                string filter = table.useColumn.ColumnName;
                avgLogEr = 0;
                IList<double> IavgLogEr = MyMath.ListFrom(table.XColumn, 1, filter, true);
                avgLogEr = IavgLogEr.Average();
                avgqo_qo2a_1 = 0;
                if (todoType.Equals(LINAA.ToDoType.fAlphaBare))
                {
                    avgqo_qo2a_1 = MyMath.ListFrom(table.__qoa_qo2a__1Column, 1, filter, true).Average();
                }
                avgQoa_1 = 0;
                avgQoa_1 = MyMath.ListFrom(table.qoalphaColumn, -1, filter, true).Average();
            }

            return new double[] { avgLogEr, avgqo_qo2a_1, avgQoa_1 };
        }

        public static void GetUncfAlpha(LINAA.ToDoType tocalculate, double[] args, LINAA.ToDoAvgUncDataTable table)
        {
            double avgLogEr = args[0];
            double avgqo_qo2a_1 = args[1];
            double avgQoa_1 = args[2];

            string filter = table.useColumn.ColumnName;

            IEnumerable<LINAA.ToDoAvgUncRow> rows = table.AsEnumerable();
            if (tocalculate == LINAA.ToDoType.fAlphaBare)
            {
                foreach (LINAA.ToDoAvgUncRow t in rows)
                {
                    try
                    {
                        // if (t.use)
                        {
                            LINAA.ToDoAvgRow a = t.ToDoAvgRow;
                            t.Bi = ((a.qoalpha * Math.Log(a.Er)) - (a.qo2alpha * Math.Log(a.Er2)));
                            t.Bi = t.Bi * a.__qoa_qo2a__1;
                        }
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(t, ex);
                    }
                }
                double avgBi = 0;
                avgBi = MyMath.ListFrom(table.BiColumn, 1, table.useColumn.ColumnName, true).Average();
                int used = 0;

                foreach (LINAA.ToDoAvgUncRow r in rows)
                {
                    try
                    {
                        // if (r.use)
                        {
                            LINAA.ToDoAvgRow a = r.ToDoAvgRow;
                            r.Ji2 = ((a.X - avgLogEr) * (a.__qoa_qo2a__1 - avgqo_qo2a_1));
                            r.Ji = ((a.X - avgLogEr) * (r.Bi - avgBi));
                            used++;
                        }
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(r, ex);
                    }
                }

                double avgJi = 0;
                double avgJi2 = 0;

                string use = table.useColumn.ColumnName;

                avgJi = MyMath.ListFrom(table.JiColumn, 1, use, true).Average();
                avgJi2 = MyMath.ListFrom(table.Ji2Column, 1, use, true).Average();

                GetUncfAlphaBare(avgJi, avgJi2, used, avgLogEr, ref rows);
                return;
            }
            if (tocalculate == LINAA.ToDoType.fAlphaCdCovered || tocalculate == LINAA.ToDoType.fAlphaCdRatio) //cd-covered
            {
                foreach (LINAA.ToDoAvgUncRow t in rows)
                {
                    try
                    {
                        // if (t.use)
                        {
                            LINAA.ToDoAvgRow a = t.ToDoAvgRow;
                            t.Vi = ((a.qoalpha / a.Qo1) * Math.Log(a.Er));
                            t.Vi = t.Vi + (((0.26 * a.Calpha) / a.Qo1) * ((1.67 / (a.alpha + 0.5)) - 1));
                        }
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(t, ex);
                    }
                }

                double avgVi = 0;

                avgVi = MyMath.ListFrom(table.ViColumn, 1, filter, true).Average();
                int used = 0;
                foreach (LINAA.ToDoAvgUncRow r in rows)
                {
                    try
                    {
                        // if (r.use)
                        {
                            LINAA.ToDoAvgRow a = r.ToDoAvgRow;

                            r.Ui = ((a.X - avgLogEr) * (r.Vi - avgVi));
                            r.Wi = ((a.X - avgLogEr) * (Math.Pow(a.qoalpha, -1) - avgQoa_1));
                            used++;
                        }
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(r, ex);
                    }
                }
                double avgWi = 0;
                double avgUi = 0;
                avgUi = MyMath.ListFrom(table.UiColumn, 1, filter, true).Average();
                avgWi = MyMath.ListFrom(table.WiColumn, 1, filter, true).Average();

                if (tocalculate == LINAA.ToDoType.fAlphaCdCovered)
                {
                    GetUncfAlphaCdCovered(avgLogEr, used, avgWi, avgUi, ref rows);
                }
                else
                {
                    GetUncfAlphaCdRatio(avgLogEr, used, avgWi, avgUi, ref rows);
                }
            }

            return;
        }

        protected internal static void GetUncfAlphaBare(double avgJi, double avgJi2, int used, double avgLogEr, ref IEnumerable<LINAA.ToDoAvgUncRow> rows)
        {
            foreach (LINAA.ToDoAvgUncRow r in rows)
            {
                try
                {
                    // if (r.use)
                    {
                        LINAA.ToDoAvgRow a = r.ToDoAvgRow;
                        r.ZaAsp = 0.434 * (a.f + a.Qo1);
                        r.ZaAsp = r.ZaAsp * Math.Abs((a.X - avgLogEr) * a.__qoa_qo2a__1 * Math.Pow((avgJi * used * a.alpha), -1));

                        r.ZaAsp2 = 0.434 * (a.f + a.Qo2);
                        r.ZaAsp2 = r.ZaAsp2 * Math.Abs(avgJi2 * Math.Pow(a.alpha * avgJi, -1));

                        r.ZaEr = a.qoalpha * Math.Abs(a.alpha); // first ZEr calculation, including uncertainty in Er
                        r.ZaEr = (r.ZaEr * r.ZaAsp) * Math.Pow((a.f + a.Qo1), -1);

                        r.ZaEr2 = a.qo2alpha * Math.Abs(a.alpha); //frist ZEr2 calculation, including uncertainty in Er2
                        r.ZaEr2 = (r.ZaEr2 * r.ZaAsp2) * Math.Pow((a.f + a.Qo2), -1);

                        r.ZaECd = 0;

                        r.ZaQo = (a._QoEr_a * r.ZaAsp) * Math.Pow((a.f + a.Qo1), -1);
                        r.ZaQo2 = (a._Qo2Er2_a * r.ZaAsp2) * Math.Pow((a.f + a.Qo2), -1);

                        r.ZfQo = Math.Abs(a._QoEr_a * (1 + (a.Qo2 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));
                        r.ZfQo2 = Math.Abs(a._Qo2Er2_a * (1 + (a.Qo1 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));

                        r.Zfa = (((a.f + a.Qo1) * a.qo2alpha * Math.Log(a.Er2)) - ((a.f + a.Qo2) * a.qoalpha * Math.Log(a.Er)));
                        r.Zfa = (r.Zfa * Math.Pow(a.Qo1 - a.Qo2, -1)) + (0.60 * a.Calpha * ((1.67 / (a.alpha + 0.5)) - 1));
                        r.Zfa = Math.Abs(r.Zfa * (a.alpha / a.f));

                        r.ZfEr = Math.Abs(a.alpha * a.qoalpha * (1 + (a.Qo2 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));
                        r.ZfEr2 = Math.Abs(a.alpha * a.qo2alpha * (1 + (a.Qo1 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));
                        r.ZfAsp = Math.Abs((a.f + a.Qo1) * (a.f + a.Qo2) * Math.Pow(a.f * (a.Qo1 - a.Qo2), -1));

                        a.alphaUnc = Math.Pow(r.ZaAsp * a.SD * 0.5, 2) + Math.Pow(r.ZaAsp2 * a.SD * 0.5, 2);

                        //fixed accuracy (nuclear data uncertainties)

                        a.alphaUnc += Math.Pow(r.ZaECd * 15, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr * a.ErUnc, 2) + Math.Pow(r.ZaQo * a.QoUnc, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr2 * a.Er2Unc, 2) + Math.Pow(r.ZaQo2 * a.Qo2Unc, 2);
                        a.alphaUnc = Math.Sqrt(a.alphaUnc);

                        a.fUnc = Math.Pow(r.Zfa * a.alphaUnc, 2) + Math.Pow(r.ZfAsp * a.SD, 2);
                        a.fUnc += Math.Pow(r.ZfEr * a.ErUnc, 2) + Math.Pow(r.ZfQo * a.QoUnc, 2);
                        a.fUnc += Math.Pow(r.ZfEr2 * a.Er2Unc, 2) + Math.Pow(r.ZfQo2 * a.Qo2Unc, 2);
                        a.fUnc = Math.Sqrt(a.fUnc);
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(r, ex);
                }
            }
        }

        protected internal static void GetUncfAlphaCdCovered(double avgLogEr, int used, double avgWi, double avgUi, ref IEnumerable<LINAA.ToDoAvgUncRow> rows)
        {
            foreach (LINAA.ToDoAvgUncRow r in rows)
            {
                try
                {
                    // if (r.use)
                    {
                        LINAA.ToDoAvgRow a = r.ToDoAvgRow;
                        r.ZaAsp = 0.434 * Math.Abs((a.X - avgLogEr) * Math.Pow((a.alpha * avgUi * used), -1));
                        r.ZaAsp2 = 0;

                        r.ZaEr = a.qoalpha * Math.Abs(a.alpha); // first ZEr calculation, including uncertainty in Er
                        r.ZaEr = r.ZaEr * r.ZaAsp * Math.Pow(a.qoalpha, -1);
                        r.ZaEr2 = 0;

                        r.ZaECd = 0.434 * a.Calpha * (a.alpha + 0.5); // first ZaECd calculation
                        r.ZaECd = r.ZaECd * Math.Abs(Math.Pow(a.alpha, -1) * (avgWi / avgUi)); //second ZaECd calculation

                        r.ZaQo = a._QoEr_a; //first ZaQo first calculation, including uncertainty in Qo
                        r.ZaQo = r.ZaQo * r.ZaAsp * Math.Pow(a.qoalpha, -1);
                        r.ZaQo2 = 0;

                        r.ZfEr2 = 0;
                        r.ZfQo2 = 0;
                        r.ZfAsp = 0;
                        r.ZfEr = 0;
                        r.ZfQo = 0;

                        a.alphaUnc = Math.Pow(r.ZaAsp * a.SD, 2);

                        //fixed accuracy (nuclear data uncertainties)

                        a.alphaUnc += Math.Pow(r.ZaECd * 15, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr * a.ErUnc, 2) + Math.Pow(r.ZaQo * a.QoUnc, 2);

                        a.alphaUnc = Math.Sqrt(a.alphaUnc);
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(r, ex);
                }
            }
        }

        protected internal static void GetUncfAlphaCdRatio(double avgLogEr, int used, double avgWi, double avgUi, ref IEnumerable<LINAA.ToDoAvgUncRow> rows)
        {
            foreach (LINAA.ToDoAvgUncRow r in rows)
            {
                try
                {
                    //if (r.use)
                    {
                        LINAA.ToDoAvgRow a = r.ToDoAvgRow;

                        r.ZaAsp = 0.434 * Math.Pow(a.f, -1) * (a.f + a.Qo1);
                        r.ZaAsp = r.ZaAsp * Math.Abs((a.X - avgLogEr) * Math.Pow((a.alpha * avgUi * used), -1));
                        r.ZaAsp2 = 0;

                        r.ZaEr = a.qoalpha * Math.Abs(a.alpha); // first ZEr calculation, including uncertainty in Er
                        r.ZaEr = (r.ZaEr * r.ZaAsp * Math.Pow(a.qoalpha, -1) * a.f) * Math.Pow((a.f + a.Qo1), -1);
                        r.ZaEr2 = 0;

                        r.ZaECd = 0.434 * a.Calpha * (a.alpha + 0.5); // first ZaECd calculation
                        r.ZaECd = r.ZaECd * Math.Abs(Math.Pow(a.alpha, -1) * (avgWi / avgUi)); //second ZaECd calculation

                        r.ZaQo = a._QoEr_a; //first ZaQo first calculation, including uncertainty in Qo
                        r.ZaQo = (r.ZaQo * r.ZaAsp * Math.Pow(a.qoalpha, -1) * a.f) * Math.Pow((a.f + a.Qo1), -1);
                        r.ZaQo2 = 0;

                        r.Zfa = Math.Abs((a.alpha / a.Qo1) * ((a.qoalpha * Math.Log(a.Er)) + (0.60 * a.Calpha * ((1.67 / (a.alpha + 0.5)) - 1))));
                        r.ZfEr = Math.Abs(a.alpha * (a.qoalpha / a.Qo1));
                        r.ZfQo = Math.Abs(a._QoEr_a / a.Qo1);
                        r.ZfAsp = 1 + (a.Qo1 / a.f);

                        r.ZfEr2 = 0;
                        r.ZfQo2 = 0;

                        a.alphaUnc = Math.Pow(r.ZaAsp * 0.5 * a.SD, 2) + Math.Pow(r.ZaAsp2 * 0.5 * a.SD, 2);
                        //fixed accuracy (nuclear data uncertainties)

                        a.alphaUnc += Math.Pow(r.ZaECd * 15, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr * a.ErUnc, 2) + Math.Pow(r.ZaQo * a.QoUnc, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr2 * a.Er2Unc, 2) + Math.Pow(r.ZaQo2 * a.Qo2Unc, 2);
                        a.alphaUnc = Math.Sqrt(a.alphaUnc);

                        a.fUnc = Math.Pow(r.Zfa * a.alphaUnc, 2) + Math.Pow(r.ZfAsp * a.SD, 2);
                        a.fUnc += Math.Pow(r.ZfEr * a.ErUnc, 2) + Math.Pow(r.ZfQo * a.QoUnc, 2);
                        a.fUnc += Math.Pow(r.ZfEr2 * a.Er2Unc, 2) + Math.Pow(r.ZfQo2 * a.Qo2Unc, 2);
                        a.fUnc = Math.Sqrt(a.fUnc);
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(r, ex);
                }
            }
        }

        public static double GetResiduals(ref IEnumerable<LINAA.ToDoAvgRow> rows)
        {
            DateTime start = DateTime.Now;
            foreach (LINAA.ToDoAvgRow r in rows)
            {
                try
                {
                    r.YErrorHigh = r.Y + (r.Y * (r.SD / 100));
                    r.YErrorLow = r.Y - (r.Y * (r.SD / 100));
                }
                catch (SystemException ex)
                {
                    r.use = false;
                    EC.SetRowError(r, ex);
                }
            }

            return (DateTime.Now - start).TotalSeconds;
        }

        public static string Push(ref LINAA.ToDoAvgDataTable clone, ref IEnumerable<LINAA.ToDoAvgRow> rows, string newColBase, ref DataColumn[] arrOfColToPush)
        {
            rows = rows.ToList();
            if (rows.Count() == 0) return "SetValues Error: Collection is empty";

            try
            {
                //for overriding f and alpha with reference when Qo determination

                foreach (LINAA.ToDoAvgRow avg in rows)
                {
                    foreach (DataColumn col in arrOfColToPush)
                    {
                        LINAA.ToDoAvgRow cl = clone.FirstOrDefault(c => c.ToDoNr == avg.ToDoNr);
                        if (Rsx.EC.IsNuDelDetch(cl)) continue;
                        object valueToPush = avg.Field<object>(col.ColumnName);
                        cl.SetField<object>(col.ColumnName + "." + newColBase, valueToPush);
                    }
                }
            }
            catch (SystemException ex)
            {
                string msg = ex.Message;
            }
            return "Push was OK";
        }

        protected internal static void OverridefAlpha(ref LINAA.ToDoAvgRow t, double f0, double alpha)
        {
            t.f = f0;
            t.alpha = alpha;
        }
    }
}