using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Rsx.Generic
{
    /// <summary>
    /// This is a System.ComponentModel.BackgroundWorker For be used by another class?
    /// </summary>
    public class Loader : BackgroundWorker
    {
        public Loader()
        {
        }

        /// <summary>
        /// The function that should be invoked to report progress
        /// </summary>
        /// <param name="percent">the progress percent</param>
        // public delegate void Reporter(int percent);

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // if (report == null) return;

            if (e.UserState != null)
            {
                SystemException ex = e.UserState as SystemException;
                exceptionReport?.Invoke(ex);
            }
            int percentage = e.ProgressPercentage;
            report?.Invoke(percentage);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int length = mainMethods.Count;
            double step = 100;
            if (length != 1) step = (100.0 / (length - 1));

            for (int i = 0; i < mainMethods.Count; i++)
            {
                if (e.Cancel) continue;
                int perc = Convert.ToInt32(Math.Ceiling((step * i)));
                SystemException x = null;
                try
                {
                    Action async = mainMethods[i];
                    // if (async == null) continue;
                    async?.Invoke();
                }
                catch (SystemException ex)
                {
                    x = ex;
                }
                ReportProgress(perc, x);
            }
        }

        public void Set(IList<Action> LoadMethods, Action CallBackMethod, Action<int> ReportMethod, Action<Exception> ExceptionMethod=null)
        {
            mainMethods = LoadMethods;
            callback = CallBackMethod;
            report = ReportMethod;

            exceptionReport = defaultExceptionReport;
            if (ExceptionMethod!=null)
            {
                exceptionReport = ExceptionMethod;
            }

            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            this.DoWork += worker_DoWork;
            this.ProgressChanged += worker_ProgressChanged;
            this.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            callback?.Invoke();

            this.Dispose();
        }

        private void defaultExceptionReport(Exception ex)
        {
            throw ex;
           // MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.TargetSite, "Problems loading a data table content");
        }

        private IList<Action> mainMethods;
        private Action callback;
        private Action<int> report;
        private Action<Exception> exceptionReport;
    }
}