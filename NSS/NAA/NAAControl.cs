using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GADB;
using GeneticSharp.Domain.Chromosomes;
using Rsx.Math;

namespace DB
{
    public partial class NAAControl : ControllerBase
    {
        private int numberOfEqs = 5;

        public override void FillStrings<T>(ref GADataSet.SolutionsRow r, ref T stringRow)
        {
        }

        //  public delegate double y(double[] a, double x);

        /// <summary>
        /// BASIC CALCULATION NECESSARY FOR FITNESS
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public override void FillBasic(ref GADataSet.SolutionsRow r)
        {
            double Fine = 0;
            r.Fitness = 1;

            double[] ai = r.GenesAsDoubles.ToArray();//.Select(o => Math.Abs(o)).ToArray();

            double FC = 1;
            //      int indexOfRadioOfInterest = 0;
            double ActivityOfInterest = 0;

            //lista para otras actividades
            List<double> otherActivities = new List<double>();
            int SampleID = this.ProblemData.FirstOrDefault().Field<int>("ProblemID");
            LINAA.SubSamplesRow s = interf.IDB.SubSamples.FindBySubSamplesID(SampleID);

            if (s == null)
            {
                r.Okays = "Sample is null?";
                r.Fitness = 0;
            }
            else
            {
                if (s.IrradiationCode.Contains("X") || s.IrradiationCode.Contains("Z")) FC = 2900;
                else if (s.IrradiationCode.Contains("Y")) FC = 200;

                IEnumerable<LINAA.IRequestsAveragesRow> irrs = s.GetIRequestsAveragesRows();
                //  IEnumerable<LINAA.MeasurementsRow> measurements = s.GetMeasurementsRows();
                //  interf.IPopulate.ISamples.LoadSampleData(false);

                foreach (DataRow d in this.ProblemData)
                {
                    try
                    {
                        //search by NAAId which is in the K field
                        LINAA.IRequestsAveragesRow ir = irrs.FirstOrDefault(o => o.NAAID == d.Field<int>("K"));
                        if (ir == null)
                        {
                            r.Fitness = 0;
                            r.Okays = "Isotope is null?";
                            continue;
                        }
                        //look for the radioisotope of interest marke with L field equal to 1
                        int isRadioIsotopeOfInterest = d.Field<int>("L");

                        double timeFactor = d.Field<double>("A");
                        //     if (isRadioIsotopeOfInterest == 1) indexOfRadioOfInterest = ir.NAAID;
                        //   LINAA.MeasurementsRow m = measurements.FirstOrDefault();

                        //PONER ESTO
                        //DB.Tools.WC.Rate(s.Alpha, s.f, ref ir);
                        //  DB.Tools.WC.FindDecayTimes(ref measurements);

                        LINAA.SigmasRow sigma = ir.NAARow.ReactionsRowParent.SigmasRowParent;

                        if (sigma == null) continue;

                        if (ir.NAARow.ReactionsRowParent == null) continue;

                        LINAA.SigmasSalRow sigmaSal = ir.NAARow.ReactionsRowParent.SigmasSalRow;

                        LINAA.NAARow n = ir.NAARow;

                        //  timeFactor *= 60;
                        double countFactor = ai[2] * timeFactor;
                        double decayFactor = ai[1] * timeFactor;
                        double irradiationFactor = ai[0] * timeFactor;

                        if (countFactor < 0.05 * timeFactor) continue;
                        if (decayFactor < 0.05 * timeFactor) continue;
                        if (irradiationFactor < 0.05 * timeFactor) continue;

                        double satActivity = (s.Net * 1e-6) * (MyMath.NAvg * sigma.sigma0 * 1e-24 * sigma.theta * 0.01 / sigmaSal.Mat) * (FC * 1e6 / 0.2882) * ir.R0;
                        double totalTime = 1;
                        /*
                        //version 1
                        totalTime = MyMath.S((0.693 / n.T2), irradiationFactor);
                        double decay = MyMath.D((0.693 / n.T2), decayFactor);
                        double count = MyMath.C((0.693 / n.T2), countFactor);
                        totalTime *= decay * count;
                        */

                        //version 2
                        //     activity = satActivity / ir.T0;
                        IEnumerable<LINAA.PeaksRow> peaks = new List<LINAA.PeaksRow>();

                        DB.Tools.WC.Temporal(ref ir, ref peaks, irradiationFactor, decayFactor, countFactor);
                        totalTime = ir._T0;

                        double activity = satActivity * (totalTime);

                        if (isRadioIsotopeOfInterest == 1)
                        {
                            ActivityOfInterest = activity;
                        }
                        else
                        {
                            otherActivities.Add(activity);
                        }

                        //   ir.Asp = (s.Concentration * s.DryNet * 0.001 * 1e-6) * (MyMath.NAvg * sigma.sigma0 * 1e-24 * sigma.theta * 0.01 / ir.NAARow.ReactionsRowParent.SigmasSalRow.Mat) * (FC * 1e6 / 0.2882) * ir.R0 * MyMath.S((0.693 / n.T2), s.IrradiationTotalTime); //result in Bq
                        //   ir.Asp = ir.Asp * 0.001; //result in kBq

                        //   LINAA.irr
                    }
                    catch (SystemException e)
                    {
                        r.Okays = e.StackTrace;
                    }
                    /*
                    double x = d.Field<double>("A");
                    double yexp = d.Field<double>("B");

                    int index = Convert.ToInt32(ai[0]);
                    //  yi[0] = func[index](ai, x);
                    di[0] += Math.Pow(yexp - yi[0], 2);
                    */
                }

                try
                {
                    //   interf.IPopulate.ISamples.LoadSampleData(true);

                    double sum = 0;
                    sum = otherActivities.Where(o => !double.IsNaN(o)).Sum();
                    if (double.IsNaN(sum)) r.Fitness = 0;
                    if (sum <= 0) r.Fitness = 0;
                    if (ActivityOfInterest <= 0) r.Fitness = 0;
                    if (double.IsNaN(ActivityOfInterest)) r.Fitness = 0;
                    else
                    {
                        if (r.Fitness != 0) r.Fitness = ActivityOfInterest / sum;
                    }
                }
                catch (Exception)
                {
                    r.Fitness = 0;
                    r.Okays = "Second Kind of Problem";
                }
            }
            // DB.Tools.WC.Calculate(ref irrs, false, false);

            //   Fine = Math.Sqrt(di[0]);
            //   r.Okays = ai[0] + " " + Decimal.Round(Convert.ToDecimal(Fine), 3);
            //    r.Fitness = 1 / (1 + Fine); //max vol, max value * (1+fine)

            r.Genotype = Aid.SetStrings(r.GenesAsDoubles, 3);
        }

        /// <summary>
        /// POST CALCULATION TO DECODE
        /// </summary>
        /// <param name="r"></param>

        // //// / / / / / //////// //////////////////////////// AQUI TEMPLATE
        /// <summary>
        /// INITIALIZER
        /// </summary>
        /// <param name="dt"></param>
        public NAAControl() : base()
        {
        }

        private Interface interf;

        public Interface Interface
        {
            set
            {
                interf = value;
                //  int? id = interf.IPopulate.IIrradiations.FindIrrReqID("X1702");
                //  interf.IPopulate.ISamples.PopulateSubSamples((int)id);
            }
        }

        /// <summary>
        /// NATURAL FUNCTION, COMPULSORY
        /// </summary>
        /// <returns></returns>
        public override IChromosome CreateChromosome()
        {
            //no junk? last argument
            NAAChromo c = new NAAChromo(SIZE, numberOfEqs);
            return c;
        }
    }
}