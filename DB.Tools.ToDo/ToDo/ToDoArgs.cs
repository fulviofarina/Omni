using System.Collections.Generic;
using static DB.Tools.ToDo;

namespace DB.Tools
{
    public partial class ToDoArgs
    {
        public ToDoCalculator<LINAA.ToDoDataRow> A;

        public ToDoCalculator<LINAA.ToDoDataRow> A2;

        public ToDoCalculator<LINAA.ToDoAvgRow> B;

        private bool bare;

        private bool cdCov;

        private bool cdRatio;

        private bool K0;

        private short k0bareminPos;

        private bool optimize;

        private bool qo;

        private bool qoOrCd;

        private IList<LINAA.ToDoAvgRow> references;

        private bool sameDecay = true;

        public static ToDoArgs Empty
        {
            get { return null; }
        }

        public bool Bare
        {
            get { return bare; }
            set { bare = value; }
        }

        public bool CdCov
        {
            get { return cdCov; }
            set { cdCov = value; }
        }

        public bool CdRatio
        {
            get { return cdRatio; }
            set { cdRatio = value; }
        }

        public bool k0
        {
            get { return K0; }
            set { K0 = value; }
        }

        public short k0BareMinPos
        {
            get { return k0bareminPos; }
            set { k0bareminPos = value; }
        }

        public bool Optimize
        {
            get { return optimize; }
            set { optimize = value; }
        }

        public bool Qo
        {
            get { return qo; }
            set { qo = value; }
        }

        public bool QoOrCd
        {
            get { return qoOrCd; }
            set { qoOrCd = value; }
        }

        public IList<LINAA.ToDoAvgRow> References
        {
            get { return references; }
            set { references = value; }
        }

        public bool SameDecay
        {
            get { return sameDecay; }
            set { sameDecay = value; }
        }

        public void SetDelegates()
        {
            ToDoCalculator<LINAA.ToDoDataRow> a = Reset;  //minimum RESET!!!
            ToDoCalculator<LINAA.ToDoDataRow> a2 = null;

            // if (!x.QoOrCd) optimize = false;

            if (!this.Optimize || this.k0)
            //withut Res and Res Avg for other isotopes
            //and for assigning just basic k0 data in k0 determination
            //(cuz later the k0 is calculated differently than with Asp)
            {
                //prepare then for non optimize! (taking asps from peaks)
                a += SetAsp;
                a += SetUncs;
            }

            if (Bare) a += DofAlphaBare;
            else if (k0)
            {
                if (!Optimize) a += Dok0DeterminationOneNormal;
                else a += DoQoOrk0OrCdRatioOptimized; //yes yes indeed, this function is quite generic...
                a2 = Dok0DeterminationTwo; //in others, this is null
            }
            else if (CdCov) a += DofAlphaCdCovered;
            else if (QoOrCd)
            {
                if (Optimize) a += DoQoOrk0OrCdRatioOptimized;
                else a += DoQ0OrCdRatioNormal;
            }

            a += CheckRCd;
            a += CheckSD;

            //set delegates!!!
            A = a;
            A2 = a2;

            ToDoCalculator<LINAA.ToDoAvgRow> b = null;

            if (!k0)
            {
                b += AvgRCd;
                b += CheckRCd;
                b += CheckSD;
            }

            if (Bare) b += DofAlphaBare;
            else if (k0) b += Dok0Determination;
            else if (CdCov) b += DofAlphaCdCovered;
            else if (QoOrCd)
            {
                if (CdRatio) b += DofAlphaCdRatio;
                else b += DoQoDetermination;
            }

            if (!k0) b += CalculateXY;  //in k0 determination this is not needed..

            B = b;    //set delegates!!!
        }

        public ToDoArgs(LINAA.ToDoType TodoType, short minPosition, bool Optimum)
        {
            k0bareminPos = minPosition;
            optimize = Optimum;
            cdRatio = TodoType == LINAA.ToDoType.fAlphaCdRatio;
            qo = TodoType == LINAA.ToDoType.Q0determination;
            qoOrCd = CdRatio || Qo;
            bare = TodoType == LINAA.ToDoType.fAlphaBare;
            cdCov = TodoType == LINAA.ToDoType.fAlphaCdCovered;
            K0 = TodoType == LINAA.ToDoType.k0determination;
            sameDecay = true;
        }
    }
}