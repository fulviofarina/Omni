using Rsx.Dumb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static DB.LINAA;
using static DB.Tools.XCOM.XCOMTXT;
using static DB.Tools.XCOM.Engine;

namespace DB.Tools
{
    public partial class XCOMBase
    {

     
        protected internal Interface Interface;
        protected internal bool calculating = false;
        protected internal Action<Exception> exceptionAdder;
        protected internal Hashtable loaders = new Hashtable();
        protected internal bool offline = false;
        protected internal XCOMPrefRow pref;
        protected internal Action<string, string, object, bool> reporter;
        protected internal IList<MatrixRow> rows;
        protected internal decimal seconds = 0;
        protected internal Stopwatch stopwatch;


     

    }

    public partial class XCOMBase : CalculateBase
    {
        public XCOMBase() : base()
        {
            stopwatch = new Stopwatch();
        }

        public Action<Exception> ExceptionAdder
        {
            get
            {
                return exceptionAdder;
            }

            set
            {
                exceptionAdder = value;
            }
        }



        public bool Offline
        {
            get
            {
                return offline;
            }

            set
            {
                offline = value;
            }
        }

        public XCOMPrefRow Preferences
        {
            set
            {
                pref = value;
            }
        }

        public Action<string, string, object, bool> Reporter
        {
            set
            {
                reporter = value;
            }
        }

        public IList<MatrixRow> Rows
        {
            get
            {
                return rows;
            }

            set
            {
                rows = value;
            }
        }


        public void Set(ref Interface inter)
        {
            Interface = inter;
        }


        public new bool IsCalculating
        {
            get
            {
                return calculating;
            }
            set
            {
                calculating = value;

                if (!calculating)
                {
                    CheckCompletedOrCancelled();
                }
                else
                {
                    loaders.Clear();
                    stopwatch.Start();
                    seconds = 0;
                }
            }
        }

        public void CheckCompletedOrCancelled()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                seconds = Decimal.Round(Convert.ToDecimal(stopwatch.Elapsed.TotalSeconds), 0);
            }
            string log = string.Empty;
            bool ok = true;
            string title = "Completed!";
            if (loaders.Count == 0)
            {
                log = "Everything completed in ";
            }
            else
            {
                cancelLoaders();
                ok = false;
                loaders.Clear();
                title = "Cancelled!";
                log = "Computations cancelled after ";
            }
            log += seconds + " seconds";

            reporter(log, title, ok, false);
        }

        private void cancelLoaders()
        {
            foreach (int i in loaders.Keys)
            {
                ILoader item = (ILoader)loaders[i];
                item.CancelLoader();

                MatrixRow toCancel = rows.Where(o => o.IsBusy).FirstOrDefault(o => o.MatrixID == i);
                toCancel.SetAsNotCalculated();
            }
        }

    }
  
    
}