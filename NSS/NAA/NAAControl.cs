using System;
using System.Data;
using System.Linq;
using GADB;
using GeneticSharp.Domain.Chromosomes;

namespace DB
{
    public partial class NAAControl : ControllerBase
    {
        private int numberOfEqs = 5;

        public override void FillStrings<T>(ref GADataSet.SolutionsRow r, ref T stringRow)
        {
        }

        public delegate double y(double[] a, double x);

        /// <summary>
        /// BASIC CALCULATION NECESSARY FOR FITNESS
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public override void FillBasic(ref GADataSet.SolutionsRow r)
        {
            y[] func = new y[5];

            func[0] = y1;
            func[1] = y2;
            func[2] = y3;
            func[3] = y4;
            func[4] = y5;

            double Fine = 0;

            double[] ai = r.GenesAsDoubles.ToArray();

            double[] yi = new double[ai.Length];
            double[] di = new double[ai.Length];

            for (int i = 0; i < di.Length; i++) di[i] = yi[i] = 0;
            for (int i = 1; i < ai.Length; i++) ai[i] *= 5;

            foreach (DataRow d in this.ProblemData)
            {
                double x = d.Field<double>("A");
                double yexp = d.Field<double>("B");

                int index = Convert.ToInt32(ai[0]);
                yi[0] = func[index](ai, x);
                di[0] += Math.Pow(yexp - yi[0], 2);
            }

            Fine = Math.Sqrt(di[0]);
            r.Okays = ai[0] + " " + Decimal.Round(Convert.ToDecimal(Fine), 3);
            r.Fitness = 1 / (1 + Fine); //max vol, max value * (1+fine)

            r.Genotype = Aid.SetStrings(r.GenesAsDoubles, 4);
        }

        private double y1(double[] a, double x)
        {
            return a[1] * Math.Sin(a[2] * x + a[3]) + a[4];
        }

        private double y2(double[] a, double x)
        {
            return a[1] * Math.Cos(a[2] * x + a[3]) + a[4];
        }

        private double y3(double[] a, double x)
        {
            return a[1] * Math.Exp(a[2] * x + a[3]) + a[4];
        }

        private double y4(double[] a, double x)
        {
            return a[1] + (a[2] * x);
        }

        private double y5(double[] a, double x)
        {
            return a[1] + (a[2] * x) + (a[3] * Math.Pow(x, 2)) + (a[4] * Math.Pow(x, 3));
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