using System.Collections.Generic;

namespace DB.Tools
{
    public partial class FitParameters : DB.Tools.IFitted
    {
        private decimal _alpha = 0;

        private decimal _f = 0;

        private IList<double> _Qo;

        private decimal _R2 = 0;

        private decimal _SEAlpha = 0;

        private decimal _SEf = 0;

        private IList<double> alphas;

        private decimal alphaSD = 0;

        private IList<double> alphaUncsSqr;

        private IList<string> isotopes;

        private IList<double> quantity;

        private IList<double> xLog;

        private IList<double> xs;

        private IList<double> ycalc;

        private IList<double> yerrHigh;

        private IList<double> yerrLow;

        private IList<double> yLog;

        private IList<double> ys;

        public decimal Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public IList<double> Alphas
        {
            get { return alphas; }
            set { alphas = value; }
        }

        public decimal AlphaSD
        {
            get { return alphaSD; }
            set { alphaSD = value; }
        }

        public IList<double> AlphaUncsSqr
        {
            get { return alphaUncsSqr; }
            set { alphaUncsSqr = value; }
        }

        public decimal f
        {
            get { return _f; }
            set { _f = value; }
        }

        public IList<string> Isotopes
        {
            get { return isotopes; }
            set { isotopes = value; }
        }

        public IList<double> Qo
        {
            get { return _Qo; }
            set { _Qo = value; }
        }

        public IList<double> Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public decimal R2
        {
            get { return _R2; }
            set { _R2 = value; }
        }

        public decimal SEAlpha
        {
            get { return _SEAlpha; }
            set { _SEAlpha = value; }
        }

        public decimal SEf
        {
            get { return _SEf; }
            set { _SEf = value; }
        }

        public IList<double> X
        {
            get { return xs; }
            set { xs = value; }
        }

        public IList<double> XLog
        {
            get { return xLog; }
            set { xLog = value; }
        }

        public IList<double> Y
        {
            get { return ys; }
            set { ys = value; }
        }

        public IList<double> YCalc
        {
            get { return ycalc; }
            set { ycalc = value; }
        }

        public IList<double> YErrorHigh
        {
            get { return yerrHigh; }
            set { yerrHigh = value; }
        }

        public IList<double> YErrorLow
        {
            get { return yerrLow; }
            set { yerrLow = value; }
        }

        public IList<double> YLog
        {
            get { return yLog; }
            set { yLog = value; }
        }

        public FitParameters()
        {
        }
    }
}